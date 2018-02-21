Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @radio commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoRadio(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            argument = argument.Replace(".", GameController.DecSeparator)

            Select Case command.ToLower()
                Case "allowchannel"
                    Screen.Level.AllowedRadioChannels.Add(CDec(dbl(argument)))
                Case "blockchannel"
                    Dim d As Decimal = CDec(dbl(argument))
                    If Screen.Level.AllowedRadioChannels.Contains(d) = True Then
                        Screen.Level.AllowedRadioChannels.Remove(d)
                    End If
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@radio." & command & ") Command not found.")
            End Select

            IsReady = True
        End Sub ' Crash Handler

    End Class

End Namespace