Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @pokedex commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoPokedex(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "setautodetect"
                    Pokedex.AutoDetect = ScriptConversion.ToBoolean(argument)
                Case "changeentry"
                    Dim ForceChange As Boolean = False
                    If argument.Split(",").Count > 2 Then
                        ForceChange = CBool(argument.GetSplit(2, ","))
                    End If
                    Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, argument.GetSplit(0, ","), CInt(argument.GetSplit(1, ",")), ForceChange)
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@pokedex." & command & ") Command not found.")
            End Select

            IsReady = True
        End Sub ' Crash Handler

    End Class

End Namespace