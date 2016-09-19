''' <summary>
''' A generic class for a screen with a window GUI.
''' </summary>
Public MustInherit Class WindowScreen

    Inherits Screen

    'Shared controller to check if any other window screen is drawing a gradient already. If so, don't draw another gradient on top.
    Public Shared IsDrawingGradients As Boolean = False

    Const STARTWINDOWSINK As Integer = -35

    Private _gradientFade As Integer = 0
    Private _windowSink As Integer = STARTWINDOWSINK
    Private _windowLocation As Vector2 = New Vector2(0, 0)
    Private _isCentered As Boolean = False
    Private _texture As Texture2D
    Private _windowElementsX As Integer = 8
    Private _windowElementsY As Integer = 7 'includes title bar
    Private _textureScale As Single = 5.0F
    Private _title As String = "{WindowTitle}"
    Private _drawingGradient As Boolean = False 'Checks if this window screen is drawing the gradient.
    Private _closing As Boolean = False

    Public Sub New(ByVal PreScreen As Screen, ByVal Identification As Identifications, ByVal Title As String)
        Me.PreScreen = PreScreen
        Me.Identification = Identification
        Me._isCentered = True
        Me._title = Title
        Me.MouseVisible = False

        Me._texture = TextureManager.GetTexture("GUI\Menus\General")
    End Sub

    Public Sub New(ByVal PreScreen As Screen, ByVal Identification As Identifications, ByVal Title As String, ByVal WindowLocation As Vector2)
        Me.PreScreen = PreScreen
        Me.Identification = Identification
        Me._windowLocation = WindowLocation
        Me._isCentered = False
        Me._title = Title
        Me.MouseVisible = False

        Me._texture = TextureManager.GetTexture("GUI\Menus\General")
    End Sub

    ''' <summary>
    ''' Draws the gradients and window.
    ''' </summary>
    Public Overrides Sub Draw()
        'Gradients:
        If _drawingGradient = True Then
            Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, Me._gradientFade), New Color(42, 167, 198, 0), False, -1)
            Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198, Me._gradientFade), False, -1)
        End If

        Me.DrawWindow()
    End Sub

    Private Sub DrawWindow()
        If _gradientFade = 255 Then
            Dim startPosition As Vector2 = Me._windowLocation
            If _isCentered = True Then
                Dim windowWidth As Integer = CInt(GetWindowElementSize() * _windowElementsX)
                Dim windowHeight As Integer = CInt(GetWindowElementSize() * _windowElementsY)

                startPosition = New Vector2(CSng(Core.windowSize.Width / 2 - windowWidth / 2), CSng(Core.windowSize.Height / 2 - windowHeight / 2))
            End If

            'Frame:
            For x = 0 To _windowElementsX - 1
                For y = 0 To _windowElementsY - 1

                    'Grab texture and sprite effect:
                    Dim r As New Vector2(0, 64)
                    Dim e As SpriteEffects = SpriteEffects.None

                    If x = 0 And y = 0 Then 'Title, left
                        r = New Vector2(0, 64)
                    ElseIf x = 1 And y = 0 Then 'Title, left2
                        r = New Vector2(16, 64)
                    ElseIf x > 1 And x < _windowElementsX - 2 And y = 0 Then 'Title, center
                        r = New Vector2(32, 64)
                    ElseIf x = _windowElementsX - 2 And y = 0 Then 'Title right2
                        r = New Vector2(16, 64)
                        e = SpriteEffects.FlipHorizontally
                    ElseIf x = _windowElementsX - 1 And y = 0 Then 'Title, right
                        r = New Vector2(0, 64)
                        e = SpriteEffects.FlipHorizontally
                    ElseIf x = 0 And y = 1 Then 'Window, up left
                        r = New Vector2(0, 80)
                    ElseIf x = _windowElementsX - 1 And y = 1 Then 'Window, up right
                        r = New Vector2(32, 80)
                    ElseIf x = 0 And y = _windowElementsY - 1 Then 'Window, down left
                        r = New Vector2(0, 112)
                    ElseIf x = _windowElementsX - 1 And y = _windowElementsY - 1 Then 'Window, down right
                        r = New Vector2(32, 112)
                    ElseIf x > 0 And x < _windowElementsX - 1 And y = 1 Then 'Window, up
                        r = New Vector2(16, 80)
                    ElseIf x = 0 And y > 1 And y < _windowElementsY - 1 Then 'Window, left
                        r = New Vector2(0, 96)
                    ElseIf x = _windowElementsX - 1 And y > 1 And y < _windowElementsY - 1 Then 'Window, right
                        r = New Vector2(32, 96)
                    ElseIf x > 0 And x < _windowElementsX - 1 And y = _windowElementsY - 1 Then 'Window, down
                        r = New Vector2(16, 112)
                    Else 'Window, center
                        r = New Vector2(16, 96)
                    End If

                    'Render:
                    Core.SpriteBatch.Draw(Me._texture, New Rectangle(CInt(startPosition.X + x * GetWindowElementSize()), CInt(startPosition.Y + y * GetWindowElementSize()) + _windowSink, GetWindowElementSize(), GetWindowElementSize()), New Rectangle(CInt(r.X), CInt(r.Y), 16, 16), Color.White, 0.0F, Vector2.Zero, e, 0.0F)
                Next
            Next

            'Title:
            Dim titleStartX As Integer = CInt(32 * _textureScale + startPosition.X) 'Skip first two panels
            Dim titleStartY As Integer = CInt(startPosition.Y)
            Dim titleAreaWidth As Integer = CInt((_windowElementsX - 4) * GetWindowElementSize()) 'Width of the window minus two panels on each side.
            Dim titleAreaHeight As Integer = GetWindowElementSize()
            Dim FontSize As Vector2 = FontManager.MainFont.MeasureString(_title)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Me._title, New Vector2(CSng(titleStartX + titleAreaWidth / 2 - FontSize.X / 2), CSng(titleStartY + titleAreaHeight / 2 - FontSize.Y / 2) + _textureScale + _windowSink), Color.White)
        End If
    End Sub

    Private Function GetWindowElementSize() As Integer
        Return CInt(16 * _textureScale)
    End Function

    ''' <summary>
    ''' Updates the fade in of the gradients.
    ''' </summary>
    Public Overrides Sub Update()
        If Me._drawingGradient = False And IsDrawingGradients = False Then
            Me._drawingGradient = True
            IsDrawingGradients = True
        End If

        If _closing = True Then
            If _windowSink > STARTWINDOWSINK Then
                _windowSink -= 5
                If _windowSink < STARTWINDOWSINK Then
                    _windowSink = STARTWINDOWSINK
                End If
            Else
                If _drawingGradient = False Then
                    _gradientFade = 0
                Else
                    _gradientFade -= 25
                End If
                If _gradientFade <= 0 Then
                    _gradientFade = 0
                    Core.SetScreen(Me.PreScreen)
                End If
            End If
        Else
            If _gradientFade < 255 Then
                If _drawingGradient = False Then
                    _gradientFade = 255
                Else
                    _gradientFade += 25
                End If
                If _gradientFade >= 255 Then
                    _gradientFade = 255
                End If
            Else
                If _windowSink < 0 Then
                    If _windowSink < -20 Then
                        _windowSink += 4
                    ElseIf _windowSink < -10 Then
                        _windowSink += 3
                    Else
                        _windowSink += 2
                    End If
                    If _windowSink >= 0 Then
                        _windowSink = 0
                        MouseVisible = True
                    End If
                End If
            End If
        End If
    End Sub

    Public ReadOnly Property FadedIn() As Boolean
        Get
            Return (_gradientFade = 255)
        End Get
    End Property

    Public ReadOnly Property PlayerCanInteract() As Boolean
        Get
            Return (_gradientFade = 255 And _windowSink = 0 And _closing = False)
        End Get
    End Property

    Public Sub CloseScreen()
        _closing = True
    End Sub

    Protected Function GetPositionInWindowTopLeft(ByVal x As Single, ByVal y As Single) As Vector2
        Dim startPosition As Vector2 = Me._windowLocation

        If _isCentered = True Then
            Dim windowWidth As Integer = CInt(GetWindowElementSize() * _windowElementsX)
            Dim windowHeight As Integer = CInt(GetWindowElementSize() * _windowElementsY)
            startPosition = New Vector2(CSng(Core.windowSize.Width / 2 - windowWidth / 2), CSng(Core.windowSize.Height / 2 - windowHeight / 2))
        End If

        Return New Vector2(startPosition.X + x, startPosition.Y + y)
    End Function

    Protected Function OffsetVector(ByVal p As Vector2) As Vector2
        Return New Vector2(p.X, p.Y + _windowSink)
    End Function

    Protected Function OffsetRectangle(ByVal r As Rectangle) As Rectangle
        Return New Rectangle(r.X, r.Y + _windowSink, r.Width, r.Height)
    End Function

    Public Overrides Sub ChangeFrom()
        MyBase.ChangeFrom()
        If Me._drawingGradient = True Then
            IsDrawingGradients = False
        End If
    End Sub

End Class
