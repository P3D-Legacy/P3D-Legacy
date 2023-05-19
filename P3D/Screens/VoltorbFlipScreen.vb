Imports P3D.Screens.UI

Namespace VoltorbFlip
    Public Class VoltorbFlipScreen

        Inherits Screen

        'Animation:

        Private _enrollY As Single = 0F
        Private _interfaceFade As Single = 0F
        Private BoardCursorPosition As New Vector2(0, 0)
        Private BoardCursorDestination As New Vector2(0, 0)

        Private MemoIndex As Integer = 0

        Public GameState As States = States.Opening
        Public Shared ReadOnly GridSize As Integer = 5
        Public Property PreviousLevel As Integer = 1
        Public Property CurrentLevel As Integer = 1
        Public Property CurrentCoins As Integer = 0
        Public Property TotalCoins As Integer = 0
        Public Property MaxCoins As Integer = 1

        Public Board As List(Of List(Of Tile))
        Private GameSize As New Vector2(512, 512)
        Private TileSize As New Vector2(66, 66)
        Private GameOrigin As New Vector2(CInt(windowSize.Width - GameSize.X / 2), CInt(windowSize.Height / 2 - GameSize.Y / 2))

        Public Enum States
            Opening
            Closing
            Game
            Memo
            GameWon
            GameLost
            NewLevel
        End Enum

        'Stuff related to blurred PreScreens
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

        End Sub


        Private _blur As Resources.Blur.BlurHandler

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
            Dim mainBackgroundColor As Color = New Color(48, 151, 70)
            If GameState = States.Closing Then
                mainBackgroundColor = New Color(48, 151, 70, CInt(255 * _interfaceFade))
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(GameOrigin.X), CInt(GameOrigin.Y - _enrollY), CInt(GameSize.X), CInt(GameSize.Y)), mainBackgroundColor)

        End Sub

        Public Function CreateBoard(ByVal Level As Integer) As List(Of List(Of Tile))

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

            Return Board

        End Function

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

        Public Function GetCursorOffset(ByVal Column As Integer, ByVal Row As Integer) As Vector2
            Dim Offset As Vector2 = New Vector2(Nothing, Nothing)
            If Column = Not Nothing Then
                Offset.X = TileSize.X * Column
            End If
            If Row = Not Nothing Then
                Offset.Y = TileSize.Y * Row
            End If
            Return Offset
        End Function
        Public Function GetCurrentTile() As Vector2
            Return GetCursorOffset(CInt(BoardCursorDestination.X / TileSize.X), CInt(BoardCursorDestination.Y / TileSize.Y))
        End Function
        Public Function GetLevelData(ByVal LevelNumber As Integer) As List(Of Integer)
            Select Case LevelNumber
                Case 1
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {5, 0, 6}.ToList
                        Case 1
                            Return {4, 1, 6}.ToList
                        Case 2
                            Return {3, 1, 6}.ToList
                        Case 3
                            Return {2, 2, 6}.ToList
                        Case 4
                            Return {0, 3, 6}.ToList
                    End Select
                Case 2
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {6, 0, 7}.ToList
                        Case 1
                            Return {5, 1, 7}.ToList
                        Case 2
                            Return {3, 2, 7}.ToList
                        Case 3
                            Return {1, 3, 7}.ToList
                        Case 4
                            Return {0, 4, 7}.ToList
                    End Select
                Case 3
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {7, 0, 8}.ToList
                        Case 1
                            Return {6, 1, 8}.ToList
                        Case 2
                            Return {4, 2, 8}.ToList
                        Case 3
                            Return {2, 3, 8}.ToList
                        Case 4
                            Return {1, 4, 8}.ToList
                    End Select
                Case 4
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {8, 0, 10}.ToList
                        Case 1
                            Return {5, 2, 10}.ToList
                        Case 2
                            Return {3, 3, 8}.ToList
                        Case 3
                            Return {2, 4, 10}.ToList
                        Case 4
                            Return {0, 5, 8}.ToList
                    End Select
                Case 5
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {9, 0, 10}.ToList
                        Case 1
                            Return {7, 1, 10}.ToList
                        Case 2
                            Return {6, 2, 10}.ToList
                        Case 3
                            Return {4, 3, 10}.ToList
                        Case 4
                            Return {1, 5, 10}.ToList
                    End Select
                Case 6
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {8, 1, 10}.ToList
                        Case 1
                            Return {5, 3, 10}.ToList
                        Case 2
                            Return {3, 4, 10}.ToList
                        Case 3
                            Return {2, 5, 10}.ToList
                        Case 4
                            Return {0, 6, 10}.ToList
                    End Select
                Case 7
                    Dim chance As Integer = CInt(Random.Next(0, 5))
                    Select Case chance
                        Case 0
                            Return {9, 1, 13}.ToList
                        Case 1
                            Return {7, 2, 10}.ToList
                        Case 2
                            Return {6, 3, 10}.ToList
                        Case 3
                            Return {4, 4, 10}.ToList
                        Case 4
                            Return {1, 6, 13}.ToList
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
            GameOrigin = New Vector2(CInt(windowSize.Width - GameSize.X / 2), CInt(windowSize.Height / 2 - GameSize.Y / 2))
        End Sub
        Public Overrides Sub Update()

            If GameState = States.Game OrElse GameState = States.Memo Then
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

            End If

            If KeyBoardHandler.KeyPressed(KeyBindings.OpenInventoryKey) Or ControllerHandler.ButtonPressed(Buttons.X) Then
                If GameState = States.Game Then
                    GameState = States.Memo
                ElseIf GameState = States.Memo Then
                    GameState = States.Game
                End If
            End If

            If GameState = States.Memo Then

                If Controls.Left(False, False, True, False, False, False) = True OrElse ControllerHandler.ButtonPressed(Buttons.LeftShoulder) Then
                    MemoIndex -= 1
                    If MemoIndex < 0 Then
                        MemoIndex = 3
                    End If
                End If
                If Controls.Right(False, False, True, False, False, False) = True OrElse ControllerHandler.ButtonPressed(Buttons.RightShoulder) Then
                    MemoIndex += 1
                    If MemoIndex > 3 Then
                        MemoIndex = 0
                    End If
                End If

            End If
            If Controls.Dismiss And GameState = States.Game Then
                GameState = States.Closing
            End If
            If Controls.Dismiss And GameState = States.Memo Then

            End If

            If GameState = States.Closing Then
                If _interfaceFade > 0F Then
                    _interfaceFade = MathHelper.Lerp(0, _interfaceFade, 0.8F)
                    If _interfaceFade < 0F Then
                        _interfaceFade = 0F
                    End If
                End If
                If _enrollY > 0 Then
                    _enrollY = MathHelper.Lerp(0, _enrollY, 0.8F)
                    If _enrollY <= 0 Then
                        _enrollY = 0
                    End If
                End If
                If _enrollY <= 2.0F Then
                    SetScreen(PreScreen)
                End If
            Else

                Dim maxWindowHeight As Integer = CInt(GameSize.Y / 2)
                If _enrollY < maxWindowHeight Then
                    _enrollY = MathHelper.Lerp(maxWindowHeight, _enrollY, 0.8F)
                    If _enrollY >= maxWindowHeight Then
                        _enrollY = maxWindowHeight
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
        Private Property Memo1 As Boolean = False
        Private Property Memo2 As Boolean = False
        Private Property Memo3 As Boolean = False
        Private Property Memo4 As Boolean = False

        Public Sub Flip()
            If Flipped = False Then
                Flipped = True
            End If
        End Sub
        Public Sub Reset()
            Row = 0
            Column = 0
            Value = Tile.Values.Voltorb
            Flipped = False
        End Sub
        Public Function GetMemo(ByVal MemoNumber As Integer) As Boolean
            Select Case MemoNumber
                Case 1
                    Return Memo1
                Case 2
                    Return Memo2
                Case 3
                    Return Memo3
                Case 3
                    Return Memo4
                Case Else
                    Return Nothing
            End Select
        End Function
        Public Sub SetMemo(ByVal MemoNumber As Integer, ByVal Value As Boolean)
            Select Case MemoNumber
                Case Tile.Values.Voltorb
                    Memo1 = Value
                Case Tile.Values.One
                    Memo2 = Value
                Case Tile.Values.Two
                    Memo3 = Value
                Case Tile.Values.Three
                    Memo4 = Value
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