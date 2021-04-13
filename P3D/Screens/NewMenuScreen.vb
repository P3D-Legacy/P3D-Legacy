Public Class NewMenuScreen

    Inherits Screen

    Private _menuOptions As New List(Of String)
    Private _menuIndex As Integer = 0

    Private _texture As Texture2D

    'Intro animation:
    Private _gradientFade As Integer = 0
    Private _buttonFadeIndex As Integer = 0
    Private _currentButtonFade As Integer = 0
    Private _buttonIntroFinished As Boolean = False

    'cursor animation:
    Private _cursorPosition As Vector2 = New Vector2(0, 0)
    Private _cursorDestPosition As Vector2 = New Vector2(0, 0)

    Private _preScreenTexture As RenderTarget2D
    Private _preScreenTarget As RenderTarget2D


    Public Sub New(ByVal currentScreen As Screen)
        Identification = Identifications.MenuScreen
        PreScreen = currentScreen
        IsDrawingGradients = True

        MouseVisible = True
        _preScreenTarget = New RenderTarget2D(GraphicsDevice, windowSize.Width, windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        _texture = TextureManager.GetTexture("GUI\Menus\General")

        ConstructMenu()

        _menuIndex = Player.Temp.MenuIndex
        SetCursorPosition(_menuIndex)
        _cursorPosition = _cursorDestPosition

        _blur = New Resources.Blur.BlurHandler(windowSize.Width, windowSize.Height)
    End Sub

    Private Sub ConstructMenu()
        If Core.Player.HasPokedex = True Then
            _menuOptions.Add("Pokédex")
        End If

        If Screen.Level.IsBugCatchingContest = True Then
            _menuOptions.AddRange({Screen.Level.BugCatchingContestData.GetSplit(2) & " x" & Core.Player.Inventory.GetItemAmount(177),
                                    Localization.Translate("menu_bag_title"),
                                    "|||" & Core.Player.Name, 'Trainer card
                                    "End Contest"})
        Else
            If Core.Player.Pokemons.Count > 0 Then
                _menuOptions.Add("Pokémon")
            End If
            _menuOptions.AddRange({Localization.Translate("menu_bag_title"),
                                     "|||" & Core.Player.Name,
                                    Localization.Translate("global_save")})
        End If

        _menuOptions.Add(Localization.Translate("option_title"))
    End Sub

    Private _blur As Resources.Blur.BlurHandler

    Private Sub DrawPrescreen()
        If _preScreenTexture Is Nothing OrElse _preScreenTexture.IsContentLost Then
            SpriteBatch.EndBatch()

            Dim target As RenderTarget2D = _preScreenTarget
            GraphicsDevice.SetRenderTarget(target)
            GraphicsDevice.Clear(BackgroundColor)

            SpriteBatch.BeginBatch()

            PreScreen.Draw()

            SpriteBatch.EndBatch()

            GraphicsDevice.SetRenderTarget(Nothing)

            SpriteBatch.BeginBatch()

            _preScreenTexture = target
        End If

        SpriteBatch.Draw(_blur.Perform(_preScreenTexture), windowSize, Color.White)
    End Sub

    Public Overrides Sub Draw()
        DrawPrescreen()

        DrawGradients(_gradientFade)

        If Me.IsCurrentScreen() = True Then
            'Draw buttons:
            If _gradientFade = 255 Then
                If Core.Player.IsGameJoltSave = True Then
                    GameJolt.Emblem.Draw(GameJolt.API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem, New Vector2(CSng(Core.windowSize.Width / 2 - 256), 30), 4, Core.GameJoltSave.DownloadedSprite)
                End If

                For i = 0 To _menuOptions.Count - 1
                    Dim text As String = _menuOptions(i).Replace("|||", "")

                    If _buttonIntroFinished = True Or _buttonFadeIndex > i Then
                        Dim pos = GetButtonPosition(i)

                        Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X), CInt(pos.Y), 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                        Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X) + 64, CInt(pos.Y), 64 * 4, 64), New Rectangle(32, 16, 16, 16), Color.White)
                        Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X) + 64 * 5, CInt(pos.Y), 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(CInt(pos.X) + 20, CInt(pos.Y) + 16), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
                    Else
                        Dim pos = GetButtonPosition(i)

                        Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X), CInt(pos.Y), 64, 64), New Rectangle(16, 16, 16, 16), New Color(255, 255, 255, _currentButtonFade))
                        Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X) + 64, CInt(pos.Y), 64 * 4, 64), New Rectangle(32, 16, 16, 16), New Color(255, 255, 255, _currentButtonFade))
                        Core.SpriteBatch.Draw(_texture, New Rectangle(CInt(pos.X) + 64 * 5, CInt(pos.Y), 64, 64), New Rectangle(16, 16, 16, 16), New Color(255, 255, 255, _currentButtonFade), 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(CInt(pos.X) + 20, CInt(pos.Y) + 16), New Color(0, 0, 0, _currentButtonFade), 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)

                        Exit For
                    End If
                Next
            End If

            DrawCursor()
        Else
            _buttonFadeIndex = 0
            _currentButtonFade = 0
            _buttonIntroFinished = False
        End If
    End Sub

    Private Sub DrawCursor()
        If _buttonIntroFinished = True Or _buttonFadeIndex > _menuIndex Then
            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(_cursorPosition.X), CInt(_cursorPosition.Y), 64, 64), Color.White)
        End If
    End Sub

    Private Sub SetCursorPosition(ByVal _buttonIndex As Integer)
        Dim pos = GetButtonPosition(_buttonIndex)
        Dim cPosition As Vector2 = New Vector2(pos.X + 180, pos.Y - 42)
        _cursorDestPosition = cPosition
    End Sub

    Private Function GetButtonPosition(ByVal index As Integer) As Vector2
        'button size: 384x64
        'space between buttons horizontally: 150
        'space between buttons vertically: 80

        Dim X As Single = 0F
        Dim Y As Single = 0F

        If (index Mod 2) = 0 Then
            X = Core.ScreenSize.Width / 2.0F - 384 - 75
        Else
            X = Core.ScreenSize.Width / 2.0F + 75
        End If

        Select Case Math.Floor(index / 2)
            Case 0
                Y = (Core.ScreenSize.Height / 2.0F) - 64 - 80 - 32
            Case 1
                Y = (Core.ScreenSize.Height / 2.0F) - 32
            Case 2
                Y = (Core.ScreenSize.Height / 2.0F) + 32 + 80
        End Select

        Return New Vector2(X, Y)
    End Function

    Public Overrides Sub Update()
        If _gradientFade < 255 Then
            _gradientFade += 25
            If _gradientFade >= 255 Then
                _gradientFade = 255
            End If
        Else
            If _buttonIntroFinished = False Then
                _currentButtonFade += 45
                If _currentButtonFade >= 255 Then
                    _currentButtonFade = 0
                    _buttonFadeIndex += 1
                    If _buttonFadeIndex > _menuOptions.Count - 1 Then
                        _buttonIntroFinished = True
                    End If
                End If
            End If
        End If

        If _buttonIntroFinished = True Then
            Player.Temp.MenuIndex = _menuIndex

            Dim preMenuIndex As Integer = _menuIndex

            If _cursorDestPosition.X <> _cursorPosition.X Or _cursorDestPosition.Y <> _cursorPosition.Y Then
                _cursorPosition.X = MathHelper.Lerp(_cursorDestPosition.X, _cursorPosition.X, 0.75F)
                _cursorPosition.Y = MathHelper.Lerp(_cursorDestPosition.Y, _cursorPosition.Y, 0.75F)

                If Math.Abs(_cursorDestPosition.X - _cursorPosition.X) < 0.1F Then
                    _cursorPosition.X = _cursorDestPosition.X
                End If
                If Math.Abs(_cursorDestPosition.Y - _cursorPosition.Y) < 0.1F Then
                    _cursorPosition.Y = _cursorDestPosition.Y
                End If
            End If
            If Math.Abs(_cursorDestPosition.Y - _cursorPosition.Y) < 5.0F Then
                If Controls.Accept(True, False, False) = True Then
                    For i = 0 To _menuOptions.Count - 1
                        Dim pos = GetButtonPosition(i)
                        If New Rectangle(CInt(pos.X), CInt(pos.Y), 64 * 6, 64).Contains(MouseHandler.MousePosition) Then
                            If _menuIndex = i Then
                                _cursorPosition.X = _cursorDestPosition.X
                                SoundManager.PlaySound("select")
                                PressButton()
                            Else
                                _menuIndex = i
                                _cursorDestPosition = New Vector2(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y - 42)
                                preMenuIndex = _menuIndex 'Prevent the update of the mouse position below.
                                Exit For
                            End If
                        End If
                    Next
                End If
                If Controls.Accept(False, True, True) = True Then
                    _cursorPosition.X = _cursorDestPosition.X
                    SoundManager.PlaySound("select")
                    PressButton()
                End If
            End If

            If Controls.Up(True, True, False, True, True, True) = True Then
                If _menuIndex > 1 Then
                    _menuIndex -= 2
                End If
            End If
            If Controls.Down(True, True, False, True, True, True) = True Then
                If _menuIndex < _menuOptions.Count - 2 Then
                    _menuIndex += 2
                End If
            End If
            If Controls.Right(True, True, True, True, True, True) = True Then
                If _menuIndex < _menuOptions.Count - 1 Then
                    _menuIndex += 1
                End If
            End If
            If Controls.Left(True, True, True, True, True, True) = True Then
                If _menuIndex > 0 Then
                    _menuIndex -= 1
                End If
            End If

            If _menuIndex <> preMenuIndex Then
                SetCursorPosition(_menuIndex)
            End If
        End If

        If Controls.Dismiss() = True Then
            Core.SetScreen(PreScreen)
        End If
    End Sub

    Private Sub PressButton()
        Select Case _menuOptions(_menuIndex)
            Case "Pokédex"
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New PokedexSelectScreen(Me), Color.White, False))
            Case "Pokémon"
                SetScreen(New PartyScreen(Me))
            Case Localization.Translate("menu_bag_title")
                Core.SetScreen(New NewInventoryScreen(Me))
            Case "|||" & Core.Player.Name
                Core.SetScreen(New NewTrainerScreen(Me))
            Case Localization.Translate("global_save")
                Core.SetScreen(New SaveScreen(Me))
            Case Localization.Translate("option_title")
                Core.SetScreen(New NewOptionScreen(Me))
            Case Localization.Translate("global_close")
                Core.SetScreen(PreScreen)
            Case Screen.Level.BugCatchingContestData.GetSplit(2) & " x" & Core.Player.Inventory.GetItemAmount(177)
                Me.ShowBalls()
            Case "End Contest"
                Me.EndContest()
        End Select
    End Sub

    Private Sub ShowBalls()
        ''' Requires new scripting system
        'Dim s As Screen = Me.PreScreen
        'Construct.Controller.GetInstance().RunFromFile(Screen.Level.BugCatchingContestData.GetSplit(1))
        'Core.SetScreen(s)

        Dim s As Screen = Me.PreScreen
        CType(s, OverworldScreen).ActionScript.StartScript(Screen.Level.BugCatchingContestData.GetSplit(1), 0)
        Core.SetScreen(s)
    End Sub

    Private Sub EndContest()
        ''' Requires new scripting system
        'Dim s As Screen = Me.PreScreen
        'Construct.Controller.GetInstance().RunFromFile(Screen.Level.BugCatchingContestData.GetSplit(0))
        'Core.SetScreen(s)

        Dim s As Screen = Me.PreScreen
        CType(s, OverworldScreen).ActionScript.StartScript(Screen.Level.BugCatchingContestData.GetSplit(0), 0)
        Core.SetScreen(s)
    End Sub

    Public Overrides Sub SizeChanged()
        SetCursorPosition(_menuIndex)
        _cursorPosition = _cursorDestPosition
    End Sub

End Class