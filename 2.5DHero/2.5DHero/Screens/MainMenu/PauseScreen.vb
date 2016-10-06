Public Class PauseScreen

    Inherits Screen

    Dim mainIndex As Integer = 0
    Dim quitIndex As Integer = 0
    Dim mainTexture As Texture2D
    Dim leftEscapeKey As Boolean = False
    Dim menuIndex As Integer = 0

    Dim canCreateAutosave As Boolean = True

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.PauseScreen
        Me.CanBePaused = False
        Me.MouseVisible = True
        Me.CanChat = False

        If Not currentScreen Is Nothing Then
            Me.PreScreen = currentScreen
        End If

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        If Core.Player.IsGamejoltSave = True Then
            Me.canCreateAutosave = False
        Else
            If Not Me.PreScreen Is Nothing Then
                If Camera.Name <> "Overworld" Then
                    Me.canCreateAutosave = False
                Else
                    Dim s As Screen = Me.PreScreen
                    While s.Identification <> Identifications.OverworldScreen And Not s.PreScreen Is Nothing
                        s = s.PreScreen
                    End While
                    If s.Identification = Identifications.OverworldScreen Then
                        If CType(s, OverworldScreen).ActionScript.IsReady = False Then
                            Me.canCreateAutosave = False
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.ScreenSize.Width, Core.ScreenSize.Height), New Color(0, 0, 0, 150))
        Dim pX As Integer = CInt(Core.ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.GetString("pause_menu_title")).X / 2)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.GetString("pause_menu_title"), New Vector2(pX - 7, CInt(Core.ScreenSize.Height / 6.8) + 3), Color.Black)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.GetString("pause_menu_title"), New Vector2(pX - 10, CInt(Core.ScreenSize.Height / 6.8)), Color.White)

        If Me.menuIndex = 0 Then
            DrawMenu()
        Else
            DrawQuit()
        End If

        If Me.canCreateAutosave = False Then
            Dim text As String = Localization.GetString("pause_menu_autosave_fail")

            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, text, New Vector2(9, Core.ScreenSize.Height - FontManager.InGameFont.MeasureString(text).Y), Color.Black)
            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, text, New Vector2(5, Core.ScreenSize.Height - FontManager.InGameFont.MeasureString(text).Y - 4), Color.White)
        End If

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, "Accept")
        d.Add(Buttons.B, "Go back")

        DrawGamePadControls(d)
    End Sub

    Public Overrides Sub Update()
        If PreScreen.Identification = Identifications.OverworldScreen And JoinServerScreen.Online = True Then
            Screen.Level.Update()
        End If

        Screen.TextBox.reDelay = 0.0F

        If Me.menuIndex = 0 Then
            UpdateMain()
        Else
            UpdateQuit()
        End If
    End Sub

#Region "MainMenu"

    Private Sub DrawMenu()
        For i = 0 To 1
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = Localization.GetString("pause_menu_back_to_game")
                Case 1
                    Text = Localization.GetString("pause_menu_quit_to_menu")
            End Select

            If i = mainIndex Then
                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 48, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180, 220 + i * 128, 320, 64), True)
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180, 220 + i * 128, 320, 64), True)
            End If

            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(Core.ScreenSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10, 256 + i * 128), Color.Black)
        Next
    End Sub

    Private Sub UpdateMain()
        If Controls.Up(True, True, True) = True Then
            Me.mainIndex -= 1
        End If
        If Controls.Down(True, True, True) = True Then
            Me.mainIndex += 1
        End If

        If Core.GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If Core.ScaleScreenRec(New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180, 220 + i * 128, 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    Me.mainIndex = i

                    If Controls.Accept(True, False) = True Then
                        Select Case Me.mainIndex
                            Case 0
                                ClickContinue()
                            Case 1
                                ClickQuit()
                        End Select
                    End If
                End If
            Next
        End If

        Me.mainIndex = CInt(MathHelper.Clamp(Me.mainIndex, 0, 1))

        If Controls.Accept(False, True) = True Then
            Select Case Me.mainIndex
                Case 0
                    ClickContinue()
                Case 1
                    ClickQuit()
            End Select
        End If

        If Controls.Dismiss() = True Or KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) = True And leftEscapeKey = True Or ControllerHandler.ButtonPressed(Buttons.Start) = True Then
            ClickContinue()
        End If

        If KeyBoardHandler.KeyDown(KeyBindings.EscapeKey) = False And ControllerHandler.ButtonDown(Buttons.Start) = False Then
            Me.leftEscapeKey = True
        End If
    End Sub

    Private Sub ClickContinue()
        Core.SetScreen(Me.PreScreen)
    End Sub

    Private Sub ClickQuit()
        If Me.canCreateAutosave = True Then
            QuitGame()
        Else
            Me.menuIndex = 1
            Me.quitIndex = 0
        End If
    End Sub

