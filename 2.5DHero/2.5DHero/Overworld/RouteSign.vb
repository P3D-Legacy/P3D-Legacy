''' <summary>
''' The sign displaying the current location in the world.
''' </summary>
Public Class RouteSign

    Private _positionY As Single = -60
    Private _show As Boolean = False
    Private _delay As Single = 0
    Private _text As String = ""

    ''' <summary>
    ''' Sets the values of the RouteSign and displays it on the screen.
    ''' </summary>
    Public Sub Setup(ByVal newText As String)
        'Only if the text is different from last time the RouteSign showed up, display the RouteSign.
        If newText.ToLower() <> Me._text.ToLower() Then
            _show = True
            _delay = 13.0F
            _text = newText
        End If
    End Sub

    ''' <summary>
    ''' Hides the RouteSign.
    ''' </summary>
    Public Sub Hide()
        Me._show = False
    End Sub

    ''' <summary>
    ''' Update the RouteSign.
    ''' </summary>
    Public Sub Update()
        If Me._delay > 0.0F Then
            If Me._positionY < 5.0F Then
                Me._positionY += 1.2F
            End If
            Me._delay -= 0.1F
            If Me._delay <= 0.0F Then
                Me._delay = 0.0F
            End If
        Else
            If Me._positionY > -60 Then
                Me._positionY -= 1.2F
                If Me._positionY <= -60 Then
                    Me._show = False
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Renders the RouteSign.
    ''' </summary>
    Public Sub Draw()
        If Me._show = True Then
            Dim placeString As String = Localization.GetString("Places_" & Me._text, Me._text)

            'Get the point to render the text to.
            Dim pX As Integer = CInt(316 / 2) - CInt(FontManager.InGameFont.MeasureString(placeString).X / 2)

            Core.SpriteBatch.DrawInterface(TextureManager.GetTexture("GUI\Overworld\Sign"), New Rectangle(5, CInt(Me._positionY), 316, 60), Color.White) 'Draw the sign image.
            Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, placeString, New Vector2(pX, CInt(Me._positionY) + 13), Color.Black) 'Draw the text on the sign.
        End If
    End Sub

End Class