Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @music commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoMusic(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "play"
                    MusicManager.Play(argument, True)

                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        Screen.Level.MusicLoop = argument
                    End If
                Case "forceplay"
                    MusicManager.ClearCurrentlyPlaying()
                    MusicManager.Play(argument)
                Case "setmusicloop"
                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        Screen.Level.MusicLoop = argument
                    End If
                Case "stop"
                    MusicManager.Stop()
                Case "mute"
                    MusicManager.Muted() = True
                Case "unmute"
                    MusicManager.Muted() = False
                Case "pause"
                    MusicManager.Paused() = True
                Case "resume"
                    MusicManager.Paused() = False
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace