Imports GameDevCommon.Drawing
Imports P3D.Screens.MainMenu

''' <summary>
''' This is the game screen that includes the shimmering Pokémon 3D logo, the "Press {BUTTON} to start" text, and the time-dependent animated background.
''' </summary>
Public Class PressStartScreen

    Inherits Screen

    Public _fadeInMain As Single = 1.0F
    Private _introDelay As Single = 4.0F
    Private _logoFade As Single = 1.0F

    Private _logoTexture As Texture2D = Nothing
    Private _shineTexture As Texture2D = Nothing

    Private _logoRenderer As SpriteBatch
    Private _shineRenderer As SpriteBatch
    Private _backgroundRenderer As SpriteBatch

    Private target As RenderTarget2D

    Private _shader As GameDevCommon.Rendering.Shader
    Private _entities As List(Of MainMenuEntity) = New List(Of MainMenuEntity)()
    Private _camera As Screens.MainMenu.Scene.MainMenuCamera
    Private _fromColor As Color
    Private _toColor As Color
    Private _textColor As Color

    Dim tempF As Single = 0F
    Dim tempG As Single = 0F

    Public Sub New()
        Identification = Identifications.PressStartScreen
        CanBePaused = False
        MouseVisible = True
        CanChat = False

        TextBox.Showing = False
        PokemonImageView.Showing = False
        ImageView.Showing = False
        ChooseBox.Showing = False

        GameModeManager.SetGameModePointer("Kolben")

        Core.Player.Skin = "Hilbert"

        If IO.Directory.Exists(GameController.GamePath & "\Save\") = False Then
            IO.Directory.CreateDirectory(GameController.GamePath & "\Save\")
        End If

        GameJolt.Emblem.ClearOnlineSpriteCache()

        _logoTexture = TextureManager.GetTexture("GUI\Logos\Pokemon_Small")
        _shineTexture = TextureManager.GetTexture("GUI\Logos\logo_shine")

        _logoRenderer = New SpriteBatch(GraphicsDevice)
        _shineRenderer = New SpriteBatch(GraphicsDevice)
        _backgroundRenderer = New SpriteBatch(GraphicsDevice)

        target = New RenderTarget2D(GraphicsDevice, 1200, 680, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)

        ' TODO: Replace bad workaround.
        Screens.MainMenu.NewNewGameScreen.CharacterSelectionScreen.SelectedSkin = ""
        Core.Player.Unload()

        _shader = New GameDevCommon.Rendering.BasicShader()
        CType(_shader.Effect, BasicEffect).LightingEnabled = False
        _camera = New Scene.MainMenuCamera()
        World.setDaytime = -1
        Dim dayTime = World.GetTime

        Select Case dayTime
            Case World.DayTimes.Morning
                _fromColor = New Color(246, 170, 109)
                _toColor = New Color(248, 248, 248)
                _textColor = Color.Black
            Case World.DayTimes.Day
                _fromColor = New Color(120, 160, 248)
                _toColor = New Color(248, 248, 248)
                _textColor = Color.Black
            Case World.DayTimes.Evening
                _fromColor = New Color(32, 64, 168)
                _toColor = New Color(40, 80, 88)
                _textColor = Color.White
            Case World.DayTimes.Night
                _fromColor = New Color(32, 64, 168)
                _toColor = New Color(0, 0, 0)
                _textColor = Color.White
        End Select

        If dayTime = World.DayTimes.Day OrElse dayTime = World.DayTimes.Morning Then
            Dim clouds = New Scene.Clouds()
            clouds.LoadContent()
            _entities.Add(clouds)

            Dim hooh = New Scene.HoOh(_entities)
            hooh.LoadContent()
            _entities.Add(hooh)
        Else
            Dim ground = New Scene.Ground()
            ground.LoadContent()
            _entities.Add(ground)

            Dim lugia = New Scene.Lugia(_entities)
            lugia.LoadContent()
            _entities.Add(lugia)
        End If

    End Sub

    Public Overrides Sub Update()
        For i = 0 To _entities.Count - 1
            If i < _entities.Count Then
                _entities(i).Update()
                If _entities(i).ToBeRemoved Then
                    _entities(i).Dispose()
                    _entities.RemoveAt(i)
                    i -= 1
                End If
            End If
        Next

        _camera.Update()

        If _introDelay > 0F Then
            _introDelay -= 0.1F
            If _introDelay <= 0F Then
                _introDelay = 0F
            End If
        Else
            If IsCurrentScreen() Then
                If _fadeInMain > 0.0F Then
                    _fadeInMain = MathHelper.Lerp(0.0F, _fadeInMain, 0.93F)
                    If _fadeInMain - 0.01F <= 0.0F Then
                        _fadeInMain = 0.0F
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
                    _fadeInMain = 0.0F
                    SetScreen(New NewMainMenuScreen(Me))
                End If
            End If
        End If
    End Sub

    Private _blurHandler As Resources.Blur.BlurHandler

    Public Overrides Sub Draw()
        If _blurHandler Is Nothing Then
            _blurHandler = New Resources.Blur.BlurHandler(target.Width, target.Height)
        End If

        GraphicsDevice.SetRenderTarget(target)
        GraphicsDevice.ResetFull()
        GraphicsDevice.Clear(_fromColor)


        ' DRAW HERE
        _backgroundRenderer.Begin()

        _backgroundRenderer.DrawGradient(New Rectangle(0, CType(target.Height / 2, Integer), target.Width, CType(target.Height / 4, Integer)),
                                         _fromColor, _toColor, False, 1, 10)
        _backgroundRenderer.DrawRectangle(New Rectangle(0, CType(target.Height / 4 * 3, Integer), target.Width, CType(target.Height / 4, Integer)),
                                          _toColor)

        _backgroundRenderer.End()

        GraphicsDevice.ResetFull()
        _shader.Prepare(_camera)
        For Each entity In _entities
            _shader.Render(entity)
        Next

        GraphicsDevice.SetRenderTarget(Nothing)


        _backgroundRenderer.Begin()
        Dim blurred = _blurHandler.Perform(target)
        _backgroundRenderer.Draw(blurred, New Rectangle(0, 0, windowSize.Width, windowSize.Height), Color.White)

        '_backgroundRenderer.Draw(target, New Rectangle(0, 0, ScreenSize.Width, ScreenSize.Height), Color.White)
        _backgroundRenderer.End()

        If IsCurrentScreen() Then
            _logoRenderer.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone)
        Else
            _logoRenderer.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone)
        End If
        _shineRenderer.Begin(SpriteSortMode.Texture, BlendState.Additive)

        _logoRenderer.Draw(_logoTexture, New Rectangle(CInt(windowSize.Width / 2 - 350 * SpriteBatch.InterfaceScale), CInt(160 * _fadeInMain + 64),
                                                       CInt(700 * SpriteBatch.InterfaceScale), CInt(300 * SpriteBatch.InterfaceScale)), New Color(255, 255, 255, CInt(255 * _logoFade)))
        _shineRenderer.Draw(_shineTexture, New Rectangle(CInt(windowSize.Width / 2 - 250 * SpriteBatch.InterfaceScale + Math.Sin(tempF) * 240.0F), CInt(-100 + Math.Sin(tempG) * 10.0F + CInt(160 * _fadeInMain + 64)),
                                                         CInt(512 * SpriteBatch.InterfaceScale), CInt(512 * SpriteBatch.InterfaceScale)), New Color(255, 255, 255, CInt(255 * _logoFade)))

        If _fadeInMain = 0F Then
            If IsCurrentScreen() And Core.GameOptions.ShowGUI Then  ' Want to implement fading of text, but core doesn't seem to support this.
                Dim text As String = String.Empty
                If ControllerHandler.IsConnected() Then
                    text = Localization.GetString("start_screen_press", "Press") & "           " & Localization.GetString("start_screen_tostart", "to start.")
                Else
                    text = Localization.GetString("start_screen_press", "Press") & " " & KeyBindings.EnterKey1.ToString().ToUpper & " " & Localization.GetString("start_screen_tostart", "to start.")
                    'text = "Press " & KeyBindings.EnterKey1.ToString() & ", " & KeyBindings.EnterKey2.ToString() & ", or primary mouse button to start."
                End If

                Dim textSize As Vector2 = FontManager.InGameFont.MeasureString(text)

                GetFontRenderer().DrawString(FontManager.InGameFont, text, New Vector2(CInt(windowSize.Width / 2.0F - textSize.X / 2.0F),
                                                                                       CInt(windowSize.Height - textSize.Y - 50)), _textColor)

                If ControllerHandler.IsConnected() Then
                    SpriteBatch.Draw(TextureManager.GetTexture("GUI\GamePad\xboxControllerButtonA"), New Rectangle(CInt(windowSize.Width / 2 - textSize.X / 2 + FontManager.InGameFont.MeasureString("Press" & "  ").X + 2), CInt(windowSize.Height - textSize.Y - 58), 40, 40), Color.White)
                End If
            End If
        End If

        _logoRenderer.End()
        _shineRenderer.End()

        Canvas.DrawRectangle(windowSize, New Color(0, 0, 0, CInt(255 * _fadeInMain)))
    End Sub

    Public Overrides Sub ChangeTo()
        Core.Player.Unload()
        Core.Player.Skin = "Hilbert"
        TextBox.Hide()
        TextBox.CanProceed = True
        OverworldScreen.FadeValue = 0

        MusicManager.Play("title", True, 0.0F)
    End Sub

End Class

Public Class NewMainMenuScreen

    Inherits Screen

    Public _screenOffset As Vector2 = New Vector2(0, 0) 'Position of the top row relative to the _screenOrigin
    Public _screenOffsetTarget As Vector2 = New Vector2(0, 0) 'Target where the top needs to move to
    Public _screenOrigin As Vector2 = New Vector2(CSng(windowSize.Width / 2 - 80 - 180), CInt(windowSize.Height / 4)) 'Center of the game window. It's adjusted when resizing the window.
    Private _mainOffset As Vector2 = _screenOffset

    Public _optionsOffset As Vector2 = New Vector2(0, 0) 'Position of the options row relative to the _screenOrigin
    Public _optionsOffsetTarget As Vector2 = New Vector2(0, 0) 'Target where the options row needs to move to

    Private _gameJoltOffset As Vector2 = New Vector2(0, 0) 'Position of the gamejolt row relative to the _screenOrigin
    Private _gameJoltOffsetTarget As Vector2 = New Vector2(0, 0) 'Target where the gamejolt needs to move to


    Private _loading As Boolean = True
    Public _fadeInMain As Single = 0F
    Public _fadeInOptions As Single = 0F
    Private _fadeInGameJolt As Single = 0F
    Private _GameJoltOpacity As Single = 0F
    Private _expandDisplay As Single = 0F
    Private _closingDisplay As Boolean = False
    Private _displayTextFade As Single = 0F

    Private _sliderPosition As Single = 0F 'Horizontal position of the main menu arrow
    Private _sliderTarget As Integer = 0 'Target where the arrow needs to move to

    Private _menuTexture As Texture2D
    Private _oldMenuTexture As Texture2D

    Private _MainProfiles As New List(Of GameProfile)
    Private _GameJoltProfiles As New List(Of GameProfile)
    Private _OptionsProfiles As New List(Of GameProfile)
    Public Shared _selectedProfile As Integer = 1
    Public Shared _selectedProfileTemp As Integer = _selectedProfile
    Private _GameJoltButtonIndex As Integer = 0 'This is to tell which button on a GameJolt profile is selected: the profile itself or the gender & reset buttons
    Public Shared _menuIndex As Integer = 0 '0 = Game Profiles, 1 = GameJolt Submenu, 2 = Options Submenu, 3 = GameJolt Options Submenu

    Public GameModeSplash As Texture2D

    Private _messageBox As UI.MessageBox

    Public Sub New(ByVal currentScreen As Screen)
        Identification = Identifications.MainMenuScreen
        PreScreen = currentScreen

        CanBePaused = False
        MouseVisible = True
        CanChat = False

        _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
        _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        _screenOffset = New Vector2(0, 0)
        _screenOffsetTarget = _screenOffset

        _mainOffset = _screenOffset

        _optionsOffset = New Vector2(0, 0)
        _optionsOffsetTarget = _optionsOffset

        _gameJoltOffset = New Vector2(0, 0)
        _gameJoltOffsetTarget = _gameJoltOffset

        _selectedProfile = 1

        _sliderTarget = GetSliderTarget(_selectedProfile)
        _sliderPosition = _sliderTarget

        _messageBox = New UI.MessageBox(Me)
    End Sub


    Public Overrides Sub Update()
        PreScreen.Update()
        If _loading = False Then
            'Shift the Top row horizontally
            _mainOffset.X = _screenOffset.X
            'Shift the main menu vertically
            Select Case _menuIndex
                Case 0
                    _screenOffsetTarget.Y = 0
                    _optionsOffsetTarget.X = _screenOffsetTarget.X
                Case 1
                    _screenOffsetTarget.Y = -180 - 32
                    _optionsOffsetTarget.X = _gameJoltOffsetTarget.X
                Case 2
                    _screenOffsetTarget.Y = -180 - 32
                Case 3
                    _screenOffsetTarget.Y = -320 - 32
            End Select

            'Fade in Options row
            If _menuIndex = 2 Or _menuIndex = 3 Then
                If _fadeInOptions < 1.0F Then
                    _fadeInOptions = MathHelper.Lerp(1.0F, _fadeInOptions, 0.93F)
                    If _fadeInOptions + 0.01F >= 1.0F Then
                        _fadeInOptions = 1.0F
                    End If
                End If
            Else
                If _fadeInOptions > 0.0F Then
                    _fadeInOptions = MathHelper.Lerp(0.0F, _fadeInOptions, 0.93F)
                    If _fadeInOptions - 0.01F <= 0.0F Then
                        _fadeInOptions = 0.0F
                    End If
                End If
            End If

            'Fade in the Top row
            If _fadeInMain < 1.0F Then
                _fadeInMain = MathHelper.Lerp(1.0F, _fadeInMain, 0.93F)
                If _fadeInMain + 0.01F >= 1.0F Then
                    _fadeInMain = 1.0F
                    _sliderPosition = _sliderTarget
                End If
            Else
                If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                    If Controls.Accept(True, False, False) Then
                        ' Click on profiles.
                        Select Case _menuIndex
                            Case 0
                                For x = 0 To _MainProfiles.Count - 1
                                    Dim xOffset As Single = _screenOrigin.X + _screenOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))

                                    If New Rectangle(CInt(xOffset), CInt(_screenOrigin.Y + _screenOffset.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                                        If _selectedProfile = x Then
                                            If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                                ClickedProfile()
                                                SoundManager.PlaySound("select")
                                            End If
                                        Else
                                            GameModeSplash = Nothing
                                            If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                                Dim diff As Integer = x - _selectedProfile
                                                _screenOffsetTarget.X -= diff * 180

                                                _selectedProfile = x
                                                If _MainProfiles(_selectedProfile)._gameModeExists Then
                                                    GameModeManager.SetGameModePointer(_MainProfiles(_selectedProfile)._gameMode)
                                                Else
                                                    GameModeManager.SetGameModePointer("Kolben")
                                                End If
                                                _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
                                                _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")

                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next
                                If _MainProfiles(_selectedProfile).IsGameJolt AndAlso _MainProfiles(_selectedProfile).Loaded Then
                                    For x = 0 To _MainProfiles.Count - 1
                                        ' Click on gamejolt buttons
                                        Dim xOffset As Single = _screenOrigin.X + _screenOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))
                                        Dim r As New Vector2(xOffset + 400, _screenOrigin.Y + _screenOffset.Y + 200)
                                        If New Rectangle(CInt(r.X), CInt(r.Y), 32, 32).Contains(MouseHandler.MousePosition) Then
                                            ButtonChangeMale()
                                        ElseIf New Rectangle(CInt(r.X), CInt(r.Y) + 48, 32, 32).Contains(MouseHandler.MousePosition) Then
                                            ButtonChangeFemale()
                                        ElseIf New Rectangle(CInt(r.X), CInt(r.Y) + 48 + 48, 32, 32).Contains(MouseHandler.MousePosition) Then
                                            ButtonChangeGenderless()
                                        ElseIf New Rectangle(CInt(r.X), CInt(r.Y) + 48 + 48 + 48, 32, 32).Contains(MouseHandler.MousePosition) Then
                                            ButtonResetSave()
                                        End If
                                    Next
                                End If
                                If Controls.Dismiss(True, False, False) Then
                                    ' Click on profiles.
                                    For x = 0 To _MainProfiles.Count - 1
                                        Dim xOffset As Single = _screenOrigin.X + _screenOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))

                                        If New Rectangle(CInt(xOffset), CInt(_screenOrigin.Y + _screenOffset.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                                            If _selectedProfile = x Then
                                                SoundManager.PlaySound("select")
                                                DismissProfile()
                                            End If
                                            Exit For
                                        End If
                                    Next
                                End If
                            Case 2
                                GameModeSplash = Nothing
                                For x = 0 To _MainProfiles.Count - 1
                                    Dim xOffset As Single = _screenOrigin.X + _mainOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))
                                    If New Rectangle(CInt(xOffset), CInt(_screenOrigin.Y + _screenOffsetTarget.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                                        _menuIndex = 0
                                        Dim diff As Integer = CInt(_screenOffset.X + x * 180) - CInt(_optionsOffset.X + _selectedProfile * 180)
                                        _screenOffsetTarget.X -= diff
                                        _selectedProfile = x
                                        _sliderTarget = GetSliderTarget(x)
                                        SoundManager.PlaySound("select")
                                    End If
                                Next
                                For x = 0 To _OptionsProfiles.Count - 1
                                    Dim xOffset As Single = _screenOrigin.X + _optionsOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))

                                    If New Rectangle(CInt(xOffset), CInt(_screenOrigin.Y + _optionsOffset.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                                        If _selectedProfile = x Then
                                            If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                                ClickedProfile()
                                                SoundManager.PlaySound("select")
                                            End If
                                        Else
                                            If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                                Dim diff As Integer = x - _selectedProfile
                                                _optionsOffsetTarget.X -= diff * 180
                                                _selectedProfile = x
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next
                            Case 3
                                For x = 0 To _GameJoltProfiles.Count - 1
                                    Dim xOffset As Single = _screenOrigin.X + _gameJoltOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))
                                    If New Rectangle(CInt(xOffset), CInt(_screenOrigin.Y + _screenOffsetTarget.Y + -180 + 32), 160, 160).Contains(MouseHandler.MousePosition) Then
                                        Dim diff As Integer = x - _selectedProfile
                                        _gameJoltOffsetTarget.X -= diff * 180
                                        _menuIndex = 1
                                        _selectedProfile = x
                                        _sliderTarget = GetSliderTarget(x)
                                        SoundManager.PlaySound("select")
                                    End If
                                Next
                                For x = 0 To _OptionsProfiles.Count - 1
                                    Dim xOffset As Single = _screenOrigin.X + _optionsOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))

                                    If New Rectangle(CInt(xOffset), CInt(_screenOrigin.Y + _optionsOffset.Y), 160, 160).Contains(MouseHandler.MousePosition) Then
                                        If _selectedProfile = x Then
                                            If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                                ClickedProfile()
                                                SoundManager.PlaySound("select")
                                            End If
                                        Else
                                            GameModeSplash = Nothing
                                            If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                                Dim diff As Integer = x - _selectedProfile
                                                _optionsOffsetTarget.X -= diff * 180

                                                _selectedProfile = x
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next
                        End Select
                    End If
                    If CurrentScreen.Identification = Screen.Identifications.MainMenuScreen Then
                        Select Case _menuIndex
                            Case 0
                                If Controls.Right(True) And _selectedProfile < _MainProfiles.Count - 1 Then
                                    _selectedProfile += 1
                                    _screenOffsetTarget.X -= 180
                                    _GameJoltButtonIndex = 0
                                    GameModeSplash = Nothing
                                    If _MainProfiles(_selectedProfile)._gameModeExists Then
                                        GameModeManager.SetGameModePointer(_MainProfiles(_selectedProfile)._gameMode)
                                    End If
                                    _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
                                    _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")
                                End If
                                If Controls.Left(True) And _selectedProfile > 0 Then
                                    _selectedProfile -= 1
                                    _screenOffsetTarget.X += 180
                                    _GameJoltButtonIndex = 0
                                    GameModeSplash = Nothing
                                    If _MainProfiles(_selectedProfile)._gameModeExists Then
                                        GameModeManager.SetGameModePointer(_MainProfiles(_selectedProfile)._gameMode)
                                    End If
                                    _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
                                    _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")
                                End If
                            Case 1
                                If Controls.Right(True) And _selectedProfile < _GameJoltProfiles.Count - 1 Then
                                    _selectedProfile += 1
                                    _gameJoltOffsetTarget.X -= 180
                                    _GameJoltButtonIndex = 0
                                    GameModeSplash = Nothing
                                    If _MainProfiles(_selectedProfile)._gameModeExists Then
                                        GameModeManager.SetGameModePointer(_MainProfiles(_selectedProfile)._gameMode)
                                    End If
                                    _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
                                    _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")
                                End If
                                If Controls.Left(True) And _selectedProfile > 0 Then
                                    _selectedProfile -= 1
                                    _gameJoltOffsetTarget.X += 180
                                    _GameJoltButtonIndex = 0
                                    GameModeSplash = Nothing
                                    If _MainProfiles(_selectedProfile)._gameModeExists Then
                                        GameModeManager.SetGameModePointer(_MainProfiles(_selectedProfile)._gameMode)
                                    End If
                                    _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
                                    _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")
                                End If
                            Case 2, 3
                                If Controls.Right(True) And _selectedProfile < _OptionsProfiles.Count - 1 Then
                                    _selectedProfile += 1
                                    _optionsOffsetTarget.X -= 180
                                End If
                                If Controls.Left(True) And _selectedProfile > 0 Then
                                    _selectedProfile -= 1
                                    _optionsOffsetTarget.X += 180
                                End If
                        End Select
                    End If
                    If _menuIndex = 0 Then
                        If _MainProfiles(_selectedProfile).IsGameJolt AndAlso _MainProfiles(_selectedProfile).Loaded Then
                            If Controls.Down(True, True, False) Then
                                _GameJoltButtonIndex += 1
                            End If
                            If Controls.Up(True, True, False) Then
                                _GameJoltButtonIndex -= 1
                            End If
                            _GameJoltButtonIndex = Clamp(_GameJoltButtonIndex, 0, 4)
                        End If
                    End If

                    Select Case _menuIndex
                        Case 0
                            _selectedProfile = _selectedProfile.Clamp(0, _MainProfiles.Count - 1)
                        Case 1
                            _selectedProfile = _selectedProfile.Clamp(0, _GameJoltProfiles.Count - 1)
                        Case 2, 3
                            _selectedProfile = _selectedProfile.Clamp(0, _OptionsProfiles.Count - 1)
                    End Select

                    If _fadeInMain = 1.0F Then
                        If Controls.Accept(False, True, True) Then
                            Select Case _GameJoltButtonIndex
                                Case 0
                                    SoundManager.PlaySound("select")
                                    ClickedProfile()
                                Case 1
                                    SoundManager.PlaySound("select")
                                    ButtonChangeMale()
                                Case 2
                                    SoundManager.PlaySound("select")
                                    ButtonChangeFemale()
                                Case 3
                                    SoundManager.PlaySound("select")
                                    ButtonChangeGenderless()
                                Case 4
                                    SoundManager.PlaySound("select")
                                    ButtonResetSave()
                            End Select
                        End If
                        If Controls.Dismiss(False, True, True) Then
                            SoundManager.PlaySound("select")
                            DismissProfile()
                            _GameJoltButtonIndex = 0
                        End If
                        ' Try to load the GameJolt profile once the player has logged in.
                        _MainProfiles(1).LoadGameJolt()
                    End If
                    If _menuIndex = 0 Then
                        If _MainProfiles(_selectedProfile).Loaded = False Then
                            _closingDisplay = True
                        Else
                            _closingDisplay = False
                        End If
                    End If

                    _sliderTarget = GetSliderTarget(_selectedProfile)
                    If _sliderPosition < _sliderTarget Then
                        _sliderPosition = MathHelper.Lerp(_sliderTarget, _sliderPosition, 0.8F)
                    ElseIf _sliderPosition > _sliderTarget Then
                        _sliderPosition = MathHelper.Lerp(_sliderTarget, _sliderPosition, 0.8F)
                    End If
                    If KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey1) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey2) Or MouseHandler.ButtonPressed(MouseHandler.MouseButtons.RightButton) Or ControllerHandler.ButtonPressed(Buttons.B) Then
                        Select Case _menuIndex
                            Case 0
                                'SetScreen(New TransitionScreen(CurrentScreen, Me.PreScreen, Color.White, False))
                                If CurrentScreen.Identification = Identifications.MainMenuScreen Then
                                    SetScreen(New PressStartScreen())
                                End If
                                SoundManager.PlaySound("select")
                            Case 1, 2
                                _menuIndex = 0
                                _selectedProfile = _selectedProfileTemp
                                _sliderTarget = GetSliderTarget(_selectedProfile)
                                SoundManager.PlaySound("select")
                            Case 3
                                _menuIndex = 1
                                _selectedProfile = _selectedProfileTemp
                                _sliderTarget = GetSliderTarget(_selectedProfile)
                                SoundManager.PlaySound("select")
                        End Select
                    End If
                    If KeyBoardHandler.KeyPressed(KeyBindings.ForwardMoveKey) Or KeyBoardHandler.KeyPressed(KeyBindings.UpKey) Then
                        Select Case _menuIndex
                            Case 1, 2
                                _menuIndex = 0
                                _selectedProfile = _selectedProfileTemp
                                _sliderTarget = GetSliderTarget(_selectedProfile)
                                SoundManager.PlaySound("select")
                            Case 3
                                _menuIndex = 1
                                _selectedProfile = _selectedProfileTemp
                                _sliderTarget = GetSliderTarget(_selectedProfile)
                                SoundManager.PlaySound("select")
                        End Select
                    End If

                    If _fadeInMain = 1.0F Then
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
                UpdateOptionsOffset()

                If (_menuIndex = 0 OrElse _menuIndex = 1) AndAlso _MainProfiles(_selectedProfile).GameMode <> "Kolben" Then
                    If GameModeSplash Is Nothing Then
                        Try
                            Dim fileName As String = GameController.GamePath & "\GameModes\" & _MainProfiles(_selectedProfile).GameMode & "\MainMenu.png"
                            If IO.File.Exists(fileName) = True Then
                                Using stream As IO.Stream = IO.File.Open(fileName, IO.FileMode.OpenOrCreate)
                                    GameModeSplash = Texture2D.FromStream(GraphicsDevice, stream)
                                End Using
                            End If
                        Catch ex As Exception
                            Logger.Log(Logger.LogTypes.ErrorMessage, "MainMenuScreen.vb/UpdateNewGameMenu: An error occurred trying to load the splash image at """ & GameController.GamePath & "\GameModes\" & _MainProfiles(_selectedProfile).GameMode & "\MainMenu.png"". This could have been caused by an invalid file header. (Exception: " & ex.Message & ")")
                        End Try
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ButtonChangeMale()
        If GameJoltSave.Gender = "Male" Then
            Exit Sub
        End If
        GameJoltSave.Gender = "Male"

        Core.Player.Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
        _MainProfiles(_selectedProfile).Sprite = GameJolt.Emblem.GetPlayerSprite(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
    End Sub
    Private Sub ButtonChangeFemale()
        If GameJoltSave.Gender = "Female" Then
            Exit Sub
        End If
        GameJoltSave.Gender = "Female"

        Core.Player.Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
        _MainProfiles(_selectedProfile).Sprite = GameJolt.Emblem.GetPlayerSprite(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
    End Sub
    Private Sub ButtonChangeGenderless()
        If GameJoltSave.Gender = "Other" Then
            Exit Sub
        End If
        GameJoltSave.Gender = "Other"

        Core.Player.Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
        _MainProfiles(_selectedProfile).Sprite = GameJolt.Emblem.GetPlayerSprite(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
    End Sub

    Private Sub ButtonResetSave()
        GameJoltSave.ResetSave()
        _MainProfiles(_selectedProfile).Sprite = GameJolt.Emblem.GetPlayerSprite(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
        _MainProfiles(_selectedProfile).SetToDefault()
    End Sub

    Private Sub ClickedProfile()
        Select Case _menuIndex
            Case 0
                If _selectedProfile = 1 And Security.FileValidation.IsValid(False) = False Then
                    _messageBox.Show(Localization.GetString("main_menu_error_filevalidation", "File validation failed!~Redownload the game's files to solve this problem.").Replace(CChar("~"), Environment.NewLine).Replace(CChar("*"), Environment.NewLine & Environment.NewLine))
                Else
                    _MainProfiles(_selectedProfile).SelectProfile()
                End If
            Case 2, 3
                _OptionsProfiles(_selectedProfile).SelectProfile()
        End Select
    End Sub

    Private Sub DismissProfile()
        _MainProfiles(_selectedProfile).UnSelectProfile()
    End Sub

    Private Sub UpdateScreenOffset()

        _screenOrigin = New Vector2(CInt((windowSize.Width / 2 - 80 - 180)), CInt(windowSize.Height / 4))
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
    Private Sub UpdateOptionsOffset()
        If _optionsOffset.X > _optionsOffsetTarget.X Then
            _optionsOffset.X = MathHelper.Lerp(_optionsOffsetTarget.X, _optionsOffset.X, 0.93F)
            If _optionsOffset.X - 0.01F <= _optionsOffsetTarget.X Then
                _optionsOffset.X = _optionsOffsetTarget.X
            End If
        End If
        If _optionsOffset.X < _optionsOffsetTarget.X Then
            _optionsOffset.X = MathHelper.Lerp(_optionsOffsetTarget.X, _optionsOffset.X, 0.93F)
            If _optionsOffset.X + 0.01F >= _optionsOffsetTarget.X Then
                _optionsOffset.X = _optionsOffsetTarget.X
            End If
        End If
        If _optionsOffset.Y > _optionsOffsetTarget.Y Then
            _optionsOffset.Y = MathHelper.Lerp(_optionsOffsetTarget.Y, _optionsOffset.Y, 0.93F)
            If _optionsOffset.Y - 0.01F <= _optionsOffsetTarget.Y Then
                _optionsOffset.Y = _optionsOffsetTarget.Y
            End If
        End If
        If _optionsOffset.Y < _optionsOffsetTarget.Y Then
            _optionsOffset.Y = MathHelper.Lerp(_optionsOffsetTarget.Y, _optionsOffset.Y, 0.93F)
            If _optionsOffset.Y + 0.01F >= _optionsOffsetTarget.Y Then
                _optionsOffset.Y = _optionsOffsetTarget.Y
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        If _loading = False Then
            Select Case _menuIndex
                Case 0
                    If _MainProfiles(_selectedProfile).IsGameJolt = True Then
                        DrawGradients(CInt(255 * _fadeInMain), True)
                    Else
                        DrawGradients(CInt(255 * _fadeInMain), False)
                    End If
                Case 1
                    If Not _GameJoltProfiles(_selectedProfile).IsOptionsMenuButton Then
                        DrawGradients(CInt(255 * _fadeInMain), True)
                    Else
                        DrawGradients(CInt(255 * _fadeInMain), False)
                    End If
                Case Else
                    DrawGradients(CInt(255 * _fadeInMain), False)
            End Select
        End If

        If IsCurrentScreen() Then
            If _loading Then
                Dim textSize As Vector2 = FontManager.InGameFont.MeasureString("Please wait..")
                GetFontRenderer().DrawString(FontManager.InGameFont, "Please wait" & LoadingDots.Dots, New Vector2(windowSize.Width / 2.0F - textSize.X / 2.0F, windowSize.Height / 2.0F - textSize.Y / 2.0F + 100), Color.White)
            Else
                If GameModeSplash IsNot Nothing Then
                    DrawGameModeSplash()
                End If
                Select Case _menuIndex
                    Case 1, 3
                        DrawOptionsProfiles(True)
                    Case Else
                        DrawOptionsProfiles(False)
                End Select
                DrawMainProfiles()
            End If
        End If
    End Sub
    Public Sub DrawGameModeSplash()
        Dim backSize As New Size(windowSize.Width, windowSize.Height)
        Dim origSize As New Size(GameModeSplash.Width, GameModeSplash.Height)
        Dim aspectRatio As Single = CSng(origSize.Width / origSize.Height)

        backSize.Width = CInt(windowSize.Width * aspectRatio)
        backSize.Height = CInt(backSize.Width / aspectRatio)

        If backSize.Width > backSize.Height Then
            backSize.Width = windowSize.Width
            backSize.Height = CInt(windowSize.Width / aspectRatio)
        Else
            backSize.Height = windowSize.Height
            backSize.Width = CInt(windowSize.Height / aspectRatio)
        End If
        If backSize.Height < windowSize.Height Then
            backSize.Height = windowSize.Height
            backSize.Width = CInt(windowSize.Height / origSize.Height * origSize.Width)
        End If
        Dim xOffset As Integer = 0
        If windowSize.Width < backSize.Width Then
            Dim xAspectRatio As Single = CSng(origSize.Width / backSize.Width)
            xOffset = CInt(Math.Floor((backSize.Width - windowSize.Width) * xAspectRatio) / 2)
        End If

        Core.SpriteBatch.Draw(GameModeSplash, New Rectangle(0, 0, backSize.Width, backSize.Height), New Rectangle(xOffset, 0, origSize.Width, origSize.Height), Color.White)
    End Sub
    Public Sub DrawGameJoltButtons(ByVal offset As Vector2)
        Dim r As New Rectangle(CInt(offset.X + 400), CInt(offset.Y + 200), 512, 128)
        Dim y As Integer = 0

        Dim fontColor As Color = Color.White
        Dim dayTime = World.GetTime
        If dayTime = World.DayTimes.Day OrElse dayTime = World.DayTimes.Morning Then
            fontColor = Color.Black
        End If

        If ScaleScreenRec(New Rectangle(r.X, r.Y, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible OrElse Not GameInstance.IsMouseVisible And _GameJoltButtonIndex = 1 Then
            y = 16

            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, "Change to male", New Vector2(r.X + 64 + 4, r.Y + 4), fontColor)
        End If
        SpriteBatch.DrawInterface(_oldMenuTexture, New Rectangle(r.X, r.Y, 32, 32), New Rectangle(144, 32 + y, 16, 16), Color.White)

        y = 0
        If ScaleScreenRec(New Rectangle(r.X, r.Y + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible OrElse Not GameInstance.IsMouseVisible And _GameJoltButtonIndex = 2 Then
            y = 16

            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, "Change to female", New Vector2(r.X + 64 + 4, r.Y + 4 + 48), fontColor)
        End If
        SpriteBatch.DrawInterface(_oldMenuTexture, New Rectangle(r.X, r.Y + 48, 32, 32), New Rectangle(160, 32 + y, 16, 16), Color.White)

        y = 0
        If ScaleScreenRec(New Rectangle(r.X, r.Y + 48 + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible OrElse Not GameInstance.IsMouseVisible And _GameJoltButtonIndex = 3 Then
            y = 16

            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, "Change to genderless", New Vector2(r.X + 64 + 4, r.Y + 4 + 48 + 48), fontColor)
        End If
        SpriteBatch.DrawInterface(_oldMenuTexture, New Rectangle(r.X, r.Y + 48 + 48, 32, 32), New Rectangle(208, 32 + y, 16, 16), Color.White)

        y = 0
        If ScaleScreenRec(New Rectangle(r.X, r.Y + 48 + 48 + 48, 32, 32)).Contains(MouseHandler.MousePosition) = True And GameInstance.IsMouseVisible OrElse Not GameInstance.IsMouseVisible And _GameJoltButtonIndex = 4 Then
            y = 16

            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, "Reset save", New Vector2(r.X + 64 + 4, r.Y + 4 + 48 + 48 + 48), fontColor)
        End If
        SpriteBatch.DrawInterface(_oldMenuTexture, New Rectangle(r.X, r.Y + 48 + 48 + 48, 32, 32), New Rectangle(176, 32 + y, 16, 16), Color.White)

    End Sub
    Private Sub DrawMainProfiles()

        For x = 0 To _MainProfiles.Count - 1
            ' Draw main profiles.
            Dim xmain As Boolean = x = _selectedProfile
            If _menuIndex <> 0 Then
                xmain = False
            End If
            Dim xOffset As Single = _screenOrigin.X + _mainOffset.X + x * 180 + ((x + 1) * 100 * (1 - _fadeInMain))

            _MainProfiles(x).Draw(New Vector2(CInt(xOffset), CInt(_screenOrigin.Y + _screenOffset.Y)), CInt(_fadeInMain * 255), (xmain), _menuTexture)
            If _MainProfiles(x).IsGameJolt AndAlso _MainProfiles(x).Loaded AndAlso (xmain) Then
                DrawGameJoltButtons(New Vector2(CInt(xOffset), CInt(_screenOrigin.Y + _screenOffset.Y)))
            End If
        Next

        If _fadeInMain = 1.0F And _menuIndex = 0 Then
            ' Draw arrow.
            If _MainProfiles(_selectedProfile).IsGameJolt = False Then
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(_screenOrigin.X + _sliderPosition - 16), CInt(_screenOrigin.Y + 170), 32, 16), New Rectangle(0, 16, 32, 16), New Color(255, 255, 255, CInt(_fadeInMain * 255)))
            Else
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(_screenOrigin.X + _sliderPosition - 16), CInt(_screenOrigin.Y + 170), 32, 16), New Rectangle(32, 16, 32, 16), Color.White)
            End If
            Dim displayRect = New Rectangle(CInt((_screenOrigin.X + _sliderPosition - 300).Clamp(20, windowSize.Width - 620)), CInt(_screenOrigin.Y + 170 + 16), 600, CInt(240 * _expandDisplay))

            ' Draw display.
            If _MainProfiles(_selectedProfile).IsGameJolt Then
                If _expandDisplay > 0F Then
                    Canvas.DrawRectangle(displayRect, Screens.UI.ColorProvider.MainColor(True))
                    Canvas.DrawRectangle(New Rectangle(displayRect.X, displayRect.Y + displayRect.Height - 3, displayRect.Width, 3), Screens.UI.ColorProvider.AccentColor(True, CInt(255 * _expandDisplay)))
                End If
            Else
                If _expandDisplay > 0F Then
                    Canvas.DrawRectangle(displayRect, Screens.UI.ColorProvider.MainColor(False))
                    Canvas.DrawRectangle(New Rectangle(displayRect.X, displayRect.Y + displayRect.Height - 3, displayRect.Width, 3), Screens.UI.ColorProvider.AccentColor(False, CInt(255 * _expandDisplay)))
                End If
            End If

            ' Draw profile info.
            Dim tmpProfile = _MainProfiles(_selectedProfile)

            If _expandDisplay = 1.0F Then
                If tmpProfile.GameModeExists Then
                    For i = 0 To tmpProfile.PokemonTextures.Count - 1
                        SpriteBatch.Draw(tmpProfile.PokemonTextures(i), New Rectangle(displayRect.X + 30 + i * 70, displayRect.Y + 70, 64, 64), Color.White)
                    Next
                    GetFontRenderer().DrawString(FontManager.InGameFont, Localization.GetString("main_menu_savefile_name", "Player Name") & ": " & tmpProfile.Name & Environment.NewLine &
                                                                            Localization.GetString("main_menu_savefile_gamemode", "GameMode") & ": " & GameModeManager.GetGameMode(tmpProfile.GameMode).Name, New Vector2(displayRect.X + 30, displayRect.Y + 20), Color.White, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
                    GetFontRenderer().DrawString(FontManager.InGameFont, Localization.GetString("main_menu_savefile_badges", "Badges") & ": " & tmpProfile.Badges.ToString() & Environment.NewLine &
                                                                            Localization.GetString("main_menu_savefile_playtime", "Play time") & ": " & tmpProfile.TimePlayed & Environment.NewLine &
                                                                            Localization.GetString("main_menu_savefile_location", "Location") & ": " & tmpProfile.Location, New Vector2(displayRect.X + 30, displayRect.Y + 150), Color.White, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
                Else
                    GetFontRenderer().DrawString(FontManager.InGameFont, Localization.GetString("main_menu_savefile_name", "Player Name") & ": " & tmpProfile.Name & Environment.NewLine &
                                                                            Localization.GetString("main_menu_savefile_gamemode", "GameMode") & ": " & tmpProfile.GameMode, New Vector2(displayRect.X + 30, displayRect.Y + 20), Color.White, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

                    SpriteBatch.Draw(_menuTexture, New Rectangle(displayRect.X + 30, displayRect.Y + 70, 32, 32), New Rectangle(0, 32, 32, 32), Color.White)
                    Dim errorText As String

                    If tmpProfile.IsGameJolt() Then
                        errorText = Localization.GetString("main_menu_error_gamejolt_1", "Download failed. Press Accept to try again.") & Environment.NewLine & Environment.NewLine &
                                       Localization.GetString("main_menu_error_gamejolt_2", "If the problem persists, please try again later") & Environment.NewLine &
                                        Localization.GetString("main_menu_error_gamejolt_3", "or contact us in our Discord server:") & Environment.NewLine & Environment.NewLine &
                                        Localization.GetString("main_menu_error_gamejolt_4", "http://www.discord.me/p3d")
                    Else
                        errorText = Localization.GetString("main_menu_error_gamemode_profile", "The required GameMode does not exist!")
                    End If
                    GetFontRenderer().DrawString(FontManager.InGameFont, errorText, New Vector2(displayRect.X + 70, displayRect.Y + 78), Color.White, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

                End If
            End If
        End If
    End Sub

    Private Sub DrawOptionsProfiles(IsGameJoltOptions As Boolean)
        ' Draw profiles.
        For x = 0 To _OptionsProfiles.Count - 1
            Dim xOffset As Single = _screenOrigin.X + _optionsOffset.X + x * 180

            _OptionsProfiles(x).Draw(New Vector2(CInt(xOffset), CInt(_screenOrigin.Y)), CInt(_fadeInOptions * 255), (x = _selectedProfile), _menuTexture)
        Next

        ' Draw arrow.
        Select Case _menuIndex
            Case 2
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(_screenOrigin.X + _sliderPosition - 16), CInt(_screenOrigin.Y + 170), 32, 16), New Rectangle(0, 16, 32, 16), Color.White)
            Case 3
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(_screenOrigin.X + _sliderPosition - 16), CInt(_screenOrigin.Y + 170), 32, 16), New Rectangle(32, 16, 32, 16), Color.White)
        End Select

    End Sub

    Private Function GetSliderTarget(ByVal index As Integer) As Integer
        Select Case _menuIndex
            Case 2, 3
                Return CInt(_optionsOffset.X + index * 180 + 80)
            Case 1
                Return CInt(_gameJoltOffset.X + index * 180 + 80)
            Case Else
                Return CInt(_screenOffset.X + index * 180 + 80)
        End Select
    End Function

    Public Overrides Sub ChangeTo()
        If _MainProfiles.Count = 0 Then
            'Dim t As New Threading.Thread(AddressOf LoadMainProfiles)
            't.IsBackground = True
            't.Start()
            LoadMainProfiles()
        End If
        If _OptionsProfiles.Count = 0 Then
            LoadOptionProfiles()
        End If
    End Sub

    Private Sub LoadMainProfiles()
        _MainProfiles.Add(New GameProfile("", False, True))
        _MainProfiles.Add(New GameProfile("", False, False))

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
                _MainProfiles.Add(New GameProfile(path, False, False))
            End If
        Next

        GameModeManager.SetGameModePointer("Kolben")

        _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
        _oldMenuTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        _MainProfiles.Add(New GameProfile("", True, False))

        _loading = False
    End Sub
    Private Sub LoadOptionProfiles()
        _OptionsProfiles.Add(New GameProfile("", False, False, 0))
        _OptionsProfiles.Add(New GameProfile("", False, False, 1))
        _OptionsProfiles.Add(New GameProfile("", False, False, 2))
        _OptionsProfiles.Add(New GameProfile("", False, False, 3))
    End Sub

    Private Class GameProfile

        Private _isGameJolt As Boolean = False
        Private _loaded As Boolean = False
        Private _isLoading As Boolean = False
        Private _failedGameJoltLoading As Boolean = False
        Public _OptionsMenuIndex As Integer = -1
        Private _path As String = ""
        Private _isNewGameButton As Boolean = False
        Private _IsOptionsMenuButton As Boolean = False

        Private _name As String = ""
        Public _gameMode As String
        Private _pokedexSeen As Integer
        Private _pokedexCaught As Integer
        Private _badges As Integer
        Private _timePlayed As String
        Private _location As String
        Private _pokemonTextures As New List(Of Texture2D)
        Private _sprite As Texture2D
        Public _gameModeExists As Boolean
        Private _skin As String = ""
        Private _surfing As Boolean = False
        Private _tempSurfSkin As String = ""

        Private _fontSize As Single = 1.0F
        Private _spriteIndex As Integer = 0
        Private _spriteDelay As Single = 1.5F
        Private _logoBounce As Single = 0F
        Private ReadOnly _spriteOrder As Integer() = {0, 1, 0, 2}

        Public Property Sprite As Texture2D
            Get
                Return _sprite
            End Get
            Set(value As Texture2D)
                _sprite = value
            End Set
        End Property

        Public ReadOnly Property IsGameJolt As Boolean
            Get
                Return _isGameJolt
            End Get
        End Property
        Public ReadOnly Property IsNewGameButton As Boolean
            Get
                Return _isNewGameButton
            End Get
        End Property

        Public ReadOnly Property IsOptionsMenuButton As Boolean
            Get
                Return _IsOptionsMenuButton
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

        Public Sub SetToDefault()
            _timePlayed = "00:00:00"
            _location = "Your Room"
            _pokemonTextures.Clear()
            _badges = 0
        End Sub

        Public Sub New(ByVal path As String, ByVal isNewGameButton As Boolean, ByVal IsOptionsMenuButton As Boolean, Optional ByVal OptionsMenuIndex As Integer = -1)
            If isNewGameButton Then
                _isNewGameButton = True
                _fontSize = 1.0F
            ElseIf IsOptionsMenuButton Then
                _IsOptionsMenuButton = True
                _fontSize = 1.0F
                _sprite = TextureManager.GetTexture("Textures\UI\OptionsMenu")
            Else
                If OptionsMenuIndex <> -1 Then
                    _OptionsMenuIndex = OptionsMenuIndex
                    Select Case _OptionsMenuIndex
                        Case 0
                            _sprite = TextureManager.GetTexture("Textures\UI\Options\Language")
                        Case 1
                            _sprite = TextureManager.GetTexture("Textures\UI\Options\Audio")
                        Case 2
                            _sprite = TextureManager.GetTexture("Textures\UI\Options\Controls")
                        Case 3
                            _sprite = TextureManager.GetTexture("Textures\UI\Options\ContentPacks")
                    End Select
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
            End If
        End Sub

        Private Sub LoadContent(ByVal pokedata As String)
            If GameModeManager.GameModeExists(_gameMode) Then
                _gameModeExists = True

                GameModeManager.SetGameModePointer(_gameMode)
                PokemonForms.Initialize()
                Dim pokemonData As String() = pokedata.SplitAtNewline()
                For Each line As String In pokemonData
                    If line.StartsWith("{") Then
                        _pokemonTextures.Add(Pokemon.GetPokemonByData(line).GetMenuTexture(True))
                    End If
                Next

                If _isGameJolt = False Then
                    If _surfing Then
                        _sprite = TextureManager.GetTexture("Textures\NPC\" & _tempSurfSkin)
                    Else
                        _sprite = TextureManager.GetTexture("Textures\NPC\" & _skin)
                    End If
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

                            While FontManager.InGameFont.MeasureString(_name).X * _fontSize > 140
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
                        Case "surfing"
                            If content = "1" Then
                                _surfing = True
                            End If
                        Case "tempsurfskin"
                            _tempSurfSkin = content

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
            ElseIf _menuIndex = 3 Then
                For x = 0 To 9
                    For y = 0 To 9
                        SpriteBatch.Draw(t, New Rectangle(CInt(x * 16 + offset.X), CInt(y * 16 + offset.Y), 16, 16), New Rectangle(32, 0, 16, 16), New Color(255, 255, 255, alpha))
                    Next
                Next
                Canvas.DrawRectangle(New Rectangle(CInt(offset.X), CInt(offset.Y), 160, 3), Screens.UI.ColorProvider.AccentColor(True, alpha))
            Else
                For x = 0 To 9
                    For y = 0 To 9
                        SpriteBatch.Draw(t, New Rectangle(CInt(x * 16 + offset.X), CInt(y * 16 + offset.Y), 16, 16), New Rectangle(0, 0, 16, 16), New Color(255, 255, 255, alpha))
                    Next
                Next
                Canvas.DrawRectangle(New Rectangle(CInt(offset.X), CInt(offset.Y), 160, 3), Screens.UI.ColorProvider.AccentColor(False, alpha))
            End If
            If _isNewGameButton Then
                Dim textA As String = Localization.GetString("main_menu_newgame_line1", "New")
                Dim textB As String = Localization.GetString("main_menu_newgame_line2", "Game")

                If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                    FontRenderer.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2 + 2), CInt(offset.Y + 72 - FontManager.InGameFont.MeasureString(textA).Y / 2 + 2)), New Color(0, 0, 0, alpha))
                    FontRenderer.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2), CInt(offset.Y + 72 - FontManager.InGameFont.MeasureString(textA).Y / 2)), New Color(255, 255, 255, alpha))

                    FontRenderer.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2 + 2), CInt(offset.Y + 72 + FontManager.InGameFont.MeasureString(textB).Y / 2 + 2)), New Color(0, 0, 0, alpha))
                    FontRenderer.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2), CInt(offset.Y + 72 + FontManager.InGameFont.MeasureString(textB).Y / 2)), New Color(255, 255, 255, alpha))
                Else
                    SpriteBatch.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2 + 2), CInt(offset.Y + 72 - FontManager.InGameFont.MeasureString(textA).Y / 2 + 2)), New Color(0, 0, 0, alpha))
                    SpriteBatch.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2), CInt(offset.Y + 72 - FontManager.InGameFont.MeasureString(textA).Y / 2)), New Color(255, 255, 255, alpha))

                    SpriteBatch.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2 + 2), CInt(offset.Y + 72 + FontManager.InGameFont.MeasureString(textB).Y / 2 + 2)), New Color(0, 0, 0, alpha))
                    SpriteBatch.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2), CInt(offset.Y + 72 + FontManager.InGameFont.MeasureString(textB).Y / 2)), New Color(255, 255, 255, alpha))

                End If
            ElseIf _IsOptionsMenuButton Then
                Dim text As String = Localization.GetString("main_menu_options", "Options")
                If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                    FontRenderer.DrawString(FontManager.InGameFont, text, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(text).X) / 2 + 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(text).Y) / 2 + 2)), New Color(0, 0, 0, alpha))
                    FontRenderer.DrawString(FontManager.InGameFont, text, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(text).X) / 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(text).Y) / 2)), New Color(255, 255, 255, alpha))
                Else
                    SpriteBatch.DrawString(FontManager.InGameFont, text, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(text).X) / 2 + 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(text).Y) / 2 + 2)), New Color(0, 0, 0, alpha))
                    SpriteBatch.DrawString(FontManager.InGameFont, text, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(text).X) / 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(text).Y) / 2)), New Color(255, 255, 255, alpha))
                End If
                If _menuIndex = 0 Then
                    If isSelected Then
                        _logoBounce += 0.2F
                    Else
                        _logoBounce = 0F
                    End If
                End If
                SpriteBatch.Draw(_sprite, New Rectangle(CInt(offset.X + 40), CInt(offset.Y + 36 + Math.Sin(_logoBounce) * 8.0F), 80, 80), New Color(255, 255, 255, alpha))

            ElseIf _OptionsMenuIndex <> -1 Then
                Dim textA As String = ""
                Dim textB As String = ""
                Select Case _OptionsMenuIndex
                    Case 0
                        textA = Localization.GetString("main_menu_options_language", "Language")
                    Case 1
                        textA = Localization.GetString("main_menu_options_audio", "Audio")
                    Case 2
                        textA = Localization.GetString("main_menu_options_controls", "Controls")
                    Case 3
                        textA = Localization.GetString("main_menu_options_contentpacks_line1", "Content")
                        textB = Localization.GetString("main_menu_options_contentpacks_line2", "Packs")
                End Select

                If _OptionsMenuIndex <> 3 Then
                    If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                        FontRenderer.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2 + 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(textA).Y) / 2 + 2)), New Color(0, 0, 0, alpha))
                        FontRenderer.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(textA).Y) / 2)), New Color(255, 255, 255, alpha))
                    Else
                        SpriteBatch.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2 + 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(textA).Y) / 2 + 2)), New Color(0, 0, 0, alpha))
                        SpriteBatch.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2), CInt(offset.Y + 132 - (FontManager.InGameFont.MeasureString(textA).Y) / 2)), New Color(255, 255, 255, alpha))
                    End If
                Else
                    If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                        FontRenderer.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2 + 2), CInt(offset.Y + 116 - (FontManager.InGameFont.MeasureString(textA).Y) / 2 + 2)), New Color(0, 0, 0, alpha))
                        FontRenderer.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2), CInt(offset.Y + 116 - (FontManager.InGameFont.MeasureString(textA).Y) / 2)), New Color(255, 255, 255, alpha))

                        FontRenderer.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2 + 2), CInt(offset.Y + 116 + FontManager.InGameFont.MeasureString(textB).Y / 2 + 2)), New Color(0, 0, 0, alpha))
                        FontRenderer.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2), CInt(offset.Y + 116 + FontManager.InGameFont.MeasureString(textB).Y / 2)), New Color(255, 255, 255, alpha))
                    Else
                        SpriteBatch.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2 + 2), CInt(offset.Y + 116 - (FontManager.InGameFont.MeasureString(textA).Y) / 2 + 2)), New Color(0, 0, 0, alpha))
                        SpriteBatch.DrawString(FontManager.InGameFont, textA, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textA).X) / 2), CInt(offset.Y + 116 - (FontManager.InGameFont.MeasureString(textA).Y) / 2)), New Color(255, 255, 255, alpha))

                        SpriteBatch.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2 + 2), CInt(offset.Y + 116 + FontManager.InGameFont.MeasureString(textB).Y / 2 + 2)), New Color(0, 0, 0, alpha))
                        SpriteBatch.DrawString(FontManager.InGameFont, textB, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(textB).X) / 2), CInt(offset.Y + 116 + FontManager.InGameFont.MeasureString(textB).Y / 2)), New Color(255, 255, 255, alpha))
                    End If
                End If

                If _menuIndex = 2 Or _menuIndex = 3 Then
                    If isSelected Then
                        _logoBounce += 0.2F
                    Else
                        _logoBounce = 0F
                    End If
                End If
                SpriteBatch.Draw(_sprite, New Rectangle(CInt(offset.X + 40), CInt(offset.Y + 24 + Math.Sin(_logoBounce) * 8.0F), 80, 80), New Color(255, 255, 255, alpha))

            Else
                If _loaded Then
                    Dim frameSize As Size
                    If _sprite.Width = _sprite.Height / 2 Then
                        frameSize = New Size(CInt(_sprite.Width / 2), CInt(_sprite.Height / 4))
                    ElseIf _sprite.Width = _sprite.Height Then
                        frameSize = New Size(CInt(_sprite.Width / 4), CInt(_sprite.Height / 4))
                    Else
                        frameSize = New Size(CInt(_sprite.Width / 3), CInt(_sprite.Height / 4))
                    End If

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
                        FontRenderer.DrawString(FontManager.InGameFont, _name, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(_name).X * _fontSize) / 2 + 2), CInt(offset.Y + 120 + 2)), New Color(0, 0, 0, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                        FontRenderer.DrawString(FontManager.InGameFont, _name, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(_name).X * _fontSize) / 2), CInt(offset.Y + 120)), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    Else
                        SpriteBatch.DrawString(FontManager.InGameFont, _name, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(_name).X * _fontSize) / 2 + 2), CInt(offset.Y + 120 + 2)), New Color(0, 0, 0, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                        SpriteBatch.DrawString(FontManager.InGameFont, _name, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(_name).X * _fontSize) / 2), CInt(offset.Y + 120)), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    End If
                Else
                    If _menuIndex = 0 Then
                        If isSelected Then
                            _logoBounce += 0.2F
                        Else
                            _logoBounce = 0F
                        End If
                    End If
                    Dim text As String = Localization.GetString("global_login", "Log in")
                    If _isLoading Then
                        text = Localization.GetString("global_loading", "Loading") & "..."
                    End If

                    SpriteBatch.Draw(_sprite, New Rectangle(CInt(offset.X + 46), CInt(offset.Y + 36 + Math.Sin(_logoBounce) * 8.0F), 68, 72), New Color(255, 255, 255, alpha))

                    If alpha >= 250 And CurrentScreen.Identification = Identifications.MainMenuScreen Then
                        FontRenderer.DrawString(FontManager.InGameFont, text, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(text).X * _fontSize) / 2), CInt(offset.Y + 120)), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
                    Else
                        SpriteBatch.DrawString(FontManager.InGameFont, text, New Vector2(CInt(offset.X + 80 - (FontManager.InGameFont.MeasureString(text).X * _fontSize) / 2), CInt(offset.Y + 120)), New Color(255, 255, 255, alpha), 0F, Vector2.Zero, New Vector2(_fontSize), SpriteEffects.None, 0F)
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
                _fontSize = 1.0F
                GameJolt.API.LoggedIn = False
            End If
        End Sub

        Public Sub SelectProfile()
            Select Case _menuIndex
                Case 0
                    If _isGameJolt And _loaded = False And GameJolt.API.LoggedIn = False Then
                        SetScreen(New GameJolt.LogInScreen(CurrentScreen))
                    ElseIf _isNewGameButton Then
                        World.IsMainMenu = False
                        ' Prompt GameMode selection screen:
                        SetScreen(New GameModeSelectionScreen(CurrentScreen))
                    Else
                        If _gameModeExists Then
                            GameModeManager.SetGameModePointer(_gameMode)
                            Localization.ReloadGameModeTokens()
                            Water.ClearAnimationResources()
                            Waterfall.ClearAnimationResources()
                            Water.AddDefaultWaterAnimationResources()
                            Waterfall.AddDefaultWaterAnimationResources()
                            AnimatedBlock.ClearAnimationResources()

                            World.IsMainMenu = False
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
                            ElseIf IsOptionsMenuButton = False Then
                                Dim messageBox As New UI.MessageBox(CurrentScreen)
                                messageBox.Show(Localization.GetString("main_menu_error_gamemode_message", "The required GameMode does not exist.~Reaquire the GameMode to play on this profile.").Replace("~", Environment.NewLine))
                            Else
                                _menuIndex = 2
                                _selectedProfileTemp = _selectedProfile
                                _selectedProfile = 0
                                'SetScreen(New NewOptionScreen(CurrentScreen))
                            End If
                        End If
                    End If
                Case 2, 3
                    SetScreen(New NewOptionScreen(CurrentScreen, _OptionsMenuIndex + 1))
            End Select
        End Sub

    End Class

End Class

Public Class GameModeSelectionScreen

    Inherits Screen

    Private _gameModes As GameMode()
    Private _index As Integer = 0
    Private _offset As Single = 0F

    Private tempGameModesDisplay As String = ""
    Private GameModeSplash As Texture2D = Nothing

    Private _menuTexture As Texture2D

    Private Const WIDTH = 320
    Private Const HEIGHT = 64
    Private Const GAP = 32

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.GameModeSelectionScreen
        CanBePaused = False
        CanChat = False
        CanDrawDebug = True
        CanGoFullscreen = True
        CanMuteAudio = True
        CanTakeScreenshot = True

        PreScreen = currentScreen
        _gameModes = GameModeManager.GetAllGameModes
        GameModeManager.SetGameModePointer(_gameModes(_index).DirectoryName)
        _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        If GameModeSplash IsNot Nothing Then
            DrawGameModeSplash()
        End If

        Dim text = Localization.GetString("gamemode_menu_select1", "Select a GameMode") + Environment.NewLine + Localization.GetString("gamemode_menu_select2", "to start the new game with.")

        GetFontRenderer().DrawString(FontManager.InGameFont, text, New Vector2(30, 30), Color.White)

        'Draw buttons
        Dim center = CInt(windowSize.Width / 2 + 320)
        For i = 0 To _gameModes.Length - 1
            Dim ButtonY = CInt(i * (HEIGHT + GAP) + _offset + windowSize.Height / 2 - HEIGHT / 2)
            Dim halfWidth = CInt(WIDTH / 2)
            Dim ButtonColor = New Rectangle(0, 0, 16, 16)
            Dim ButtonAccent = Screens.UI.ColorProvider.AccentColor(False, CInt(255))
            If i <> _index Then
                ButtonColor = New Rectangle(40, 48, 16, 16)
                ButtonAccent = Screens.UI.ColorProvider.MainColor(False, CInt(255))
            End If

            Dim displayText = _gameModes(i).Name.CropStringToWidth(FontManager.InGameFont, WIDTH - 32)

            If displayText = "Kolben" Then
                displayText = "Pokémon 3D"
            End If
            For x = 0 To CInt(WIDTH / 16)
                For y = 0 To CInt(HEIGHT / 16)
                    SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(x * 16 + (center - halfWidth)), CInt(y * 16 + ButtonY - 8), 16, 16), ButtonColor, Color.White)
                    Canvas.DrawRectangle(New Rectangle(CInt(center - halfWidth), CInt(ButtonY - 8), WIDTH + 16, 3), ButtonAccent)
                Next
            Next

            Dim textSize = FontManager.InGameFont.MeasureString(displayText)

            GetFontRenderer().DrawString(FontManager.InGameFont, displayText, New Vector2(center - halfWidth + 32 + 2, CType(ButtonY + HEIGHT / 2 - (textSize.Y / 2) + 2, Integer)), Color.Black, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
            GetFontRenderer().DrawString(FontManager.InGameFont, displayText, New Vector2(center - halfWidth + 32, CType(ButtonY + HEIGHT / 2 - (textSize.Y / 2), Integer)), Color.White, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
        Next

        'Draw GameMode description box
        If tempGameModesDisplay = "" Then
            Dim GameMode As GameMode = GameModeManager.GetGameMode(_gameModes(_index).DirectoryName)

            Dim dispName As String = GameMode.Name
            If dispName = "Kolben" Then
                dispName = "Pokémon 3D"
            End If
            Dim dispDescription As String = GameMode.Description.Replace("~", Environment.NewLine).Replace("*", Environment.NewLine)
            Dim dispVersion As String = GameMode.Version
            Dim dispAuthor As String = GameMode.Author

            tempGameModesDisplay = Localization.GetString("gamemode_menu_name") & ": " & dispName & Environment.NewLine &
                Localization.GetString("gamemode_menu_version") & ": " & dispVersion & Environment.NewLine &
                Localization.GetString("gamemode_menu_author") & ": " & dispAuthor & Environment.NewLine &
                Localization.GetString("gamemode_menu_description") & ": " & dispDescription
        End If

        tempGameModesDisplay = tempGameModesDisplay.CropStringToWidth(FontManager.InGameFont, 400)
        Dim displayRect = New Rectangle(CInt(windowSize.Width / 2 - 400 - 32), CInt(windowSize.Height / 2 - FontManager.InGameFont.MeasureString(tempGameModesDisplay).Y / 2 - 32), 480 + 32, CInt(FontManager.InGameFont.MeasureString(tempGameModesDisplay).Y + 64))
        Dim displayWidth = CInt(displayRect.Width / 16)
        Dim displayHeight = CInt(displayRect.Height / 16)

        For x = 0 To displayWidth
            For y = 0 To displayHeight
                SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(x * 16 + displayRect.X), CInt(y * 16) + displayRect.Y - 8, 16, 16), New Rectangle(0, 0, 16, 16), Color.White)
            Next
        Next
        Canvas.DrawRectangle(New Rectangle(displayRect.X, displayRect.Y - 8, displayWidth * 16 + 16, 3), Screens.UI.ColorProvider.AccentColor(False, CInt(255)))

        SpriteBatch.DrawString(FontManager.InGameFont, tempGameModesDisplay, New Vector2(displayRect.X + 32 + 2, displayRect.Y + 32 + 2), Color.Black)
        SpriteBatch.DrawString(FontManager.InGameFont, tempGameModesDisplay, New Vector2(displayRect.X + 32, displayRect.Y + 32), Color.White)

        'Draw Arrow
        SpriteBatch.Draw(_menuTexture, New Rectangle(CInt(displayRect.X + displayRect.Width + 16), CInt(displayRect.Y + (FontManager.InGameFont.MeasureString(tempGameModesDisplay).Y + 64) / 2 - 16), 16, 32), New Rectangle(64, 0, 16, 32), Color.White)

    End Sub

    Public Sub DrawGameModeSplash()
        Dim backSize As New Size(windowSize.Width, windowSize.Height)
        Dim origSize As New Size(GameModeSplash.Width, GameModeSplash.Height)
        Dim aspectRatio As Single = CSng(origSize.Width / origSize.Height)

        backSize.Width = CInt(windowSize.Width * aspectRatio)
        backSize.Height = CInt(backSize.Width / aspectRatio)

        If backSize.Width > backSize.Height Then
            backSize.Width = windowSize.Width
            backSize.Height = CInt(windowSize.Width / aspectRatio)
        Else
            backSize.Height = windowSize.Height
            backSize.Width = CInt(windowSize.Height / aspectRatio)
        End If
        If backSize.Height < windowSize.Height Then
            backSize.Height = windowSize.Height
            backSize.Width = CInt(windowSize.Height / origSize.Height * origSize.Width)
        End If
        Dim xOffset As Integer = 0
        If windowSize.Width < backSize.Width Then
            Dim xAspectRatio As Single = CSng(origSize.Width / backSize.Width)
            xOffset = CInt(Math.Floor((backSize.Width - windowSize.Width) * xAspectRatio) / 2)
        End If

        Core.SpriteBatch.Draw(GameModeSplash, New Rectangle(0, 0, backSize.Width, backSize.Height), New Rectangle(xOffset, 0, origSize.Width, origSize.Height), Color.White)
    End Sub

    Public Overrides Sub Update()
        ' PreScreen is the MainMenuScreen, so update the previous screen of that to achieve the background world.
        PreScreen.PreScreen.Update()

        If _index > 0 AndAlso Controls.Up(True, True, True, True, True, True) Then
            _index -= 1
            GameModeManager.SetGameModePointer(_gameModes(_index).DirectoryName)
            tempGameModesDisplay = ""
            GameModeSplash = Nothing
            _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
        End If
        If _index < _gameModes.Length - 1 AndAlso Controls.Down(True, True, True, True, True, True) Then
            _index += 1
            GameModeManager.SetGameModePointer(_gameModes(_index).DirectoryName)
            tempGameModesDisplay = ""
            GameModeSplash = Nothing
            _menuTexture = TextureManager.GetTexture("GUI\Menus\MainMenu")
        End If
        If KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey1) Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey2) Or MouseHandler.ButtonPressed(MouseHandler.MouseButtons.RightButton) Or ControllerHandler.ButtonPressed(Buttons.B) Then
            SoundManager.PlaySound("select")
            SetScreen(PreScreen)
        End If
        If Controls.Accept(True, True, True) Then
            GameModeManager.SetGameModePointer(_gameModes(_index).DirectoryName)
            
            Localization.ReloadGameModeTokens()
            SoundManager.PlaySound("select")
            If GameModeManager.ActiveGameMode.IntroType = "0" Then
                SetScreen(New TransitionScreen(Me.PreScreen, New NewGameScreen(), Color.Black, False))
            Else
                SetScreen(New Screens.MainMenu.NewNewGameScreen(PreScreen))
            End If
        End If

        Dim targetOffset = GetTargetOffset()

        If _offset <> targetOffset Then
            _offset = MathHelper.Lerp(_offset, targetOffset, 0.25F)
            If Math.Abs(_offset - targetOffset) <= 0.01F Then
                _offset = targetOffset
            End If
        End If

        If GameModeSplash Is Nothing Then
            Try
                Dim fileName As String = GameController.GamePath & "\GameModes\" & _gameModes(_index).DirectoryName & "\GameMode.png"
                If IO.File.Exists(fileName) = True Then
                    Using stream As IO.Stream = IO.File.Open(fileName, IO.FileMode.OpenOrCreate)
                        GameModeSplash = Texture2D.FromStream(GraphicsDevice, stream)
                    End Using
                End If
            Catch ex As Exception
                Logger.Log(Logger.LogTypes.ErrorMessage, "MainMenuScreen.vb/UpdateNewGameMenu: An error occurred trying to load the splash image at """ & GameController.GamePath & "\GameModes\" & _gameModes(_index).DirectoryName & "\GameMode.png"". This could have been caused by an invalid file header. (Exception: " & ex.Message & ")")
            End Try
        End If
    End Sub

    Private Function GetTargetOffset() As Integer
        Return -_index * (HEIGHT + GAP)
    End Function

End Class