Public Class SaveScreen

    Inherits Screen

    Dim ready As Boolean = False
    Dim delay As Single = 15.0F
    Dim mainTexture As Texture2D

    Dim savingStarted As Boolean = False
    Dim saveSessionFailed As Boolean = False
    Dim backupFileLocation As String = ""

    Private Yslide As Integer = 0
    Private YslideMAX As Integer = CInt(Core.windowSize.Height / 2) + 270

    Dim menuTexture As Texture2D
    Dim _closing As Boolean = False
    Dim _opening As Boolean = True
    Private target As RenderTarget2D
    Private target2 As RenderTarget2D

    Public Sub New(ByVal currentScreen As Screen)
        Yslide = YslideMAX

        Me.Identification = Identifications.SaveScreen
        Me.PreScreen = currentScreen

        target = New RenderTarget2D(GraphicsDevice, 700, 440, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        target2 = New RenderTarget2D(GraphicsDevice, Math.Max(1, Core.windowSize.Width), Math.Max(1, Core.windowSize.Height), False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
        Me.menuTexture = TextureManager.GetTexture("GUI\Menus\SaveBook")
        ChooseBox.Show({Localization.GetString("save_screen_yes"), Localization.GetString("save_screen_no")}, 0, {})

        SaveGameHelpers.ResetSaveCounter()
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        If Core.Player.IsGameJoltSave = True Then
            GameJolt.Emblem.Draw(GameJolt.API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem, New Vector2(CSng(Core.windowSize.Width / 2 - 256), 30), 4, Core.GameJoltSave.DownloadedSprite)
        End If

        Dim saveBookBatch = New SpriteBatch(GraphicsDevice)
        Dim renderBatch = New SpriteBatch(GraphicsDevice)

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Dim Render_X As Integer = halfWidth - 350
        Dim Render_Y As Integer = halfHeight - 220 + Yslide

        Dim Delta_X As Integer = 0
        Dim Delta_Y As Integer = 0
        GraphicsDevice.SetRenderTarget(target)
        GraphicsDevice.Clear(Color.Transparent)

        saveBookBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise)
        With saveBookBatch
            .Draw(menuTexture, New Rectangle(Delta_X, Delta_Y, 700, 440), Color.White)

            If saveSessionFailed = True Then
                .DrawString(FontManager.InGameFont, "Saving failed!", New Vector2(Delta_X + 90, Delta_Y + 50), Color.Red)

                If Core.GameOptions.Extras.Contains("Backup Save Feature") Then
                    .DrawString(FontManager.MiniFont,
                        "Press Dismiss to close this" & Environment.NewLine &
                        "screen and try to save again" & Environment.NewLine &
                        "in order to prevent data" & Environment.NewLine &
                        "corruption." & Environment.NewLine & Environment.NewLine & Environment.NewLine &
                        "Your save has been backed" & Environment.NewLine &
                        "up in the event of the" & Environment.NewLine &
                        "Gamejolt API being down.", New Vector2(Delta_X + 90, Delta_Y + 100), Color.Black)
                    .DrawString(FontManager.MiniFont,
                        "You may safely quit the" & Environment.NewLine &
                        "game now or try to save" & Environment.NewLine &
                        "again later." & Environment.NewLine & Environment.NewLine & Environment.NewLine &
                        "The backup save can be" & Environment.NewLine &
                        "found in the Backup Save" & Environment.NewLine &
                        "folder", New Vector2(Delta_X + 390, Delta_Y + 100), Color.Black)
                Else
                    .DrawString(FontManager.MiniFont,
                        "Press Dismiss to close this" & Environment.NewLine &
                        "screen and try to save again" & Environment.NewLine &
                        "in order to prevent data" & Environment.NewLine &
                        "corruption." & Environment.NewLine & Environment.NewLine & Environment.NewLine &
                        "If the problem persists, the" & Environment.NewLine &
                        "GameJolt servers could be" & Environment.NewLine &
                        "down for maintenance right" & Environment.NewLine &
                        "now.", New Vector2(Delta_X + 90, Delta_Y + 100), Color.Black)
                    .DrawString(FontManager.MiniFont, "Please try again later," & Environment.NewLine &
                        "or contact us here:" & Environment.NewLine & Environment.NewLine &
                        "Discord server" & Environment.NewLine &
                        "www.discord.me/p3d" & Environment.NewLine & Environment.NewLine &
                        "Official Forum" & Environment.NewLine &
                        "pokemon3d.net/forum/news", New Vector2(Delta_X + 390, Delta_Y + 100), Color.Black)
                End If

                Dim text As String = String.Empty
                If ControllerHandler.IsConnected() Then
                    text = "Press      to continue"
                Else
                    text = "Press " & KeyBindings.BackKey1.ToString() & " to continue"
                End If

                Dim textSize As Vector2 = FontManager.GameJoltFont.MeasureString(text)

                GetFontRenderer().DrawString(FontManager.MiniFont, text, New Vector2(Delta_X + 610 - textSize.X / 2.0F,
                                                                                   Delta_Y + 350 - textSize.Y / 2.0F), Color.DarkBlue)

                If ControllerHandler.IsConnected() Then
                    SpriteBatch.Draw(TextureManager.GetTexture("GUI\GamePad\xboxControllerButtonB"), New Rectangle(CInt(Delta_X + 610 - textSize.X / 2 + FontManager.MiniFont.MeasureString("Press ").X),
                                                                                                               CInt(Delta_Y + 350 - textSize.Y / 2), 20, 20), Color.White)
                End If

            Else
                If ready = True Then
                    .DrawString(FontManager.InGameFont, "Saved the game.", New Vector2(Delta_X + 90, Delta_Y + 50), Color.DarkBlue)
                Else
                    If SaveGameHelpers.GameJoltSaveDone() = False And savingStarted = True Then
                        If SaveGameHelpers.StartedDownloadCheck = True Then
                            .DrawString(FontManager.InGameFont, "Validating data" & LoadingDots.Dots, New Vector2(Delta_X + 90, Delta_Y + 50), Color.Black)
                        Else
                            .DrawString(FontManager.InGameFont, "Saving, please wait" & LoadingDots.Dots, New Vector2(Delta_X + 77, Delta_Y + 50), Color.Black)
                        End If
                    Else
                        .DrawString(FontManager.InGameFont, "Would you like to", New Vector2(Delta_X + 90, Delta_Y + 50), Color.Black)
                        .DrawString(FontManager.InGameFont, "save the game?", New Vector2(Delta_X + 90, Delta_Y + 80), Color.Black)
                    End If
                End If

                For i = 0 To Core.Player.Pokemons.Count - 1
                    Dim Pos As New Vector2(Delta_X + 390 + (i Mod 3) * 80, Delta_Y + 50 + CInt(Math.Floor(i / 3)) * 80)
                    Dim pokeTexture = Core.Player.Pokemons(i).GetMenuTexture()
                    .Draw(pokeTexture, New Rectangle(CInt(Pos.X), CInt(Pos.Y), 64, 64), Color.White)

                    If Not Core.Player.Pokemons(i).Item Is Nothing And Core.Player.Pokemons(i).IsEgg() = False Then
                        .Draw(Core.Player.Pokemons(i).Item.Texture, New Rectangle(CInt(Pos.X) + 36, CInt(Pos.Y) + 36, 32, 32), Color.White)
                    End If
                Next

                .DrawString(FontManager.MainFont, Localization.GetString("save_screen_name") & ": " & Core.Player.Name & Environment.NewLine & Environment.NewLine & Localization.GetString("save_screen_badges") & ": " & Core.Player.Badges.Count.ToString() & Environment.NewLine & Environment.NewLine & Localization.GetString("save_screen_money") & ": $" & Core.Player.Money & Environment.NewLine & Environment.NewLine & Localization.GetString("save_screen_time") & ": " & TimeHelpers.GetDisplayTime(TimeHelpers.GetCurrentPlayTime(), True), New Vector2(Delta_X + 400, Delta_Y + 215), Color.DarkBlue)
            End If
        End With
        saveBookBatch.End()
        GraphicsDevice.SetRenderTarget(target2)
        GraphicsDevice.Clear(Color.Transparent)

        renderBatch.Begin()

        renderBatch.Draw(target, New Rectangle(CInt(Core.windowSize.Width / 2 - 350 * SpriteBatch.InterfaceScale()), CInt(Core.windowSize.Height / 2 - 220 * SpriteBatch.InterfaceScale() + Yslide), CInt(target.Width * SpriteBatch.InterfaceScale()), CInt(target.Height * SpriteBatch.InterfaceScale())), Nothing, Color.White, 0.0F, Vector2.Zero, SpriteEffects.None, 0F)

        renderBatch.End()
        GraphicsDevice.SetRenderTarget(Nothing)

        SpriteBatch.Draw(target2, New Vector2(0, 0), Color.White)
        Dim ChooseBoxPositionOffset As New Vector2(0, 0)
        Select Case Core.SpriteBatch.InterfaceScale
            Case 0.5
                ChooseBoxPositionOffset = New Vector2(-96, 16)
            Case 2
                ChooseBoxPositionOffset = New Vector2(-256, -128)
        End Select

        Screen.ChooseBox.Draw(New Vector2(CInt(Render_X + 115 + Math.Floor(Core.SpriteBatch.InterfaceScale - 1) * ChooseBoxPositionOffset.X), CInt(Render_Y + 155 + Math.Floor(Core.SpriteBatch.InterfaceScale - 1) * ChooseBoxPositionOffset.Y)), False, 1.5F)
    End Sub

    Public Overrides Sub Update()
        Screen.ChooseBox.Update()

        If _opening Then
            If Yslide < 5 Then
                Yslide = 0
                _opening = False
            Else
                Yslide = CInt(MathHelper.Lerp(Yslide, 0, 0.1F))
            End If

            Exit Sub
        ElseIf _closing Then
            If Yslide < CInt(YslideMAX * 0.91) Then
                Yslide = CInt(MathHelper.Lerp(Yslide, YslideMAX, 0.1F))
            Else
                _closing = False
                Core.SetScreen(Me.PreScreen)
            End If

            Exit Sub
        End If

        If Core.Player.IsGameJoltSave = True Then
            If SaveGameHelpers.GameJoltSaveDone() = True Then
                ready = True

                If SaveGameHelpers.EncounteredErrors = True Then
                    Me.saveSessionFailed = True
                Else
                    If (File.Exists(GameController.GamePath & "\Backup Save\" & GameJoltSave.GameJoltID.ToString() & "\Encrypted\Encrypted.dat")) Then
                        File.Delete(GameController.GamePath & "\Backup Save\" & GameJoltSave.GameJoltID.ToString() & "\Encrypted\Encrypted.dat")
                    End If
                    SoundManager.PlaySound("save")
                End If

                SaveGameHelpers.ResetSaveCounter()
            End If
        End If

        If saveSessionFailed = False Then
            If ChooseBox.Showing = False Then
                If ready = True Then
                    If Me.delay <= 0.0F Then
                        _closing = True
                        'Core.SetScreen(Me.PreScreen)
                    Else
                        Me.delay -= 0.2F
                        If delay <= 0.0F Then
                            delay = 0.0F
                        End If
                    End If
                End If
            End If

            If ChooseBox.readyForResult = True And savingStarted = False Then
                If ChooseBox.result = 0 Then
                    If ready = False Then
                        Core.Player.SaveGame(False)

                        savingStarted = True

                        If Core.Player.IsGameJoltSave = False Then
                            ready = True
                            SoundManager.PlaySound("save")
                        End If
                    End If
                Else
                    delay = 0.0F
                    ready = True
                End If
            End If

            If Controls.Dismiss() And ready = False Then
                SoundManager.PlaySound("select")
                ChooseBox.Showing = False
                _closing = True
                'Core.SetScreen(Me.PreScreen)
            End If
        Else
            If Controls.Dismiss() = True Then
                SoundManager.PlaySound("select")
                ChooseBox.Showing = False
                _closing = True
                'Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub
End Class