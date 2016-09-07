Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @title commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoTitle(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Select Case command.ToLower()
                    Case "add"
                        Dim t As New OverworldScreen.Title()

                        Dim args As List(Of String) = Script.ParseArguments(argument)
                        For i = 0 To args.Count - 1
                            Dim arg As String = args(i)
                            Select Case i
                                Case 0 'text
                                    t.Text = arg
                                Case 1 'delay
                                    t.Delay = sng(arg)
                                Case 2 'R
                                    t.TextColor = New Color(CByte(int(arg).Clamp(0, 255)), t.TextColor.G, t.TextColor.B)
                                Case 3 'G
                                    t.TextColor = New Color(t.TextColor.R, CByte(int(arg).Clamp(0, 255)), t.TextColor.B)
                                Case 4 'B
                                    t.TextColor = New Color(t.TextColor.R, t.TextColor.G, CByte(int(arg).Clamp(0, 255)))
                                Case 5 'Scale
                                    t.Scale = sng(arg)
                                Case 6 'IsCentered
                                    t.IsCentered = CBool(arg)
                                Case 7 'X
                                    t.Position = New Vector2(sng(arg), t.Position.Y)
                                Case 8 'Y
                                    t.Position = New Vector2(t.Position.X, sng(arg))
                            End Select
                        Next

                        CType(Core.CurrentScreen, OverworldScreen).Titles.Add(t)
                    Case "clear"
                        CType(Core.CurrentScreen, OverworldScreen).Titles.Clear()
                End Select
            End If

            IsReady = True
        End Sub

    End Class

End Namespace