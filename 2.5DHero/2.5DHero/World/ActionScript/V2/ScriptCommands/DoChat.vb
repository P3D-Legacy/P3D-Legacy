Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @chat commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoChat(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "clear"
                    Chat.ClearChat()
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@chat." & command & ") Command not found.")
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace