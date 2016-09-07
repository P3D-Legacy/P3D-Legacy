Namespace GameJolt

    Public Class WonderTradeScreen

        Inherits Screen

        Enum ScreenStates
            Choose
            Downloading
            Idle
            PerformingTrade
            Trading
            Stopped
        End Enum

        Public Const WONDERTRADE_VERSION As String = "1.0"

        'GameJolt keys:     GTS_WONDERTRADE_VX
        '                   GTS_WONDERTRADE_TRADEID

        Dim WonderTradePokemonData As String = ""
        Dim WonderTradeID As Integer = 0
        Dim GameJoltID As String = ""

        Dim SelectedPokemonIndex As Integer = -1
        Dim SelectedPokemon As Pokemon = Nothing
        Dim WonderTradePokemon As Pokemon = Nothing

        Dim OpenedSelect As Boolean = False
        Dim WaitForSave As Boolean = False
        Dim PartnerEmblem As Emblem = Nothing

        Dim DisconnectMessage As String = ""
        Dim ScreenState As ScreenStates = ScreenStates.Choose

        Dim MenuItems() As String = {"Yes", "No"}
        Dim texture As Texture2D = Nothing
        Dim Cursor As Integer = 0

        Public Sub New(ByVal currentScreen As Screen)
            Me.Identification = Identifications.WonderTradeScreen
            Me.PreScreen = currentScreen

            Me.MouseVisible = True
            Me.CanChat = False
            Me.texture = TextureManager.GetTexture("GUI\Menus\General")
        End Sub

        Private Sub DownloadData()
            Dim APICall As New APICall(AddressOf GotPokemonData)
            APICall.GetStorageData("GTS_WONDERTRADE_V" & WONDERTRADE_VERSION, False)
        End Sub

        Private Sub GotPokemonData(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(1).Value.Contains("|") = True Then
                Me.GameJoltID = list(1).Value.Remove(list(1).Value.IndexOf("|"))
                Me.WonderTradePokemonData = list(1).Value.Remove(0, list(1).Value.IndexOf("|") + 1).Replace("\""", """")

                If Me.GameJoltID = Core.GameJoltSave.GameJoltID And GameController.IS_DEBUG_ACTIVE = False Then
                    DisconnectMessage = "We are sorry, but we couldn't find a trade partner for you."
                    Me.ScreenState = ScreenStates.Stopped
                Else
                    Dim APICall1 As New APICall(AddressOf GotTradeID)
                    APICall1.GetStorageData("GTS_WONDERTRADE_TRADEID", False)
                End If
            Else
                DisconnectMessage = "We are sorry, but we couldn't find a trade partner for you."
                Me.ScreenState = ScreenStates.Stopped
            End If
        End Sub

        Private Sub GotTradeID(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If IsNumeric(list(1).Value) = True Then
                Me.WonderTradeID = CInt(list(1).Value)
                Me.PartnerEmblem = New Emblem(GameJoltID, 0)
            Else
                DisconnectMessage = "We are sorry, but we couldn't find a trade partner for you."
                Me.ScreenState = ScreenStates.Stopped
            End If
        End Sub

        Public Overrides Sub Draw()
            Canvas.DrawGradient(Core.windowSize, New Color(237, 236, 126), New Color(201, 175, 31), False, -1)

            Select Case Me.ScreenState
                Case ScreenStates.Choose
                    Dim t As String = "With Wonder Trade, you trade your" & vbNewLine &
                                      "Pokémon instantly with a random one" & vbNewLine &
                                      "from anyone anywhere in the world!" & vbNewLine & vbNewLine &
                                      "Do you want to do a Wonder Trade?"

                    Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 150, 600, 200), New Color(135, 168, 20, 100))
                    Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 190), Color.Black, 0.0F, Vector2.Zero, New Vector2(1.0F, 1.1F), SpriteEffects.None, 0.0F)

                    For i = 0 To Me.MenuItems.Count - 1
                        Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 400 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                        Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 64, 400 + i * 96, 64 * 2, 64), New Rectangle(32, 16, 16, 16), Color.White)
                        Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 64 * 3, 400 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, Me.MenuItems(i), New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 20, 416 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
                    Next

                    DrawCursor()
                Case ScreenStates.Downloading
                    Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 200, Core.windowSize.Width, 50), New Color(255, 255, 255, 0), New Color(135, 168, 20), False, -1)
                    Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - 150, Core.windowSize.Width, 50), New Color(135, 168, 20))
                    Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 100, Core.windowSize.Width, 50), New Color(135, 168, 20), New Color(255, 255, 255, 0), False, -1)

                    Core.SpriteBatch.DrawString(FontManager.MainFont, "Searching for a trade partner" & LoadingDots.Dots, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Searching for a trade partner" & LoadingDots.Dots).X / 2), Core.windowSize.Height - 140), Color.White)
                Case ScreenStates.Stopped
                    Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 200, Core.windowSize.Width, 50), New Color(255, 255, 255, 0), New Color(135, 168, 20), False, -1)
                    Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - 150, Core.windowSize.Width, 50), New Color(135, 168, 20))
                    Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 100, Core.windowSize.Width, 50), New Color(135, 168, 20), New Color(255, 255, 255, 0), False, -1)

                    Core.SpriteBatch.DrawString(FontManager.MainFont, DisconnectMessage, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(DisconnectMessage).X / 2), Core.windowSize.Height - 140), Color.White)
                Case ScreenStates.Idle, ScreenStates.PerformingTrade
                    If Me.SelectedPokemonIndex > -1 Then
                        Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 200, Core.windowSize.Width, 50), New Color(255, 255, 255, 0), New Color(135, 168, 20), False, -1)
                        Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - 150, Core.windowSize.Width, 50), New Color(135, 168, 20))
                        Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 100, Core.windowSize.Width, 50), New Color(135, 168, 20), New Color(255, 255, 255, 0), False, -1)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, "Your trade partner:", New Vector2(CSng(Core.windowSize.Width / 2 - 256), Core.windowSize.Height - 140), Color.White)
                        Me.PartnerEmblem.Draw(New Vector2(CSng(Core.windowSize.Width / 2), Core.windowSize.Height - 160), 2)

                        Dim p As Pokemon = Me.SelectedPokemon
                        Core.SpriteBatch.Draw(p.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), 80, 256, 256), Color.White)

                        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 350, 600, 40), New Color(135, 168, 20, 100))
                        Core.SpriteBatch.DrawString(FontManager.MainFont, p.GetDisplayName(), New Vector2(CInt(Core.windowSize.Width / 2 - 298), 355), Color.White)

                        If p.Gender = Pokemon.Genders.Female Then
                            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(CInt(Core.windowSize.Width / 2 - 100), 358, 12, 20), New Rectangle(102, 0, 6, 10), Color.White)
                        ElseIf p.Gender = Pokemon.Genders.Male Then
                            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(CInt(Core.windowSize.Width / 2 - 100), 358, 12, 20), New Rectangle(96, 0, 6, 10), Color.White)
                        End If

                        Core.SpriteBatch.DrawString(FontManager.MainFont, "Lv. " & p.Level, New Vector2(CInt(Core.windowSize.Width / 2 - 50), 355), Color.White)

                        If p.Item Is Nothing Then
                            Core.SpriteBatch.DrawString(FontManager.MainFont, "None", New Vector2(CInt(Core.windowSize.Width / 2 + 60), 355), Color.White)
                        Else
                            Core.SpriteBatch.DrawString(FontManager.MainFont, p.Item.Name, New Vector2(CInt(Core.windowSize.Width / 2 + 60), 355), Color.White)
                        End If
                    End If
                Case ScreenStates.Trading
                    DrawTrading()
            End Select

            Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, 40), New Color(135, 168, 20))
            Canvas.DrawGradient(New Rectangle(0, 40, Core.windowSize.Width, 100), New Color(135, 168, 20), New Color(255, 255, 255, 0), False, -1)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Wondertrade", New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Wondertrade").X / 2), 10), Color.White)
        End Sub

        Private Sub DrawCursor()
            Dim cPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 160, 400 + Me.Cursor * 96 - 42)

            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
        End Sub

        Public Overrides Sub Update()
            LogInScreen.KickFromOnlineScreen(Me)

            Select Case Me.ScreenState
                Case ScreenStates.Choose
                    If Controls.Up(True, True, True, True, True, True) = True Then
                        Me.Cursor -= 1
                        If Controls.ShiftDown() = True Then
                            Me.Cursor -= 4
                        End If
                    End If
                    If Controls.Down(True, True, True, True, True, True) = True Then
                        Me.Cursor += 1
                        If Controls.ShiftDown() = True Then
                            Me.Cursor += 4
                        End If
                    End If

                    Me.Cursor = Me.Cursor.Clamp(0, Me.MenuItems.Count - 1)

                    If Controls.Accept(True, False, False) = True Then
                        For i = 0 To Me.MenuItems.Count - 1
                            If New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 400 + i * 96, 64 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                                If i = Cursor Then
                                    Me.SelectMenuEntry()
                                Else
                                    Cursor = i
                                End If
                            End If
                        Next
                    End If

                    If Controls.Accept(False, True, True) = True Then
                        Me.SelectMenuEntry()
                    End If
                Case ScreenStates.Idle
                    If Me.SelectedPokemonIndex = -1 Then
                        If Me.OpenedSelect = True Then
                            Me.CloseScreen()
                        Else
                            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf Me.SelectPokemonForTrade, "Choose Pokémon for Trade", True, True, False))
                            CType(Core.CurrentScreen, ChoosePokemonScreen).CanChooseHMPokemon = False
                            Me.OpenedSelect = True
                        End If
                    Else
                        Me.PerformTrade()
                    End If
                Case ScreenStates.Downloading
                    If Not Me.PartnerEmblem Is Nothing Then
                        If Me.PartnerEmblem.DoneLoading = True Then
                            Me.ScreenState = ScreenStates.Idle
                        End If
                    End If
                Case ScreenStates.Stopped
                    If KeyBoardHandler.GetPressedKeys().Count > 0 Or ControllerHandler.HasControlerInput() = True Or Controls.Accept() = True Or Controls.Dismiss() = True Then
                        CloseScreen()
                    End If
                Case ScreenStates.Trading
                    UpdateTrading()
                Case ScreenStates.PerformingTrade
                    If SaveGameHelpers.GameJoltSaveDone() = True And WaitForSave = True Then
                        SaveGameHelpers.ResetSaveCounter()
                        Me.ScreenState = ScreenStates.Trading
                        ownPokemonPosition = Core.windowSize.Height
                        tState = 0
                        messageDelay = 220
                        MusicManager.PlayMusic("evolution", True)
                    End If
            End Select
        End Sub

        Private Sub SelectMenuEntry()
            Select Case Me.Cursor
                Case 0
                    Me.ScreenState = ScreenStates.Downloading
                    Me.DownloadData()
                Case 1
                    CloseScreen()
            End Select
        End Sub

        Private Sub SelectPokemonForTrade(ByVal pokeIndex As Integer)
            Me.SelectedPokemonIndex = pokeIndex
            Me.SelectedPokemon = Core.Player.Pokemons(pokeIndex)
        End Sub

#Region "PerformTrade"

        Private Sub PerformTrade()
            Me.ScreenState = ScreenStates.PerformingTrade

            Core.Player.Pokemons(SelectedPokemonIndex).FullRestore()

            Dim APICall As New APICall(AddressOf PerformTradeStep1)
            APICall.GetStorageData("GTS_WONDERTRADE_V" & WONDERTRADE_VERSION, False)
        End Sub

        Private Sub PerformTradeStep1(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(1).Value.Replace("\""", """") = Me.GameJoltID & "|" & Me.WonderTradePokemonData Then
                Me.WonderTradePokemon = Pokemon.GetPokemonByData(Me.WonderTradePokemonData)
                Dim APICall1 As New APICall(AddressOf PerformTradeStep2)
                APICall1.GetStorageData("GTS_WONDERTRADE_TRADEID", False)
            Else
                Me.ScreenState = ScreenStates.Stopped
                Me.DisconnectMessage = "Trade not successful. Press any key to exit."
            End If
        End Sub

        Private Sub PerformTradeStep2(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(1).Value = Me.WonderTradeID.ToString() Then
                Logger.Debug("Step 2 success")
                Dim APICall As New APICall(AddressOf PerformTradeStep3)
                APICall.SetStorageData("GTS_WONDERTRADE_TRADEID", (Me.WonderTradeID + 1).ToString(), False)
            Else
                Me.ScreenState = ScreenStates.Stopped
                Me.DisconnectMessage = "Trade not successful. Press any key to exit."
            End If
        End Sub

        Private Sub PerformTradeStep3(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(0).Value.ToLower() = "true" Then
                Logger.Debug("Step 3 success")
                Dim APICall As New APICall(AddressOf PerformTradeStep4)
                APICall.SetStorageData("GTS_WONDERTRADE_V" & WONDERTRADE_VERSION, Core.GameJoltSave.GameJoltID & "|" & Core.Player.Pokemons(SelectedPokemonIndex).GetSaveData(), False)
            Else
                Me.ScreenState = ScreenStates.Stopped
                Me.DisconnectMessage = "Trade not successful. Press any key to exit."
            End If
        End Sub

        Private Sub PerformTradeStep4(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list(0).Value.ToLower() = "true" Then
                Logger.Debug("Step 4 success")
                WaitForSave = True
                Core.Player.Pokemons.RemoveAt(SelectedPokemonIndex)
                Core.Player.Pokemons.Add(Pokemon.GetPokemonByData(Me.WonderTradePokemonData))
                Core.Player.PokedexData = Pokedex.RegisterPokemon(Core.Player.PokedexData, Core.Player.Pokemons(Core.Player.Pokemons.Count - 1))
                PlayerStatistics.Track("Wondertrades", 1)
                Core.Player.SaveGame(False)
            Else
                Me.ScreenState = ScreenStates.Stopped
                Me.DisconnectMessage = "Trade not successful. Press any key to exit."
            End If
        End Sub

#End Region

#Region "DisplayTrade"

        Dim ownPokemonPosition As Integer = 0
        Dim oppPokemonPosition As Integer = 0
        Dim tState As Integer = 0
        Dim messageDelay As Integer = 220

        Private Sub DrawTrading()
            Select Case tState
                Case 0
                    Core.SpriteBatch.Draw(Me.SelectedPokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), ownPokemonPosition, 256, 256), Color.White)
                Case 1
                    Core.SpriteBatch.Draw(Me.SelectedPokemon.GetTexture(False), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), CInt(Core.windowSize.Height / 2 - 128), 256, 256), Color.White)

                    Dim t As String = "Sending " & Me.SelectedPokemon.GetDisplayName() & " to Wondertrade." & vbNewLine & "Good-bye, " & Me.SelectedPokemon.GetDisplayName() & "!"

                    Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CInt(Core.windowSize.Height / 2 + 130)), Color.White)
                Case 2
                    Core.SpriteBatch.Draw(Me.SelectedPokemon.GetTexture(False), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), ownPokemonPosition, 256, 256), Color.White)
                Case 3
                    Core.SpriteBatch.Draw(Me.WonderTradePokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), oppPokemonPosition, 256, 256), Color.White)
                Case 4
                    Core.SpriteBatch.Draw(Me.WonderTradePokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), CInt(Core.windowSize.Height / 2 - 128), 256, 256), Color.White)

                    Dim t As String = Me.PartnerEmblem.Username & " sent over " & Me.WonderTradePokemon.GetDisplayName() & "."

                    Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CInt(Core.windowSize.Height / 2 + 130)), Color.White)
            End Select
        End Sub

        Private Sub UpdateTrading()
            Select Case tState
                Case 0
                    If ownPokemonPosition > CInt(Core.windowSize.Height / 2 - 128) Then
                        ownPokemonPosition -= 4
                        If ownPokemonPosition <= CInt(Core.windowSize.Height / 2 - 128) Then
                            ownPokemonPosition = CInt(Core.windowSize.Height / 2 - 128)
                            tState = 1
                            SoundManager.PlayPokemonCry(SelectedPokemon.Number)
                        End If
                    End If
                Case 1
                    If messageDelay > 0 Then
                        messageDelay -= 1
                        If messageDelay <= 0 Then
                            messageDelay = 220
                            tState = 2
                        End If
                    End If
                Case 2
                    If ownPokemonPosition > -256 Then
                        ownPokemonPosition -= 4
                        If ownPokemonPosition <= -256 Then
                            ownPokemonPosition = -256
                            tState = 3
                            oppPokemonPosition = -256
                        End If
                    End If
                Case 3
                    If oppPokemonPosition < CInt(Core.windowSize.Height / 2 - 128) Then
                        oppPokemonPosition += 4
                        If oppPokemonPosition >= CInt(Core.windowSize.Height / 2 - 128) Then
                            oppPokemonPosition = CInt(Core.windowSize.Height / 2 - 128)
                            tState = 4
                            SoundManager.PlayPokemonCry(WonderTradePokemon.Number)
                        End If
                    End If
                Case 4
                    If messageDelay > 0 Then
                        messageDelay -= 1
                        If messageDelay = 180 Then
                            SoundManager.PlaySound("success", True)
                        End If
                        If messageDelay <= 0 Then
                            messageDelay = 220
                            EndTrade()
                        End If
                    End If
            End Select
        End Sub

        Private Sub EndTrade()
            Me.ScreenState = ScreenStates.Stopped
            Me.DisconnectMessage = "Trade successful. Press any button to exit."

            MusicManager.PlayMusic("gts", True)

            If Core.Player.Pokemons(Core.Player.Pokemons.Count - 1).CanEvolve(EvolutionCondition.EvolutionTrigger.Trading, "") = True Then
                Core.SetScreen(New EvolutionScreen(Me, {Core.Player.Pokemons.Count - 1}.ToList(), Me.SelectedPokemon.Number.ToString(), EvolutionCondition.EvolutionTrigger.Trading))
            End If

            Me.CanChat = True
        End Sub

#End Region

        Private Sub CloseScreen()
            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
        End Sub

        Public Overrides Sub ChangeTo()
            MusicManager.PlayMusic("gts", True)
        End Sub

    End Class

End Namespace