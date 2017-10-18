''' <summary>
''' The "Press {BUTTON} to start" screen.
''' </summary>
Public Class PressStartScreen

    Inherits Screen

    Private _fadeIn As Single = 1.0F
    Private _introDelay As Single = 4.0F
    Private _logoFade As Single = 1.0F

    'Private _mainMenuMaps As DataModel.Json.Game.MainMenuMapModel()

    Private _logoTexture As Texture2D = Nothing
    Private _shineTexture As Texture2D = Nothing

    Private _logoRenderer As SpriteBatch
    Private _shineRenderer As SpriteBatch
    Private _backgroundRenderer As SpriteBatch

    Private target As RenderTarget2D

    Dim tempF As Single = 0F
    Dim tempG As Single = 0F

    Public Sub New()
        Identification = Identifications.PressStartScreen
        CanBePaused = False
        MouseVisible = True
        CanChat = False

        TextBox.Showing = False
        PokemonImageView.Showing = False
        ChooseBox.Showing = False

        GameModeManager.SetGameModePointer("Kolben")

        Effect = New BasicEffect(GraphicsDevice)
        Effect.FogEnabled = True

        SkyDome = New SkyDome()
        Camera = New MainMenuCamera()

        Core.Player.Skin = "Hilbert"
        Level = New Level()
        ChangeLevel()
        Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

        If IO.Directory.Exists(GameController.GamePath & "\Save\") = False Then
            IO.Directory.CreateDirectory(GameController.GamePath & "\Save\")
        End If

        GameJolt.Emblem.ClearOnlineSpriteCache()

        _logoTexture = TextureManager.GetTexture("GUI\Logos\Pokemon_Small")
        _shineTexture = TextureManager.GetTexture("GUI\Logos\logo_shine")

        _logoRenderer = New SpriteBatch(GraphicsDevice)
        _shineRenderer = New SpriteBatch(GraphicsDevice)
        _backgroundRenderer = New SpriteBatch(GraphicsDevice)

        target = New RenderTarget2D(GraphicsDevice, windowSize.Width, windowSize.Height)

        'crappy fix
        Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen.SelectedSkin = ""
        Core.Player.Unload()
    End Sub

    Public Overrides Sub Update()
        Lighting.UpdateLighting(Effect)

        Camera.Update()
        Level.Update()
        SkyDome.Update()
        Level.World.Initialize(Level.EnvironmentType, Level.WeatherType)

        If _introDelay > 0F Then
            _introDelay -= 0.1F
            If _introDelay <= 0F Then
                _introDelay = 0F
            End If
        Else
            If IsCurrentScreen() Then
                If _fadeIn > 0.0F Then
                    _fadeIn = MathHelper.Lerp(0.0F, _fadeIn, 0.93F)
                    If _fadeIn - 0.01F <= 0.0F Then
                        _fadeIn = 0.0F
                    End If
                End If
            Else
                If _logoFade > 0.0F Then
                    _logoFade = MathHelper.Lerp(0.0F, _logoFade, 0.2F)
                    If _logoFade - 0.01F <= 0.0F Then
                        _logoFade = 0.0F
                    End If
                End If
            End If

            tempF += 0.01F
            tempG += 0.04F

            If IsCurrentScreen() Then
                If KeyBoardHandler.KeyPressed(KeyBindings.EnterKey1) Or KeyBoardHandler.KeyPressed(KeyBindings.EnterKey2) Or ControllerHandler.ButtonPressed(Buttons.A) Or MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) Then
                    SetScreen(New NewMainMenuScreen(Me))
                End If
            End If
        End If
    End Sub

    Private _blurHandler As Resources.GaussianEffect

    Public Overrides Sub Draw()
        If _blurHandler Is Nothing Then
            _blurHandler = New Resources.GaussianEffect(windowSize.Width, windowSize.Height)
        End If

        GraphicsDevice.SetRenderTarget(target)
        GraphicsDevice.Clear(BackgroundColor)

        SkyDome.Draw(45.0F)
        Level.Draw()

        GraphicsDevice.SetRenderTarget(Nothing)

        _backgroundRenderer.Begin()
        _backgroundRenderer.Draw(_blurHandler.Perform(target), New Rectangle(0, 0, windowSize.Width, windowSize.Height), Color.White)
        _backgroundRenderer.End()

        If IsCurrentScreen() Then
            _logoRenderer.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone)
        Else
            _logoRenderer.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone)
        End If
        _shineRenderer.Begin(SpriteSortMode.Texture, BlendState.Additive)

        _logoRenderer.Draw(_logoTexture, New Rectangle(CInt(ScreenSize.Width / 2) - 400, CInt(160 * _fadeIn), 768, 282), New Color(255, 255, 255, CInt(255 * _logoFade)))
        _shineRenderer.Draw(_shineTexture, New Rectangle(CInt(ScreenSize.Width / 2 - 250 + Math.Sin(tempF) * 240.0F), CInt(-100 + Math.Sin(tempG) * 10.0F + CInt(160 * _fadeIn)), 512, 512), New Color(255, 255, 255, CInt(255 * _logoFade)))

        If _fadeIn = 0F Then
            If IsCurrentScreen() And CInt(Date.Now.Millisecond / 500) = 1 Then
                Dim text As String = String.Empty
                If ControllerHandler.IsConnected() Then
                    text = "Press      to start"
                Else
                    text = "Press " & KeyBindings.EnterKey1.ToString() & " or click to start"
                End If

                Dim textSize As Vector2 = FontManager.GameJoltFont.MeasureString(text)

                GetFontRenderer().DrawString(FontManager.GameJoltFont, text, New Vector2(windowSize.Width / 2.0F - textSize.X / 2.0F,
                                                                                       windowSize.Height / 2.0F - textSize.Y / 2.0F + 100), Color.White)

                If ControllerHandler.IsConnected() Then
                    SpriteBatch.Draw(TextureManager.GetTexture("GUI\GamePad\xboxControllerButtonA"), New Rectangle(CInt(windowSize.Width / 2 - textSize.X / 2 + FontManager.GameJoltFont.MeasureString("Press ").X),
                                                                                                                   CInt(windowSize.Height / 2 - textSize.Y / 2 + 98), 40, 40), Color.White)
                End If
            End If
        End If

        _logoRenderer.End()
        _shineRenderer.End()

        Canvas.DrawRectangle(windowSize, New Color(0, 0, 0, CInt(255 * _fadeIn)))
    End Sub

    Private Sub ChangeLevel()
        Dim levelCount As Integer = 0
        For Each levelPath As String In System.IO.Directory.GetFiles(GameController.GamePath & "\maps\mainmenu\")
            Dim levelFile As String = System.IO.Path.GetFileName(levelPath)
            If levelFile.StartsWith("mainmenu") = True And levelFile.EndsWith(".dat") = True Then
                levelCount += 1
            End If
        Next

        Dim levelID As Integer = Core.Random.Next(0, levelCount)

        Level.Load("mainmenu\mainmenu" & levelID & ".dat")
    End Sub

    Public Overrides Sub ChangeTo()
        Core.Player.Unload()
        Core.Player.Skin = "Hilbert"
        TextBox.Hide()
        TextBox.CanProceed = True
        OverworldScreen.FadeValue = 0

        MusicManager.PlayMusic("title", True, 0.0F, 0.0F)
    End Sub

End Class

Public Class NewMainMenuScreen

    'TODO: Replace old MainMenuScreen.

    Inherits Screen

    Private _screenOffset As Vector2 = New Vector2(20, 180)
    Private _screenOffsetTarget As Vector2 = New Vector2(20, 180)

    Private _loading As Boolean = True
    Private _fadeIn As Single = 0F
    Private _expandDisplay As Single = 0F
    Private _closingDisplay As Boolean = False
    Private _displayTextFade As Single = 0F

    Private _sliderPosition As Single = 0F
    Private _sliderTarget As Integer = 0

    Private _menuTexture As Texture2D = Nothing

    Private _profiles As New List(Of GameProfile)
    Private _selectedProfile As Integer = 0
    Private _menuIndex As Integer = 1 '0 = New Game, 1 = Profiles

    Private _messageBox As UI.MessageBox

    Public Sub New(ByVal currentScreen As Screen)
        Identification = Identifications.MainMenuScreen
        PreScreen = currentScreen

        CanBePaused = False
        MouseVisible = True
        CanChat = False

        _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")

        _screenOffset.X = CSng(windowSize.Width / 2 - 80)
        _screenOffsetTarget.X = _screenOffset.X
        _sliderTarget = GetSliderTarget(_selectedProfile)
        _sliderPosition = _sliderTarget

        _messageBox = New UI.MessageBox(Me)
    End Sub

    Public Overrides Sub Update()
        PreScreen.Update()
        If _loading = False Then
            If _fadeIn < 1.0F Then
                _fadeIn = MathHelper.Lerp(1.0F, _fadeIn, 0.93F)
                If _fadeIn + 0.01F >= 1.0F Then
                    _fadeIn = 1.0F
                    _sliderPosition = _sliderTarget
                End If
            Else
                If Controls.Accept(True, False, False) Then
                    'Click on profiles:
                    For x = 0 To _profiles.Count - 1
                        Dim xOffset As Single = _screenOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeIn))

                        If New Rectangle(CInt(xOffset), CInt(_screenOffset.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                            If _selectedProfile = x Then
                                ClickedProfile()
                            Else
                                Dim diff As Integer = x - _selectedProfile
                                _screenOffsetTarget.X -= diff * 180

                                _selectedProfile = x
                            End If
                            Exit For
                        End If
                    Next
                End If
                If Controls.Dismiss(True, False, False) Then
                    'Click on profiles:
                    For x = 0 To _profiles.Count - 1
                        Dim xOffset As Single = _screenOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeIn))

                        If New Rectangle(CInt(xOffset), CInt(_screenOffset.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                            If _selectedProfile = x Then
                                DismissProfile()
                            End If
                            Exit For
                        End If
                    Next
                End If

                If Controls.Right(True) And _selectedProfile < _profiles.Count - 1 Then
                    _selectedProfile += 1
                    _screenOffsetTarget.X -= 180
                End If
                If Controls.Left(True) And _selectedProfile > 0 Then
                    _selectedProfile -= 1
                    _screenOffsetTarget.X += 180
                End If
                _selectedProfile = _selectedProfile.Clamp(0, _profiles.Count - 1)

                If _fadeIn = 1.0F Then
                    If Controls.Accept(False, True, True) Then
                        ClickedProfile()
                    End If
                    If Controls.Dismiss(False, True, True) Then
                        DismissProfile()
                    End If
                    'Try to load the GameJolt profile once the player has logged in.
                    _profiles(0).LoadGameJolt()
                End If

                If _profiles(_selectedProfile).Loaded = False Then
                    _closingDisplay = True
                Else
                    _closingDisplay = False
                End If

                _sliderTarget = GetSliderTarget(_selectedProfile)
                If _sliderPosition < _sliderTarget Then
                    _sliderPosition = MathHelper.Lerp(_sliderTarget, _sliderPosition, 0.8F)
                ElseIf _sliderPosition > _sliderTarget Then
                    _sliderPosition = MathHelper.Lerp(_sliderTarget, _sliderPosition, 0.8F)
                End If

                If _fadeIn = 1.0F Then
                    If _closingDisplay Then
                        If _expandDisplay > 0.0F Then
                            _expandDisplay = MathHelper.Lerp(0.0F, _expandDisplay, 0.9F)
                            If _expandDisplay - 0.01F <= 0.0F Then
                                _expandDisplay = 0.0F
                            End If
                        End If
                    Else
                        If _expandDisplay < 1.0F Then
                            _expandDisplay = MathHelper.Lerp(1.0F, _expandDisplay, 0.9F)
                            If _expandDisplay + 0.01F >= 1.0F Then
                                _expandDisplay = 1.0F
                            End If
                        End If
                    End If
                End If
            End If
            UpdateScreenOffset()
        End If
    End Sub

    Private Sub ClickedProfile()
        If _selectedProfile = 0 And Security.FileValidation.IsValid(False) = False Then
            _messageBox.Show("File validation failed!" & vbNewLine & "Redownload the game's files to solve this problem.")
        Else
            _profiles(_selectedProfile).SelectProfile()
        End If
    End Sub

    Private Sub DismissProfile()
        _profiles(_selectedProfile).UnSelectProfile()
    End Sub

    Private Sub UpdateScreenOffset()
        If _screenOffset.X > _screenOffsetTarget.X Then
            _screenOffset.X = MathHelper.Lerp(_screenOffsetTarget.X, _screenOffset.X, 0.93F)
            If _screenOffset.X - 0.01F <= _screenOffsetTarget.X Then
                _screenOffset.X = _screenOffsetTarget.X
            End If
        End If
        If _screenOffset.X < _screenOffsetTarget.X Then
            _screenOffset.X = MathHelper.Lerp(_screenOffsetTarget.X, _screenOffset.X, 0.93F)
            If _screenOffset.X + 0.01F >= _screenOffsetTarget.X Then
                _screenOffset.X = _screenOffsetTarget.X
            End If
        End If
        If _screenOffset.Y > _screenOffsetTarget.Y Then
            _screenOffset.Y = MathHelper.Lerp(_screenOffsetTarget.Y, _screenOffset.Y, 0.93F)
            If _screenOffset.Y - 0.01F <= _screenOffsetTarget.Y Then
                _screenOffset.Y = _screenOffsetTarget.Y
            End If
        End If
        If _screenOffset.Y < _screenOffsetTarget.Y Then
            _screenOffset.Y = MathHelper.Lerp(_screenOffsetTarget.Y, _screenOffset.Y, 0.93F)
            If _screenOffset.Y + 0.01F >= _screenOffsetTarget.Y Then
                _screenOffset.Y = _screenOffsetTarget.Y
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        If _loading = False Then
            If _selectedProfile = 0 Then
                DrawGradients(CInt(255 * _fadeIn), True)
            Else
                DrawGradients(CInt(255 * _fadeIn), False)
            End If
        End If

        If IsCurrentScreen() Then
            If _loading Then
                Dim textSize As Vector2 = FontManager.GameJoltFont.MeasureString("Please wait..")
                GetFontRenderer().DrawString(FontManager.GameJoltFont, "Please wait" & LoadingDots.Dots, New Vector2(windowSize.Width / 2.0F - textSize.X / 2.0F,
                                                                                                                        windowSize.Height / 2.0F - textSize.Y / 2.0F + 100), Color.White)
            Else
                DrawProfiles()
            End If
        End If
    End Sub

    Private Sub DrawProfiles()
        'Draw profiles:
        For x = 0 To _profiles.Count - 1
            Dim xOffset As Single = _screenOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeIn))

            _profiles(x).Draw(New Vector2(xOffset, _screenOffset.Y), CInt(_fadeIn * 255), (x = _selectedProfile), _menuTexture)
        Next

        If _fadeIn = 1.0F Then
            'Draw arrow:
            SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(_sliderPosition - 16), CInt(_screenOffset.Y + 170), 32, 16), New Rectangle(0, 16, 32, 16), Color.White)

            Dim displayRect = New Rectangle(CInt((_sliderPosition - 300).Clamp(20, windowSize.Width - 620)), CInt(_screenOffset.Y + 170 + 16), 600, CInt(240 * _expandDisplay))

            'Draw display:
            If _expandDisplay > 0F Then
                Canvas.DrawRectangle(displayRect, Screens.UI.ColorProvider.MainColor(False))
                Canvas.DrawRectangle(New Rectangle(displayRect.X, displayRect.Y + displayRect.Height - 3, displayRect.Width, 3), Screens.UI.ColorProvider.AccentColor(False, CInt(255 * _expandDisplay)))
            End If

            'Dark theme:
            If (_selectedProfile = 0 Or _selectedProfile = 1) And _sliderPosition <= GetSliderTarget(1) Then
                Dim maxDistance As Integer = 180
                Dim distance As Integer = CInt(Math.Abs(_sliderTarget - _sliderPosition))
                Dim dist As Double = distance / maxDistance

                If _selectedProfile = 0 Then
                    dist = 1 - dist
                End If

                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(_sliderPosition - 16), CInt(_screenOffset.Y + 170), 32, 16), New Rectangle(32, 16, 32, 16), New Color(255, 255, 255, CInt(255 * dist)))
                If _expandDisplay > 0F Then
                    Canvas.DrawRectangle(displayRect, Screens.UI.ColorProvider.MainColor(True, CInt(255 * dist)))
                    Canvas.DrawRectangle(New Rectangle(displayRect.X, displayRect.Y + displayRect.Height - 3, displayRect.Width, 3), Screens.UI.ColorProvider.AccentColor(True, CInt(255 * dist * _expandDisplay)))
                End If
            End If

            'Draw profile info:
            Dim tmpProfile = _profiles(_selectedProfile)

            If _expandDisplay = 1.0F Then
                If tmpProfile.GameModeExists Then
                    For i = 0 To tmpProfile.PokemonTextures.Count - 1
                        SpriteBatch.Draw(tmpProfile.PokemonTextures(i), New Rectangle(displayRect.X + 30 + i * 70, displayRect.Y + 70, 64, 64), Color.White)
                    Next
                    GetFontRenderer().DrawString(FontManager.GameJoltFont, "Name: " & tmpProfile.Name & vbNewLine &
                                                                        "GameMode: " & tmpProfile.GameMode, New Vector2(displayRect.X + 30, displayRect.Y + 20), Color.White, 0F, Vector2.Zero, 0.5F, SpriteEffects.None, 0F)
                    GetFontRenderer().DrawString(FontManager.GameJoltFont, "Badges: " & tmpProfile.Badges.ToString() & vbNewLine &
                                                                        "Time played: " & tmpProfile.TimePlayed & vbNewLine &
                                                                        "Location: " & tmpProfile.Location, New Vector2(displayRect.X + 30, displayRect.Y + 150), Color.White, 0F, Vector2.Zero, 0.5F, SpriteEffects.None, 0F)
                Else
                    GetFontRenderer().DrawString(FontManager.GameJoltFont, "Name: " & tmpProfile.Name & vbNewLine &
                                                                        "GameMode: " & tmpProfile.GameMode, New Vector2(displayRect.X + 30, displayRect.Y + 20), Color.White, 0F, Vector2.Zero, 0.5F, SpriteEffects.None, 0F)

                    SpriteBatch.Draw(_menuTexture, New Rectangle(displayRect.X + 30, displayRect.Y + 70, 32, 32), New Rectangle(0, 32, 32, 32), Color.White)
                    Dim errorText As String = ""

                    If tmpProfile.IsGameJolt() Then
                        errorText = "Download failed. Press Accept to try again." & vbNewLine & vbNewLine &
                                    "If the problem persists, please try again later" & vbNewLine &
                                    "or contact us in our Discord server:" & vbNewLine & vbNewLine &
                                    "http://www.discord.me/p3d"
                    Else
                        errorText = "The required GameMode does not exist!"
                    End If
                    GetFontRenderer().DrawString(FontManager.GameJoltFont, errorText, New Vector2(displayRect.X + 70, displayRect.Y + 78), Color.White, 0F, Vector2.Zero, 0.5F, SpriteEffects.None, 0F)

                End If
            End If
        End If
    End Sub

    Private Function GetSliderTarget(ByVal index As Integer) As Integer
        Return CInt(_screenOffset.X + index * 180 + 80)
    End Function

    Public Overrides Sub ChangeTo()
        If _profiles.Count = 0 Then
            'Dim t As New Threading.Thread(AddressOf LoadProfiles)
            't.IsBackground = True
            't.Start()
            LoadProfiles()
        End If
    End Sub

    Private Sub LoadProfiles()
        _profiles.Add(New GameProfile("", False))

        Dim files As String() = {"Apricorns.dat", "Berries.dat", "Box.dat", "Daycare.dat", "HallOfFame.dat", "ItemData.dat", "Items.dat", "NPC.dat", "Options.dat", "Party.dat", "Player.dat", "Pokedex.dat", "Register.dat", "RoamingPokemon.dat", "SecretBase.dat", "Statistics.dat"}
        For Each path As String In IO.Directory.GetDirectories(GameController.GamePath & "\Save\")

            Dim exists As Boolean = True
            For Each file As String In files
                If IO.File.Exists(path & "\" & file) = False Then
                    exists = False
                    Exit For
                End If
            Next

            If exists = True Then
                _profiles.Add(New GameProfile(path, False))
            End If
        Next

        GameModeManager.SetGameModePointer("Kolben")

        _profiles.Add(New GameProfile("", True))

        _loading = False
    End Sub

    Private Class GameProfile

        Private _isGameJolt As Boolean = False
        Private _loaded As Boolean = False
        Private _isLoading As Boolean = False
        Private _failedGameJoltLoading As Boolean = False

        Private _path As String = ""
        Private _isNewGameButton As Boolean = False

        Private _name As String = ""
        Private _gameMode As String
        Private _pokedexSeen As Integer
        Private _pokedexCaught As Integer
        Private _badges As Integer
        Private _timePlayed As String
        Private _location As String
        Private _pokemonTextures As New List(Of Texture2D)
        Private _sprite As Texture2D
        Private _gameModeExists As Boolean
        Private _skin As String = ""

        Private _fontSize As Single = 0.75F
        Private _spriteIndex As Integer = 0
        Private _spriteDelay As Single = 1.5F
        Private _logoBounce As Single = 0F
        Private ReadOnly _spriteOrder As Integer() = {0, 1, 0, 2}

        Public ReadOnly Property IsGameJolt As Boolean
            Get
                Return _isGameJolt
            End Get
        End Property

        Public ReadOnly Property Path() As String
            Get
                Return _path
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property GameMode() As String
            Get
                Return _gameMode
            End Get
        End Property

        Public ReadOnly Property PokedexSeen() As Integer
            Get
                Return _pokedexSeen
            End Get
        End Property

        Public ReadOnly Property PokedexCaught() As Integer
            Get
                Return _pokedexCaught
            End Get
        End Property

        Public ReadOnly Property Badges() As Integer
            Get
                Return _badges
            End Get
        End Property

        Public ReadOnly Property TimePlayed() As String
            Get
                Return _timePlayed
            End Get
        End Property

        Public ReadOnly Property Location() As String
            Get
                Return _location
            End Get
        End Property

        Public ReadOnly Property PokemonTextures() As List(Of Texture2D)
            Get
                Return _pokemonTextures
            End Get
        End Property

        Public ReadOnly Property Loaded() As Boolean
            Get
                Return _loaded
            End Get
        End Property

        Public ReadOnly Property IsLoading() As Boolean
            Get
                Return _isLoading
            End Get
        End Property

        Public ReadOnly Property GameModeExists() As Boolean
            Get
                Return _gameModeExists
            End Get
        End Property

        Public Sub New(ByVal path As String, ByVal isNewGameButton As Boolean)
            If isNewGameButton Then
                _isNewGameButton = True
                _fontSize = 1.0F
            Else
                If path = "" Then
                    _isGameJolt = True
                    _loaded = False


                    _sprite = TextureManager.GetTexture("Textures\UI\GameJolt\gameJoltIcon")

                    LoadGameJolt()
                Else
                    _path = path

                    LoadFromPlayerData(IO.File.ReadAllText(path & "\Player.dat"))
                    LoadContent(IO.File.ReadAllText(path & "\Party.dat"))

                    _loaded = True
                End If
            End If
        End Sub

        Private Sub LoadContent(ByVal data As String)
            If GameModeManager.GameModeExists(_gameMode) Then
                _gameModeExists = True

                GameModeManager.SetGameModePointer(_gameMode)

                Dim pokemonData As String() = data.SplitAtNewline()
                For Each line As String In pokemonData
                    If line.StartsWith("{") Then
                        _pokemonTextures.Add(Pokemon.GetPokemonByData(line).GetMenuTexture(True))
                    End If
                Next

                If _isGameJolt = False Then
                    _sprite = TextureManager.GetTexture("Textures\NPC\" & _skin)
                End If
            Else
                _gameModeExists = False
                _sprite = TextureManager.GetTexture("GUI\unknownSprite")
            End If
        End Sub

        Private Sub LoadFromPlayerData(ByVal data As String)
            Dim playerData As String() = data.SplitAtNewline()
            For Each line As String In playerData
                If line.Contains("|") Then
                    Dim id As String = line.Split("|"c)(0)
                    Dim content As String = line.Split("|"c)(1)

                    Select Case id.ToLower()
                        Case "name"
                            _name = content

                            While FontManager.GameJoltFont.MeasureString(_name).X * _fontSize > 140
                                _fontSize -= 0.01F
                            End While
                        Case "badges"
                            If content.Length > 0 AndAlso content <> "0" Then
                                _badges = content.Split(","c).Length
                            Else
                                _badges = 0
                            End If
                        Case "playtime"
                            Dim timedata As String() = content.Split(","c)

                            Dim hours As Integer = CInt(timedata(0)) + CInt(timedata(3)) * 24
                            Dim minutes As Integer = CInt(timedata(1))
                            Dim seconds As Integer = CInt(timedata(2))

                            _timePlayed = hours.ToString("D2") & ":" & minutes.ToString("D2") & ":" & seconds.ToString("D2")
                        Case "location"
                            _location = content
                        Case "gamemode"
                            _gameMode = content
                        Case "skin"
                            _skin = content
                    End Select
                End If
            Next
        End Sub

        Public Sub Draw(ByVal offset As Vector2, ByVal alpha As Integer, ByVal isSelected As Boolean, ByVal t As Texture2D)
            If _isGameJolt Then
                For x = 0 To 9
                    For y = 0 To 9
                        SpriteBatch.Draw(t, New Rectangle(CInt(x * 16 + offset.X), CInt(y * 16 + offset.Y), 16, 16), New Rectangle(32, 0, 16, 16), New Color(255, 255, 255, alpha))
                    Next
                Next
                Canvas.DrawRectangle(New Rectangle(CInt(offset.X), CInt(offset.Y), 160, 3), Screens.UI.ColorProvider.AccentColor(True, alpha))

                If _isLoading And GameJoltSave.DownloadProgress > 0 Then
                    Dim width As Integer = CInt((GameJoltSave.DownloadProgress / (GameJolt.GamejoltSave.SAVEFILECOUNT + GameJolt.GamejoltSave.EXTRADATADOWNLOADCOUNT)) * 160)
                    Canvas.DrawRectangle(New Rectangle(CInt(offset.X), CInt(offset.Y + 3), width, 157), New Color(100, 100, 100, 128))
                End If
            Else
                For x = 0 To 9
                    For y = 0 To 9
                        SpriteBatch.Draw(t, New Rectangle(CInt(x * 16 + offset.X), CInt(y * 16 + offset.Y), 16, 16), New Rectangle(0, 0, 16, 16), New Color(255, 255, 255, alpha))
                    Next
                Next
                Canvas.DrawRectangle(New Rectangle(CInt(offset.X), CInt(offset.Y), 160, 3), Screens.UI.ColorProvider.AccentColor(False, alpha))
            End If

            If _isNewGameButton Then
                Dim text As String = "NEW" & vbNewLine & "GAME"

                If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                    FontRenderer.DrawString(FontManager.GameJoltFont, text, New Vector2(offset.X + 80 - (FontManager.GameJoltFont.MeasureString(text).X) / 2,
                                                                                        offset.Y + 80 - (FontManager.GameJoltFont.MeasureString(text).Y) / 2), New Color(255, 255, 255, alpha))
                Else
                    SpriteBatch.DrawString(FontManager.GameJoltFont, text, New Vector2(offset.X + 80 - (FontManager.GameJoltFont.MeasureString(text).X) / 2,
                                                                                        offset.Y + 80 - (FontManager.GameJoltFont.MeasureString(text).Y) / 2), New Color(255, 255, 255, alpha))
                End If
            Else
                If _loaded Then
                    Dim frameSize As Size = New Size(CInt(_sprite.Width / 3), CInt(_sprite.Height / 4))
                    If isSelected Then
                        _spriteDelay -= 0.1F
                        If _spriteDelay <= 0F Then
                            _spriteDelay = 1.5F
                            _spriteIndex += 1
                            If _spriteIndex = _spriteOrder.Length Then
                                _spriteIndex = 0
                            End If
                        End If
                    Else
                        _spriteIndex = 0
                    End If

                    SpriteBatch.Draw(_sprite, New Rectangle(CInt(offset.X + 17), CInt(offset.Y - 10), 128, 128), New Rectangle(frameSize.Width * _spriteOrder(_spriteIndex), frameSize.Height * 2, frameSize.Width, frameSize.Height), New Color(255, 255, 255, alpha))

                    If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                        FontRenderer.DrawString(FontManager.GameJoltFont, _name, New Vector2(offset.X + 80 - (FontManager.GameJoltFont.MeasureString(_name).X * _fontSize) / 2, offset.Y + 120), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    Else
                        SpriteBatch.DrawString(FontManager.GameJoltFont, _name, New Vector2(offset.X + 80 - (FontManager.GameJoltFont.MeasureString(_name).X * _fontSize) / 2, offset.Y + 120), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    End If
                Else
                    If isSelected Then
                        _logoBounce += 0.2F
                    Else
                        _logoBounce = 0F
                    End If

                    Dim text As String = "Log in"
                    If _isLoading Then
                        text = "Loading..."
                    End If

                    SpriteBatch.Draw(_sprite, New Rectangle(CInt(offset.X + 46), CInt(offset.Y + 36 + Math.Sin(_logoBounce) * 8.0F), 68, 72), New Color(255, 255, 255, alpha))

                    If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                        FontRenderer.DrawString(FontManager.GameJoltFont, text, New Vector2(offset.X + 80 - (FontManager.GameJoltFont.MeasureString(text).X * _fontSize) / 2, offset.Y + 120), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    Else
                        SpriteBatch.DrawString(FontManager.GameJoltFont, text, New Vector2(offset.X + 80 - (FontManager.GameJoltFont.MeasureString(text).X * _fontSize) / 2, offset.Y + 120), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    End If
                End If
            End If
        End Sub

        Public Sub LoadGameJolt()
            If GameJolt.API.LoggedIn Then
                If _isGameJolt And _loaded = False And _isLoading = False Then
                    _isLoading = True

                    GameJoltSave.DownloadSave(GameJolt.LogInScreen.LoadedGameJoltID, True)
                ElseIf _isGameJolt And _loaded = False And _isLoading Then
                    If GameJoltSave.DownloadFinished Then
                        _loaded = True
                        _isLoading = False

                        _sprite = GameJoltSave.DownloadedSprite
                        If _sprite Is Nothing Then
                            _sprite = GameJolt.Emblem.GetPlayerSprite(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
                        End If

                        LoadFromPlayerData(GameJoltSave.Player)
                        LoadContent(GameJoltSave.Party)
                    Else
                        If GameJoltSave.DownloadFailed Then
                            _loaded = True
                            _isLoading = False
                            _failedGameJoltLoading = True

                            _sprite = TextureManager.GetTexture("GUI\unknownSprite")
                        End If
                    End If
                End If
            End If
        End Sub
        Public Sub UnSelectProfile()
            If IsGameJolt AndAlso _loaded Then
                _loaded = False
                _isLoading = False
                _pokemonTextures.Clear()
                _sprite = TextureManager.GetTexture("Textures\UI\GameJolt\gameJoltIcon")
                _fontSize = 0.75F
                GameJolt.API.LoggedIn = False
            End If
        End Sub
        Public Sub SelectProfile()
            If _isNewGameButton Then
                If GameModeManager.GameModeCount = 1 Then
                    'There's only the default GameMode available, so just load that one:
                    GameModeManager.SetGameModePointer("Kolben")
                    SetScreen(New Screens.MainMenu.NewNewGameScreen(CurrentScreen))
                Else
                    'There is more than one GameMode, prompt a selection screen:
                    SetScreen(New GameModeSelectionScreen(CurrentScreen))
                End If
            Else
                If _isGameJolt And _loaded = False And GameJolt.API.LoggedIn = False Then
                    SetScreen(New GameJolt.LogInScreen(CurrentScreen))
                Else
                    If _gameModeExists Then
                        GameModeManager.SetGameModePointer(_gameMode)

                        If _isGameJolt Then
                            Core.Player.IsGameJoltSave = True
                            Core.Player.LoadGame("GAMEJOLTSAVE")

                            SetScreen(New JoinServerScreen(CurrentScreen))
                        Else
                            Core.Player.IsGameJoltSave = False
                            Core.Player.LoadGame(IO.Path.GetFileName(_path))

                            SetScreen(New JoinServerScreen(CurrentScreen))
                        End If
                    Else
                        If Me.IsGameJolt Then
                            _loaded = False
                            _sprite = TextureManager.GetTexture("Textures\UI\GameJolt\gameJoltIcon")
                            LoadGameJolt()
                        Else
                            Dim messageBox As New UI.MessageBox(CurrentScreen)
                            messageBox.Show("The required GameMode does not exist." & vbNewLine & "Reaquire the GameMode to play on this profile.")
                        End If

                    End If
                End If
            End If
        End Sub

    End Class

End Class

Public Class GameModeSelectionScreen

    Inherits Screen

    Private _gameModes As GameMode()
    Private _index As Integer = 0
    Private _offset As Single = 0F

    Private Const WIDTH = 400
    Private Const HEIGHT = 64
    Private Const GAP = 32

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.GameModeSelectionScreen
        CanBePaused = True
        CanChat = False
        CanDrawDebug = True
        CanGoFullscreen = True
        CanMuteMusic = True
        CanTakeScreenshot = True

        PreScreen = currentScreen

        _gameModes = GameModeManager.GetAllGameModes
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        Dim text = "Select a GameMode" + vbNewLine + "to start the new game with"

        GetFontRenderer().DrawString(FontManager.GameJoltFont, text, New Vector2(30, 30), Color.White)

        Dim center = windowSize.Width - 250
        For i = 0 To _gameModes.Length - 1
            Dim y = CType(i * (HEIGHT + GAP) + _offset + windowSize.Height / 2 - HEIGHT / 2, Integer)
            Dim color = Screens.UI.ColorProvider.LightColor
            Dim alphaColor = New Color(color.R, color.G, color.B, 0)
            Dim halfWidth = CType(WIDTH / 2, Integer)

            Canvas.DrawGradient(New Rectangle(center - halfWidth, y, 50, HEIGHT), alphaColor, color, True, -1)
            Canvas.DrawGradient(New Rectangle(center + halfWidth - 50, y, 50, HEIGHT), color, alphaColor, True, -1)
            Canvas.DrawRectangle(New Rectangle(center - halfWidth + 50, y, WIDTH - 100, HEIGHT), color)

            Dim displayText = _gameModes(i).Name
            Dim textSize = FontManager.GameJoltFont.MeasureString(displayText)

            GetFontRenderer().DrawString(FontManager.GameJoltFont, displayText, New Vector2(center - halfWidth + 50, CType(y + HEIGHT / 2 - ((textSize.Y * 0.75F) / 2), Integer)), Color.White, 0F, Vector2.Zero, 0.75F, SpriteEffects.None, 0F)
        Next
    End Sub

    Public Overrides Sub Update()
        ' PreScreen is the MainMenuScreen, so update the pre screen of that to achieve the background world:
        PreScreen.PreScreen.Update()

        If _index > 0 AndAlso Controls.Up(True, True, True, True, True, True) Then
            _index -= 1
        End If
        If _index < _gameModes.Length - 1 AndAlso Controls.Down(True, True, True, True, True, True) Then
            _index += 1
        End If
        If Controls.Dismiss(True, True, True) Then
            SetScreen(PreScreen)
        End If
        If Controls.Accept(True, True, True) Then
            GameModeManager.SetGameModePointer(_gameModes(_index).DirectoryName)
            SetScreen(New Screens.MainMenu.NewNewGameScreen(PreScreen))
        End If

        Dim targetOffset = GetTargetOffset()

        If _offset <> targetOffset Then
            _offset = MathHelper.Lerp(_offset, targetOffset, 0.25F)
            If Math.Abs(_offset - targetOffset) <= 0.01F Then
                _offset = targetOffset
            End If
        End If
    End Sub

    Private Function GetTargetOffset() As Integer
        Return -_index * (HEIGHT + GAP)
    End Function

End Class