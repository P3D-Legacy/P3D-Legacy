Public Class StorageSystemScreen

    Inherits Screen

    Public Shared TileOffset As Integer = 0

    Public Enum FeatureTypes
        Deposit
        Withdraw
        Organize
    End Enum

    Public Enum SelectionModes
        SingleMove
        EasyMove
        ItemMove
        Withdraw
        Deposit
    End Enum

    Private Enum CursorModes
        Selection
        Box
    End Enum

    Public Enum FilterTypes
        Pokémon
        Type1
        Type2
        Move
        Ability
        Nature
        Gender
        HeldItem
    End Enum

    Public FeatureType As FeatureTypes = FeatureTypes.Organize

    Public SelectionMode As SelectionModes = SelectionModes.SingleMove

    Public Structure Filter
        Public FilterType As FilterTypes
        Public FilterValue As String
    End Structure

    Public Filters As New List(Of Filter)

    Dim CursorMode As CursorModes = CursorModes.Selection
    Dim CursorPosition As Vector2
    Dim CursorMovePosition As Vector2 = New Vector2(0)
    Dim CursorAimPosition As Vector2 = New Vector2(0)
    Dim CursorMoving As Boolean = False
    Dim CursorSpeed As Integer = 0

    Dim MovingPokemon As Pokemon = Nothing
    Dim PickupPlace As Vector2 = New Vector2(1)
    Dim PickupBox As Integer = 0

    Dim texture As Texture2D
    Dim menuTexture As Texture2D

    Dim MenuEntries As New List(Of MenuEntry)
    Dim MenuVisible As Boolean = False
    Dim MenuCursor As Integer = 0
    Dim MenuHeader As String = ""

    Dim BoxChooseMode As Boolean = False

    Dim Boxes As New List(Of Box)
    Dim CurrentBox As Integer = 0

    Dim modelRoll As Single = 0.0F
    Dim modelPan As Single = 0.0F

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.StorageSystemScreen
        Me.MouseVisible = True

        Me.CanBePaused = True
        Me.CanChat = True
        Me.CanMuteMusic = True

        Me.texture = TextureManager.GetTexture("GUI\Box\storage")
        Me.menuTexture = TextureManager.GetTexture("GUI\Menus\General")

        LoadScreen()
    End Sub

    Private Shared Function LoadBoxes() As List(Of Box)
        Dim boxes As New List(Of Box)

        For i = 0 To Core.Player.BoxAmount - 1
            boxes.Add(New Box(i))
        Next

        For Each line As String In Core.Player.BoxData.SplitAtNewline()
            If line.StartsWith("BOX") = False And line <> "" Then
                Dim Data() As String = line.Split(CChar(","))

                Dim boxIndex As String = Data(0)
                Dim pokemonIndex As String = Data(1)
                Dim pokemonData As String = line.Remove(0, line.IndexOf("{"))

                If GetBox(CInt(boxIndex), boxes) Is Nothing Then
                    boxes.Add(New Box(CInt(boxIndex)))
                End If

                If GetBox(CInt(boxIndex), boxes).Pokemon.ContainsKey(CInt(pokemonIndex)) = False Then
                    GetBox(CInt(boxIndex), boxes).Pokemon.Add(CInt(pokemonIndex), New PokemonWrapper(pokemonData)) ' Pokemon.GetPokemonByData(pokemonData))
                End If
            ElseIf line.StartsWith("BOX") = True Then
                Dim boxData() As String = line.Split(CChar("|"))

                Dim boxIndex As Integer = CInt(boxData(1))
                Dim boxName As String = boxData(2)
                Dim boxBackground As Integer = CInt(boxData(3))

                If GetBox(boxIndex, boxes) Is Nothing Then
                    boxes.Add(New Box(boxIndex))
                End If

                GetBox(boxIndex, boxes).Background = boxBackground
                GetBox(boxIndex, boxes).Name = boxName
            End If
        Next

        Dim minBox As Integer = -1
        Dim maxBox As Integer = -1

        For Each b As Box In boxes
            If b.index < minBox Or minBox = -1 Then
                minBox = b.index
            End If
            If b.index > maxBox Or maxBox = -1 Then
                maxBox = b.index
            End If
        Next

        For i = minBox To maxBox
            If GetBox(i, boxes) Is Nothing Then
                boxes.Add(New Box(i))
            End If
        Next

        Dim lastBox As Box = boxes(0)
        For Each b As Box In boxes
            If b.index > lastBox.index Then
                lastBox = b
            End If
        Next
        lastBox.IsBattleBox = True

        Return boxes
    End Function

    Private Sub LoadScreen()
        SelectionMode = Player.Temp.PCSelectionType

        CursorMode = CursorModes.Selection
        CursorPosition = Player.Temp.StorageSystemCursorPosition

        Me.Boxes = LoadBoxes()

        Me.CurrentBox = Player.Temp.PCBoxIndex
        Me.BoxChooseMode = Player.Temp.PCBoxChooseMode
    End Sub

#Region "Update"

    Public Overrides Sub Update()
        If ControllerHandler.ButtonPressed(Buttons.Y) = True Or KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Then
            Core.SetScreen(New StorageSystemFilterScreen(Me))
        End If

        If MenuVisible = True Then
            For i = 0 To Me.MenuEntries.Count - 1
                If i <= Me.MenuEntries.Count - 1 Then
                    Dim m As MenuEntry = Me.MenuEntries(i)

                    m.Update(Me)
                End If
            Next

            If Controls.Up(True, True) = True Then
                Me.MenuCursor -= 1
            End If
            If Controls.Down(True, True) = True Then
                Me.MenuCursor += 1
            End If

            Dim maxIndex As Integer = 0
            Dim minIndex As Integer = 100

            For Each e As MenuEntry In Me.MenuEntries
                If e.Index < minIndex Then
                    minIndex = e.Index
                End If
                If e.Index > maxIndex Then
                    maxIndex = e.Index
                End If
            Next

            If Me.MenuCursor > maxIndex Then
                Me.MenuCursor = minIndex
            ElseIf Me.MenuCursor < minIndex Then
                Me.MenuCursor = maxIndex
            End If
        Else
            TurnModel()
            If CursorMoving = True Then
                MoveCursor()
            Else
                If ControllerHandler.ButtonPressed(Buttons.RightShoulder) = True Or Controls.Right(True, False, True, False, False, False) = True Then
                    Me.CurrentBox += 1
                    If CurrentBox > Me.Boxes.Count - 1 Then
                        CurrentBox = 0
                    End If
                End If
                If ControllerHandler.ButtonPressed(Buttons.LeftShoulder) = True Or Controls.Left(True, False, True, False, False, False) = True Then
                    Me.CurrentBox -= 1
                    If CurrentBox < 0 Then
                        CurrentBox = Me.Boxes.Count - 1
                    End If
                End If

                PressNumberButtons()

                If GetRelativeMousePosition() <> New Vector2(-1) AndAlso GetRelativeMousePosition() = CursorPosition AndAlso Controls.Accept(True, False, False) = True Then
                    ChooseObject()
                End If

                ControlCursor()

                If Controls.Accept(False, True, True) = True Then
                    ChooseObject()
                End If

                If Controls.Dismiss(True, True, True) = True Then
                    CloseScreen()
                End If
            End If
        End If

        StorageSystemScreen.TileOffset += 1
        If StorageSystemScreen.TileOffset >= 64 Then
            StorageSystemScreen.TileOffset = 0
        End If
    End Sub

    Private Sub TurnModel()
        If Controls.ShiftDown("L", False) = True Then
            modelRoll -= 0.1F
        End If
        If ControllerHandler.ButtonDown(Buttons.RightThumbstickLeft) = True Then
            Dim gPadState As GamePadState = GamePad.GetState(PlayerIndex.One)
            modelRoll -= gPadState.ThumbSticks.Right.X * 0.1F
        End If
        If ControllerHandler.ButtonDown(Buttons.RightThumbstickRight) = True Then
            Dim gPadState As GamePadState = GamePad.GetState(PlayerIndex.One)
            modelRoll -= gPadState.ThumbSticks.Right.X * 0.1F
        End If
        If Controls.ShiftDown("R", False) = True Then
            modelRoll += 0.1F
        End If
    End Sub

    Private Sub PressNumberButtons()
        Dim switchTo As Integer = -1
        If KeyBoardHandler.KeyPressed(Keys.D1) = True Then
            switchTo = 0
        End If
        If KeyBoardHandler.KeyPressed(Keys.D2) = True Then
            switchTo = 1
        End If
        If KeyBoardHandler.KeyPressed(Keys.D3) = True Then
            switchTo = 2
        End If
        If KeyBoardHandler.KeyPressed(Keys.D4) = True Then
            switchTo = 3
        End If
        If KeyBoardHandler.KeyPressed(Keys.D5) = True Then
            switchTo = 4
        End If
        If KeyBoardHandler.KeyPressed(Keys.D6) = True Then
            switchTo = 5
        End If
        If KeyBoardHandler.KeyPressed(Keys.D7) = True Then
            switchTo = 6
        End If
        If KeyBoardHandler.KeyPressed(Keys.D8) = True Then
            switchTo = 7
        End If
        If KeyBoardHandler.KeyPressed(Keys.D9) = True Then
            switchTo = 8
        End If
        If KeyBoardHandler.KeyPressed(Keys.D0) = True Then
            switchTo = 9
        End If

        If switchTo > -1 Then
            If Me.Boxes.Count - 1 >= switchTo Then
                CurrentBox = switchTo
            End If
        End If
    End Sub

    Private Sub ChooseObject()
        Select Case CursorPosition.Y
            Case 0
                Select Case CursorPosition.X
                    Case 0
                        Me.CurrentBox -= 1
                        If CurrentBox < 0 Then
                            CurrentBox = Me.Boxes.Count - 1
                        End If
                    Case 1, 2, 3, 4
                        If Me.BoxChooseMode = True Then
                            Me.BoxChooseMode = False
                        Else
                            Dim e As New MenuEntry(3, "CHOOSE BOX", False, AddressOf Me.ChooseBox)
                            Dim e1 As New MenuEntry(4, "CHANGE MODE", False, AddressOf Me.ChangemodeMenu)
                            If GetBox(CurrentBox).IsBattleBox = True Then
                                Dim e4 As New MenuEntry(5, "CANCEL", True, Nothing)
                                Me.SetupMenu({e, e1, e4}, "What do you want to do?")
                            Else
                                Dim e2 As New MenuEntry(5, "WALLPAPER", False, AddressOf WallpaperMain)
                                Dim e3 As New MenuEntry(6, "NAME", False, AddressOf SelectNameBox)
                                Dim e4 As New MenuEntry(7, "CANCEL", True, Nothing)
                                Me.SetupMenu({e, e1, e2, e3, e4}, "What do you want to do?")
                            End If
                        End If
                    Case 5
                        Me.CurrentBox += 1
                        If CurrentBox > Me.Boxes.Count - 1 Then
                            CurrentBox = 0
                        End If
                    Case 6
                        SelectPokemon()
                End Select
            Case 1, 2, 3, 4, 5
                If BoxChooseMode = True And CursorPosition.X < 6 And CursorPosition.Y > 0 Then
                    Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)

                    If Not GetBox(id) Is Nothing Then
                        Me.CurrentBox = id
                        Me.BoxChooseMode = False
                    End If
                Else
                    SelectPokemon()
                End If
        End Select
    End Sub

