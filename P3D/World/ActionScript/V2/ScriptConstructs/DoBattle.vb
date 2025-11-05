Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <battle> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoBattle(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "defeatmessage"
                    Dim t As New Trainer(argument)

                    Return t.DefeatMessage
                Case "intromessage"
                    Dim t As New Trainer(argument)

                    Return t.IntroMessage
                Case "outromessage"
                    Dim t As New Trainer(argument)

                    Return t.OutroMessage
                Case "won"
                    Return ReturnBoolean(BattleSystem.Battle.Won)
                Case "caught"
                    Return ReturnBoolean(BattleSystem.Battle.Caught)
                Case "trainername"
                    Dim index As Integer = 0
                    If argument <> "" Then
                        index = CInt(argument)
                    End If
                    If CurrentScreen.Identification = Screen.Identifications.BattleScreen Then
                        Select Case index
                            Case 0
                                Return CType(CurrentScreen, BattleSystem.BattleScreen).Trainer.Name
                            Case 1
                                Return CType(CurrentScreen, BattleSystem.BattleScreen).Trainer.Name2
                            Case Else
                                Return CType(CurrentScreen, BattleSystem.BattleScreen).Trainer.Name
                        End Select
                    End If
                Case "pokemonname"
                    Dim own As Boolean = True
                    If argument <> "" Then
                        own = CBool(argument)
                    End If
                    If CurrentScreen.Identification = Screen.Identifications.BattleScreen Then
                        Select Case own
                            Case True
                                Return CType(CurrentScreen, BattleSystem.BattleScreen).OwnPokemon.GetDisplayName()
                            Case False
                                Return CType(CurrentScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName()
                        End Select
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace