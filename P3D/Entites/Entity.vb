Public Class Entity

    Inherits BaseEntity

    Public Shared MakeShake As Boolean = False
    Public Shared drawViewBox As Boolean = False

    Public ID As Integer = -1

    Public EntityID As String = ""
    Public MapOrigin As String = ""
    Public IsOffsetMapContent As Boolean = False
    Public Offset As Vector3 = New Vector3(0)
    Public Position As Vector3
    Public Rotation As Vector3 = New Vector3(0)
    Public Scale As Vector3 = New Vector3(1)
    Public Textures() As Texture2D
    Public TextureIndex() As Integer
    Public ActionValue As Integer
    Public AdditionalValue As String
    Public Model As BaseModel
    Public Visible As Boolean = True
    Public Shader As New Vector3(1.0F)
    Public Shaders As New List(Of Vector3)

    Public CameraDistanceDelta As Single = 0.0F

    Public SeasonColorTexture As String = ""

    Public FaceDirection As Integer = 0
    Public Moved As Single = 0.0F
    Public Speed As Single = 0.04F
    Public CanMove As Boolean = False
    Public isDancing As Boolean = False

    Public Opacity As Single = 1.0F
    Private _normalOpactity As Single = 1.0F
    Public Property NormalOpacity As Single
        Get
            Return Me._normalOpactity
        End Get
        Set(value As Single)
            Me.Opacity = value
            Me._normalOpactity = value
        End Set
    End Property

    Public boundingBoxScale As Vector3 = New Vector3(1.25F)
    Public boundingBox As BoundingBox

    Public ViewBox As BoundingBox
    Public viewBoxScale As Vector3 = New Vector3(1.0F)

    Public CameraDistance As Single
    Public World As Matrix
    Public CreatedWorld As Boolean = False
    Public CreateWorldEveryFrame As Boolean = False

    Public Collision As Boolean = True

    Public CanBeRemoved As Boolean = False
    Public NeedsUpdate As Boolean = False

    Shared newRasterizerState As RasterizerState
    Shared oldRasterizerState As RasterizerState

    Private BoundingPositionCreated As Vector3 = New Vector3(1110)
    Private BoundingRotationCreated As Vector3 = New Vector3(-1)

    Public HasEqualTextures As Integer = -1

    Private DrawnLastFrame As Boolean = True
    Protected DropUpdateUnlessDrawn As Boolean = True

    Public Sub New()
        MyBase.New(EntityTypes.Entity)
    End Sub

    Public Sub New(ByVal X As Single, ByVal Y As Single, ByVal Z As Single, ByVal EntityID As String, ByVal Textures() As Texture2D, ByVal TextureIndex() As Integer, ByVal Collision As Boolean, ByVal Rotation As Integer, ByVal Scale As Vector3, ByVal Model As BaseModel, ByVal ActionValue As Integer, ByVal AdditionalValue As String, ByVal Shader As Vector3)
        MyBase.New(EntityTypes.Entity)

        Me.Position = New Vector3(X, Y, Z)
        Me.EntityID = EntityID
        Me.Textures = Textures
        Me.TextureIndex = TextureIndex
        Me.Collision = Collision
        Me.Rotation = GetRotationFromInteger(Rotation)
        Me.Scale = Scale
        Me.Model = Model
        Me.ActionValue = ActionValue
        Me.AdditionalValue = AdditionalValue
        Me.Shader = Shader

        Initialize()
    End Sub

    Public Overridable Sub Initialize()
        If GetRotationFromVector(Me.Rotation) Mod 2 = 1 Then
            ViewBox = New BoundingBox(
                     Vector3.Transform(New Vector3(-(Me.Scale.Z / 2), -(Me.Scale.Y / 2), -(Me.Scale.X / 2)), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)),
                     Vector3.Transform(New Vector3((Me.Scale.Z / 2), (Me.Scale.Y / 2), (Me.Scale.X / 2)), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)))
        Else
            ViewBox = New BoundingBox(
                     Vector3.Transform(New Vector3(-(Me.Scale.X / 2), -(Me.Scale.Y / 2), -(Me.Scale.Z / 2)), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)),
                     Vector3.Transform(New Vector3((Me.Scale.X / 2), (Me.Scale.Y / 2), (Me.Scale.Z / 2)), Matrix.CreateScale(viewBoxScale) * Matrix.CreateTranslation(Position)))
        End If

        boundingBox = New BoundingBox(
                     Vector3.Transform(New Vector3(-0.5F), Matrix.CreateScale(boundingBoxScale) * Matrix.CreateTranslation(Position)),
                     Vector3.Transform(New Vector3(0.5F), Matrix.CreateScale(boundingBoxScale) * Matrix.CreateTranslation(Position)))

        Me.BoundingPositionCreated = Me.Position
        Me.BoundingRotationCreated = Me.Rotation

        If newRasterizerState Is Nothing Then
            newRasterizerState = New RasterizerState
            oldRasterizerState = New RasterizerState

            newRasterizerState.CullMode = CullMode.None
            oldRasterizerState.CullMode = CullMode.CullCounterClockwiseFace
        End If

        Me.LoadSeasonTextures()

        Me.UpdateEntity()
    End Sub

    Public Shared Function GetNewEntity(ByVal EntityID As String, ByVal Position As Vector3, ByVal Textures() As Texture2D, ByVal TextureIndex() As Integer, ByVal Collision As Boolean, ByVal Rotation As Vector3, ByVal Scale As Vector3, ByVal Model As BaseModel, ByVal ActionValue As Integer, ByVal AdditionalValue As String, ByVal Visible As Boolean, ByVal Shader As Vector3, ByVal ID As Integer, ByVal MapOrigin As String, ByVal SeasonColorTexture As String, ByVal Offset As Vector3, Optional ByVal Params() As Object = Nothing, Optional ByVal Opacity As Single = 1.0F, Optional ByVal AnimationData As List(Of List(Of Integer)) = Nothing, Optional ByVal CameraDistanceDelta As Single = 0.0F) As Entity
        Dim newEnt As New Entity()
        Dim propertiesEnt As New Entity()

        propertiesEnt.EntityID = EntityID
        propertiesEnt.Position = Position
        propertiesEnt.Textures = Textures
        propertiesEnt.TextureIndex = TextureIndex
        propertiesEnt.Collision = Collision
        propertiesEnt.Rotation = Rotation
        propertiesEnt.Scale = Scale
        propertiesEnt.Model = Model
        propertiesEnt.ActionValue = ActionValue
        propertiesEnt.AdditionalValue = AdditionalValue
        propertiesEnt.Visible = Visible
        propertiesEnt.Shader = Shader
        propertiesEnt.NormalOpacity = Opacity

        propertiesEnt.ID = ID
        propertiesEnt.MapOrigin = MapOrigin
        propertiesEnt.SeasonColorTexture = SeasonColorTexture
        propertiesEnt.Offset = Offset
        propertiesEnt.CameraDistanceDelta = CameraDistanceDelta

        Select Case EntityID.ToLower()
            Case "animatedblock"
                newEnt = New AnimatedBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, AnimatedBlock).Initialize(AnimationData)
            Case "wallblock"
                newEnt = New WallBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, WallBlock).Initialize()
            Case "cube", "allsidesobject"
                newEnt = New AllSidesObject()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, AllSidesObject).Initialize()
            Case "slideblock"
                newEnt = New SlideBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, SlideBlock).Initialize()
            Case "wallbill"
                newEnt = New WallBill()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, WallBill).Initialize()
            Case "signblock"
                newEnt = New SignBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, SignBlock).Initialize()
            Case "warpblock"
                newEnt = New WarpBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, WarpBlock).Initialize()
            Case "floor"
                newEnt = New Floor()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, Floor).Initialize(True, False, True)
            Case "step"
                newEnt = New StepBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, StepBlock).Initialize()
            Case "cuttree"
                newEnt = New CutDownTree()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, CutDownTree).Initialize()
            Case "water"
                newEnt = New Water()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, Water).Initialize()
            Case "grass"
                newEnt = New Grass()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, Grass).Initialize()
            Case "berryplant"
                newEnt = New BerryPlant()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, BerryPlant).Initialize()
            Case "loamysoil"
                newEnt = New LoamySoil()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, LoamySoil).Initialize()
            Case "itemobject"
                newEnt = New ItemObject()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, ItemObject).Initialize()
            Case "scriptblock"
                newEnt = New ScriptBlock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, ScriptBlock).Initialize()
            Case "turningsign"
                newEnt = New TurningSign()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, TurningSign).Initialize()
            Case "apricornplant"
                newEnt = New ApricornPlant()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, ApricornPlant).Initialize()
            Case "headbutttree"
                newEnt = New HeadbuttTree()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, HeadbuttTree).Initialize()
            Case "smashrock"
                newEnt = New SmashRock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, SmashRock).Initialize()
            Case "strengthrock"
                newEnt = New StrengthRock()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, StrengthRock).Initialize()
            Case "npc"
                newEnt = New NPC()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, NPC).Initialize(CStr(Params(0)), CInt(Params(1)), CStr(Params(2)), CInt(Params(3)), CBool(Params(4)), CStr(Params(5)), CType(Params(6), List(Of Rectangle)))
            Case "waterfall"
                newEnt = New Waterfall()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, Waterfall).Initialize()
            Case "whirlpool"
                newEnt = New Whirlpool()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, Whirlpool).Initialize()
            Case "strengthtrigger"
                newEnt = New StrengthTrigger()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, StrengthTrigger).Initialize()
            Case "modelentity"
                newEnt = New ModelEntity()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, ModelEntity).Initialize()
            Case "rotationtile"
                newEnt = New RotationTile()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, RotationTile).Initialize()
            Case "divetile"
                newEnt = New DiveTile()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, DiveTile).Initialize()
            Case "rockclimbentity"
                newEnt = New RockClimbEntity()
                SetProperties(newEnt, propertiesEnt)
                CType(newEnt, RockClimbEntity).Initialize()
        End Select

        Return newEnt
    End Function

    Friend Shared Sub SetProperties(ByRef newEnt As Entity, ByVal PropertiesEnt As Entity)
        newEnt.EntityID = PropertiesEnt.EntityID
        newEnt.Position = PropertiesEnt.Position
        newEnt.Textures = PropertiesEnt.Textures
        newEnt.TextureIndex = PropertiesEnt.TextureIndex
        newEnt.Collision = PropertiesEnt.Collision
        newEnt.Rotation = PropertiesEnt.Rotation
        newEnt.Scale = PropertiesEnt.Scale
        newEnt.Model = PropertiesEnt.Model
        newEnt.ActionValue = PropertiesEnt.ActionValue
        newEnt.AdditionalValue = PropertiesEnt.AdditionalValue
        newEnt.Visible = PropertiesEnt.Visible
        newEnt.Shader = PropertiesEnt.Shader
        newEnt.ID = PropertiesEnt.ID
        newEnt.MapOrigin = PropertiesEnt.MapOrigin
        newEnt.SeasonColorTexture = PropertiesEnt.SeasonColorTexture
        newEnt.Offset = PropertiesEnt.Offset
        newEnt.NormalOpacity = PropertiesEnt.Opacity
        newEnt.CameraDistanceDelta = PropertiesEnt.CameraDistanceDelta
    End Sub

    Public Shared Function GetRotationFromInteger(ByVal i As Integer) As Vector3
        Select Case i
            Case 0
                Return New Vector3(0, 0, 0)
            Case 1
                Return New Vector3(0, MathHelper.PiOver2, 0)
            Case 2
                Return New Vector3(0, MathHelper.Pi, 0)
            Case 3
                Return New Vector3(0, MathHelper.Pi * 1.5F, 0)
        End Select
    End Function

    Public Shared Function GetRotationFromVector(ByVal v As Vector3) As Integer
        Select Case v.Y
            Case 0
                Return 0
            Case MathHelper.PiOver2
                Return 1
            Case MathHelper.Pi
                Return 2
            Case MathHelper.Pi * 1.5F
                Return 3
        End Select

        Return 0
    End Function

    Protected Friend Sub LoadSeasonTextures()
        If SeasonColorTexture <> "" Then
            Dim newTextures As New List(Of Texture2D)
            For Each t As Texture2D In Textures
                newTextures.Add(P3D.World.GetSeasonTexture(TextureManager.GetTexture("Textures\Seasons\" & Me.SeasonColorTexture), t))
            Next
            Me.Textures = newTextures.ToArray()
        End If
    End Sub

    Public Overridable Sub Update()

    End Sub

    Public Overridable Sub OpacityCheck()
        If Me.CameraDistance > 10.0F Or
            Screen.Level.OwnPlayer IsNot Nothing AndAlso CameraDistance > Screen.Level.OwnPlayer.CameraDistance Then

            Me.Opacity = Me._normalOpactity
            Exit Sub
        End If

        Dim notNames() As String = {"Floor", "OwnPlayer", "Water", "Whirlpool", "Particle", "OverworldPokemon", "ItemObject", "NetworkPokemon", "NetworkPlayer"}
        If Screen.Camera.Name = "Overworld" AndAlso notNames.Contains(Me.EntityID) = False Then
            Me.Opacity = Me._normalOpactity
            If CType(Screen.Camera, OverworldCamera).ThirdPerson = True Then
                Dim Ray As Ray = Screen.Camera.Ray
                Dim result As Single? = Ray.Intersects(Me.boundingBox)
                If result.HasValue = True Then
                    If result.Value < 0.3F + (CType(Screen.Camera, OverworldCamera).ThirdPersonOffset.Z - 1.5F) Then
                        Me.Opacity = Me._normalOpactity - 0.5F
                        If Me.Opacity < 0.3F Then
                            Me.Opacity = 0.3F
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Overridable Function GetCameraDistanceCenterPoint() As Vector3
        Return Me.Position + Me.GetCenter()
    End Function

    Protected Overridable Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return Vector3.Distance(Me.GetCameraDistanceCenterPoint(), CPosition) + CameraDistanceDelta
    End Function

    Public Overridable Sub UpdateEntity()
        Dim CPosition As Vector3 = Screen.Camera.Position
        Dim ActionScriptActive As Boolean = False


        If Not Core.CurrentScreen Is Nothing Then
            CPosition = Screen.Camera.CPosition
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                ActionScriptActive = Not CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady
            End If
        End If


        CameraDistance = CalculateCameraDistance(CPosition)

        If Me.DropUpdateUnlessDrawn = True And Me.DrawnLastFrame = False And Me.Visible = True And ActionScriptActive = False Then
            Exit Sub
        End If


        If Me.Moved > 0.0F And Me.CanMove = True Then
            Me.Moved -= Me.Speed

            Dim movement As Vector3 = Vector3.Zero
            Select Case Me.FaceDirection
                Case 0
                    movement = New Vector3(0, 0, -1)
                Case 1
                    movement = New Vector3(-1, 0, 0)
                Case 2
                    movement = New Vector3(0, 0, 1)
                Case 3
                    movement = New Vector3(1, 0, 0)
            End Select

            movement *= Speed

            Me.Position += movement
            Me.CreatedWorld = False

            If Me.Moved <= 0.0F Then
                Me.Moved = 0.0F

                Me.Position.X = CInt(Me.Position.X)
                Me.Position.Z = CInt(Me.Position.Z)

            End If
        End If

        If Me.IsOffsetMapContent = False Then
            OpacityCheck()
        End If

        If CreatedWorld = False Or CreateWorldEveryFrame = True Then
            World = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position)
            CreatedWorld = True
        End If

        If CameraDistance < Screen.Camera.FarPlane * 2 Then
            If Me.Position <> Me.BoundingPositionCreated Then
                Dim diff As New List(Of Single)
                diff.AddRange({Me.BoundingPositionCreated.X - Me.Position.X, Me.BoundingPositionCreated.Y - Me.Position.Y, Me.BoundingPositionCreated.Z - Me.Position.Z})

                ViewBox.Min.X -= diff(0)
                ViewBox.Min.Y -= diff(1)
                ViewBox.Min.Z -= diff(2)

                ViewBox.Max.X -= diff(0)
                ViewBox.Max.Y -= diff(1)
                ViewBox.Max.Z -= diff(2)

                boundingBox.Min.X -= diff(0)
                boundingBox.Min.Y -= diff(1)
                boundingBox.Min.Z -= diff(2)

                boundingBox.Max.X -= diff(0)
                boundingBox.Max.Y -= diff(1)
                boundingBox.Max.Z -= diff(2)

                Me.BoundingPositionCreated = Me.Position
            End If
        End If

        If MakeShake = True Then
            If Core.Random.Next(0, 1) = 0 Then
                Me.Rotation.X += CSng((Core.Random.Next(1, 6) - 3) / 100)
                Me.Rotation.Z += CSng((Core.Random.Next(1, 6) - 3) / 100)
                Me.Rotation.Y += CSng((Core.Random.Next(1, 6) - 3) / 100)

                Me.Position.X += CSng((Core.Random.Next(1, 6) - 3) / 100)
                Me.Position.Z += CSng((Core.Random.Next(1, 6) - 3) / 100)
                Me.Position.Y += CSng((Core.Random.Next(1, 6) - 3) / 100)

                Me.Scale.X += CSng((Core.Random.Next(1, 6) - 3) / 100)
                Me.Scale.Z += CSng((Core.Random.Next(1, 6) - 3) / 100)
                Me.Scale.Y += CSng((Core.Random.Next(1, 6) - 3) / 100)

                CreatedWorld = False
            End If
        End If

        If Not Screen.Level.World Is Nothing Then
            Select Case Screen.Level.World.EnvironmentType
                Case P3D.World.EnvironmentTypes.Outside
                    Me.Shader = SkyDome.GetDaytimeColor(True).ToVector3()
                Case P3D.World.EnvironmentTypes.Dark
                    Me.Shader = New Vector3(0.5F, 0.5F, 0.6F)
                Case Else
                    Me.Shader = New Vector3(1.0F)
            End Select
        End If

        For Each s As Vector3 In Me.Shaders
            Me.Shader *= s
        Next
    End Sub

    Dim tempCenterVector As Vector3 = Vector3.Zero

    ''' <summary>
    ''' Returns the offset from the 0,0,0 center of the position of the entity.
    ''' </summary>
    Private Function GetCenter() As Vector3
        If CreatedWorld = False Or CreateWorldEveryFrame = True Then
            Dim v As Vector3 = Vector3.Zero '(Me.ViewBox.Min - Me.Position) + (Me.ViewBox.Max - Me.Position)

            If Not Me.Model Is Nothing Then
                Select Case Me.Model.ID
                    Case 0, 9, 10, 11
                        v.Y -= 0.5F
                End Select
            End If
            Me.tempCenterVector = v
        End If

        Return Me.tempCenterVector
    End Function

    Public Overridable Sub Draw(ByVal Model As BaseModel, ByVal Textures() As Texture2D, ByVal setRasterizerState As Boolean)
        If Visible = True Then
            If Me.IsInFieldOfView() = True Then
                If setRasterizerState = True Then
                    Core.GraphicsDevice.RasterizerState = newRasterizerState
                End If

                Model.Draw(Me, Textures)

                If setRasterizerState = True Then
                    Core.GraphicsDevice.RasterizerState = oldRasterizerState
                End If

                Me.DrawnLastFrame = True

                If Me.EntityID <> "Floor" And Me.EntityID <> "Water" Then
                    If drawViewBox = True Then
                        BoundingBoxRenderer.Render(ViewBox, Core.GraphicsDevice, Screen.Camera.View, Screen.Camera.Projection, Color.LightCoral)
                    End If
                End If
            Else
                Me.DrawnLastFrame = False
            End If
        Else
            Me.DrawnLastFrame = False
        End If
    End Sub

    Public Overridable Sub Render()

    End Sub

    Public Overridable Sub ClickFunction()

    End Sub

    Public Overridable Function WalkAgainstFunction() As Boolean
        Return True
    End Function

    Public Overridable Function WalkIntoFunction() As Boolean
        Return False
    End Function

    Public Overridable Sub WalkOntoFunction()

    End Sub

    Public Overridable Sub ResultFunction(ByVal Result As Integer)

    End Sub

    Public Overridable Function LetPlayerMove() As Boolean
        Return True
    End Function

    Public _visibleLastFrame As Boolean = False
    Public _occluded As Boolean = False

    Public Function IsInFieldOfView() As Boolean
        If Not Screen.Camera.BoundingFrustum.Contains(Me.ViewBox) = ContainmentType.Disjoint Then
            Me._visibleLastFrame = True
            Return True
        Else
            Me._visibleLastFrame = False
            Return False
        End If
    End Function

    Dim _cachedVertexCount As Integer = -1 'Stores the vertex count so it doesnt need to be recalculated.

    Public ReadOnly Property VertexCount() As Integer
        Get
            If Me._cachedVertexCount = -1 Then
                If Not Me.Model Is Nothing Then
                    Dim c As Integer = CInt(Me.Model.vertexBuffer.VertexCount / 3)
                    Dim min As Integer = 0

                    For i = 0 To Me.TextureIndex.Length - 1
                        If i <= c - 1 Then
                            If TextureIndex(i) > -1 Then
                                min += 1
                            End If
                        End If
                    Next

                    Me._cachedVertexCount = min
                Else
                    Me._cachedVertexCount = 0
                End If
            End If
            Return Me._cachedVertexCount
        End Get
    End Property

    Protected Function GetEntity(ByVal List As List(Of Entity), ByVal Position As Vector3, ByVal IntComparison As Boolean, ByVal validEntitytypes As Type()) As Entity
        For Each e As Entity In (From selEnt As Entity In List Select selEnt Where validEntitytypes.Contains(selEnt.GetType()))
            If IntComparison = True Then
                If e.Position.X.ToInteger() = Position.X.ToInteger() And e.Position.Y.ToInteger() = Position.Y.ToInteger() And e.Position.Z.ToInteger() = Position.Z.ToInteger() Then
                    Return e
                End If
            Else
                If e.Position.X = Position.X And e.Position.Y = Position.Y And e.Position.Z = Position.Z Then
                    Return e
                End If
            End If
        Next
        Return Nothing
    End Function

End Class
