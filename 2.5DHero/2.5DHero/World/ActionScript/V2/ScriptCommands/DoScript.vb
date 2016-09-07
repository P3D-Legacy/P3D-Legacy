Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @script commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoScript(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Select Case command.ToLower()
                    Case "start"
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(argument, 0, True, False)
                    Case "text"
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(argument, 1, True, False)
                    Case "run"
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(argument, 2, True, False)
                End Select
            End If

            IsReady = True
        End Sub

    End Class

End Namespace