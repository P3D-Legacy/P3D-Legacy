Namespace BattleSystem

    Public Class SwitchPokemonQueryObject

        Inherits QueryObject

        Dim _ready As Boolean = False

#Region "TextStuff"

        Dim _text As String = ""
        Dim _textColor As Color = Color.White
        Dim _TextReady As Boolean = False

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

        Private Sub TransformText(ByVal text As String)
            Me._text = text

            Me._text = Me._text.Replace("*", " ")
            Me._text = Me._text.Replace("~", " ")
            Me._text = Me._text.Replace("<player.name>", Core.Player.Name)
            Me._text = Me._text.Replace("<playername>", Core.Player.Name)
            Me._text = Me._text.Replace("<rivalname>", Core.Player.RivalName)
            Me._text = Me._text.Replace("[POKE]", "Poké")
        End Sub

        Private Sub UpdateText()
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
                    Me._TextReady = True
                End If
            End If
        End Sub

        Private Sub DrawText(BV2Screen As BattleScreen)
            Dim rec As New Rectangle(100, Core.windowSize.Height - 250, Core.windowSize.Width - 200, 200)

            Canvas.DrawRectangle(rec, New Color(0, 0, 0, 150))

            Dim text As String = Me._text.Substring(0, _textIndex)
            text = text.CropStringToWidth(FontManager.InGameFont, Core.windowSize.Width - 300)

            Core.SpriteBatch.DrawString(FontManager.InGameFont, text, New Vector2(rec.X + 20, rec.Y + 20), Color.White)

            If GamePad.GetState(PlayerIndex.One).IsConnected = True And Core.GameOptions.GamePadEnabled = True And BV2Screen.IsCurrentScreen() = True Then
                Dim d As New Dictionary(Of Buttons, String)
                d.Add(Buttons.A, "OK")
                BV2Screen.DrawGamePadControls(d, New Vector2(rec.X + rec.Width - 100, rec.Y + rec.Height - 40))
            Else
                If TextReady = True Then
                    Core.SpriteBatch.DrawString(FontManager.InGameFont, "OK", New Vector2(rec.X + rec.Width - FontManager.InGameFont.MeasureString("OK").X - 20, rec.Y + rec.Height - FontManager.InGameFont.MeasureString("OK").Y - 5), Color.White)
                End If
            End If
        End Sub

#End Region

#Region "ChooseStuff"

        Dim _chooseIndex As Integer = 0

        Private Sub UpdateChoose()

        End Sub

        Private Sub DrawChoose()
            Dim rec As New Rectangle(Core.windowSize.Width - 250, Core.windowSize.Height - 450, 150, 150)

            Canvas.DrawRectangle(rec, New Color(0, 0, 0, 150))

            If Controls.Down(True, True, True, True, True) = True Then
                _chooseIndex += 1
            End If
            If Controls.Up(True, True, True, True, True) = True Then
                _chooseIndex -= 1
            End If

            _chooseIndex = _chooseIndex.Clamp(0, 1)

            If _chooseIndex = 0 Then
                Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 213, Core.windowSize.Height - 438, 80, 50), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Yes", New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 430), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "No", New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 370), Color.White)
            Else
                Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 213, Core.windowSize.Height - 378, 80, 50), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Yes", New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 430), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "No", New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 370), Color.Black)
            End If

            If Controls.Accept(True, True, True) = True Then
                If _chooseIndex = 0 Then
                    Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf Me.ChoosePokemon, "Choose Pokémon to battle:", False))
                Else
                    _ready = True
                End If
            End If
        End Sub

        Dim TempScreen As BattleScreen

        Private Sub ChoosePokemon(ByVal PokeIndex As Integer)
            TempScreen.Battle.SwitchOutOwn(TempScreen, PokeIndex, insertIndex)
            Me._ready = True
        End Sub

#End Region

        Dim insertIndex As Integer = 0

        Public Sub New(ByVal BattleScreen As BattleScreen, ByVal NewPokemon As Pokemon)
            MyBase.New(QueryTypes.SwitchPokemon)

            Me.insertIndex = BattleScreen.BattleQuery.Count
            Me.TempScreen = BattleScreen
            TransformText(BattleScreen.Trainer.Name & " is about to send out a " & NewPokemon.GetDisplayName() & "! Do you want to switch your Pokémon?")
        End Sub

        Dim delay As Single = 3.0F

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If TextReady = False Then
                UpdateText()
            Else
                delay -= 0.1F
                If delay <= 0.0F Then
                    delay = 0.0F
                End If

                If delay = 0.0F And BV2Screen.IsCurrentScreen() = True Then
                    UpdateChoose()
                End If
            End If
        End Sub

        Public Overrides Sub Draw(BV2Screen As BattleScreen)
            DrawText(BV2Screen)
            If TextReady = True Then
                DrawChoose()
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return Me._ready
            End Get
        End Property

    End Class

End Namespace