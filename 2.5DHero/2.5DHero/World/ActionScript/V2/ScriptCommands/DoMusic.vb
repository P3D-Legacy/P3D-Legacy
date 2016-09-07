Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @music commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoMusic(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "play"
                    MusicManager.PlayMusic(argument, True)

                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        Screen.Level.MusicLoop = argument
                    End If
                Case "setmusicloop"
                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        Screen.Level.MusicLoop = argument
                    End If
                Case "stop"
                    MusicManager.StopMusic()
                Case "pause"
                    MusicManager.Pause()
                Case "resume"
                    MusicManager.ResumeMusic()
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace