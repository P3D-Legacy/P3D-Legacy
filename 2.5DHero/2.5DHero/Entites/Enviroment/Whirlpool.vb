Public Class Whirlpool

    Inherits Entity

    Public Shared LoadedWaterTemp As Boolean = False
    Public Shared WaterTexturesTemp As New List(Of Texture2D)

    Dim WaterAnimation As Animation
    Dim currentRectangle As New Rectangle(0, 0, 0, 0)

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        WaterAnimation = New Animation(TextureManager.GetTexture("Textures\Routes"), 1, 4, 16, 16, 9, 12, 0)

        If LoadedWaterTemp = False Then

        End If
    End Sub

    Public Shared Sub CreateWaterTextureTemp()
        If Core.GameOptions.GraphicStyle = 1 Then
            WaterTexturesTemp.Clear()

            WaterTexturesTemp.Add(TextureManager.GetTexture("Routes", New Rectangle(0, 176, 16, 16)))
            WaterTexturesTemp.Add(TextureManager.GetTexture("Routes", New Rectangle(16, 176, 16, 16)))
            WaterTexturesTemp.Add(TextureManager.GetTexture("Routes", New Rectangle(32, 176, 16, 16)))
            WaterTexturesTemp.Add(TextureManager.GetTexture("Routes", New Rectangle(48, 176, 16, 16)))
            LoadedWaterTemp = True
        End If
    End Sub

    Public Overrides Sub UpdateEntity()
        If Not WaterAnimation Is Nothing Then
            WaterAnimation.Update(0.01)
            If currentRectangle <> WaterAnimation.TextureRectangle Then
                ChangeTexture()

                currentRectangle = WaterAnimation.TextureRectangle
            End If
        End If

        MyBase.UpdateEntity()
    End Sub

    Private Sub ChangeTexture()
        If Core.GameOptions.GraphicStyle = 1 Then
            If LoadedWaterTemp = False Then
                CreateWaterTextureTemp()
            End If
            Select Case WaterAnimation.CurrentColumn
                Case 0
                    Textures(0) = WaterTexturesTemp(0)
                Case 1
                    Textures(0) = WaterTexturesTemp(1)
                Case 2
                    Textures(0) = WaterTexturesTemp(2)
                Case 3
                    Textures(0) = WaterTexturesTemp(3)
            End Select
        End If
    End Sub

    Public Overrides Sub Render()
        Draw(Model, Textures, False)
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
        If ActionValue = 1 Then
            Return Collision
        End If

        If Screen.Level.Surfing = True Then
            Dim pName As String = ReturnWhirlPoolPokemonName()
            Dim s As String = ""

            If Badge.CanUseHMMove(Badge.HMMoves.Whirlpool) = True And pName <> "" Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                s = "@text.show(" & pName & " used~Whirlpool!)
@player.move(2)"
                PlayerStatistics.Track("Whirlpool used", 1)
            Else
                s = "@player.move(1)
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
@text.show(It's a vicious~whirlpool!*A Pokémon may be~able to pass it.)"
            End If

            Construct.Controller.GetInstance().RunFromString(s, {Construct.Controller.ScriptRunOptions.CheckDelay})
            Return True
        End If

        Return True
    End Function

End Class