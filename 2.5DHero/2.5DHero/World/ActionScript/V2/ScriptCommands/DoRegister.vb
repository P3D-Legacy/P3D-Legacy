Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @register commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoRegister(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "register"
                    If argument.Contains(",") = True Then
                        Dim args() As String = argument.Split(CChar(","))
                        ActionScript.RegisterID(args(0), args(1), args(2))
                    Else
                        ActionScript.RegisterID(argument)
                    End If
                Case "unregister"
                    If argument.Contains(",") = True Then
                        Dim args() As String = argument.Split(CChar(","))
                        ActionScript.UnregisterID(args(0), args(1))
                    Else
                        ActionScript.UnregisterID(argument)
                    End If
                Case "change"
                    Dim args() As String = argument.Split(CChar(","))
                    ActionScript.ChangeRegister(args(0), args(1))
                Case "registertime"
                    Dim args() As String = argument.Split(CChar(","))
                    '0=register name, 1=value, 2=format

                    Dim format As String = "days"
                    Dim isValidFormat As Boolean = True
                    Select Case args(2).ToLower()
                        Case "day", "days"
                            format = "days"
                        Case "hour", "hours"
                            format = "hours"
                        Case "minute", "minutes"
                            format = "minutes"
                        Case "second", "seconds"
                            format = "seconds"
                        Case "year", "years"
                            format = "years"
                        Case "week", "weeks"
                            format = "weeks"
                        Case "month", "months"
                            format = "months"
                        Case "dayofweek"
                            format = "dayofweek"
                        Case Else
                            isValidFormat = False
                    End Select

                    Dim value As Integer = -1
                    Dim validValue As Boolean = True
                    If Integer.TryParse(args(1), value) = False Then
                        validValue = False
                    Else
                        If value < 0 Then
                            validValue = False
                        End If
                    End If

                    If validValue = True Then
                        If isValidFormat = True Then
                            Dim s As String = "[TIME|" & ActionScript.TimeToUnix(Date.Now) & "|" & args(1) & "|" & format & "]" & args(0)
                            ActionScript.RegisterID(s)
                        Else
                            Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@register." & command & ") Invalid date format used for time based register!")
                        End If
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@register." & command & ") Invalid value used for time based register!")
                    End If
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@register." & command & ") Command not found.")
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace