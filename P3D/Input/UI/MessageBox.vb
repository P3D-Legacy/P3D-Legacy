Namespace UI

    ''' <summary>
    ''' Displays a message to the player.
    ''' </summary>
    Public Class MessageBox

        Inherits Screen

        Private _fadeIn As Single = 0F

        Private _closing As Boolean = False

        Private _text As String = ""
        Private Scale As Single = CInt(2.0F)
        Private _width As Integer = 320
        Private _height As Integer = 320
        Private _backColor As Color = Color.Black
        Private _textColor As Color = Color.White
        Private _shadowColor As Color = Color.Black

        ''' <summary>
        ''' Creates a new instance of the message box class.
        ''' </summary>
        ''' <param name="currentScreen"></param>
        Public Sub New(ByVal currentScreen As Screen)
            PreScreen = currentScreen
            Identification = Identifications.MessageBoxScreen

            CanBePaused = False
            CanChat = False
            CanDrawDebug = True
            CanGoFullscreen = True
            CanMuteAudio = True
            CanTakeScreenshot = True
            MouseVisible = True
        End Sub

        ''' <summary>
        ''' Displays the Message box.
        ''' </summary>
        ''' <param name="text">The text to display.</param>
        Public Sub Show(ByVal text As String, Optional backColor As Color = Nothing, Optional textColor As Color = Nothing, Optional shadowColor As Color = Nothing)
            _fadeIn = 0F
            _text = text
            _closing = False
            If Not backColor = Nothing Then
                _backColor = backColor
            End If
            If Not textColor = Nothing Then
                _textColor = textColor
            End If
            If Not shadowColor = Nothing Then
                _shadowColor = shadowColor
            End If

            Dim fontSize As Vector2 = FontManager.MainFont.MeasureString(_text) * Scale
            If fontSize.X > 196 Then
                _width = CInt(fontSize.X + 24)
            End If

            SetScreen(Me)
        End Sub

        Public Overrides Sub Draw()
            PreScreen.Draw()

            Dim fontSize As Vector2 = New Vector2(CInt(FontManager.MainFont.MeasureString(_text).X * Scale * Core.SpriteBatch.InterfaceScale), CInt(FontManager.MainFont.MeasureString(_text).Y * Scale * Core.SpriteBatch.InterfaceScale))

            Canvas.DrawRectangle(windowSize, New Color(0, 0, 0, CInt(140 * _fadeIn)))
            Dim boxRect = New Rectangle(CInt(ScreenSize.Width / 2 - _width / 2 - (_width / 10)),
                                               CInt(ScreenSize.Height / 2 - _height / 2 - (_height / 5) * (1 - _fadeIn)),
                                               CInt(_width + (_width / 5)),
                                               CInt(_height + fontSize.Y))
            Canvas.DrawRectangle(boxRect, New Color(_backColor.R, _backColor.G, _backColor.B, CInt(255 * _fadeIn)), True)

            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, _text, New Vector2(CInt(windowSize.Width / 2.0F - fontSize.X / 2.0F + 2 * Scale), CInt(boxRect.Y + (boxRect.Height / 2) - fontSize.Y / 2.0F + 2 * Scale)), New Color(_shadowColor.R, _shadowColor.G, _shadowColor.B, CInt(255 * _fadeIn)), 0F, Vector2.Zero, Scale, SpriteEffects.None, 0F)
            Core.SpriteBatch.DrawInterfaceString(FontManager.MainFont, _text, New Vector2(CInt(windowSize.Width / 2.0F - fontSize.X / 2.0F), CInt(boxRect.Y + (boxRect.Height / 2) - fontSize.Y / 2.0F)), New Color(_textColor.R, _textColor.G, _textColor.B, CInt(255 * _fadeIn)), 0F, Vector2.Zero, Scale, SpriteEffects.None, 0F)
        End Sub

        Public Overrides Sub Update()
            If _closing Then
                If _fadeIn > 0.0F Then
                    _fadeIn = MathHelper.Lerp(0.0F, _fadeIn, 0.7F)
                    If _fadeIn - 0.01F <= 0.0F Then
                        _fadeIn = 0.0F
                        SetScreen(PreScreen)
                    End If
                End If
            Else
                If _fadeIn < 1.0F Then
                    _fadeIn = MathHelper.Lerp(1.0F, _fadeIn, 0.95F)
                    If _fadeIn + 0.01F >= 1.0F Then
                        _fadeIn = 1.0F
                    End If
                End If
                If _fadeIn > 0.75F Then
                    If Controls.Dismiss(True, True, True) Or Controls.Accept(True, True, True) Then
                        _closing = True
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace