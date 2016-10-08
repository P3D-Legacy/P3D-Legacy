Namespace BattleSystem

    Public Class BattleMenu

        Enum MenuStates
            Main
            Moves
        End Enum

        Public MenuState As MenuStates = MenuStates.Main
        Public Visible As Boolean = False

        Public Sub New()
            Me.Reset()
        End Sub

        Public Sub Reset()
            _moveMenuAlpha = 255
            _moveMenuChoseMove = False
            MenuState = MenuStates.Main
        End Sub

        Private Sub DrawWeather(ByVal BattleScreen As BattleScreen)
            Dim y As Integer = -1
            Dim x As Integer = 0
            Dim t As String = ""

            Select Case BattleScreen.FieldEffects.Weather
                Case BattleWeather.WeatherTypes.Sunny
                    y = 0
                    t = "Sunny"
                Case BattleWeather.WeatherTypes.Rain
                    y = 34
                    t = "Rain"
                Case BattleWeather.WeatherTypes.Sandstorm
                    y = 68
                    t = "Sandstorm"
                Case BattleWeather.WeatherTypes.Hailstorm
                    y = 102
                    t = "Hailstorm"
                Case BattleWeather.WeatherTypes.Foggy
                    x = 88
                    y = 0
                    t = "Foggy"
                Case BattleWeather.WeatherTypes.Snow
                    x = 88
                    y = 34
                    t = "Snow"
            End Select

            If y > -1 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\WeatherIcons"), New Rectangle(22, Core.windowSize.Height - 90, 176, 68), New Rectangle(x, y, 88, 34), Color.White)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(110 - FontManager.MiniFont.MeasureString(t).X / 2, Core.windowSize.Height - 42), Color.Black)
            End If
        End Sub

        Private Sub DrawPokemonStats(ByVal pos As Vector2, ByVal p As Pokemon, ByVal BattleScreen As BattleScreen, ByVal largeStatsDisplay As Boolean, ByVal DrawCaught As Boolean)
            Dim shinyHue As Color = Color.White

            If p.IsShiny = True Then
                shinyHue = Color.Gold
            End If
            If _moveMenuChoseMove = True Then
                shinyHue.A = CByte(_moveMenuAlpha.Clamp(0, 255))
            End If

            'Draw large stats:
            If largeStatsDisplay = True Then
                'Background
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + 14, CInt(pos.Y) + 14, 182, 42), New Rectangle(0, 0, 91, 21), shinyHue)

                'Name:
                Dim nameInformation As String = p.GetDisplayName() & " Lv. " & p.Level.ToString()

                Core.SpriteBatch.DrawString(FontManager.MiniFont, nameInformation, New Vector2(pos.X + 2, pos.Y + 2), New Color(0, 0, 0, _moveMenuAlpha))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, nameInformation, New Vector2(pos.X, pos.Y), shinyHue)

                'Gender:
                If p.Gender = Pokemon.Genders.Male Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X + 6 + FontManager.MiniFont.MeasureString(nameInformation).X), CInt(pos.Y), 12, 20), New Rectangle(0, 104, 6, 10), New Color(255, 255, 255, _moveMenuAlpha))
                ElseIf p.Gender = Pokemon.Genders.Female Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X + 6 + FontManager.MiniFont.MeasureString(nameInformation).X), CInt(pos.Y), 12, 20), New Rectangle(6, 104, 6, 10), New Color(255, 255, 255, _moveMenuAlpha))
                End If

                'HP indicator:
                Core.SpriteBatch.DrawString(FontManager.MiniFont, p.HP & "/" & p.MaxHP, New Vector2(pos.X + 102, pos.Y + 37), New Color(0, 0, 0, _moveMenuAlpha))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, p.HP & "/" & p.MaxHP, New Vector2(pos.X + 100, pos.Y + 35), shinyHue)

                'EXP Bar:
                If BattleScreen.CanReceiveEXP = True Then
                    Dim NextLvExp As Integer = p.NeedExperience(p.Level + 1) - p.NeedExperience(p.Level)
                    Dim currentExp As Integer = p.Experience - p.NeedExperience(p.Level)
                    If p.Level = 1 Then
                        NextLvExp = p.NeedExperience(p.Level + 1)
                        currentExp = p.Experience
                    End If

                    Dim NeedExp As Integer = NextLvExp - currentExp

                    If p.Level = CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                        NextLvExp = 0
                    Else
                        Dim barPercentage As Integer = CInt((currentExp / NextLvExp) * 100)
                        If barPercentage > 0 Then
                            Dim EXPlength As Integer = CInt(Math.Ceiling(144 / 100 * barPercentage))

                            If currentExp = 0 Then
                                EXPlength = 0
                            Else
                                If EXPlength <= 0 Then
                                    EXPlength = 1
                                End If
                            End If
                            If EXPlength = 144 Then
                                EXPlength = 143
                            End If

                            Dim t = TextureManager.GetTexture("GUI\Battle\Interface")
                            For dX As Integer = 0 To EXPlength Step 4
                                Core.SpriteBatch.Draw(t, New Rectangle(CInt(pos.X) + 50 + dX, CInt(pos.Y) + 54, 4, 6), New Rectangle(0, 43, 2, 3), New Color(255, 255, 255, _moveMenuAlpha))
                            Next
                        End If
                    End If
                End If
            Else 'Smaller stats display
                'Background
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + 14, CInt(pos.Y) + 14, 182, 32), New Rectangle(0, 21, 91, 16), shinyHue)

                'Name:
                Dim nameInformation As String = p.GetDisplayName() & " Lv. " & p.Level.ToString()

                Core.SpriteBatch.DrawString(FontManager.MiniFont, nameInformation, New Vector2(pos.X + 2, pos.Y + 2), New Color(0, 0, 0, _moveMenuAlpha))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, nameInformation, New Vector2(pos.X, pos.Y), shinyHue)

                'Gender:
                If p.Gender = Pokemon.Genders.Male Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X + 6 + FontManager.MiniFont.MeasureString(nameInformation).X), CInt(pos.Y), 12, 20), New Rectangle(0, 104, 6, 10), New Color(255, 255, 255, _moveMenuAlpha))
                ElseIf p.Gender = Pokemon.Genders.Female Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X + 6 + FontManager.MiniFont.MeasureString(nameInformation).X), CInt(pos.Y), 12, 20), New Rectangle(6, 104, 6, 10), New Color(255, 255, 255, _moveMenuAlpha))
                End If
            End If

            Dim HPpercentage As Single = (100.0F / p.MaxHP) * p.HP
            Dim HPlength As Integer = CInt(Math.Ceiling(140 / 100 * HPpercentage.Clamp(1, 999)))

            If p.HP = 0 Then
                HPlength = 0
            Else
                If HPlength <= 0 Then
                    HPlength = 1
                End If
            End If
            If p.HP = p.MaxHP Then
                HPlength = 140
            Else
                If HPlength = 140 Then
                    HPlength = 139
                End If
            End If

            Dim cX As Integer = 0
            If HPpercentage <= 50.0F And HPpercentage > 15.0F Then
                cX = 2
            ElseIf HPpercentage <= 15.0F Then
                cX = 4
            End If

            If HPlength > 0 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + 54, CInt(pos.Y) + 26, 2, 12), New Rectangle(cX, 37, 1, 6), New Color(255, 255, 255, _moveMenuAlpha))
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + 2 + 54, CInt(pos.Y) + 26, HPlength - 4, 12), New Rectangle(cX + 1, 37, 1, 6), New Color(255, 255, 255, _moveMenuAlpha))
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + HPlength - 2 + 54, CInt(pos.Y) + 26, 2, 12), New Rectangle(cX, 37, 1, 6), New Color(255, 255, 255, _moveMenuAlpha))
            End If

            Dim caughtX As Integer = 0
            Dim StatusTexture As Texture2D = BattleStats.GetStatImage(p.Status)
            If Not StatusTexture Is Nothing Then
                Core.SpriteBatch.Draw(StatusTexture, New Rectangle(CInt(pos.X) + 10, CInt(pos.Y) + 26, 38, 12), New Color(255, 255, 255, _moveMenuAlpha))
                caughtX = -16
            End If

            If DrawCaught = True Then
                If Pokedex.GetEntryType(Core.Player.PokedexData, p.Number) > 1 Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + caughtX, CInt(pos.Y) + 22, 20, 20), New Rectangle(0, 46, 10, 10), New Color(255, 255, 255, _moveMenuAlpha))
                End If
            End If
        End Sub

        Private Sub DrawPokeBalls(ByVal pos As Vector2, ByVal BattleScreen As BattleScreen, ByVal PokemonList As List(Of Pokemon), ByVal Mirrored As Boolean)
            If Mirrored = True Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X), CInt(pos.Y), 160, 14), New Rectangle(128, 7, 80, 7), New Color(255, 255, 255, _moveMenuAlpha))
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X), CInt(pos.Y), 160, 14), New Rectangle(128, 0, 80, 7), New Color(255, 255, 255, _moveMenuAlpha))
            End If

            Dim mouseHovers As Boolean = False

            Dim startX As Integer = 12
            If Mirrored = True Then
                startX = 76
            End If

            For i = 0 To 5
                Dim texturePos As Integer = 0

                If PokemonList.Count - 1 >= i Then
                    Dim p As Pokemon = PokemonList(i)
                    If p.Status = Pokemon.StatusProblems.Fainted Then
                        texturePos = 10
                    ElseIf p.Status = Pokemon.StatusProblems.None Then
                        texturePos = 0
                    Else
                        texturePos = 30
                    End If

                    If MouseHandler.IsInRectangle(New Rectangle(CInt(pos.X) + startX + 12 * i, CInt(pos.Y) + 1, 10, 10)) = True And Mirrored = False And _moveMenuChoseMove = False Then
                        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + startX + 12 * i - 27, CInt(pos.Y) - 86, 64, 84), New Rectangle(128, 16, 32, 42), New Color(255, 255, 255, _mainMenuTeamPreviewAlpha))
                        Core.SpriteBatch.Draw(PokemonList(i).GetMenuTexture(True), New Rectangle(CInt(pos.X) + startX + 12 * i - 27, CInt(pos.Y) - 86, 64, 64), New Color(255, 255, 255, _mainMenuTeamPreviewAlpha))

                        If _mainMenuTeamPreviewAlpha < 255 Then
                            _mainMenuTeamPreviewAlpha += 25
                            If _mainMenuTeamPreviewAlpha > 255 Then
                                _mainMenuTeamPreviewAlpha = 255
                            End If
                        End If

                        _mainMenuTeamPreviewLastIndex = i

                        mouseHovers = True
                    End If
                Else
                    texturePos = 20
                End If
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + startX + 12 * i, CInt(pos.Y) + 1, 10, 10), New Rectangle(texturePos, 46, 10, 10), New Color(255, 255, 255, _moveMenuAlpha))
            Next

            If mouseHovers = False And Mirrored = False Then
                If _mainMenuTeamPreviewAlpha > 0 Then
                    _mainMenuTeamPreviewAlpha -= 25
                    If _mainMenuTeamPreviewAlpha < 0 Then
                        _mainMenuTeamPreviewAlpha = 0
                    End If
                    If _mainMenuTeamPreviewLastIndex > -1 Then
                        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + startX + 12 * _mainMenuTeamPreviewLastIndex - 27, CInt(pos.Y) - 86, 64, 84), New Rectangle(128, 16, 32, 42), New Color(255, 255, 255, _mainMenuTeamPreviewAlpha))
                        Core.SpriteBatch.Draw(PokemonList(_mainMenuTeamPreviewLastIndex).GetMenuTexture(True), New Rectangle(CInt(pos.X) + startX + 12 * _mainMenuTeamPreviewLastIndex - 27, CInt(pos.Y) - 86, 64, 64), New Color(255, 255, 255, _mainMenuTeamPreviewAlpha))
                    End If
                End If
            End If
        End Sub

        Public Sub Draw(ByVal BattleScreen As BattleScreen)
            If BattleScreen.IsCurrentScreen() = True Then
                DrawPokemonStats(New Vector2(50, 50), BattleScreen.OppPokemon, BattleScreen, False, Not BattleScreen.IsTrainerBattle)

                If BattleScreen.BattleMode <> BattleScreen.BattleModes.Safari Then
                    DrawPokemonStats(New Vector2(Core.windowSize.Width - 280, Core.windowSize.Height - 100), BattleScreen.OwnPokemon, BattleScreen, True, False)
                End If

                DrawPokeBalls(New Vector2(Core.windowSize.Width - 292, Core.windowSize.Height - 112), BattleScreen, Core.Player.Pokemons, False)
                If BattleScreen.IsTrainerBattle = True Then
                    DrawPokeBalls(New Vector2(38, 38), BattleScreen, BattleScreen.Trainer.Pokemons, True)
                End If

                Select Case MenuState
                    Case MenuStates.Main
                        DrawMainMenu(BattleScreen)
                    Case MenuStates.Moves
                        DrawMoveMenu(BattleScreen)
                End Select

                DrawWeather(BattleScreen)
            End If
        End Sub

        Public Sub Update(ByVal BattleScreen As BattleScreen)
            Select Case MenuState
                Case MenuStates.Main
                    UpdateMainMenu(BattleScreen)
                Case MenuStates.Moves
                    UpdateMoveMenu(BattleScreen)
            End Select
        End Sub

        Private _allItemsExtended As Integer = 0
        Private _selectedItemExtended As Integer = 0
        Private _nextMenuState As MenuStates = MenuStates.Main
        Private _retractMenu As Boolean = False

        Private _isRetracting As Boolean = False
        Private _isExtracting As Boolean = True

        Private _moveMenuIndex As Integer = 0
        Private _moveMenuNextIndex As Integer = 0
        Private _moveMenuItemList As New List(Of MoveMenuItem)
        Private _moveMenuCreatedID As String = ""
        Private _moveMenuAlpha As Integer = 255
        Private _moveMenuChoseMove As Boolean = False

        Private _mainMenuIndex As Integer = 0
        Private _mainMenuNextIndex As Integer = 0
        Private _mainMenuItemList As New List(Of MainMenuItem)
        Private _mainMenuTeamPreviewAlpha As Integer = 0
        Private _mainMenuTeamPreviewLastIndex As Integer = -1

        Public Delegate Sub D_MainMenuClick(ByVal BattleScreen As BattleScreen)
        Public Delegate Sub D_MoveMenuClick(ByVal BattleScreen As BattleScreen)

        Class MainMenuItem

            Private IconSelected As Texture2D
            Private IconUnselected As Texture2D
            Private IconFading As Integer

            Public Text As String
            Public Index As Integer

            Private Texture As Texture2D

            Private ClickAction As D_MainMenuClick

            Public Sub New(ByVal IconIndex As Integer, ByVal Text As String, ByVal Index As Integer, ByVal ClickAction As D_MainMenuClick)
                If IconIndex > 4 Then
                    Me.IconUnselected = TextureManager.GetTexture("GUI\Battle\Interface", New Rectangle(160 + (IconIndex - 5) * 24, 56, 24, 24), "")
                    Me.IconSelected = TextureManager.GetTexture("GUI\Battle\Interface", New Rectangle(160 + (IconIndex - 5) * 24, 80, 24, 24), "")
                Else
                    Me.IconUnselected = TextureManager.GetTexture("GUI\Battle\Interface", New Rectangle(IconIndex * 24, 56, 24, 24), "")
                    Me.IconSelected = TextureManager.GetTexture("GUI\Battle\Interface", New Rectangle(IconIndex * 24, 80, 24, 24), "")
                End If

                Me.IconFading = 0

                Me.Text = Text
                Me.Index = Index

                Me.Texture = TextureManager.GetTexture("GUI\Menus\General")

                Me.ClickAction = ClickAction
            End Sub

            Public Sub Draw(ByVal AllExtended As Integer, ByVal SelExtended As Integer, ByVal isSelected As Boolean)
                Dim extraExtended As Integer = 0
                If isSelected = True Then
                    Canvas.DrawGradient(New Rectangle(Core.ScreenSize.Width - 255, 100 + Index * 96, 255, 112), New Color(42, 167, 198, 0), New Color(42, 167, 198, (SelExtended + AllExtended)), True, -1)

                    extraExtended = SelExtended
                End If
                Core.SpriteBatch.Draw(Me.Texture, New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended), 116 + Index * 96, 80, 80), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.Texture, New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended) + 80, 116 + Index * 96, AllExtended + extraExtended - 80, 80), New Rectangle(32, 16, 16, 16), Color.White)

                Core.SpriteBatch.Draw(Me.IconUnselected, New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended) + 28, 132 + Index * 96, 48, 48), Color.White)
                If isSelected = True Then
                    Core.SpriteBatch.Draw(Me.IconSelected, New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended) + 28, 132 + Index * 96, 48, 48), New Color(255, 255, 255, (SelExtended + AllExtended)))
                    Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Text, New Vector2(Core.ScreenSize.Width - (AllExtended + extraExtended) + 86, 144 + Index * 96), New Color(0, 0, 0, (SelExtended + AllExtended)))
                Else
                    If IconFading > 0 Then
                        Core.SpriteBatch.Draw(Me.IconSelected, New Rectangle(Core.ScreenSize.Width - (AllExtended) + 28, 132 + Index * 96, 48, 48), New Color(255, 255, 255, IconFading))
                    End If
                End If
            End Sub

            Public Sub Update(ByVal BattleScreen As BattleScreen, ByVal AllExtended As Integer, ByVal isSelected As Boolean)
                Me.Activate(BattleScreen, AllExtended, isSelected)
                If isSelected = False Then
                    If MouseHandler.IsInRectangle(New Rectangle(Core.ScreenSize.Width - (AllExtended) + 28, 132 + Index * 96, 48, 48)) = True Then
                        If IconFading < 255 Then
                            IconFading += 15
                            If IconFading > 255 Then
                                IconFading = 255
                            End If
                        End If
                    Else
                        If IconFading > 0 Then
                            IconFading -= 15
                            If IconFading < 0 Then
                                IconFading = 0
                            End If
                        End If
                    End If
                Else
                    IconFading = 255
                End If
            End Sub

            Public Sub Activate(ByVal BattleScreen As BattleScreen, ByVal AllExtended As Integer, ByVal isSelected As Boolean)
                If BattleScreen.BattleMenu._isExtracting = False And BattleScreen.BattleMenu._isRetracting = False Then
                    If Controls.Accept(False, True, True) = True And isSelected = True Then
                        Me.ClickAction(BattleScreen)
                    End If
                    If Controls.Accept(True, False, False) = True Then
                        If MouseHandler.IsInRectangle(New Rectangle(Core.ScreenSize.Width - 255, 116 + Index * 96, 255, 80)) = True Then
                            If isSelected = True Then
                                Me.ClickAction(BattleScreen)
                            Else
                                BattleScreen.BattleMenu._mainMenuNextIndex = Me.Index
                                BattleScreen.BattleMenu._isRetracting = True
                            End If
                        End If
                    End If
                End If
            End Sub

        End Class

        Class MoveMenuItem

            Private Move As Attack
            Public Index As Integer = 0

            Private Texture As Texture2D

            Private ClickAction As D_MoveMenuClick

            Public Sub New(ByVal Index As Integer, ByVal Move As Attack, ByVal ClickAction As D_MoveMenuClick)
                Me.Index = Index
                Me.Move = Move
                Me.ClickAction = ClickAction
                Me.Texture = TextureManager.GetTexture("GUI\Menus\General")
            End Sub

            Public Sub Draw(ByVal AllExtended As Integer, ByVal SelExtended As Integer, ByVal isSelected As Boolean, ByVal BattleScreen As BattleScreen)
                Dim deductAlpha As Integer = 255 - BattleScreen.BattleMenu._moveMenuAlpha

                Dim extraExtended As Integer = 0
                If isSelected = True Then
                    Canvas.DrawGradient(New Rectangle(Core.ScreenSize.Width - 255, 100 + Index * 96, 255, 112), New Color(42, 167, 198, 0), New Color(42, 167, 198, (SelExtended + AllExtended) - deductAlpha), True, -1)

                    extraExtended = SelExtended
                End If
                Core.SpriteBatch.Draw(Me.Texture, New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended), 116 + Index * 96, 80, 80), New Rectangle(16, 16, 16, 16), New Color(255, 255, 255, 255 - deductAlpha))
                Core.SpriteBatch.Draw(Me.Texture, New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended) + 80, 116 + Index * 96, AllExtended + extraExtended - 80, 80), New Rectangle(32, 16, 16, 16), New Color(255, 255, 255, 255 - deductAlpha))

                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types", Me.Move.Type.GetElementImage(), ""), New Rectangle(Core.ScreenSize.Width - (AllExtended + extraExtended) + 28, 132 + Index * 96, 48, 16), New Color(255, 255, 255, 255 - deductAlpha))

                If isSelected = True Then
                    Dim ppColor As Color = GetPPColor()
                    ppColor.A = CByte((extraExtended + AllExtended - deductAlpha).Clamp(0, 255))

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, Me.Move.CurrentPP & "/" & Me.Move.MaxPP, New Vector2(Core.ScreenSize.Width - (AllExtended + extraExtended) + 28, 150 + Index * 96), ppColor)
                    Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Move.Name, New Vector2(Core.ScreenSize.Width - (AllExtended + extraExtended) + 86, 144 + Index * 96), New Color(0, 0, 0, (SelExtended + AllExtended) - deductAlpha))
                Else
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, Me.Move.Name, New Vector2(Core.ScreenSize.Width - (AllExtended + extraExtended) + 28, 150 + Index * 96), New Color(0, 0, 0, 255 - (extraExtended + AllExtended) - deductAlpha))
                End If
            End Sub

            Private Function GetPPColor() As Color
                Dim c As Color = Color.Black
                Dim per As Integer = CInt((Me.Move.CurrentPP / Me.Move.MaxPP) * 100)

                If per <= 50 And per > 25 Then
                    c = Color.Orange
                ElseIf per <= 25 Then
                    c = Color.IndianRed
                End If

                Return c
            End Function

            Public Sub Update(ByVal BattleScreen As BattleScreen, ByVal AllExtended As Integer, ByVal isSelected As Boolean)
                Me.Activate(BattleScreen, AllExtended, isSelected)
            End Sub

            Public Sub Activate(ByVal BattleScreen As BattleScreen, ByVal AllExtended As Integer, ByVal isSelected As Boolean)
                If BattleScreen.BattleMenu._isExtracting = False And BattleScreen.BattleMenu._isRetracting = False Then
                    If Me.Move.CurrentPP > 0 Or isSelected = False Then
                        If Controls.Accept(False, True, True) = True And isSelected = True Then
                            Me.ClickAction(BattleScreen)
                        End If
                        If Controls.Accept(True, False, False) = True Then
                            If MouseHandler.IsInRectangle(New Rectangle(Core.ScreenSize.Width - 255, 116 + Index * 96, 255, 80)) = True Then
                                If isSelected = True Then
                                    Me.ClickAction(BattleScreen)
                                Else
                                    BattleScreen.BattleMenu._moveMenuNextIndex = Me.Index
                                    BattleScreen.BattleMenu._isRetracting = True
                                End If
                            End If
                        End If
                    End If
                End If
            End Sub

        End Class

        Private Sub UpdateMenuOptions(ByRef MenuIndex As Integer, ByRef NextMenuIndex As Integer, ByVal ItemListCount As Integer)
            Dim l_canSelect As Boolean = True

            If _retractMenu = False Then
                If _isRetracting = True Then
                    If _selectedItemExtended > 0 Then
                        _selectedItemExtended -= 40
                        If _selectedItemExtended <= 0 Then
                            _selectedItemExtended = 0
                            _isRetracting = False
                            MenuIndex = NextMenuIndex
                            _isExtracting = True
                        End If
                    End If

                    l_canSelect = False
                ElseIf _isExtracting = True Then
                    If _selectedItemExtended < 175 Then
                        _selectedItemExtended += 40
                        If _selectedItemExtended >= 175 Then
                            _selectedItemExtended = 175
                            _isExtracting = False
                        End If
                    End If

                    l_canSelect = False
                End If
            End If

            If _retractMenu = True Then
                If _allItemsExtended > 0 Then
                    _allItemsExtended -= 5
                    _selectedItemExtended -= 40
                    If _allItemsExtended <= 0 Then
                        _allItemsExtended = 0
                        _selectedItemExtended = 0
                        _retractMenu = False
                        MenuState = _nextMenuState
                        _isExtracting = True
                    End If
                End If

                l_canSelect = False
            ElseIf _allItemsExtended < 80 Then
                _allItemsExtended += 5

                l_canSelect = False
            End If

            If l_canSelect = True Then
                If Controls.Down(True) = True Then
                    NextMenuIndex = MenuIndex + 1

                    If NextMenuIndex = ItemListCount Then
                        NextMenuIndex = 0
                    End If

                    _isRetracting = True
                End If
                If Controls.Up(True) = True Then
                    NextMenuIndex = MenuIndex - 1

                    If NextMenuIndex = -1 Then
                        NextMenuIndex = ItemListCount - 1
                    End If

                    _isRetracting = True
                End If
            End If
        End Sub

#Region "MainMenu"

        Private Sub DrawMainMenu(ByVal BattleScreen As BattleScreen)
            For Each m As MainMenuItem In _mainMenuItemList
                m.Draw(_allItemsExtended, _selectedItemExtended, (m.Index = _mainMenuIndex))
            Next
        End Sub

        Private Sub UpdateMainMenu(ByVal BattleScreen As BattleScreen)
            If _mainMenuItemList.Count = 0 Then
                CreateMainMenuItems(BattleScreen)
            End If
            If _retractMenu = False Then
                For Each m As MainMenuItem In _mainMenuItemList
                    m.Update(BattleScreen, _allItemsExtended, (m.Index = _mainMenuIndex))
                Next
            End If

            UpdateMenuOptions(_mainMenuIndex, _mainMenuNextIndex, _mainMenuItemList.Count)
            If BattleScreen.ClearMenuTime = True Then
                _mainMenuItemList.Clear()
                BattleScreen.ClearMenuTime = False
            End If
        End Sub

        Private Sub CreateMainMenuItems(ByVal BattleScreen As BattleScreen)
            _mainMenuItemList.Clear()

            Select Case BattleScreen.BattleMode
                Case BattleSystem.BattleScreen.BattleModes.Safari
                    Dim safariBallText As String = "Safari Ball x" & Core.Player.Inventory.GetItemAmount(181).ToString()
                    If Core.Player.Inventory.GetItemAmount(181) = 0 Then
                        safariBallText = "No Safari Balls."
                    End If
                    _mainMenuItemList.Add(New MainMenuItem(4, safariBallText, 0, AddressOf MainMenuUseSafariBall))
                    _mainMenuItemList.Add(New MainMenuItem(0, "Throw Mud", 1, AddressOf MainMenuThrowMud))
                    _mainMenuItemList.Add(New MainMenuItem(0, "Throw Bait", 2, AddressOf MainMenuThrowBait))

                    _mainMenuItemList.Add(New MainMenuItem(3, "Run", 3, AddressOf MainMenuRun))

                Case BattleSystem.BattleScreen.BattleModes.BugContest
                    _mainMenuItemList.Add(New MainMenuItem(0, "Battle", 0, AddressOf MainMenuOpenBattleMenu))

                    Dim sportBallText As String = "Sport Ball x" & Core.Player.Inventory.GetItemAmount(177).ToString()
                    If Core.Player.Inventory.GetItemAmount(177) = 0 Then
                        sportBallText = "No Sport Balls."
                    End If
                    _mainMenuItemList.Add(New MainMenuItem(4, sportBallText, 1, AddressOf MainMenuUseSportBall))
                    _mainMenuItemList.Add(New MainMenuItem(1, "Pokémon", 2, AddressOf MainMenuOpenPokemon))
                    _mainMenuItemList.Add(New MainMenuItem(3, "Run", 3, AddressOf MainMenuRun))

                Case BattleSystem.BattleScreen.BattleModes.Standard
                    _mainMenuItemList.Add(New MainMenuItem(0, "Battle", 0, AddressOf MainMenuOpenBattleMenu))
                    _mainMenuItemList.Add(New MainMenuItem(1, "Pokémon", 1, AddressOf MainMenuOpenPokemon))
                    _mainMenuItemList.Add(New MainMenuItem(2, "Bag", 2, AddressOf MainMenuOpenBag))

                    If BattleScreen.IsTrainerBattle = False Then
                        _mainMenuItemList.Add(New MainMenuItem(3, "Run", 3, AddressOf MainMenuRun))
                        MainMenuAddMegaEvolution(BattleScreen, 4)
                    Else
                        MainMenuAddMegaEvolution(BattleScreen, 3)
                    End If

                Case BattleSystem.BattleScreen.BattleModes.PVP
                    _mainMenuItemList.Add(New MainMenuItem(0, "Battle", 0, AddressOf MainMenuOpenBattleMenu))
                    _mainMenuItemList.Add(New MainMenuItem(1, "Pokémon", 1, AddressOf MainMenuOpenPokemon))
                    _mainMenuItemList.Add(New MainMenuItem(3, "Surrender", 2, AddressOf MainMenuOpenBag))
            End Select
        End Sub

        Private Sub MainMenuAddMegaEvolution(ByVal BattleScreen As BattleScreen, ByVal Index As Integer)
            If _mainMenuIndex >= 3 Then
                _mainMenuIndex = 0
            End If

            For i = 0 To Core.Player.Pokemons.Count - 1
                Dim _str As String = Core.Player.Pokemons(i).AdditionalData
                Select Case _str
                    Case "mega", "mega_x", "mega_y"
                        Exit Sub
                    Case Else
                        'do nothing
                End Select
            Next
            Dim PokeIndex As Integer = BattleScreen.OwnPokemonIndex
            If BattleScreen.FieldEffects.OwnMegaEvolved = False Then
                If Not Core.Player.Pokemons(PokeIndex).Item Is Nothing Then
                    If Core.Player.Pokemons(PokeIndex).Item.IsMegaStone = True Then
                        Dim megaStone = CType(Core.Player.Pokemons(PokeIndex).Item, Items.MegaStone)

                        If megaStone.MegaPokemonNumber = Core.Player.Pokemons(PokeIndex).Number Then
                            _mainMenuItemList.Add(New MainMenuItem(5, "Mega Evolve!", Index, AddressOf MainMenuMegaEvolve))
                        End If
                    End If
                End If
            End If
        End Sub

#End Region

#Region "MainMenuActions"

        Private Sub MainMenuOpenBattleMenu(ByVal BattleScreen As BattleScreen)
            _retractMenu = True
            _nextMenuState = MenuStates.Moves

            BattleScreen.BattleQuery.Clear()
            Dim q As New CameraQueryObject(New Vector3(11, 0.5F, 14.0F), New Vector3(11, 0.5F, 14.0F), Screen.Camera.Speed, Screen.Camera.Speed, -(CSng(MathHelper.PiOver4) + 0.3F), -(CSng(MathHelper.PiOver4) + 0.3F), -0.3F, -0.3F, 0.04F, 0.04F)
            BattleScreen.BattleQuery.AddRange({q})
        End Sub

        Private Sub MainMenuOpenPokemon(ByVal BattleScreen As BattleScreen)
            TempBattleScreen = BattleScreen
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf ShowPokemonMenu, "Choose Pokémon", True))
            CType(Core.CurrentScreen, ChoosePokemonScreen).index = BattleScreen.OwnPokemonIndex
        End Sub

        Private Sub MainMenuOpenBag(ByVal BattleScreen As BattleScreen)
            If BattleScreen.CanUseItems = True Then
                TempBattleScreen = BattleScreen
                Core.SetScreen(New InventoryScreen(Core.CurrentScreen, {}, AddressOf SelectedItem))
            End If
        End Sub

        Private Sub MainMenuRun(ByVal BattleScreen As BattleScreen)
            If BattleCalculation.CanRun(True, BattleScreen) = True And BattleScreen.CanRun = True Then
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                BattleScreen.BattleQuery.Add(BattleScreen.FocusOwnPlayer())
                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\running", False))
                BattleScreen.BattleQuery.Add(New TextQueryObject("Got away safely!"))
                BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                Battle.Won = True
                Battle.Fled = True
            Else
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                BattleScreen.Battle.InitializeRound(BattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = "Failed to run away."})
            End If
        End Sub

        Private Sub MainMenuUseSafariBall(ByVal BattleScreen As BattleScreen)
            If Core.Player.Inventory.GetItemAmount(181) > 0 Then
                Core.Player.Inventory.RemoveItem(181, 1)
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                Core.SetScreen(New BattleCatchScreen(BattleScreen, Item.GetItemByID(181)))

                Dim safariBallText As String = "Safari Ball x" & Core.Player.Inventory.GetItemAmount(181).ToString()
                If Core.Player.Inventory.GetItemAmount(181) = 0 Then
                    safariBallText = "No Safari Balls."
                End If
                _mainMenuItemList(0).Text = safariBallText
            End If
        End Sub

        Private Sub MainMenuUseSportBall(ByVal BattleScreen As BattleScreen)
            If Core.Player.Inventory.GetItemAmount(177) > 0 Then
                Core.Player.Inventory.RemoveItem(177, 1)
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                Core.SetScreen(New BattleCatchScreen(BattleScreen, Item.GetItemByID(177)))

                Dim sportBallText As String = "Sport Ball x" & Core.Player.Inventory.GetItemAmount(177).ToString()
                If Core.Player.Inventory.GetItemAmount(177) = 0 Then
                    sportBallText = "No Sport Balls."
                End If
                _mainMenuItemList(0).Text = sportBallText
            End If
        End Sub

        Private Sub MainMenuThrowMud(ByVal BattleScreen As BattleScreen)
            If BattleScreen.PokemonSafariStatus > 0 Then
                BattleScreen.PokemonSafariStatus = 0
            End If
            BattleScreen.PokemonSafariStatus -= Core.Random.Next(2, 5)
            BattleScreen.BattleQuery.Clear()
            BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
            BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
            BattleScreen.Battle.InitializeRound(BattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = "Threw Mud at " & BattleScreen.OppPokemon.GetDisplayName() & "!"})
        End Sub

        Private Sub MainMenuThrowBait(ByVal BattleScreen As BattleScreen)
            If BattleScreen.PokemonSafariStatus < 0 Then
                BattleScreen.PokemonSafariStatus = 0
            End If
            BattleScreen.PokemonSafariStatus += Core.Random.Next(2, 5)
            BattleScreen.BattleQuery.Clear()
            BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
            BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
            BattleScreen.Battle.InitializeRound(BattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = "Threw a Bait at " & BattleScreen.OppPokemon.GetDisplayName() & "!"})
        End Sub

        Private Sub MainMenuMegaEvolve(ByVal BattleScreen As BattleScreen)
            _retractMenu = True
            _nextMenuState = MenuStates.Moves

            BattleScreen.BattleQuery.Clear()
            Dim q As New CameraQueryObject(New Vector3(11, 0.5F, 14.0F), New Vector3(11, 0.5F, 14.0F), Screen.Camera.Speed, Screen.Camera.Speed, -(CSng(MathHelper.PiOver4) + 0.3F), -(CSng(MathHelper.PiOver4) + 0.3F), -0.3F, -0.3F, 0.04F, 0.04F)
            BattleScreen.BattleQuery.AddRange({q})

            BattleScreen.IsMegaEvolvingOwn = True
            For i = 0 To Core.Player.Pokemons.Count - 1

                Dim _additionalData As String = Core.Player.Pokemons(i).AdditionalData
                Select Case _additionalData
                    Case "mega", "mega_x", "mega_y"
                        BattleScreen.IsMegaEvolvingOwn = False
                    Case Else
                        'do nothing
                End Select
            Next
        End Sub
#End Region

#Region "MoveMenu"

        Private Sub DrawMoveMenu(ByVal BattleScreen As BattleScreen)
            For Each m As MoveMenuItem In _moveMenuItemList
                m.Draw(_allItemsExtended, _selectedItemExtended, (m.Index = _moveMenuIndex), BattleScreen)
            Next
        End Sub

        Private Sub UpdateMoveMenu(ByVal BattleScreen As BattleScreen)
            If _moveMenuChoseMove = True Then
                _moveMenuAlpha -= 15
                If _moveMenuAlpha <= 0 Then
                    _moveMenuAlpha = 0
                    MoveMenuStartRound(BattleScreen)
                    Visible = False
                End If
            Else
                UseStruggle(BattleScreen)

                If _moveMenuItemList.Count = 0 Or _moveMenuCreatedID <> BattleScreen.OwnPokemon.IndividualValue Then
                    If _moveMenuCreatedID <> BattleScreen.OwnPokemon.IndividualValue Then
                        _moveMenuIndex = 0
                    End If
                    CreateMoveMenuItems(BattleScreen)
                    _moveMenuCreatedID = BattleScreen.OwnPokemon.IndividualValue
                End If
                If _retractMenu = False Then
                    For Each m As MoveMenuItem In _moveMenuItemList
                        m.Update(BattleScreen, _allItemsExtended, (m.Index = _moveMenuIndex))
                    Next
                End If

                UpdateMenuOptions(_moveMenuIndex, _moveMenuNextIndex, _moveMenuItemList.Count)

                If Controls.Dismiss(True, True, True) = True And _retractMenu = False And _isExtracting = False And _isRetracting = False Then
                    _retractMenu = True
                    _nextMenuState = MenuStates.Main

                    BattleScreen.BattleQuery.Clear()
                    For i = 0 To 99
                        BattleScreen.InsertCasualCameramove()
                    Next

                    Dim fQ As CameraQueryObject = CType(BattleScreen.BattleQuery(0), CameraQueryObject)

                    Dim q As New CameraQueryObject(fQ.StartPosition, Screen.Camera.Position, 0.06F, 0.06F, fQ.StartYaw, Screen.Camera.Yaw, fQ.TargetPitch, Screen.Camera.Pitch, 0.06F, 0.06F)
                    BattleScreen.BattleQuery.Insert(0, q)

                    BattleScreen.IsMegaEvolvingOwn = False
                End If

                If BattleScreen.BattleQuery.Count = 0 Then
                    For i = 0 To 50
                        Dim q As New CameraQueryObject(New Vector3(11.5, 0.5F, 13.0F), Screen.Camera.Position, 0.001F, Screen.Camera.Yaw, Screen.Camera.Pitch)
                        Dim q1 As New CameraQueryObject(New Vector3(11, 0.5F, 14.0F), New Vector3(11.5, 0.5F, 13.0F), 0.001F, Screen.Camera.Yaw, Screen.Camera.Pitch)
                        BattleScreen.BattleQuery.AddRange({q, q1})
                    Next
                End If
            End If
        End Sub

        Private Sub CreateMoveMenuItems(ByVal BattleScreen As BattleScreen)
            _moveMenuItemList.Clear()

            For i = 0 To BattleScreen.OwnPokemon.Attacks.Count - 1
                _moveMenuItemList.Add(New MoveMenuItem(i, BattleScreen.OwnPokemon.Attacks(i), AddressOf MoveMenuChooseMove))
            Next
        End Sub

        Private Sub MoveMenuChooseMove(ByVal BattleScreen As BattleScreen)
            _moveMenuChoseMove = True
            _moveMenuAlpha = 255
        End Sub

        Private Sub MoveMenuStartRound(ByVal BattleScreen As BattleScreen)
            If BattleScreen.IsRemoteBattle = True And BattleScreen.IsHost = False Then
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                If BattleScreen.IsMegaEvolvingOwn Then
                    BattleScreen.SendClientCommand("MEGA|" & BattleScreen.OwnPokemon.Attacks(_moveMenuIndex).ID.ToString())
                Else
                    BattleScreen.SendClientCommand("MOVE|" & BattleScreen.OwnPokemon.Attacks(_moveMenuIndex).ID.ToString())
                End If
            Else
                BattleScreen.OwnStatistics.Moves += 1
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                BattleScreen.Battle.InitializeRound(BattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Move, .Argument = BattleScreen.OwnPokemon.Attacks(_moveMenuIndex)})
            End If
            _moveMenuChoseMove = False
            _moveMenuAlpha = 255
            _moveMenuItemList.Clear()
        End Sub

        Private Sub UseStruggle(ByVal BattleScreen As BattleScreen)
            If BattleScreen.OwnPokemon.CountPP() <= 0 Then 'Use Struggle
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Add(BattleScreen.FocusBattle())
                BattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                BattleScreen.Battle.InitializeRound(BattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Move, .Argument = Attack.GetAttackByID(165)})
            End If
        End Sub

#End Region

#Region "SwitchPokemon"

        Shared TempBattleScreen As BattleScreen

        Private Shared Sub ShowPokemonMenu(ByVal PokeIndex As Integer)
            Core.SetScreen(New BattlePokemonInfoScreen(Core.CurrentScreen, PokeIndex, AddressOf SwitchPokemonTo, TempBattleScreen))
        End Sub

        Private Shared Sub SwitchPokemonTo(ByVal PokeIndex As Integer)
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If PokeIndex = TempBattleScreen.OwnPokemonIndex Then
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is already~in battle!", {}, True, False)
            Else
                If Pokemon.IsEgg() = False Then
                    If Pokemon.Status <> net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                        If BattleCalculation.CanSwitch(TempBattleScreen, True) = False Then
                            Screen.TextBox.Show("Cannot switch out.", {}, True, False)
                        Else
                            If TempBattleScreen.OwnPokemonIndex <> PokeIndex Then
                                If TempBattleScreen.IsRemoteBattle = True And TempBattleScreen.IsHost = False Then
                                    TempBattleScreen.OwnStatistics.Switches += 1
                                    TempBattleScreen.BattleQuery.Clear()
                                    TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                                    TempBattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                                    TempBattleScreen.SendClientCommand("SWITCH|" & PokeIndex.ToString())
                                Else
                                    TempBattleScreen.BattleQuery.Clear()
                                    TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                                    TempBattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                                    TempBattleScreen.Battle.InitializeRound(TempBattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Switch, .Argument = PokeIndex.ToString()})
                                End If
                            End If
                        End If
                    Else
                        Screen.TextBox.Show(Pokemon.GetDisplayName() & " is fainted!", {}, True, False)
                    End If
                Else
                    Screen.TextBox.Show("Cannot switch in~the egg!", {}, True, False)
                End If
            End If
        End Sub

#End Region

#Region "UseItem"

        Private Shared Sub SelectedItem(ByVal itemID As Integer)
            Dim Item As Item = Item.GetItemByID(itemID)

            If Item.CanBeUsedInBattle = True Then
                If Item.IsBall = True Then
                    Core.Player.Inventory.RemoveItem(itemID, 1)
                    If TempBattleScreen.IsTrainerBattle = False Then
                        If BattleScreen.CanCatch = True Or CBool(GameModeManager.GetGameRuleValue("OnlyCaptureFirst", "0")) = True And Core.Player.PokeFiles.Contains(BattleScreen.TempPokeFile) = True Then
                            TempBattleScreen.BattleQuery.Clear()
                            TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                            TempBattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                            Core.SetScreen(New BattleCatchScreen(TempBattleScreen, Item.GetItemByID(itemID)))
                            PlayerStatistics.Track("[4]Poké Balls used", 1)
                        Else
                            TempBattleScreen.BattleQuery.Clear()
                            TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                            TempBattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                            TempBattleScreen.Battle.InitializeRound(TempBattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = "The wild Pokémon blocked the Pokéball!"})
                        End If
                    Else
                        TempBattleScreen.BattleQuery.Clear()
                        TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                        TempBattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                        TempBattleScreen.Battle.InitializeRound(TempBattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = "Hey! Don't be a thief!"})
                    End If
                Else
                    TempItemID = itemID

                    If Item.BattleSelectPokemon = True Then
                        Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item, AddressOf UseItem, "Use " & Item.Name, True))
                    Else
                        UseItem(0)
                    End If
                End If
            End If
        End Sub

        Shared TempItemID As Integer = -1

        Private Shared Sub UseItem(ByVal PokeIndex As Integer)
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            Dim Item As Item = Item.GetItemByID(TempItemID)

            TempBattleScreen.BattleQuery.Clear()
            If Item.UseOnPokemon(PokeIndex) = True Then
                TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                TempBattleScreen.BattleQuery.Insert(0, New ToggleMenuQueryObject(True))
                TempBattleScreen.Battle.InitializeRound(TempBattleScreen, New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Item, .Argument = TempItemID.ToString()})
            End If
        End Sub

#End Region

    End Class

End Namespace
