Namespace XmlLevel

    Public Class XmlEntity

        Inherits BaseEntity

#Region "PropertyShenanigans"

        Private _properties As New List(Of XmlProperty) 'The list of properties. The values of the properties represent the spawned or added property values. If they change, they get changed in the stored property list.
        Private _storedProperties As New Dictionary(Of String, Object) 'This efficiently stores all currently active property values.

        Private _propertyListeners As New List(Of XmlPropertyListener) 'The list of property listeners. A property listener provides functionality for an entity.

        ''' <summary>
        ''' Returns the value of a property.
        ''' </summary>
        ''' <typeparam name="T">The type of the property value.</typeparam>
        ''' <param name="Name">The name of the property.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPropertyValue(Of T)(ByVal Name As String) As T
            If _storedProperties.ContainsKey(Name.ToLower()) = True Then
                Return CType(_storedProperties(Name.ToLower()), T)
            End If

            'If the property isn't stored already, add it to the store.
            Dim newPropertyValue As Object = XmlProperty.ConvertFromString(GetProperty(Name))
            _storedProperties.Add(Name.ToLower(), newPropertyValue)
            Return CType(newPropertyValue, T)
        End Function

        ''' <summary>
        ''' Returns a property of this entity based on its name.
        ''' </summary>
        ''' <param name="Name">The name of the property.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetProperty(ByVal Name As String) As XmlProperty
            For i = 0 To Me._properties.Count - 1
                If Me._properties(i).Name.ToLower() = Name.ToLower() Then
                    Return Me._properties(i)
                End If
            Next

            'If this entity doesn't have the property, add the default property.
            Dim newProperty As XmlProperty = XmlProperty.GetDefaultProperty(Name)
            Me._properties.Add(newProperty)
            Return newProperty
        End Function

        ''' <summary>
        ''' Sets the value of a property.
        ''' </summary>
        ''' <typeparam name="T">The type of the propertie's value</typeparam>
        ''' <param name="Name">The name of the property.</param>
        ''' <param name="Value">The new value of the property.</param>
        ''' <remarks></remarks>
        Public Sub SetPropertyValue(Of T)(ByVal Name As String, ByVal Value As T)
            If _storedProperties.ContainsKey(Name.ToLower()) = True Then
                _storedProperties(Name.ToLower()) = Value
            Else
                'Add a new store property if it doesn't exist already.
                _storedProperties.Add(Name.ToLower(), Value)
            End If
        End Sub

        ''' <summary>
        ''' Adds a new property to the list of properties.
        ''' </summary>
        ''' <param name="Name">The name of the property.</param>
        ''' <param name="Value">The value of the property.</param>
        ''' <remarks></remarks>
        Public Sub AddProperty(ByVal Name As String, ByVal Value As String)
            Dim defaultProperty As XmlProperty = XmlProperty.GetDefaultProperty(Name)
            Me._properties.Add(New XmlProperty(Name, Value, defaultProperty.Type))
        End Sub

        Public Function GetPropertyListener(ByVal AssociatedPropertyName As String) As XmlPropertyListener
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                If PropertyListener.AssociatedPropertyName.ToLower() = AssociatedPropertyName.ToLower() Then
                    Return PropertyListener
                End If
            Next
            Return Nothing
        End Function

        Public Function GetPropertyListener(Of T)(ByVal AssociatedPropertyName As String) As T
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                If PropertyListener.AssociatedPropertyName.ToLower() = AssociatedPropertyName.ToLower() Then
                    Return CType(CObj(PropertyListener), T)
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Checks if this entity has the requested PropertyListener.
        ''' </summary>
        ''' <param name="AssociatedPropertyName">The name of the PropertyListener.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasPropertyListener(ByVal AssociatedPropertyName As String) As Boolean
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                If PropertyListener.AssociatedPropertyName.ToLower() = AssociatedPropertyName.ToLower() Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' Adds a PropertyListener to the list of PropertyListeners.
        ''' </summary>
        ''' <param name="PropertyListener">The PropertyListener to add.</param>
        ''' <remarks></remarks>
        Public Sub AddPropertyListener(ByVal PropertyListener As XmlPropertyListener)
            Me._propertyListeners.Add(PropertyListener)
        End Sub

        ''' <summary>
        ''' Returns if a property with a specific name exists for this entity.
        ''' </summary>
        ''' <param name="PropertyName">The name of the property.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PropertyExists(ByVal PropertyName As String) As Boolean
            Get
                For Each p As XmlProperty In Me._properties
                    If p.Name.ToLower() = propertyName.ToLower() Then
                        Return True
                    End If
                Next
                Return False
            End Get
        End Property

        ''' <summary>
        ''' Returns a list of names of the properties of this entity.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPropertyNameList() As List(Of String)
            Dim ll As New List(Of String)

            For Each p As XmlProperty In Me._properties
                ll.Add(p.Name)
            Next

            Return ll
        End Function

#End Region

#Region "QuickAccessProperties"

        Public Property Scale() As Vector3
            Get
                Return GetPropertyValue(Of Vector3)("scale")
            End Get
            Set(value As Vector3)
                SetPropertyValue(Of Vector3)("scale", value)
            End Set
        End Property

        Public Property Rotation() As Vector3
            Get
                Return GetPropertyValue(Of Vector3)("rotation")
            End Get
            Set(value As Vector3)
                SetPropertyValue(Of Vector3)("rotation", value)
            End Set
        End Property

        Public Property Position() As Vector3
            Get
                Return GetPropertyValue(Of Vector3)("position")
            End Get
            Set(value As Vector3)
                SetPropertyValue(Of Vector3)("position", value)
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return GetPropertyValue(Of Boolean)("visible")
            End Get
            Set(value As Boolean)
                SetPropertyValue(Of Boolean)("visible", value)
            End Set
        End Property

        Public Property RenderType() As String
            Get
                Dim s As String = GetPropertyValue(Of String)("rendertype")
                Dim validRenderTypes() As String = {"model", "basemodel"}
                If validRenderTypes.Contains(s.ToLower()) = True Then
                    Return s
                Else
                    Return "basemodel"
                End If
            End Get
            Set(value As String)
                SetPropertyValue(Of String)("rendertype", value)
            End Set
        End Property

        Public Property TextureIndex() As Integer()
            Get
                Return Me.GetPropertyValue(Of List(Of Integer))("textureindex").ToArray()
            End Get
            Set(value As Integer())
                Me.SetPropertyValue(Of List(Of Integer))("textureindex", value.ToList())
            End Set
        End Property

        Public Property Shader() As Vector3
            Get
                Return GetPropertyValue(Of Vector3)("shader")
            End Get
            Set(value As Vector3)
                SetPropertyValue(Of Vector3)("shader", value)
            End Set
        End Property

        Public Property Opacity() As Single
            Get
                Return GetPropertyValue(Of Single)("opacity")
            End Get
            Set(value As Single)
                Me.SetPropertyValue(Of Single)("opacity", value)
            End Set
        End Property

#End Region

        Public CreatedWorld As Boolean = False
        Public CreateWorldEveryFrame As Boolean = False
        Public CameraDistance As Single = 0.0F
        Public Textures As New List(Of Texture2D)
        Public World As Matrix = Nothing
        Public Shaders As New List(Of Vector3)

        Public IsOffsetMapContent As Boolean = False

        Public Viewbox As BoundingBox
        Public BoundingBox As BoundingBox
        Dim ViewBoxScale As Vector3 = Vector3.One
        Dim BoundingBoxScale As Vector3 = New Vector3(1.5F)
        Dim BoundingBoxCreated As Vector3 = New Vector3(-999)

        Private _needsUpdate As Boolean = False 'Can only be set to true by PropertyListeners
        Private EntityRenderer As XmlEntityRenderer

#Region "Initialize"

        Public Sub New()
            MyBase.New(EntityTypes.XmlEntity)

            Me.EntityRenderer = New XmlEntityRenderer(Me)
        End Sub

        Public Sub Initialize()
            Me._propertyListeners = XmlPropertyListener.GetPropertyListeners(Me)

            Me.CreateBoundingBoxes()
            Me.LoadTextures()
            Me.LoadSeasonTextures()
            Me.UpdateEntity()
        End Sub

        Private Sub LoadTextures()
            Me.Textures.Clear()

            Dim TexturePath As String = Me.GetPropertyValue(Of String)("texturepath")

            For Each TextureRectangle As Rectangle In GetPropertyValue(Of List(Of Rectangle))("textures")
                Me.Textures.Add(net.Pokemon3D.Game.TextureManager.GetTexture(TexturePath, TextureRectangle))
            Next
        End Sub

        Private Sub LoadSeasonTextures()
            Dim seasonTexture As String = GetPropertyValue(Of String)("seasontexture")

            If seasonTexture <> "" Then
                Dim newTextures As New List(Of Texture2D)
                For Each t As Texture2D In Textures
                    newTextures.Add(net.Pokemon3D.Game.World.GetSeasonTexture(net.Pokemon3D.Game.TextureManager.GetTexture("Textures\Seasons\" & seasonTexture), t))
                Next
                Me.Textures = newTextures
            End If
        End Sub

        Private Sub CreateBoundingBoxes()
            Viewbox = New BoundingBox(
                 Vector3.Transform(New Vector3(-(Me.Scale.X / 2), -(Me.Scale.Y / 2), -(Me.Scale.Z / 2)), Matrix.CreateScale(ViewBoxScale) * Matrix.CreateTranslation(Position)),
                 Vector3.Transform(New Vector3((Me.Scale.X / 2), (Me.Scale.Y / 2), (Me.Scale.Z / 2)), Matrix.CreateScale(ViewBoxScale) * Matrix.CreateTranslation(Position)))

            BoundingBox = New BoundingBox(
             Vector3.Transform(New Vector3(-0.5F), Matrix.CreateScale(BoundingBoxScale) * Matrix.CreateTranslation(Position)),
             Vector3.Transform(New Vector3(0.5F), Matrix.CreateScale(BoundingBoxScale) * Matrix.CreateTranslation(Position)))

            Me.BoundingBoxCreated = Me.Position
        End Sub

#End Region

#Region "Interactions"

        Public Sub PlayerInteraction()
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                PropertyListener.PlayerInteraction()
            Next
        End Sub

        Public Function WalkAgainst() As Boolean
            Dim b As Boolean = True

            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                If PropertyListener.ImplementWalkAgainst = True Then
                    b = PropertyListener.WalkAgainst()
                End If
            Next

            Return b
        End Function

        Public Function WalkInto() As Boolean
            Dim b As Boolean = True

            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                If PropertyListener.ImplementWalkInto = True Then
                    b = PropertyListener.WalkInto()
                End If
            Next

            Return b
        End Function

        Public Sub WalkOnto()
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                PropertyListener.WalkOnto()
            Next
        End Sub

        Public Sub ResultFunction(ByVal Result As Integer)
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                PropertyListener.ResultFunction(Result)
            Next
        End Sub

        Public Function LetPlayerMove() As Boolean
            Dim b As Boolean = True

            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                If PropertyListener.ImplementLetPlayerMove = True Then
                    b = PropertyListener.LetPlayerMove()
                End If
            Next

            Return b
        End Function

#End Region

#Region "Update/Render"

        Public Sub UpdateEntity()
            For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                PropertyListener.UpdateEntity()
            Next

            'Do normal entity update stuff:
            Me.CalculateCameraDistance()
            Me.OpacityCheck()
            Me.CreateWorldMatrix()
            Me.UpdateBoundingBoxes()
            Me.SetShader()
        End Sub

        Public Sub Update()
            If Me._needsUpdate = True Then
                For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                    PropertyListener.Update()
                Next
            End If
        End Sub

        Public Sub Draw()
            If Visible = True Then
                'Draw everything on screen like weird stuff an entity might need. Is empty right now.
                For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                    PropertyListener.Draw()
                Next
            End If
        End Sub

        Public Sub Render()
            If Visible = True Then
                If IsInFieldOfView() = True Then
                    For Each PropertyListener As XmlPropertyListener In Me._propertyListeners
                        PropertyListener.Render()
                    Next

                    Me.EntityRenderer.Render()
                End If
            End If
        End Sub

#End Region

        Public Sub EnableUpdate()
            Me._needsUpdate = True
        End Sub

        Public Function IsInFieldOfView() As Boolean
            If Not Screen.Camera.BoundingFrustum.Contains(Me.Viewbox) = ContainmentType.Disjoint Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function GetCenter() As Vector3
            If Not Me.EntityRenderer.BaseModel Is Nothing Then
                Select Case Me.EntityRenderer.BaseModel.ID
                    Case 0, 9, 10, 11
                        Return New Vector3(0.0F, -0.5F, 0.0F)
                End Select
            End If

            'Default:
            Return Vector3.Zero
        End Function

        Private Sub CalculateCameraDistance()
            'Get camera position:
            Dim CPosition As Vector3 = Screen.Camera.Position
            If Not Core.CurrentScreen Is Nothing Then
                If Screen.Camera.Name.ToLower() = "overworld" Then
                    CPosition = CType(Screen.Camera, OverworldCamera).CPosition
                End If
            End If

            'Calculate camera distance:
            Me.CameraDistance = Vector3.Distance(Me.Position + Me.GetCenter(), CPosition)
        End Sub

        Private Sub OpacityCheck()
            'Do opacity check:
            If Me.IsOffsetMapContent = False Then
                'Me.OpacityCheck()
            End If
        End Sub

        Private Sub CreateWorldMatrix()
            'Create the world matrix, if required:
            If Me.CreatedWorld = False Or Me.CreateWorldEveryFrame = True Then
                World = Matrix.CreateScale(Me.Scale) * Matrix.CreateFromYawPitchRoll(Me.Rotation.Y, Me.Rotation.X, Me.Rotation.Z) * Matrix.CreateTranslation(Me.Position)
                CreatedWorld = True
            End If
        End Sub

        Private Sub UpdateBoundingBoxes()
            'Create view and bounding boxes, if required:
            If CameraDistance < Screen.Camera.FarPlane * 2 Then
                If Me.Position <> Me.BoundingBoxCreated Then
                    Dim diff As New List(Of Single)
                    diff.AddRange({Me.BoundingBoxCreated.X - Me.Position.X, Me.BoundingBoxCreated.Y - Me.Position.Y, Me.BoundingBoxCreated.Z - Me.Position.Z})

                    Viewbox.Min.X -= diff(0)
                    Viewbox.Min.Y -= diff(1)
                    Viewbox.Min.Z -= diff(2)

                    Viewbox.Max.X -= diff(0)
                    Viewbox.Max.Y -= diff(1)
                    Viewbox.Max.Z -= diff(2)

                    BoundingBox.Min.X -= diff(0)
                    BoundingBox.Min.Y -= diff(1)
                    BoundingBox.Min.Z -= diff(2)

                    BoundingBox.Max.X -= diff(0)
                    BoundingBox.Max.Y -= diff(1)
                    BoundingBox.Max.Z -= diff(2)

                    Me.BoundingBoxCreated = Me.Position
                End If
            End If
        End Sub

        Private Sub SetShader()
            Select Case Screen.Level.World.EnvironmentType
                Case net.Pokemon3D.Game.World.EnvironmentTypes.Outside
                    Me.Shader = SkyDome.GetDaytimeColor(True).ToVector3()
                Case net.Pokemon3D.Game.World.EnvironmentTypes.Dark
                    Me.Shader = New Vector3(0.5F, 0.5F, 0.6F)
                Case Else
                    Me.Shader = New Vector3(1.0F)
            End Select

            For Each s As Vector3 In Me.Shaders
                Me.Shader *= s
            Next
        End Sub

    End Class

End Namespace