#End Region

#Region "QuitMenu"

    Private Sub DrawQuit()
        Dim pX As Integer = CInt(Core.ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.GetString("pause_menu_confirmation")).X / 2)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.GetString("pause_menu_confirmation"), New Vector2(pX - 7, CInt(Core.ScreenSize.Height / 6.8) + 3 + 110), Color.Black)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.GetString("pause_menu_confirmation"), New Vector2(pX - 10, CInt(Core.ScreenSize.Height / 6.8) + 110), Color.White)

        For i = 0 To 1
            Dim Text As String = ""
            Dim x As Integer = 0
            Select Case i
                Case 0
                    Text = Localization.GetString("pause_menu_no")
                    x = -200
                Case 1
                    Text = Localization.GetString("pause_menu_yes")
                    x = 200
            End Select

            If i = quitIndex Then
                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 48, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180 + x, 320, 320, 64), True)
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180 + x, 320, 320, 64), True)
            End If

            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(Core.ScreenSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10 + x, 356), Color.Black)
        Next
    End Sub

    Private Sub UpdateQuit()
        If Controls.Left(True, True, True) = True Then
            Me.quitIndex -= 1
        End If
        If Controls.Right(True, True, True) = True Then
            Me.quitIndex += 1
        End If

        If Core.GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                Dim x As Integer = -200
                If i = 1 Then
                    x = 200
                End If

                If Core.ScaleScreenRec(New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180 + x, 320, 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    Me.quitIndex = i

                    If Controls.Accept(True, False) = True Then
                        Select Case Me.quitIndex
                            Case 0
                                ClickBack()
                            Case 1
                                ClickConfirmationQuit()
                        End Select
                    End If
                End If
            Next
        End If

        Me.quitIndex = CInt(MathHelper.Clamp(Me.quitIndex, 0, 1))

        If Controls.Accept(False, True) = True Then
            Select Case Me.quitIndex
                Case 0
                    ClickBack()
                Case 1
                    ClickConfirmationQuit()
            End Select
        End If

        If Controls.Dismiss(False, True) = True Then
            ClickBack()
        End If
    End Sub

    Private Sub ClickBack()
        Me.menuIndex = 0
    End Sub

    Private Sub ClickConfirmationQuit()
        Me.QuitGame()
    End Sub

#End Region

    Private Sub QuitGame()
        If JoinServerScreen.Online = True Then
            Core.ServersManager.ServerConnection.Disconnect()
        End If
        Chat.ClearChat()
        If Core.Player.loadedSave = True And Me.canCreateAutosave = True Then
            Core.Player.SaveGame(True)
        End If
        ScriptStorage.Clear()
        Core.SetScreen(New MainMenuScreen())
        Core.Player.loadedSave = False
    End Sub

    Public Overrides Sub ChangeTo()
        MusicManager.MasterVolume /= 4
        MusicManager.ForceVolumeUpdate()
    End Sub

    Public Overrides Sub ChangeFrom()
        MusicManager.MasterVolume *= 4
        MusicManager.ForceVolumeUpdate()
    End Sub
End Class