#Region "ChangeMode"

    Private Sub ChangemodeMenu()
        Dim e As New MenuEntry(3, "WITHDRAW", False, AddressOf SelectWithdraw)
        Dim e1 As New MenuEntry(4, "DEPOSIT", False, AddressOf SelectDeposit)
        Dim e2 As New MenuEntry(5, "SINGLE MOVE", False, AddressOf Me.SelectSingleMove)
        Dim e3 As New MenuEntry(6, "EASY MOVE", False, AddressOf Me.SelectEasyMove)
        Dim e4 As New MenuEntry(7, "CANCEL", True, AddressOf Me.ChooseObject)
        Me.SetupMenu({e, e1, e2, e3, e4}, "Choose a mode to use.")
    End Sub

    Private Sub SelectWithdraw()
        Me.SelectionMode = SelectionModes.Withdraw
    End Sub

    Private Sub SelectDeposit()
        Me.SelectionMode = SelectionModes.Deposit
    End Sub

    Private Sub SelectSingleMove()
        Me.SelectionMode = SelectionModes.SingleMove
    End Sub

    Private Sub SelectEasyMove()
        Me.SelectionMode = SelectionModes.EasyMove
    End Sub

#End Region

    Private Shadows Sub ChooseBox()
        Me.BoxChooseMode = Not Me.BoxChooseMode
    End Sub

    Private Sub SelectNameBox()
        Core.SetScreen(New InputScreen(Core.CurrentScreen, "BOX " & CStr(GetBox(CurrentBox).index + 1), InputScreen.InputModes.Text, GetBox(CurrentBox).Name, 11, New List(Of Texture2D), AddressOf Me.NameBox))
    End Sub

    Private Sub NameBox(ByVal name As String)
        GetBox(CurrentBox).Name = name
    End Sub

#Region "Backgrounds"

    Private Sub WallpaperMain()
        Dim badges As Integer = Core.Player.Badges.Count

        If Core.Player.SandBoxMode = True Or GameController.IS_DEBUG_ACTIVE = True Then
            badges = 16
        End If

        Select Case badges
            Case 0, 1
                Dim e As New MenuEntry(3, "PACKAGE 1", False, AddressOf WallpaperPackage1)
                Dim e4 As New MenuEntry(4, "CANCEL", True, AddressOf ChooseObject)
                SetupMenu({e, e4}, "Please pick a theme.")
            Case 2, 3, 4
                Dim e As New MenuEntry(3, "PACKAGE 1", False, AddressOf WallpaperPackage1)
                Dim e1 As New MenuEntry(4, "PACKAGE 2", False, AddressOf WallpaperPackage2)
                Dim e4 As New MenuEntry(5, "CANCEL", True, AddressOf ChooseObject)
                SetupMenu({e, e1, e4}, "Please pick a theme.")
            Case 5, 6, 7
                Dim e As New MenuEntry(3, "PACKAGE 1", False, AddressOf WallpaperPackage1)
                Dim e1 As New MenuEntry(4, "PACKAGE 2", False, AddressOf WallpaperPackage2)
                Dim e2 As New MenuEntry(5, "PACKAGE 3", False, AddressOf WallpaperPackage3)
                Dim e4 As New MenuEntry(6, "CANCEL", True, AddressOf ChooseObject)
                SetupMenu({e, e1, e2, e4}, "Please pick a theme.")
            Case Else
                Dim e As New MenuEntry(3, "PACKAGE 1", False, AddressOf WallpaperPackage1)
                Dim e1 As New MenuEntry(4, "PACKAGE 2", False, AddressOf WallpaperPackage2)
                Dim e2 As New MenuEntry(5, "PACKAGE 3", False, AddressOf WallpaperPackage3)
                Dim e3 As New MenuEntry(6, "PACKAGE 4", False, AddressOf WallpaperPackage4)
                Dim e4 As New MenuEntry(7, "CANCEL", True, AddressOf ChooseObject)
                SetupMenu({e, e1, e2, e3, e4}, "Please pick a theme.")
        End Select
    End Sub

    Private Sub WallpaperPackage1()
        Dim e As New MenuEntry(3, "FOREST", False, AddressOf PickWallpaper, 0)
        Dim e1 As New MenuEntry(4, "CITY", False, AddressOf PickWallpaper, 1)
        Dim e2 As New MenuEntry(5, "DESERT", False, AddressOf PickWallpaper, 2)
        Dim e3 As New MenuEntry(6, "SAVANNA", False, AddressOf PickWallpaper, 3)
        Dim e4 As New MenuEntry(7, "CAVE", False, AddressOf PickWallpaper, 8)
        Dim e5 As New MenuEntry(8, "RIVER", False, AddressOf PickWallpaper, 11)
        Dim e6 As New MenuEntry(9, "CANCEL", True, AddressOf WallpaperMain)
        SetupMenu({e, e1, e2, e3, e4, e5, e6}, "Pick the wallpaper.")
    End Sub

    Private Sub WallpaperPackage2()
        Dim e As New MenuEntry(3, "VOLCANO", False, AddressOf PickWallpaper, 5)
        Dim e1 As New MenuEntry(4, "SNOW", False, AddressOf PickWallpaper, 6)
        Dim e2 As New MenuEntry(5, "BEACH", False, AddressOf PickWallpaper, 9)
        Dim e3 As New MenuEntry(6, "SEAFLOOR", False, AddressOf PickWallpaper, 10)
        Dim e4 As New MenuEntry(7, "CRAG", False, AddressOf PickWallpaper, 4)
        Dim e5 As New MenuEntry(8, "STEEL", False, AddressOf PickWallpaper, 7)
        Dim e6 As New MenuEntry(9, "CANCEL", True, AddressOf WallpaperMain)
        SetupMenu({e, e1, e2, e3, e4, e5, e6}, "Pick the wallpaper.")
    End Sub

    Private Sub WallpaperPackage3()
        Dim e As New MenuEntry(3, "VOLCANO 2", False, AddressOf PickWallpaper, 14)
        Dim e1 As New MenuEntry(4, "CITY 2", False, AddressOf PickWallpaper, 15)
        Dim e2 As New MenuEntry(5, "SNOW 2", False, AddressOf PickWallpaper, 16)
        Dim e3 As New MenuEntry(6, "DESERT 2", False, AddressOf PickWallpaper, 17)
        Dim e4 As New MenuEntry(7, "SAVANNA 2", False, AddressOf PickWallpaper, 18)
        Dim e5 As New MenuEntry(8, "STEEL 2", False, AddressOf PickWallpaper, 19)
        Dim e6 As New MenuEntry(9, "CANCEL", True, AddressOf WallpaperMain)
        SetupMenu({e, e1, e2, e3, e4, e5, e6}, "Pick the wallpaper.")
    End Sub

    Private Sub WallpaperPackage4()
        Dim e As New MenuEntry(3, "SYSTEM", False, AddressOf PickWallpaper, 22)
        Dim e1 As New MenuEntry(4, "SIMPLE", False, AddressOf PickWallpaper, 13)
        Dim e2 As New MenuEntry(5, "CHECKS", False, AddressOf PickWallpaper, 12)
        Dim e3 As New MenuEntry(6, "SEASONS", False, AddressOf PickWallpaper, 23)
        Dim e4 As New MenuEntry(7, "RETRO 1", False, AddressOf PickWallpaper, 20)
        Dim e5 As New MenuEntry(8, "RETRO 2", False, AddressOf PickWallpaper, 21)
        Dim e6 As New MenuEntry(9, "CANCEL", True, AddressOf WallpaperMain)
        SetupMenu({e, e1, e2, e3, e4, e5, e6}, "Pick the wallpaper.")
    End Sub

    Private Sub PickWallpaper(ByVal e As MenuEntry)
        GetBox(CurrentBox).Background = CInt(e.TAG)
    End Sub

#End Region

    Private Sub GetYOffset(ByVal p As Pokemon)
        Dim t As Texture2D = p.GetTexture(True)
        Me.yOffset = -1

        Dim cArr(t.Width * t.Height - 1) As Color
        t.GetData(cArr)

        For y = 0 To t.Height - 1
            For x = 0 To t.Width - 1
                If cArr(x + y * t.Height) <> Color.Transparent Then
                    Me.yOffset = y
                    Exit For
                End If
            Next

            If Me.yOffset <> -1 Then
                Exit For
            End If
        Next
    End Sub

    Private Sub MoveCursor()
        Dim changedPosition As Boolean = False

        If CursorMovePosition <> CursorAimPosition Then
            changedPosition = True
        End If

        If CursorMovePosition.X < CursorAimPosition.X Then
            CursorMovePosition.X += Me.CursorSpeed
            If CursorMovePosition.X >= CursorAimPosition.X Then
                CursorMovePosition.X = CursorAimPosition.X
            End If
        End If
        If CursorMovePosition.X > CursorAimPosition.X Then
            CursorMovePosition.X -= Me.CursorSpeed
            If CursorMovePosition.X <= CursorAimPosition.X Then
                CursorMovePosition.X = CursorAimPosition.X
            End If
        End If
        If CursorMovePosition.Y < CursorAimPosition.Y Then
            CursorMovePosition.Y += Me.CursorSpeed
            If CursorMovePosition.Y >= CursorAimPosition.Y Then
                CursorMovePosition.Y = CursorAimPosition.Y
            End If
        End If
        If CursorMovePosition.Y > CursorAimPosition.Y Then
            CursorMovePosition.Y -= Me.CursorSpeed
            If CursorMovePosition.Y <= CursorAimPosition.Y Then
                CursorMovePosition.Y = CursorAimPosition.Y
            End If
        End If

        If CursorAimPosition = CursorMovePosition Then
            Me.CursorMoving = False

            If Me.SelectionMode = SelectionModes.EasyMove And changedPosition = True And Me.ClickedObject = True Then
                ChooseObject()
            End If
        End If
    End Sub

    Dim ClickedObject As Boolean = False

    Private Sub ControlCursor()
        Dim PreCursor As Vector2 = CursorPosition
        If Controls.Right(True, True, False) = True Then
            Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
            Me.CursorPosition.X += 1
            If Me.CursorPosition.Y = 0 And Me.CursorPosition.X > 1 And Me.CursorPosition.X < 5 Then
                Me.CursorPosition.X = 5
            End If
            Me.CursorMoving = True
            Me.ClickedObject = False
        End If
        If Controls.Left(True, True, False) = True Then
            Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
            Me.CursorPosition.X -= 1
            If Me.CursorPosition.Y = 0 And Me.CursorPosition.X > 0 And Me.CursorPosition.X < 4 Then
                Me.CursorPosition.X = 0
            End If
            Me.CursorMoving = True
            Me.ClickedObject = False
        End If
        If Controls.Up(True, True, False) Then
            Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
            Me.CursorPosition.Y -= 1
            Me.CursorMoving = True
            Me.ClickedObject = False
        End If
        If Controls.Down(True, True, False) Then
            Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
            Me.CursorPosition.Y += 1
            Me.CursorMoving = True
            Me.ClickedObject = False
            If Me.CursorPosition.Y = 1 And GetBox(CurrentBox).IsBattleBox = True And Me.CursorPosition.X < 6 And BoxChooseMode = False Then
                Me.CursorPosition.X = 2
            End If
        End If
        If ControllerHandler.ButtonPressed(Buttons.X) = True Then
            Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
            Me.CursorPosition = New Vector2(1, 0)
            Me.CursorMoving = True
            Me.ClickedObject = False
        End If

        If Controls.Accept(True, False, False) = True AndAlso GetRelativeMousePosition() <> New Vector2(-1) Then
            Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
            Me.CursorPosition = GetRelativeMousePosition()
            Me.CursorMoving = True
            Me.ClickedObject = True
        End If

        Dim XRange() As Integer = {0, 6}

        If Me.BoxChooseMode = False Then
            If Me.SelectionMode = SelectionModes.Withdraw And CursorPosition.Y > 0 Then
                If GetBox(CurrentBox).IsBattleBox = True Then
                    XRange = {2, 3}
                Else
                    XRange = {0, 5}
                End If
            ElseIf Me.SelectionMode = SelectionModes.Deposit And CursorPosition.Y > 0 Then
                XRange = {6, 6}
            Else
                If GetBox(CurrentBox).IsBattleBox = True Then
                    If CursorPosition.Y = 0 Then
                        XRange = {0, 6}
                    Else
                        XRange = {2, 6}
                    End If
                End If
            End If
        End If

        If CursorPosition.X < XRange(0) Then
            CursorPosition.X = XRange(1)
        End If
        If CursorPosition.X > XRange(1) Then
            CursorPosition.X = XRange(0)
        End If

        If GetBox(CurrentBox).IsBattleBox = True And Me.BoxChooseMode = False Then
            If Me.CursorPosition.Y > 0 And Me.CursorPosition.X > 3 And Me.CursorPosition.X < 6 Then
                If PreCursor.X > Me.CursorPosition.X Then
                    Me.CursorPosition.X = 3
                Else
                    Me.CursorPosition.X = 6
                End If
            End If
        End If

        Dim YRange() As Integer = {0, 5}

        If Me.BoxChooseMode = False Then
            If GetBox(CurrentBox).IsBattleBox = True And Me.CursorPosition.X < 6 Then
                YRange = {0, 3}
            End If
        End If

        If CursorPosition.Y < YRange(0) Then
            CursorPosition.Y = YRange(1)
        End If
        If CursorPosition.Y > YRange(1) Then
            CursorPosition.Y = YRange(0)
        End If

        CursorAimPosition = GetAbsoluteCursorPosition(Me.CursorPosition)

        Me.CursorSpeed = CInt(Vector2.Distance(CursorMovePosition, CursorAimPosition) * (30 / 100))
    End Sub

    Private Sub CloseScreen()
        If Me.BoxChooseMode = True Then
            Me.BoxChooseMode = False
        Else
            If Not MovingPokemon Is Nothing Then
                If PickupPlace.X = 6 Then
                    Core.Player.Pokemons.Add(Me.MovingPokemon)
                Else
                    Dim id As Integer = CInt(PickupPlace.X) + CInt((PickupPlace.Y - 1) * 6)

                    If GetBox(PickupBox).IsBattleBox = True Then
                        GetBox(PickupBox).Pokemon.Add(GetBox(PickupBox).Pokemon.Count, New PokemonWrapper(Me.MovingPokemon)) ' Me.MovingPokemon))
                    Else
                        GetBox(PickupBox).Pokemon.Add(id, New PokemonWrapper(Me.MovingPokemon)) 'Me.MovingPokemon)
                    End If

                    CurrentBox = PickupBox
                End If
                Me.MovingPokemon = Nothing
            Else
                Player.Temp.StorageSystemCursorPosition = Me.CursorPosition
                Player.Temp.PCBoxIndex = Me.CurrentBox
                Player.Temp.PCBoxChooseMode = Me.BoxChooseMode
                Player.Temp.PCSelectionType = Me.SelectionMode

                Core.Player.BoxData = GetBoxSaveData(Me.Boxes)

                Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.Black, False))
            End If
        End If
    End Sub

    Private Shared Function GetBoxSaveData(ByVal boxes As List(Of Box)) As String
        Dim BoxesFull As Boolean = True
        Dim newData As New List(Of String)
        For Each b As Box In boxes
            If b.IsBattleBox = False Then
                newData.Add("BOX|" & b.index & "|" & b.Name & "|" & b.Background)

                Dim hasPokemon As Boolean = False
                For i = 0 To 29
                    If b.Pokemon.Keys.Contains(i) = True Then
                        hasPokemon = True
                        newData.Add(b.index.ToString() & "," & i.ToString() & "," & b.Pokemon(i).PokemonData)
                    End If
                Next
                If hasPokemon = False Then
                    BoxesFull = False
                End If
            End If
        Next

        Dim addedBoxes As Integer = 0
        If BoxesFull = True And boxes.Count < 30 Then
            Dim newBoxes As Integer = (5).Clamp(1, 30 - boxes.Count)
            addedBoxes = newBoxes

            For i = 0 To newBoxes - 1
                Dim newBoxID As Integer = boxes.Count - 1 + i

                newData.Add("BOX|" & newBoxID.ToString() & "|BOX " & (newBoxID + 1).ToString() & "|" & Core.Random.Next(0, 19).ToString())
            Next
        End If

        Dim battleBox As Box = boxes.Last
        newData.Add("BOX|" & CStr(boxes.Count - 1 + addedBoxes) & "|" & battleBox.Name & "|" & battleBox.Background)

        For i = 0 To 29
            If battleBox.Pokemon.Keys.Contains(i) = True Then
                newData.Add(CStr(boxes.Count - 1 + addedBoxes) & "," & i.ToString() & "," & battleBox.Pokemon(i).PokemonData)
            End If
        Next

        Dim returnData As String = ""
        For Each l As String In newData
            If returnData <> "" Then
                returnData &= vbNewLine
            End If
            returnData &= l
        Next

        Return returnData
    End Function

    Private Function GetRelativeMousePosition() As Vector2
        For x = 0 To 5
            For y = 0 To 4
                If New Rectangle(50 + x * 100, 200 + y * 84, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                    Return New Vector2(x, y + 1)
                End If
            Next
        Next

        For y = 0 To 5
            If New Rectangle(Core.windowSize.Width - 260, y * 100 + 50, 128, 80).Contains(MouseHandler.MousePosition) = True Then
                Return New Vector2(6, y)
            End If
        Next

        If New Rectangle(10, 52, 96, 96).Contains(MouseHandler.MousePosition) = True Then
            Return New Vector2(0, 0)
        End If
        If New Rectangle(655, 52, 96, 96).Contains(MouseHandler.MousePosition) = True Then
            Return New Vector2(5, 0)
        End If

        If New Rectangle(80, 50, 600, 100).Contains(MouseHandler.MousePosition) = True Then
            Return New Vector2(1, 0)
        End If

        Return New Vector2(-1)
    End Function

    Private Function GetAbsoluteCursorPosition(ByVal relPos As Vector2) As Vector2
        Select Case relPos.Y
            Case 0
                Select Case relPos.X
                    Case 0
                        Return New Vector2(60, 20)
                    Case 1, 2, 3, 4
                        Return New Vector2(380, 30)
                    Case 5
                        Return New Vector2(705, 20)
                    Case 6
                        Return New Vector2(Core.windowSize.Width - 200, 20)
                End Select
            Case 1, 2, 3, 4, 5
                Select Case relPos.X
                    Case 0, 1, 2, 3, 4, 5
                        Return New Vector2(50 + relPos.X * 100 + 42, 200 + (relPos.Y - 1) * 84 - 42)
                    Case 6
                        Return New Vector2(Core.windowSize.Width - 200, 20 + 100 * relPos.Y)
                End Select
        End Select
    End Function

    Private Function GetBattleBoxID() As Integer
        Dim id As Integer = -1

        If CursorPosition.X = 2 Then
            Select Case CursorPosition.Y
                Case 1
                    Return 0
                Case 2
                    Return 2
                Case 3
                    Return 4
            End Select
        ElseIf CursorPosition.X = 3 Then
            Select Case CursorPosition.Y
                Case 1
                    Return 1
                Case 2
                    Return 3
                Case 3
                    Return 5
            End Select
        End If

        Return -1
    End Function

    Private Sub SelectPokemon()
        Select Case Me.SelectionMode
            Case SelectionModes.SingleMove, SelectionModes.Withdraw, SelectionModes.Deposit
                If Me.MovingPokemon Is Nothing Then
                    Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)

                    If GetBox(CurrentBox).IsBattleBox = True Then
                        id = GetBattleBoxID()
                    End If

                    If GetBox(CurrentBox).Pokemon.Keys.Contains(id) = True And CursorPosition.X < 6 Or CursorPosition.X = 6 And Core.Player.Pokemons.Count - 1 >= CInt(CursorPosition.Y) Then
                        Dim p As Pokemon = Nothing
                        If CursorPosition.X = 6 Then
                            p = Core.Player.Pokemons(CInt(CursorPosition.Y))
                        Else
                            p = GetBox(CurrentBox).Pokemon(id).GetPokemon()
                        End If

                        Dim e As MenuEntry

                        Select Case Me.SelectionMode
                            Case SelectionModes.Withdraw
                                e = New MenuEntry(3, "WITHDRAW", False, AddressOf WithdrawPokemon)
                            Case SelectionModes.Deposit
                                e = New MenuEntry(3, "DEPOSIT", False, AddressOf DepositPokemon)
                            Case Else
                                e = New MenuEntry(3, "MOVE", False, AddressOf PickupPokemon)
                        End Select

                        Dim e1 As New MenuEntry(4, "SUMMARY", False, AddressOf SummaryPokemon)
                        Dim e2 As New MenuEntry(5, "RELEASE", False, AddressOf ReleasePokemon)
                        Dim e3 As New MenuEntry(6, "CANCEL", True, Nothing)
                        SetupMenu({e, e1, e2, e3}, p.GetDisplayName() & " is selected.")
                    End If
                Else
                    PickupPokemon()
                End If
            Case SelectionModes.EasyMove
                PickupPokemon()
        End Select
    End Sub

    Private Sub PickupPokemon()
        If CursorPosition.X = 6 Then
            If Core.Player.Pokemons.Count - 1 >= CursorPosition.Y Then
                If Not Me.MovingPokemon Is Nothing Then
                    Dim l As New List(Of Pokemon)
                    l.AddRange(Core.Player.Pokemons.ToArray())
                    l.RemoveAt(CInt(CursorPosition.Y))
                    l.Add(Me.MovingPokemon)
                    Dim hasPokemon As Boolean = False
                    For Each p As Pokemon In l
                        If p.IsEgg() = False And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0 Then
                            hasPokemon = True
                            Exit For
                        End If
                    Next

                    If hasPokemon = True Then
                        Dim sPokemon As Pokemon = Core.Player.Pokemons(CInt(CursorPosition.Y))
                        Me.MovingPokemon.FullRestore()
                        Core.Player.Pokemons.Insert(CInt(CursorPosition.Y), Me.MovingPokemon)
                        MovingPokemon = sPokemon
                        Core.Player.Pokemons.RemoveAt(CInt(CursorPosition.Y) + 1)
                    Else
                        Dim e As New MenuEntry(3, "OK", True, Nothing)
                        SetupMenu({e}, "Can't remove last Pokémon from party.")
                    End If
                Else
                    Dim l As New List(Of Pokemon)
                    l.AddRange(Core.Player.Pokemons.ToArray())
                    l.RemoveAt(CInt(CursorPosition.Y))
                    Dim hasPokemon As Boolean = False
                    For Each p As Pokemon In l
                        If p.IsEgg() = False And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0 Then
                            hasPokemon = True
                            Exit For
                        End If
                    Next

                    If hasPokemon = True Then
                        Me.MovingPokemon = Core.Player.Pokemons(CInt(CursorPosition.Y))
                        Core.Player.Pokemons.RemoveAt(CInt(CursorPosition.Y))

                        PickupBox = 0
                        PickupPlace = New Vector2(6, 0)
                    Else
                        Dim e As New MenuEntry(3, "OK", True, Nothing)
                        SetupMenu({e}, "Can't remove last Pokémon from party.")
                    End If
                End If
            Else
                If Not Me.MovingPokemon Is Nothing Then
                    Me.MovingPokemon.FullRestore()
                    Core.Player.Pokemons.Add(Me.MovingPokemon)
                    Me.MovingPokemon = Nothing
                End If
            End If
        Else
            Dim pokemonExists As Boolean = False
            Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)

            If GetBox(CurrentBox).IsBattleBox = True Then
                id = GetBattleBoxID()
            End If

            pokemonExists = GetBox(CurrentBox).Pokemon.Keys.Contains(id)

            If pokemonExists = True Then
                If Me.MovingPokemon Is Nothing Then
                    Me.MovingPokemon = GetBox(CurrentBox).Pokemon(id).GetPokemon()
                    GetBox(CurrentBox).Pokemon.Remove(id)

                    PickupBox = CurrentBox
                    PickupPlace = CursorPosition
                    RearrangeBattleBox(GetBox(CurrentBox))
                Else
                    Me.MovingPokemon.FullRestore()
                    Dim sPokemon As Pokemon = GetBox(CurrentBox).Pokemon(id).GetPokemon()
                    GetBox(CurrentBox).Pokemon(id) = New PokemonWrapper(Me.MovingPokemon) 'Me.MovingPokemon
                    Me.MovingPokemon = sPokemon
                End If
            Else
                If Not Me.MovingPokemon Is Nothing Then
                    Me.MovingPokemon.FullRestore()

                    If GetBox(CurrentBox).IsBattleBox = True Then
                        GetBox(CurrentBox).Pokemon.Add(GetBox(CurrentBox).Pokemon.Count, New PokemonWrapper(Me.MovingPokemon)) 'Me.MovingPokemon)
                    Else
                        GetBox(CurrentBox).Pokemon.Add(id, New PokemonWrapper(Me.MovingPokemon)) 'Me.MovingPokemon)
                    End If

                    Me.MovingPokemon = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub WithdrawPokemon()
        If Core.Player.Pokemons.Count < 6 Then
            Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)

            If GetBox(CurrentBox).IsBattleBox = True Then
                id = GetBattleBoxID()
            End If

            Dim pokemonExists As Boolean = GetBox(CurrentBox).Pokemon.Keys.Contains(id)

            If pokemonExists = True Then
                Core.Player.Pokemons.Add(GetBox(CurrentBox).Pokemon(id).GetPokemon())
                GetBox(CurrentBox).Pokemon.Remove(id)
            End If
        Else
            Dim e As New MenuEntry(3, "OK", True, Nothing)
            SetupMenu({e}, "Party is full!")
        End If

        RearrangeBattleBox(GetBox(CurrentBox))
    End Sub

    Private Sub DepositPokemon()
        If GetBox(CurrentBox).Pokemon.Count < 30 Then
            If Core.Player.Pokemons.Count - 1 >= CInt(Me.CursorPosition.Y) Then
                Dim l As New List(Of Pokemon)
                l.AddRange(Core.Player.Pokemons.ToArray())
                l.RemoveAt(CInt(CursorPosition.Y))
                Dim hasPokemon As Boolean = False
                For Each p As Pokemon In l
                    If p.IsEgg() = False And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0 Then
                        hasPokemon = True
                        Exit For
                    End If
                Next

                If hasPokemon = True Then
                    Dim nextIndex As Integer = 0
                    While GetBox(CurrentBox).Pokemon.Keys.Contains(nextIndex) = True
                        nextIndex += 1
                    End While
                    Core.Player.Pokemons(CInt(Me.CursorPosition.Y)).FullRestore()
                    GetBox(CurrentBox).Pokemon.Add(nextIndex, New PokemonWrapper(Core.Player.Pokemons(CInt(Me.CursorPosition.Y)))) 'Core.Player.Pokemons(CInt(Me.CursorPosition.Y)))
                    Core.Player.Pokemons.RemoveAt(CInt(Me.CursorPosition.Y))
                Else
                    Dim e As New MenuEntry(3, "OK", True, Nothing)
                    SetupMenu({e}, "Can't remove last Pokémon from party.")
                End If
            End If
        End If
    End Sub

    Private Sub SummaryPokemon()
        If CursorPosition.X = 6 Then
            Core.SetScreen(New PokemonStatusScreen(Core.CurrentScreen, CInt(CursorPosition.Y), Core.Player.Pokemons.ToArray(), Core.Player.Pokemons(CInt(CursorPosition.Y)), False))
        Else
            Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)
            If GetBox(CurrentBox).IsBattleBox = True Then
                id = GetBattleBoxID()
            End If

            Core.SetScreen(New PokemonStatusScreen(Core.CurrentScreen, GetBox(CurrentBox).GetPokemonList().IndexOf(GetBox(CurrentBox).Pokemon(id).GetPokemon()), GetBox(CurrentBox).GetPokemonList().ToArray(), GetBox(CurrentBox).Pokemon(id).GetPokemon(), False))
        End If
    End Sub

    Private Sub ReleasePokemon()
        Dim hasPokemon As Boolean = False

        If Me.CursorPosition.X = 6 Then
            Dim l As New List(Of Pokemon)
            l.AddRange(Core.Player.Pokemons.ToArray())
            l.RemoveAt(CInt(CursorPosition.Y))

            For Each p As Pokemon In l
                If p.IsEgg() = False And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0 Then
                    hasPokemon = True
                    Exit For
                End If
            Next
        Else
            hasPokemon = True
        End If

        If hasPokemon = True Then
            Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)

            If GetBox(CurrentBox).IsBattleBox = True Then
                id = GetBattleBoxID()
            End If

            Dim p As Pokemon = Nothing
            If CursorPosition.X = 6 Then
                p = Core.Player.Pokemons(CInt(CursorPosition.Y))
            Else
                p = GetBox(CurrentBox).Pokemon(id).GetPokemon()
            End If

            If p.IsEgg() = False Then
                Dim e1 As New MenuEntry(3, "NO", True, AddressOf SelectPokemon)
                Dim e As New MenuEntry(4, "YES", False, AddressOf ConfirmRelease)
                Me.SetupMenu({e1, e}, "Release " & p.GetDisplayName() & "?")
            Else
                Me.SetupMenu({New MenuEntry(3, "OK", True, Nothing)}, "Cannot release an egg.")
            End If
        Else
            Me.SetupMenu({New MenuEntry(3, "OK", True, Nothing)}, "Cannot release the last Pokémon.")
        End If
    End Sub

    Private Sub ConfirmRelease()
        Dim id As Integer = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)
        If CursorPosition.X = 6 Then
            Core.Player.Pokemons.RemoveAt(CInt(CursorPosition.Y))
        Else
            GetBox(CurrentBox).Pokemon.Remove(id)
        End If
    End Sub

    Private Sub RearrangeBattleBox(ByVal b As Box)
        If b.IsBattleBox = True Then
            Dim p As List(Of Pokemon) = b.GetPokemonList()
            b.Pokemon.Clear()

            For i = 0 To p.Count - 1
                b.Pokemon.Add(i, New PokemonWrapper(p(i))) 'p(i))
            Next
        End If
    End Sub

