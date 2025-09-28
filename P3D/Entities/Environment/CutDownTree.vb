Public Class CutDownTree

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

        Dim text As String = Localization.GetString("fieldmove_cut_1", "This tree looks like it~can be Cut down!")

        If pName <> "" And Badge.CanUseHMMove(Badge.HMMoves.Cut) = True Or Core.Player.SandBoxMode = True Or GameController.IS_DEBUG_ACTIVE = True Then
            text &= Localization.GetString("fieldmove_cut_2", "*Do you want to use Cut?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
        End If

        Screen.TextBox.Show(text, {Me})
        SoundManager.PlaySound("select")
    End Sub

    Public Overrides Sub ResultFunction(Result As Integer)
        If Result = 0 Then
            Dim pName As String = "MissingNo."

            For Each p As Pokemon In Core.Player.Pokemons
                If p.IsEgg() = False Then
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name = "Cut" Then
                            pName = p.GetDisplayName()
                            Exit For
                        End If
                    Next
                End If

                If pName <> "MissingNo." Then
                    Exit For
                End If
            Next

            Dim Text As String = pName & " " & Localization.GetString("fieldmove_cut_used", "used~Cut!")
            Me.CanBeRemoved = True

            Dim s As String =
                "version=2" & Environment.NewLine &
                "@text.show(" & Text & ")" & Environment.NewLine &
                "@sound.play(FieldMove_Cut,0)" & Environment.NewLine &
                ":end"

            PlayerStatistics.Track("Cut used", 1)
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
        End If
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class