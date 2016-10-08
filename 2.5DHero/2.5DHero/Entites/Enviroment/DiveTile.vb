Public Class DiveTile

    Inherits Entity

    Dim diveUp As Integer = 0

    Public Overrides Sub Initialize()
        MyBase.Initialize()
        Me.diveUp = Me.ActionValue
        Me.NeedsUpdate = True
    End Sub

    Public Overrides Sub Update()
        If Screen.Level.Surfing = True Then
            If CInt(Me.Position.X) = CInt(Screen.Camera.Position.X) And CInt(Me.Position.Y) = CInt(Screen.Camera.Position.Y) And CInt(Me.Position.Z) = CInt(Screen.Camera.Position.Z) Then
                If Controls.Accept(True, True, True) = True Then
                    Me.StartDive()
                End If
            End If
        End If
    End Sub

    Private Sub StartDive()
        If diveUp = 0 Then
            'Down
            Dim t As String = "The water seems to be~really deep at this point."
            Dim d As String = GetDivePokemon()

            If d <> "" And Badge.CanUseHMMove(Badge.HMMoves.Dive) = True Or Core.Player.SandBoxMode = True Or GameController.IS_DEBUG_ACTIVE = True Then
                t &= "*Do you want to~use Dive?%Yes|No%"
            End If
            Screen.TextBox.Show(t, {Me})
            SoundManager.PlaySound("select")
        ElseIf diveUp = 1 Then
            'Up
            Dim t As String = "Light shines down from~the surface.*Do you want to~use Dive?%Yes|No%"

            Screen.TextBox.Show(t, {Me})
            SoundManager.PlaySound("select")
        ElseIf diveUp = 2 Then
            'Up
            Dim t As String = "The boat's shadow is casted~upon the ocean floor.*Do you want to~use Dive?%Yes|No%"

            Screen.TextBox.Show(t, {Me})
            SoundManager.PlaySound("select")
        End If
    End Sub

    Public Overrides Sub ResultFunction(ByVal result As Integer)
        If result = 0 Then
            If diveUp = 0 Then
                'Down
                Dim s As String = "version=2" & vbNewLine &
                    "@text.show(" & GetDivePokemon() & "~used Dive!)" & vbNewLine &
                    "@screen.fadeout" & vbNewLine &
                    "@player.warp(" & Me.AdditionalValue & ")" & vbNewLine &
                    "@level.update" & vbNewLine &
                    "@player.setmovement(0,-0.5,0)" & vbNewLine &
                    "@screen.fadein" & vbNewLine &
                    "@player.move(8)" & vbNewLine &
                    "@player.resetmovement" & vbNewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            ElseIf diveUp = 1 Then
                'Up
                Dim s As String = "version=2" & vbNewLine &
                    "@text.show(" & GetDivePokemon() & "~used Dive!)" & vbNewLine &
                    "@player.setmovement(0,0.5,0)" & vbNewLine &
                    "@player.move(8)" & vbNewLine &
                    "@player.resetmovement" & vbNewLine &
                    "@screen.fadeout" & vbNewLine &
                    "@player.warp(" & Me.AdditionalValue & ")" & vbNewLine &
                    "@level.update" & vbNewLine &
                    "@screen.fadein" & vbNewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            ElseIf diveUp = 2 Then
                'Up
                Dim s As String = "version=2" & vbNewLine &
                    "@text.show(" & GetDivePokemon() & "~used Dive!)" & vbNewLine &
                    "@player.setmovement(0,0.5,0)" & vbNewLine &
                    "@player.move(6)" & vbNewLine &
                    "@player.resetmovement" & vbNewLine &
                    "@screen.fadeout" & vbNewLine &
                    "@player.warp(" & Me.AdditionalValue & ")" & vbNewLine &
                    "@level.update" & vbNewLine &
                    "@screen.fadein" & vbNewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            End If
        End If
    End Sub

    Private Function GetDivePokemon() As String
        For Each p As Pokemon In Core.Player.Pokemons
            For Each a As BattleSystem.Attack In p.Attacks
                If a.Name.ToLower() = "dive" Then
                    Return p.GetDisplayName()
                End If
            Next
        Next
        Return ""
    End Function

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class