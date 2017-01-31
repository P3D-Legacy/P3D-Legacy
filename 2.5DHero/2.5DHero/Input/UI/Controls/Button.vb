Namespace UI.GameControls

    Public Class Button

        Inherits Control

        Private _setSelectedBackColor As Boolean = False
        Private _setSelectedFontColor As Boolean = False

        Private _selectedBackColor As Color
        Private _selectedFontColor As Color

        Private _image As Texture2D

        Public Property SelectedBackColor As Color
            Get
                Return _selectedBackColor
            End Get
            Set(value As Color)
                _selectedBackColor = value
                _setSelectedBackColor = True
            End Set
        End Property

        Public Property SelectedFontColor As Color
            Get
                Return _selectedFontColor
            End Get
            Set(value As Color)
                _selectedFontColor = value
                _setSelectedFontColor = True
            End Set
        End Property

        Public Property Image() As Texture2D
            Get
                Return _image
            End Get
            Set(value As Texture2D)
                _image = value
            End Set
        End Property

        Public Sub New(ByVal screenInstance As Screen, ByVal font As SpriteFont)
            MyBase.New(screenInstance)

            Me.Font = font
            BorderWidth = 0
            Width = 160
            Height = 40
        End Sub

        Protected Overrides Sub DrawClient()
            If Visible Then
                'Draw border:
                If BorderWidth > 0 Then
                    SpriteBatch.DrawRectangle(New Rectangle(Position.X, 'X
                                                                 Position.Y, 'Y
                                                                 Width + (BorderWidth * 2), 'Width
                                                                 Height + (BorderWidth * 2) 'Height
                                                            ), BorderColor)
                End If

                Dim foreColor As Color = FontColor
                Dim contentColor As Color = BackColor

                If IsFocused = True Or MouseInClientArea() Then
                    If _setSelectedBackColor Then
                        contentColor = _selectedBackColor
                    End If
                    If _setSelectedFontColor Then
                        foreColor = _selectedFontColor
                    End If
                End If

                'Draw content:
                SpriteBatch.DrawRectangle(New Rectangle(Position.X + BorderWidth, Position.Y + BorderWidth, Width, Height), contentColor)

                'Draw text:
                Dim textSize As Vector2 = Font.MeasureString(TESTFORHEIGHTCHARS)
                textSize.X = Font.MeasureString(Text).X

                Dim textPos As Vector2 = New Vector2(Position.X + BorderWidth + (Width / 2.0F) - ((textSize.X * FontSize) / 2.0F), Position.Y + BorderWidth + (Height / 2.0F) - ((textSize.Y * FontSize) / 2.0F))

                If _image IsNot Nothing Then
                    textPos.X += _image.Width / 2.0F + 4

                    SpriteBatch.Draw(_image, New Rectangle(CInt(textPos.X - 12 - _image.Width), CInt(Position.Y + (Height / 2.0F) - (_image.Height / 2.0F)), _image.Width, _image.Height), New Color(255, 255, 255, foreColor.A))
                End If

                FontRenderer.DrawString(Font, Text, textPos, foreColor, 0F, Vector2.Zero, FontSize, SpriteEffects.None, 0F)
            End If
        End Sub

    End Class

End Namespace