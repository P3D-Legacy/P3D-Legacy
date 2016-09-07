Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <register> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoRegister(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "registered"
                    Return ReturnBoolean(ActionScript.IsRegistered(argument))
                Case "count"
                    Return Core.Player.RegisterData.CountSplits(",") + 1
                Case "type"
                    Dim name As String = argument
                    Dim registerContent() As Object = ActionScript.GetRegisterValue(name)

                    If registerContent(0) Is Nothing Or registerContent(1) Is Nothing Then
                        Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: (<register." & command & ">) The requested register with the name """ & name & """ doesn't exist or is not a Value Register.")
                        Return DEFAULTNULL
                    End If

                    Dim lType As String = CStr(registerContent(1))

                    Return lType
                Case "value"
                    Dim name As String = argument
                    Dim registerContent() As Object = ActionScript.GetRegisterValue(name)

                    If registerContent(0) Is Nothing Or registerContent(1) Is Nothing Then
                        Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: (<register." & command & ">) The requested register with the name """ & name & """ doesn't exist or is not a Value Register.")
                        Return DEFAULTNULL
                    End If

                    Dim lType As String = CStr(registerContent(1))
                    Dim lValue As String = CStr(registerContent(0))

                    Select Case lType.ToLower()
                        Case "bool"
                            Return ReturnBoolean(CBool(lValue))
                        Case "sng"
                            Return dbl(lValue)
                        Case "int"
                            Return int(lValue)
                        Case "str"
                            Return lValue
                        Case Else
                            Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: (<register." & command & ">) The value passed in for ""lType"" is not valid (" & lType & "). Assuming str.")
                            Return lValue
                    End Select
            End Select
            Return DEFAULTNULL
        End Function

    End Class

End Namespace