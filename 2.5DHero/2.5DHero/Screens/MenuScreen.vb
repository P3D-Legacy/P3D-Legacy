Public Class MenuScreen

    Inherits Screen

    Dim Options() As String
    Dim index As Integer = 0
    Dim drawRight As Boolean = False
    Dim nextAction As String = ""
    Dim offsetX As Integer = 175

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.MenuScreen
        Me.PreScreen = currentScreen

        Dim newOptions As New List(Of String)
        If Core.Player.hasPokedex = True Then
            newOptions.Add(Localization.GetString("game_menu_pokedex"))
        End If

        If Screen.Level.IsBugCatchingContest = True Then
            newOptions.AddRange({Screen.Level.BugCatchingContestData.GetSplit(2) & " x" & Core.Player.Inventory.GetItemAmount(177), Localization.GetString("game_menu_bag"), Localization.GetString("game_menu_trainer_card"), Localization.GetString("End Contest")})
        Else
            If Core.Player.Pokemons.Count > 0 Then
                newOptions.Add(Localization.GetString("game_menu_party"))
            End If
            newOptions.AddRange({Localization.GetString("game_menu_bag"), Localization.GetString("game_menu_trainer_card"), Localization.GetString("game_menu_save")})
        End If

        newOptions.AddRange({Localization.GetString("game_menu_options"), Localization.GetString("game_menu_exit")})
        Options = newOptions.ToArray()

        Me.index = Player.Temp.MenuIndex
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        If Me.IsCurrentScreen() = True Then
            If Core.Player.IsGamejoltSave = True Then
                GameJolt.Emblem.Draw(GameJolt.API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem, New Vector2(CSng(Core.windowSize.Width / 2 - 256), 30), 4, Core.GameJoltSave.DownloadedSprite)
            End If
        End If

        With Core.SpriteBatch
            Dim T As Texture2D = TextureManager.GetTexture("GUI\Overworld\ChooseBox")
            Dim Position As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2) - 48 + offsetX, Core.windowSize.Height - 160.0F - 96.0F - (Options.Count - 1) * 48)
            .Draw(T, New Rectangle(CInt(Position.X) + offsetX, CInt(Position.Y), 288, 48), New Rectangle(0, 0, 96, 16), Color.White)
            For i = 0 To Options.Count - 2
                .Draw(T, New Rectangle(CInt(Position.X) + offsetX, CInt(Position.Y) + 48 + i * 48, 288, 48), New Rectangle(0, 16, 96, 16), Color.White)
            Next
            .Draw(T, New Rectangle(CInt(Position.X + offsetX), CInt(Position.Y) + 96 + (Options.Count - 2) * 48, 288, 48), New Rectangle(0, 32, 96, 16), Color.White)
            For i = 0 To Options.Count - 1
                .DrawString(FontManager.InGameFont, Options(i), New Vector2(CInt(Position.X + 40) + offsetX, CInt(Position.Y) + 32 + i * 48), Color.Black)
            Next
            .Draw(T, New Rectangle(CInt(Position.X + 20) + offsetX, CInt(Position.Y) + 36 + index * 48, 10, 20), New Rectangle(96, 0, 3, 6), Color.White)
        End With

        TextBox.Draw()
    End Sub

    Public Overrides Sub Update()
        TextBox.Update()
        If TextBox.Showing = False Then
            If drawRight = True Then
                If offsetX < 175 Then
                    offsetX += 5
                Else
                    drawRight = False
                    Select Case nextAction
                        Case Localization.GetString("game_menu_pokedex")
                            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New PokedexSelectScreen(Me), Color.White, False))
                        Case Localization.GetString("game_menu_party")
                            Core.SetScreen(New PokemonScreen(Me, Player.Temp.PokemonScreenIndex))
                        Case Localization.GetString("game_menu_bag")
                            Core.SetScreen(New InventoryScreen(Me))
                        Case Localization.GetString("game_menu_trainer_card")
                            Core.SetScreen(New TrainerScreen(Me))
                        Case "Pokégear"
                            Core.SetScreen(New GameJolt.PokegearScreen(Me, GameJolt.PokegearScreen.EntryModes.MainMenu, {}))
                        Case Localization.GetString("game_menu_save")
                            Core.SetScreen(New SaveScreen(Me))
                        Case Localization.GetString("game_menu_options")
                            Core.SetScreen(New OptionScreen(Me))
                        Case Localization.GetString("game_menu_exit")
                            Core.SetScreen(Me.PreScreen)
                        Case Screen.Level.BugCatchingContestData.GetSplit(2) & " x" & Core.Player.Inventory.GetItemAmount(177)
                            ShowBalls()
                        Case "End Contest"
                            EndContest()
                    End Select
                End If
            Else
                drawRight = False

                If offsetX > 0 Then
                    offsetX -= 5
                Else
                    If Controls.Down(True, True) = True Then
                        Me.index += 1
                    End If
                    If Controls.Up(True, True) = True Then
                        Me.index -= 1
                    End If

                    If Me.index < 0 Then
                        Me.index = Me.Options.Count - 1
                    End If
                    If Me.index = Me.Options.Count Then
                        Me.index = 0
                    End If

                    If Controls.Dismiss() = True Then
                        nextAction = Localization.GetString("game_menu_exit")
                        drawRight = True
                    End If

                    If Controls.Accept() = True Then
                        drawRight = True
                        nextAction = Options(index)
                    End If
                End If
            End If

            Player.Temp.MenuIndex = Me.index
        End If
    End Sub

    Private Sub ShowBalls()
        Dim s As Screen = Me.PreScreen
        CType(s, OverworldScreen).ActionScript.StartScript(Screen.Level.BugCatchingContestData.GetSplit(1), 0)
        Core.SetScreen(s)
    End Sub

    Private Sub EndContest()
        Dim s As Screen = Me.PreScreen
        CType(s, OverworldScreen).ActionScript.StartScript(Screen.Level.BugCatchingContestData.GetSplit(0), 0)
        Core.SetScreen(s)
    End Sub

End Class