Imports P3D.Screens.UI

Namespace VoltorbFlip
    Public Class VoltorbFlipScreen

        Inherits Screen

        ' Variables & Properties

        Private _screenTransitionY As Single = 0F
        Public Shared _interfaceFade As Single = 0F

        Private Delay As Integer = 0
        Private MemoMenuX As Single = 0F
        Private MemoMenuSize As New Size(112, 112)

        Private Shared ReadOnly GameSize As New Size(576, 544)
        Public Shared ReadOnly BoardSize As New Size(384, 384)
        Public Shared ReadOnly TileSize As New Size(64, 64)
        Private Shared ReadOnly GridSize As Integer = 5

        Public Shared GameOrigin As New Vector2(CInt(windowSize.Width / 2 - GameSize.Width / 2 - 32), CInt(windowSize.Height / 2 - GameSize.Height / 2))
        Public Shared BoardOrigin As New Vector2(GameOrigin.X + 32, GameOrigin.Y + 160)
        Public Shared TutorialRectangle As New Rectangle(CInt(windowSize.Width / 2 - 512 / 2), CInt(windowSize.Height / 2 - 384 / 2), 512, 384)

        Private BoardCursorPosition As New Vector2(0, 0)
        Private BoardCursorDestination As New Vector2(0, 0)

        Private NewLevelMenuIndex As Integer = 0

        Private MemoIndex As Integer = 0

        Public Shared GameState As States = States.Opening

        Public Shared Property PreviousLevel As Integer = 1
        Public Shared Property CurrentLevel As Integer = 1

        Public Shared ReadOnly MinLevel As Integer = 1
        Public Shared ReadOnly MaxLevel As Integer = 7
        Public Shared Property CurrentFlips As Integer = 0
        Public Shared Property TotalFlips As Integer = 0

        Public Shared Property CurrentCoins As Integer = 0
        Public Shared Property TotalCoins As Integer = -1
        Public Shared Property ConsecutiveWins As Integer = 0
        Public Shared MaxCoins As Integer = 1

        Public Board As List(Of List(Of Tile))

        Public VoltorbSums As List(Of List(Of Integer))
        Public CoinSums As List(Of List(Of Integer))

        Public Enum States
            Opening
            Closing
            QuitQuestion
            Game
            Memo
            GameWon
            GameLost
            FlipWon
            FlipLost
            NewLevelQuestion
            NewLevel
        End Enum

        'Stuff related to blurred PreScreens
        Private _blur As Resources.Blur.BlurHandler
        Private _preScreenTexture As RenderTarget2D
        Private _preScreenTarget As RenderTarget2D
        Private _blurScreens As Identifications() = {Identifications.BattleScreen,
                                                 Identifications.OverworldScreen,
                                                 Identifications.DirectTradeScreen,
                                                 Identifications.WonderTradeScreen,
                                                 Identifications.GTSSetupScreen,
                                                 Identifications.GTSTradeScreen,
                                                 Identifications.PVPLobbyScreen}

        Public Sub New(ByVal currentScreen As Screen)
            GameState = States.Opening
            GameOrigin = New Vector2(CInt(windowSize.Width / 2 - GameSize.Width / 2 - 32), CInt(windowSize.Height / 2 - _screenTransitionY))
            BoardOrigin = New Vector2(GameOrigin.X + 32, GameOrigin.Y + 160)
            TutorialRectangle = New Rectangle(CInt(windowSize.Width / 2 - 512 / 2), CInt(windowSize.Height / 2 - 384 / 2), 512, 384)

            BoardCursorDestination = GetCursorOffset(0, 0)
            BoardCursorPosition = GetCursorOffset(0, 0)

            Board = CreateBoard(CurrentLevel)
            TotalCoins = 0

            _preScreenTarget = New RenderTarget2D(GraphicsDevice, windowSize.Width, windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
            _blur = New Resources.Blur.BlurHandler(windowSize.Width, windowSize.Height)

            Identification = Identifications.VoltorbFlipScreen
            PreScreen = currentScreen

            Me.MouseVisible = True
            Me.CanChat = Me.PreScreen.CanChat
            Me.CanBePaused = Me.PreScreen.CanBePaused
            ChooseBox.readyForResult = False
            TextBox.ResultFunction = Nothing

        End Sub


        Public Overrides Sub Draw()
            If _blurScreens.Contains(PreScreen.Identification) Then
                DrawPrescreen()
            Else
                PreScreen.Draw()
            End If


            DrawBackground()

            DrawMemoMenuAndButton()

            If Board IsNot Nothing Then
                DrawBoard()
                DrawCursor()
            End If

            DrawHUD()
            DrawQuitButton()

            DrawTutorial()

            ChooseBox.Draw()
            TextBox.Draw()
        End Sub

        Private Sub DrawPrescreen()
            If _preScreenTexture Is Nothing OrElse _preScreenTexture.IsContentLost Then
                SpriteBatch.EndBatch()

                Dim target As RenderTarget2D = _preScreenTarget
                GraphicsDevice.SetRenderTarget(target)
                GraphicsDevice.Clear(BackgroundColor)

                SpriteBatch.BeginBatch()

                PreScreen.Draw()

                SpriteBatch.EndBatch()

                GraphicsDevice.SetRenderTarget(Nothing)

                SpriteBatch.BeginBatch()

                _preScreenTexture = target
            End If

            If _interfaceFade < 1.0F Then
                SpriteBatch.Draw(_preScreenTexture, windowSize, Color.White)
            End If
            SpriteBatch.Draw(_blur.Perform(_preScreenTexture), windowSize, New Color(255, 255, 255, CInt(255 * _interfaceFade * 2).Clamp(0, 255)))
        End Sub

        Private Sub DrawBackground()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            Canvas.DrawImageBorder(TextureManager.GetTexture("Textures\VoltorbFlip\Background"), 2, New Rectangle(CInt(GameOrigin.X), CInt(GameOrigin.Y), CInt(GameSize.Width), CInt(GameSize.Height)), mainBackgroundColor, False)

        End Sub
        Private Sub DrawHUD()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            Dim Fontcolor As Color = New Color(0, 0, 0)
            If GameState = States.Closing Or GameState = States.Opening Then
                Fontcolor = New Color(0, 0, 0, CInt(255 * _interfaceFade))
            End If

            'Level
            Dim LevelText As String = Localization.GetString("VoltorbFlip_LV.", "LV.") & " " & CurrentLevel.ToString
            Canvas.DrawImageBorder(TextureManager.GetTexture("Textures\VoltorbFlip\HUD"), 2, New Rectangle(CInt(GameOrigin.X + 32), CInt(GameOrigin.Y + 32), 96, 96), mainBackgroundColor, False)
            SpriteBatch.DrawString(FontManager.MainFont, LevelText, New Vector2(CInt(GameOrigin.X + 80 + 4 - FontManager.MainFont.MeasureString(LevelText).X / 2), CInt(GameOrigin.Y + 80 + 4 - FontManager.MainFont.MeasureString(LevelText).Y / 2)), Fontcolor)

            'Current Coins
            Canvas.DrawImageBorder(TextureManager.GetTexture("Textures\VoltorbFlip\HUD"), 2, New Rectangle(CInt(GameOrigin.X + 128 + 24), CInt(GameOrigin.Y + 32), 192, 96), mainBackgroundColor, False)

            Dim CurrentCoinsText1 As String = Localization.GetString("VoltorbFlip_CurrentCoins_Line1", "Coins found")
            Dim CurrentCoinsText2 As String = Localization.GetString("VoltorbFlip_CurrentCoins_Line2", "in this LV.")
            Dim CurrentCoinsText3 As String = ""

            CurrentCoinsText3 &= "["
            If CurrentCoins < 10000 Then
                CurrentCoinsText3 &= "0"
            End If
            If CurrentCoins < 1000 Then
                CurrentCoinsText3 &= "0"
            End If
            If CurrentCoins < 100 Then
                CurrentCoinsText3 &= "0"
            End If
            If CurrentCoins < 10 Then
                CurrentCoinsText3 &= "0"
            End If
            CurrentCoinsText3 &= CurrentCoins.ToString & "]"

            SpriteBatch.DrawString(FontManager.MainFont, CurrentCoinsText1, New Vector2(CInt(GameOrigin.X + 232 + 24 - FontManager.MainFont.MeasureString(CurrentCoinsText1).X / 2), CInt(GameOrigin.Y + 80 + 4 - FontManager.MainFont.MeasureString(CurrentCoinsText2).Y / 2 - FontManager.MainFont.MeasureString(CurrentCoinsText1).Y)), Fontcolor)
            SpriteBatch.DrawString(FontManager.MainFont, CurrentCoinsText2, New Vector2(CInt(GameOrigin.X + 232 + 24 - FontManager.MainFont.MeasureString(CurrentCoinsText2).X / 2), CInt(GameOrigin.Y + 80 + 4 - FontManager.MainFont.MeasureString(CurrentCoinsText2).Y / 2)), Fontcolor)
            SpriteBatch.DrawString(FontManager.MainFont, CurrentCoinsText3, New Vector2(CInt(GameOrigin.X + 232 + 24 - FontManager.MainFont.MeasureString(CurrentCoinsText3).X / 2), CInt(GameOrigin.Y + 80 + 4 + FontManager.MainFont.MeasureString(CurrentCoinsText2).Y / 2)), Fontcolor)

            'Total Coins
            Canvas.DrawImageBorder(TextureManager.GetTexture("Textures\VoltorbFlip\HUD"), 2, New Rectangle(CInt(GameOrigin.X + 336 + 32), CInt(GameOrigin.Y + 32), 192, 96), mainBackgroundColor, False)

            Dim TotalCoinsText1 As String = Localization.GetString("VoltorbFlip_TotalCoins_Line1", "<player.name>'s")
            Dim TotalCoinsText2 As String = Localization.GetString("VoltorbFlip_TotalCoins_Line2", "earned Coins")
            Dim TotalCoinsText3 As String = ""

            TotalCoinsText3 &= "["
            If TotalCoins + Core.Player.Coins < 10000 Then
                TotalCoinsText3 &= "0"
            End If
            If TotalCoins < 1000 + Core.Player.Coins Then
                TotalCoinsText3 &= "0"
            End If
            If TotalCoins < 100 + Core.Player.Coins Then
                TotalCoinsText3 &= "0"
            End If
            If TotalCoins < 10 + Core.Player.Coins Then
                TotalCoinsText3 &= "0"
            End If
            TotalCoinsText3 &= CInt(TotalCoins + Core.Player.Coins).ToString & "]"

            SpriteBatch.DrawString(FontManager.MainFont, TotalCoinsText1, New Vector2(CInt(GameOrigin.X + 440 + 32 - FontManager.MainFont.MeasureString(TotalCoinsText1).X / 2), CInt(GameOrigin.Y + 80 + 4 - FontManager.MainFont.MeasureString(TotalCoinsText2).Y / 2 - FontManager.MainFont.MeasureString(TotalCoinsText1).Y)), Fontcolor)
            SpriteBatch.DrawString(FontManager.MainFont, TotalCoinsText2, New Vector2(CInt(GameOrigin.X + 440 + 32 - FontManager.MainFont.MeasureString(TotalCoinsText2).X / 2), CInt(GameOrigin.Y + 80 + 4 - FontManager.MainFont.MeasureString(TotalCoinsText2).Y / 2)), Fontcolor)
            SpriteBatch.DrawString(FontManager.MainFont, TotalCoinsText3, New Vector2(CInt(GameOrigin.X + 440 + 32 - FontManager.MainFont.MeasureString(TotalCoinsText3).X / 2), CInt(GameOrigin.Y + 80 + 4 + FontManager.MainFont.MeasureString(TotalCoinsText2).Y / 2)), Fontcolor)


        End Sub

        Private Sub DrawTutorial()
            Dim MainColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                MainColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            Dim FontColor As Color = New Color(0, 0, 0)
            If GameState = States.Closing Or GameState = States.Opening Then
                FontColor = New Color(0, 0, 0, CInt(255 * _interfaceFade))
            End If

            If GameState = States.NewLevelQuestion Then
                Select Case NewLevelMenuIndex
                    Case 2 'How to Play
                        SpriteBatch.DrawRectangle(New Rectangle(CInt(GameOrigin.X), CInt(GameOrigin.Y), GameSize.Width + 64, GameSize.Height + 32), New Color(0, 0, 0, 128))

                        Dim TutorialString1 As String = Localization.GetString("VoltorbFlip_Tutorial_HowToPlay_Image1", "If you flip the cards in this order, you'll collect: 3 x 1 x 2 x 1 x 3... A total of 18 Coins! And then...").Replace("~", Environment.NewLine)
                        Dim TutorialString2 As String = Localization.GetString("VoltorbFlip_Tutorial_HowToPlay_Image2", "If you select ""Quit"", you'll keep those 18 Coins.").Replace("~", Environment.NewLine)
                        Dim TutorialString3 As String = Localization.GetString("VoltorbFlip_Tutorial_HowToPlay_Image3", "But if you find Voltorb, you'll lose all your Coins!").Replace("~", Environment.NewLine)

                        SpriteBatch.Draw(TextureManager.GetTexture("Textures\VoltorbFlip\Tutorial_HowToPlay"), New Rectangle(CInt(TutorialRectangle.X), CInt(TutorialRectangle.Y), TutorialRectangle.Width, TutorialRectangle.Height), MainColor)

                        SpriteBatch.DrawString(FontManager.MainFont, TutorialString1.CropStringToWidth(FontManager.MainFont, 1, 448), New Vector2(CInt(TutorialRectangle.X + 256 - FontManager.MainFont.MeasureString(TutorialString1.CropStringToWidth(FontManager.MainFont, 1, 448)).X / 2), CInt(TutorialRectangle.Y + 128 - FontManager.MainFont.MeasureString(TutorialString1.CropStringToWidth(FontManager.MainFont, 1, 448)).Y / 2)), FontColor)
                        SpriteBatch.DrawString(FontManager.MainFont, TutorialString2.CropStringToWidth(FontManager.MainFont, 1, 304), New Vector2(CInt(TutorialRectangle.X + 336 - FontManager.MainFont.MeasureString(TutorialString2.CropStringToWidth(FontManager.MainFont, 1, 304)).X / 2), CInt(TutorialRectangle.Y + 256 - FontManager.MainFont.MeasureString(TutorialString2.CropStringToWidth(FontManager.MainFont, 1, 304)).Y / 2)), FontColor)
                        SpriteBatch.DrawString(FontManager.MainFont, TutorialString3.CropStringToWidth(FontManager.MainFont, 1, 304), New Vector2(CInt(TutorialRectangle.X + 336 - FontManager.MainFont.MeasureString(TutorialString3.CropStringToWidth(FontManager.MainFont, 1, 304)).X / 2), CInt(TutorialRectangle.Y + 336 - FontManager.MainFont.MeasureString(TutorialString3.CropStringToWidth(FontManager.MainFont, 1, 304)).Y / 2)), FontColor)

                        Dim QuitButtonText As String = Localization.GetString("VoltorbFlip_QuitButton", "Quit")
                        SpriteBatch.DrawString(FontManager.MainFont, QuitButtonText, New Vector2(CInt(TutorialRectangle.X + 8 + 128 / 2 - FontManager.MainFont.MeasureString(QuitButtonText).X / 2), CInt(TutorialRectangle.Y + 228 + 56 / 2 - FontManager.MainFont.MeasureString(QuitButtonText).Y / 2)), FontColor)
                        SpriteBatch.DrawString(FontManager.MainFont, QuitButtonText, New Vector2(CInt(TutorialRectangle.X + 8 + 128 / 2 - FontManager.MainFont.MeasureString(QuitButtonText).X / 2 - 2), CInt(TutorialRectangle.Y + 228 + 56 / 2 - FontManager.MainFont.MeasureString(QuitButtonText).Y / 2 - 2)), MainColor)

                    Case 3 'Hint
                        SpriteBatch.DrawRectangle(New Rectangle(CInt(GameOrigin.X), CInt(GameOrigin.Y), GameSize.Width + 64, GameSize.Height + 32), New Color(0, 0, 0, 128))

                        Dim TutorialString As String = Localization.GetString("VoltorbFlip_Tutorial_Hint_Image", "By looking at the numbers on the sides of the cards, you can see the hidden number and Voltorb totals.").Replace("~", Environment.NewLine)
                        SpriteBatch.Draw(TextureManager.GetTexture("Textures\VoltorbFlip\Tutorial_Hint"), New Rectangle(CInt(TutorialRectangle.X), CInt(TutorialRectangle.Y), TutorialRectangle.Width, TutorialRectangle.Height), MainColor)

                        SpriteBatch.DrawString(FontManager.MainFont, TutorialString.CropStringToWidth(FontManager.MainFont, 1, 448), New Vector2(CInt(TutorialRectangle.X + 256 - FontManager.MainFont.MeasureString(TutorialString.CropStringToWidth(FontManager.MainFont, 1, 448)).X / 2), CInt(TutorialRectangle.Y + 320 - FontManager.MainFont.MeasureString(TutorialString.CropStringToWidth(FontManager.MainFont, 1, 448)).Y / 2)), FontColor)

                    Case 4 'About Memos
                        SpriteBatch.DrawRectangle(New Rectangle(CInt(GameOrigin.X), CInt(GameOrigin.Y), GameSize.Width + 64, GameSize.Height + 32), New Color(0, 0, 0, 128))

                        Dim TutorialString As String = Localization.GetString("VoltorbFlip_Tutorial_AboutMemos_Image", "Select ""Open Memo"" to open the Memo Window. Select the cards and press [<system.button(enter1)>] to add and [<system.button(back1)>] to remove marks.").Replace("~", Environment.NewLine)
                        Dim ButtonTextTop As String = Localization.GetString("VoltorbFlip_MemoButton_Open_Line1", "Open")
                        Dim ButtonTextBottom As String = Localization.GetString("VoltorbFlip_MemoButton_Open_Line2", "Memos")

                        SpriteBatch.Draw(TextureManager.GetTexture("Textures\VoltorbFlip\Tutorial_AboutMemos"), New Rectangle(CInt(TutorialRectangle.X), CInt(TutorialRectangle.Y), TutorialRectangle.Width, TutorialRectangle.Height), MainColor)

                        SpriteBatch.DrawString(FontManager.MainFont, TutorialString.CropStringToWidth(FontManager.MainFont, 1, 448), New Vector2(CInt(TutorialRectangle.X + 256 - FontManager.MainFont.MeasureString(TutorialString.CropStringToWidth(FontManager.MainFont, 1, 448)).X / 2), CInt(TutorialRectangle.Y + 304 - FontManager.MainFont.MeasureString(TutorialString.CropStringToWidth(FontManager.MainFont, 1, 448)).Y / 2)), FontColor)

                        SpriteBatch.DrawString(FontManager.MainFont, ButtonTextTop, New Vector2(CInt(TutorialRectangle.X + 64 + MemoMenuSize.Width / 2 - FontManager.MainFont.MeasureString(ButtonTextTop).X / 2), CInt(TutorialRectangle.Y + 104)), FontColor)
                        SpriteBatch.DrawString(FontManager.MainFont, ButtonTextBottom, New Vector2(CInt(TutorialRectangle.X + 64 + MemoMenuSize.Width / 2 - FontManager.MainFont.MeasureString(ButtonTextBottom).X / 2), CInt(TutorialRectangle.Y + 104 + FontManager.MainFont.MeasureString(ButtonTextTop).Y)), FontColor)

                End Select

            End If

        End Sub

        Private Sub DrawBoard()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            SpriteBatch.Draw(TextureManager.GetTexture("Textures\VoltorbFlip\Board"), New Rectangle(CInt(BoardOrigin.X), CInt(BoardOrigin.Y), BoardSize.Width, BoardSize.Height), mainBackgroundColor)

            DrawTiles()

            DrawSums()

        End Sub

        Private Sub DrawTiles()
            For _row = 0 To GridSize - 1
                For _column = 0 To GridSize - 1
                    Dim _tile As Tile = Board(_row)(_column)
                    _tile.Draw()
                Next
            Next
        End Sub

        Private Sub DrawSums()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            'Draw Rows
            'Coins
            For RowIndex = 0 To GridSize - 1
                Dim CoinSumString As String = "00"
                If GameState = States.Game Or GameState = States.Memo Or GameState = States.QuitQuestion Then
                    Dim CoinSumInteger As Integer = CoinSums(0)(RowIndex)
                    If CoinSumInteger < 10 Then
                        CoinSumString = "0" & CoinSumInteger.ToString
                    Else
                        CoinSumString = CoinSumInteger.ToString
                    End If
                End If
                SpriteBatch.DrawString(FontManager.VoltorbFlipFont, CoinSumString, New Vector2(CInt(BoardOrigin.X + TileSize.Width * (GridSize + 1) - 8 - FontManager.VoltorbFlipFont.MeasureString(CoinSumString).X), BoardOrigin.Y + TileSize.Height * RowIndex + 8), mainBackgroundColor)
            Next
            'Voltorbs
            For RowIndex = 0 To GridSize - 1
                Dim VoltorbSumString As String = "0"
                If GameState = States.Game Or GameState = States.Memo Or GameState = States.QuitQuestion Then
                    VoltorbSumString = VoltorbSums(0)(RowIndex).ToString
                End If
                SpriteBatch.DrawString(FontManager.VoltorbFlipFont, VoltorbSumString, New Vector2(CInt(BoardOrigin.X + TileSize.Width * (GridSize + 1) - 8 - FontManager.VoltorbFlipFont.MeasureString(VoltorbSumString).X), BoardOrigin.Y + TileSize.Height * RowIndex + 34), mainBackgroundColor)
            Next

            'Draw Columns
            'Coins
            For ColumnIndex = 0 To GridSize - 1
                Dim CoinSumString As String = "00"
                If GameState = States.Game Or GameState = States.Memo Or GameState = States.QuitQuestion Then
                    Dim CoinSumInteger As Integer = CoinSums(1)(ColumnIndex)
                    If CoinSumInteger < 10 Then
                        CoinSumString = "0" & CoinSumInteger.ToString
                    Else
                        CoinSumString = CoinSumInteger.ToString
                    End If
                End If
                SpriteBatch.DrawString(FontManager.VoltorbFlipFont, CoinSumString, New Vector2(CInt(BoardOrigin.X + TileSize.Width * ColumnIndex + TileSize.Width - 8 - FontManager.VoltorbFlipFont.MeasureString(CoinSumString).X), BoardOrigin.Y + TileSize.Height * GridSize + 8), mainBackgroundColor)
            Next
            'Voltorbs
            For ColumnIndex = 0 To GridSize - 1
                Dim VoltorbSumString As String = "0"
                If GameState = States.Game Or GameState = States.Memo Or GameState = States.QuitQuestion Then
                    VoltorbSumString = VoltorbSums(1)(ColumnIndex).ToString
                End If
                SpriteBatch.DrawString(FontManager.VoltorbFlipFont, VoltorbSumString, New Vector2(CInt(BoardOrigin.X + TileSize.Width * ColumnIndex + TileSize.Width - 8 - FontManager.VoltorbFlipFont.MeasureString(VoltorbSumString).X), BoardOrigin.Y + TileSize.Height * GridSize + 34), mainBackgroundColor)
            Next

        End Sub

        Private Sub DrawMemoMenuAndButton()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If
            Dim Fontcolor As Color = New Color(0, 0, 0)
            If GameState = States.Closing Or GameState = States.Opening Then
                Fontcolor = New Color(0, 0, 0, CInt(255 * _interfaceFade))
            End If
            'Draw Button
            Dim ButtonOriginX As Integer = CInt(BoardOrigin.X + BoardSize.Width + TileSize.Width / 4)
            SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Button", New Rectangle(0, 0, 56, 56)), New Rectangle(ButtonOriginX, CInt(BoardOrigin.Y), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)

            Dim ButtonTextTop As String = Localization.GetString("VoltorbFlip_MemoButton_Open_Line1", "Open")
            Dim ButtonTextBottom As String = Localization.GetString("VoltorbFlip_MemoButton_Open_Line2", "Memos")

            If GameState = States.Memo Then
                ButtonTextTop = Localization.GetString("VoltorbFlip_MemoButton_Close_Line1", "Close")
                ButtonTextBottom = Localization.GetString("VoltorbFlip_MemoButton_Close_Line2", "Memos")
            End If

            SpriteBatch.DrawString(FontManager.MainFont, ButtonTextTop, New Vector2(CInt(ButtonOriginX + MemoMenuSize.Width / 2 - FontManager.MainFont.MeasureString(ButtonTextTop).X / 2), CInt(BoardOrigin.Y + 40)), Fontcolor)
            SpriteBatch.DrawString(FontManager.MainFont, ButtonTextBottom, New Vector2(CInt(ButtonOriginX + MemoMenuSize.Width / 2 - FontManager.MainFont.MeasureString(ButtonTextBottom).X / 2), CInt(BoardOrigin.Y + 40 + FontManager.MainFont.MeasureString(ButtonTextTop).Y)), Fontcolor)

            'Draw Memo Menu
            If MemoMenuX > 0 Then

                Dim CurrentTile As Tile = Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X))

                'Draw Background
                SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Background", New Rectangle(0, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)

                If GameState = States.Memo Then
                    'Draw lit up Memos in the Memo menu when it's enabled on a tile
                    If CurrentTile.GetMemo(0) = True Then 'Voltorb
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(0, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)
                    End If
                    If CurrentTile.GetMemo(1) = True Then 'x1
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(56, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)
                    End If
                    If CurrentTile.GetMemo(2) = True Then 'x2
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(56 + 56, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)
                    End If
                    If CurrentTile.GetMemo(3) = True Then 'x3
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(56 + 56 + 56, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)
                    End If

                    'Draw indicator of currently selected Memo
                    SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Index", New Rectangle(56 * MemoIndex, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height), mainBackgroundColor)
                End If
            End If

            End Subqu
        Private Sub DrawCursor()
            If GameState = States.Game OrElse GameState = States.Memo Then
                Dim mainBackgroundColor As Color = New Color(255, 255, 255)
                If GameState = States.Closing Or GameState = States.Opening Then
                    mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
                End If

                Dim CursorImage As Texture2D = TextureManager.GetTexture("Textures\VoltorbFlip\Cursor_Game")
                If GameState = States.Memo Then
                    CursorImage = TextureManager.GetTexture("Textures\VoltorbFlip\Cursor_Memo")
                End If

                SpriteBatch.Draw(CursorImage, New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + BoardCursorPosition.X), CInt(VoltorbFlipScreen.BoardOrigin.Y + BoardCursorPosition.Y), TileSize.Width, TileSize.Height), mainBackgroundColor)
            End If
        End Sub
        Private Sub DrawQuitButton()
            Dim mainColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            Dim ShadowColor As Color = New Color(0, 0, 0)
            If GameState = States.Closing Or GameState = States.Opening Then
                ShadowColor = New Color(0, 0, 0, CInt(255 * _interfaceFade))
            End If

            Dim QuitButtonRectangle As New Rectangle(CInt(GameOrigin.X + 424), CInt(GameOrigin.Y + 448), 128, 56)
            SpriteBatch.Draw(TextureManager.GetTexture("Textures\VoltorbFlip\Quit_Button"), QuitButtonRectangle, mainColor)

            Dim QuitButtonText As String = Localization.GetString("VoltorbFlip_QuitButton", "Quit")
            SpriteBatch.DrawString(FontManager.MainFont, QuitButtonText, New Vector2(CInt(QuitButtonRectangle.X + QuitButtonRectangle.Width / 2 - FontManager.MainFont.MeasureString(QuitButtonText).X / 2), CInt(QuitButtonRectangle.Y + QuitButtonRectangle.Height / 2 - FontManager.MainFont.MeasureString(QuitButtonText).Y / 2)), ShadowColor)
            SpriteBatch.DrawString(FontManager.MainFont, QuitButtonText, New Vector2(CInt(QuitButtonRectangle.X + QuitButtonRectangle.Width / 2 - FontManager.MainFont.MeasureString(QuitButtonText).X / 2 - 2), CInt(QuitButtonRectangle.Y + QuitButtonRectangle.Height / 2 - FontManager.MainFont.MeasureString(QuitButtonText).Y / 2 - 2)), mainColor)

        End Sub

        Private Function CreateBoard(ByVal Level As Integer) As List(Of List(Of Tile))

            Dim Board As List(Of List(Of Tile)) = CreateGrid()
            Dim Data As List(Of Integer) = GetLevelData(Level)
            Dim Spots As List(Of List(Of Integer)) = New List(Of List(Of Integer))

            For i = 0 To Data(0) + Data(1) + Data(2) - 1
                If Spots.Count > 0 Then
                    Dim ValueX As Integer = Random.Next(0, 5)
                    Dim ValueY As Integer = Random.Next(0, 5)
TryAgain:
                    Dim IsUnique As Boolean = True
                    For SpotIndex = 0 To Spots.Count - 1
                        If Spots(SpotIndex)(0) = ValueX AndAlso Spots(SpotIndex)(1) = ValueY Then
                            IsUnique = False
                            Exit For
                        End If
                    Next

                    If IsUnique = False Then
                        ValueX = Random.Next(0, 5)
                        ValueY = Random.Next(0, 5)
                        GoTo TryAgain
                    Else
                        Spots.Add(New List(Of Integer)({ValueX, ValueY}.ToList))
                    End If
                Else
                    Spots.Add(New List(Of Integer)({Random.Next(0, 5), Random.Next(0, 5)}.ToList))
                End If
            Next

            If Data(0) > 0 Then
                For a = 0 To Data(0) - 1
                    Dim TileX As Integer = Spots(a)(0)
                    Dim TileY As Integer = Spots(a)(1)
                    Board(TileY)(TileX).Value = Tile.Values.Two
                Next
            End If

            If Data(1) > 0 Then
                For b = 0 To Data(1) - 1
                    Dim TileX As Integer = Spots(b + Data(0))(0)
                    Dim TileY As Integer = Spots(b + Data(0))(1)

                    Board(TileY)(TileX).Value = Tile.Values.Three
                Next
            End If

            If Data(2) > 0 Then
                For c = 0 To Data(2) - 1
                    Dim TileX As Integer = Spots(c + Data(0) + Data(1))(0)
                    Dim TileY As Integer = Spots(c + Data(0) + Data(1))(1)

                    Board(TileY)(TileX).Value = Tile.Values.Voltorb
                Next
            End If

            If Data(0) > 0 AndAlso Data(1) > 0 Then
                MaxCoins = CInt(Math.Pow(2, Data(0)) * Math.Pow(3, Data(1)))
            End If
            If Data(0) > 0 AndAlso Data(1) = 0 Then
                MaxCoins = CInt(Math.Pow(2, Data(0)))
            End If
            If Data(0) = 0 AndAlso Data(1) > 0 Then
                MaxCoins = CInt(Math.Pow(3, Data(1)))
            End If

            VoltorbSums = GenerateSums(Board, True)
            CoinSums = GenerateSums(Board, False)

            Return Board

        End Function

        ''' <summary>
        ''' Returns an empty grid of Tiles
        ''' </summary>
        ''' <returns></returns>
        Private Function CreateGrid() As List(Of List(Of Tile))
            Dim Grid As New List(Of List(Of Tile))
            For _row = 0 To VoltorbFlipScreen.GridSize - 1
                Dim Column As New List(Of Tile)
                For _column = 0 To VoltorbFlipScreen.GridSize - 1
                    Column.Add(New VoltorbFlip.Tile(_row, _column, VoltorbFlip.Tile.Values.One, False))
                Next
                Grid.Add(Column)
            Next
            Return Grid
        End Function

        ''' <summary>
        ''' Returns amount of either Coins or Voltorbs in each row and column of a grid of Tiles
        ''' </summary>
        ''' <param name="Board"></param> A grid of Tiles
        ''' <param name="CoinsOrVoltorbs"></param> True returns amount of Voltorbs, False returns amount of Coins
        ''' <returns></returns>
        Private Function GenerateSums(ByVal Board As List(Of List(Of Tile)), ByVal CoinsOrVoltorbs As Boolean) As List(Of List(Of Integer))
            Dim RowSums As New List(Of Integer)
            Dim ColumnSums As New List(Of Integer)
            Dim RowBombs As New List(Of Integer)
            Dim ColumnBombs As New List(Of Integer)

            RowSums.AddRange({0, 0, 0, 0, 0}.ToList)
            ColumnSums.AddRange({0, 0, 0, 0, 0}.ToList)

            RowBombs.AddRange({0, 0, 0, 0, 0}.ToList)
            ColumnBombs.AddRange({0, 0, 0, 0, 0}.ToList)

            'Rows
            For _row = 0 To GridSize - 1
                For _column = 0 To GridSize - 1
                    If Board(_row)(_column).Value = Tile.Values.Voltorb Then
                        RowBombs(_row) += 1
                    Else
                        RowSums(_row) += Board(_row)(_column).Value
                    End If
                Next
            Next

            'Columns
            For _column = 0 To GridSize - 1
                For _row = 0 To GridSize - 1
                    If Board(_row)(_column).Value = Tile.Values.Voltorb Then
                        ColumnBombs(_column) += 1
                    Else
                        ColumnSums(_column) += Board(_row)(_column).Value
                    End If
                Next
            Next

            If CoinsOrVoltorbs = False Then
                Dim Sums As New List(Of List(Of Integer))
                Sums.AddRange({RowSums, ColumnSums})
                Return Sums
            Else
                Dim Voltorbs As New List(Of List(Of Integer))
                Voltorbs.AddRange({RowBombs, ColumnBombs})
                Return Voltorbs
            End If

        End Function

        Public Function GetCursorOffset(Optional ByVal Column As Integer = 0, Optional ByVal Row As Integer = 0) As Vector2
            Return New Vector2(TileSize.Width * Column, TileSize.Height * Row)
        End Function

        ''' <summary>
        ''' Get the tile that the cursor is on
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCurrentTile() As Vector2
            Return New Vector2((BoardCursorDestination.X / TileSize.Width).Clamp(0, GridSize - 1), (BoardCursorDestination.Y / TileSize.Height).Clamp(0, GridSize - 1))
        End Function

        Public Function GetTileUnderMouse() As Vector2
            Dim AbsoluteMousePosition As Vector2 = MouseHandler.MousePosition.ToVector2
            Dim RelativeMousePosition As Vector2 = New Vector2(Clamp(AbsoluteMousePosition.X - BoardOrigin.X, 0, BoardSize.Width), Clamp(AbsoluteMousePosition.Y - BoardOrigin.Y, 0, BoardSize.Height))
            Return New Vector2(CInt(Math.Floor(RelativeMousePosition.X / TileSize.Width).Clamp(0, GridSize - 1)), CInt(Math.Floor(RelativeMousePosition.Y / TileSize.Height).Clamp(0, GridSize - 1)))
        End Function

        Public Function GetLevelData(ByVal LevelNumber As Integer) As List(Of Integer)
            Select Case LevelNumber
                Case 1
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {3, 1, 6}.ToList
                        Case 1
                            Return {0, 3, 6}.ToList
                        Case 2
                            Return {5, 0, 6}.ToList
                        Case 3
                            Return {2, 2, 6}.ToList
                        Case 4
                            Return {4, 1, 6}.ToList
                    End Select
                Case 2
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {1, 3, 7}.ToList
                        Case 1
                            Return {6, 0, 7}.ToList
                        Case 2
                            Return {3, 2, 7}.ToList
                        Case 3
                            Return {0, 4, 7}.ToList
                        Case 4
                            Return {5, 1, 7}.ToList
                    End Select
                Case 3
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {2, 3, 8}.ToList
                        Case 1
                            Return {7, 0, 8}.ToList
                        Case 2
                            Return {4, 2, 8}.ToList
                        Case 3
                            Return {1, 4, 8}.ToList
                        Case 4
                            Return {6, 1, 8}.ToList
                    End Select
                Case 4
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {3, 3, 8}.ToList
                        Case 1
                            Return {0, 5, 8}.ToList
                        Case 2
                            Return {8, 0, 10}.ToList
                        Case 3
                            Return {5, 2, 10}.ToList
                        Case 4
                            Return {2, 4, 10}.ToList
                    End Select
                Case 5
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {7, 1, 10}.ToList
                        Case 1
                            Return {4, 3, 10}.ToList
                        Case 2
                            Return {1, 5, 10}.ToList
                        Case 3
                            Return {9, 0, 10}.ToList
                        Case 4
                            Return {6, 2, 10}.ToList
                    End Select
                Case 6
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {3, 4, 10}.ToList
                        Case 1
                            Return {0, 6, 10}.ToList
                        Case 2
                            Return {8, 1, 10}.ToList
                        Case 3
                            Return {5, 3, 10}.ToList
                        Case 4
                            Return {2, 5, 10}.ToList
                    End Select
                Case 7
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {7, 2, 10}.ToList
                        Case 1
                            Return {4, 4, 10}.ToList
                        Case 2
                            Return {1, 6, 13}.ToList
                        Case 3
                            Return {9, 1, 13}.ToList
                        Case 4
                            Return {6, 3, 10}.ToList
                    End Select
                Case 8
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {0, 7, 10}.ToList
                        Case 1
                            Return {8, 2, 10}.ToList
                        Case 2
                            Return {5, 4, 10}.ToList
                        Case 3
                            Return {2, 6, 10}.ToList
                        Case 4
                            Return {7, 3, 10}.ToList
                    End Select
                Case Else
                    Return Nothing
            End Select

            Return Nothing
        End Function

        Protected Overrides Function GetFontRenderer() As SpriteBatch
            If IsCurrentScreen() AndAlso _interfaceFade + 0.01F >= 1.0F Then
                Return FontRenderer
            Else
                Return SpriteBatch
            End If
        End Function

        Public Overrides Sub SizeChanged()
            GameOrigin = New Vector2(CInt(windowSize.Width / 2 - GameSize.Width / 2 - 32), CInt(windowSize.Height / 2 - _screenTransitionY))
            BoardOrigin = New Vector2(GameOrigin.X + 32, GameOrigin.Y + 160)
            TutorialRectangle = New Rectangle(CInt(windowSize.Width / 2 - 512 / 2), CInt(windowSize.Height / 2 - 384 / 2), 512, 384)
            BoardCursorDestination = GetCursorOffset(0, 0)
            BoardCursorPosition = GetCursorOffset(0, 0)
        End Sub

        Public Sub UpdateTiles()
            For _row = 0 To GridSize - 1
                For _column = 0 To GridSize - 1
                    Dim _tile As Tile = Board(_row)(_column)
                    _tile.Update()
                Next
            Next
        End Sub
        Public Overrides Sub Update()

            ChooseBox.Update()
            If ChooseBox.Showing = False Then
                TextBox.Update()
            End If

            If ChooseBox.Showing = False AndAlso TextBox.Showing = False Then
                If Delay > 0 Then
                    Delay -= 1
                    If Delay <= 0 Then
                        Delay = 0
                    End If
                End If
            End If

            If Board IsNot Nothing Then
                UpdateTiles()
            End If
            If Delay = 0 Then
                If ChooseBox.Showing = False AndAlso TextBox.Showing = False Then
                    If GameState = States.Game Or GameState = States.Memo Then
                        'Moving the cursor between Tiles on the board
                        If Controls.Up(True, True, False) Then
                            If BoardCursorDestination.Y > GetCursorOffset(Nothing, 0).Y Then
                                BoardCursorDestination.Y -= GetCursorOffset(Nothing, 1).Y
                            Else
                                BoardCursorDestination.Y = GetCursorOffset(Nothing, 4).Y
                            End If
                        End If

                        If Controls.Down(True, True, False) = True Then
                            If BoardCursorDestination.Y < GetCursorOffset(Nothing, 4).Y Then
                                BoardCursorDestination.Y += GetCursorOffset(Nothing, 1).Y
                            Else
                                BoardCursorDestination.Y = GetCursorOffset(Nothing, 0).Y
                            End If
                        End If

                        If Controls.Left(True, True, False) = True Then
                            If BoardCursorDestination.X > GetCursorOffset(0, Nothing).X Then
                                BoardCursorDestination.X -= GetCursorOffset(1, Nothing).X
                            Else
                                BoardCursorDestination.X = GetCursorOffset(4, Nothing).X
                            End If
                        End If

                        If Controls.Right(True, True, False) = True Then
                            If BoardCursorDestination.X < GetCursorOffset(4, Nothing).X Then
                                BoardCursorDestination.X += GetCursorOffset(1, Nothing).X
                            Else
                                BoardCursorDestination.X = GetCursorOffset(0, Nothing).X
                            End If
                        End If

                        'Animation of Cursor
                        BoardCursorPosition.X = MathHelper.Lerp(BoardCursorPosition.X, BoardCursorDestination.X, 0.6F)
                        BoardCursorPosition.Y = MathHelper.Lerp(BoardCursorPosition.Y, BoardCursorDestination.Y, 0.6F)

                    Else
                        'Reset cursor position between levels
                        BoardCursorDestination = GetCursorOffset(0, 0)
                        BoardCursorPosition = GetCursorOffset(0, 0)
                    End If

                    'Switching between Game and Memo GameStates (Keys & GamePad)
                    If KeyBoardHandler.KeyPressed(KeyBindings.RunKey) Or ControllerHandler.ButtonPressed(Buttons.X) Then
                        If GameState = States.Game Then
                            GameState = States.Memo
                            SoundManager.PlaySound("select")
                        ElseIf GameState = States.Memo Then
                            GameState = States.Game
                            SoundManager.PlaySound("select")
                        End If
                    End If

                    'Switching between Game and Memo GameStates (Mouse)
                    Dim ButtonRectangle As Rectangle = New Rectangle(CInt(BoardOrigin.X + BoardSize.Width + TileSize.Width / 4), CInt(BoardOrigin.Y), MemoMenuSize.Width, MemoMenuSize.Height)
                    If Controls.Accept(True, False, False) = True AndAlso MouseHandler.IsInRectangle(ButtonRectangle) AndAlso Delay = 0 Then
                        If GameState = States.Game Then
                            GameState = States.Memo
                            SoundManager.PlaySound("select")
                        ElseIf GameState = States.Memo Then
                            GameState = States.Game
                            SoundManager.PlaySound("select")
                        End If
                    End If

                    If GameState = States.Memo Then
                        'Animate opening the Memo window
                        If MemoMenuX < MemoMenuSize.Width + TileSize.Width / 4 Then
                            MemoMenuX = MathHelper.Lerp(CSng(MemoMenuSize.Width + TileSize.Width / 4), MemoMenuX, 0.9F)
                            If MemoMenuX >= MemoMenuSize.Width + TileSize.Width / 4 Then
                                MemoMenuX = CInt(MemoMenuSize.Width + TileSize.Width / 4)
                            End If
                        End If

                        'Cycling through the 4 Memo types (Voltorb, One, Two, Three)
                        If Controls.Left(True, False, True, False, False, False) = True OrElse ControllerHandler.ButtonPressed(Buttons.LeftShoulder) Then
                            MemoIndex -= 1
                            If MemoIndex < 0 Then
                                MemoIndex = 3
                            End If
                            SoundManager.PlaySound("select")
                        End If
                        If Controls.Right(True, False, True, False, False, False) = True OrElse ControllerHandler.ButtonPressed(Buttons.RightShoulder) Then
                            MemoIndex += 1
                            If MemoIndex > 3 Then
                                MemoIndex = 0
                            End If
                            SoundManager.PlaySound("select")
                        End If

                        'Set the Memo type to the one under the mouse
                        Dim MemoMenuRectangle As New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoMenuSize.Width + MemoMenuX), CInt(BoardOrigin.Y + MemoMenuSize.Height + TileSize.Height / 2), MemoMenuSize.Width, MemoMenuSize.Height)
                        If Controls.Accept(True, False, False) = True Then
                            If MouseHandler.IsInRectangle(New Rectangle(MemoMenuRectangle.X, MemoMenuRectangle.Y, CInt(MemoMenuRectangle.Width / 2), CInt(MemoMenuRectangle.Height / 2))) = True Then
                                'Voltorb
                                MemoIndex = 0
                                SoundManager.PlaySound("select")
                            End If
                            If MouseHandler.IsInRectangle(New Rectangle(MemoMenuRectangle.X + CInt(MemoMenuRectangle.Width / 2), MemoMenuRectangle.Y, CInt(MemoMenuRectangle.Width / 2), CInt(MemoMenuRectangle.Height / 2))) = True Then
                                'One
                                MemoIndex = 1
                                SoundManager.PlaySound("select")
                            End If
                            If MouseHandler.IsInRectangle(New Rectangle(MemoMenuRectangle.X, MemoMenuRectangle.Y + CInt(MemoMenuRectangle.Height / 2), CInt(MemoMenuRectangle.Width / 2), CInt(MemoMenuRectangle.Height / 2))) = True Then
                                'Two
                                MemoIndex = 2
                                SoundManager.PlaySound("select")
                            End If
                            If MouseHandler.IsInRectangle(New Rectangle(MemoMenuRectangle.X + CInt(MemoMenuRectangle.Width / 2), MemoMenuRectangle.Y + CInt(MemoMenuRectangle.Height / 2), CInt(MemoMenuRectangle.Width / 2), CInt(MemoMenuRectangle.Height / 2))) = True Then
                                'Three
                                MemoIndex = 3
                                SoundManager.PlaySound("select")
                            End If
                        End If
                    Else
                        'Animate Closing the Memo window
                        If MemoMenuX > 0F Then
                            MemoMenuX = MathHelper.Lerp(0F, MemoMenuX, 0.9F)
                            If MemoMenuX <= 0F Then
                                MemoMenuX = 0F
                            End If
                        End If
                    End If

                    Dim QuitQuestionText As String = Localization.GetString("VoltorbFlip_QuitQuestion_Question_1", "If you quit now, you will~receive") & " " & CurrentCoins.ToString & " " & Localization.GetString("VoltorbFlip_QuitQuestion_Question_2", "Coin(s).*Will you quit?") & "%" & Localization.GetString("VoltorbFlip_QuitQuestion_AnswerYes", "Yes") & "|" & Localization.GetString("VoltorbFlip_QuitQuestion_AnswerNo", "No") & "%"

                    'Quiting Voltorb Flip
                    If Controls.Dismiss(False, True, True) AndAlso (GameState = States.Game OrElse GameState = States.Memo) AndAlso Delay = 0 Then
                        TextBox.Show(QuitQuestionText)
                        SoundManager.PlaySound("select")
                        GameState = States.QuitQuestion
                        MemoMenuX = 0
                    End If

                    'Quiting Voltorb Flip using the mouse
                    Dim QuitButtonRectangle As New Rectangle(CInt(GameOrigin.X + 424), CInt(GameOrigin.Y + 448), 128, 56)
                    If Controls.Accept(True, False, False) AndAlso MouseHandler.IsInRectangle(QuitButtonRectangle) AndAlso GameState = States.Game OrElse GameState = States.Memo) AndAlso Delay = 0 Then
                        TextBox.Show(QuitQuestionText)
                        ChooseBox.CancelIndex = 1
                        SoundManager.PlaySound("select")
                        GameState = States.QuitQuestion
                        MemoMenuX = 0
                    End If


                    If GameState = States.QuitQuestion Then
                        If ChooseBox.readyForResult = True Then
                            If ChooseBox.result = 0 Then
                                Quit()
                                ChooseBox.CancelIndex = -1
                                ChooseBox.readyForResult = False
                            Else
                                Delay = 15
                                GameState = States.Game
                                ChooseBox.readyForResult = False
                            End If
                        End If
                    End If

                    'Flip currently selected Tile
                    If Controls.Accept(False, True, True) AndAlso GameState = States.Game AndAlso Delay = 0 Then
                        Dim CurrentTile As Vector2 = GetCurrentTile()
                        If Board(CInt(CurrentTile.Y))(CInt(CurrentTile.X)).Flipped = False Then
                            SoundManager.PlaySound("select")
                        End If
                        Board(CInt(CurrentTile.Y))(CInt(CurrentTile.X)).Flip()
                    End If

                    'Flip the Tile that the mouse is on
                    If Controls.Accept(True, False, False) AndAlso GameState = States.Game AndAlso MouseHandler.IsInRectangle(New Rectangle(CInt(BoardOrigin.X), CInt(BoardOrigin.Y), BoardSize.Width, BoardSize.Height)) AndAlso Delay = 0 Then
                        Dim TileUnderMouse As Vector2 = GetTileUnderMouse()
                        BoardCursorDestination = GetCursorOffset(CInt(TileUnderMouse.X), CInt(TileUnderMouse.Y))
                        If Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).Flipped = False Then
                            SoundManager.PlaySound("select")
                        End If
                        Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).Flip()

                    End If

                    'Adding currently selected Memo to currently selected Tile
                    If Controls.Accept(False, True, True) AndAlso GameState = States.Memo AndAlso Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).Flipped = False AndAlso Delay = 0 Then
                        If Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).GetMemo(MemoIndex) = False Then
                            SoundManager.PlaySound("select")
                        End If
                        Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).SetMemo(MemoIndex, True)
                    End If

                    'Adding currently selected Memo to Tile that the mouse is on
                    If Controls.Accept(True, False, False) AndAlso GameState = States.Memo AndAlso MouseHandler.IsInRectangle(New Rectangle(CInt(BoardOrigin.X), CInt(BoardOrigin.Y), BoardSize.Width, BoardSize.Height)) AndAlso Delay = 0 Then
                        Dim TileUnderMouse As Vector2 = GetTileUnderMouse()
                        BoardCursorDestination = GetCursorOffset(CInt(TileUnderMouse.X), CInt(TileUnderMouse.Y))
                        If Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).Flipped = False Then
                            If Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).GetMemo(MemoIndex) = False Then
                                SoundManager.PlaySound("select")
                            End If
                            Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).SetMemo(MemoIndex, True)
                        End If
                    End If

                    'Removing currently selected Memo from currently selected Tile
                    If Controls.Dismiss(False, True, True) AndAlso GameState = States.Memo AndAlso Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).Flipped = False AndAlso Delay = 0 Then
                        If Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).GetMemo(MemoIndex) = True Then
                            SoundManager.PlaySound("select")
                        End If
                        Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).SetMemo(MemoIndex, False)
                    End If

                    'Removing currently selected Memo from Tile that the mouse is on
                    If Controls.Dismiss(True, False, False) AndAlso GameState = States.Memo AndAlso MouseHandler.IsInRectangle(New Rectangle(CInt(BoardOrigin.X), CInt(BoardOrigin.Y), BoardSize.Width, BoardSize.Height)) AndAlso Delay = 0 Then
                        Dim TileUnderMouse As Vector2 = GetTileUnderMouse()
                        BoardCursorDestination = GetCursorOffset(CInt(TileUnderMouse.X), CInt(TileUnderMouse.Y))
                        If Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).Flipped = False Then
                            If Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).GetMemo(MemoIndex) = True Then
                                SoundManager.PlaySound("select")
                            End If
                            Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).SetMemo(MemoIndex, False)
                        End If
                    End If
                End If
            End If

            'Level complete!
            If CurrentCoins >= MaxCoins AndAlso GameState = States.Game Then
                Dim GameClearText = Localization.GetString("VoltorbFlip_GameWon_1", "Game clear!~You've found all of the~hidden x2 and x3 cards.*<player.name> received~") & CurrentCoins.ToString & " " & Localization.GetString("VoltorbFlip_GameWon_2", "Coin(s)!")
                SoundManager.PlaySound("VoltorbFlip\WinGame")
                TextBox.Show(GameClearText)
                If Delay = 0 Then
                    PreviousLevel = CurrentLevel

                    If CurrentFlips >= 8 Then
                        TotalFlips += 1
                    End If

                    CurrentFlips = 0
                    ConsecutiveWins += 1

                    If ConsecutiveWins = 5 AndAlso TotalFlips = 5 Then
                        CurrentLevel = MaxLevel + 1
                    Else
                        If CurrentLevel + 1 > MaxLevel Then
                            CurrentLevel = MaxLevel
                        Else
                            CurrentLevel += 1
                        End If
                    End If

                    GameState = States.GameWon
                    Delay = 5
                End If
            End If

            'Completed the level
            If GameState = States.GameWon Then
                If CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) > 0 AndAlso Core.Player.Coins + TotalCoins > CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) Then
                    TotalCoins = CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) - Core.Player.Coins
                    CurrentCoins = 0
                    TextBox.Show(Localization.GetString("VoltorbFlip_MaxCoins", "Your Coin Case can't fit~any more Coin(s)!"))
                Else
                    TotalCoins += CurrentCoins
                    CurrentCoins = 0
                End If

                'Flip all Tiles to reveal contents
                Dim ReadyAmount As Integer = 0
                For _row = 0 To GridSize - 1
                    For _column = 0 To GridSize - 1
                        Board(_row)(_column).Reveal()
                        If Board(_row)(_column).FlipProgress = 0 Then
                            ReadyAmount += 1
                        End If
                    Next
                Next

                If Controls.Accept = True AndAlso ReadyAmount = CInt(GridSize * GridSize) AndAlso TextBox.Showing = False Then
                    If Delay = 0 Then
                        SoundManager.PlaySound("select")
                        Delay = 5
                    End If
                    If Delay > 3 Then
                        If CurrentLevel > PreviousLevel Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_NewLevel_Higher1", "Advanced to Game Lv.") & " " & CurrentLevel & Localization.GetString("VoltorbFlip_NewLevel_Higher2", "!"))
                        End If
                        GameState = States.FlipWon
                    End If
                End If
            End If

            'Revealed a Voltorb
            If GameState = States.GameLost Then
                CurrentCoins = 0

                'Flip all Tiles to reveal contents
                Dim ReadyAmount As Integer = 0
                For _row = 0 To GridSize - 1
                    For _column = 0 To GridSize - 1
                        Board(_row)(_column).Reveal()
                        If Board(_row)(_column).FlipProgress = 0 Then
                            ReadyAmount += 1
                        End If
                    Next
                Next

                If Controls.Accept = True AndAlso ReadyAmount = CInt(GridSize * GridSize) AndAlso TextBox.Showing = False Then
                    PreviousLevel = CurrentLevel
                    If CurrentFlips < CurrentLevel Then
                        CurrentLevel = Math.Max(1, CurrentFlips)
                    End If
                    If Delay = 0 Then
                        SoundManager.PlaySound("select")
                        Delay = 5
                    End If
                    If Delay > 3 Then
                        If CurrentLevel < PreviousLevel Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_NewLevel_Lower1", "Dropped to Game Lv.") & " " & CurrentLevel & Localization.GetString("VoltorbFlip_NewLevel_Lower2", "!"))
                        End If
                        GameState = States.FlipLost
                    End If
                End If
            End If

            'Increase Level, reset Tiles
            If GameState = States.FlipWon Then
                Dim ReadyAmount As Integer = 0
                For _row = 0 To GridSize - 1
                    For _column = 0 To GridSize - 1
                        Board(_row)(_column).Reset()
                        If Board(_row)(_column).FlipProgress = 0 Then
                            ReadyAmount += 1
                        End If
                    Next
                Next

                If ReadyAmount = CInt(GridSize * GridSize) Then
                    GameState = States.NewLevelQuestion
                End If
            End If

            'Drop Level, reset Tiles
            If GameState = States.FlipLost AndAlso TextBox.Showing = False Then
                Dim ReadyAmount As Integer = 0
                For _row = 0 To GridSize - 1
                    For _column = 0 To GridSize - 1
                        Board(_row)(_column).Reset()
                        If Board(_row)(_column).FlipProgress = 0 Then
                            ReadyAmount += 1
                        End If
                    Next
                Next

                CurrentFlips = 0

                If ReadyAmount = CInt(GridSize * GridSize) Then
                    GameState = States.NewLevelQuestion
                End If
            End If

            'The menu that appears before starting a new level
            If GameState = States.NewLevelQuestion Then
                Select Case NewLevelMenuIndex
                    Case 0 'Main Menu
                        If Delay = 0 AndAlso TextBox.Showing = False AndAlso ChooseBox.Showing = False Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_BeforeNewLevel_Main_Question_1", "Play Voltorb Flip Lv.") & " " & CurrentLevel.ToString & Localization.GetString("VoltorbFlip_BeforeNewLevel_Main_Question_2", "?") & "%" & Localization.GetString("VoltorbFlip_BeforeNewLevel_Main_Answer_Play", "Play") & "|" & Localization.GetString("VoltorbFlip_BeforeNewLevel_Main_Answer_GameInfo", "Game Info") & "|" & Localization.GetString("VoltorbFlip_BeforeNewLevel_Main_Answer_Quit", "Quit") & "%")
                            ChooseBox.CancelIndex = 2
                            Delay = 5
                        End If
                        If ChooseBox.readyForResult = True Then
                            Select Case ChooseBox.result
                                Case 0
                                    GameState = States.NewLevel
                                    ChooseBox.readyForResult = False
                                    ChooseBox.CancelIndex = -1
                                Case 1
                                    NewLevelMenuIndex = 1
                                    ChooseBox.readyForResult = False
                                    ChooseBox.CancelIndex = -1
                                Case 2
                                    GameState = States.Closing
                                    ChooseBox.readyForResult = False
                                    ChooseBox.CancelIndex = -1
                            End Select
                        End If
                    Case 1 'Info Menu
                        If Delay = 0 AndAlso TextBox.Showing = False AndAlso ChooseBox.Showing = False Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_BeforeNewLevel_GameInfo_Question", "Which set of info?") & "%" & Localization.GetString("VoltorbFlip_BeforeNewLevel_GameInfo_Answer_HowToPlay", "How to Play") & "|" & Localization.GetString("VoltorbFlip_BeforeNewLevel_GameInfo_Answer_Hint", "Hint!") & "|" & Localization.GetString("VoltorbFlip_BeforeNewLevel_GameInfo_Answer_AboutMemos", "About Memos") & "|" & Localization.GetString("VoltorbFlip_BeforeNewLevel_GameInfo_Back", "Back") & "%")
                            ChooseBox.CancelIndex = 3
                            Delay = 5
                        End If
                        If ChooseBox.readyForResult = True Then
                            Select Case ChooseBox.result
                                Case 0
                                    NewLevelMenuIndex = 2
                                    ChooseBox.CancelIndex = -1
                                Case 1
                                    NewLevelMenuIndex = 3
                                    ChooseBox.CancelIndex = -1
                                Case 2
                                    NewLevelMenuIndex = 4
                                    ChooseBox.CancelIndex = -1
                                Case 3
                                    NewLevelMenuIndex = 0
                                    ChooseBox.CancelIndex = -1
                            End Select
                        End If
                    Case 2 'How to Play
                        If Delay = 0 Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_Tutorial_HowToPlay_Message", "Voltorb Flip is a game in which~you flip over cards to find~numbers hidden beneath them.*The cards are hiding the~numbers 1 through 3...~and Voltorb as well.*The first number you flip over~will give you that many Coins.*From then on, the next number~you find will multiply the~total amount of Coins you've~collected by that number.*If it's a 2, your total will~be multiplied by ""x2"".*If it's a 3, your total will~be multiplied by ""x3"".*But if you flip over a~Voltorb, it's game over.*When that happens, you'll lose~all the Coins you've collected~in the current level.*If you select ""Quit"", you'll~withdraw from the level.*If you get to a difficult~spot, you might want to end~the game early.*Once you've found all the~hidden 2 and 3 cards,~you've cleared the game.*Once you've flipped over~all these cards, then you'll~advance to the next level.*As you move up in levels,~you will be able to receive~more Coins. Do your best!"))
                            Delay = 5
                        End If
                        If TextBox.Showing = False AndAlso Delay > 3 Then
                            ChooseBox.readyForResult = False
                            NewLevelMenuIndex = 1
                        End If
                    Case 3 'Hint!
                        If Delay = 0 Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_Tutorial_Hint_Message", "The numbers at the side~of the board give you a clue~about the numbers hidden on~the backs of the cards.*The larger the number, the~more likely it is that there~are many large numbers hidden~in that row or column.*In the same way, you can tell~how many Voltorb are hidden~in the row or column.*Consider the hidden number~totals and the Voltorb~totals carefully as you~flip over cards."))
                            Delay = 5
                        End If
                        If TextBox.Showing = False AndAlso Delay > 3 Then
                            ChooseBox.readyForResult = False
                            NewLevelMenuIndex = 1
                        End If
                    Case 4 'About Memos
                        If Delay = 0 Then
                            TextBox.Show(Localization.GetString("VoltorbFlip_Tutorial_AboutMemos_Message", "Select ""Open Memo"" or press~[<system.button(run)>] to open the~Memo Window.*You can mark the cards with~the numbers 1 through 3,~but also with a Voltorb mark.*When you have an idea of the~numbers hidden on the back~of the cards, open the Memo~Window, choose the type of~mark you want to use with~the Mouse Wheel or the~Gamepad's Shoulder Buttons~and then press [<system.button(enter1)>]~while highlighting the card~you want to mark.*If you want to remove a mark,~choose the type of mark you~want to remove with the~Mouse Wheel or the Gamepad's~Shoulder Buttons and then~press [<system.button(back1)>] while~highlighting the card you~want to remove the mark from.*You can also use the~mouse to select a~mark type or a card."))
                            Delay = 5
                        End If
                        If TextBox.Showing = False AndAlso Delay > 3 Then
                            ChooseBox.readyForResult = False
                            NewLevelMenuIndex = 1
                        End If
                End Select
            End If
            'Prepare new Level
            If GameState = States.NewLevel Then
                If TextBox.Showing = False Then
                    SoundManager.PlaySound("VoltorbFlip\StartGame")
                    Board = CreateBoard(CurrentLevel)
                    If CurrentLevel = 8 Then
                        TotalFlips = 0
                        ConsecutiveWins = 0
                    End If

                    TextBox.Show(Localization.GetString("VoltorbFlip_NewLevel_Ready1", "Ready to play Game Lv.") & " " & CurrentLevel & Localization.GetString("VoltorbFlip_NewLevel_Ready2", "!"))
                Else
                    Delay = 15
                    GameState = States.Game
                End If
            End If

            'Animation of opening/closing the window
            If GameState = States.Closing Then
                CurrentCoins = 0

                If _interfaceFade > 0F Then
                    _interfaceFade = MathHelper.Lerp(0, _interfaceFade, 0.8F)
                    If _interfaceFade < 0F Then
                        _interfaceFade = 0F
                    End If
                End If
                If _screenTransitionY > 0 Then
                    _screenTransitionY = MathHelper.Lerp(0, _screenTransitionY, 0.8F)
                    If _screenTransitionY <= 0 Then
                        _screenTransitionY = 0
                    End If
                End If

                GameOrigin.Y = CInt(windowSize.Height / 2 - _screenTransitionY)
                BoardOrigin = New Vector2(GameOrigin.X + 32, GameOrigin.Y + 160)

                If _screenTransitionY <= 2.0F Then
                    SetScreen(PreScreen)
                End If
            Else
                Dim maxWindowHeight As Integer = CInt(GameSize.Height / 2)
                If _screenTransitionY < maxWindowHeight Then
                    _screenTransitionY = MathHelper.Lerp(maxWindowHeight, _screenTransitionY, 0.8F)
                    If _screenTransitionY >= maxWindowHeight - 0.8 Then
                        If GameState = States.Opening Then
                            GameState = States.NewLevelQuestion
                        End If
                        _screenTransitionY = maxWindowHeight
                    End If
                End If
                GameOrigin.Y = CInt(windowSize.Height / 2 - _screenTransitionY)
                BoardOrigin = New Vector2(GameOrigin.X + 32, GameOrigin.Y + 160)

                If _interfaceFade < 1.0F Then
                    _interfaceFade = MathHelper.Lerp(1, _interfaceFade, 0.95F)
                    If _interfaceFade = 1.0F Then
                        _interfaceFade = 1.0F
                    End If
                End If
            End If
        End Sub

        Public Sub Quit()
            SoundManager.PlaySound("VoltorbFlip\QuitGame", True)
            TextBox.Show(Localization.GetString("VoltorbFlip_QuitGame_1", "<player.name> received~") & CurrentCoins.ToString & " " & Localization.GetString("VoltorbFlip_QuitGame_2", "Coin(s)!"))

            If CurrentFlips < CurrentLevel Then
                CurrentLevel = Math.Max(1, CurrentFlips)
            End If

            If Delay = 0 Then
                If GameState = States.QuitQuestion OrElse GameState = States.GameWon Then
                    TotalCoins += CurrentCoins
                    CurrentFlips = 0

                    CurrentCoins = 0
                End If
                Delay = 5
            End If
            If Delay > 3 Then
                If CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) > 0 AndAlso Core.Player.Coins + TotalCoins > CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) Then
                    If CurrentLevel < PreviousLevel Then
                        TextBox.Show(Localization.GetString("VoltorbFlip_NewLevel_Lower1", "Dropped to Game Lv.") & " " & CurrentLevel & Localization.GetString("VoltorbFlip_NewLevel_Lower2", "!"))
                    End If
                    GameState = States.Closing
                    ChooseBox.readyForResult = False
                Else
                    If CurrentLevel < PreviousLevel Then
                        TextBox.Show(Localization.GetString("VoltorbFlip_NewLevel_Lower1", "Dropped to Game Lv.") & " " & CurrentLevel & Localization.GetString("VoltorbFlip_NewLevel_Lower2", "!"))
                    End If
                    ChooseBox.readyForResult = False
                    GameState = States.NewLevelQuestion
                End If
            End If
        End Sub
    End Class


    Public Class Tile
        Public Enum Values
            Voltorb
            One
            Two
            Three
        End Enum
        Public Property Row As Integer = 0
        Public Property Column As Integer = 0
        Public Property Value As Integer = Tile.Values.Voltorb
        Public Property Flipped As Boolean = False
        Private Property MemoVoltorb As Boolean = False
        Private Property Memo1 As Boolean = False
        Private Property Memo2 As Boolean = False
        Private Property Memo3 As Boolean = False

        Private Property FlipWidth As Single = 1.0F
        Private Property Activated As Boolean = False
        Public Property FlipProgress As Integer = 0

        Public Sub Flip()
            If Flipped = False Then
                FlipProgress = 3
                If Value <> Values.Voltorb Then
                    VoltorbFlipScreen.CurrentFlips += 1
                End If
            End If
        End Sub

        Public Sub Reveal()
            If Flipped = False Then
                FlipProgress = 1
            End If
        End Sub
        Public Sub Reset()
            If Flipped = True Then
                FlipProgress = 1
                Activated = False
            End If
        End Sub

        Public Sub Draw()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If VoltorbFlipScreen.GameState = VoltorbFlipScreen.States.Closing Or VoltorbFlipScreen.GameState = VoltorbFlipScreen.States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * VoltorbFlipScreen._interfaceFade))
            End If

            Dim TileWidth = VoltorbFlipScreen.TileSize.Width
            Dim TileHeight = VoltorbFlipScreen.TileSize.Height

            If FlipProgress = 1 OrElse FlipProgress = 3 Then
                If FlipWidth > 0F Then
                    FlipWidth -= 0.1F
                End If
                If FlipWidth <= 0F Then
                    FlipWidth = 0F
                    If Flipped = False Then
                        SetMemo(0, False)
                        SetMemo(1, False)
                        SetMemo(2, False)
                        SetMemo(3, False)
                        Flipped = True
                    Else
                        Flipped = False
                    End If
                    FlipProgress += 1
                End If
            End If
            If FlipProgress = 2 OrElse FlipProgress = 4 Then
                If FlipWidth < 1.0F Then
                    FlipWidth += 0.1F
                End If
                If FlipWidth >= 1.0F Then
                    FlipWidth = 1.0F
                    FlipProgress = 0
                End If
            End If

            'Draw Tile
            SpriteBatch.Draw(GetImage, New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + TileWidth * Column + (TileWidth - FlipWidth * TileWidth) / 2), CInt(VoltorbFlipScreen.BoardOrigin.Y + TileHeight * Row), CInt(TileWidth * FlipWidth), TileHeight), mainBackgroundColor)

            'Draw Memos
            If GetMemo(0) = True Then 'Voltorb
                SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Tile_MemoIcons", New Rectangle(0, 0, 32, 32)), New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + TileWidth * Column + (TileWidth - FlipWidth * TileWidth)), CInt(VoltorbFlipScreen.BoardOrigin.Y + TileHeight * Row), CInt(TileWidth * FlipWidth), TileHeight), mainBackgroundColor)
            End If
            If GetMemo(1) = True Then 'x1
                SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Tile_MemoIcons", New Rectangle(32, 0, 32, 32)), New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + TileWidth * Column + (TileWidth - FlipWidth * TileWidth)), CInt(VoltorbFlipScreen.BoardOrigin.Y + TileHeight * Row), CInt(TileWidth * FlipWidth), TileHeight), mainBackgroundColor)
            End If
            If GetMemo(2) = True Then 'x2
                SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Tile_MemoIcons", New Rectangle(32 + 32, 0, 32, 32)), New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + TileWidth * Column + (TileWidth - FlipWidth * TileWidth)), CInt(VoltorbFlipScreen.BoardOrigin.Y + TileHeight * Row), CInt(TileWidth * FlipWidth), TileHeight), mainBackgroundColor)
            End If
            If GetMemo(3) = True Then 'x3
                SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Tile_MemoIcons", New Rectangle(32 + 32 + 32, 0, 32, 32)), New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + TileWidth * Column + (TileWidth - FlipWidth * TileWidth)), CInt(VoltorbFlipScreen.BoardOrigin.Y + TileHeight * Row), CInt(TileWidth * FlipWidth), TileHeight), mainBackgroundColor)
            End If
        End Sub

        Public Sub Update()
            If FlipProgress <= 2 Then
                Activated = False
            Else
                If Flipped = True Then
                    If Activated = False Then
                        If Me.Value = Values.Voltorb Then
                            If VoltorbFlipScreen.GameState = VoltorbFlipScreen.States.Game Then
                                SoundManager.PlaySound("VoltorbFlip\LoseGame", True)
                            End If
                            Screen.TextBox.Show(Localization.GetString("VoltorbFlip_GameLost", "Oh no! You get 0 Coins!"))
                            VoltorbFlipScreen.ConsecutiveWins = 0
                            VoltorbFlipScreen.GameState = VoltorbFlipScreen.States.GameLost
                        Else
                            If VoltorbFlipScreen.CurrentCoins = 0 Then
                                VoltorbFlipScreen.CurrentCoins = Me.Value
                            Else
                                VoltorbFlipScreen.CurrentCoins *= Me.Value
                            End If
                            Activated = True
                        End If
                    End If
                End If
            End If
        End Sub

        Public Function GetImage() As Texture2D
            If Flipped = True Then
                Return TextureManager.GetTexture("VoltorbFlip\Tile_Front", New Rectangle(Value * 32, 0, 32, 32))
            Else
                Return TextureManager.GetTexture("VoltorbFlip\Tile_Back", New Rectangle(0, 0, 32, 32))
            End If
        End Function

        Public Function GetMemo(ByVal MemoNumber As Integer) As Boolean
            Select Case MemoNumber
                Case 0
                    Return MemoVoltorb
                Case 1
                    Return Memo1
                Case 2
                    Return Memo2
                Case 3
                    Return Memo3
                Case Else
                    Return Nothing
            End Select
        End Function

        Public Sub SetMemo(ByVal MemoNumber As Integer, ByVal Value As Boolean)
            Select Case MemoNumber
                Case Tile.Values.Voltorb
                    MemoVoltorb = Value
                Case Tile.Values.One
                    Memo1 = Value
                Case Tile.Values.Two
                    Memo2 = Value
                Case Tile.Values.Three
                    Memo3 = Value
            End Select
        End Sub

        Public Sub New(ByVal Row As Integer, ByVal Column As Integer, ByVal Value As Integer, ByVal Flipped As Boolean)
            Me.Row = Row
            Me.Column = Column
            Me.Value = Value
            Me.Flipped = Flipped
        End Sub

    End Class

End Namespace