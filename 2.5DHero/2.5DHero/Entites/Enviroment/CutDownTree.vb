﻿Public Class CutDownTree

    Inherits Entity

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
            Me.CreatedWorld = False
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub ClickFunction()
        Dim pName As String = ""

        For Each p As Pokemon In Core.Player.Pokemons
            If p.IsEgg() = False Then
                For Each a As BattleSystem.Attack In p.Attacks
                    If a.Name = "Cut" Then
                        pName = p.GetDisplayName()
                        Exit For
                    End If
                Next
            End If

            If pName <> "" Then
                Exit For
            End If
        Next

        Dim text As String = "This tree looks like it~can be Cut down!"

        If pName <> "" And Badge.CanUseHMMove(Badge.HMMoves.Cut) = True Or Core.Player.SandBoxMode = True Or GameController.IS_DEBUG_ACTIVE = True Then
            text &= "~Do you want to~use Cut?%Yes|No%"
        End If

        Screen.TextBox.Show(text, {Me})
        SoundManager.PlaySound("select")
    End Sub

    Public Overrides Sub ResultFunction(Result As Integer)
        If Result = 0 Then
            Dim pName As String = ""

            For Each p As Pokemon In Core.Player.Pokemons
                If p.IsEgg() = False Then
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name = "Cut" Then
                            pName = p.GetDisplayName()
                            Exit For
                        End If
                    Next
                End If

                If pName <> "" Then
                    Exit For
                End If
            Next

            Dim Text As String = pName & " used~Cut!"
            Me.CanBeRemoved = True

            Dim s As String =
                "version=2" & Environment.NewLine &
                "@text.show(" & Text & ")" & Environment.NewLine &
                "@sound.play(destroy,0)" & Environment.NewLine &
                ":end"

            PlayerStatistics.Track("Cut used", 1)
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
        End If
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class