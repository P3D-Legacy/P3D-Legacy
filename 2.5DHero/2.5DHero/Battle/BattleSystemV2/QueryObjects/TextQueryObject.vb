Namespace BattleSystem

    Public Class TextQueryObject

        Inherits QueryObject

        Dim _text As String = ""
        Dim _textColor As Color = Color.White
        Dim _ready As Boolean = False

        Dim _textIndex As Integer = 0
        Dim _textDelay As Single = 0.015F

        Private ReadOnly Property TextReady() As Boolean
            Get
                If _textIndex < Me._text.Length Then
                    Return False
                End If
                Return True
            End Get
        End Property

        Public Sub New(ByVal Text As String)
            MyBase.New(QueryTypes.Textbox)

            Me._text = Text

            Me._text = Me._text.Replace("*", " ")
            Me._text = Me._text.Replace("~", " ")
            Me._text = Me._text.Replace("<player.name>", Core.Player.Name)
            Me._text = Me._text.Replace("<playername>", Core.Player.Name)
            Me._text = Me._text.Replace("<rivalname>", Core.Player.RivalName)
            Me._text = Me._text.Replace("[POKE]", "Poké")

            If Me._text = "" Then
                Me._ready = True
            End If
        End Sub

        Public Sub New(ByVal Text As String, ByVal TextColor As Color)
            MyBase.New(QueryTypes.Textbox)

            Me._text = Text
            Me._textColor = TextColor
            If Me._text = "" Then
                Me._ready = True
            End If
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If Me.TextReady = False Then
                Me._textDelay -= 0.01F
                If Me._textDelay <= 0.0F Then
                    Me._textDelay = 0.015F
                    Me._textIndex += 1
                End If
                If Controls.Accept(True, True) = True And Me._textIndex > 2 Then
                    Me._textIndex = Me._text.Length
                End If
            Else
                If Controls.Accept(True, True) = True Then
                    Me._ready = True
                End If
            End If
        End Sub

        Public Overrides Sub Draw(BV2Screen As BattleScreen)
            Dim rec As New Rectangle(100, Core.windowSize.Height - 250, Core.windowSize.Width - 200, 200)

            Canvas.DrawRectangle(rec, New Color(0, 0, 0, 150))

            Dim text As String = Me._text.Substring(0, _textIndex)
            text = text.CropStringToWidth(FontManager.TextFont, 2.0F, Core.windowSize.Width - 300)

            Core.SpriteBatch.DrawString(FontManager.TextFont, text, New Vector2(rec.X + 20, rec.Y + 20), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

            If GamePad.GetState(PlayerIndex.One).IsConnected = True And Core.GameOptions.GamePadEnabled = True And BV2Screen.IsCurrentScreen() = True Then
                Dim d As New Dictionary(Of Buttons, String)
                d.Add(Buttons.A, "OK")
                BV2Screen.DrawGamePadControls(d, New Vector2(rec.X + rec.Width - 100, rec.Y + rec.Height - 40))
            Else
                If TextReady = True Then
                    Core.SpriteBatch.DrawString(FontManager.TextFont, "OK", New Vector2(rec.X + rec.Width - (FontManager.TextFont.MeasureString("OK").X * 2.0F) - 20, rec.Y + rec.Height - (FontManager.TextFont.MeasureString("OK").Y * 2.0F) - 5), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
                End If
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady() As Boolean
            Get
                Return Me._ready
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Dim d() As String = input.Split(CChar("|"))
            Return New TextQueryObject(d(0).Replace("*", vbNewLine), New Color(CInt(d(1)), CInt(d(2)), CInt(d(3))))
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = Me._text.Replace(vbNewLine, "*") & "|" &
                Me._textColor.R & "|" & Me._textColor.G & "|" & Me._textColor.B

            Return "{TEXT|" & s & "}"
        End Function

        Public ReadOnly Property Text() As String
            Get
                Return Me._text
            End Get
        End Property

    End Class

End Namespace