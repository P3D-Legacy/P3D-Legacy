Public Class BlackOutScreen

    Inherits Screen

    Dim BattleScreen As BattleSystem.BattleScreen
    Dim index As Integer = 0
    Dim textIndex As Integer = 0
    Dim Text() As String = {Localization.GetString("black_out_screen_line1"), vbNewLine, "        ", Localization.GetString("black_out_screen_line2"), Localization.GetString("black_out_screen_line3"), Localization.GetString("black_out_screen_line4"), Localization.GetString("black_out_screen_line5")}
    Dim ready As Boolean = False
    Dim delay As Single = 0.2F

    Dim IsGameOver As Boolean = False

    Dim FromBattle As Boolean = True

    Public Sub New(ByVal BattleScreen As BattleSystem.BattleScreen)
        Me.BattleScreen = BattleScreen
        Me.Identification = Identifications.BlackOutScreen

        Me.IsGameOver = CBool(GameModeManager.GetGameRuleValue("GameOverAt0Pokemon", "0"))

        FromBattle = True
    End Sub

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.BlackOutScreen

        Me.IsGameOver = CBool(GameModeManager.GetGameRuleValue("GameOverAt0Pokemon", "0"))

        FromBattle = False
    End Sub

    Public Overrides Sub Update()
        If Me.IsGameOver = True Then
            If Controls.Accept(True, False) = True Then
                Core.SetScreen(New MainMenuScreen())
                Core.Player.loadedSave = False
            End If
        Else
            If Me.ready = False Then
                If delay > 0.0F Then
                    delay -= 0.1F
                    If delay <= 0.0F Then
                        delay = 0.2F
                        ProceedText()
                    End If
                End If
            Else
                If Controls.Accept(True, True) = True Then
                    SoundManager.PlaySound("pokemon_heal")

                    If FromBattle = True Then
                        Core.Player.HealParty()
                        ChangeScreen()

                        ChangeFromSurfRideTexture()

                        Screen.Level.Load(Core.Player.LastRestPlace)
                        Screen.Level.OverworldPokemon.Visible = False
                        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
                        Dim positionString() As String = Core.Player.LastRestPlacePosition.Split(CChar(","))
                        CType(BattleScreen.SavedOverworld.Camera, OverworldCamera).YawLocked = False
                        Screen.Camera.Yaw = MathHelper.Pi
                        Screen.Camera.Position = New Vector3(CSng(positionString(0).Replace(".", GameController.DecSeparator)), CSng(positionString(1).Replace(".", GameController.DecSeparator)), CSng(positionString(2).Replace(".", GameController.DecSeparator)))
                        CType(BattleScreen.SavedOverworld.OverworldScreen, OverworldScreen).ActionScript.Scripts.Clear()

                        Core.SetScreen(New TransitionScreen(Me, BattleScreen.SavedOverworld.OverworldScreen, Color.Black, False))
                    Else
                        Core.Player.HealParty()

                        ChangeFromSurfRideTexture()

                        Screen.Level.Load(Core.Player.LastRestPlace)
                        Screen.Level.OverworldPokemon.Visible = False
                        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

                        Dim positionString() As String = Core.Player.LastRestPlacePosition.Split(CChar(","))
                        CType(BattleScreen.SavedOverworld.Camera, OverworldCamera).YawLocked = False
                        Screen.Camera.Yaw = MathHelper.Pi
                        Screen.Camera.Position = New Vector3(CSng(positionString(0).Replace(".", GameController.DecSeparator)), CSng(positionString(1).Replace(".", GameController.DecSeparator)), CSng(positionString(2).Replace(".", GameController.DecSeparator)))
                        CType(BattleScreen.SavedOverworld.OverworldScreen, OverworldScreen).ActionScript.Scripts.Clear()

                        While Core.CurrentScreen.Identification <> Identifications.OverworldScreen
                            Core.SetScreen(Core.CurrentScreen.PreScreen)
                        End While
                    End If
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), Color.Black)

        If IsGameOver = True Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "GAME OVER", New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString("GAME OVER").X / 2), 100), Color.White)
        Else
            Dim f As SpriteFont = FontManager.MiniFont
            Dim aText As String = ""
            For i = 0 To textIndex
                If i <> textIndex Then
                    aText &= Text(i) & vbNewLine
                Else
                    If textIndex < Text.Count() Then
                        aText &= Text(i).Remove(index)
                    End If
                End If
            Next

            Dim p As New Vector2(CSng(Core.windowSize.Width / 2 - f.MeasureString(aText).X / 2), CSng(Core.windowSize.Height / 2 - (f.MeasureString(aText).Y) / 2))

            Core.SpriteBatch.DrawString(FontManager.MiniFont, aText, p, Color.White)
        End If

        If Me.IsGameOver = True Or Me.ready = True Then
            Dim d As New Dictionary(Of Buttons, String)
            d.Add(Buttons.A, "Accept")
            DrawGamePadControls(d)
        End If
    End Sub

    Private Sub ChangeScreen()
        Screen.Level = BattleScreen.SavedOverworld.Level
        Screen.Camera = BattleScreen.SavedOverworld.Camera
        Screen.Effect = BattleScreen.SavedOverworld.Effect
        Screen.SkyDome = BattleScreen.SavedOverworld.SkyDome
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
    End Sub

    Private Sub ProceedText()
        If index >= Text(textIndex).Length - 1 Then
            index = 0
            textIndex += 1
            If textIndex > Text.Count() - 1 Then
                Me.ready = True
            End If
        Else
            index += 1
        End If
    End Sub

    Public Overrides Sub ChangeTo()
        MusicManager.PlayMusic("nomusic", False)
        Core.Player.Inventory.RemoveItem(177) 'Remove all Park Balls.
        PlayerStatistics.Track("Blackouts", 1)
    End Sub

    Private Sub ChangeFromSurfRideTexture()
        If Screen.Level.Riding = True Then
            Screen.Level.Riding = False
            Screen.Level.OwnPlayer.SetTexture(Core.Player.TempRideSkin, True)
            Core.Player.Skin = Core.Player.TempRideSkin
        End If
        If Screen.Level.Surfing = True Then
            Screen.Level.Surfing = False
            Screen.Level.OwnPlayer.SetTexture(Core.Player.TempSurfSkin, True)
            Core.Player.Skin = Core.Player.TempSurfSkin
        End If
        Screen.Level.OverworldPokemon.warped = True
        Screen.Level.OverworldPokemon.Visible = False
    End Sub

End Class