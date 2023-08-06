''' <summary>
''' The notification popup.
''' </summary>
Public Class NotificationPopup

    Private _background As Texture2D
    Private _icon As Texture2D
    Private FrameSizeBack As Integer
    Private FramesizeIcon As Integer
    Private _positionY As Single = -12

    Private _started As Boolean = False
    Private _scale As Single = CInt(SpriteBatch.InterfaceScale * 2)
    Private _soundEffect As String = ""
    Private _text As String = ""
    Private _size As Size = New Size(13, 3)
    Private _backgroundIndex As Vector2 = New Vector2(0, 0)
    Private _iconIndex As Vector2 = New Vector2(0, 0)
    Private _delay As Integer = Nothing
    Public _delayDate As Date = Nothing

    Public _waitForInput As Boolean = False
    Public _interacted As Boolean = False
    Public _scriptFile As String = ""
    Public _forceAccept As Boolean = False

    Public IsReady As Boolean = False

    ''' <summary>
    ''' Sets the values of the NotificationPopup and displays it on the screen.
    ''' </summary>
    Public Sub Setup(Text As String, Optional Delay As Integer = 500, Optional BackgroundIndex As Integer = 0, Optional IconIndex As Integer = 0, Optional SoundEffect As String = "", Optional ScriptFile As String = "", Optional ForceAccept As Boolean = False)
        _text = Text

        If Delay <> -1 Then
            If Delay = 0 Then
                _waitForInput = True
            End If
            _delay = Delay
        Else
            _delay = 500
        End If

        If BackgroundIndex <> -1 Then
            _backgroundIndex = New Vector2(BackgroundIndex, 0)
        Else
            _backgroundIndex = New Vector2(0, 0)
        End If

        While _backgroundIndex.X >= 3
            _backgroundIndex.X -= 3
            _backgroundIndex.Y += 1
        End While
        If _backgroundIndex.X < 0 Then
            _backgroundIndex.X = 0
        End If

        Dim BackTexture As Texture2D = TextureManager.GetTexture("Textures\Notifications\Backgrounds")
        FrameSizeBack = CInt(BackTexture.Width / 3)
        _background = TextureManager.GetTexture(BackTexture, New Rectangle(CInt(_backgroundIndex.X * FrameSizeBack), CInt(_backgroundIndex.Y * FrameSizeBack), FrameSizeBack, FrameSizeBack))

        _positionY = CInt(0 - _size.Height * (FrameSizeBack / 3) * _scale - 12)

        If IconIndex <> -1 Then
            _iconIndex = New Vector2(IconIndex, 0)
        Else
            _iconIndex = New Vector2(0, 0)
        End If

        While _iconIndex.X >= 3
            _iconIndex.X -= 3
            _iconIndex.Y += 1
        End While
        If _iconIndex.X < 0 Then
            _iconIndex.X = 0
        End If

        Dim IconTexture As Texture2D = TextureManager.GetTexture("Textures\Notifications\Icons")
        FramesizeIcon = CInt(IconTexture.Width / 3)
        _icon = TextureManager.GetTexture(IconTexture, New Rectangle(CInt(_iconIndex.X * FramesizeIcon), CInt(_iconIndex.Y * FramesizeIcon), FramesizeIcon, FramesizeIcon))

        _scriptFile = ScriptFile

        _soundEffect = SoundEffect

        _forceAccept = ForceAccept
    End Sub

    ''' <summary>
    ''' Dismiss the Popup
    ''' </summary>
    Public Sub Dismiss()
        Me._waitForInput = False
        Me._delayDate = Date.Now
        _interacted = True
    End Sub

    ''' <summary>
    ''' Update the NotificationPopup.
    ''' </summary>
    Public Sub Update()
        If _started = False Then
            _delayDate = Date.Now.AddMilliseconds(CDbl(_delay * 10))
            _started = True
        End If

        If _waitForInput = True Then
            If Me._positionY < 5.0F Then
                Me._positionY += CInt(0.7 * (FrameSizeBack / 3 * _scale) / _size.Height)
            Else
                If _soundEffect IsNot "" Then
                    SoundManager.PlaySound("Notifications\" & _soundEffect)
                    _soundEffect = ""
                End If
            End If
        Else
            If Date.Now < _delayDate Then
                If Me._positionY < 5.0F Then
                    Me._positionY += CInt(0.7 * (FrameSizeBack / 3 * _scale) / _size.Height)
                Else
                    If _soundEffect IsNot "" Then
                        SoundManager.PlaySound("Notifications\" & _soundEffect)
                        _soundEffect = ""
                    End If
                End If
            Else
                Dim BackY As Integer = CInt(0 - _size.Height * (FrameSizeBack / 3) * _scale - (FrameSizeBack / 3 * _scale) - 5)
                If Me._interacted = True OrElse _forceAccept = True Then
                    If Me._positionY > BackY Then
                        Me._positionY -= CInt(1.5 * (FrameSizeBack / 3 * _scale) / _size.Height)
                        If Me._positionY <= BackY Then
                            Me._positionY = BackY
                            If Me._scriptFile <> "" Then
                                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(Me._scriptFile, 0, False,, "Notification")
                            End If
                            Me.IsReady = True
                        End If
                    End If
                Else
                    If Me._positionY > BackY Then
                        Me._positionY -= CInt(0.7 * (FrameSizeBack / 3 * _scale) / _size.Height)
                        If Me._positionY <= BackY Then
                            Me._positionY = BackY
                            Me.IsReady = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Renders the NotificationPopup.
    ''' </summary>
    Public Sub Draw()
        Dim TextHeader As String = _text.GetSplit(0, "*").Replace(CChar("~"), Environment.NewLine).CropStringToWidth(FontManager.InGameFont, CInt(_scale), CInt((_size.Width * (FrameSizeBack / 3) - FrameSizeBack / 3 * 4) * _scale))
        Dim TextBody As String = _text.GetSplit(1, "*").Replace(CChar("~"), Environment.NewLine)


        While FontManager.InGameFont.MeasureString(TextHeader).Y * 2 + FontManager.InGameFont.MeasureString(TextBody).Y > CInt(((_size.Height * FrameSizeBack / 3) - FrameSizeBack / 3) * _scale - 5)
            _size.Height += 1
        End While

        Dim BackGroundOffsetX As Integer = CInt(Core.windowSize.Width - (_size.Width * (FrameSizeBack / 3) * _scale) - (FrameSizeBack / 3) * 2 - 5)

        'Draw the frame.
        Canvas.DrawImageBorder(_background, CInt(_scale), New Rectangle(BackGroundOffsetX, CInt(Me._positionY), CInt(_size.Width * (FrameSizeBack / 3) * _scale), CInt(_size.Height * (FrameSizeBack / 3) * _scale)))

        'Draw the icon.
        Core.SpriteBatch.DrawInterface(_icon, New Rectangle(CInt(BackGroundOffsetX + (FrameSizeBack / 3 + 3) * _scale - _icon.Width / 3), CInt(Me._positionY + ((FrameSizeBack / 3 * _size.Height / 2) - FrameSizeBack / 3 * 0.5) * _scale - _icon.Width / 3), CInt(_icon.Width * _scale), CInt(_icon.Height * _scale)), Color.White)

        Dim TextOffset = CInt(BackGroundOffsetX + FrameSizeBack / 3 * _scale * 4)
        If TextBody <> "" Then
            If TextHeader <> "" Then
                'Draw the header, then the body
                Core.SpriteBatch.DrawString(FontManager.InGameFont, TextHeader.CropStringToWidth(FontManager.InGameFont, CInt(_scale), CInt((_size.Width * (FrameSizeBack / 3) - FrameSizeBack / 3 * 4) * _scale)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack / 3)), Color.Black, 0.0F, Vector2.Zero, CSng(_scale), SpriteEffects.None, 0.0F)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, TextBody.CropStringToWidth(FontManager.InGameFont, CInt(_scale / 2), CInt((_size.Width * (FrameSizeBack / 3) - FrameSizeBack / 3 * 4) * _scale)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack / 3 + (FontManager.InGameFont.MeasureString(TextHeader).Y * _scale))), Color.Black, 0.0F, Vector2.Zero, CSng(_scale / 2), SpriteEffects.None, 0.0F)
            Else
                'Just draw the body
                Core.SpriteBatch.DrawString(FontManager.InGameFont, TextBody.CropStringToWidth(FontManager.InGameFont, CInt(_scale / 2), CInt((_size.Width * (FrameSizeBack / 3) - FrameSizeBack / 3 * 4) * _scale)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack / 3)), Color.Black, 0.0F, Vector2.Zero, CSng(_scale / 2), SpriteEffects.None, 0.0F)
            End If
        Else
            'Just draw the header
            Core.SpriteBatch.DrawString(FontManager.InGameFont, TextHeader.CropStringToWidth(FontManager.InGameFont, CInt(_scale), CInt((_size.Width * (FrameSizeBack / 3) - FrameSizeBack / 3 * 4) * _scale)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack / 3)), Color.Black, 0.0F, Vector2.Zero, CSng(_scale), SpriteEffects.None, 0.0F)
        End If

        Dim InteractText As String = "[" & Localization.GetString("game_notification_dismiss") & "]"
        If Me._scriptFile <> "" OrElse _waitForInput = True Then
            InteractText = "[" & Localization.GetString("game_notification_accept") & "]"
        End If
        Dim InteractOffset As Vector2 = New Vector2(CInt(Core.windowSize.Width - FrameSizeBack / 3 * _scale - FontManager.InGameFont.MeasureString(InteractText).X * _scale / 2), CInt(Me._positionY + _size.Height * (FrameSizeBack / 3) * _scale + 5))

        Core.SpriteBatch.DrawInterface(_background, New Rectangle(CInt(InteractOffset.X), CInt(InteractOffset.Y), CInt(FontManager.InGameFont.MeasureString(InteractText).X * _scale / 2), CInt(FontManager.InGameFont.MeasureString(InteractText).Y * _scale / 2)), New Rectangle(CInt(FrameSizeBack / 3), CInt(FrameSizeBack / 3), CInt(FrameSizeBack / 3), CInt(FrameSizeBack / 3)), Color.White)
        Core.SpriteBatch.DrawString(FontManager.InGameFont, InteractText, New Vector2(CInt(InteractOffset.X), CInt(InteractOffset.Y)), Color.Black)
    End Sub

End Class