Namespace UI

    ''' <summary>
    ''' Displays a message to the player.
    ''' </summary>
    Public Class MessageBox

        Inherits Screen

        Private _fadeIn As Single = 0F

        Private _closing As Boolean = False

        Private _text As String = ""

        Private _width As Integer = 500
        Private _height As Integer = 200

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
            CanMuteMusic = True
            CanTakeScreenshot = True
            MouseVisible = True
        End Sub

        ''' <summary>
        ''' Displays the Message box.
        ''' </summary>
        ''' <param name="text">The text to display.</param>
        Public Sub Show(ByVal text As String)
            _fadeIn = 0F
            _text = text
            _closing = False

            Dim fontSize As Vector2 = FontManager.GameJoltFont.MeasureString(_text)
            If fontSize.X > 480 Then
                _width = CInt(fontSize.X + 20)
            End If

            SetScreen(Me)
        End Sub

        Public Overrides Sub Draw()
            PreScreen.Draw()

            Canvas.DrawRectangle(windowSize, New Color(0, 0, 0, CInt(140 * _fadeIn)))

            Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2 - _width / 2 - (_width / 10) * (1 - _fadeIn)),
                                               CInt(windowSize.Height / 2 - _height / 2 - (_height / 10) * (1 - _fadeIn)),
                                               CInt(_width + (1 - _fadeIn) * (_width / 5)),
                                               CInt(_height + (1 - _fadeIn) * (_height / 5))), New Color(0, 0, 0, CInt(255 * _fadeIn)))


            Dim fontSizeMulti As Single = CSng(1 + (1 / 10) * (1 - _fadeIn)) * 0.75F
            Dim fontSize As Vector2 = FontManager.GameJoltFont.MeasureString(_text)
            GetFontRenderer().DrawString(FontManager.GameJoltFont, _text, New Vector2(windowSize.Width / 2.0F - (fontSize.X * fontSizeMulti) / 2.0F,
                                                                                      windowSize.Height / 2.0F - (fontSize.Y * fontSizeMulti) / 2.0F), New Color(255, 255, 255, CInt(255 * _fadeIn)), 0F, Vector2.Zero, fontSizeMulti, SpriteEffects.None, 0F)
        End Sub

        Public Overrides Sub Update()
            If _closing Then
                If _fadeIn > 0.0F Then
                    _fadeIn = MathHelper.Lerp(0.0F, _fadeIn, 0.4F)
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
                Else
                    If Controls.Dismiss(True, True, True) Or Controls.Accept(True, True, True) Then
                        _closing = True
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace