Namespace Items.Standard

    <Item(19, "Escape Rope")>
    Public Class EscapeRope

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 550
        Public Overrides ReadOnly Property Description As String = "A long and durable rope. Use it to escape instantly from a cave or a dungeon."
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(408, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If Screen.Level.CanDig = True Then

                While Core.CurrentScreen.Identification <> Screen.Identifications.OverworldScreen
                    Core.SetScreen(Core.CurrentScreen.PreScreen)
                End While

                Dim t As String = Core.Player.Name & " used~an Escape Rope!" & RemoveItem()

                Dim setToFirstPerson As Boolean = Not CType(Screen.Camera, OverworldCamera).ThirdPerson

                Dim yFinish As String = (Screen.Camera.Position.Y + 2.9F).ToString().ReplaceDecSeparator()

                Dim s As String = "version=2" & Environment.NewLine &
                    "@text.show(" & t & ")
@level.wait(20)
@camera.activatethirdperson
@camera.reset
@camera.fix
@player.turnto(0)
@sound.play(teleport)
:while:<player.position(y)><" & yFinish & "
@player.turn(1)
@player.warp(~,~+0.1,~)
@level.wait(1)
:endwhile
@screen.fadeout
@camera.defix
@player.warp(" & Core.Player.LastRestPlace & "," & Core.Player.LastRestPlacePosition & ",0)
@player.turnto(2)"

                If setToFirstPerson = True Then
                    s &= Environment.NewLine & "@camera.deactivatethirdperson"
                End If
                s &= Environment.NewLine &
"@level.update
@screen.fadein
:end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
                If Screen.Level.Surfing = True Then
                    Screen.Level.Surfing = False
                    Screen.Level.OwnPlayer.SetTexture(Core.Player.TempSurfSkin, True)
                    Core.Player.Skin = Core.Player.TempSurfSkin

                    Screen.Level.OverworldPokemon.warped = True
                    Screen.Level.OverworldPokemon.Visible = False
                End If
            Else
                Screen.TextBox.Show("Cannot use the Escape~Rope here!", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