#End Region

#Region "Draw"

    Public Overrides Sub Draw()
        'Draw3DModel()
        DrawMainWindow()
        DrawPokemonStatus()

        DrawTopBar()
        DrawTeamWindow()

        If Me.MenuVisible = False Then
            DrawCursor()
        Else
            DrawMenuEntries()
        End If

    End Sub

    Private Sub DrawTopBar()
        Dim b As Box = Nothing
        Dim boxIndex As Integer = 0
        If BoxChooseMode = True Then
            If CursorPosition.X < 6 And CursorPosition.Y > 0 Then
                boxIndex = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)
            Else
                boxIndex = CurrentBox
            End If
        Else
            boxIndex = Me.CurrentBox
        End If
        b = GetBox(boxIndex)

        If Not b Is Nothing Then
            If b.IsBattleBox = True Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Box\BattleBox"), New Rectangle(80, 50, 600, 100), Color.White)

                Dim cArr(0) As Color
                TextureManager.GetTexture("GUI\Box\BattleBox", New Rectangle(0, 0, 1, 1), "").GetData(cArr)
                Canvas.DrawScrollBar(New Vector2(80, 36), Me.Boxes.Count, 1, boxIndex, New Size(600, 14), True, New Color(0, 0, 0, 0), cArr(0))
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Box\" & b.Background.ToString()), New Rectangle(80, 50, 600, 100), Color.White)

                Dim cArr(0) As Color
                TextureManager.GetTexture("GUI\Box\" & b.Background, New Rectangle(0, 0, 1, 1), "").GetData(cArr)
                Canvas.DrawScrollBar(New Vector2(80, 36), Me.Boxes.Count, 1, boxIndex, New Size(600, 14), True, New Color(0, 0, 0, 0), cArr(0))
            End If


            Core.SpriteBatch.DrawString(FontManager.MainFont, b.Name, New Vector2(384 - FontManager.MainFont.MeasureString(b.Name).X, 80), Color.Black, 0.0F, New Vector2(0), 2, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.DrawString(FontManager.MainFont, b.Name, New Vector2(380 - FontManager.MainFont.MeasureString(b.Name).X, 76), Color.White, 0.0F, New Vector2(0), 2, SpriteEffects.None, 0.0F)

            Core.SpriteBatch.Draw(Me.menuTexture, New Rectangle(10, 52, 96, 96), New Rectangle(0, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.menuTexture, New Rectangle(655, 52, 96, 96), New Rectangle(0, 16, 16, 16), Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
        End If
    End Sub

    Private Sub DrawMainWindow()
        If BoxChooseMode = True Then
            Canvas.DrawRectangle(Core.windowSize, New Color(220, 220, 220))

            For x = 0 To 5
                For y = 0 To 4
                    Dim id As Integer = y * 6 + x

                    If Me.Boxes.Count - 1 >= id Then
                        Dim pCount As Integer = BoxPokemonCount(id, True)

                        Dim tCoord As New Vector2(64, 0)
                        If pCount = 0 Then
                            tCoord = New Vector2(64, 32)
                        ElseIf pCount = 30 Then
                            tCoord = New Vector2(32, 32)
                        End If

                        Core.SpriteBatch.Draw(Me.texture, New Rectangle(50 + x * 100, 200 + y * 84, 64, 64), New Rectangle(CInt(tCoord.X), CInt(tCoord.Y), 32, 32), Color.White)
                    End If
                Next
            Next
        Else
            If GetBox(CurrentBox).IsBattleBox = True Then
                Canvas.DrawGradient(Core.windowSize, New Color(203, 40, 41), New Color(238, 128, 128), False, -1)

                Dim cArr(0) As Color
                TextureManager.GetTexture("GUI\Box\BattleBox", New Rectangle(0, 0, 1, 1), "").GetData(cArr)

                For i = 0 To 5
                    Dim id As Integer = i
                    Dim x As Integer = i + 2
                    Dim y As Integer = 0
                    While x > 3
                        x -= 2
                        y += 1
                    End While
                    Canvas.DrawRectangle(New Rectangle(50 + x * 100, 200 + y * 84, 64, 64), New Color(cArr(0).R, cArr(0).G, cArr(0).B, 150))

                    Dim box As Box = GetBox(CurrentBox)
                    If box.Pokemon.Keys.Contains(id) = True Then
                        Dim c As Color = Color.White
                        If IsLit(box.Pokemon(id).GetPokemon()) = False Then
                            c = New Color(65, 65, 65, 255)
                        End If

                        Core.SpriteBatch.Draw(box.Pokemon(id).GetPokemon().GetMenuTexture(), New Rectangle(50 + x * 100, 200 + y * 84, 64, 64), c)
                    End If
                Next
            Else
                Dim xt As Integer = GetBox(CurrentBox).Background
                Dim yt As Integer = 0

                While xt > 7
                    xt -= 8
                    yt += 1
                End While

                For x = 0 To Core.windowSize.Width Step 64
                    For y = 0 To Core.windowSize.Height Step 64
                        Core.SpriteBatch.Draw(Me.texture, New Rectangle(x, y, 64, 64), New Rectangle(xt * 16, yt * 16 + 64, 16, 16), Color.White)
                    Next
                Next

                Dim cArr(0) As Color
                TextureManager.GetTexture("GUI\Box\" & GetBox(CurrentBox).Background, New Rectangle(0, 0, 1, 1), "").GetData(cArr)
                For x = 0 To 5
                    For y = 0 To 4
                        Dim id As Integer = y * 6 + x

                        Canvas.DrawRectangle(New Rectangle(50 + x * 100, 200 + y * 84, 64, 64), New Color(cArr(0).R, cArr(0).G, cArr(0).B, 150))

                        Dim box As Box = GetBox(CurrentBox)
                        If box.Pokemon.Keys.Contains(id) = True Then
                            Dim c As Color = Color.White
                            If IsLit(box.Pokemon(id).GetPokemon()) = False Then
                                c = New Color(65, 65, 65, 255)
                            End If

                            Core.SpriteBatch.Draw(box.Pokemon(id).GetPokemon().GetMenuTexture(), New Rectangle(50 + x * 100, 200 + y * 84, 64, 64), c)
                        End If
                    Next
                Next
            End If
        End If
    End Sub

    Dim yOffset As Integer = 0

    Private Sub DrawPokemonStatus()
        If Me.BoxChooseMode = True And CursorPosition.X < 6 And CursorPosition.Y > 0 Then
            Dim box As Box = GetBox(CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))

            If Not box Is Nothing Then
                Canvas.DrawRectangle(New Rectangle(660, 200, 200, 200), New Color(84, 198, 216, 150))

                Dim minLevel As Integer = -1
                Dim maxLevel As Integer = -1

                For x = 0 To 5
                    For y = 0 To 4
                        Dim id As Integer = y * 6 + x

                        If box.Pokemon.Keys.Contains(id) = True Then
                            Dim c As Color = Color.White
                            If IsLit(box.Pokemon(id).GetPokemon()) = False Then
                                c = New Color(65, 65, 65, 255)
                            End If

                            Core.SpriteBatch.Draw(box.Pokemon(id).GetPokemon().GetMenuTexture(), New Rectangle(664 + x * 32, 215 + y * 32, 32, 32), c)

                            If box.Pokemon(id).GetPokemon().Level < minLevel Or minLevel = -1 Then
                                minLevel = box.Pokemon(id).GetPokemon().Level
                            End If
                            If box.Pokemon(id).GetPokemon().Level > maxLevel Or maxLevel = -1 Then
                                maxLevel = box.Pokemon(id).GetPokemon().Level
                            End If
                        End If
                    Next
                Next

                Canvas.DrawRectangle(New Rectangle(660, 410, 200, 210), New Color(84, 198, 216, 150))

                Dim levelString As String = minLevel & " - " & maxLevel
                If minLevel = -1 Or maxLevel = -1 Then
                    levelString = "None"
                End If

                Dim maxPokemon As Integer = 30
                If box.IsBattleBox = True Then
                    maxPokemon = 6
                End If

                Dim t As String = "BOX:  " & box.Name & vbNewLine & "POKéMON:  " & box.Pokemon.Count & " / " & maxPokemon & vbNewLine & "LEVEL:  " & levelString

                Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(667, 417), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(665, 415), Color.White)
            End If
        Else
            Dim p As Pokemon = Nothing

            If Not Me.MovingPokemon Is Nothing Then
                p = Me.MovingPokemon
            Else
                If CursorPosition.X = 6 Then
                    If Core.Player.Pokemons.Count - 1 >= CursorPosition.Y Then
                        p = Core.Player.Pokemons(CInt(CursorPosition.Y))
                    End If
                Else
                    Dim id As Integer = CInt(Me.CursorPosition.X) + CInt((Me.CursorPosition.Y - 1) * 6)

                    If GetBox(CurrentBox).IsBattleBox = True Then
                        id = GetBattleBoxID()
                    End If

                    If GetBox(CurrentBox).Pokemon.Keys.Contains(id) = True Then
                        p = GetBox(CurrentBox).Pokemon(id).GetPokemon()
                    End If
                End If
            End If

            If Not p Is Nothing Then
                Dim cArr(0) As Color

                If GetBox(CurrentBox).IsBattleBox = True Then
                    TextureManager.GetTexture("GUI\Box\BattleBox", New Rectangle(0, 0, 1, 1), "").GetData(cArr)
                Else
                    TextureManager.GetTexture("GUI\Box\" & GetBox(CurrentBox).Background, New Rectangle(0, 0, 1, 1), "").GetData(cArr)
                End If

                Dim c As Color = New Color(cArr(0).R, cArr(0).G, cArr(0).B, 150)
                If BoxChooseMode = True Then
                    c = New Color(84, 198, 216, 150)
                End If

                Canvas.DrawRectangle(New Rectangle(660, 200, 200, 200), c)

                Dim modelName As String = p.AnimationName
                Dim shinyString As String = "Normal"
                If p.IsShiny = True Then
                    shinyString = "Shiny"
                End If
                If Core.Player.ShowModelsInBattle = True AndAlso ModelManager.ModelExist("Models\" & modelName & "\" & shinyString) = True And p.IsEgg() = False Then
                    Draw3DModel(p, "Models\" & modelName & "\" & shinyString)
                Else
                    GetYOffset(p)
                    Core.SpriteBatch.Draw(p.GetTexture(True), New Rectangle(634, 180 - yOffset, 256, 256), Color.White)
                End If

                Canvas.DrawRectangle(New Rectangle(660, 410, 200, 210), c)

                If p.IsEgg() = True Then
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "EGG", New Vector2(667, 417), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "EGG", New Vector2(665, 415), Color.White)
                Else
                    Dim itemString As String = "None"
                    If Not p.Item Is Nothing Then
                        itemString = p.Item.Name
                    End If

                    Dim nameString As String = p.GetDisplayName() & "/" & p.OriginalName
                    If p.NickName = "" Then
                        nameString = p.GetDisplayName()
                    End If

                    Dim t As String = nameString & vbNewLine &
                                                 "DEX NO. " & p.Number & vbNewLine &
                                                 "LEVEL  " & p.Level & vbNewLine &
                                                 "HP  " & p.HP & " / " & p.MaxHP & vbNewLine &
                                                 "ATTACK  " & p.Attack & vbNewLine &
                                                 "DEFENSE  " & p.Defense & vbNewLine &
                                                 "SP. ATK  " & p.SpAttack & vbNewLine &
                                                 "SP. DEF  " & p.SpDefense & vbNewLine &
                                                 "SPEED  " & p.Speed & vbNewLine &
                                                 "ITEM  " & itemString

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(667, 417), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(665, 415), Color.White)
                End If
            End If
        End If
    End Sub

    Private Sub Draw3DModel(ByVal p As Pokemon, ByVal modelName As String)
        Dim propList = p.GetModelProperties()

        Dim scale As Single = propList.Item1
        Dim x As Single = propList.Item2
        Dim y As Single = propList.Item3
        Dim z As Single = propList.Item4

        Dim roll As Single = propList.Item5

        Dim t As Texture2D = ModelManager.DrawModelToTexture(modelName, New Vector2(1200, 680), New Vector3(x, y, z), New Vector3(0.0F, 50.0F, 10.0F), New Vector3(0.0F, 0.2F, roll + modelRoll), scale, True)
        Core.SpriteBatch.Draw(t, New Rectangle(160, 50, 1200, 680), Color.White)
    End Sub

    Private Sub DrawTeamWindow()
        Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 310, 0, 400, Core.windowSize.Height), New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.menuTexture, New Rectangle(Core.windowSize.Width - 128, y + StorageSystemScreen.TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 430, 0, 128, CInt(Core.windowSize.Height / 2)), New Rectangle(96, 0, 32, 64), Color.White)
        Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 430, CInt(Core.windowSize.Height / 2), 128, CInt(Core.windowSize.Height / 2)), New Rectangle(96, 0, 32, 64), Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipVertically, 0.0F)

        For i = 0 To 5
            Canvas.DrawBorder(2, New Rectangle(Core.windowSize.Width - 260, i * 100 + 50, 128, 80), New Color(42, 167, 198))

            If Core.Player.Pokemons.Count - 1 >= i Then
                Dim c As Color = Color.White
                If IsLit(Core.Player.Pokemons(i)) = False Then
                    c = New Color(65, 65, 65, 255)
                End If

                Core.SpriteBatch.Draw(Core.Player.Pokemons(i).GetMenuTexture(), New Rectangle(Core.windowSize.Width - 228, i * 100 + 60, 64, 64), c)

                If Not Core.Player.Pokemons(i).Item Is Nothing And Core.Player.Pokemons(i).IsEgg() = False Then
                    Core.SpriteBatch.Draw(Core.Player.Pokemons(i).Item.Texture, New Rectangle(Core.windowSize.Width - 196, i * 100 + 92, 24, 24), Color.White)
                End If
            End If
        Next
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = GetAbsoluteCursorPosition(Me.CursorPosition)

        If CursorMoving = True Then
            cPosition = CursorMovePosition
        End If

        If Not Me.MovingPokemon Is Nothing Then
            Core.SpriteBatch.Draw(Me.MovingPokemon.GetMenuTexture(), New Rectangle(CInt(cPosition.X - 10), CInt(cPosition.Y + 44), 64, 64), New Color(0, 0, 0, 150))
            Core.SpriteBatch.Draw(Me.MovingPokemon.GetMenuTexture(), New Rectangle(CInt(cPosition.X - 20), CInt(cPosition.Y + 34), 64, 64), Color.White)

            If Not Me.MovingPokemon.Item Is Nothing And Me.MovingPokemon.IsEgg() = False Then
                Core.SpriteBatch.Draw(Me.MovingPokemon.Item.Texture, New Rectangle(CInt(cPosition.X - 20) + 32, CInt(cPosition.Y + 34) + 32, 24, 24), Color.White)
            End If
        End If

        Dim t As Texture2D = GetCursorTexture()
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Private Sub DrawMenuEntries()
        If Me.MenuHeader <> "" Then
            Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 370, 100, 356, 64), New Color(0, 0, 0, 180))
            Core.SpriteBatch.DrawString(FontManager.MiniFont, MenuHeader, New Vector2(Core.windowSize.Width - 192 - FontManager.MiniFont.MeasureString(MenuHeader).X / 2, 120), Color.White)
        End If

        For Each e As MenuEntry In Me.MenuEntries
            e.Draw(Me.MenuCursor, GetCursorTexture())
        Next
    End Sub

    Private Function GetCursorTexture() As Texture2D
        Select Case Me.SelectionMode
            Case SelectionModes.SingleMove
                Return TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Case SelectionModes.EasyMove
                Return TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 0, 16, 16), "")
            Case SelectionModes.Deposit
                Return TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 0, 16, 16), "")
            Case SelectionModes.Withdraw
                Return TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 32, 16, 16), "")
        End Select

        Return Nothing
    End Function

