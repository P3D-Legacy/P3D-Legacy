''' <summary>
''' The screen to display the default Overworld gameplay.
''' </summary>
Public Class OverworldScreen

    Inherits Screen

#Region "Fields"

    Private Shared _fadeValue As Integer = 0 'Fade progress value for the black screen fade.
    Private Shared _drawRodID As Integer = -1 'The rod ID to display on the screen during the fishing animation.

    Private _actionScript As ActionScript 'Private ActionScript instance.
    Private _particlesTexture As Texture2D 'A texture field to contain the particles texture, currently only used for the crosshair.
    Private _trainerEncountered As Boolean = False
    Private _titles As New List(Of Title)

    ''' <summary>
    ''' The delay until the XBOX buttons get shown since the player last pressed a button.
    ''' </summary>
    Private ShowControlsDelay As Single = 4.0F

#End Region

#Region "Properties"

    ''' <summary>
    ''' Array of Title objects to be rendered on the screen.
    ''' </summary>
    Public ReadOnly Property Titles() As List(Of Title)
        Get
            Return Me._titles
        End Get
    End Property

    ''' <summary>
    ''' The ActionScript instance that controls the scripts.
    ''' </summary>
    Public ReadOnly Property ActionScript() As ActionScript
        Get
            Return Me._actionScript
        End Get
    End Property

    ''' <summary>
    ''' Checks if the player encountered a trainer.
    ''' </summary>
    Public Property TrainerEncountered() As Boolean
        Get
            Return Me._trainerEncountered
        End Get
        Set(value As Boolean)
            Me._trainerEncountered = value
        End Set
    End Property

    ''' <summary>
    ''' Fade progress value for the black screen fade.
    ''' </summary>
    Public Shared Property FadeValue() As Integer
        Get
            Return _fadeValue
        End Get
        Set(value As Integer)
            _fadeValue = value
        End Set
    End Property

    ''' <summary>
    ''' The Fishing Rod that should be rendered on the screen.
    ''' </summary>
    ''' <remarks>-1 = No Rod, 0 = Old Rod, 1 = Good Rod, 2 = Super Rod</remarks>
    Public Shared Property DrawRodID() As Integer
        Get
            Return _drawRodID
        End Get
        Set(value As Integer)
            _drawRodID = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Returns information about the Overworld Screen.
    ''' </summary>
    ''' <remarks>Implements the GetScreenStatus method.</remarks>
    Public Overrides Function GetScreenStatus() As String
        Dim s As String = "IsSurfing=" & Level.Surfing.ToString() & vbNewLine &
            "IsRiding=" & Level.Riding.ToString() & vbNewLine &
            "LevelFile=" & Level.LevelFile & vbNewLine &
            "UsedStrength=" & Level.UsedStrength.ToString() & vbNewLine &
            "EntityCount=" & Level.Entities.Count

        Return s
    End Function

    ''' <summary>
    ''' Creates a new instance of the OverworldScreen.
    ''' </summary>
    Public Sub New()
        'Set default information:
        Me.Identification = Identifications.OverworldScreen
        Me.CanChat = True
        Me.MouseVisible = False

        'Set up 3D environment variables (Effect, Camera, SkyDome and Level):
        Effect = New BasicEffect(Core.GraphicsDevice)
        Effect.FogEnabled = True

        Camera = New OverworldCamera()
        SkyDome = New SkyDome()
        Level = New Level()
        Level.Load(Core.Player.startMap)

        'Play music depending on the player state in the level (surfing and riding):
        If Level.Surfing = True Then
            MusicManager.PlayMusic("surf", True) 'Play "surf" when player is surfing.
        Else
            If Level.Riding = True Then
                MusicManager.PlayMusic("ride", True) 'Play "ride" when player is riding.
            Else
                MusicManager.PlayMusic(Level.MusicLoop, True) 'Play default MusicLoop.
            End If
        End If

        'Setup the RouteSign for the initial level.
        Level.RouteSign.Setup(Level.MapName)

        'Create a new instance of the ActionScript.
        Me._actionScript = New ActionScript(Level)

        'Initialize the World information with the loaded level.
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

        'Load the particle texture.
        Me._particlesTexture = TextureManager.GetTexture("GUI\Overworld\Particles")
    End Sub

    ''' <summary>
    ''' Updates the OverworldScreen.
    ''' </summary>
    Public Overrides Sub Update()
        'If the MapScript has a value loaded from the MapScript map tag and there is no script running, start that script:
        If LevelLoader.MapScript <> "" And ActionScript.IsReady = True Then
            ActionScript.reDelay = 0.0F
            ActionScript.StartScript(LevelLoader.MapScript, 0)

            LevelLoader.MapScript = "" 'Reset the MapScript.
        End If

        Lighting.UpdateLighting(Screen.Effect) 'Update the lighting on the basic effect.

        'Update the Dialogues:
        ChooseBox.Update()
        If ChooseBox.Showing = False Then
            TextBox.Update()
        End If
        If PokemonImageView.Showing = True Then
            PokemonImageView.Update()
        End If

        'Middle click/Thumbstick press: Show first Pokémon in party.
        If ActionScript.IsReady = True Then
            If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.MiddleButton) = True Or ControllerHandler.ButtonPressed(Buttons.LeftStick) = True Then
                If Core.Player.Pokemons.Count > 0 Then
                    Core.SetScreen(New PokemonStatusScreen(Core.CurrentScreen, 0, {}, Core.Player.Pokemons(0), True))
                End If
            End If
        End If

        'If no dialogue is showing, do level update tasks:
        If TextBox.Showing = False And ChooseBox.Showing = False And PokemonImageView.Showing = False Then
            'If no script is running and no MapScript is in the queue, update camera and the level.
            If ActionScript.IsReady = True And LevelLoader.MapScript = "" Then
                If Me.HandleServerRequests() = True Then
                    Camera.Update()
                    Level.Update()
                End If
            Else
                'Because other players (and their Pokémon) are moving around while you are standing still and no level update occurs,
                'only sort entity lists so they can display transparency normally.
                If JoinServerScreen.Online = True Then
                    Level.SortEntities()
                End If
            End If

            'Open the MenuScreen:
            If KeyBoardHandler.KeyPressed(KeyBindings.OpenInventoryKey) = True Or ControllerHandler.ButtonPressed(Buttons.X) = True Then
                If Screen.Camera.IsMoving() = False And ActionScript.IsReady = True Then
                    Level.RouteSign.Hide()

                    SoundManager.PlaySound("menu_open")
                    Core.SetScreen(New MenuScreen(Me))
                End If
            End If

            'Open the PokégearScreen:
            If KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Or ControllerHandler.ButtonPressed(Buttons.Y) = True Then
                If Core.Player.HasPokegear = True Or GameController.IS_DEBUG_ACTIVE = True Then
                    If Screen.Camera.IsMoving() = False And ActionScript.IsReady = True Then
                        Core.SetScreen(New GameJolt.PokegearScreen(Me, GameJolt.PokegearScreen.EntryModes.MainMenu, {}))
                    End If
                End If
            End If

            ActionScript.Update() 'Update the action script.
        Else 'Dialogues are showing:
            'Update some parts of the camera:
            If Camera.Name = "Overworld" Then
                If CType(Camera, OverworldCamera).ThirdPerson = False Then
                    If CType(Camera, OverworldCamera).IsPointingToNormalDirection() = False Then
                        If Camera.Turning = False Then
                            Camera.Turning = True

                            CType(Camera, OverworldCamera).SetAimDirection(Camera.GetFacingDirection())
                        End If

                        CType(Camera, OverworldCamera).AimCamera()
                        Level.UpdateEntities()
                    End If
                End If

                CType(Camera, OverworldCamera).PitchForward()
                CType(Camera, OverworldCamera).UpdateViewMatrix()
                CType(Camera, OverworldCamera).UpdateFrustum()
            End If

            'If it's online play (on Servers), update network entities, because Level.Update doesn't get called.
            If JoinServerScreen.Online = True Then
                For Each p As NetworkPlayer In Level.NetworkPlayers
                    p.UpdateEntity()
                    p.Update()
                Next
                For Each p As NetworkPokemon In Level.NetworkPokemon
                    p.UpdateEntity()
                    p.Update()
                Next
            End If
        End If

        SkyDome.Update()
        Level.RouteSign.Update()

        'Update the World with new environment variables.
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

        Me.UpdateShowControlDelay()

        'If for some mysterical reason, a player with a GameJolt account is not logged in during a play session, prompt the LogInScreen.
        GameJolt.LogInScreen.KickFromOnlineScreen(Me)

        Me.UpdateTitles()
    End Sub

    ''' <summary>
    ''' Updates the delay of the XBOX button render.
    ''' </summary>
    Private Sub UpdateShowControlDelay()
        If Me.ShowControlsDelay > 0.0F Then
            'If any Gamepad is connected, countdown the delay.
            If Core.GameOptions.GamePadEnabled = True AndAlso GamePad.GetState(PlayerIndex.One).IsConnected = True Then
                ShowControlsDelay -= 0.1F
                If ShowControlsDelay <= 0.0F Then
                    ShowControlsDelay = 0.0F
                End If
            End If
        End If
        If Screen.Camera.IsMoving() = True Or Camera.Turning = True Or ActionScript.IsReady = False Or TextBox.Showing = True Or ChooseBox.Showing = True Then
            'If any input is received, reset the delay:
            ShowControlsDelay = 8.0F
        End If
    End Sub

    ''' <summary>
    ''' Handles PVP and Trade requests incoming from other players via Servers and prompts the accept screens.
    ''' </summary>
    ''' <returns>True, if no requests are in the queue, False otherwise.</returns>
    Private Function HandleServerRequests() As Boolean
        If GameJolt.PokegearScreen.BattleRequestData <> -1 Then 'A Servers ID from another player is set here.
            If Core.ServersManager.PlayerCollection.HasPlayer(GameJolt.PokegearScreen.BattleRequestData) = True Then 'If the player still exists on the server.
                Core.SetScreen(New GameJolt.PokegearScreen(Core.CurrentScreen, GameJolt.PokegearScreen.EntryModes.BattleRequest, {GameJolt.PokegearScreen.BattleRequestData, Core.ServersManager.PlayerCollection.GetPlayer(GameJolt.PokegearScreen.BattleRequestData).GameJoltId}))
                Return False
            Else 'Otherwise, reset the data.
                GameJolt.PokegearScreen.BattleRequestData = -1
            End If
        End If
        If GameJolt.PokegearScreen.TradeRequestData <> -1 Then 'A Servers ID from another player is set here.
            If Core.ServersManager.PlayerCollection.HasPlayer(GameJolt.PokegearScreen.TradeRequestData) = True Then 'If the player still exists on the server.
                Core.SetScreen(New GameJolt.PokegearScreen(Core.CurrentScreen, GameJolt.PokegearScreen.EntryModes.TradeRequest, {GameJolt.PokegearScreen.TradeRequestData, Core.ServersManager.PlayerCollection.GetPlayer(GameJolt.PokegearScreen.TradeRequestData).GameJoltId}))
                Return False
            Else 'Otherwise, reset the data.
                GameJolt.PokegearScreen.TradeRequestData = -1
            End If
        End If
        Return True
    End Function

    Public Overrides Sub Draw()
        SkyDome.Draw(Camera.FOV)

        Level.Draw()

        World.DrawWeather(Screen.Level.World.CurrentMapWeather)

        DrawGUI()
        PokemonImageView.Draw()
        TextBox.Draw()

        'Only draw the ChooseBox when it's the current screen, cause the same ChooseBox might get used on other screens.
        If Me.IsCurrentScreen() = True Then
            ChooseBox.Draw()
        End If

        Level.RouteSign.Draw()

        'If the XBOX render control delay is 0, render the controls.
        If ShowControlsDelay = 0.0F Then
            Dim d As New Dictionary(Of Buttons, String)
            d.Add(Buttons.A, "Interact")
            d.Add(Buttons.X, "Menu")

            If Core.Player.hasPokegear = True Then
                d.Add(Buttons.Y, "Pokégear")
            End If

            d.Add(Buttons.Start, "Game Menu")

            DrawGamePadControls(d)
        End If
    End Sub

    ''' <summary>
    ''' Renders the GUI on the screen.
    ''' </summary>
    ''' <remarks>GUI includes: a fishing rod, the crosshair, titles and the black screen fade.</remarks>
    Private Sub DrawGUI()
        'Determine if third person mode is used:
        Dim isThirdPerson As Boolean = True
        If Camera.Name = "Overworld" Then
            Dim c As OverworldCamera = CType(Camera, OverworldCamera)
            isThirdPerson = c.ThirdPerson
        End If

        'Render the Rod (based on the DrawRodID property).
        If DrawRodID > -1 And isThirdPerson = False Then
            Dim t As Texture2D = TextureManager.GetTexture("GUI\Overworld\Rods", New Rectangle(DrawRodID * 8, 0, 8, 64), "") 'Load the texture.
            Dim P As New Vector2(CSng(Core.windowSize.Width / 2 - 32), Core.windowSize.Height - 490)

            Core.SpriteBatch.Draw(t, New Rectangle(CInt(P.X), CInt(P.Y), 64, 512), Color.White)
        End If

        'Render Crosshair:
        If Core.GameOptions.ShowGUI = True And isThirdPerson = False Then
            Dim P As Vector2 = Core.GetMiddlePosition(New Size(16, 16))
            Core.SpriteBatch.Draw(_particlesTexture, New Rectangle(CInt(P.X), CInt(P.Y), 16, 16), New Rectangle(0, 0, 9, 9), Color.White)
        End If

        'Render all active titles:
        For Each Title As Title In Me._titles
            Title.Draw()
        Next

        'If the black fade is visible, render it:
        If FadeValue > 0 Then
            Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, FadeValue))
        End If
    End Sub

    Public Overrides Sub ChangeTo()
        'When changed to the Overworld screen from somewhere else, set the Rod ID to -1.
        DrawRodID = -1

        'Center the mouse:
        Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)
        c.oldX = MouseHandler.MousePosition.X
        c.oldY = MouseHandler.MousePosition.Y

        Player.Temp.IsInBattle = False

        'Set to correct music:
        If TrainerEncountered = False Then
            Dim theme As String = Level.MusicLoop
            If Screen.Level.Surfing = True Then
                theme = "surf"
            End If
            If Screen.Level.Riding = True Then
                theme = "ride"
            End If

            'If the radio is activated and the station can be played on the current map, play the music.
            If Level.IsRadioOn = True AndAlso GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = True Then
                theme = Level.SelectedRadioStation.Music
            Else
                Level.IsRadioOn = False
            End If

            MusicManager.PlayMusic(theme, True)
        End If
    End Sub

    ''' <summary>
    ''' Update all title objects in the _titles array.
    ''' </summary>
    Private Sub UpdateTitles()
        For Each t As Title In Me._titles
            t.Update()
            If t.IsReady = True Then 'If the title animation is ready, remove it from the array.
                Me._titles.Remove(t)
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' A class to display text on the OverworldScreen.
    ''' </summary>
    Public Class Title

        Private _text As String = "Sample Text"
        Private _textColor As Color = Color.White
        Private _scale As Single = 10.0F
        Private _position As Vector2 = Vector2.Zero
        Private _isCentered As Boolean = True
        Private _delay As Single = 20.0F

        ''' <summary>
        ''' The text to be displayed on the screen.
        ''' </summary>
        ''' <remarks>The default is "Sample Text".</remarks>
        Public Property Text() As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        ''' <summary>
        ''' The color of the text on the screen.
        ''' </summary>
        ''' <remarks>The default is White (255,255,255). No transparency is suppoorted.</remarks>
        Public Property TextColor() As Color
            Get
                Return Me._textColor
            End Get
            Set(value As Color)
                Me._textColor = value
            End Set
        End Property

        ''' <summary>
        ''' The scale of the text.
        ''' </summary>
        ''' <remarks>The default is x10.</remarks>
        Public Property Scale() As Single
            Get
                Return Me._scale
            End Get
            Set(value As Single)
                Me._scale = value
            End Set
        End Property

        ''' <summary>
        ''' The position of the text on the screen.
        ''' </summary>
        ''' <remarks>The default is 0,0 - This position gets ignored when IsCentered is set to True.</remarks>
        Public Property Position() As Vector2
            Get
                Return Me._position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        ''' <summary>
        ''' This determines if the text is always centered on the screen.
        ''' </summary>
        ''' <remarks>The default is True. If this is set to True, the Position Property is getting ignored.</remarks>
        Public Property IsCentered() As Boolean
            Get
                Return Me._isCentered
            End Get
            Set(value As Boolean)
                Me._isCentered = value
            End Set
        End Property

        ''' <summary>
        ''' The delay in ticksx10 until the text fades from the screen.
        ''' </summary>
        ''' <remarks>The default is 20.</remarks>
        Public Property Delay() As Single
            Get
                Return Me._delay
            End Get
            Set(value As Single)
                Me._delay = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a new instance of the Title class and assigns the default values to all properties.
        ''' </summary>
        Public Sub New()
            '//Empty constructor.
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Title class.
        ''' </summary>
        ''' <param name="Text">The text to be displayed on the screen.</param>
        ''' <param name="Delay">The delay until the text fades.</param>
        ''' <param name="TextColor">The color of the text.</param>
        ''' <param name="Scale">The scale of the text.</param>
        ''' <param name="Position">The position of the text on the screen.</param>
        ''' <param name="IsCentered">If the text should always get centered on the screen.</param>
        Public Sub New(ByVal Text As String, ByVal Delay As Single, ByVal TextColor As Color, ByVal Scale As Single, ByVal Position As Vector2, ByVal IsCentered As Boolean)
            Me._text = Text
            Me._delay = Delay
            Me._textColor = TextColor
            Me._scale = Scale
            Me._position = Position
            Me._isCentered = IsCentered
        End Sub

        ''' <summary>
        ''' Renders the text on the screen.
        ''' </summary>
        Public Sub Draw()
            Dim p As Vector2 = Vector2.Zero

            'If the text is centered, set the draw position to the center of the screen, then add the position.
            If Me._isCentered = True Then
                Dim v As Vector2 = FontManager.TextFont.MeasureString(Me._text) * Me._scale
                p = New Vector2(CSng(Core.windowSize.Width / 2 - v.X / 2), CSng(Core.windowSize.Height / 2 - v.Y / 2))
            End If
            p += Me._position

            'Determine the alpha value.
            Dim A As Integer = 255
            If Me._delay <= 3.0F Then
                A = CInt(255 * (1 / 3 * Me._delay))
            End If

            Core.SpriteBatch.DrawString(FontManager.TextFont, Me._text, p, New Color(Me._textColor.R, Me._textColor.G, Me._textColor.B, A), 0.0F, Vector2.Zero, Me._scale, SpriteEffects.None, 0.0F)
        End Sub

        ''' <summary>
        ''' Updates the Title object.
        ''' </summary>
        Public Sub Update()
            If Me._delay > 0.0F Then
                Me._delay -= 0.1F
                If Me._delay <= 0.0F Then
                    Me._delay = 0.0F
                End If
            End If
        End Sub

        ''' <summary>
        ''' Returns, if the title object faded from the screen.
        ''' </summary>
        Public ReadOnly Property IsReady() As Boolean
            Get
                Return Me._delay = 0.0F
            End Get
        End Property

    End Class

End Class