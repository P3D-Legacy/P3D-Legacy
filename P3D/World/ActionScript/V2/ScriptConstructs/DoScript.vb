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
                    Dim args() As String = argument.Split(CChar(","))

                    If ActionScript.IsRegistered("SCRIPTDELAY_" & args(0)) = True Then

                        Dim registerContent() As Object = ActionScript.GetRegisterValue("SCRIPTDELAY_" & args(0))

                        If registerContent(0) Is Nothing Or registerContent(1) Is Nothing Then
                            Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: No valid script has been set to be executed.")
                            ActionScript.UnregisterID("SCRIPTDELAY_" & args(0), "str")
                            ActionScript.UnregisterID("SCRIPTDELAY_" & args(0))
                            Return DefaultNull
                        End If

                        Select Case args(1).ToLower
                            Case "type"
                                Return CStr(registerContent(0)).GetSplit(0, ";")
                            Case "script"
                                Return CStr(registerContent(0)).GetSplit(1, ";")
                            Case "value"
                                Dim DelayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                                Select Case DelayType
                                    Case "steps"
                                        Return Core.Player.ScriptDelaySteps
                                    Case "itemcount"
                                        Dim ItemDelayList As List(Of String) = Core.Player.ScriptDelayItems.Split(";").ToList
                                        For Each entry As String In ItemDelayList
                                            If entry.GetSplit(0, ",") = args(0) Then
                                                Return entry.GetSplit(3)
                                                Exit For
                                            End If
                                        Next
                                End Select
                        End Select

                    End If
            End Select

            Return DefaultNull
        End Function

    End Class

End Namespace