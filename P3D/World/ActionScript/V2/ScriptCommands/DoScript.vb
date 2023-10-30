Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @script commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoScript(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Select Case command.ToLower()
                    Case "start"
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(argument, 0, True, False, "ScriptCommand")
                    Case "text"
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(argument, 1, True, False, "ScriptCommand")
                    Case "run"
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(argument, 2, True, False, "ScriptCommand")
                    Case "delay"
                        If argument.Contains(",") = True Then
                            ActionScript.UnregisterID("SCRIPTDELAY", "str")
                            ActionScript.UnregisterID("SCRIPTDELAY")
                            Core.Player.ScriptDelaySteps = 0

                            Dim args() As String = argument.Split(CChar(","))
                            Select Case args(1).ToLower
                                Case "steps"
                                    Core.Player.ScriptDelaySteps = CInt(args(2))
                            End Select

                            ActionScript.RegisterID("SCRIPTDELAY", "str", CStr(args(1) & ";" & args(0)))
                        End If
                        IsReady = True
                    Case "cleardelay"
                        ActionScript.UnregisterID("SCRIPTDELAY", "str")
                        ActionScript.UnregisterID("SCRIPTDELAY")
                        Core.Player.ScriptDelaySteps = 0
                        IsReady = True
                End Select
            End If

            IsReady = True
        End Sub

    End Class

End Namespace