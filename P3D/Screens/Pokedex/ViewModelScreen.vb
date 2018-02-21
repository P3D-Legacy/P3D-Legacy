Public Class ViewModelScreen

    Inherits Screen

    Dim Model As ModelEntity
    Dim Ground As WallBlock

    Dim c As Camera

    Dim normalModel As Boolean = True
    Dim PokemonAnimationName As String = ""
    Dim CanViewShiny As Boolean = False

    Public Sub New(ByVal currentScreen As Screen, ByVal PokemonAnimationName As String, ByVal CanViewShiny As Boolean)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.ViewModelScreen

        Me.Model = CType(Entity.GetNewEntity("ModelEntity", New Vector3(0), {}, {}, False, New Vector3(MathHelper.Pi * 0.5F, 0, 0), New Vector3(0.1F), BaseModel.BlockModel, 0, "Models\" & PokemonAnimationName & "\Normal", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing), ModelEntity)
        Me.Ground = CType(Entity.GetNewEntity("WallBlock", New Vector3(0, -0.5, 0), {TextureManager.GetTexture("Textures\ModelViewer\Ground")}, {-1, -1, -1, -1, -1, -1, -1, -1, 0, 0}, False, New Vector3(0.0F), New Vector3(4.0F, 1.0F, 4.0F), BaseModel.BlockModel, 0, "", True, New Vector3(1.0F), 0, "", "", New Vector3(0.0F), {}), WallBlock)

        Me.CanBePaused = True
        Me.MouseVisible = False

        Me.c = Screen.Camera

        Me.PokemonAnimationName = PokemonAnimationName
        Me.CanViewShiny = CanViewShiny

        Screen.Camera = New ViewModelCamera()
    End Sub

    Public Overrides Sub Draw()
        SkyDome.Draw(Camera.FOV)
        Me.Model.Render()

        Screen.Effect.View = Screen.Camera.View
        Screen.Effect.Projection = Screen.Camera.Projection
        Me.Ground.Render()
    End Sub

    Dim turnDelay As Single = 10.0F

    Public Overrides Sub Update()
        Camera.Update()
        SkyDome.Update()

        turnDelay -= 0.1F

        If KeyBoardHandler.KeyDown(Keys.Left) = True Then
            Me.Model.Rotation.Y -= 0.025F
            Me.Model.CreatedWorld = False
            Me.Ground.Rotation.Y -= 0.025F
            Me.Ground.CreatedWorld = False
            turnDelay = 10.0F
        End If
        If KeyBoardHandler.KeyDown(Keys.Right) = True Then
            Me.Model.Rotation.Y += 0.025F
            Me.Model.CreatedWorld = False
            Me.Ground.Rotation.Y += 0.025F
            Me.Ground.CreatedWorld = False
            turnDelay = 10.0F
        End If

        If turnDelay <= 0.0F Then
            turnDelay = 0.0F
            Me.Model.Rotation.Y -= 0.015F
            Me.Model.CreatedWorld = False
            Me.Ground.Rotation.Y -= 0.015F
            Me.Ground.CreatedWorld = False
        End If

        If Controls.Accept(True, True, True) = True And CanViewShiny = True Then
            Me.normalModel = Not Me.normalModel
            If Me.normalModel = False Then
                Me.Model.LoadModel("Models\" & Me.PokemonAnimationName & "\Shiny")
            Else
                Me.Model.LoadModel("Models\" & Me.PokemonAnimationName & "\Normal")
            End If
        End If

        Me.Model.UpdateEntity()
        Me.Model.Update()

        Me.Ground.UpdateEntity()
        Me.Ground.Update()

        If Controls.Dismiss(True, True, True) = True Then
            Screen.Camera = c
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

End Class