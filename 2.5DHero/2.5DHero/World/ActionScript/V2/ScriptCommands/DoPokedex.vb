Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @pokedex commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoPokedex(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "setautodetect"
                    Pokedex.AutoDetect = ScriptConversion.ToBoolean(argument)
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@pokedex." & command & ") Command not found.")
            End Select

            IsReady = True
        End Sub 'crash handle

    End Class

End Namespace