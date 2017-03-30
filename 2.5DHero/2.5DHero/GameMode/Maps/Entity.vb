Namespace GameModes.Maps

    ''' <summary>
    ''' Possible render modes for an entity.
    ''' </summary>
    Enum EntityRenderMode
        Primitives
        Model
    End Enum

    ''' <summary>
    ''' The directions an entity can face.
    ''' </summary>
    Enum EntityFaceDirection
        North = 0
        West = 1
        South = 2
        East = 3
    End Enum

    ''' <summary>
    ''' Represents part of a map.
    ''' </summary>
    Class Entity

        Private Shared ReadOnly DARK_COLOR As Vector3 = New Vector3(0.5F, 0.5F, 0.6F)
        Private Shared ReadOnly DEFAULT_COLOR As Vector3 = Vector3.One

        Private _dataModel As DataModel.Json.Game.EntityModel
        Private _properties As Dictionary(Of String, EntityProperties.EntityProperty)

        Private _model As Model = Nothing
        Private _modelBoundingSphere As BoundingSphere? = Nothing

        Private _textures As List(Of Texture2D)
        Private _worldCreator As EntityWorldCreator
        Private _cameraDistance As Single = 0F

        'Stores the opacity of the normal state of this entity.
        'We need to overwrite the datamodel's opacity when we make this entity see-through for the camera.
        Private _normalOpacity As Single = 1.0F

        Private _position As Vector3 = Vector3.Zero
        Private _rotation As Vector3 = Vector3.Zero
        Private _scale As Vector3 = Vector3.One

        Public Property Position() As Vector3
            Get
                Return _position
            End Get
            Set(value As Vector3)
                _position = value
                _worldCreator.SetDirty(EntityWorldDirtyFlags.Position)
            End Set
        End Property

        Public Property Scale() As Vector3
            Get
                Return _scale
            End Get
            Set(value As Vector3)
                _scale = value
                _worldCreator.SetDirty(EntityWorldDirtyFlags.Scale)
            End Set
        End Property

        Public Property Rotation() As Vector3
            Get
                Return _rotation
            End Get
            Set(value As Vector3)
                _rotation = value
                _worldCreator.SetDirty(EntityWorldDirtyFlags.Rotation)
            End Set
        End Property

        ''' <summary>
        ''' Returns the tint of this entity.
        ''' </summary>
        Public ReadOnly Property DiffuseColor() As Vector3
            Get
                Dim diffColor As Vector3

                If Not Screen.Level.World Is Nothing Then
                    Select Case Screen.Level.World.EnvironmentType
                        Case Game.World.EnvironmentTypes.Outside
                            diffColor = SkyDome.GetEntityColor().ToVector3()
                        Case Game.World.EnvironmentTypes.Dark
                            diffColor = DARK_COLOR
                        Case Else
                            diffColor = DEFAULT_COLOR
                    End Select
                End If

                For Each col As Vector3 In Colors
                    diffColor *= col
                Next

                Return diffColor
            End Get
        End Property

        ''' <summary>
        ''' All tints that get applied to this entity.
        ''' </summary>
        Public Property Colors As List(Of Vector3) = New List(Of Vector3)()

        ''' <summary>
        ''' The world matrix describing transformation of this entity.
        ''' </summary>
        Public ReadOnly Property World() As Matrix
            Get
                Return _worldCreator.GetWorldMatrix()
            End Get
        End Property

        ''' <summary>
        ''' The data model of this entity.
        ''' </summary>
        Public ReadOnly Property DataModel As DataModel.Json.Game.EntityModel
            Get
                Return _dataModel
            End Get
        End Property

#Region "DataModel proxy properties"

        Public Property Visible() As Boolean
            Get
                Return _dataModel.RenderMode.Visible
            End Get
            Set(value As Boolean)
                _dataModel.RenderMode.Visible = value
            End Set
        End Property

        Public Property RenderMethod() As EntityRenderMode
            Get
                Return _dataModel.RenderMode.RenderMethod
            End Get
            Set(value As EntityRenderMode)
                _dataModel.RenderMode.RenderMethod = value
            End Set
        End Property

        Public Property RenderBackfaces() As Boolean
            Get
                Return _dataModel.RenderMode.RenderBackfaces
            End Get
            Set(value As Boolean)
                _dataModel.RenderMode.RenderBackfaces = value
            End Set
        End Property

        Public ReadOnly Property Model3D() As Model
            Get
                If _model Is Nothing AndAlso Not String.IsNullOrEmpty(_dataModel.RenderMode.ModelPath) Then
                    _model = GameMode.Active.GetModelManager().GetModel(_dataModel.RenderMode.ModelPath)
                End If

                Return _model
            End Get
        End Property

        Public ReadOnly Property Textures() As Texture2D()
            Get
                If _textures Is Nothing Then
                    _textures = New List(Of Texture2D)()

                    For Each textureSource In _dataModel.RenderMode.Textures
                        _textures.Add(GameMode.Active.GetTextureManager().GetTexture(textureSource))
                    Next
                End If

                Return _textures.ToArray()
            End Get
        End Property

        Public Property Opacity() As Single
            Get
                Return _dataModel.RenderMode.Opacity
            End Get
            Set(value As Single)
                _dataModel.RenderMode.Opacity = CDec(value)
                _normalOpacity = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Gets or sets this entity's face direction.
        ''' </summary>
        Public Property FaceDirection() As EntityFaceDirection
            Get
                Dim yRot As Single = _dataModel.Rotation.Y
                While yRot >= MathHelper.TwoPi
                    yRot -= MathHelper.TwoPi
                End While

                If yRot <= MathHelper.Pi * 0.25F Or yRot > MathHelper.Pi * 1.75F Then
                    Return EntityFaceDirection.North
                ElseIf yRot <= MathHelper.Pi * 0.75F And yRot > MathHelper.Pi * 0.25F Then
                    Return EntityFaceDirection.West
                ElseIf yRot <= MathHelper.Pi * 1.25F And yRot > MathHelper.Pi * 0.75F Then
                    Return EntityFaceDirection.South
                Else
                    Return EntityFaceDirection.East
                End If
            End Get
            Set(value As EntityFaceDirection)
                _dataModel.Rotation.Y = CDec(CInt(value) * MathHelper.PiOver2)
            End Set
        End Property

        ''' <summary>
        ''' Creates a new instance of the Entity class.
        ''' </summary>
        Public Sub New(ByVal dataModel As DataModel.Json.Game.EntityModel, ByVal position As Vector3)
            _dataModel = dataModel
            _normalOpacity = dataModel.RenderMode.Opacity
            _position = position
            _scale = dataModel.Scale.ToVector3()
            _rotation = dataModel.Scale.ToVector3()
            _colors.Add(dataModel.RenderMode.Shader.ToVector3())

            _worldCreator = New EntityWorldCreator(Me)

            InitializeProperties()
        End Sub

#Region "Properties"

        Private Sub InitializeProperties()
            ' Loops over the data models of the properties and creates actual instances:

            For Each propModel In _dataModel.Properties
                If Not _properties.Keys.Contains(propModel.Name) Then
                    Dim prop = EntityProperties.EntityPropertyFactory(Me, propModel)
                    _properties.Add(prop.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture), prop)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Returns a property of this entity.
        ''' </summary>
        ''' <param name="propertyName">The name of the property to return.</param>
        Public Function GetProperty(ByVal propertyName As String) As EntityProperties.EntityProperty
            If OwnsProperty(propertyName) Then
                Return _properties(propertyName.ToLower(System.Globalization.CultureInfo.InvariantCulture))
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Returns a property of this entity as its proper type.
        ''' </summary>
        Public Function GetProperty(Of T)(ByVal propertyName As String) As T
            If OwnsProperty(Of T)(propertyName) Then
                Return CType(CObj(GetProperty(propertyName)), T)
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Returns if this entity owns a property with a given name.
        ''' </summary>
        Public Function OwnsProperty(ByVal propertyName As String) As Boolean
            Return _properties.Keys.Contains(propertyName.ToLower(System.Globalization.CultureInfo.InvariantCulture))
        End Function

        ''' <summary>
        ''' Returns if this entity owns a property with a given name and type.
        ''' </summary>
        Public Function OwnsProperty(Of T)(ByVal propertyName As String) As Boolean
            Dim propNameLowerCase As String = propertyName.ToLower(System.Globalization.CultureInfo.InvariantCulture)

            If _properties.Keys.Contains(propNameLowerCase) Then
                Dim prop = _properties(propNameLowerCase)

                If TypeOf prop Is T Then
                    Return True
                End If
            End If

            Return False
        End Function

