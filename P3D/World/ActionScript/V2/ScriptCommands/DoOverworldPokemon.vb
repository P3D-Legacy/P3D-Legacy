Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @overworldpokemon commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoOverworldPokemon(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "hide"
                    Screen.Level.OverworldPokemon.Visible = False
                    IsReady = True
                Case "show"
                    Screen.Level.OverworldPokemon.Visible = True
                    IsReady = True
                Case "toggle"
                    Screen.Level.OverworldPokemon.Visible = Not Screen.Level.OverworldPokemon.Visible
                    IsReady = True
                Case Else
                    IsReady = True
            End Select

        End Sub

    End Class

End Namespace