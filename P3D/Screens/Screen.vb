Imports net.Pokemon3D.Game
''' <summary>
''' The base class for all screens in the game.
''' </summary>
Public MustInherit Class Screen

    ''' <summary>
    ''' The ID for a screen.
    ''' </summary>
    Public Enum Identifications
        MainMenuScreen
        OverworldScreen
        MenuScreen
        PokedexSelectScreen
        PokedexScreen
        PokedexViewScreen
        PokedexSearchScreen
        PokedexHabitatScreen
        PartyScreen
        SummaryScreen
        InventoryScreen
        BerryScreen
        TrainerScreen
        PauseScreen
        SaveScreen
        NewGameScreen
        OptionScreen
        StorageSystemScreen
        TradeScreen
        MapScreen
        ChatScreen
        UseItemScreen
        ItemDetailScreen
        ChooseItemScreen
        ChoosePokemonScreen
        EvolutionScreen
        ApricornScreen
        TransitionScreen
        BattleCatchScreen
        BlackOutScreen
        BattlePokemonScreen
        CreditsScreen
        DonationScreen
        NameObjectScreen
        LearnAttackScreen
        SecretBaseScreen
        PokegearScreen
        BattleGrowStatsScreen
        DaycareScreen
        HatchEggScreen
        SplashScreen
        GameJoltLoginScreen
        GameJoltUserViewerScreen
        GameJoltLobbyScreen
        GameJoltAddFriendScreen
        ChooseAttackScreen
        BattleScreen
        BattleIniScreen
        BattleAnimationScreen
        GTSMainScreen
        GTSInboxScreen
        GTSSearchScreen
        GTSSelectLevelScreen
        GTSSelectPokemonScreen
        GTSSelectGenderScreen
        GTSSetupScreen
        GTSEditTradeScreen
        GTSSelectAreaScreen
        GTSSelectUserScreen
        GTSTradeScreen
        GTSTradingScreen
        TeachMovesScreen
        OfflineGameWarningScreen
        HallofFameScreen
        ViewModelScreen
        MailSystemScreen
        PVPLobbyScreen
        InputScreen
        JoinServerScreen
        ConnectScreen
        AddServerScreen
        MysteryEventScreen
        DirectTradeScreen
        StorageSystemFilterScreen
        WonderTradeScreen
        RegisterBattleScreen
        StatisticsScreen
        MapPreviewScreen
        KeyBindingScreen
        MessageBoxScreen
        PressStartScreen
        CharacterSelectionScreen
        GameModeSelectionScreen
        VoltorbFlipScreen

        'TEMPORARY, OLD
        PokemonScreen
        IntroScreen
        PokemonStatusScreen
    End Enum

#Region "Shared values"

    ''' <summary>
    ''' A global camera instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property Camera() As Camera

    Private Shared _globalLevel As Level
    ''' <summary>
    ''' A global level instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property Level() As Level
        Get
            Return _globalLevel
        End Get
        Set(value As Level)
            If _globalLevel IsNot Nothing Then
                _globalLevel.StopOffsetMapUpdate()
            End If
            _globalLevel = value
            _globalLevel.StartOffsetMapUpdate()
        End Set
    End Property

    ''' <summary>
    ''' A global BasicEffect instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property Effect() As BasicEffectWithAlphaTest

    ''' <summary>
    ''' A global SkyDome instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property SkyDome() As SkyDome

    ''' <summary>
    ''' A global TextBox instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property TextBox() As TextBox = New TextBox()

    ''' <summary>
    ''' A global ChooseBox instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property ChooseBox() As ChooseBox = New ChooseBox()

    ''' <summary>
    ''' A global PokemonImageView instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property PokemonImageView() As PokemonImageView = New PokemonImageView()
    ''' <summary>
    ''' A global ImageView instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property ImageView() As ImageView = New ImageView()

#End Region

#Region "Fields"

    ''' <summary>
    ''' A value to store the screen that underlies the current screen in.
    ''' </summary>
    Public Property PreScreen() As Screen = Nothing

    ''' <summary>
    ''' The ID of the screen.
    ''' </summary>
    Public Property Identification() As Identifications = Identifications.MainMenuScreen

    ''' <summary>
    ''' Wether the mouse is visible on the screen.
    ''' </summary>
    Public Property MouseVisible() As Boolean = False

    ''' <summary>
    ''' Wether the game can be paused when pressing Escape.
    ''' </summary>
    Public Property CanBePaused() As Boolean = True

    ''' <summary>
    ''' Wether the game can be muted by pressing M (default).
    ''' </summary>
    Public Property CanMuteAudio() As Boolean = True

    ''' <summary>
    ''' Wether the ChatScreen can be opened by pressing T (default).
    ''' </summary>
    Public Property CanChat() As Boolean = True

    ''' <summary>
    ''' Wether a screenshot can be taken by pressing F2 (default).
    ''' </summary>
    Public Property CanTakeScreenshot() As Boolean = True

    ''' <summary>
    ''' Wether the debug information can be drawn on the screen.
    ''' </summary>
    Public Property CanDrawDebug() As Boolean = True

    ''' <summary>
    ''' Wether the game can switch its fullscreen state by pressing F11 (default).
    ''' </summary>
    Public Property CanGoFullscreen() As Boolean = True

    ''' <summary>
    ''' Wether this screen draws gradients.
    ''' </summary>
    Protected Property IsDrawingGradients() As Boolean = False

    ''' <summary>
    ''' If this screen completely overlays its PreScreen.
    ''' </summary>
    Public Property IsOverlay() As Boolean = False

    Public UpdateFadeOut As Boolean = False 'Sets if the screen gets updated during its set as a FadeOut screen on the TransitionScreen.
    Public UpdateFadeIn As Boolean = False 'Sets if the screen gets updated during its set as a FadeIn screen on the TransitionScreen.

#End Region

    ''' <summary>
    ''' Copies variables from another screen.
    ''' </summary>
    ''' <param name="scr">The source screen.</param>
    Protected Sub CopyFrom(ByVal scr As Screen)
        _MouseVisible = scr._MouseVisible
        _CanBePaused = scr._CanBePaused
        _CanMuteAudio = scr._CanMuteAudio
        _CanChat = scr._CanChat
        _CanTakeScreenshot = scr._CanTakeScreenshot
        _CanDrawDebug = scr._CanDrawDebug
        _CanGoFullscreen = scr._CanGoFullscreen
    End Sub

    ''' <summary>
    ''' The base draw function of a screen.
    ''' </summary>
    ''' <remarks>Contains no default code.</remarks>
    Public Overridable Overloads Sub Draw()

    End Sub

    ''' <summary>
    ''' The base render function of the screen. Used to render models above sprites.
    ''' </summary>
    Public Overridable Overloads Sub Render()

    End Sub

    ''' <summary>
    ''' The base update function of a screen.
    ''' </summary>
    ''' <remarks>Contains no default code.</remarks>
    Public Overridable Overloads Sub Update()

    End Sub

    ''' <summary>
    ''' An event that gets raised when this screen gets changed to.
    ''' </summary>
    ''' <remarks>Contains no default code.</remarks>
    Public Overridable Sub ChangeTo()

    End Sub

    ''' <summary>
    ''' An event that gets raised when this screen gets changed from.
    ''' </summary>
    ''' <remarks>Contains no default code.</remarks>
    Public Overridable Sub ChangeFrom()

    End Sub

    ''' <summary>
    ''' Returns if this screen instance is the currently active screen (set in the global Basic.CurrentScreen).
    ''' </summary>
    ''' <returns></returns>
    Public Function IsCurrentScreen() As Boolean
        If CurrentScreen.Identification = Identification Then 'If the screen stored in the CurrentScreen field has the same ID as this screen, return true.
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' An event that gets raised when the window handle size changed.
    ''' </summary>
    ''' <remarks>Contains no default code.</remarks>
    Public Overridable Sub SizeChanged()

    End Sub

    ''' <summary>
    ''' A void that gets raised when the mute option of the game gets toggled.
    ''' </summary>
    ''' <remarks>Contains no default code.</remarks>
    Public Overridable Sub ToggledMute()

    End Sub

    ''' <summary>
    ''' An event that is getting raised when the Escape button is getting pressed. The PauseScreen is getting brought up if the CanBePaused field is set to true.
    ''' </summary>
    Public Overridable Sub EscapePressed()
        'If the game can be paused on this screen, open the PauseScreen.
        If CurrentScreen.CanBePaused = True Then
            SetScreen(New PauseScreen(CurrentScreen))
        End If
    End Sub

    ''' <summary>
    ''' Draws XBOX controls on the bottom right of the screen.
    ''' </summary>
    ''' <param name="Descriptions">The button types and descriptions.</param>
    ''' <remarks>Calculates the position and calls DrawGamePadControls(Descriptions,Position)</remarks>
    Public Sub DrawGamePadControls(ByVal Descriptions As Dictionary(Of Buttons, String))
        Dim x As Integer = windowSize.Width 'Store the x position of the start of the controls render.

        'Loop through the buttons and add to the x location.
        For i = 0 To Descriptions.Count - 1
            Select Case Descriptions.Keys(i)
                Case Buttons.A, Buttons.B, Buttons.X, Buttons.Y, Buttons.Start, Buttons.Back, Buttons.LeftStick, Buttons.RightStick, Buttons.LeftTrigger, Buttons.RightTrigger
                    x -= 32 + 4
                Case Buttons.LeftShoulder, Buttons.RightShoulder
                    x -= 64 + 4
            End Select

            'Add to the x location for the length of the string and a separator.
            x -= CInt(FontManager.MainFont.MeasureString(Descriptions.Values(i)).X) + 16
        Next

        'Finally, render the buttons:
        DrawGamePadControls(Descriptions, New Vector2(x, windowSize.Height - 40))
    End Sub

    ''' <summary>
    ''' Generic void to render XBOX Gamepad controls on the screen.
    ''' </summary>
    ''' <param name="Descriptions">The button types and descriptions.</param>
    ''' <param name="Position">The position to draw the buttons.</param>
    Public Sub DrawGamePadControls(ByVal Descriptions As Dictionary(Of Buttons, String), ByVal Position As Vector2)
        'Only if a Gamepad is connected and the screen is active, render the buttons:
        If GamePad.GetState(PlayerIndex.One).IsConnected = True And Core.GameOptions.GamePadEnabled = True And IsCurrentScreen() = True Then
            'Transform the position to integers and store the current drawing position:
            Dim x As Integer = CInt(Position.X)
            Dim y As Integer = CInt(Position.Y)

            'Loop through the button list:
            For i = 0 To Descriptions.Count - 1
                Dim t As String = "GUI\GamePad\xboxController" 'Store the texture path.
                Dim width As Integer = 32 'Store the width of the image.
                Dim height As Integer = 32 'Store the height of the image.

                'Get the correct button image and size (currently, all buttons use the same size of 32x32 pixels).
                Select Case Descriptions.Keys(i)
                    Case Buttons.A
                        t &= "ButtonA"
                    Case Buttons.B
                        t &= "ButtonB"
                    Case Buttons.X
                        t &= "ButtonX"
                    Case Buttons.Y
                        t &= "ButtonY"
                    Case Buttons.LeftShoulder
                        t &= "LeftShoulder"
                    Case Buttons.RightShoulder
                        t &= "RightShoulder"
                    Case Buttons.LeftStick
                        t &= "LeftStick"
                    Case Buttons.RightStick
                        t &= "RightStick"
                    Case Buttons.LeftTrigger
                        t &= "LeftTrigger"
                    Case Buttons.RightTrigger
                        t &= "RightTrigger"
                    Case Buttons.Start
                        t &= "Start"
                    Case Buttons.Back
                        t &= "Back"
                End Select

                'Draw the buttons (first, the "shadow" with a black color, then the real button).
                SpriteBatch.Draw(TextureManager.GetTexture(t), New Rectangle(x + 2, y + 2, width, height), Color.Black)
                SpriteBatch.Draw(TextureManager.GetTexture(t), New Rectangle(x, y, width, height), Color.White)

                'Add the button width and a little offset to the drawing position:
                x += width + 4

                'Draw the button description (again, with a shadow):
                SpriteBatch.DrawString(FontManager.MainFont, Descriptions.Values(i), New Vector2(x + 3, y + 7), Color.Black)
                SpriteBatch.DrawString(FontManager.MainFont, Descriptions.Values(i), New Vector2(x, y + 4), Color.White)

                'Add the text width and the offset for the next button description to the drawing position:
                x += CInt(FontManager.MainFont.MeasureString(Descriptions.Values(i)).X) + 16
            Next
        End If
    End Sub

    ''' <summary>
    ''' Renders fading gradients.
    ''' </summary>
    ''' <param name="alpha">The alpha to draw the gradients at.</param>
    Protected Sub DrawGradients(ByVal alpha As Integer)
        DrawGradients(alpha, Screens.UI.ColorProvider.IsGameJolt)
    End Sub

    Protected Sub DrawGradients(ByVal alpha As Integer, ByVal isGameJolt As Boolean)
        Dim canDrawGradients As Boolean = True
        Dim s As Screen = Me

        While s.PreScreen IsNot Nothing And canDrawGradients = True
            If s._IsOverlay = False Then
                s = s.PreScreen
                If s.IsDrawingGradients = True Then
                    canDrawGradients = False
                End If
            Else
                Exit While
            End If
        End While

        If canDrawGradients = True Then
            Dim c As Color = Screens.UI.ColorProvider.GradientColor(isGameJolt, 0)
            Dim cA As Color = Screens.UI.ColorProvider.GradientColor(isGameJolt, alpha)

            Canvas.DrawGradient(New Rectangle(0, 0, CInt(windowSize.Width), 200), cA, c, False, -1)
            Canvas.DrawGradient(New Rectangle(0, CInt(windowSize.Height - 200), CInt(windowSize.Width), 200), c, cA, False, -1)
        End If
    End Sub

    ''' <summary>
    ''' Returns the screen status of the current screen. Override this function to return a screen state.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function GetScreenStatus() As String
        '// Return the generic "not implemented" message:
        Return "Screen state not implemented for screen class: " & Identification.ToString()
    End Function

    ''' <summary>
    ''' Returns the minimum size for the screen size to display a large interface before switching to the small size.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>The default size is 800x620 pixels.</remarks>
    Public Overridable Function GetScreenScaleMinimum() As Size
        '// Default size: 800x620 pixels.
        Return New Size(800, 620)
    End Function

    ''' <summary>
    ''' Returns the spritebatch that should render a font.
    ''' </summary>
    ''' <returns></returns>
    Protected Overridable Function GetFontRenderer() As SpriteBatch
        If IsCurrentScreen() Then
            Return FontRenderer
        Else
            Return SpriteBatch
        End If
    End Function

End Class