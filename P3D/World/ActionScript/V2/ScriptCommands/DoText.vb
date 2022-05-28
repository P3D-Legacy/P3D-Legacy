﻿Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @text commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoText(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "notification"
                    '@text.notification(message,[int_delay=500],[int_backgroundID=0],[int_IconID=0],[str_sfx],[str_script],[bool_force=0])
                    Dim _NotificationPopup As New NotificationPopup
                    Dim args As String() = argument.Split(CChar(","))
                    Select Case args.Length
                        Case 1
                            _NotificationPopup.Setup(argument)
                        Case 2
                            _NotificationPopup.Setup(args(0), int(args(1)))
                        Case 3
                            _NotificationPopup.Setup(args(0), int(args(1)), int(args(2)))
                        Case 4
                            _NotificationPopup.Setup(args(0), int(args(1)), int(args(2)), int(args(3)))
                        Case 5
                            _NotificationPopup.Setup(args(0), int(args(1)), int(args(2)), int(args(3)), args(4))
                        Case 6, 7
                            _NotificationPopup.Setup(args(0), int(args(1)), int(args(2)), int(args(3)), args(4), args(5))
                    End Select
                    If args.Length = 7 AndAlso CBool(args(6)) = True Then
                        CType(CurrentScreen, OverworldScreen).NotificationPopupList.Insert(0, _NotificationPopup)
                    Else
                        CType(CurrentScreen, OverworldScreen).NotificationPopupList.Add(_NotificationPopup)
                    End If
                Case "show"
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(argument, {}, False, False)

                    CanContinue = False
                Case "setfont"
                    Dim f As FontContainer = FontManager.GetFontContainer(argument)
                    If Not f Is Nothing Then
                        Screen.TextBox.TextFont = f
                    Else
                        Screen.TextBox.TextFont = FontManager.GetFontContainer("textfont")
                    End If
                Case "debug"
                    Logger.Debug("DEBUG: " & argument.ToString())
                Case "log"
                    Logger.Log(Logger.LogTypes.Debug, argument.ToString())
                Case "color"
                    Dim args As String() = argument.Split(CChar(","))

                    If args.Length = 1 Then
                        Select Case args(0).ToLower()
                            Case "playercolor", "player"
                                Screen.TextBox.TextColor = TextBox.PlayerColor
                            Case "defaultcolor", "default"
                                Screen.TextBox.TextColor = TextBox.DefaultColor
                            Case Else ' Try to convert the input color name into a color: (https://msdn.microsoft.com/en-us/library/system.drawing.knowncolor%28v=vs.110%29.aspx)
                                Screen.TextBox.TextColor = Drawing.Color.FromName(args(0)).ToXNA()
                        End Select
                    ElseIf args.Length = 3 Then
                        Screen.TextBox.TextColor = New Color(int(args(0)), int(args(1)), int(args(2)))
                    End If
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace