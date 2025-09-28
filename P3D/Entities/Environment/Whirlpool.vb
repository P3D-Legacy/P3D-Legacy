Public Class Whirlpool

    Inherits Entity

    Public Shared LoadedWaterTemp As Boolean = False
    Public Shared WaterTexturesTemp As New Dictionary(Of String, Texture2D)

    Dim WaterTextureName As String = ""
    Dim WaterAnimation As Animation
    Dim currentRectangle As New Rectangle(0, 0, 0, 0)

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        WaterAnimation = New Animation(P3D.TextureManager.GetTexture("Textures\Routes"), 1, 4, 16, 16, 9, 12, 0)

        CreateWaterTextureTemp()
    End Sub
    Public Shared Sub ClearAnimationResources()
        WaterTexturesTemp.Clear()
    End Sub

    Public Sub CreateWaterTextureTemp()
        If Core.GameOptions.GraphicStyle = 1 Then
            Dim textureData As List(Of String) = Me.AdditionalValue.Split(CChar(",")).ToList()
            If textureData.Count >= 4 Then
                Dim r As New Rectangle(CInt(textureData(1)), CInt(textureData(2)), CInt(textureData(3)), CInt(textureData(4)))
                Dim texturePath As String = textureData(0)
                Me.WaterTextureName = AdditionalValue

                If Whirlpool.WaterTexturesTemp.ContainsKey(AdditionalValue & "_0") = False Then
                    Whirlpool.WaterTexturesTemp.Add(AdditionalValue & "_0", TextureManager.GetTexture(texturePath, New Rectangle(r.X, r.Y, r.Width, r.Height)))
                    Whirlpool.WaterTexturesTemp.Add(AdditionalValue & "_1", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width, r.Y, r.Width, r.Height)))
                    Whirlpool.WaterTexturesTemp.Add(AdditionalValue & "_2", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 2, r.Y, r.Width, r.Height)))
                    Whirlpool.WaterTexturesTemp.Add(AdditionalValue & "_3", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 3, r.Y, r.Width, r.Height)))
                End If
            Else
                If Whirlpool.WaterTexturesTemp.ContainsKey("_0") = False Then
                    Whirlpool.WaterTexturesTemp.Add("_0", TextureManager.GetTexture("Routes", New Rectangle(0, 176, 16, 16)))
                    Whirlpool.WaterTexturesTemp.Add("_1", TextureManager.GetTexture("Routes", New Rectangle(16, 176, 16, 16)))
                    Whirlpool.WaterTexturesTemp.Add("_2", TextureManager.GetTexture("Routes", New Rectangle(32, 176, 16, 16)))
                    Whirlpool.WaterTexturesTemp.Add("_3", TextureManager.GetTexture("Routes", New Rectangle(48, 176, 16, 16)))
                End If
            End If
        End If
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Model Is Nothing Then
            If Not WaterAnimation Is Nothing Then
                WaterAnimation.Update(0.01)
                If currentRectangle <> WaterAnimation.TextureRectangle Then
                    ChangeTexture()

                    currentRectangle = WaterAnimation.TextureRectangle
                End If
            End If
        Else
            Me.Rotation.Y += 0.01F
        End If
        MyBase.UpdateEntity()
    End Sub

    Private Sub ChangeTexture()
        If Core.GameOptions.GraphicStyle = 1 Then
            If WaterTexturesTemp.Count = 0 Then
                ClearAnimationResources()
                CreateWaterTextureTemp()
            End If
            Select Case WaterAnimation.CurrentColumn
                Case 0
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(WaterTextureName & "_0")
                Case 1
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(WaterTextureName & "_1")
                Case 2
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(WaterTextureName & "_2")
                Case 3
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(WaterTextureName & "_3")
            End Select
        End If
    End Sub

    Public Overrides Sub Render()
        Dim setRasterizerState As Boolean = Me.BaseModel.ID <> 0
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, setRasterizerState)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

    Private Function ReturnWhirlPoolPokemonName() As String
        For Each p As Pokemon In Core.Player.Pokemons
            If p.IsEgg() = False Then
                For Each a As BattleSystem.Attack In p.Attacks
                    If a.Name.ToLower() = "whirlpool" Then
                        Return p.GetDisplayName()
                    End If
                Next
            End If
        Next
        Return ""
    End Function

    Public Overrides Function WalkAgainstFunction() As Boolean
        If Me.ActionValue = 1 Then
            Return Me.Collision
        End If

        If Screen.Level.Surfing = True Then
            Dim pName As String = ReturnWhirlPoolPokemonName()
            Dim s As String = ""

            If Badge.CanUseHMMove(Badge.HMMoves.Whirlpool) = True And pName <> "" Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                s = "version=2
@text.show(" & pName & " used~Whirlpool!)
@player.move(2)
:end"
                PlayerStatistics.Track("Whirlpool used", 1)
            Else
                s = "version=2
@player.move(1)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.move(1)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@player.turn(1)
@level.wait(3)
@text.show(It's a vicious~whirlpool!*A Pokémon may be~able to pass it.)
:end"
            End If

            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            Return True
        End If

        Return True
    End Function

End Class