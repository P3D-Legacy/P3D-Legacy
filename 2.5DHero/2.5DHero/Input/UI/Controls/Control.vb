Namespace UI.GameControls

    ''' <summary>
    ''' The base class for all UI controls.
    ''' </summary>
    Public MustInherit Class Control

        Protected Const TESTFORHEIGHTCHARS As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz;:_-.,*~+'#1234567890?ß\/!""§$%&/()}][{"

        Private _text As String = String.Empty

        Private _isFocused As Boolean = False
        Private _focusOnClick As Boolean = True

        'Visuals:
        Private _font As SpriteFont
        Private _fontSize As Single = 1.0F
        Private _fontColor As Color = Color.Black
        Private _backColor As Color = Color.White
        Private _borderColor As Color = Color.Black
        Private _width As Integer = 120
        Private _height As Integer = -1
        Private _borderWidth As Integer = 1
        Private _position As Drawing.Point = New Drawing.Point(100, 100)
        Private _visible As Boolean = True

        Private _createdScreenInstance As Screen

        ''' <summary>
        ''' Returns the font renderer applicable to the situation.
        ''' </summary>
        ''' <returns></returns>
        Protected ReadOnly Property FontRenderer() As SpriteBatch
            Get
                If _createdScreenInstance.Equals(CurrentScreen) Then
                    Return Core.FontRenderer
                Else
                    Return Core.SpriteBatch
                End If
            End Get
        End Property

        Protected Sub New(ByVal createdScreenInstance As Screen)
            _createdScreenInstance = createdScreenInstance
        End Sub

        ''' <summary>
        ''' The text property of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(value As String)
                _text = value
            End Set
        End Property

        ''' <summary>
        ''' If the control is focused.
        ''' </summary>
        ''' <returns></returns>
        Public Property IsFocused() As Boolean
            Get
                Return _isFocused
            End Get
            Set(value As Boolean)
                If value Then
                    OnFocused(New EventArgs())
                Else
                    OnDeFocused(New EventArgs())
                End If
            End Set
        End Property

        ''' <summary>
        ''' The font that renders on this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property Font() As SpriteFont
            Get
                Return _font
            End Get
            Set(value As SpriteFont)
                _font = value
            End Set
        End Property

        ''' <summary>
        ''' The width of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(value As Integer)
                _width = value
            End Set
        End Property

        ''' <summary>
        ''' The height of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property Height As Integer
            Get
                Return _height
            End Get
            Set(value As Integer)
                _height = value
            End Set
        End Property

        ''' <summary>
        ''' The position of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property Position As Drawing.Point
            Get
                Return _position
            End Get
            Set(value As Drawing.Point)
                _position = value
            End Set
        End Property

        ''' <summary>
        ''' The font size of the font of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property FontSize As Single
            Get
                Return _fontSize
            End Get
            Set(value As Single)
                _fontSize = value
            End Set
        End Property

        ''' <summary>
        ''' The color of the font of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property FontColor() As Color
            Get
                Return _fontColor
            End Get
            Set(value As Color)
                _fontColor = value
            End Set
        End Property

        ''' <summary>
        ''' The border color of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property BorderColor() As Color
            Get
                Return _borderColor
            End Get
            Set(value As Color)
                _borderColor = value
            End Set
        End Property

        ''' <summary>
        ''' The background color of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property BackColor() As Color
            Get
                Return _backColor
            End Get
            Set(value As Color)
                _backColor = value
            End Set
        End Property

        ''' <summary>
        ''' The border width of this control.
        ''' </summary>
        ''' <returns></returns>
        Public Property BorderWidth() As Integer
            Get
                Return _borderWidth
            End Get
            Set(value As Integer)
                _borderWidth = value
            End Set
        End Property

        ''' <summary>
        ''' If this control gets focused when clicked on.
        ''' </summary>
        ''' <returns></returns>
        Public Property FocusOnClick() As Boolean
            Get
                Return _focusOnClick
            End Get
            Set(value As Boolean)
                _focusOnClick = value
            End Set
        End Property

        ''' <summary>
        ''' If the control is visible or not.
        ''' </summary>
        ''' <returns></returns>
        Public Property Visible() As Boolean
            Get
                Return _visible
            End Get
            Set(value As Boolean)
                _visible = value
            End Set
        End Property

        ''' <summary>
        ''' Draws the control.
        ''' </summary>
        Public Sub Draw()
            If _visible = True Then
                DrawClient()
            End If
        End Sub

        Protected Overridable Sub DrawClient()
            'Draw in client control.
        End Sub

        ''' <summary>
        ''' Updates the control.
        ''' </summary>
        Public Sub Update()
            If Controls.Accept(True, False, False) = True Then
                If MouseInClientArea() Then
                    If _focusOnClick Then
                        IsFocused = True
                    End If
                    If IsFocused Then
                        OnClick(New OnClickEventArgs(ActivationMethod.Click))
                    End If
                End If
            End If
            If IsFocused Then
                If Controls.Accept(False, True, False) Then
                    OnClick(New OnClickEventArgs(ActivationMethod.Keyboard))
                ElseIf Controls.Accept(False, False, True) Then
                    OnClick(New OnClickEventArgs(ActivationMethod.Controller))
                End If
            End If

            UpdateClient()
        End Sub

        Protected Overridable Sub UpdateClient()
            'Update in client control.
        End Sub

        Protected Overridable Function GetClientRectangle() As Rectangle
            Return New Rectangle(Position.X, 'X
                                Position.Y, 'Y
                                Width + (BorderWidth * 2), 'Width
                                Height + (BorderWidth * 2) 'Height
                                )
        End Function

        Protected Function MouseInClientArea() As Boolean
            Return GetClientRectangle().Contains(MouseHandler.MousePosition)
        End Function

#Region "Events"

        ''' <summary>
        ''' The event that gets fired when the control gets focused.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event Focused(ByVal sender As Object, ByVal e As EventArgs)

        ''' <summary>
        ''' The event that gets fired when the control loses focus.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event DeFocused(ByVal sender As Object, ByVal e As EventArgs)

        ''' <summary>
        ''' The event that occurrs when the control gets clicked.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event Click(ByVal sender As Object, ByVal e As EventArgs)

        ''' <summary>
        ''' Raises the Focused event.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Sub OnFocused(e As EventArgs)
            _isFocused = True

            RaiseEvent Focused(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the DeFocused event.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Sub OnDeFocused(e As EventArgs)
            _isFocused = False

            RaiseEvent DeFocused(Me, e)
        End Sub

        Protected Sub OnClick(e As OnClickEventArgs)
            RaiseEvent Click(Me, e)
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' The activation method for a click or similar event.
    ''' </summary>
    Public Enum ActivationMethod
        Click
        Keyboard
        Controller
    End Enum

    ''' <summary>
    ''' Event arguments for a click event.
    ''' </summary>
    Public Class OnClickEventArgs

        Inherits EventArgs

        Private _method As ActivationMethod

        ''' <summary>
        ''' Creates an instance of the OnClickEventArgs class.
        ''' </summary>
        ''' <param name="method"></param>
        Public Sub New(ByVal method As ActivationMethod)
            _method = method
        End Sub

        ''' <summary>
        ''' The method used to initiate this click event.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ActivationMethod() As ActivationMethod
            Get
                Return _method
            End Get
        End Property

    End Class

End Namespace