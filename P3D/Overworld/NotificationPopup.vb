''' <summary>
''' The notification popup.
''' </summary>
Public Class NotificationPopup

    Private _background As Texture2D
    Private _box As Texture2D
    Private _icon As Texture2D
    Private FrameSizeBack As Integer
    Private FramesizeBox As Integer
    Private FramesizeIcon As Integer
    Private _positionY As Single = -12
    Private _delay As Date
    Private _soundEffect As String = ""
    Private _text As String = ""
    Private _size As Size = New Size(10, 3)
    Private _backgroundIndex As Vector2 = New Vector2(0, 0)
    Private _iconIndex As Vector2 = New Vector2(0, 0)

    Public _waitForInput As Boolean = False
    Public _scriptFile As String = ""

    Public _show As Boolean = False

    ''' <summary>
    ''' Sets the values of the NotificationPopup and displays it on the screen.
    ''' </summary>
    Public Sub Setup(Text As String, Optional Delay As Integer = 500, Optional BackgroundIndex As Integer = 0, Optional IconIndex As Integer = 0, Optional SoundEffect As String = "", Optional ScriptFile As String = "")
        _text = Text

        If Delay <= 0 Then
            _waitForInput = True
            _delay = Date.Now
        Else
            _delay = Date.Now.AddMilliseconds(CDbl(Delay * 10))
        End If

        _backgroundIndex = New Vector2(BackgroundIndex, 0)
        While _backgroundIndex.X >= 3
            _backgroundIndex.X -= 3
            _backgroundIndex.Y += 1
        End While
        If _backgroundIndex.X < 0 Then
            _backgroundIndex.X = 0
        End If
        FrameSizeBack = CInt(TextureManager.GetTexture(GameModeManager.ActiveGameMode.ContentPath & "Textures\Notifications\Backgrounds").Width / 3)
        _background = TextureManager.GetTexture(GameModeManager.ActiveGameMode.ContentPath & "Textures\Notifications\Backgrounds", New Rectangle(CInt(_backgroundIndex.X * FrameSizeBack), CInt(_backgroundIndex.Y * FrameSizeBack), FrameSizeBack, FrameSizeBack))

        _positionY = CInt(0 - _size.Height * (FrameSizeBack / 3) - 12)

        FramesizeBox = CInt(TextureManager.GetTexture(GameModeManager.ActiveGameMode.ContentPath & "Textures\Notifications\Boxes").Width / 3)
        _box = TextureManager.GetTexture(GameModeManager.ActiveGameMode.ContentPath & "Textures\Notifications\Backgrounds", New Rectangle(CInt(_backgroundIndex.X * FramesizeBox), CInt(_backgroundIndex.Y * FramesizeBox), FramesizeBox, FramesizeBox))

        FramesizeIcon = CInt(TextureManager.GetTexture(GameModeManager.ActiveGameMode.ContentPath & "Textures\Notifications\Icons").Width / 3)
        _iconIndex = New Vector2(IconIndex, 0)
        While _iconIndex.X >= 3
            _iconIndex.X -= 3
            _iconIndex.Y += 1
        End While
        If _iconIndex.X < 0 Then
            _iconIndex.X = 0
        End If
        _icon = TextureManager.GetTexture(GameModeManager.ActiveGameMode.ContentPath & "Textures\Notifications\Icons", New Rectangle(CInt(_iconIndex.X * FramesizeIcon), CInt(_iconIndex.Y * FramesizeIcon), FramesizeIcon, FramesizeIcon))

        _scriptFile = ScriptFile

        If Me._scriptFile <> "" Then
            Me._text &= "~[" & Localization.GetString("game_notification_respond") & "]"
        Else
            Me._text &= "~[" & Localization.GetString("game_notification_dismiss") & "]"
        End If

        _soundEffect = SoundEffect
        _show = True
    End Sub

    ''' <summary>
    ''' Execute the specified script
    ''' </summary>
    Public Sub Accept()
        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(_scriptFile, 0, False)
        Me._waitForInput = False
        Me._delay = Date.Now
    End Sub

    ''' <summary>
    ''' Dismiss the Popup
    ''' </summary>
    Public Sub Dismiss()
        Me._waitForInput = False
        Me._delay = Date.Now
    End Sub

    ''' <summary>
    ''' Update the NotificationPopup.
    ''' </summary>
    Public Sub Update()
        If Date.Now < Me._delay Then
            If Me._positionY < 5.0F Then
                Me._positionY += CInt(0.7 * (FrameSizeBack / _size.Height))
            Else
                If _soundEffect IsNot "" Then
                    SoundManager.PlaySound("Notifications\" & _soundEffect)
                    _soundEffect = ""
                End If
            End If
        Else
            If _waitForInput = False Then
                Dim BackY As Integer = CInt(0 - _size.Height * FrameSizeBack - 12)
                If Me._positionY > BackY Then
                    Me._positionY -= CInt(0.7 * (FrameSizeBack / _size.Height))
                    If Me._positionY <= BackY Then
                        Me._positionY = BackY
                        Me._show = False
                    End If
                End If
            Else
                If Me._scriptFile <> "" Then
                    CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(Me._scriptFile, 0, False)
                    Me._waitForInput = False
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Renders the NotificationPopup.
    ''' </summary>
    Public Sub Draw()
        If Me._show = True Then
            Dim TextBody As String = ""
            Dim TextHeader As String
            Dim Scale As Double = SpriteBatch.InterfaceScale() * 2

            If _text.Contains("*") Then
                TextHeader = _text.GetSplit(0, "*").Replace(CChar("~"), Environment.NewLine)
                TextBody = _text.GetSplit(1, "*").Replace(CChar("~"), Environment.NewLine)
            Else
                TextHeader = _text.Replace(CChar("~"), Environment.NewLine)
            End If

            While FontManager.InGameFont.MeasureString(_text.Replace(CChar("~"), Environment.NewLine)).Y > CInt(_size.Height * FrameSizeBack - 2 * FrameSizeBack)
                _size.Height += 1
            End While

            Dim BackGroundOffsetX As Integer = CInt(Core.windowSize.Width - _size.Width * FrameSizeBack - 5)
            Dim TextOffset As Integer

            'Draw the frame.
            Canvas.DrawImageBorder(_background, CInt(Scale), New Rectangle(BackGroundOffsetX, CInt(Me._positionY), CInt(_size.Width * FrameSizeBack), CInt(_size.Height * FrameSizeBack)), True)

            'Draw the (icon) box.
            Core.SpriteBatch.DrawInterface(_box, New Rectangle(CInt(FramesizeBox * Scale + 5), CInt(FramesizeBox * Scale + Me._positionY), CInt(_box.Width * Scale), CInt(_box.Height * Scale)), Color.White)

            'Draw the icon.
            Core.SpriteBatch.DrawInterface(_icon, New Rectangle(CInt(FramesizeIcon * Scale + 5), CInt(FramesizeIcon * Scale + Me._positionY), CInt(_icon.Width * Scale), CInt(_icon.Height * Scale)), Color.White)

            TextOffset = CInt(FrameSizeBack * Scale + BackGroundOffsetX * Scale)

            If TextBody <> "" Then
                If TextHeader <> "" Then
                    'Draw the header, then the body
                    Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, TextBody.CropStringToWidth(FontManager.InGameFont, CInt(Scale), CInt(_size.Width * FrameSizeBack - FrameSizeBack * 4)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack)), Color.Black)
                    Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, TextHeader.CropStringToWidth(FontManager.InGameFont, CInt(Scale / 2), CInt(_size.Width * FrameSizeBack - FrameSizeBack * 4)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack + FontManager.InGameFont.MeasureString(TextHeader).Y)), Color.Black)
                Else
                    'Just draw the body
                    Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, TextBody.CropStringToWidth(FontManager.InGameFont, CInt(Scale / 2), CInt(_size.Width * FrameSizeBack - FrameSizeBack * 4)), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack)), Color.Black)
                End If
            Else
                'Just draw the header
                Core.SpriteBatch.DrawInterfaceString(FontManager.InGameFont, TextHeader.CropStringToWidth(FontManager.InGameFont, CInt(Scale), CInt(_size.Width * FrameSizeBack) - FrameSizeBack * 4), New Vector2(TextOffset, CInt(Me._positionY + FrameSizeBack)), Color.Black)
            End If
        End If
    End Sub

End Class