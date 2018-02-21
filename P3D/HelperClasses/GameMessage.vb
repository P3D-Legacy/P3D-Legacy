''' <summary>
''' This class can show a message ingame.
''' </summary>
Public Class GameMessage

    Inherits BasicObject

    Public Enum DockStyles
        Top
        Down
        Left
        Right
        None
    End Enum

#Region "Fields"

    Private _Duration As Single = CSng(0)
    Private _BackgroundColor As Color = Color.White
    Private _TextureRectangle As Rectangle = New Rectangle(0, 0, 0, 0)
    Private _Fullscreen As Boolean = False
    Private _Dock As DockStyles = DockStyles.Top
    Private _Text As String = ""
    Private _TextPosition As Vector2 = New Vector2(0, 0)
    Private _spriteFont As SpriteFont
    Private _TextColor As Color = Color.White
    Private _showAlways As Boolean = False
    Private _alpha As Integer = 0
    Private _alphaBlend As Boolean = True

#End Region

#Region "Properties"

    ''' <summary>
    ''' The duretion in milliseconds this message will be displayed.
    ''' </summary>
    Public Property Duration As Single
        Get
            Return _Duration
        End Get
        Set(value As Single)
            _Duration = value
        End Set
    End Property

    ''' <summary>
    ''' The backgroundcolor the texture will be colored with.
    ''' </summary>
    Public Property BackgroundColor As Color
        Get
            Return _BackgroundColor
        End Get
        Set(value As Color)
            _BackgroundColor = value
        End Set
    End Property

    ''' <summary>
    ''' The rectangle from the texture to be drawn.
    ''' </summary>
    Public Property TextureRectangle As Rectangle
        Get
            Return _TextureRectangle
        End Get
        Set(value As Rectangle)
            _TextureRectangle = value
        End Set
    End Property

    ''' <summary>
    ''' Shows the message in fullscreen (overides size and dock)
    ''' </summary>
    Public Property Fullscreen As Boolean
        Get
            Return _Fullscreen
        End Get
        Set(value As Boolean)
            _Fullscreen = value
        End Set
    End Property

    ''' <summary>
    ''' Docks the message at a side of the window (overides size)
    ''' </summary>
    Public Property Dock As DockStyles
        Get
            Return _Dock
        End Get
        Set(value As DockStyles)
            _Dock = value
        End Set
    End Property

    ''' <summary>
    ''' The text that will be displayed.
    ''' </summary>
    Public Property Text As String
        Get
            Return _Text
        End Get
        Set(value As String)
            _Text = value
        End Set
    End Property

    ''' <summary>
    ''' The position the text is drawn (relative to message-position)
    ''' </summary>
    Public Property TextPosition As Vector2
        Get
            Return _TextPosition
        End Get
        Set(value As Vector2)
            _TextPosition = value
        End Set
    End Property

    Public Property SpriteFont As SpriteFont
        Get
            Return _spriteFont
        End Get
        Set(value As SpriteFont)
            _spriteFont = value
        End Set
    End Property

    ''' <summary>
    ''' The color for the text.
    ''' </summary>
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
        End Set
    End Property

    ''' <summary>
    ''' If this value is true, the duretion has no effect on the visibility of this message.
    ''' </summary>
    Public Property ShowAlways As Boolean
        Get
            Return _showAlways
        End Get
        Set(value As Boolean)
            Me._showAlways = value
        End Set
    End Property

    Public Property AlphaBlend As Boolean
        Get
            Return _alphaBlend
        End Get
        Set(value As Boolean)
            Me._alphaBlend = value
        End Set
    End Property

