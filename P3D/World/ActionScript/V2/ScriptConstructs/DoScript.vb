Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <script> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoScript(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "delay"
                    If ActionScript.IsRegistered("SCRIPTDELAY") = True Then

                        Dim registerContent() As Object = ActionScript.GetRegisterValue("SCRIPTDELAY")

                        If registerContent(0) Is Nothing Or registerContent(1) Is Nothing Then
                            Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: No valid script has been set to be executed.")
                            ActionScript.UnregisterID("SCRIPTDELAY", "str")
                            ActionScript.UnregisterID("SCRIPTDELAY")
                            Return DefaultNull
                        End If

                        Select Case argument.ToLower
                            Case "type"
                                Return CStr(registerContent(0)).GetSplit(0, ";")
                            Case "script"
                                Return CStr(registerContent(0)).GetSplit(1, ";")
                            Case "value"
                                Dim DelayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                                Select Case DelayType
                                    Case "steps"
                                        Return Core.Player.ScriptDelaySteps
                                End Select
                        End Select

                    End If
            End Select

            Return DefaultNull
        End Function

    End Class

End Namespace