#End Region

    Private Function IsLit(ByVal p As Pokemon) As Boolean
        If Me.Filters.Count > 0 Then
            If p.IsEgg() = True Then
                Return False
            End If
            For Each f As Filter In Filters
                Select Case f.FilterType
                    Case FilterTypes.Ability
                        If p.Ability.Name.ToLower() <> f.FilterValue.ToLower() Then
                            Return False
                        End If
                    Case FilterTypes.Gender
                        If p.Gender.ToString().ToLower() <> f.FilterValue.ToLower() Then
                            Return False
                        End If
                    Case FilterTypes.HeldItem
                        If f.FilterValue = "Has no Held Item" And Not p.Item Is Nothing Then
                            Return False
                        ElseIf f.FilterValue = "Has a Held Item" And p.Item Is Nothing Then
                            Return False
                        End If
                    Case FilterTypes.Move
                        Dim hasAttack As Boolean = False
                        For Each a As BattleSystem.Attack In p.Attacks
                            If a.Name.ToLower() = f.FilterValue.ToLower() Then
                                hasAttack = True
                                Exit For
                            End If
                        Next
                        If hasAttack = False Then
                            Return False
                        End If
                    Case FilterTypes.Nature
                        If p.Nature.ToString().ToLower() <> f.FilterValue.ToLower() Then
                            Return False
                        End If
                    Case FilterTypes.Pokémon
                        If p.GetName() <> f.FilterValue Then
                            Return False
                        End If
                    Case FilterTypes.Type1
                        Dim t As Element = New Element(f.FilterValue)
                        If p.Type1.Type <> t.Type Then
                            Return False
                        End If
                    Case FilterTypes.Type2
                        Dim t As Element = New Element(f.FilterValue)
                        If p.Type2.Type <> t.Type Then
                            Return False
                        End If
                End Select
            Next
        End If

        Return True
    End Function

    ''' <summary>
    ''' Adds a pokemon to the next free spot and returns the index of that box
    ''' </summary>
    Public Shared Function DepositPokemon(ByVal p As Pokemon, Optional ByVal BoxIndex As Integer = -1) As Integer
        p.FullRestore()

        Dim Boxes As List(Of Box) = LoadBoxes()
        Dim startIndex As Integer = 0
        If BoxIndex > -1 Then
            startIndex = BoxIndex
        End If

        For i = startIndex To Boxes.Count - 1
            If GetBox(i, Boxes).Pokemon.Count < 30 Then
                For l = 0 To 29
                    If GetBox(i, Boxes).Pokemon.Keys.Contains(l) = False Then
                        GetBox(i, Boxes).Pokemon.Add(l, New PokemonWrapper(p)) 'p)
                        Exit For
                    End If
                Next
                Core.Player.BoxData = GetBoxSaveData(Boxes)
                Return i
            End If
        Next

        If startIndex <> 0 Then
            For i = 0 To startIndex - 1
                If GetBox(i, Boxes).Pokemon.Count < 30 Then
                    For l = 0 To 29
                        If GetBox(i, Boxes).Pokemon.Keys.Contains(l) = False Then
                            GetBox(i, Boxes).Pokemon.Add(l, New PokemonWrapper(p)) 'p)
                            Exit For
                        End If
                    Next
                    Core.Player.BoxData = GetBoxSaveData(Boxes)
                    Return i
                End If
            Next
        End If

        Return -1
    End Function

    Public Shared Function GetBoxName(ByVal boxIndex As Integer) As String
        Return GetBox(boxIndex, LoadBoxes()).Name
    End Function

    Private Shared Function GetBox(ByVal index As Integer, ByVal boxes As List(Of Box)) As Box
        For Each b As Box In boxes
            If b.index = index Then
                Return b
            End If
        Next

        Return Nothing
    End Function

    Private Function GetBox(ByVal index As Integer) As Box
        For Each b As Box In Me.Boxes
            If b.index = index Then
                Return b
            End If
        Next

        Return Nothing
    End Function

    Private Function BoxPokemonCount(ByVal selBox As Integer, ByVal lit As Boolean) As Integer
        Dim c As Integer = 0

        Dim box As Box = GetBox(selBox)
        If Not box Is Nothing Then
            For Each p As PokemonWrapper In box.Pokemon.Values
                If lit = True Then
                    If IsLit(p.GetPokemon()) = True Then
                        c += 1
                    End If
                Else
                    c += 1
                End If
            Next
        End If

        Return c
    End Function

    Private Sub SetupMenu(ByVal entries() As MenuEntry, ByVal header As String)
        Me.MenuEntries.Clear()
        Me.MenuEntries.AddRange(entries)
        Me.MenuVisible = True
        Me.MenuCursor = MenuEntries(0).Index
        Me.MenuHeader = header
    End Sub

    Public Class PokemonWrapper

        Private _pokemon As Pokemon = Nothing
        Private _pokemonData As String
        Private _loaded As Boolean = False

        Public Sub New(ByVal PokemonData As String)
            Me._pokemonData = PokemonData
        End Sub

        Public Sub New(ByVal p As Pokemon)
            Me._loaded = True
            Me._pokemon = p
            Me._pokemonData = p.GetSaveData()
        End Sub

        Public Function GetPokemon() As Pokemon
            If _loaded = False Then
                _loaded = True
                _pokemon = Pokemon.GetPokemonByData(Me._pokemonData)
            End If
            Return Me._pokemon
        End Function

        Public ReadOnly Property PokemonData() As String
            Get
                If _loaded = True Then
                    Return Me._pokemon.GetSaveData()
                Else
                    Return Me._pokemonData
                End If
            End Get
        End Property

    End Class

    Class Box

        Public index As Integer = 0
        Public Name As String = "BOX 0"

        Public Pokemon As New Dictionary(Of Integer, PokemonWrapper)
        Public Background As Integer = 0
        Public isSelected As Boolean = False

        Private _isBattleBox As Boolean = False

        Public Sub New(ByVal index As Integer)
            Me.index = index
            Me.Name = "BOX " & (index + 1).ToString()
            Me.Background = index
        End Sub

        Public ReadOnly Property HasPokemon() As Boolean
            Get
                Return (Pokemon.Count > 0)
            End Get
        End Property

        Public Function GetPokemonList() As List(Of Pokemon)
            Dim l As New List(Of Pokemon)
            For i = 0 To Pokemon.Count - 1
                l.Add(Pokemon.Values(i).GetPokemon())
            Next
            Return l
        End Function

        Public Property IsBattleBox() As Boolean
            Get
                Return Me._isBattleBox
            End Get
            Set(value As Boolean)
                Me._isBattleBox = value
                If Me._isBattleBox = True Then
                    Me.Name = "BATTLE BOX"
                End If
            End Set
        End Property

    End Class

    Class MenuEntry

        Public Index As Integer = 0
        Public TAG As Object = Nothing

        Public Text As String = "Menu"
        Public IsBack As Boolean = False
        Public Delegate Sub ClickEvent(ByVal m As MenuEntry)
        Public ClickHandler As ClickEvent = Nothing

        Dim t1 As Texture2D
        Dim t2 As Texture2D

        Public Sub New(ByVal Index As Integer, ByVal text As String, ByVal isBack As Boolean, ByVal ClickHandler As ClickEvent)
            Me.New(Index, text, isBack, ClickHandler, Nothing)
        End Sub

        Public Sub New(ByVal Index As Integer, ByVal text As String, ByVal isBack As Boolean, ByVal ClickHandler As ClickEvent, ByVal TAG As Object)
            Me.Index = Index
            Me.TAG = TAG

            Me.Text = text
            Me.IsBack = isBack
            Me.ClickHandler = ClickHandler

            t1 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), "")
            t2 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), "")
        End Sub

        Public Sub Update(ByVal s As StorageSystemScreen)
            If Controls.Accept(True, False, False) = True And s.MenuCursor = Me.Index And New Rectangle(Core.windowSize.Width - 270, 66 * Index, 256, 64).Contains(MouseHandler.MousePosition) = True Or
                Controls.Accept(False, True, True) = True And s.MenuCursor = Me.Index Or Controls.Dismiss(True, True, True) = True And Me.IsBack = True Then
                s.MenuVisible = False
                If Not ClickHandler Is Nothing Then
                    ClickHandler(Me)
                End If
            End If
            If New Rectangle(Core.windowSize.Width - 270, 66 * Index, 256, 64).Contains(MouseHandler.MousePosition) = True And Controls.Accept(True, False, False) = True Then
                s.MenuCursor = Me.Index
            End If
        End Sub

        Public Sub Draw(ByVal CursorIndex As Integer, ByVal CursorTexture As Texture2D)
            Dim startPos As New Vector2(Core.windowSize.Width - 270, 66 * Index)

            Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X), CInt(startPos.Y), 64, 64), Color.White)
            Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 64), CInt(startPos.Y), 64, 64), Color.White)
            Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 128), CInt(startPos.Y), 64, 64), Color.White)
            Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X + 192), CInt(startPos.Y), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Text, New Vector2(startPos.X + 128 - (FontManager.MainFont.MeasureString(Me.Text).X * 1.4F) / 2, startPos.Y + 15), Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

            If Me.Index = CursorIndex Then
                Dim cPosition As Vector2 = New Vector2(startPos.X + 128, startPos.Y - 40)
                Dim t As Texture2D = CursorTexture
                Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
            End If
        End Sub

    End Class

    Public Shared Function GetAllBoxPokemon() As List(Of Pokemon)
        Dim Pokemons As New List(Of Pokemon)
        Dim Data() As String = Core.Player.BoxData.SplitAtNewline()
        For Each line As String In Data
            If line.StartsWith("BOX|") = False And line <> "" Then
                Dim pokeData As String = line.Remove(0, line.IndexOf("{"))
                Pokemons.Add(Pokemon.GetPokemonByData(pokeData))
            End If
        Next
        Return Pokemons
    End Function

    Public Function GetPokemonList(ByVal includeTeam As Boolean, ByVal lit As Boolean) As List(Of Pokemon)
        Dim L As New List(Of Pokemon)
        For Each Box As Box In Me.Boxes
            If Box.HasPokemon = True Then
                For i = 0 To Box.Pokemon.Count - 1
                    If (lit = True AndAlso IsLit(Box.Pokemon.Values(i).GetPokemon()) = True) = True Or lit = False Then
                        L.Add(Box.Pokemon.Values(i).GetPokemon())
                    End If
                Next
            End If
        Next

        If includeTeam = True Then
            For Each p As Pokemon In Core.Player.Pokemons
                If (lit = True AndAlso IsLit(p) = True) = True Or lit = False Then
                    L.Add(p)
                End If
            Next
        End If

        Return L
    End Function

    Public Shared Function GetBattleBoxPokemon() As List(Of Pokemon)
        Dim BattleBoxID As Integer = 0
        Dim Data() As String = Core.Player.BoxData.SplitAtNewline()
        Dim PokemonList As New List(Of Pokemon)

        For Each line As String In Data
            If line.StartsWith("BOX|") = True Then
                Dim boxData() As String = line.Split(CChar("|"))
                If CInt(boxData(1)) > BattleBoxID Then
                    BattleBoxID = CInt(boxData(1))
                End If
            End If
        Next
        For Each line As String In Data
            If line.StartsWith(BattleBoxID.ToString() & ",") = True And line.EndsWith("}") = True Then
                Dim pokemonData As String = line.Remove(0, line.IndexOf("{"))
                PokemonList.Add(Pokemon.GetPokemonByData(pokemonData))
            End If
        Next

        'Prevent more than 6 pokemon:
        While PokemonList.Count > 6
            PokemonList.RemoveAt(PokemonList.Count - 1)
        End While

        Return PokemonList
    End Function

End Class

Public Class StorageSystemFilterScreen

    Inherits Screen

    Private Class SelectMenu

        Dim Items As New List(Of String)
        Dim Index As Integer = 0
        Public Delegate Sub ClickEvent(ByVal s As SelectMenu)
        Dim ClickHandler As ClickEvent = Nothing
        Dim BackIndex As Integer = 0
        Public Visible As Boolean = True
        Public Scroll As Integer = 0

        Dim t1 As Texture2D
        Dim t2 As Texture2D

        Public Sub New(ByVal Items As List(Of String), ByVal Index As Integer, ByVal ClickHandle As ClickEvent, ByVal BackIndex As Integer)
            Me.Items = Items
            Me.Index = Index
            Me.ClickHandler = ClickHandle
            Me.BackIndex = BackIndex
            If Me.BackIndex < 0 Then
                Me.BackIndex = Me.Items.Count + Me.BackIndex
            End If
            Me.Visible = True

            t1 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), "")
            t2 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), "")
        End Sub

        Public Sub Update()
            If Visible = True Then
                If Controls.Up(True, True, True, True, True, True) = True Then
                    Me.Index -= 1
                End If
                If Controls.Down(True, True, True, True, True, True) = True Then
                    Me.Index += 1
                End If
                Me.Index = Me.Index.Clamp(0, Me.Items.Count - 1)

                For i = Scroll To Me.Scroll + 8
                    If i <= Me.Items.Count - 1 Then
                        If Controls.Accept(True, False, False) = True And i = Me.Index And New Rectangle(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll), 256, 64).Contains(MouseHandler.MousePosition) = True Or
                            Controls.Accept(False, True, True) = True And i = Me.Index Or Controls.Dismiss(True, True, True) = True And Me.BackIndex = Me.Index Then

                            If Not ClickHandler Is Nothing Then
                                ClickHandler(Me)
                            End If
                            Me.Visible = False
                        End If
                        If Controls.Dismiss(True, True, True) = True Then
                            Me.Index = Me.BackIndex
                            If Not ClickHandler Is Nothing Then
                                ClickHandler(Me)
                            End If
                            Me.Visible = False
                        End If
                        If New Rectangle(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll), 256, 64).Contains(MouseHandler.MousePosition) = True And Controls.Accept(True, False, False) = True Then
                            Me.Index = i
                        End If
                    End If
                Next

                If Index - Scroll > 8 Then
                    Scroll = Index - 8
                End If
                If Index - Scroll < 0 Then
                    Scroll = Index
                End If
            End If
        End Sub

        Public Sub Draw()
            If Visible = True Then
                For i = Scroll To Me.Scroll + 8
                    If i <= Me.Items.Count - 1 Then
                        Dim Text As String = Items(i)

                        Dim startPos As New Vector2(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll))

                        Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 64), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 128), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X + 192), CInt(startPos.Y), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(startPos.X + 128 - (FontManager.MainFont.MeasureString(Text).X * 1.4F) / 2, startPos.Y + 15), Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

                        If Me.Index = i Then
                            Dim cPosition As Vector2 = New Vector2(startPos.X + 128, startPos.Y - 40)
                            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
                            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
                        End If
                    End If
                Next
            End If
        End Sub

        Public ReadOnly Property SelectedItem() As String
            Get
                Return Items(Me.Index)
            End Get
        End Property

    End Class

    Private _storageSystemScreen As StorageSystemScreen
    Dim texture As Texture2D

    Dim Filters As New List(Of StorageSystemScreen.Filter)
    Dim Menu As SelectMenu
    Dim mainMenuItems As New List(Of String)
    Dim Cursor As Integer = 0
    Dim Scroll As Integer = 0

    Dim Results As Integer = 0
    Dim CalculatedFilters As New List(Of StorageSystemScreen.Filter)

    Public Sub New(ByVal currentScreen As StorageSystemScreen)
        Me.Identification = Identifications.StorageSystemFilterScreen
        Me._storageSystemScreen = currentScreen

        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        For Each Filter As StorageSystemScreen.Filter In currentScreen.Filters
            Me.Filters.Add(Filter)
        Next

        Me.MouseVisible = True
        Me.CanMuteMusic = True
        Me.CanBePaused = True

        Me.mainMenuItems = {"Pokémon", "Type1", "Type2", "Move", "Ability", "Nature", "Gender", "HeldItem"}.ToList()

        Me.Menu = New SelectMenu({""}.ToList(), 0, Nothing, 0)
        Me.Menu.Visible = False
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + StorageSystemScreen.TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198), False, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Configure the filters:", New Vector2(100, 24), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        For i = Scroll To Scroll + 5
            If i <= Me.mainMenuItems.Count - 1 Then
                Dim p As Integer = i - Scroll

                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 100 + p * 96, 64 * 8, 64), New Rectangle(32, 16, 16, 16), Color.White)
                Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 9, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Dim s As String = mainMenuItems(i)
                If GetFilterText(mainMenuItems(i)) <> "" Then
                    s &= " (" & GetFilterText(mainMenuItems(i)) & ")"
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(120, 116 + p * 96, 32, 32), New Rectangle(16, 48, 16, 16), Color.White)
                Else
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(120, 116 + p * 96, 32, 32), New Rectangle(16, 32, 16, 16), Color.White)
                End If

                Core.SpriteBatch.DrawString(FontManager.MainFont, s, New Vector2(160, 116 + p * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
            End If
        Next

        If Filters.Count > 0 Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Results: " & vbNewLine & vbNewLine & "Filters: ", New Vector2(90 + 64 * 11, 119), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Results & vbNewLine & vbNewLine & Me.Filters.Count, New Vector2(190 + 64 * 11, 119), Color.White)
        End If

        If Menu.Visible = True Then
            Menu.Draw()
        Else
            DrawCursor()
        End If
    End Sub

    Private Function GetFilterText(ByVal filterTypeString As String) As String
        For Each f As StorageSystemScreen.Filter In Me.Filters
            If f.FilterType.ToString().ToLower() = filterTypeString.ToLower() Then
                Return f.FilterValue
            End If
        Next
        Return ""
    End Function

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(520, 100 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Private Sub ApplyFilters()
        Me._storageSystemScreen.Filters.Clear()
        For Each f As StorageSystemScreen.Filter In Me.Filters
            Me._storageSystemScreen.Filters.Add(f)
        Next
    End Sub

    Public Overrides Sub Update()
        If Menu.Visible = True Then
            Menu.Update()
        Else
            If Controls.Down(True, True, True, True, True, True) = True Then
                Me.Cursor += 1
                If Controls.ShiftDown() = True Then
                    Me.Cursor += 4
                End If
            End If
            If Controls.Up(True, True, True, True, True, True) = True Then
                Me.Cursor -= 1
                If Controls.ShiftDown() = True Then
                    Me.Cursor -= 4
                End If
            End If

            While Me.Cursor > 5
                Me.Cursor -= 1
                Me.Scroll += 1
            End While
            While Me.Cursor < 0
                Me.Cursor += 1
                Me.Scroll -= 1
            End While

            If Me.mainMenuItems.Count < 7 Then
                Me.Scroll = 0
            Else
                Me.Scroll = Me.Scroll.Clamp(0, Me.mainMenuItems.Count - 6)
            End If

            If Me.mainMenuItems.Count < 6 Then
                Me.Cursor = Me.Cursor.Clamp(0, Me.mainMenuItems.Count - 1)
            Else
                Me.Cursor = Me.Cursor.Clamp(0, 5)
            End If

            If Me.mainMenuItems.Count > 0 Then
                If Controls.Accept(True, False, False) = True Then
                    For i = Scroll To Scroll + 5
                        If i <= Me.mainMenuItems.Count - 1 Then
                            If New Rectangle(100, 100 + (i - Scroll) * 96, 640, 64).Contains(MouseHandler.MousePosition) = True Then
                                If i = Cursor + Scroll Then
                                    SelectFilter()
                                Else
                                    Cursor = i - Scroll
                                End If
                            End If
                        End If
                    Next
                End If

                If Controls.Accept(False, True, True) = True Then
                    SelectFilter()
                End If
            End If

            If Controls.Dismiss(True, True, True) = True Then
                ApplyFilters()
                Core.SetScreen(Me._storageSystemScreen)
            End If
        End If

        CalculateResults()

        StorageSystemScreen.TileOffset += 1
        If StorageSystemScreen.TileOffset >= 64 Then
            StorageSystemScreen.TileOffset = 0
        End If
    End Sub

    Private Sub CalculateResults()
        Dim s As String = ""
        Dim s1 As String = ""
        For Each f As StorageSystemScreen.Filter In Me.CalculatedFilters
            s &= f.FilterType.ToString() & "|" & f.FilterValue
        Next
        For Each f As StorageSystemScreen.Filter In Me.Filters
            s1 &= f.FilterType.ToString() & "|" & f.FilterValue
        Next

        If s1 <> s Then
            Me.CalculatedFilters.Clear()
            For Each f As StorageSystemScreen.Filter In Me.Filters
                Me.CalculatedFilters.Add(f)
            Next
            ApplyFilters()
            Me.Results = Me._storageSystemScreen.GetPokemonList(True, True).Count
        End If
    End Sub

    Private Sub SelectFilter()
        Dim filterType As String = Me.mainMenuItems(Me.Scroll + Me.Cursor)

        Select Case filterType.ToLower()
            Case "pokémon"
                Me.OpenPokemonMenu()
            Case "type1"
                Me.OpenType1Menu()
            Case "type2"
                Me.OpenType2Menu()
            Case "move"
                Me.OpenMoveMenu()
            Case "ability"
                Me.OpenAbilityMenu()
            Case "nature"
                Me.OpenNatureMenu()
            Case "gender"
                Me.OpenGenderMenu()
            Case "helditem"
                Me.OpenHeldItemMenu()
        End Select
    End Sub

#Region "PokémonFilter"

    Private Sub OpenPokemonMenu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim letters As New List(Of String)

        For Each p As Pokemon In l
            If letters.Contains(p.GetName()(0).ToString().ToUpper()) = False Then
                letters.Add(p.GetName()(0).ToString().ToUpper())
            End If
        Next

        letters = (From letter As String In letters Order By letter Ascending).ToList()
        letters.Add("Back")

        If GetFilterText("Pokémon") <> "" Then
            letters.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(letters, 0, AddressOf SelectPokemonLetter, -1)
    End Sub

    Private Sub SelectPokemonLetter(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Pokémon Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                Dim chosenLetter As String = s.SelectedItem

                Dim pokemonList As New List(Of String)
                Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)

                For Each p As Pokemon In l
                    If p.GetName(0).ToString().ToUpper() = chosenLetter And pokemonList.Contains(p.GetName()) = False Then
                        pokemonList.Add(p.GetName)
                    End If
                Next

                pokemonList = (From name As String In pokemonList Order By name Ascending).ToList()

                pokemonList.Add("Back")

                Me.Menu = New SelectMenu(pokemonList, 0, AddressOf SelectPokemon, -1)
        End Select
    End Sub

    Private Sub SelectPokemon(ByVal s As SelectMenu)
        If s.SelectedItem <> "Back" Then
            For Each Filter As StorageSystemScreen.Filter In Me.Filters
                If Filter.FilterType = StorageSystemScreen.FilterTypes.Pokémon Then
                    Filters.Remove(Filter)
                    Exit For
                End If
            Next

            Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Pokémon, .FilterValue = s.SelectedItem})
        Else
            OpenPokemonMenu()
        End If
    End Sub

#End Region

#Region "Type1Filter"

    Private Sub OpenType1Menu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim types As New List(Of String)

        For Each p As Pokemon In l
            If types.Contains(p.Type1.Type.ToString()) = False Then
                types.Add(p.Type1.Type.ToString())
            End If
        Next

        types = (From type As String In types Order By type Ascending).ToList()
        types.Add("Back")

        If GetFilterText("Type1") <> "" Then
            types.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(types, 0, AddressOf SelectType1Type, -1)
    End Sub

    Private Sub SelectType1Type(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'Go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Type1 Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Type1 Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next

                Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Type1, .FilterValue = s.SelectedItem})
        End Select
    End Sub

#End Region

#Region "Type2Filter"

    Private Sub OpenType2Menu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim types As New List(Of String)

        For Each p As Pokemon In l
            If types.Contains(p.Type2.Type.ToString()) = False Then
                types.Add(p.Type2.Type.ToString())
            End If
        Next

        types = (From type As String In types Order By type Ascending).ToList()
        types.Add("Back")

        If GetFilterText("Type2") <> "" Then
            types.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(types, 0, AddressOf SelectType2Type, -1)
    End Sub

    Private Sub SelectType2Type(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'Go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Type2 Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Type2 Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next

                Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Type2, .FilterValue = s.SelectedItem})
        End Select
    End Sub

#End Region

#Region "MoveFilter"

    Private Sub OpenMoveMenu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim letters As New List(Of String)

        For Each p As Pokemon In l
            For Each a As BattleSystem.Attack In p.Attacks
                If letters.Contains(a.Name(0).ToString().ToUpper()) = False Then
                    letters.Add(a.Name(0).ToString().ToUpper())
                End If
            Next
        Next

        letters = (From letter As String In letters Order By letter Ascending).ToList()
        letters.Add("Back")

        If GetFilterText("Move") <> "" Then
            letters.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(letters, 0, AddressOf SelectMoveLetter, -1)
    End Sub

    Private Sub SelectMoveLetter(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Move Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                Dim chosenLetter As String = s.SelectedItem

                Dim attackList As New List(Of String)
                Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)

                For Each p As Pokemon In l
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name(0).ToString().ToUpper() = chosenLetter And attackList.Contains(a.Name) = False Then
                            attackList.Add(a.Name)
                        End If
                    Next
                Next

                attackList = (From name As String In attackList Order By name Ascending).ToList()

                attackList.Add("Back")

                Me.Menu = New SelectMenu(attackList, 0, AddressOf SelectMove, -1)
        End Select
    End Sub

    Private Sub SelectMove(ByVal s As SelectMenu)
        If s.SelectedItem <> "Back" Then
            For Each Filter As StorageSystemScreen.Filter In Me.Filters
                If Filter.FilterType = StorageSystemScreen.FilterTypes.Move Then
                    Filters.Remove(Filter)
                    Exit For
                End If
            Next

            Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Move, .FilterValue = s.SelectedItem})
        Else
            OpenMoveMenu()
        End If
    End Sub

#End Region

#Region "AbilityFilter"

    Private Sub OpenAbilityMenu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim letters As New List(Of String)

        For Each p As Pokemon In l
            If letters.Contains(p.Ability.Name(0).ToString().ToUpper()) = False Then
                letters.Add(p.Ability.Name(0).ToString().ToUpper())
            End If
        Next

        letters = (From letter As String In letters Order By letter Ascending).ToList()
        letters.Add("Back")

        If GetFilterText("Ability") <> "" Then
            letters.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(letters, 0, AddressOf SelectAbilityLetter, -1)
    End Sub

    Private Sub SelectAbilityLetter(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Ability Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                Dim chosenLetter As String = s.SelectedItem

                Dim abilityList As New List(Of String)
                Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)

                For Each p As Pokemon In l
                    If p.Ability.Name(0).ToString().ToUpper() = chosenLetter And abilityList.Contains(p.Ability.Name) = False Then
                        abilityList.Add(p.Ability.Name)
                    End If
                Next

                abilityList = (From name As String In abilityList Order By name Ascending).ToList()

                abilityList.Add("Back")

                Me.Menu = New SelectMenu(abilityList, 0, AddressOf SelectAbility, -1)
        End Select
    End Sub

    Private Sub SelectAbility(ByVal s As SelectMenu)
        If s.SelectedItem <> "Back" Then
            For Each Filter As StorageSystemScreen.Filter In Me.Filters
                If Filter.FilterType = StorageSystemScreen.FilterTypes.Ability Then
                    Filters.Remove(Filter)
                    Exit For
                End If
            Next

            Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Ability, .FilterValue = s.SelectedItem})
        Else
            OpenAbilityMenu()
        End If
    End Sub

#End Region

#Region "NatureFilter"

    Private Sub OpenNatureMenu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim natures As New List(Of String)

        For Each p As Pokemon In l
            If natures.Contains(p.Nature.ToString()) = False Then
                natures.Add(p.Nature.ToString())
            End If
        Next

        natures = (From nature As String In natures Order By nature Ascending).ToList()
        natures.Add("Back")

        If GetFilterText("Nature") <> "" Then
            natures.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(natures, 0, AddressOf SelectNatureType, -1)
    End Sub

    Private Sub SelectNatureType(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'Go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Nature Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Nature Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next

                Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Nature, .FilterValue = s.SelectedItem})
        End Select
    End Sub

#End Region

#Region "GenderFilter"

    Private Sub OpenGenderMenu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim genders As New List(Of String)

        For Each p As Pokemon In l
            If genders.Contains(p.Gender.ToString()) = False Then
                genders.Add(p.Gender.ToString())
            End If
        Next

        genders = (From gender As String In genders Order By gender Ascending).ToList()
        genders.Add("Back")

        If GetFilterText("Gender") <> "" Then
            genders.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(genders, 0, AddressOf SelectGenderType, -1)
    End Sub

    Private Sub SelectGenderType(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'Go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Gender Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.Gender Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next

                Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.Gender, .FilterValue = s.SelectedItem})
        End Select
    End Sub

#End Region

#Region "HeldItemFilter"

    Private Sub OpenHeldItemMenu()
        Dim l As List(Of Pokemon) = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim helditems As List(Of String) = {"Has a Held Item", "Has no Held Item", "Back"}.ToList()

        If GetFilterText("HeldItem") <> "" Then
            helditems.Insert(0, "Clear")
        End If

        Me.Menu = New SelectMenu(helditems, 0, AddressOf SelectHeldItemType, -1)
    End Sub

    Private Sub SelectHeldItemType(ByVal s As SelectMenu)
        Select Case s.SelectedItem
            Case "Back"
                'Go back
            Case "Clear"
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.HeldItem Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next
            Case Else
                For Each Filter As StorageSystemScreen.Filter In Me.Filters
                    If Filter.FilterType = StorageSystemScreen.FilterTypes.HeldItem Then
                        Filters.Remove(Filter)
                        Exit For
                    End If
                Next

                Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = StorageSystemScreen.FilterTypes.HeldItem, .FilterValue = s.SelectedItem})
        End Select
    End Sub

#End Region

End Class
