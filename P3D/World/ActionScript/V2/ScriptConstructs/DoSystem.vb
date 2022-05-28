﻿Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <system> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoSystem(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "token"
                    Dim tokenName As String = Localization.GetString(argument)
                    Return tokenName
                Case "button"
                    Dim key As Keys = Nothing
                    Dim buttonName As String = argument
                    Select Case buttonName.ToLower()
                        Case "moveforward"
                            key = KeyBindings.ForwardMoveKey
                        Case "moveleft"
                            key = KeyBindings.LeftMoveKey
                        Case "movebackward"
                            key = KeyBindings.BackwardMoveKey
                        Case "moveright"
                            key = KeyBindings.RightMoveKey
                        Case "openmenu"
                            key = KeyBindings.OpenInventoryKey
                        Case "chat"
                            key = KeyBindings.ChatKey
                        Case "special", "phone"
                            key = KeyBindings.SpecialKey
                        Case "muteaudio"
                            key = KeyBindings.MuteAudioKey
                        Case "cameraleft"
                            key = KeyBindings.LeftKey
                        Case "cameraright"
                            key = KeyBindings.RightKey
                        Case "cameraup"
                            key = KeyBindings.UpKey
                        Case "cameradown"
                            key = KeyBindings.DownKey
                        Case "cameralock"
                            key = KeyBindings.CameraLockKey
                        Case "guicontrol"
                            key = KeyBindings.GUIControlKey
                        Case "screenshot"
                            key = KeyBindings.ScreenshotKey
                        Case "debugcontrol"
                            key = KeyBindings.DebugKey
                        Case "perspectiveswitch"
                            key = KeyBindings.PerspectiveSwitchKey
                        Case "fullscreen"
                            key = KeyBindings.FullScreenKey
                        Case "enter1"
                            key = KeyBindings.EnterKey1
                        Case "enter2"
                            key = KeyBindings.EnterKey2
                        Case "back1"
                            key = KeyBindings.BackKey1
                        Case "back2"
                            key = KeyBindings.BackKey2
                        Case "escape", "esc"
                            key = KeyBindings.EscapeKey
                        Case "onlinestatus"
                            key = KeyBindings.OnlineStatusKey
                        Case "lighting"
                            key = KeyBindings.LightKey
                        Case "hidegui"
                            key = KeyBindings.GUIControlKey
                    End Select
                    If key <> Nothing Then
                        buttonName = KeyBindings.GetKeyName(key)
                    End If
                    Return buttonName
                Case "random"
                    Dim minRange As Integer = 1
                    Dim maxRange As Integer = 2
                    If argument <> "" Then
                        If argument.Contains(",") = True Then
                            minRange = int(argument.GetSplit(0))
                            maxRange = int(argument.GetSplit(1))
                        Else
                            If StringHelper.IsNumeric(argument) = True Then
                                maxRange = int(argument)
                            End If
                        End If
                    End If
                    Return Core.Random.Next(minRange, maxRange + 1)
                Case "unixtimestamp"
                    Return (DateTime.Now - New DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds
                Case "dayofyear"
                    Return My.Computer.Clock.LocalTime.DayOfYear
                Case "year"
                    Return My.Computer.Clock.LocalTime.Year
                Case "booltoint"
                    If argument.ToLower() = "false" Then
                        Return "0"
                    ElseIf argument.ToLower() = "true" Then
                        Return "1"
                    End If
                    Return "0"
                Case "calcint", "int"
                    Return int(argument)
                Case "calcsng", "sng"
                    Return dbl(argument)
                Case "sort"
                    Dim args() As String = argument.Split(CChar(","))

                    Dim sortMode As String = args(0)
                    Dim returnIndex As Integer = CInt(args(1))
                    Dim sortList As New List(Of String)
                    For i = 2 To args.Count() - 1
                        sortList.Add(args(i))
                    Next

                    If sortMode.ToLower() = "ascending" Then
                        Dim sortedList As List(Of String) = (From i In sortList Order By i.ToString() Ascending).ToList()

                        Return sortedList(returnIndex)
                    ElseIf sortMode.ToLower() = "descending" Then
                        Dim sortedList As List(Of String) = (From i In sortList Order By i.ToString() Descending).ToList()

                        Return sortedList(returnIndex)
                    End If

                    Return DefaultNull
                Case "isinsightscript"
                    Return ReturnBoolean(ActionScript.IsInsightScript)
                Case "lastinput"
                    Return InputScreen.LastInput
                Case "return"
                    Return ScriptV2.TempReturn
                Case "isint"
                    Return ReturnBoolean(ScriptConversion.IsArithmeticExpression(argument))
                Case "issng"
                    Return ReturnBoolean(ScriptConversion.IsArithmeticExpression(argument))
                Case "chrw"
                    Dim chars() As String = argument.Split(CChar(","))
                    Dim output As String = ""
                    For Each c As String In chars
                        If StringHelper.IsNumeric(c) = True Then
                            output &= StringHelper.GetChar(CInt(c))
                        End If
                    Next
                    Return output
                Case "scriptlevel"
                    Return ActionScript.ScriptLevelIndex.ToString()
            End Select

            Return DefaultNull
        End Function

    End Class

End Namespace