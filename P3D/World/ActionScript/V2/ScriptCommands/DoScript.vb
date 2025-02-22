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
                            'arg 0 = Identifier
                            'arg 1 = Script Path
                            'arg 2 = Delay Type
                            '    //Steps
                            'arg 3 = Amount of steps
                            'arg 4 = Whether to display the remaining steps in the start menu
                            '    //ItemCount
                            'arg 3 = Item ID
                            'arg 4 = Compare Type
                            'arg 5 = Item Amount

                            Dim args() As String = argument.Split(CChar(","))
                            If ActionScript.IsRegistered("SCRIPTDELAY_" & args(0).ToLower) = True Then
                                ActionScript.UnregisterID("SCRIPTDELAY_" & args(0), "str")
                                ActionScript.UnregisterID("SCRIPTDELAY_" & args(0))
                            End If

                            If Core.Player.ScriptDelaySteps > 0 AndAlso args(2).ToLower = "steps" Then
                                Dim Data() As String = Core.Player.RegisterData.Split(CChar(","))

                                For Each line As String In Data
                                    If line.StartsWith("[") = True And line.Contains("]") = True And line.EndsWith("]") = False Then
                                        Dim lineName As String = line.Remove(0, line.IndexOf("]") + 1)
                                        Dim delayID As String = ""
                                        If lineName.StartsWith("SCRIPTDELAY_") Then
                                            Dim registerContent() As Object = ActionScript.GetRegisterValue(lineName)
                                            Dim delayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                                            If delayType.ToLower = "steps" Then
                                                ActionScript.UnregisterID(lineName, "str")
                                                ActionScript.UnregisterID(lineName)
                                                Core.Player.ScriptDelaySteps = 0
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                            Select Case args(2).ToLower
                                Case "steps"
                                    Core.Player.ScriptDelaySteps = CInt(args(3))
                                    Dim DisplaySteps As Boolean = False
                                    If args.Count > 4 Then
                                        DisplaySteps = CBool(args(4))
                                    End If
                                    Core.Player.ScriptDelayDisplaySteps = DisplaySteps
                                Case "itemcount"
                                    Dim CompareType As String = ""
                                    Select Case args(4).ToLower
                                        Case "equal"
                                            CompareType = "equal"
                                        Case "below"
                                            CompareType = "below"
                                        Case "equalorbelow"
                                            CompareType = "equalorbelow"
                                        Case "above"
                                            CompareType = "above"
                                        Case "equalorabove"
                                            CompareType = "equalorabove"
                                    End Select
                                    If args(3) <> "" And CompareType <> "" And StringHelper.IsNumeric(args(5)) Then
                                        If Core.Player.ScriptDelayItems.Contains(args(0) & "," & args(3) & "," & CompareType & "," & CInt(args(5))) = False Then
                                            If Core.Player.ScriptDelayItems <> "" Then
                                                Core.Player.ScriptDelayItems &= ";"
                                            End If
                                            '0=delayIdentifier,1=itemID,2=compareType,3=itemAmount
                                            Core.Player.ScriptDelayItems &= args(0) & "," & args(3) & "," & CompareType & "," & CInt(args(5))
                                        End If
                                    End If
                            End Select

                            ActionScript.RegisterID("SCRIPTDELAY_" & args(0), "str", CStr(args(2).ToLower & ";" & args(1)))

                        End If
                        IsReady = True
                    Case "cleardelay"
                        If argument <> "" Then
                            Dim registerContent() As Object = ActionScript.GetRegisterValue("SCRIPTDELAY_" & argument)
                            If registerContent(0) IsNot Nothing Then
                                Dim delayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                                Select Case delayType.ToLower
                                    Case "steps"
                                        Core.Player.ScriptDelaySteps = 0
                                        Core.Player.ScriptDelayDisplaySteps = False
                                    Case "itemcount"
                                        Dim ItemDelayList As List(Of String) = Core.Player.ScriptDelayItems.Split(",").ToList
                                        For Each entry As String In ItemDelayList
                                            If entry.GetSplit(0, ",") = argument Then
                                                ItemDelayList.Remove(entry)
                                                Exit For
                                            End If
                                        Next
                                        Core.Player.ScriptDelayItems = String.Join(";", ItemDelayList)
                                End Select
                            End If
                            ActionScript.UnregisterID("SCRIPTDELAY_" & argument, "str")
                            ActionScript.UnregisterID("SCRIPTDELAY_" & argument)
                        End If

                        IsReady = True
                End Select
            End If

            IsReady = True
        End Sub

    End Class

End Namespace