#End Region

    Public Sub New(ByVal Texture As Texture2D, ByVal Width As Integer, ByVal Height As Integer, ByVal Position As Vector2)
        Me.New(Texture, New Size(Width, Height), Position)
    End Sub

    Public Sub New(ByVal Texture As Texture2D, ByVal Size As Size, ByVal Position As Vector2)
        MyBase.New(Texture, Size.Width, Size.Height, Position)

        Me.Size = Size
        Me.Visible = False
    End Sub

    ''' <summary>
    ''' Sets the properties of the text.
    ''' </summary>
    ''' <param name="Text">The actual text to be displayed.</param>
    ''' <param name="SpriteFont">The spritefont, the text will be drawn with.</param>
    ''' <param name="TextColor">The color the text will be drawn in.</param>
    Public Sub SetupText(ByVal Text As String, ByVal SpriteFont As SpriteFont, ByVal TextColor As Color)
        Me._Text = Text
        Me._spriteFont = SpriteFont
        Me._TextColor = TextColor
    End Sub

    ''' <summary>
    ''' Updates the message (required!)
    ''' </summary>
    Public Sub Update()
        If _showAlways = False Then
            If _Duration > CSng(0) Then
                Me.Visible = True

                Me._Duration -= CSng(0.1)
                If _Duration <= CSng(0) Then
                    _Duration = CSng(0)
                End If

                If _alphaBlend = True Then
                    If Me._alpha < 255 Then
                        Me._alpha += 5
                        If Me._alpha > 255 Then
                            Me._alpha = 255
                        End If
                    End If
                Else
                    Me._alpha = 255
                End If
            Else
                If Me._alphaBlend = True Then
                    If Me._alpha > 0 Then
                        Me._alpha -= 5
                        If Me._alpha <= 0 Then
                            Me._alpha = 0
                            Me.Visible = False
                        End If
                    End If
                Else
                    Me.Visible = False
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Draw the message.
    ''' </summary>
    Public Sub Draw()
        If Visible = True Then
            If _Fullscreen = True Then
                DrawMe(New Size(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height), New Vector2(0, 0))
            Else
                Select Case Me._Dock
                    Case DockStyles.None
                        DrawMe(Me.Size, Me.Position)
                    Case DockStyles.Top
                        DrawMe(New Size(Core.ScreenSize.Width, Me.Size.Height), New Vector2(0, 0))
                    Case DockStyles.Down
                        DrawMe(New Size(Core.ScreenSize.Width, Me.Size.Height), New Vector2(0, Core.ScreenSize.Height - Me.Size.Height))
                    Case DockStyles.Left
                        DrawMe(New Size(Me.Size.Width, Core.ScreenSize.Height), New Vector2(0, 0))
                    Case DockStyles.Right
                        DrawMe(New Size(Me.Size.Width, Core.ScreenSize.Height), New Vector2(Core.ScreenSize.Width - Me.Size.Width, 0))
                End Select
            End If
        End If
    End Sub

    ''' <summary>
    ''' Actual drawing stuff.
    ''' </summary>
    Private Sub DrawMe(ByVal drawSize As Size, ByVal drawPosition As Vector2)
        Core.SpriteBatch.DrawInterface(Me.Texture, New Rectangle(CInt(drawPosition.X), CInt(drawPosition.Y), drawSize.Width, drawSize.Height), TextureRectangle, New Color(_BackgroundColor.R, _BackgroundColor.G, _BackgroundColor.B, Me._alpha))
        Core.SpriteBatch.DrawInterfaceString(_spriteFont, Me._Text, LinkVector2(Me.Position, Me._TextPosition, 1), New Color(Me._TextColor.R, Me._TextColor.G, Me._TextColor.B, Me._alpha))
    End Sub

    Private Function LinkVector2(ByVal Vector1 As Vector2, ByVal Vector2 As Vector2, ByVal factor As Single) As Vector2
        Return New Vector2(Vector1.X + factor * Vector2.X, Vector1.Y + factor * Vector2.Y)
    End Function

    ''' <summary>
    ''' Display the message on the screen.
    ''' </summary>
    ''' <param name="Duration">The time span the message will appear on the screen.</param>
    ''' <param name="graphics">The graphics device the message will be drawn on.</param>
    Public Sub ShowMessage(ByVal Duration As Single, ByVal graphics As GraphicsDevice)
        Me._Duration = Duration
        Me.Visible = True

        Me._alpha = 0
    End Sub

    Public Sub ShowMessage(ByVal Text As String, ByVal Duration As Single, ByVal SpriteFont As SpriteFont, ByVal TextColor As Color)
        Me._Text = Text
        Me._spriteFont = SpriteFont
        Me._TextColor = TextColor
        Me._Duration = Duration
        Me.Visible = True

        Me._alpha = 0
    End Sub

    Public Sub HideMessage()
        Me.Visible = False
        Me._Duration = 0.0F
        Me._alpha = 0
    End Sub

End Class