#End Region

#Region "Updating"

        ''' <summary>
        ''' Updates this entity.
        ''' </summary>
        Public Sub Update()
            UpdateProperties()
            UpdateEntity()
        End Sub

        ''' <summary>
        ''' Updates the properties of this entity.
        ''' </summary>
        Private Sub UpdateProperties()
            For Each prop In _properties.Values
                prop.Update()
            Next
        End Sub

        Private Sub UpdateEntity()
            DetermineCameraDistance()

            UpdateCameraObstructionOpacity()
        End Sub

        ''' <summary>
        ''' Updates this entity's opacity if it obstructs the camera.
        ''' </summary>
        Private Sub UpdateCameraObstructionOpacity()
            'We set the datamodel's opacity directly in this method,
            'because we don't want to set normalOpacity.

            _dataModel.RenderMode.Opacity = CDec(_normalOpacity)

            If Not _dataModel.RenderMode.ObstructCamera Then
                If _cameraDistance < 10.0F AndAlso Screen.Level.OwnPlayer IsNot Nothing AndAlso _cameraDistance < Screen.Level.OwnPlayer.CameraDistance Then
                    If Screen.Camera IsNot Nothing Then
                        If TypeOf Screen.Camera Is OverworldCamera Then
                            Dim ray As Ray = Screen.Camera.Ray
                            Dim rayCastResult As Single? = ray.Intersects(_worldCreator.GetBoundingBox())

                            If rayCastResult.HasValue Then
                                If rayCastResult.Value <= 0.3F + (CType(Screen.Camera, OverworldCamera).ThirdPersonOffset.Z - 1.5F) Then
                                    _dataModel.RenderMode.Opacity = CDec(_normalOpacity) - 0.5D
                                    If _dataModel.RenderMode.Opacity < 0.3D Then
                                        _dataModel.RenderMode.Opacity = 0.3D
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub DetermineCameraDistance()
            If GameCore.State.CurrentScreen IsNot Nothing Then
                Dim cameraPosition As Vector3 = Screen.Camera.Position

                If TypeOf Screen.Camera Is OverworldCamera Then
                    cameraPosition = CType(Screen.Camera, OverworldCamera).CPosition
                ElseIf TypeOf Screen.Camera Is BattleSystem.BattleCamera
                    cameraPosition = CType(Screen.Camera, BattleSystem.BattleCamera).CPosition
                End If

                _cameraDistance = Vector3.Distance(Position, cameraPosition)
            End If
        End Sub

#End Region

#Region "Rendering"

        Private Shared _noneCullRasterizerState As RasterizerState = Nothing
        Private Shared _normalCullRasterizerState As RasterizerState = Nothing

        Private _drawnLastFrame As Boolean = False

        Private ReadOnly Property NoneCullRasterizerState() As RasterizerState
            Get
                If _noneCullRasterizerState Is Nothing Then
                    _noneCullRasterizerState = New RasterizerState()
                    _noneCullRasterizerState.CullMode = CullMode.None
                End If
                Return _noneCullRasterizerState
            End Get
        End Property

        Private ReadOnly Property NormalCullRasterizerState() As RasterizerState
            Get
                If _normalCullRasterizerState Is Nothing Then
                    _normalCullRasterizerState = New RasterizerState()
                    _normalCullRasterizerState.CullMode = CullMode.CullCounterClockwiseFace
                End If
                Return _normalCullRasterizerState
            End Get
        End Property

        ''' <summary>
        ''' Renders this entity.
        ''' </summary>
        Public Sub Render()
            Dim rendered As Boolean = False

            For Each p As EntityProperties.EntityProperty In _properties.Values
                Dim renderResult = p.Render()
                Select Case renderResult
                    Case EntityProperties.EntityPropertyRenderResultType.Rendered
                        rendered = True
                        Exit For
                    Case EntityProperties.EntityPropertyRenderResultType.RenderedButPassed
                        rendered = True
                End Select
            Next

            'If none of the entity properties had a special render method, then render this entity with the default settings:
            If rendered = False Then
                DefaultEntityRender()
            End If
        End Sub

        ''' <summary>
        ''' Renders the entity with its default settings, ignoring render settings of properties.
        ''' </summary>
        Public Sub DefaultEntityRender()
            If Visible Then
                If IsInFieldOfView() Then
                    _drawnLastFrame = True

                    If RenderMethod = EntityRenderMode.Primitives Then
                        If RenderBackfaces Then
                            GameCore.State.GameController.GraphicsDevice.RasterizerState = NoneCullRasterizerState
                            Primitives.PrimitiveRenderer.Render(Me)
                            GameCore.State.GameController.GraphicsDevice.RasterizerState = NormalCullRasterizerState
                        Else
                            Primitives.PrimitiveRenderer.Render(Me)
                        End If

                    ElseIf RenderMethod = EntityRenderMode.Model
                        If Not Model3D Is Nothing Then
                            EntityModelRenderer.Render(Me)
                        End If
                    End If
                Else
                    _drawnLastFrame = False
                End If
            Else
                _drawnLastFrame = False
            End If
        End Sub

        Private Function IsInFieldOfView() As Boolean
            If RenderMethod = EntityRenderMode.Primitives Then
                Return Not Screen.Camera.BoundingFrustum.Contains(_worldCreator.GetViewBox()) = ContainmentType.Disjoint
            ElseIf RenderMethod = EntityRenderMode.Model
                If Model3D IsNot Nothing Then
                    If _modelBoundingSphere.HasValue = False Then
                        CreateModelBoundingSphere()
                    End If

                    Return Not Screen.Camera.BoundingFrustum.Contains(_modelBoundingSphere.Value) = ContainmentType.Disjoint
                End If
            End If

            Return False
        End Function

        Private Sub CreateModelBoundingSphere()
            Dim s As BoundingSphere = Model3D.Meshes(0).BoundingSphere
            If Model3D.Meshes.Count > 1 Then
                For i = 1 To Model3D.Meshes.Count - 1
                    s = BoundingSphere.CreateMerged(s, Model3D.Meshes(i).BoundingSphere)
                Next
            End If

            If _scale <> Vector3.One Then
                'Scale up the bounding sphere to the scale.
                'We determine the largest scale var, because the sphere has to stay in ball shape:
                Dim scaleV As Single = _scale.X
                If _scale.Y > scaleV Then
                    scaleV = _scale.Y
                End If
                If _scale.Z > scaleV Then
                    scaleV = _scale.Z
                End If

                s = s.Transform(Matrix.CreateScale(scaleV))
            End If

            _modelBoundingSphere = s
        End Sub

#End Region

#Region "Entity interactions"

        ''' <summary>
        ''' When the player interacts with this entity.
        ''' </summary>
        Public Sub Click()
            For Each p As EntityProperties.EntityProperty In _properties.Values
                p.Click()
            Next
        End Sub

        Public Function WalkAgainst() As Boolean
            For Each p As EntityProperties.EntityProperty In _properties.Values
                Dim response As EntityProperties.FunctionResponse = p.WalkAgainst()
                If response = EntityProperties.FunctionResponse.ValueFalse Then
                    Return False
                ElseIf response = EntityProperties.FunctionResponse.ValueTrue Then
                    Return True
                End If
            Next
            Return True
        End Function

        Public Function WalkInto() As Boolean
            For Each p As EntityProperties.EntityProperty In _properties.Values
                Dim response As EntityProperties.FunctionResponse = p.WalkInto()
                If response = EntityProperties.FunctionResponse.ValueFalse Then
                    Return False
                ElseIf response = EntityProperties.FunctionResponse.ValueTrue Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub WalkOnto()
            For Each p As EntityProperties.EntityProperty In _properties.Values
                p.WalkOnto()
            Next
        End Sub

        Public Sub ChooseBoxResult(ByVal resultIndex As Integer)
            For Each p As EntityProperties.EntityProperty In _properties.Values
                p.ChooseBoxResult(resultIndex)
            Next
        End Sub

        Public Function LetPlayerMove() As Boolean
            For Each p As EntityProperties.EntityProperty In _properties.Values
                Dim response As EntityProperties.FunctionResponse = p.LetPlayerMove()
                If response = EntityProperties.FunctionResponse.ValueFalse Then
                    Return False
                ElseIf response = EntityProperties.FunctionResponse.ValueTrue Then
                    Return True
                End If
            Next
            Return True
        End Function

#End Region

#Region "Misc entity functions"

        ''' <summary>
        ''' Snaps this entity to the grid of the game.
        ''' </summary>
        Public Sub SnapToGrid()
            Position = New Vector3(CInt(Position.X), Position.Y, CInt(Position.Z))
        End Sub

#End Region

    End Class

End Namespace
