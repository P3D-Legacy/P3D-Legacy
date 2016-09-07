Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @options commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoOptions(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "show"
                    If Not Screen.TextBox Is Nothing And Not Screen.TextBox.Text Is Nothing Then
                        If Screen.TextBox.Text.Length > 0 Then
                            Screen.TextBox.Showing = True
                        End If
                    End If

                    Dim Options() As String = argument.Split(CChar(","))

                    For i = 0 To Options.Count - 1
                        If i <= Options.Count - 1 Then
                            Dim flag = Options(i)
                            Dim removeFlag As Boolean = False

                            Select Case flag
                                Case "[TEXT=FALSE]"
                                    removeFlag = True
                                    Screen.TextBox.Showing = False
                            End Select

                            If removeFlag = True Then
                                Dim l As List(Of String) = Options.ToList()
                                l.RemoveAt(i)
                                Options = l.ToArray()
                                i -= 1
                            End If
                        End If
                    Next
                    ActionScript.CSL().WhenIndex += 1
                    Screen.ChooseBox.Show(Options, 0, True)

                    CanContinue = False
                Case "setcancelindex"
                    ChooseBox.CancelIndex = int(argument)
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace