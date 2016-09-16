Imports System.Threading

Friend Class SplashScreen

    Inherits Screen

    Private Const LICENSE_TEXT As String = """MonoGame"", the MonoGame Logo and source code are copyrights of MonoGame Team (monogame.net)." + vbNewLine +
                                           "Pokémon 3D is not affiliated with Nintendo, Creatures Inc. or GAME FREAK Inc."

    Private ReadOnly _monoGameLogo As Texture2D
    Private ReadOnly _licenseFont As SpriteFont
    Private ReadOnly _licenseTextSize As Vector2

    Private _delay As Single = 7.0F
    Private _loadThread As Thread
    Private _startedLoad As Boolean
    Private _game As GameController

    Public Sub New(ByVal GameReference As GameController)
        _game = GameReference

        CanBePaused = False
        CanMuteMusic = False
        CanChat = False
        CanTakeScreenshot = False
        CanDrawDebug = False
        MouseVisible = True
        CanGoFullscreen = False

        _monoGameLogo = Core.Content.Load(Of Texture2D)("GUI\Logos\MonoGame")
        _licenseFont = Core.Content.Load(Of SpriteFont)("Fonts\BMP\mainFont")
        _licenseTextSize = _licenseFont.MeasureString(LICENSE_TEXT)

        Me.Identification = Identifications.SplashScreen
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, Color.Black)

        Core.SpriteBatch.Draw(_monoGameLogo, New Vector2(CSng(Core.windowSize.Width / 2 - _monoGameLogo.Width / 2),
                                                         CSng(Core.windowSize.Height / 2 - _monoGameLogo.Height / 2 - 50)), Color.White)

        Core.SpriteBatch.DrawString(_licenseFont, LICENSE_TEXT, New Vector2(CSng(Core.windowSize.Width / 2 - _licenseTextSize.X / 2),
                                                                            CSng(Core.windowSize.Height - _licenseTextSize.Y - 50)), Color.White)
    End Sub

    Public Overrides Sub Update()
        If _startedLoad = False Then
            _startedLoad = True

            _loadThread = New Thread(AddressOf LoadContent)
            _loadThread.Start()
        End If

        If _loadThread.IsAlive = False Then
            If _delay <= 0.0F Or GameController.IS_DEBUG_ACTIVE = True Then
                Core.GraphicsManager.ApplyChanges()

                Logger.Debug("---Loading content ready---")

                If MapPreviewScreen.MapViewMode = True Then
                    Core.SetScreen(New MapPreviewScreen())
                Else
                    Core.SetScreen(New MainMenuScreen())
                End If
                'Core.SetScreen(New TransitionScreen(Me, New IntroScreen(), Color.Black, False))
            End If
        End If

        _delay -= 0.1F
    End Sub

    Private Sub LoadContent()
        Logger.Debug("---Start loading content---")

        Core.LoadContent()
    End Sub

End Class