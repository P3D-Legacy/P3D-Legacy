Public Class Whirlpool

    Inherits Entity

    Public Shared LoadedWaterTemp As Boolean = False
    Public Shared WaterTexturesTemp As New List(Of Texture2D)

    Dim WaterAnimation As Animation
    Dim currentRectangle As New Rectangle(0, 0, 0, 0)

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        WaterAnimation = New Animation(net.Pokemon3D.Game.TextureManager.GetTexture("Textures\Routes"), 1, 4, 16, 16, 9, 12, 0)

        If Whirlpool.LoadedWaterTemp = False Then

        End If
    End Sub

    Public Shared Sub CreateWaterTextureTemp()
        If Core.GameOptions.GraphicStyle = 1 Then
            Whirlpool.WaterTexturesTemp.Clear()

            Whirlpool.WaterTexturesTemp.Add(net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(0, 176, 16, 16)))
            Whirlpool.WaterTexturesTemp.Add(net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(16, 176, 16, 16)))
            Whirlpool.WaterTexturesTemp.Add(net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(32, 176, 16, 16)))
            Whirlpool.WaterTexturesTemp.Add(net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(48, 176, 16, 16)))
            Whirlpool.LoadedWaterTemp = True
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
            If Whirlpool.LoadedWaterTemp = False Then
                Whirlpool.CreateWaterTextureTemp()
            End If
            Select Case WaterAnimation.CurrentColumn
                Case 0
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(0)
                Case 1
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(1)
                Case 2
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(2)
                Case 3
                    Me.Textures(0) = Whirlpool.WaterTexturesTemp(3)
            End Select
        End If
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
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