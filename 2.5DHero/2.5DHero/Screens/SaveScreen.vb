Public Class SaveScreen

    Inherits Screen

    Dim ready As Boolean = False
    Dim delay As Single = 15.0F
    Dim mainTexture As Texture2D

    Dim savingStarted As Boolean = False
    Dim saveSessionFailed As Boolean = False
    Dim backupFileLocation As String = ""

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.SaveScreen
        Me.PreScreen = currentScreen

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
        ChooseBox.Show({Localization.GetString("save_screen_yes"), Localization.GetString("save_screen_no")}, 0, {})

        SaveGameHelpers.ResetSaveCounter()
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        If Core.Player.IsGameJoltSave = True Then
            GameJolt.Emblem.Draw(GameJolt.API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem, New Vector2(CSng(Core.windowSize.Width / 2 - 256), 30), 4, Core.GameJoltSave.DownloadedSprite)
        End If

        Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(168, 168, 640, 320))

        With Core.SpriteBatch
            If saveSessionFailed = True Then
                .DrawString(FontManager.InGameFont, "Saving failed!", New Vector2(188, 186), Color.Red)
                .DrawString(FontManager.MiniFont, "Press Dismiss to close this screen and try to save" & vbNewLine &
                            "again in order to prevent data corruption.", New Vector2(188, 240), Color.Black)
            Else
                If ready = True Then
                    .DrawString(FontManager.InGameFont, Localization.GetString("save_screen_success"), New Vector2(188, 186), Color.DarkBlue)
                Else
                    If SaveGameHelpers.GameJoltSaveDone() = False And savingStarted = True Then
                        If SaveGameHelpers.StartedDownloadCheck = True Then
                            .DrawString(FontManager.InGameFont, "Validating uploaded data" & LoadingDots.Dots, New Vector2(188, 186), Color.Black)
                        Else
                            .DrawString(FontManager.InGameFont, "Saving, please wait" & LoadingDots.Dots, New Vector2(188, 186), Color.Black)
                        End If
                    Else
                        .DrawString(FontManager.InGameFont, Localization.GetString("save_screen_title"), New Vector2(188, 186), Color.Black)
                    End If
                End If

                For i = 0 To Core.Player.Pokemons.Count - 1
                    .Draw(Core.Player.Pokemons(i).GetMenuTexture(), New Rectangle(220 + i * 80, 260, 64, 64), Color.White)

                    If Not Core.Player.Pokemons(i).Item Is Nothing And Core.Player.Pokemons(i).IsEgg() = False Then
                        .Draw(Core.Player.Pokemons(i).Item.Texture, New Rectangle(CInt(252 + i * 80), 290, 32, 32), Color.White)
                    End If
                Next

                .DrawString(FontManager.MiniFont, Localization.GetString("save_screen_name") & ": " & Core.Player.Name & vbNewLine & vbNewLine & Localization.GetString("save_screen_badges") & ": " & Core.Player.Badges.Count.ToString() & vbNewLine & vbNewLine & Localization.GetString("save_screen_money") & ": " & Core.Player.Money & vbNewLine & vbNewLine & Localization.GetString("save_screen_time") & ": " & TimeHelpers.GetDisplayTime(TimeHelpers.GetCurrentPlayTime(), True), New Vector2(192, 350), Color.DarkBlue)
            End If
        End With

        Screen.ChooseBox.Draw()
    End Sub

    Public Overrides Sub Update()
        Screen.ChooseBox.Update()

        If Core.Player.IsGameJoltSave = True Then
            If SaveGameHelpers.GameJoltSaveDone() = True Then
                ready = True

                If SaveGameHelpers.EncounteredErrors = True Then
                    Me.saveSessionFailed = True
                Else
                    SoundManager.PlaySound("save")
                End If

                SaveGameHelpers.ResetSaveCounter()
            End If
        End If

        If saveSessionFailed = False Then
            If ChooseBox.Showing = False Then
                If ready = True Then
                    If Me.delay <= 0.0F Then
                        Core.SetScreen(Me.PreScreen)
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
                ChooseBox.Showing = False
                Core.SetScreen(Me.PreScreen)
            End If
        Else
            If Controls.Dismiss() = True Then
                ChooseBox.Showing = False
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub
End Class