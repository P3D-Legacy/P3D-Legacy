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
        PokemonScreen
        PokemonStatusScreen
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
        IntroScreen
    End Enum

#Region "Shared values"

    Private Shared _globalCamera As Camera
    Private Shared _globalBasicEffect As BasicEffect
    Private Shared _globalLevel As Level
    Private Shared _globalSkyDome As SkyDome
    Private Shared _globalTextBox As New TextBox
    Private Shared _globalChooseBox As New ChooseBox
    Private Shared _globalPokemonImageView As New PokemonImageView

    ''' <summary>
    ''' A global camera instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property Camera() As Camera
        Get
            Return _globalCamera
        End Get
        Set(value As Camera)
            _globalCamera = value
        End Set
    End Property

    ''' <summary>
    ''' A global level instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property Level() As Level
        Get
            Return _globalLevel
        End Get
        Set(value As Level)
            _globalLevel = value
        End Set
    End Property

    ''' <summary>
    ''' A global BasicEffect instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property Effect() As BasicEffect
        Get
            Return _globalBasicEffect
        End Get
        Set(value As BasicEffect)
            _globalBasicEffect = value
        End Set
    End Property

    ''' <summary>
    ''' A global SkyDome instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property SkyDome() As SkyDome
        Get
            Return _globalSkyDome
        End Get
        Set(value As SkyDome)
            _globalSkyDome = value
        End Set
    End Property

    ''' <summary>
    ''' A global TextBox instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property TextBox() As TextBox
        Get
            Return _globalTextBox
        End Get
        Set(value As TextBox)
            _globalTextBox = value
        End Set
    End Property

    ''' <summary>
    ''' A global ChooseBox instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property ChooseBox() As ChooseBox
        Get
            Return _globalChooseBox
        End Get
        Set(value As ChooseBox)
            _globalChooseBox = value
        End Set
    End Property

    ''' <summary>
    ''' A global PokemonImageView instance, that carries over screen instances.
    ''' </summary>
    Public Shared Property PokemonImageView() As PokemonImageView
        Get
            Return _globalPokemonImageView
        End Get
        Set(value As PokemonImageView)
            _globalPokemonImageView = value
        End Set
    End Property

#End Region

#Region "Fields"

    Private _preScreen As Screen = Nothing
    Private _identification As Identifications = Identifications.MainMenuScreen 'Some default value I guess...

    Private _mouseVisible As Boolean = False
    Private _canBePaused As Boolean = True
    Private _canMuteMusic As Boolean = True
    Private _canChat As Boolean = True
    Private _canTakeScreenshot As Boolean = True
    Private _canDrawDebug As Boolean = True
    Private _canGoFullscreen As Boolean = True

    ''' <summary>
    ''' A value to store the screen that underlies the current screen in.
    ''' </summary>
    Public Property PreScreen() As Screen
        Get
            Return Me._preScreen
        End Get
        Set(value As Screen)
            Me._preScreen = value
        End Set
    End Property

    ''' <summary>
    ''' The ID of the screen.
    ''' </summary>
    Public Property Identification() As Identifications
        Get
            Return Me._identification
        End Get
        Set(value As Identifications)
            Me._identification = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the mouse is visible on the screen.
    ''' </summary>
    ''' <remarks>The default value is "False".</remarks>
    Public Property MouseVisible() As Boolean
        Get
            Return Me._mouseVisible
        End Get
        Set(value As Boolean)
            Me._mouseVisible = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the game can be paused when pressing Escape.
    ''' </summary>
    ''' <remarks>The default value is "True".</remarks>
    Public Property CanBePaused() As Boolean
        Get
            Return Me._canBePaused
        End Get
        Set(value As Boolean)
            Me._canBePaused = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the game can be muted by pressing M (default).
    ''' </summary>
    ''' <remarks>The default value is "True".</remarks>
    Public Property CanMuteMusic() As Boolean
        Get
            Return Me._canMuteMusic
        End Get
        Set(value As Boolean)
            Me._canMuteMusic = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the ChatScreen can be opened by pressing T (default).
    ''' </summary>
    ''' <remarks>The default value is "True".</remarks>
    Public Property CanChat() As Boolean
        Get
            Return Me._canChat
        End Get
        Set(value As Boolean)
            Me._canChat = value
        End Set
    End Property

    ''' <summary>
    ''' Wether a screenshot can be taken by pressing F2 (default).
    ''' </summary>
    ''' <remarks>The default value is "True".</remarks>
    Public Property CanTakeScreenshot() As Boolean
        Get
            Return Me._canTakeScreenshot
        End Get
        Set(value As Boolean)
            Me._canTakeScreenshot = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the debug information can be drawn on the screen.
    ''' </summary>
    ''' <remarks>The default value is "True".</remarks>
    Public Property CanDrawDebug() As Boolean
        Get
            Return Me._canDrawDebug
        End Get
        Set(value As Boolean)
            Me._canDrawDebug = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the game can switch its fullscreen state by pressing F11 (default).
    ''' </summary>
    ''' <remarks>The default value is "True".</remarks>
    Public Property CanGoFullscreen() As Boolean
        Get
            Return Me._canGoFullscreen
        End Get
        Set(value As Boolean)
            Me._canGoFullscreen = value
        End Set
    End Property

    Public UpdateFadeOut As Boolean = False 'Sets if the screen gets updated during its set as a FadeOut screen on the TransitionScreen.
    Public UpdateFadeIn As Boolean = False 'Sets if the screen gets updated during its set as a FadeIn screen on the TransitionScreen.

#End Region

    ''' <summary>
    ''' Sets all default fields of the screen instance.
    ''' </summary>
    ''' <param name="Identification">The ID of the screen.</param>
    ''' <param name="MouseVisible">Sets if the mouse is visible on the screen.</param>
    ''' <param name="CanBePaused">Sets if the PauseScreen can be opened by pressing Escape.</param>
    ''' <param name="CanMuteMusic">Sets if the M button (default) can mute the music.</param>
    ''' <param name="CanChat">Sets if the T button (default) can open the chat screen.</param>
    ''' <param name="CanTakeScreenshot">Sets if the F2 button (default) can take a screenshot.</param>
    ''' <param name="CanDrawDebug">Sets if the debug information can be drawn on this screen.</param>
    ''' <param name="CanGoFullscreen">Sets if the F11 button (default) can sets the game to fullscreen (or back).</param>
    Private Sub Setup(ByVal Identification As Identifications, ByVal MouseVisible As Boolean, ByVal CanBePaused As Boolean, ByVal CanMuteMusic As Boolean, ByVal CanChat As Boolean, ByVal CanTakeScreenshot As Boolean, ByVal CanDrawDebug As Boolean, ByVal CanGoFullscreen As Boolean)
        Me.Identification = Identification
        Me.MouseVisible = MouseVisible
        Me.CanBePaused = CanBePaused
        Me.CanChat = CanChat
        Me.CanDrawDebug = CanDrawDebug
        Me.CanGoFullscreen = CanGoFullscreen
        Me.CanMuteMusic = CanMuteMusic
        Me.CanTakeScreenshot = CanTakeScreenshot
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
    ''' The base update fucntion of a screen.
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
    Public Function IsCurrentScreen() As Boolean
        If Core.CurrentScreen.Identification = Me.Identification Then 'If the screen stored in the CurrentScreen field has the same ID as this screen, return true.
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
        If Core.CurrentScreen.CanBePaused = True Then
            Core.SetScreen(New PauseScreen(Core.CurrentScreen))
        End If
    End Sub

    ''' <summary>
    ''' Draws XBOX controls on the bottom right of the screen.
    ''' </summary>
    ''' <param name="Descriptions">The button types and descriptions.</param>
    ''' <remarks>Calculates the position and calls DrawGamePadControls(Descriptions,Position)</remarks>
    Friend Sub DrawGamePadControls(ByVal Descriptions As Dictionary(Of Microsoft.Xna.Framework.Input.Buttons, String))
        Dim x As Integer = Core.windowSize.Width 'Store the x position of the start of the controls render.

        'Loop through the buttons and add to the x location.
        For i = 0 To Descriptions.Count - 1
            Select Case Descriptions.Keys(i)
                Case Buttons.A, Buttons.B, Buttons.X, Buttons.Y, Buttons.Start, Buttons.LeftStick, Buttons.RightStick, Buttons.LeftTrigger, Buttons.RightTrigger
                    x -= 32 + 4
                Case Buttons.LeftShoulder, Buttons.RightShoulder
                    x -= 64 + 4
            End Select

            'Add to the x location for the length of the string and a separator.
            x -= CInt(FontManager.MainFont.MeasureString(Descriptions.Values(i)).X) + 16
        Next

        'Finally, render the buttons:
        DrawGamePadControls(Descriptions, New Vector2(x, Core.windowSize.Height - 40))
    End Sub

    ''' <summary>
    ''' Generic void to render XBOX Gamepad controls on the screen.
    ''' </summary>
    ''' <param name="Descriptions">The button types and descriptions.</param>
    ''' <param name="Position">The position to draw the buttons.</param>
    Friend Sub DrawGamePadControls(ByVal Descriptions As Dictionary(Of Microsoft.Xna.Framework.Input.Buttons, String), ByVal Position As Vector2)
        'Only if a Gamepad is connected and the screen is active, render the buttons:
        If GamePad.GetState(PlayerIndex.One).IsConnected = True And Core.GameOptions.GamePadEnabled = True And Me.IsCurrentScreen() = True Then
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
                End Select

                'Draw the buttons (first, the "shadow" with a black color, then the real button).
                Core.SpriteBatch.Draw(TextureManager.GetTexture(t), New Rectangle(x + 2, y + 2, width, height), Color.Black)
                Core.SpriteBatch.Draw(TextureManager.GetTexture(t), New Rectangle(x, y, width, height), Color.White)

                'Add the button width and a little offset to the drawing position:
                x += width + 4

                'Draw the button description (again, with a shadow):
                Core.SpriteBatch.DrawString(FontManager.MainFont, Descriptions.Values(i), New Vector2(x + 3, y + 7), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, Descriptions.Values(i), New Vector2(x, y + 4), Color.White)

                'Add the text width and the offset for the next button description to the drawing position:
                x += CInt(FontManager.MainFont.MeasureString(Descriptions.Values(i)).X) + 16
            Next
        End If
    End Sub

    ''' <summary>
    ''' Returns the screen status of the current screen. Override this function to return a screen state.
    ''' </summary>
    Public Overridable Function GetScreenStatus() As String
        '// Return the generic "not implemented" message:
        Return "Screen state not implemented for screen class: " & Me.Identification.ToString()
    End Function

    ''' <summary>
    ''' Returns the minimum size for the screen size to display a large interface before switching to the small size.
    ''' </summary>
    ''' <remarks>The default size is 800x620 pixels.</remarks>
    Public Overridable Function GetScreenScaleMinimum() As Size
        '// Default size: 800x620 pixels.
        Return New Size(800, 620)
    End Function

End Class
