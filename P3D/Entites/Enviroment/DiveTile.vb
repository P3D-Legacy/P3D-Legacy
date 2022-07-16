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
                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        If CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady Then
                            Me.StartDive()
                        End If
                    End If
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
            Dim t As String = "The boat's shadow is cast~upon the ocean floor.*Do you want to~use Dive?%Yes|No%"

            Screen.TextBox.Show(t, {Me})
            SoundManager.PlaySound("select")
        End If
    End Sub

    Public Overrides Sub ResultFunction(ByVal result As Integer)
        If result = 0 Then
            If diveUp = 0 Then
                'Down
                Dim s As String = "version=2" & Environment.NewLine &
                    "@text.show(" & GetDivePokemon() & "~used Dive!)" & Environment.NewLine &
                    "@screen.fadeout" & Environment.NewLine &
                    "@player.warp(" & Me.AdditionalValue & ")" & Environment.NewLine &
                    "@level.update" & Environment.NewLine &
                    "@player.setmovement(0,-0.5,0)" & Environment.NewLine &
                    "@screen.fadein" & Environment.NewLine &
                    "@player.move(8)" & Environment.NewLine &
                    "@player.resetmovement" & Environment.NewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            ElseIf diveUp = 1 Then
                'Up
                Dim s As String = "version=2" & Environment.NewLine &
                    "@text.show(" & GetDivePokemon() & "~used Dive!)" & Environment.NewLine &
                    "@player.setmovement(0,0.5,0)" & Environment.NewLine &
                    "@player.move(8)" & Environment.NewLine &
                    "@player.resetmovement" & Environment.NewLine &
                    "@screen.fadeout" & Environment.NewLine &
                    "@player.warp(" & Me.AdditionalValue & ")" & Environment.NewLine &
                    "@level.update" & Environment.NewLine &
                    "@screen.fadein" & Environment.NewLine &
                    ":end"

                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
            ElseIf diveUp = 2 Then
                'Up
                Dim s As String = "version=2" & Environment.NewLine &
                    "@text.show(" & GetDivePokemon() & "~used Dive!)" & Environment.NewLine &
                    "@player.setmovement(0,0.5,0)" & Environment.NewLine &
                    "@player.move(6)" & Environment.NewLine &
                    "@player.resetmovement" & Environment.NewLine &
                    "@screen.fadeout" & Environment.NewLine &
                    "@player.warp(" & Me.AdditionalValue & ")" & Environment.NewLine &
                    "@level.update" & Environment.NewLine &
                    "@screen.fadein" & Environment.NewLine &
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
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class