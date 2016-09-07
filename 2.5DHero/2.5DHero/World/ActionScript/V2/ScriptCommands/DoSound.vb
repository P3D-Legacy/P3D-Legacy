Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @sound commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoSound(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "play"
                    Dim sound As String = argument
                    Dim stopMusic As Boolean = False

                    If argument.Contains(",") = True Then
                        sound = argument.GetSplit(0)
                        stopMusic = CBool(argument.GetSplit(1))
                    End If

                    SoundManager.PlaySound(sound, stopMusic)
                Case "playadvanced"
                    Dim args() As String = argument.Split(CChar(","))

                    Dim sound As String = args(0)
                    Dim stopMusic As Boolean = CBool(args(1))
                    Dim pitch As Single = sng(args(2))
                    Dim pan As Single = sng(args(3))
                    Dim volume As Single = sng(args(4))

                    SoundManager.PlaySound(sound, pitch, pan, volume, stopMusic)
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace