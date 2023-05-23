Imports P3D.Screens.UI

Namespace VoltorbFlip
    Public Class VoltorbFlipScreen

        Inherits Screen

        ' Variables & Properties

        Private _screenTransitionY As Single = 0F
        Public Shared _interfaceFade As Single = 0F

        Private MemoWindowX As Single = 0F
        Private MemoWindowSize As New Size(112, 112)

        Private Shared ReadOnly GameSize As New Size(512, 512)
        Public Shared ReadOnly BoardSize As New Size(384, 384)
        Public Shared ReadOnly TileSize As New Size(64, 64)
        Private Shared ReadOnly GridSize As Integer = 5

        Public Shared GameOrigin As New Vector2(CInt(windowSize.Width - GameSize.Width / 2), CInt(windowSize.Height / 2 - GameSize.Height / 2))
        Public Shared BoardOrigin As New Vector2(GameOrigin.X + 32, GameOrigin.Y + 96)

        Private BoardCursorPosition As New Vector2(0, 0)
        Private BoardCursorDestination As New Vector2(0, 0)

        Private MemoIndex As Integer = 0

        Public Shared GameState As States = States.Opening

        Public Property PreviousLevel As Integer = 1
        Public Property CurrentLevel As Integer = 1

        Public Shared ReadOnly MinLevel As Integer = 1
        Public Shared ReadOnly MaxLevel As Integer = 8
        Public Shared Property CurrentCoins As Integer = 0
        Public Property TotalCoins As Integer = 0
        Public Property MaxCoins As Integer = 1

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

            Board = CreateBoard(1)

            _preScreenTarget = New RenderTarget2D(GraphicsDevice, windowSize.Width, windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
            _blur = New Resources.Blur.BlurHandler(windowSize.Width, windowSize.Height)

            Identification = Identifications.VoltorbFlipScreen
            PreScreen = currentScreen
            IsDrawingGradients = True

            Me.MouseVisible = True
            Me.CanChat = Me.PreScreen.CanChat
            Me.CanBePaused = Me.PreScreen.CanBePaused

        End Sub


        Public Overrides Sub Draw()
            If _blurScreens.Contains(PreScreen.Identification) Then
                DrawPrescreen()
            Else
                PreScreen.Draw()
            End If

            DrawGradients(CInt(255 * _interfaceFade))

            DrawBackground()

            DrawMemoMenuAndButton()

            DrawBoard()
            DrawCursor()

            TextBox.Draw()
            ChooseBox.Draw()
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

            Canvas.DrawImageBorder(TextureManager.GetTexture("Textures\VoltorbFlip\Background"), 2, New Rectangle(CInt(GameOrigin.X), CInt(GameOrigin.Y - _screenTransitionY), CInt(GameSize.Width), CInt(GameSize.Height)), mainBackgroundColor, True)

        End Sub

        Private Sub DrawBoard()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            SpriteBatch.Draw(TextureManager.GetTexture("Textures\VoltorbFlip\Board"), New Rectangle(CInt(BoardOrigin.X), CInt(BoardOrigin.Y), BoardSize.Width, BoardSize.Height), mainBackgroundColor)

            DrawTiles()
        End Sub

        Private Sub DrawTiles()
            For _row = 0 To GridSize - 1
                For _column = 0 To GridSize - 1
                    Dim _tile As Tile = Board(_row)(_column)
                    _tile.Draw()
                Next
            Next
        End Sub

        Private Sub DrawMemoMenuAndButton()
            Dim mainBackgroundColor As Color = New Color(255, 255, 255)
            If GameState = States.Closing Or GameState = States.Opening Then
                mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
            End If

            'Draw Button
            Dim ButtonOriginX As Integer = CInt(BoardOrigin.X + BoardSize.Width + MemoWindowSize.Width)
            SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Button", New Rectangle(0, 0, 56, 56)), New Rectangle(ButtonOriginX, CInt(BoardOrigin.Y), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)

            Dim ButtonTextTop As String = "Open"
            Dim ButtonTextBottom As String = "Memos"

            If GameState = States.Memo Then
                ButtonTextTop = "Close"
            End If

            SpriteBatch.DrawString(FontManager.MainFont, ButtonTextTop, New Vector2(CInt(ButtonOriginX + MemoWindowSize.Width / 2 - FontManager.MainFont.MeasureString(ButtonTextTop).X / 2), CInt(BoardOrigin.Y + 22)), mainBackgroundColor)
            SpriteBatch.DrawString(FontManager.MainFont, ButtonTextBottom, New Vector2(CInt(ButtonOriginX + MemoWindowSize.Width / 2 - FontManager.MainFont.MeasureString(ButtonTextBottom).X / 2), CInt(BoardOrigin.Y + 22 + FontManager.MainFont.MeasureString(ButtonTextTop).Y)), mainBackgroundColor)

            'Draw Memo Menu
            If MemoWindowX > 0 Then
                Dim CurrentTile As Tile = Board(CInt(GetCurrentTile.X))(CInt(GetCurrentTile.Y))

                'Draw Background
                SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Background", New Rectangle(0, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoWindowSize.Width + MemoWindowX), CInt(BoardOrigin.Y + MemoWindowSize.Height + 32), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)

                If GameState = States.Memo Then
                    'Draw lit up Memos in the Memo menu when it's enabled on a tile
                    If CurrentTile.GetMemo(0) = True Then 'Voltorb
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(0, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoWindowSize.Width + MemoWindowX), CInt(BoardOrigin.Y + MemoWindowSize.Height + 32), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)
                    End If
                    If CurrentTile.GetMemo(1) = True Then 'x1
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(56, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoWindowSize.Width + MemoWindowX), CInt(BoardOrigin.Y + MemoWindowSize.Height + 32), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)
                    End If
                    If CurrentTile.GetMemo(2) = True Then 'x2
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(56 + 56, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoWindowSize.Width + MemoWindowX), CInt(BoardOrigin.Y + MemoWindowSize.Height + 32), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)
                    End If
                    If CurrentTile.GetMemo(3) = True Then 'x3
                        SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Enabled", New Rectangle(56 + 56 + 56, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoWindowSize.Width + MemoWindowX), CInt(BoardOrigin.Y + MemoWindowSize.Height + 32), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)
                    End If

                    'Draw indicator of currently selected Memo
                    SpriteBatch.Draw(TextureManager.GetTexture("VoltorbFlip\Memo_Index", New Rectangle(56 * MemoIndex, 0, 56, 56)), New Rectangle(CInt(BoardOrigin.X + BoardSize.Width - MemoWindowSize.Width + MemoWindowX), CInt(BoardOrigin.Y + MemoWindowSize.Height + 32), MemoWindowSize.Width, MemoWindowSize.Height), mainBackgroundColor)
                End If
            End If

        End Sub
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

        Private Function CreateBoard(ByVal Level As Integer) As List(Of List(Of Tile))

            Dim Board As List(Of List(Of Tile)) = CreateGrid()
            Dim Data As List(Of Integer) = GetLevelData(Level)
            Dim Spots As List(Of List(Of Integer)) = New List(Of List(Of Integer))

            For i = 0 To Data(0) + Data(1) + Data(2)
                Dim SpotList As List(Of Integer) = New List(Of Integer)
                Dim ValueX As Integer = Random.Next(0, 5)
                Dim ValueY As Integer = Random.Next(0, 5)
                SpotList.AddRange({ValueX, ValueY})
                If Spots.Count > 0 Then
                    Dim AddList As Boolean = True
                    For SpotIndex = 0 To Spots.Count - 1
                        If Spots(SpotIndex)(0) = ValueX AndAlso Spots(SpotIndex)(1) = ValueY Then
                            AddList = False
                        End If
                    Next
                    If AddList = True Then
                        Spots.Add(SpotList)
                    End If
                Else
                    Spots.Add(SpotList)
                End If
            Next

            Dim a = 0
            While a < Data(0)
                Dim TileX As Integer = Spots(a)(0)
                Dim TileY As Integer = Spots(a)(1)

                Board(TileX)(TileY).Value = Tile.Values.Two
                a += 1
            End While

            While a < Data(0) + Data(1)
                Dim TileX As Integer = Spots(a)(0)
                Dim TileY As Integer = Spots(a)(1)

                Board(TileX)(TileY).Value = Tile.Values.Three
                a += 1
            End While

            While a < Data(0) + Data(1) + Data(2)
                Dim TileX As Integer = Spots(a)(0)
                Dim TileY As Integer = Spots(a)(1)

                Board(TileX)(TileY).Value = Tile.Values.Voltorb
                a += 1
            End While

            MaxCoins = CInt(Math.Pow(2, Data(0)) * Math.Pow(3, Data(1)))

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
            For _row = 1 To VoltorbFlipScreen.GridSize
                Dim Column As New List(Of Tile)
                For _column = 1 To VoltorbFlipScreen.GridSize
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

            For _row = 0 To GridSize - 1
                For _column = 0 To GridSize - 1
                    If Board(_row)(_column).Value = Tile.Values.Voltorb Then
                        If RowBombs(_row) = Nothing Then
                            RowBombs.Add(1)
                        Else
                            RowBombs(_row) += 1
                        End If
                    Else
                        If RowSums(_row) = Nothing Then
                            RowSums.Add(Board(_row)(_column).Value)
                        Else
                            RowSums(_row) += Board(_row)(_column).Value
                        End If
                    End If
                Next
            Next

            For _column = 0 To GridSize - 1
                For _row = 0 To GridSize - 1
                    If Board(_row)(_column).Value = Tile.Values.Voltorb Then
                        If ColumnBombs(_column) = Nothing Then
                            ColumnBombs.Add(1)
                        Else
                            ColumnBombs(_column) += 1
                        End If
                    Else
                        If ColumnSums(_column) = Nothing Then
                            ColumnSums.Add(Board(_row)(_column).Value)
                        Else
                            ColumnSums(_column) += Board(_row)(_column).Value
                        End If
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

        Public Function GetCursorOffset(ByVal Column As Integer, ByVal Row As Integer) As Vector2
            Dim Offset As Vector2 = New Vector2(Nothing, Nothing)
            If Column = Not Nothing Then
                Offset.X = TileSize.Width * Column
            End If
            If Row = Not Nothing Then
                Offset.Y = TileSize.Height * Row
            End If
            Return Offset
        End Function

        ''' <summary>
        ''' Get the tile that the cursor is on
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCurrentTile() As Vector2
            Return GetCursorOffset(CInt(BoardCursorDestination.X / TileSize.Width), CInt(BoardCursorDestination.Y / TileSize.Height))
        End Function

        Public Function GetTileUnderMouse() As Vector2
            Dim AbsoluteMousePosition As Vector2 = MouseHandler.MousePosition.ToVector2
            Dim RelativeMousePosition As Vector2 = New Vector2(Clamp(AbsoluteMousePosition.X - BoardOrigin.X, 0, BoardSize.Width), Clamp(AbsoluteMousePosition.Y - BoardOrigin.Y, 0, BoardSize.Height))
            Return New Vector2(CInt(Math.Floor(RelativeMousePosition.X / TileSize.Width)), CInt(Math.Floor(RelativeMousePosition.Y / TileSize.Height)))
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
            If IsCurrentScreen() And _interfaceFade + 0.01F >= 1.0F Then
                Return FontRenderer
            Else
                Return SpriteBatch
            End If
        End Function

        Public Overrides Sub SizeChanged()
            GameOrigin = New Vector2(CInt(windowSize.Width - GameSize.Width / 2), CInt(windowSize.Height / 2 - GameSize.Height / 2))
            BoardOrigin = New Vector2(GameOrigin.X + 32, GameOrigin.Y + 32)
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
            UpdateTiles()

            If ChooseBox.Showing = False AndAlso TextBox.Showing = False AndAlso GameState = States.Game Or GameState = States.Memo Then

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
                    If BoardCursorDestination.X > GetCursorOffset(Nothing, 0).X Then
                        BoardCursorDestination.X -= GetCursorOffset(Nothing, 1).X
                    Else
                        BoardCursorDestination.X = GetCursorOffset(Nothing, 4).X
                    End If
                End If

                If Controls.Right(True, True, False) = True Then
                    If BoardCursorDestination.X < GetCursorOffset(Nothing, 4).X Then
                        BoardCursorDestination.X += GetCursorOffset(Nothing, 1).X
                    Else
                        BoardCursorDestination.X = GetCursorOffset(Nothing, 0).X
                    End If
                End If

                'Animation of Cursor
                BoardCursorPosition.X = MathHelper.Lerp(BoardCursorDestination.X, BoardCursorPosition.X, 0.8F)
                BoardCursorPosition.Y = MathHelper.Lerp(BoardCursorDestination.Y, BoardCursorPosition.Y, 0.8F)

            Else
                'Reset cursor position between levels
                BoardCursorDestination = GetCursorOffset(0, 0)
                BoardCursorPosition = GetCursorOffset(0, 0)
            End If

            'Switching between Game and Memo GameStates (Keys & GamePad)
            If KeyBoardHandler.KeyPressed(KeyBindings.OpenInventoryKey) Or ControllerHandler.ButtonPressed(Buttons.X) Then
                If GameState = States.Game Then
                    GameState = States.Memo
                ElseIf GameState = States.Memo Then
                    GameState = States.Game
                End If
            End If

            'Switching between Game and Memo GameStates (Mouse)
            Dim ButtonRectangle As Rectangle = New Rectangle(CInt(BoardOrigin.X + BoardSize.Width + MemoWindowSize.Width), CInt(BoardOrigin.Y), MemoWindowSize.Width, MemoWindowSize.Height)
            If Controls.Accept(True, False, False) = True AndAlso MouseHandler.IsInRectangle(ButtonRectangle) Then
                If GameState = States.Game Then
                    GameState = States.Memo
                ElseIf GameState = States.Memo Then
                    GameState = States.Game
                End If
            End If

            If GameState = States.Memo Then
                'Animate opening the Memo window
                If MemoWindowX < MemoWindowSize.Width Then
                    MemoWindowX = MathHelper.Lerp(MemoWindowSize.Width, MemoWindowX, 0.9F)
                    If MemoWindowX >= MemoWindowSize.Width Then
                        MemoWindowX = MemoWindowSize.Width
                    End If
                End If

                'Cycling through the 4 Memo types (Voltorb, One, Two, Three)
                If Controls.Left(True, False, True, False, False, False) = True OrElse ControllerHandler.ButtonPressed(Buttons.LeftShoulder) Then
                    MemoIndex -= 1
                    If MemoIndex < 0 Then
                        MemoIndex = 3
                    End If
                End If
                If Controls.Right(True, False, True, False, False, False) = True OrElse ControllerHandler.ButtonPressed(Buttons.RightShoulder) Then
                    MemoIndex += 1
                    If MemoIndex > 3 Then
                        MemoIndex = 0
                    End If
                End If
            Else
                'Animate Closing the Memo window
                If MemoWindowX > 0F Then
                    MemoWindowX = MathHelper.Lerp(0F, MemoWindowX, 0.9F)
                    If MemoWindowX <= 0F Then
                        MemoWindowX = 0F
                    End If
                End If
            End If

            'Quiting Voltorb Flip
            If Controls.Dismiss And GameState = States.Game Then
                GameState = States.QuitQuestion
                TextBox.Show("Do you want to stop~playing Voltorb Flip?%Yes|No%")
                If ChooseBox.readyForResult = True Then
                    If ChooseBox.result = 0 Then
                        GameState = States.Closing
                    Else
                        GameState = States.Game
                    End If
                End If

            End If

            'Flip currently selected Tile
            If Controls.Accept(False, True, True) And GameState = States.Game Then
                Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).Flip()
            End If

            'Flip the Tile that the mouse is on
            If Controls.Accept(True, False, False) And GameState = States.Game Then
                Dim TileUnderMouse As Vector2 = GetTileUnderMouse()
                BoardCursorDestination = TileUnderMouse
                Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).Flip()
            End If

            'Adding currently selected Memo to currently selected Tile
            If Controls.Accept(False, True, True) And GameState = States.Memo Then
                Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).SetMemo(MemoIndex, True)
            End If

            'Adding currently selected Memo to Tile that the mouse is on
            If Controls.Accept(True, False, False) And GameState = States.Memo Then
                Dim TileUnderMouse As Vector2 = GetTileUnderMouse()
                BoardCursorDestination = TileUnderMouse
                Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).SetMemo(MemoIndex, True)
            End If

            'Removing currently selected Memo from currently selected Tile
            If Controls.Dismiss(False, True, True) And GameState = States.Memo Then
                Board(CInt(GetCurrentTile.Y))(CInt(GetCurrentTile.X)).SetMemo(MemoIndex, False)
            End If

            'Removing currently selected Memo from Tile that the mouse is on
            If Controls.Dismiss(True, False, False) And GameState = States.Memo Then
                Dim TileUnderMouse As Vector2 = GetTileUnderMouse()
                BoardCursorDestination = TileUnderMouse
                Board(CInt(TileUnderMouse.Y))(CInt(TileUnderMouse.X)).SetMemo(MemoIndex, False)
            End If

            'Completed the level
            If GameState = States.GameWon Then
                TextBox.Show("Game clear! You received" & " " & CurrentCoins & " " & "Coins!")

                Dim ResultCoins As Integer = TotalCoins + CurrentCoins
                Dim AnimationTotalCoins As Single = TotalCoins
                Dim AnimationCurrentCoins As Single = CurrentCoins

                While TotalCoins < ResultCoins
                    AnimationTotalCoins += 0.05F
                    If AnimationTotalCoins >= ResultCoins Then
                        AnimationTotalCoins = ResultCoins
                    End If

                    AnimationCurrentCoins -= -0.05F
                    If AnimationCurrentCoins <= 0 Then
                        AnimationCurrentCoins = 0
                    End If

                    TotalCoins = CInt(Math.Floor(AnimationTotalCoins))
                    If TotalCoins > 99999 Then
                        TotalCoins = 99999
                    End If

                    CurrentCoins = CInt(Math.Ceiling(AnimationCurrentCoins))
                End While

                'Flip all Tiles to reveal contents
                For _row = 0 To GridSize
                    For _column = 0 To GridSize
                        Board(_row)(_column).Reveal()
                    Next
                Next
                GameState = States.FlipWon
            End If

            'Revealed a Voltorb
            If GameState = States.GameLost Then
                TextBox.Show("Oh no! You get 0 coins")

                Dim ResultCoins As Integer = 0
                Dim AnimationCurrentCoins As Single = CurrentCoins

                While CurrentCoins > ResultCoins
                    AnimationCurrentCoins -= -0.05F
                    If AnimationCurrentCoins <= 0 Then
                        AnimationCurrentCoins = 0
                    End If

                    CurrentCoins = CInt(Math.Ceiling(AnimationCurrentCoins))
                End While

                'Flip all Tiles to reveal contents
                For _row = 0 To GridSize
                    For _column = 0 To GridSize
                        Board(_row)(_column).Reveal()
                    Next
                Next
                GameState = States.FlipLost

            End If

            'Change Level, reset Tiles
            If GameState = States.FlipWon Then
                If Controls.Accept = True And TextBox.Showing = False Then
                    For _row = 0 To GridSize
                        For _column = 0 To GridSize
                            Board(_row)(_column).Reset()
                        Next
                    Next

                    PreviousLevel = CurrentLevel
                    CurrentLevel += 1

                    If CurrentLevel > MaxLevel Then
                        CurrentLevel = MaxLevel
                    End If

                    GameState = States.NewLevel
                End If
            End If

            'Change Level, reset Tiles
            If GameState = States.FlipLost Then
                If Controls.Accept = True And TextBox.Showing = False Then
                    For _row = 0 To GridSize
                        For _column = 0 To GridSize
                            Board(_row)(_column).Reset()
                        Next
                    Next

                    PreviousLevel = CurrentLevel
                    CurrentLevel -= 1

                    If CurrentLevel < MinLevel Then
                        CurrentLevel = MinLevel
                    End If

                    GameState = States.NewLevel
                End If
            End If

            'Prepare new Level
            If GameState = States.NewLevel Then

                If CurrentLevel < PreviousLevel Then
                    TextBox.Show("Dropped to Game Lv." & " " & CurrentLevel & "!")
                End If

                If CurrentLevel = PreviousLevel Then
                    TextBox.Show("Ready to play Game Lv." & " " & CurrentLevel & "!")
                End If

                If CurrentLevel > PreviousLevel Then
                    TextBox.Show("Advanced to Game Lv." & " " & CurrentLevel & "!")
                End If

                Board = CreateBoard(CurrentLevel)

                If TextBox.Showing = False Then
                    GameState = States.Game
                End If
            End If

            'Animation of opening/closing the window
            If GameState = States.Closing Then
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
                If _screenTransitionY <= 2.0F Then
                    SetScreen(PreScreen)
                End If
            Else

                Dim maxWindowHeight As Integer = CInt(GameSize.Height / 2)
                If _screenTransitionY < maxWindowHeight Then
                    _screenTransitionY = MathHelper.Lerp(maxWindowHeight, _screenTransitionY, 0.8F)
                    If _screenTransitionY >= maxWindowHeight Then
                        _screenTransitionY = maxWindowHeight
                    End If
                End If
                If _interfaceFade < 1.0F Then
                    _interfaceFade = MathHelper.Lerp(1, _interfaceFade, 0.95F)
                    If _interfaceFade > 1.0F Then
                        _interfaceFade = 1.0F
                        If GameState = States.Opening Then
                            GameState = States.Game
                        End If
                    End If
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

        Private Property Activated As Boolean = False
        Private Property FlipProgress As Integer = 0

        Public Sub Flip()
            If Flipped = False Then
                FlipProgress = 3
            End If
        End Sub

        Public Sub Reveal()
            If Flipped = False Then
                FlipProgress = 1
            End If
        End Sub
        Public Sub Reset()
            If Flipped = True Then
                Flipped = False
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
            Dim FlipWidth As Single = 1.0F

            If FlipProgress = 1 OrElse FlipProgress = 3 Then
                If FlipWidth > 0F Then
                    FlipWidth -= 0.05F
                End If
                If FlipWidth <= 0F Then
                    FlipWidth = 0F
                    Flipped = True
                    FlipProgress += 1
                End If
            End If
            If FlipProgress = 2 OrElse FlipProgress = 4 Then
                If FlipWidth < 1.0F Then
                    FlipWidth += 0.05F
                End If
                If FlipWidth >= 1.0F Then
                    FlipWidth = 1.0F
                    FlipProgress = 0
                End If
            End If

            'Draw Tile
            SpriteBatch.Draw(GetImage, New Rectangle(CInt(VoltorbFlipScreen.BoardOrigin.X + TileWidth * Column + (TileWidth - FlipWidth * TileWidth)), CInt(VoltorbFlipScreen.BoardOrigin.Y + TileHeight * Row), CInt(TileWidth * FlipWidth), TileHeight), mainBackgroundColor)

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
                            VoltorbFlipScreen.GameState = VoltorbFlipScreen.States.GameLost
                        Else
                            VoltorbFlipScreen.CurrentCoins *= Me.Value
                        End If
                        Activated = True
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