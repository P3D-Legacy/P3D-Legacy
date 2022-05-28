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

        If currentScreen IsNot Nothing Then
            Me.PreScreen = currentScreen
        End If

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        If Core.Player.IsGameJoltSave = True Then
            Me.canCreateAutosave = False
        Else
            If Me.PreScreen IsNot Nothing Then
                If Camera Is Nothing Then
                    Camera = New OverworldCamera()
                ElseIf Camera.Name IsNot Nothing Then
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
        End If
    End Sub

    Public Overrides Sub Draw()
        If Me.PreScreen IsNot Nothing Then
            Me.PreScreen.Draw()
        End If

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), New Color(0, 0, 0, 150))
        Dim titletext As String = Localization.Translate("title")
        Dim pX As Integer = CInt(Core.ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(titletext).X / 2)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, titletext, New Vector2(CInt(pX - 7), CInt(160 - FontManager.InGameFont.MeasureString(titletext).Y / 2 + 3)), Color.Black)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, titletext, New Vector2(CInt(pX - 10), CInt(160 - FontManager.InGameFont.MeasureString(titletext).Y / 2)), Color.White)

        If Me.menuIndex = 0 Then
            DrawMenu()
        Else
            DrawQuit()
        End If

        If Me.canCreateAutosave = False Then
            Dim autosaveFailText As String = Localization.Translate("autosave_fail")

            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, autosaveFailText, New Vector2(9, CInt(Core.ScreenSize.Height - FontManager.InGameFont.MeasureString(autosaveFailText).Y)), Color.Black)
            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, autosaveFailText, New Vector2(5, CInt(Core.ScreenSize.Height - FontManager.InGameFont.MeasureString(autosaveFailText).Y - 4)), Color.White)
        End If

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, Localization.Translate("global.accept"))
        d.Add(Buttons.B, Localization.Translate("global.cancel"))

        DrawGamePadControls(d)
    End Sub

    Public Overrides Sub Update()
        If PreScreen.Identification = Identifications.OverworldScreen Then
            Screen.Level.Update()
        End If

        Screen.TextBox.reDelay = 0.0F

        If Core.GameInstance.IsActive = True Then
            If Me.menuIndex = 0 Then
                UpdateMain()
            Else
                UpdateQuit()
            End If
        End If
    End Sub

#Region "MainMenu"

    Private Sub DrawMenu()
        Dim FontColor As Color
        Dim FontShadow As Color = New Color(0, 0, 0, 0)
        For i = 0 To 1
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = Localization.Translate("back_to_game")
                Case 1
                    Text = Localization.Translate("quit_to_menu")
            End Select

            If i = mainIndex Then
                FontColor = Color.White
                FontShadow.A = 255

                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 48, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180, 220 + i * 128, 320, 64), True)
            Else
                FontColor = Color.Black
                FontShadow.A = 0

                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180, 220 + i * 128, 320, 64), True)
            End If

            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(Core.ScreenSize.Width / 2 - FontManager.InGameFont.MeasureString(Text).X / 2 - 10 + 2), CInt(256 + i * 128 + 2)), FontShadow)
            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(Core.ScreenSize.Width / 2 - FontManager.InGameFont.MeasureString(Text).X / 2 - 10), CInt(256 + i * 128)), FontColor)
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
                If Core.ScaleScreenRec(New Rectangle(CInt(Core.ScreenSize.Width / 2) - 180, CInt(220 + i * 128), 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    Me.mainIndex = i

                    If Controls.Accept(True, False) = True Then
                        Select Case Me.mainIndex
                            Case 0
                                SoundManager.PlaySound("select")
                                ClickContinue()
                            Case 1
                                SoundManager.PlaySound("select")
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
                    SoundManager.PlaySound("select")
                    ClickContinue()
                Case 1
                    SoundManager.PlaySound("select")
                    ClickQuit()
            End Select
        End If

        If Controls.Dismiss() = True Or KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) = True And leftEscapeKey = True Or ControllerHandler.ButtonPressed(Buttons.Start) = True Then
            SoundManager.PlaySound("select")
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
        Dim pX As Integer = CInt(Core.ScreenSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Localization.Translate("confirmation")).X / 2)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.Translate("pause_menu_confirmation"), New Vector2(CInt(pX - 7), CInt(Core.ScreenSize.Height / 7.5) + 110 + 2), Color.Black)
        Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Localization.Translate("pause_menu_confirmation"), New Vector2(CInt(pX - 10), CInt(Core.ScreenSize.Height / 7.5) + 110), Color.White)
        Dim FontColor As Color
        Dim FontShadow As Color = New Color(0, 0, 0, 0)

        For i = 0 To 1
            Dim Text As String = ""
            Dim x As Integer = 0
            Select Case i
                Case 0
                    Text = Localization.Translate("global.no")
                    x = -200
                Case 1
                    Text = Localization.Translate("global.yes")
                    x = 200
            End Select

            If i = quitIndex Then
                FontColor = Color.White
                FontShadow.A = 255

                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 48, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2 - 180 + x), 320, 320, 64), True)
            Else
                FontColor = Color.Black
                FontShadow.A = 0

                Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(Core.ScreenSize.Width / 2 - 180 + x), 320, 320, 64), True)
            End If

            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(Core.ScreenSize.Width / 2 - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10 + x + 2), 356 + 2), FontShadow)
            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(Core.ScreenSize.Width / 2 - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10 + x), 356), FontColor)
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

                If Core.ScaleScreenRec(New Rectangle(CInt(Core.ScreenSize.Width / 2 - 180 + x), 320, 320 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    Me.quitIndex = i

                    If Controls.Accept(True, False) = True Then
                        Select Case Me.quitIndex
                            Case 0
                                SoundManager.PlaySound("select")
                                ClickBack()
                            Case 1
                                SoundManager.PlaySound("select")
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
                    SoundManager.PlaySound("select")
                    ClickBack()
                Case 1
                    SoundManager.PlaySound("select")
                    ClickConfirmationQuit()
            End Select
        End If

        If Controls.Dismiss(False, True) = True Then
            SoundManager.PlaySound("select")
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
        ScriptStorage.Clear()
        Core.SetScreen(New PressStartScreen())
        Core.Player.loadedSave = False
    End Sub

    Public Overrides Sub ChangeTo()
        MusicManager.MasterVolume /= 4
        MusicManager.UpdateVolume()
    End Sub

    Public Overrides Sub ChangeFrom()
        MusicManager.MasterVolume *= 4
        MusicManager.UpdateVolume()
    End Sub
End Class