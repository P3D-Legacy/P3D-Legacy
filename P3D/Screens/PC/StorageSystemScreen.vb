Public Class StorageSystemScreen

    Inherits Screen

    Public Shared TileOffset As Integer = 0

    Private renderTarget As RenderTarget2D

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
        Me.renderTarget = New RenderTarget2D(Core.GraphicsDevice, 1200, 680, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.StorageSystemScreen
        Me.MouseVisible = True

        Me.CanBePaused = True
        Me.CanChat = True
        Me.CanMuteAudio = True

        Me.texture = TextureManager.GetTexture("GUI\Box\storage")
        Me.menuTexture = TextureManager.GetTexture("GUI\Menus\General")

        LoadScreen()
    End Sub

    Public Shared Function LoadBoxes() As List(Of Box)
        Dim boxes = New Dictionary(Of Integer, Box)

        For i = 0 To Core.Player.BoxAmount - 1
            boxes.Add(i, New Box(i))
        Next

        For Each line In Core.Player.BoxData.SplitAtNewline()
            If Not line.StartsWith("BOX") And line <> "" Then
                Dim Data = line.Split(",")

                Dim boxIndex = CInt(Data(0))
                Dim pokemonIndex = CInt(Data(1))
                Dim pokemonData = line.Remove(0, line.IndexOf("{"))
                Dim box As Box = Nothing

                If Not boxes.TryGetValue(boxIndex, box) Then
                    boxes.Add(boxIndex, New Box(boxIndex))
                End If

                If Not box.Pokemon.ContainsKey(pokemonIndex) Then
                    box.Pokemon.Add(pokemonIndex, New PokemonWrapper(pokemonData)) ' Pokemon.GetPokemonByData(pokemonData))
                End If
            ElseIf line.StartsWith("BOX") Then
                Dim boxData = line.Split("|")

                Dim boxIndex = CInt(boxData(1))
                If boxIndex > boxes.Count - 1 Then
                    Dim boxCount = boxes.Count + 5
                    If boxCount > 30 Then
                        boxCount = 30
                    End If
                    For b = boxes.Count - 1 To boxCount - 1
                        If boxes.ContainsKey(b) = False Then
                            boxes.Add(b, New Box(b))
                        End If
                    Next
                End If
                Dim box As Box = Nothing

                If Not boxes.TryGetValue(boxIndex, box) Then
                    boxes.Add(boxIndex, New Box(boxIndex))
                End If

                box.Background = CInt(boxData(3))
                box.Name = boxData(2)
            End If
        Next

        Dim bounds = (min:=boxes.Min(Function(x) x.Value.index), max:=boxes.Max(Function(x) x.Value.index))

        For i = bounds.min To bounds.max
            If Not boxes.ContainsKey(i) Then boxes.Add(i, New Box(i))
        Next
        boxes(bounds.max).IsBattleBox = True

        Return boxes.Values.ToList()
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
        TextBox.Update()

        If ControllerHandler.ButtonPressed(Buttons.Back) Or KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) Then
            Core.SetScreen(New StorageSystemFilterScreen(Me))
        End If

        If Not TextBox.Showing Then
            If MenuVisible Then
                For i = 0 To Me.MenuEntries.Count - 1
                    If i <= Me.MenuEntries.Count - 1 Then Me.MenuEntries(i).Update(Me)
                Next

                If Controls.Up(True, True) Then Me.MenuCursor -= 1
                If Controls.Down(True, True) Then Me.MenuCursor += 1

                Dim maxIndex = Me.MenuEntries.Max(Function(x) x.Index)
                Dim minIndex = Me.MenuEntries.Min(Function(x) x.Index)

                If Me.MenuCursor > maxIndex Then Me.MenuCursor = minIndex
                If Me.MenuCursor < minIndex Then Me.MenuCursor = maxIndex
            Else
                TurnModel()
                If CursorMoving Then
                    MoveCursor()
                Else
                    Dim rightInput = ControllerHandler.ButtonPressed(Buttons.RightTrigger) Or Controls.Right(True, False, True, False, False, False)
                    Dim leftInput = ControllerHandler.ButtonPressed(Buttons.LeftTrigger) Or Controls.Left(True, False, True, False, False, False)
                    If leftInput Then Me.CurrentBox -= 1
                    If rightInput Then Me.CurrentBox += 1
                    If CurrentBox < 0 Then CurrentBox = Me.Boxes.Count - 1
                    If CurrentBox > Me.Boxes.Count - 1 Then CurrentBox = 0

                    PressNumberButtons()

                    If GetRelativeMousePosition() <> New Vector2(-1) AndAlso GetRelativeMousePosition() = CursorPosition AndAlso Controls.Accept(True, False, False) Then
                        SoundManager.PlaySound("select")
                        ChooseObject()
                    End If

                    ControlCursor()

                    If Controls.Accept(False, True, True) Then
                        SoundManager.PlaySound("select")
                        ChooseObject()
                    End If

                    If Controls.Dismiss(True, True, True) Then
                        SoundManager.PlaySound("select")
                        CloseScreen()
                    End If
                End If
            End If

            StorageSystemScreen.TileOffset = (StorageSystemScreen.TileOffset + 1) Mod 64
        End If
    End Sub

    Private Sub TurnModel()
        If Controls.ShiftDown("L", False) Then modelRoll -= 0.1F
        If Controls.ShiftDown("R", False) Then modelRoll += 0.1F
        If Not ControllerHandler.ButtonDown(Buttons.RightThumbstickLeft Or Buttons.RightThumbstickRight) Then Return
        Dim gPadState = GamePad.GetState(PlayerIndex.One)
        modelRoll -= gPadState.ThumbSticks.Right.X * 0.1F
    End Sub

    Private Sub PressNumberButtons()
        Dim switchTo = If(KeyBoardHandler.KeyPressed(Keys.D0), 9, -1)
        If switchTo < 0 Then
            Dim keysPressed = KeyBoardHandler.GetPressedKeys.Where(Function(key) key >= Keys.D1 AndAlso key <= Keys.D9)
            switchTo = If(keysPressed.Count < 1, switchTo, keysPressed.Max() - Keys.D1)
        End If

        If switchTo < 0 Then Return
        If Me.Boxes.Count - 1 >= switchTo Then CurrentBox = switchTo
    End Sub

    Private Sub ChooseObject()
        If CursorPosition.Y = 0 Then
            If CursorPosition.X = 0 Then Me.CurrentBox -= 1
            If CursorPosition.X = 5 Then Me.CurrentBox += 1
            If CurrentBox < 0 Then CurrentBox = Me.Boxes.Count - 1
            If CurrentBox > Me.Boxes.Count - 1 Then CurrentBox = 0
            If CursorPosition.X = 6 Then
                SelectPokemon()
            ElseIf CursorPosition.X > 0 AndAlso CursorPosition.X < 5 Then
                If Me.BoxChooseMode Then
                    Me.BoxChooseMode = False
                    Return
                End If
                Dim entries = New List(Of MenuEntry)
                entries.Add(New MenuEntry(3, Localization.GetString("storage_screen_menu_box_ChooseBox", "Choose Box"), False, Sub() Me.BoxChooseMode = Not Me.BoxChooseMode))
                entries.Add(New MenuEntry(4, Localization.GetString("storage_screen_menu_box_ChangeMode", "Change Mode"), False, AddressOf Me.ChangeModeMenu))
                If Not GetBox(CurrentBox).IsBattleBox Then
                    entries.Add(New MenuEntry(5, Localization.GetString("storage_screen_menu_box_Wallpaper", "Wallpaper"), False, AddressOf WallpaperMain))
                    entries.Add(New MenuEntry(6, Localization.GetString("global_name", "Name"), False, AddressOf SelectNameBox))
                End If
                entries.Add(New MenuEntry(entries.Max(Function(x) x.Index) + 1, Localization.GetString("global_cancel", "Cancel"), True, Nothing))
                Me.SetupMenu(entries.ToArray(), Localization.GetString("storage_screen_menu_box_WhatDoYouWant", "What do you want to do?"))
            End If
        ElseIf CursorPosition.Y < 6 Then
            If Not BoxChooseMode Or CursorPosition.X > 5 Or CursorPosition.Y < 1 Then
                SelectPokemon()
                Return
            End If
            Dim id = CInt(CursorPosition.X + (CursorPosition.Y - 1) * 6)
            If GetBox(id) Is Nothing Then Return
            Me.CurrentBox = id
            Me.BoxChooseMode = False
        End If
    End Sub

    Private Sub ChangeModeMenu()
        Dim e As New MenuEntry(3, Localization.GetString("storage_screen_menu_mode_Withdraw", "Withdraw"), False, Sub() Me.SelectionMode = SelectionModes.Withdraw)
        Dim e1 As New MenuEntry(4, Localization.GetString("storage_screen_menu_mode_Deposit", "Deposit"), False, Sub() Me.SelectionMode = SelectionModes.Deposit)
        Dim e2 As New MenuEntry(5, Localization.GetString("storage_screen_menu_mode_SingleMove", "Single Move"), False, Sub() Me.SelectionMode = SelectionModes.SingleMove)
        Dim e3 As New MenuEntry(6, Localization.GetString("storage_screen_menu_mode_EasyMove", "Easy Move"), False, Sub() Me.SelectionMode = SelectionModes.EasyMove)
        Dim e4 As New MenuEntry(7, Localization.GetString("global_cancel", "Cancel"), True, AddressOf Me.ChooseObject)
        Me.SetupMenu({e, e1, e2, e3, e4}, Localization.GetString("storage_screen_menu_mode_ChooseModeToUse", "Choose a mode to use."))
    End Sub

    Private Sub SelectNameBox()
        Dim box = GetBox(CurrentBox)
        Dim defaultName = CStr(Localization.GetString("storage_screen_box_DefaultName", "BOX [NUMBER]")).Replace("[NUMBER]", $"{box.index + 1}")
        Dim inputMode = InputScreen.InputModes.Text
        Dim rename = Sub(name As String) box.Name = name
        Dim screen = New InputScreen(Core.CurrentScreen, defaultName, inputMode, box.Name, 11, New List(Of Texture2D), rename)
        Core.SetScreen(screen)
    End Sub

#Region "Backgrounds"

    Private Sub WallpaperMain()
        Dim badges = Core.Player.Badges.Count
        Dim package1 = New Dictionary(Of String, Integer)
        Dim package2 = New Dictionary(Of String, Integer)
        Dim package3 = New Dictionary(Of String, Integer)
        Dim package4 = New Dictionary(Of String, Integer)
        package1.Add(Localization.GetString("storage_screen_menu_wallpaper_Forest", "Forest"), 0)
        package1.Add(Localization.GetString("storage_screen_menu_wallpaper_City", "City"), 1)
        package1.Add(Localization.GetString("storage_screen_menu_wallpaper_Desert", "Desert"), 2)
        package1.Add(Localization.GetString("storage_screen_menu_wallpaper_Savanna", "Savanna"), 3)
        package1.Add(Localization.GetString("storage_screen_menu_wallpaper_Cave", "Cave"), 8)
        package1.Add(Localization.GetString("storage_screen_menu_wallpaper_River", "River"), 11)

        package2.Add(Localization.GetString("storage_screen_menu_wallpaper_Volcano", "Volcano"), 5)
        package2.Add(Localization.GetString("storage_screen_menu_wallpaper_Snow", "Snow"), 6)
        package2.Add(Localization.GetString("storage_screen_menu_wallpaper_Beach", "Beach"), 9)
        package2.Add(Localization.GetString("storage_screen_menu_wallpaper_Seafloor", "Seafloor"), 10)
        package2.Add(Localization.GetString("storage_screen_menu_wallpaper_Crag", "Crag"), 4)
        package2.Add(Localization.GetString("storage_screen_menu_wallpaper_Steel", "Steel"), 7)

        package3.Add(Localization.GetString("storage_screen_menu_wallpaper_Volcano2", "Volcano 2"), 14)
        package3.Add(Localization.GetString("storage_screen_menu_wallpaper_City2", "City 2"), 15)
        package3.Add(Localization.GetString("storage_screen_menu_wallpaper_Snow2", "Snow 2"), 16)
        package3.Add(Localization.GetString("storage_screen_menu_wallpaper_Desert2", "Desert 2"), 17)
        package3.Add(Localization.GetString("storage_screen_menu_wallpaper_Savanna2", "Savanna 2"), 18)
        package3.Add(Localization.GetString("storage_screen_menu_wallpaper_Steel2", "Steel 2"), 19)

        package4.Add(Localization.GetString("storage_screen_menu_wallpaper_System", "System"), 22)
        package4.Add(Localization.GetString("storage_screen_menu_wallpaper_Simple ", "Simple "), 13)
        package4.Add(Localization.GetString("storage_screen_menu_wallpaper_Checks ", "Checks "), 12)
        package4.Add(Localization.GetString("storage_screen_menu_wallpaper_Seasons", "Seasons"), 23)
        package4.Add(Localization.GetString("storage_screen_menu_wallpaper_Retro1", "Retro 1"), 20)
        package4.Add(Localization.GetString("storage_screen_menu_wallpaper_Retro2", "Retro 2"), 21)
        If Core.Player.SandBoxMode Or GameController.IS_DEBUG_ACTIVE Then
            badges = 16
        End If

        Dim entries = New List(Of MenuEntry)
        Dim cancelIndex = 4
        entries.Add(New MenuEntry(3, Localization.GetString("storage_screen_menu_theme_Package1", "Package 1"), False, Sub() Me.WallpaperList(package1)))
        If badges > 1 Then entries.Add(New MenuEntry(4, Localization.GetString("storage_screen_menu_theme_Package2", "Package 2"), False, Sub() Me.WallpaperList(package2)))
        If badges > 4 Then entries.Add(New MenuEntry(5, Localization.GetString("storage_screen_menu_theme_Package3", "Package 3"), False, Sub() Me.WallpaperList(package3)))
        If badges > 7 Then entries.Add(New MenuEntry(6, Localization.GetString("storage_screen_menu_theme_Package4", "Package 4"), False, Sub() Me.WallpaperList(package4)))
        entries.Add(New MenuEntry(entries.Max(Function(x) x.Index) + 1, Localization.GetString("global_cancel", "Cancel"), True, AddressOf ChooseObject))
        SetupMenu(entries.ToArray(), Localization.GetString("storage_screen_menu_theme_PickTheme", "Please pick a theme."))
    End Sub

    Private Sub WallpaperList(package As Dictionary(Of String, Integer))
        Dim itemList = New List(Of MenuEntry)(package.Count + 1)
        Dim index = 3
        For Each wallpaper In package
            itemList.Add(New MenuEntry(index, wallpaper.Key, False, Sub() GetBox(CurrentBox).Background = wallpaper.Value))
            index += 1
        Next
        itemList.Add(New MenuEntry(index, Localization.GetString("global_cancel", "Cancel"), True, AddressOf WallpaperMain))
        SetupMenu(itemList.ToArray(), Localization.GetString("storage_screen_menu_wallpaper_PickWallpaper", "Pick the wallpaper."))
    End Sub
#End Region

    Private Sub GetYOffset(ByVal p As Pokemon)
        Dim t = p.GetTexture(True)
        Me.yOffset = -1

        Dim cArr(t.Width * t.Height - 1) As Color
        t.GetData(cArr)

        For y = 0 To t.Height - 1
            For x = 0 To t.Width - 1
                If cArr(x + y * t.Height) = Color.Transparent Then Continue For
                Me.yOffset = y
                Exit For
            Next

            If Me.yOffset <> -1 Then Exit For
        Next
    End Sub

    Dim ClickedObject As Boolean = False

    Private Sub MoveCursor()
        Dim changedPosition = CursorMovePosition <> CursorAimPosition

        Dim difference = CursorMovePosition - CursorAimPosition
        Dim speed = New Vector2(Math.Sign(difference.X), Math.Sign(difference.Y)) * Me.CursorSpeed
        CursorMovePosition -= speed

        CursorMovePosition.X = If(speed.X > 0, Math.Max(CursorMovePosition.X, CursorAimPosition.X), Math.Min(CursorMovePosition.X, CursorAimPosition.X))
        CursorMovePosition.Y = If(speed.Y > 0, Math.Max(CursorMovePosition.Y, CursorAimPosition.Y), Math.Min(CursorMovePosition.Y, CursorAimPosition.Y))

        If CursorAimPosition <> CursorMovePosition Then Return
        Me.CursorMoving = False

        If Me.SelectionMode = SelectionModes.EasyMove And changedPosition And Me.ClickedObject Then
            ChooseObject()
        End If
    End Sub

    Private Sub ControlCursor()
        Dim PreCursor = CursorPosition
        Dim box = GetBox(CurrentBox)
        Dim direction = Vector2.Zero
        Dim cancel = ControllerHandler.ButtonPressed(Buttons.X)
        Dim confirm = Controls.Accept(True, False, False) AndAlso GetRelativeMousePosition() <> New Vector2(-1)
        Me.CursorMovePosition = GetAbsoluteCursorPosition(Me.CursorPosition)
        If cancel Then
            Me.CursorPosition = New Vector2(1, 0)
        ElseIf confirm Then
            Me.CursorPosition = GetRelativeMousePosition()
        Else
            If Controls.Right(True, True, False) Then direction.X += 1
            If Controls.Left(True, True, False) Then direction.X -= 1
            If Controls.Up(True, True, False) Then direction.Y -= 1
            If Controls.Down(True, True, False) Then direction.Y += 1
            Me.CursorPosition += direction
            If direction.X > 0 And Me.CursorPosition.Y = 0 And Me.CursorPosition.X > 1 And Me.CursorPosition.X < 5 Then
                Me.CursorPosition.X = 5
            End If
            If direction.X < 0 And Me.CursorPosition.Y = 0 And Me.CursorPosition.X > 0 And Me.CursorPosition.X < 4 Then
                Me.CursorPosition.X = 0
            End If
            If direction.Y > 0 And Me.CursorPosition.Y = 1 And box.IsBattleBox And Me.CursorPosition.X < 6 And Not BoxChooseMode Then
                Me.CursorPosition.X = 2
            End If
        End If
        Me.CursorMoving = cancel Or confirm Or direction <> Vector2.Zero
        Me.ClickedObject = confirm


        Dim XRange() = {0, 6}

        If Not Me.BoxChooseMode Then
            If Me.SelectionMode = SelectionModes.Withdraw And CursorPosition.Y > 0 Then
                XRange = If(box.IsBattleBox, {2, 3}, {0, 5})
            ElseIf Me.SelectionMode = SelectionModes.Deposit And CursorPosition.Y > 0 Then
                XRange = {6, 6}
            ElseIf box.IsBattleBox Then
                XRange = If(CursorPosition.Y = 0, {0, 6}, {2, 6})
            End If
        End If

        If CursorPosition.X < XRange(0) Then CursorPosition.X = XRange(1)
        If CursorPosition.X > XRange(1) Then CursorPosition.X = XRange(0)

        If box.IsBattleBox And Not Me.BoxChooseMode Then
            If Me.CursorPosition.Y > 0 And Me.CursorPosition.X > 3 And Me.CursorPosition.X < 6 Then
                Me.CursorPosition.X = If(PreCursor.X > Me.CursorPosition.X, 3, 6)
            End If
        End If

        Dim YRange() = {0, 5}

        If Not Me.BoxChooseMode Then
            If box.IsBattleBox And Me.CursorPosition.X < 6 Then
                YRange = {0, 3}
            End If
        End If

        If CursorPosition.Y < YRange(0) Then CursorPosition.Y = YRange(1)
        If CursorPosition.Y > YRange(1) Then CursorPosition.Y = YRange(0)

        CursorAimPosition = GetAbsoluteCursorPosition(Me.CursorPosition)

        Me.CursorSpeed = CInt(Vector2.Distance(CursorMovePosition, CursorAimPosition) * 0.3)
    End Sub

    Private Sub CloseScreen()
        If Me.BoxChooseMode Then
            Me.BoxChooseMode = False
            Return
        End If
        If MovingPokemon IsNot Nothing Then
            If PickupPlace.X = 6 Then
                Core.Player.Pokemons.Add(Me.MovingPokemon)
            Else
                Dim id = CInt(PickupPlace.X) + CInt((PickupPlace.Y - 1) * 6)

                Dim box = GetBox(PickupBox)
                Dim index = If(box.IsBattleBox, box.Pokemon.Count, id)
                box.Pokemon.Add(index, New PokemonWrapper(Me.MovingPokemon)) ' Me.MovingPokemon))

                CurrentBox = PickupBox
            End If
            Me.MovingPokemon = Nothing
        Else
            If GetBox(Me.CurrentBox).IsBattleBox = True Then
                For NewBoxIndex = 0 To Boxes.Count - 1
                    If Boxes(NewBoxIndex).IsFull = False Then
                        Player.Temp.PCBoxIndex = NewBoxIndex
                        Exit For
                    End If
                Next
            Else
                Player.Temp.PCBoxIndex = Me.CurrentBox
            End If

            Player.Temp.StorageSystemCursorPosition = Me.CursorPosition
            Player.Temp.PCBoxChooseMode = Me.BoxChooseMode
            Player.Temp.PCSelectionType = Me.SelectionMode

            Core.Player.BoxData = GetBoxSaveData(Me.Boxes)

            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.Black, False))
        End If
    End Sub

    Private Shared Function GetBoxSaveData(ByVal boxes As List(Of Box)) As String
        Dim BoxesFull = True
        Dim newData = New List(Of String)
        For Each b In boxes
            If b.IsBattleBox Then Continue For
            newData.Add($"BOX|{b.index}|{b.Name}|{b.Background}")

            Dim hasPokemon = False
            For i = 0 To 29
                If Not b.Pokemon.ContainsKey(i) Then Continue For
                hasPokemon = True
                newData.Add($"{b.index},{i},{b.Pokemon(i).PokemonData}")
            Next
            If Not hasPokemon Then BoxesFull = False
        Next

        Dim addedBoxes = 0
        If BoxesFull And boxes.Count < 30 Then
            Dim newBoxes = 5
            If boxes.Count + 5 > 30 Then
                newBoxes = 30 - boxes.Count
            End If

            addedBoxes = newBoxes
            Core.Player.BoxAmount = boxes.Count + newBoxes

            For i = 0 To newBoxes - 1
                Dim newBoxID = boxes.Count - 1 + i

                newData.Add($"BOX|{newBoxID}|BOX {newBoxID + 1}|{Core.Random.Next(0, 19)}")
            Next
        End If

        Dim battleBox = boxes.Last()
        newData.Add($"BOX|{boxes.Count - 1 + addedBoxes}|{battleBox.Name}|{battleBox.Background}")

        For i = 0 To 29
            If Not battleBox.Pokemon.ContainsKey(i) Then Continue For
            newData.Add($"{boxes.Count - 1 + addedBoxes},{i},{battleBox.Pokemon(i).PokemonData}")
        Next

        Return String.Join(Environment.NewLine, newData)
    End Function

    Private Function GetRelativeMousePosition() As Vector2
        For x = 0 To 5
            For y = 0 To 4
                Dim boxTile = New Rectangle(50 + x * 100, 200 + y * 84, 64, 64)
                If boxTile.Contains(MouseHandler.MousePosition) Then Return New Vector2(x, y + 1)
            Next
        Next

        For y = 0 To 5
            Dim partyTile = New Rectangle(Core.windowSize.Width - 260, y * 100 + 50, 128, 80)
            If partyTile.Contains(MouseHandler.MousePosition) Then Return New Vector2(6, y)
        Next

        Dim labelArea = New Rectangle(80, 50, 600, 100)
        Dim leftArrowArea = New Rectangle(10, 52, 96, 96)
        Dim rightArrowArea = New Rectangle(655, 52, 96, 96)
        If labelArea.Contains(MouseHandler.MousePosition) Then Return New Vector2(1, 0)
        If leftArrowArea.Contains(MouseHandler.MousePosition) Then Return New Vector2(0, 0)
        If rightArrowArea.Contains(MouseHandler.MousePosition) Then Return New Vector2(5, 0)


        Return New Vector2(-1)
    End Function

    Private Function GetAbsoluteCursorPosition(ByVal relPos As Vector2) As Vector2
        If relPos.Y = 0 Then
            If relPos.X < 0 Or relPos.X > 6 Then Return New Vector2()
            Dim leftArrow = New Vector2(60, 20)
            Dim rightArrow = New Vector2(705, 20)
            Dim label = New Vector2(380, 30)
            Dim party = New Vector2(Core.windowSize.Width - 200, 20)
            Dim positions = {leftArrow, label, label, label, label, rightArrow, party}
            Return positions(CInt(relPos.X))
        End If
        If relPos.Y > 0 And relPos.Y < 6 Then
            Dim boxTile = New Vector2(50 + relPos.X * 100 + 42, 200 + (relPos.Y - 1) * 84 - 42)
            Dim partyTile = New Vector2(Core.windowSize.Width - 200, 20 + 100 * relPos.Y)
            If relPos.X >= 0 And relPos.X < 6 Then Return boxTile
            If relPos.X = 6 Then Return partyTile
        End If
        Return New Vector2()
    End Function

    Private Function GetBattleBoxID() As Integer
        If CursorPosition.Y < 1 Or CursorPosition.Y > 3 Then Return -1
        If CursorPosition.X = 2 Then Return CInt(CursorPosition.Y * 2 - 2)
        If CursorPosition.X = 3 Then Return CInt(CursorPosition.Y * 2 - 1)
        Return -1
    End Function

    Private Sub SelectPokemon()
        If SelectionMode = SelectionModes.EasyMove Then
            PickupPokemon()
            Return
        End If
        If SelectionMode <> SelectionModes.SingleMove And SelectionMode <> SelectionModes.Withdraw And SelectionMode <> SelectionModes.Deposit Then
            Return
        End If
        If Me.MovingPokemon IsNot Nothing Then
            PickupPokemon()
            Return
        End If
        Dim box = GetBox(CurrentBox)
        Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))


        If box.Pokemon.ContainsKey(id) And CursorPosition.X < 6 Or CursorPosition.X = 6 And Core.Player.Pokemons.Count - 1 >= CInt(CursorPosition.Y) Then
            Dim p = If(CursorPosition.X = 6, Core.Player.Pokemons(CInt(CursorPosition.Y)), box.Pokemon(id).pokemon)

            Dim entries = New List(Of MenuEntry)

            If Me.SelectionMode = SelectionModes.Withdraw Then
                entries.Add(New MenuEntry(3, Localization.GetString("storage_screen_menu_pokemon_Withdraw", "Withdraw"), False, AddressOf WithdrawPokemon))
            ElseIf Me.SelectionMode = SelectionModes.Deposit Then
                entries.Add(New MenuEntry(3, Localization.GetString("storage_screen_menu_pokemon_Deposit", "Deposit"), False, AddressOf DepositPokemon))
            Else
                entries.Add(New MenuEntry(3, Localization.GetString("storage_screen_menu_pokemon_Move", "Move"), False, AddressOf PickupPokemon))
            End If

            entries.Add(New MenuEntry(4, Localization.GetString("global_summary", "Summary"), False, AddressOf SummaryPokemon))

            Dim itemOffset = If(p.Item IsNot Nothing, 1, 0)

            If p.Item IsNot Nothing Then entries.Add(New MenuEntry(5, Localization.GetString("storage_screen_menu_pokemon_TakeItem", "Take Item"), False, Sub() TakeItemPokemon()))
            entries.Add(New MenuEntry(5 + itemOffset, Localization.GetString("storage_screen_menu_pokemon_Release", "Release"), False, AddressOf ReleasePokemon))
            entries.Add(New MenuEntry(6 + itemOffset, Localization.GetString("global_cancel", "Cancel"), True, Nothing))
            SetupMenu(entries.ToArray(), Localization.GetString("storage_screen_menu_pokemon_IsSelected", "//POKEMONNAME// is selected.").Replace("//POKEMONNAME//", p.GetDisplayName()))
        End If
    End Sub

    Private Sub PickupPokemon()
        If CursorPosition.X = 6 Then
            If Core.Player.Pokemons.Count - 1 >= CursorPosition.Y Then
                Dim l = New List(Of Pokemon)(Core.Player.Pokemons.ToArray())
                l.RemoveAt(CInt(CursorPosition.Y))
                If Me.MovingPokemon IsNot Nothing Then
                    l.Add(Me.MovingPokemon)
                End If
                Dim hasPokemon = l.Any(Function(p) Not p.IsEgg() And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0)

                If Not hasPokemon Then
                    SetupMenu({New MenuEntry(3, Localization.GetString("global_ok", "OK"), True, Nothing)}, Localization.GetString("storage_screen_pokemon_CannotRemoveLastPokemon", "Can't remove last Pokémon from party."))
                Else
                    If Me.MovingPokemon IsNot Nothing Then
                        Dim sPokemon = Core.Player.Pokemons(CInt(CursorPosition.Y))
                        Me.MovingPokemon.FullRestore()
                        Core.Player.Pokemons.Insert(CInt(CursorPosition.Y), Me.MovingPokemon)
                        Me.MovingPokemon = sPokemon
                        Core.Player.Pokemons.RemoveAt(CInt(CursorPosition.Y) + 1)
                    Else
                        Me.MovingPokemon = Core.Player.Pokemons(CInt(CursorPosition.Y))
                        Core.Player.Pokemons.RemoveAt(CInt(CursorPosition.Y))

                        PickupBox = 0
                        PickupPlace = New Vector2(6, 0)
                    End If
                End If
            ElseIf Me.MovingPokemon IsNot Nothing Then
                Me.MovingPokemon.FullRestore()
                Core.Player.Pokemons.Add(Me.MovingPokemon)
                Me.MovingPokemon = Nothing
            End If
        Else
            Dim box = GetBox(CurrentBox)
            Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))


            Dim pokemonExists = box.Pokemon.ContainsKey(id)

            If pokemonExists Then
                If Me.MovingPokemon Is Nothing Then
                    Me.MovingPokemon = box.Pokemon(id).pokemon
                    box.Pokemon.Remove(id)

                    PickupBox = CurrentBox
                    PickupPlace = CursorPosition
                    RearrangeBattleBox(box)
                Else
                    Me.MovingPokemon.FullRestore()
                    Dim sPokemon = box.Pokemon(id).pokemon
                    box.Pokemon(id) = New PokemonWrapper(Me.MovingPokemon) ' Me.MovingPokemon
                    Me.MovingPokemon = sPokemon
                End If
            ElseIf Me.MovingPokemon IsNot Nothing Then
                Me.MovingPokemon.FullRestore()

                Dim index = If(box.IsBattleBox, box.Pokemon.Count, id)
                box.Pokemon.Add(index, New PokemonWrapper(Me.MovingPokemon)) ' Me.MovingPokemon)

                Me.MovingPokemon = Nothing
            End If
        End If
    End Sub

    Private Sub WithdrawPokemon()
        Dim box = GetBox(CurrentBox)
        Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))

        If Core.Player.Pokemons.Count > 5 Then
            SetupMenu({New MenuEntry(3, Localization.GetString("global_ok", "OK"), True, Nothing)}, Localization.GetString("storage_screen_pokemon_PartyIsFull", "Party is full!"))
        ElseIf box.Pokemon.ContainsKey(id) Then
            Core.Player.Pokemons.Add(box.Pokemon(id).pokemon)
            box.Pokemon.Remove(id)
        End If
        RearrangeBattleBox(box)
    End Sub

    Private Sub DepositPokemon()
        Dim box = GetBox(CurrentBox)
        If box.Pokemon.Count > 29 Then Return
        If Core.Player.Pokemons.Count - 1 < CInt(Me.CursorPosition.Y) Then Return
        Dim l = New List(Of Pokemon)(Core.Player.Pokemons.ToArray())
        l.RemoveAt(CInt(CursorPosition.Y))
        Dim hasPokemon = l.Any(Function(p) Not p.IsEgg() And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0)

        If Not hasPokemon Then
            SetupMenu({New MenuEntry(3, Localization.GetString("global_ok", "OK"), True, Nothing)}, Localization.GetString("storage_screen_pokemon_CannotRemoveLastPokemon", "Can't remove last Pokémon from party."))
        Else
            Dim nextIndex = 0
            While box.Pokemon.ContainsKey(nextIndex)
                nextIndex += 1
            End While
            Core.Player.Pokemons(CInt(Me.CursorPosition.Y)).FullRestore()
            box.Pokemon.Add(nextIndex, New PokemonWrapper(Core.Player.Pokemons(CInt(Me.CursorPosition.Y)))) ' Core.Player.Pokemons(CInt(Me.CursorPosition.Y)))
            Core.Player.Pokemons.RemoveAt(CInt(Me.CursorPosition.Y))
        End If
    End Sub

    Private Sub SummaryPokemon()
        If CursorPosition.X = 6 Then
            Core.SetScreen(New SummaryScreen(Me, Core.Player.Pokemons.ToArray(), CInt(CursorPosition.Y)))
            Return
        End If
        Dim box = GetBox(CurrentBox)
        Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))
        Dim pokemonList = box.GetPokemonList()
        Dim partyIndex = pokemonList.IndexOf(box.Pokemon(id).pokemon)

        Core.SetScreen(New SummaryScreen(Me, pokemonList.ToArray(), partyIndex))
    End Sub

    Private Function TakeItemPokemon(Optional logImmediate As Boolean = True) As String
        Dim box = GetBox(CurrentBox)
        Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))
        Dim pokemon = If(CursorPosition.X = 6, Core.Player.Pokemons(CInt(CursorPosition.Y)), box.Pokemon(id).pokemon)
        Dim message = ""
        If pokemon.Item Is Nothing Then Return message
        If pokemon.Item.IsMail And pokemon.Item.AdditionalData <> "" Then
            message = Localization.GetString("storage_screen_pokemon_item_MailWasTaken", "The Mail was taken to your~inbox on your PC.")

            Core.Player.Mails.Add(Items.MailItem.GetMailDataFromString(pokemon.Item.AdditionalData))
        Else
            message = Localization.GetString("storage_screen_pokemon_item_TakenItemFromPokemon", "Taken //ITEM//~from //POKEMONNAME//.").Replace("//ITEM//", pokemon.Item.OneLineName()).Replace("//POKEMONNAME//", pokemon.GetDisplayName())
            Dim ItemID = If(pokemon.Item.IsGameModeItem, pokemon.Item.gmID, pokemon.Item.ID.ToString())

            Core.Player.Inventory.AddItem(ItemID, 1)
        End If
        If logImmediate Then Screen.TextBox.Show(message)
        pokemon.Item = Nothing
        Return message
    End Function

    Private Sub ReleasePokemon()
        Dim hasPokemon = False

        If Me.CursorPosition.X <> 6 Then
            hasPokemon = True
        Else
            Dim l = New List(Of Pokemon)(Core.Player.Pokemons.ToArray())
            l.RemoveAt(CInt(CursorPosition.Y))

            hasPokemon = l.Any(Function(p) Not p.IsEgg() And p.Status <> Pokemon.StatusProblems.Fainted And p.HP > 0)
        End If

        If hasPokemon Then
            Dim box = GetBox(CurrentBox)
            Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6))


            Dim p = If(CursorPosition.X = 6, Core.Player.Pokemons(CInt(CursorPosition.Y)), box.Pokemon(id).pokemon)

            If Not p.IsEgg() Then
                Dim e1 = New MenuEntry(3, Localization.GetString("global_no", "No"), True, AddressOf SelectPokemon)
                Dim e = New MenuEntry(4, Localization.GetString("global_yes", "Yes"), False, AddressOf ConfirmRelease)
                Me.SetupMenu({e1, e}, Localization.GetString("storage_screen_pokemon_release_Question", "Release //POKEMONNAME//?").Replace("//POKEMONNAME//", p.GetDisplayName()))
            Else
                Me.SetupMenu({New MenuEntry(3, Localization.GetString("global_ok", "OK"), True, Nothing)}, Localization.GetString("storage_screen_pokemon_release_CannotReleaseEgg", "Cannot release an Egg."))
            End If
        Else
            Me.SetupMenu({New MenuEntry(3, Localization.GetString("global_ok", "OK"), True, Nothing)}, Localization.GetString("storage_screen_pokemon_release_CannotReleaseLastPokemon", "Cannot release the last Pokémon."))
        End If
    End Sub

    Private Sub ConfirmRelease()
        Dim id = CInt(CursorPosition.X) + CInt((CursorPosition.Y - 1) * 6)
        Dim box = GetBox(CurrentBox)
        Dim pokemon = If(CursorPosition.X = 6, Core.Player.Pokemons(CInt(CursorPosition.Y)), box.Pokemon(id).pokemon)
        Dim text = ""
        If pokemon.Item IsNot Nothing Then text &= Me.TakeItemPokemon(False)
        If text <> "" Then text &= "*"
        text &= Localization.GetString("storage_screen_pokemon_release_Goodbye", "Goodbye, //POKEMONNAME//!").Replace("//POKEMONNAME//", pokemon.GetDisplayName())
        Screen.TextBox.Show(text)

        If CursorPosition.X = 6 Then
            Core.Player.Pokemons.RemoveAt(CInt(CursorPosition.Y))
        Else
            box.Pokemon.Remove(id)
        End If
    End Sub

    Private Sub RearrangeBattleBox(ByVal b As Box)
        If Not b.IsBattleBox Then Return
        Dim p = b.GetPokemonList()
        b.Pokemon.Clear()

        For i = 0 To p.Count - 1
            b.Pokemon.Add(i, New PokemonWrapper(p(i))) ' p(i))
        Next
    End Sub

#End Region

#Region "Draw"

    Public Overrides Sub Draw()
        ' Draw3DModel()
        DrawMainWindow()
        DrawPokemonStatus()

        DrawTopBar()
        DrawTeamWindow()

        Dim Draw = If(Me.MenuVisible, CType(AddressOf Me.DrawMenuEntries, Action), AddressOf Me.DrawCursor)
        Draw()
        TextBox.Draw()

    End Sub

    Private Sub DrawTopBar()
        Dim boxIndex = Me.CurrentBox
        If BoxChooseMode Then
            boxIndex = If(CursorPosition.X < 6 And CursorPosition.Y > 0, CInt(CursorPosition.X + (CursorPosition.Y - 1) * 6), CurrentBox)
        End If
        Dim b = GetBox(boxIndex)

        If b Is Nothing Then Return
        Dim texturePath = If(b.IsBattleBox, "GUI\Box\BattleBox", $"GUI\Box\{b.Background}")
        Core.SpriteBatch.Draw(TextureManager.GetTexture(texturePath), New Rectangle(80, 50, 600, 100), Color.White)

        Dim cArr(0) As Color
        TextureManager.GetTexture(texturePath, New Rectangle(0, 0, 1, 1), "").GetData(cArr)
        Canvas.DrawScrollBar(New Vector2(80, 36), Me.Boxes.Count, 1, boxIndex, New Size(600, 14), True, Color.TransparentBlack, cArr(0))


        Dim font = FontManager.MainFont
        Dim textWidth = font.MeasureString(b.Name).X
        Dim labelPosition = New Vector2(380 - textWidth, 76)
        Dim labelShadowPosition = labelPosition + New Vector2(4)

        Core.SpriteBatch.DrawString(font, b.Name, labelShadowPosition, Color.Black, 0.0F, Vector2.Zero, 2, SpriteEffects.None, 0.0F)
        Core.SpriteBatch.DrawString(font, b.Name, labelPosition, Color.White, 0.0F, Vector2.Zero, 2, SpriteEffects.None, 0.0F)

        Dim textureArea = New Rectangle(0, 16, 16, 16)
        Dim leftArrowArea = New Rectangle(10, 52, 96, 96)
        Dim rightArrowArea = New Rectangle(655, 52, 96, 96)
        Core.SpriteBatch.Draw(Me.menuTexture, leftArrowArea, textureArea, Color.White)
        Core.SpriteBatch.Draw(Me.menuTexture, rightArrowArea, textureArea, Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)
    End Sub

    Private Sub DrawPokemonIcon(area As Rectangle, pokemon As Pokemon, Optional drawItem As Boolean = False, Optional drawShadow As Boolean = False)
        Dim light = If(IsLit(pokemon), Color.White, New Color(65, 65, 65, 255))
        Dim texture = pokemon.GetMenuTexture()
        If drawShadow Then
            Dim shadowArea = New Rectangle(area.Location + New Point(10, 10), area.Size)
            Core.SpriteBatch.Draw(texture, shadowArea, New Color(0, 0, 0, 150))
        End If
        Core.SpriteBatch.Draw(texture, area, light)
        If pokemon.IsEgg() Or Not drawItem Or pokemon.Item Is Nothing Then Return
        Dim itemArea = New Rectangle(area.Location + New Point(32), New Point(24))
        Core.SpriteBatch.Draw(pokemon.Item.Texture, itemArea, Color.White)
    End Sub

    Private Sub DrawMainWindow()
        Dim backgroundColor = New Color(220, 220, 220)
        Dim TilePosition = Function(x As Integer, y As Integer) New Point(x, y) * New Point(100, 84) + New Point(50, 200)
        If BoxChooseMode Then
            Canvas.DrawRectangle(Core.windowSize, backgroundColor)

            For x = 0 To 5
                For y = 0 To 4
                    Dim id = y * 6 + x

                    If Me.Boxes.Count - 1 < id Then Continue For
                    Dim pBox = Me.GetBox(id)
                    Dim pCount = If(pBox Is Nothing, 0, pBox.Pokemon.Values.Where(Function(p) IsLit(p.pokemon)).Count())

                    Dim empty = New Point(64, 32)
                    Dim notEmpty = New Point(64, 0)
                    Dim full = New Point(32, 32)
                    Dim tCoord = notEmpty
                    If pCount = 0 Then tCoord = empty
                    If pCount = 30 Then tCoord = full

                    Dim tileArea = New Rectangle(TilePosition(x, y), New Point(64))
                    Dim textureArea = New Rectangle(tCoord, New Point(32))
                    Core.SpriteBatch.Draw(Me.texture, tileArea, textureArea, Color.White)
                Next
            Next
            Return
        End If
        Dim box = GetBox(CurrentBox)
        Dim wrapper As PokemonWrapper = Nothing
        Dim cArr(0) As Color
        Dim background = If(box.IsBattleBox, "GUI\Box\Battlebox", $"GUI\Box\{box.Background}")
        TextureManager.GetTexture(background, New Rectangle(0, 0, 1, 1), "").GetData(cArr, 0, 1)
        backgroundColor = New Color(cArr(0).R, cArr(0).G, cArr(0).B, 150)
        If box.IsBattleBox Then
            Canvas.DrawGradient(Core.windowSize, New Color(203, 40, 41), New Color(238, 128, 128), False, -1)


            For i = 0 To 5
                Dim x = i + 2
                Dim y = 0
                While x > 3
                    x -= 2
                    y += 1
                End While
                Dim area = New Rectangle(TilePosition(x, y), New Point(64))
                Canvas.DrawRectangle(area, backgroundColor)
                If Not box.Pokemon.TryGetValue(i, wrapper) Then Continue For
                Me.DrawPokemonIcon(area, wrapper.pokemon, True)
            Next
        Else
            Dim xt = box.Background
            Dim yt = 0
            While xt > 7
                xt -= 8
                yt += 1
            End While
            For x = 0 To Core.windowSize.Width Step 64
                For y = 0 To Core.windowSize.Height Step 64
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(x, y, 64, 64), New Rectangle(xt * 16, yt * 16 + 64, 16, 16), Color.White)
                Next
            Next

            For x = 0 To 5
                For y = 0 To 4
                    Dim id = y * 6 + x

                    Dim area = New Rectangle(TilePosition(x, y), New Point(64))
                    Canvas.DrawRectangle(area, backgroundColor)
                    If Not box.Pokemon.TryGetValue(id, wrapper) Then Continue For
                    Me.DrawPokemonIcon(area, wrapper.pokemon, True)
                Next
            Next
            Dim text = Localization.GetString("storage_screen_filter_Hint", "Press <system.button(special)> on the keyboard to filter.")
            Dim textPosition = New Vector2(44, 200 + 5 * 84)
            Dim shadowPosition = textPosition + New Vector2(2)
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, shadowPosition, Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, textPosition, Color.White)
        End If
    End Sub

    Dim yOffset As Integer = 0

    Private Sub DrawPokemonStatus()
        If Me.BoxChooseMode And CursorPosition.X < 6 And CursorPosition.Y > 0 Then
            Dim box = GetBox(CInt(CursorPosition.X + (CursorPosition.Y - 1) * 6))

            If box Is Nothing Then Return
            Dim overviewArea = New Rectangle(660, 200, 200, 200)
            Dim detailsArea = New Rectangle(660, 410, 200, 210)
            Canvas.DrawRectangle(overviewArea, New Color(84, 198, 216, 150))

            Dim minLevel = Integer.MaxValue
            Dim maxLevel = Integer.MinValue

            For x = 0 To 5
                For y = 0 To 4
                    Dim id = y * 6 + x

                    Dim wrapper As PokemonWrapper = Nothing
                    If Not box.Pokemon.TryGetValue(id, wrapper) Then Continue For
                    Dim position = New Point(x, y) * New Point(32) + New Point(664, 215)
                    Dim pokemon = wrapper.pokemon
                    Me.DrawPokemonIcon(New Rectangle(position, New Point(32)), pokemon)

                    minLevel = Math.Min(minLevel, pokemon.Level)
                    maxLevel = Math.Max(maxLevel, pokemon.Level)
                Next
            Next

            Canvas.DrawRectangle(detailsArea, New Color(84, 198, 216, 150))

            Dim levelString = If(minLevel = Integer.MaxValue Or maxLevel = Integer.MinValue, Localization.GetString("global_none", "None"), $"{minLevel} - {maxLevel}")

            Dim maxPokemon = If(box.IsBattleBox, 6, 30)

            Dim t = Localization.GetString("storage_screen_pokemon_summary_Box", "Box") & $":  {box.Name}{Environment.NewLine}"
            t &= Localization.GetString("storage_screen_pokemon_summary_Pokemon", "Pokémon") & $":  {box.Pokemon.Count} / {maxPokemon}{Environment.NewLine}"
            t &= Localization.GetString("storage_screen_pokemon_summary_Level", "Level") & $":  {levelString}"

            Dim infoPosition = New Vector2(665, 415)
            Dim shadowPosition = infoPosition + New Vector2(2)
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, shadowPosition, Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, infoPosition, Color.White)
        Else
            Dim box = GetBox(CurrentBox)
            Dim p = Me.MovingPokemon

            If p Is Nothing Then
                If CursorPosition.X = 6 Then
                    If Core.Player.Pokemons.Count - 1 >= CursorPosition.Y Then
                        p = Core.Player.Pokemons(CInt(CursorPosition.Y))
                    End If
                Else
                    Dim id = If(box.IsBattleBox, GetBattleBoxID(), CInt(Me.CursorPosition.X + (Me.CursorPosition.Y - 1) * 6))


                    If box.Pokemon.ContainsKey(id) Then
                        p = box.Pokemon(id).pokemon
                    End If
                End If
            End If

            If p Is Nothing Then Return
            Dim cArr(0) As Color

            Dim texturePath = If(box.IsBattleBox, "GUI\Box\BattleBox", $"GUI\Box\{box.Background}")
            TextureManager.GetTexture(texturePath, New Rectangle(0, 0, 1, 1), "").GetData(cArr, 0, 1)

            Dim overviewArea = New Rectangle(660, 200, 256, 256)
            Dim detailsArea = New Rectangle(660, 472, 320, 240)
            Dim backgroundColor = If(BoxChooseMode, New Color(84, 198, 216, 150), New Color(cArr(0).R, cArr(0).G, cArr(0).B, 150))

            Canvas.DrawRectangle(overviewArea, backgroundColor)

            Dim modelName = p.AnimationName
            Dim shinyString = If(p.IsShiny, Localization.GetString("storage_screen_pokemon_summary_Shiny", "Shiny"), Localization.GetString("storage_screen_pokemon_summary_Normal", "Normal"))
            If Core.Player.ShowModelsInBattle AndAlso ModelManager.ModelExist($"Models\Pokemon\{modelName}\{shinyString}") And Not p.IsEgg() Then
                Draw3DModel(p, $"Models\Pokemon\{modelName}\{shinyString}")
            Else
                GetYOffset(p)
                Dim texture = p.GetTexture(True)
                Dim size = Vector2.Min(New Vector2(texture.Width, texture.Height) * 3, New Vector2(288, 288)).ToPoint()
                Dim position = New Point(792 - CInt(size.X / 2), 192 - yOffset)
                Core.SpriteBatch.Draw(texture, New Rectangle(position, size), Color.White)
            End If

            Canvas.DrawRectangle(detailsArea, backgroundColor)

            Dim text = ""
            If p.IsEgg() Then
                text = Localization.GetString("storage_screen_pokemon_summary_Egg", "Egg")
            Else
                Dim itemString = If(p.Item Is Nothing, Localization.GetString("global_none", "None"), p.Item.Name)

                Dim nameString = If(p.NickName = "", p.GetDisplayName(), $"{p.GetDisplayName()}/{p.GetName}")

                text = $"{nameString}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_DexNo", "DEX NO.") & $"  {p.Number}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_Level", "LEVEL") & $"  {p.Level}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_HP", "HP") & $"  {p.HP} / {p.MaxHP}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_Attack", "ATTACK") & $"  {p.Attack}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_Defense", "DEFENSE") & $"  {p.Defense}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_SpAtk", "SP. ATK") & $"  {p.SpAttack}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_SpDef", "SP. DEF") & $"  {p.SpDefense}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_Speed", "SPEED") & $"  {p.Speed}{Environment.NewLine}"
                text &= Localization.GetString("storage_screen_pokemon_summary_Item", "ITEM") & $"  {itemString}"
            End If
            Dim textPosition = New Vector2(665, 477)
            Dim shadowPosition = textPosition + New Vector2(2)
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, shadowPosition, Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, textPosition, Color.White)
        End If
    End Sub

    Private Sub Draw3DModel(ByVal p As Pokemon, ByVal modelName As String)
        Dim propList = p.GetModelProperties()

        Dim scale = propList.Item1 * 10
        Dim position = New Vector3(propList.Item2, propList.Item3, propList.Item4)

        Dim roll = propList.Item5

        Dim t = ModelManager.DrawModelToTexture(modelName, renderTarget, position, New Vector3(0, 10, 50), New Vector3(roll + modelRoll, 0, 0), scale, True)
        Core.SpriteBatch.Draw(t, New Rectangle(192, 72, 1200, 680), Color.White)
    End Sub

    Private Sub DrawTeamWindow()
        Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 310, 0, 400, Core.windowSize.Height), New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Dim scrollArea = New Rectangle(Core.windowSize.Width - 128, y + StorageSystemScreen.TileOffset, 128, 64)
            Dim scrollTextureArea = New Rectangle(48, 0, 16, 16)
            Core.SpriteBatch.Draw(Me.menuTexture, scrollArea, scrollTextureArea, Color.White)
        Next
        Dim halfHeight = CInt(Core.windowSize.Height / 2)
        Dim cutoutArea = New Rectangle(Core.windowSize.Width - 430, 0, 128, halfHeight)
        Dim cutoutTextureArea = New Rectangle(96, 0, 32, 64)

        Core.SpriteBatch.Draw(Me.texture, cutoutArea, cutoutTextureArea, Color.White)
        cutoutArea.Location += New Point(0, halfHeight)
        Core.SpriteBatch.Draw(Me.texture, cutoutArea, cutoutTextureArea, Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipVertically, 0.0F)

        For i = 0 To 5
            Dim outlineArea = New Rectangle(Core.windowSize.Width - 260, i * 100 + 50, 128, 80)
            Canvas.DrawBorder(2, outlineArea, New Color(42, 167, 198))

            If Core.Player.Pokemons.Count - 1 < i Then Continue For
            Dim pokemon = Core.Player.Pokemons(i)
            Dim spriteArea = New Rectangle(Core.windowSize.Width - 228, i * 100 + 60, 64, 64)
            Me.DrawPokemonIcon(spriteArea, pokemon, True)
        Next
    End Sub

    Private Sub DrawCursor()
        Dim cPosition = If(CursorMoving, CursorMovePosition, GetAbsoluteCursorPosition(Me.CursorPosition)).ToPoint()


        If Me.MovingPokemon IsNot Nothing Then
            Dim spriteArea = New Rectangle(cPosition + New Point(-20, 34), New Point(64))
            Me.DrawPokemonIcon(spriteArea, Me.MovingPokemon, True, True)
        End If

        Core.SpriteBatch.Draw(GetCursorTexture(), New Rectangle(cPosition, New Point(64)), Color.White)
    End Sub

    Private Sub DrawMenuEntries()
        If Me.MenuHeader <> "" Then
            Dim font = FontManager.MainFont
            Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 370, 100, 356, 64), New Color(0, 0, 0, 180))
            Core.SpriteBatch.DrawString(font, MenuHeader, New Vector2(Core.windowSize.Width - 192 - font.MeasureString(MenuHeader).X / 2, 120), Color.White)
        End If

        Me.MenuEntries.ForEach(Sub(x) x.Draw(Me.MenuCursor, GetCursorTexture()))
    End Sub

    Private Function GetCursorTexture() As Texture2D
        Dim rectangles = New Dictionary(Of SelectionModes, Rectangle)
        rectangles.Add(SelectionModes.SingleMove, New Rectangle(0, 0, 16, 16))
        rectangles.Add(SelectionModes.EasyMove, New Rectangle(16, 0, 16, 16))
        rectangles.Add(SelectionModes.Deposit, New Rectangle(32, 0, 16, 16))
        rectangles.Add(SelectionModes.Withdraw, New Rectangle(0, 32, 16, 16))

        Return If(rectangles.ContainsKey(Me.SelectionMode), TextureManager.GetTexture("GUI\Menus\General", rectangles(Me.SelectionMode), ""), Nothing)
    End Function

#End Region

    Private Function IsLit(ByVal p As Pokemon) As Boolean
        If Me.Filters.Count < 1 Then Return True
        If p.IsEgg() Then Return False
        Dim criteria = New Dictionary(Of FilterTypes, Func(Of Filter, Boolean))
        criteria.Add(FilterTypes.Ability, Function(f) p.Ability.Name.ToLower() = f.FilterValue.ToLower())
        criteria.Add(FilterTypes.Gender, Function(f) p.Gender.ToString().ToLower() = f.FilterValue.ToLower())
        criteria.Add(FilterTypes.Nature, Function(f) p.Nature.ToString().ToLower() = f.FilterValue.ToLower())
        criteria.Add(FilterTypes.Pokémon, Function(f) p.GetName().ToLower() = f.FilterValue.ToLower())
        criteria.Add(FilterTypes.Move, Function(f) p.Attacks.Any(Function(a) a.Name.ToLower() = f.FilterValue.ToLower()))
        criteria.Add(FilterTypes.Type1, Function(f) p.Type1.Type = BattleSystem.GameModeElementLoader.GetElementByName(f.FilterValue).Type)
        criteria.Add(FilterTypes.Type2, Function(f) p.Type2.Type = BattleSystem.GameModeElementLoader.GetElementByName(f.FilterValue).Type)
        For Each f As Filter In Filters
            Dim check As Func(Of Filter, Boolean) = Nothing
            If criteria.TryGetValue(f.FilterType, check) AndAlso Not criteria(f.FilterType)(f) Then Return False
            If f.FilterType = FilterTypes.HeldItem Then
                If f.FilterValue = Localization.GetString("storage_screen_filter_HeldItem_No", "Has no Held Item") And p.Item IsNot Nothing Then Return False
                If f.FilterValue = Localization.GetString("storage_screen_filter_HeldItem_Yes", "Has a Held Item") And p.Item Is Nothing Then Return False
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Adds a Pokémon to the next free spot and returns the index of that box.
    ''' </summary>
    Public Shared Function DepositPokemon(ByVal p As Pokemon, Optional ByVal BoxIndex As Integer = -1) As Integer
        p.FullRestore()

        Dim Boxes = LoadBoxes()
        Dim startIndex = If(BoxIndex > -1, BoxIndex, 0)

        For i = startIndex To Boxes.Count - 1
            Dim pokemons = GetBox(i, Boxes).Pokemon
            If pokemons.Count > 29 Then Continue For
            For l = 0 To 29
                If pokemons.ContainsKey(l) Then Continue For
                pokemons.Add(l, New PokemonWrapper(p)) ' p)
                Exit For
            Next
            Core.Player.BoxData = GetBoxSaveData(Boxes)
            Return i
        Next

        If startIndex = 0 Then Return -1
        For i = 0 To startIndex - 1
            Dim pokemons = GetBox(i, Boxes).Pokemon
            If pokemons.Count > 29 Then Continue For
            For l = 0 To 29
                If pokemons.ContainsKey(l) Then Continue For
                pokemons.Add(l, New PokemonWrapper(p)) ' p)
                Exit For
            Next
            Core.Player.BoxData = GetBoxSaveData(Boxes)
            Return i
        Next

        Return -1
    End Function

    Public Shared Function GetBoxName(ByVal boxIndex As Integer) As String
        Return GetBox(boxIndex, LoadBoxes()).Name
    End Function

    Private Shared Function GetBox(ByVal index As Integer, ByVal boxes As List(Of Box)) As Box
        Return boxes.FirstOrDefault(Function(x) x.index = index)
    End Function

    Private Function GetBox(ByVal index As Integer) As Box
        Return GetBox(index, Me.Boxes)
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

        Public ReadOnly Property pokemon As Pokemon
            Get
                If _loaded Then Return Me._pokemon
                _loaded = True
                _pokemon = Pokemon.GetPokemonByData(Me._pokemonData)
                Return Me._pokemon
            End Get
        End Property

        Public ReadOnly Property PokemonData() As String
            Get
                Return If(_loaded, Me._pokemon.GetSaveData(), Me._pokemonData)
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
            Me.Name = CStr(Localization.GetString("storage_screen_box_DefaultName", "BOX [NUMBER]")).Replace("[NUMBER]", $"{index + 1}")
            Me.Background = index
        End Sub

        Public ReadOnly Property HasPokemon() As Boolean
            Get
                Return (Pokemon.Count > 0)
            End Get
        End Property

        Public ReadOnly Property IsFull() As Boolean
            Get
                Return (Pokemon.Count = 30)
            End Get
        End Property

        Public Function GetPokemonList() As List(Of Pokemon)
            Return Pokemon.Values.Select(Function(x) x.pokemon).ToList()
        End Function

        Public Property IsBattleBox() As Boolean
            Get
                Return Me._isBattleBox
            End Get
            Set(value As Boolean)
                Me._isBattleBox = value
                If Me._isBattleBox Then Me.Name = Localization.GetString("storage_screen_box_BattleBoxName", "BATTLE BOX")
            End Set
        End Property

    End Class

    Class MenuEntry

        Public Index As Integer = 0

        Public Text As String = "Menu"
        Public IsBack As Boolean = False
        Public Delegate Sub ClickEvent(ByVal m As MenuEntry)
        Public ClickHandler As ClickEvent = Nothing

        Dim t1 As Texture2D
        Dim t2 As Texture2D

        Public Sub New(ByVal Index As Integer, ByVal text As String, ByVal isBack As Boolean, ByVal ClickHandler As ClickEvent)
            Me.Index = Index

            Me.Text = text
            Me.IsBack = isBack
            Me.ClickHandler = ClickHandler

            t1 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), "")
            t2 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), "")
        End Sub

        Public Sub Update(ByVal s As StorageSystemScreen)
            Dim hovering = New Rectangle(Core.windowSize.Width - 270, 66 * Index, 256, 64).Contains(MouseHandler.MousePosition)
            Dim acceptPointer = Controls.Accept(True, False, False) And hovering
            Dim acceptButtons = Controls.Accept(False, True, True)
            Dim dismiss = Controls.Dismiss(True, True, True) And Me.IsBack
            If (acceptPointer Or acceptButtons) And s.MenuCursor = Me.Index Or dismiss Then
                s.MenuVisible = False
                ClickHandler?(Me)
            End If
            If acceptPointer Then
                s.MenuCursor = Me.Index
            End If
        End Sub

        Public Sub Draw(ByVal CursorIndex As Integer, ByVal CursorTexture As Texture2D)
            Dim startPos = New Point(Core.windowSize.Width - 270, 66 * Index)

            Core.SpriteBatch.Draw(t1, New Rectangle(startPos.X, startPos.Y, 64, 64), Color.White)
            Core.SpriteBatch.Draw(t2, New Rectangle(startPos.X + 64, startPos.Y, 64, 64), Color.White)
            Core.SpriteBatch.Draw(t2, New Rectangle(startPos.X + 128, startPos.Y, 64, 64), Color.White)
            Core.SpriteBatch.Draw(t1, New Rectangle(startPos.X + 192, startPos.Y, 64, 64), Nothing, Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Dim font = FontManager.MainFont
            Dim textSize = font.MeasureString(Me.Text).X * 0.7F
            Dim position = New Vector2(startPos.X + 128 - textSize, startPos.Y + 15)
            Core.SpriteBatch.DrawString(font, Me.Text, position, Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

            If Me.Index <> CursorIndex Then Return
            Dim cPosition = startPos + New Point(128, -40)
            Core.SpriteBatch.Draw(CursorTexture, New Rectangle(cPosition, New Point(64)), Color.White)
        End Sub

    End Class

    Public Shared Function GetAllBoxPokemon() As List(Of Pokemon)
        Dim Pokemons = New List(Of Pokemon)
        Dim Data() = Core.Player.BoxData.SplitAtNewline()
        For Each line In Data
            If Not line.StartsWith("BOX|") Or line = "" Then Continue For
            Dim pokeData = line.Remove(0, line.IndexOf("{"))
            Pokemons.Add(Pokemon.GetPokemonByData(pokeData))
        Next
        Return Pokemons
    End Function

    Public Function GetPokemonList(ByVal includeTeam As Boolean, ByVal lit As Boolean) As List(Of Pokemon)
        Dim L = New List(Of Pokemon)
        For Each Box In Me.Boxes
            If Not Box.HasPokemon Then Continue For
            Dim pokemons = Box.Pokemon.Values.Select(Function(x) x.pokemon)
            L.AddRange(pokemons.Where(Function(pokemon) (lit AndAlso IsLit(pokemon)) Or Not lit))
        Next

        If includeTeam Then
            L.AddRange(Core.Player.Pokemons.Where(Function(pokemon) (lit AndAlso IsLit(pokemon)) Or Not lit))
        End If

        Return L
    End Function

    Public Shared Function GetBattleBoxPokemon() As List(Of Pokemon)
        Dim BattleBoxID = 0
        Dim Data() = Core.Player.BoxData.SplitAtNewline()
        Dim PokemonList = New List(Of Pokemon)

        For Each line In Data
            If Not line.StartsWith("BOX|") Then Continue For
            Dim boxData() = line.Split(CChar("|"))
            BattleBoxID = Math.Min(CInt(boxData(1)), BattleBoxID)
        Next
        For Each line In Data
            If Not line.StartsWith(BattleBoxID.ToString() & ",") Or Not line.EndsWith("}") Then Continue For
            Dim pokemonData = line.Remove(0, line.IndexOf("{"))
            PokemonList.Add(Pokemon.GetPokemonByData(pokemonData))
        Next

        If PokemonList.Count > 6 Then
            PokemonList.RemoveRange(5, PokemonList.Count - 6)
        End If

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
            If Not Visible Then Return
            If Controls.Up(True, True, True, True, True, True) Then Me.Index -= 1
            If Controls.Down(True, True, True, True, True, True) Then Me.Index += 1
            Me.Index = Me.Index.Clamp(0, Me.Items.Count - 1)

            For i = Scroll To Me.Scroll + 8
                If i > Me.Items.Count - 1 Then Continue For
                Dim hovering = New Rectangle(Core.windowSize.Width - 270, 66 * (i + 1 - Scroll), 256, 64).Contains(MouseHandler.MousePosition)
                Dim acceptPointer = Controls.Accept(True, False, False) And hovering
                Dim acceptButtons = Controls.Accept(False, True, True)
                Dim dismiss = Controls.Dismiss(True, True, True)
                If (acceptPointer Or acceptButtons) And i = Me.Index Or dismiss And Me.BackIndex = Me.Index Then

                    If ClickHandler IsNot Nothing Then
                        ClickHandler(Me)
                        SoundManager.PlaySound("select")
                    End If
                    Me.Visible = False
                End If
                If dismiss Then Me.Index = Me.BackIndex
                If acceptPointer Then Me.Index = i
            Next

            If Index - Scroll > 8 Then Scroll = Index - 8
            If Index - Scroll < 0 Then Scroll = Index
        End Sub

        Public Sub Draw()
            If Not Visible Then Return
            For i = Scroll To Me.Scroll + 8
                If i > Me.Items.Count - 1 Then Continue For
                Dim Text = Items(i)

                Dim startPos = New Point(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll))

                Core.SpriteBatch.Draw(t1, New Rectangle(startPos.X, startPos.Y, 64, 64), Color.White)
                Core.SpriteBatch.Draw(t2, New Rectangle(startPos.X + 64, startPos.Y, 64, 64), Color.White)
                Core.SpriteBatch.Draw(t2, New Rectangle(startPos.X + 128, startPos.Y, 64, 64), Color.White)
                Core.SpriteBatch.Draw(t1, New Rectangle(startPos.X + 192, startPos.Y, 64, 64), Nothing, Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                Dim font = FontManager.MainFont
                Dim textSize = font.MeasureString(Text).X * 0.7F
                Dim position = New Vector2(startPos.X + 128 - textSize, startPos.Y + 15)
                Core.SpriteBatch.DrawString(font, Text, position, Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

                If Me.Index <> i Then Continue For
                Dim cPosition = startPos + New Point(128, -40)
                Dim t = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
                Core.SpriteBatch.Draw(t, New Rectangle(cPosition, New Point(64)), Color.White)
            Next
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

        Me.Filters.AddRange(currentScreen.Filters)

        Me.MouseVisible = True
        Me.CanMuteAudio = True
        Me.CanBePaused = True

        Me.mainMenuItems = {Localization.GetString("storage_screen_filter_Pokemon", "Pokémon"), Localization.GetString("storage_screen_filter_Type1", "Type1"), Localization.GetString("storage_screen_filter_Type2", "Type2"), Localization.GetString("storage_screen_filter_Move", "Move"), Localization.GetString("storage_screen_filter_Ability", "Ability"), Localization.GetString("storage_screen_filter_Nature", "Nature"), Localization.GetString("storage_screen_filter_Gender", "Gender"), Localization.GetString("storage_screen_filter_HeldItem", "Held Item")}.ToList()

        Me.Menu = New SelectMenu({""}.ToList(), 0, Nothing, 0)
        Me.Menu.Visible = False
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Dim source = New Rectangle(48, 0, 16, 16)
            Dim destination = New Rectangle(Core.windowSize.Width - 128, y + StorageSystemScreen.TileOffset, 128, 64)
            Core.SpriteBatch.Draw(Me.texture, destination, source, Color.White)
        Next
        Dim tones = (A:=New Color(42, 167, 198), B:=New Color(42, 167, 198, 0))

        Canvas.DrawGradient(New Rectangle(0, 0, Core.windowSize.Width, 200), tones.A, tones.B, False, -1)
        Canvas.DrawGradient(New Rectangle(0, Core.windowSize.Height - 200, Core.windowSize.Width, 200), tones.B, tones.A, False, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("storage_screen_filter_Title", "Configure the filters:"), New Vector2(100, 24), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        For i = Scroll To Scroll + 5
            If i > Me.mainMenuItems.Count - 1 Then Continue For
            Dim p = i - Scroll

            Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 100 + p * 96, 64 * 8, 64), New Rectangle(32, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 9, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Dim filterText = GetFilterText(mainMenuItems(i))
            Dim s = If(filterText <> "", $"{mainMenuItems(i)} ({filterText})", mainMenuItems(i))
            Dim sourceRectangle = If(filterText <> "", New Rectangle(16, 48, 16, 16), New Rectangle(16, 32, 16, 16))
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(120, 116 + p * 96, 32, 32), sourceRectangle, Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, s, New Vector2(160, 116 + p * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
        Next

        If Filters.Count > 0 Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, $"{Localization.GetString("storage_screen_filter_Results", "Results:")} {Environment.NewLine}{Environment.NewLine}{Localization.GetString("storage_screen_filter_Filters", "Filters:")} ", New Vector2(90 + 64 * 11, 119), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MainFont, $"{Me.Results}{Environment.NewLine}{Environment.NewLine}{Me.Filters.Count}", New Vector2(190 + 64 * 11, 119), Color.White)
        End If

        Dim draw = If(Menu.Visible, CType(AddressOf Menu.Draw, Action), AddressOf DrawCursor)
        draw()
    End Sub

    Private Function GetFilterText(ByVal filterTypeString As String) As String
        Dim Equals = Function(f As StorageSystemScreen.Filter?) $"{f.Value.FilterType}".ToLower() = filterTypeString.ToLower()
        Dim filter = Me.Filters.Cast(Of StorageSystemScreen.Filter?).FirstOrDefault(Equals)
        Return If(filter.HasValue, filter.Value.FilterValue, "")
    End Function

    Private Sub DrawCursor()
        Dim cPosition = New Point(520, 100 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(cPosition, New Point(64)), Color.White)
    End Sub

    Private Sub ApplyFilters()
        Me._storageSystemScreen.Filters.Clear()
        Me._storageSystemScreen.Filters.AddRange(Me.Filters)
    End Sub

    Public Overrides Sub Update()
        If Menu.Visible Then
            Menu.Update()
        Else
            Dim direction = 0
            If Controls.Down(True, True, True, True, True, True) Then direction = 1
            If Controls.Up(True, True, True, True, True, True) Then direction = -1
            Me.Cursor += direction
            If Controls.ShiftDown() Then Me.Cursor += direction * 4

            While Me.Cursor > 5
                Me.Cursor -= 1
                Me.Scroll += 1
            End While
            While Me.Cursor < 0
                Me.Cursor += 1
                Me.Scroll -= 1
            End While

            Me.Scroll = If(Me.mainMenuItems.Count < 7, 0, Me.Scroll.Clamp(0, Me.mainMenuItems.Count - 6))

            Me.Cursor = If(Me.mainMenuItems.Count < 6, Me.Cursor.Clamp(0, Me.mainMenuItems.Count - 1), Me.Cursor.Clamp(0, 5))

            If Me.mainMenuItems.Count > 0 Then
                If Controls.Accept(True, False, False) Then
                    For i = Scroll To Scroll + 5
                        Dim hovering = New Rectangle(100, 100 + (i - Scroll) * 96, 640, 64).Contains(MouseHandler.MousePosition)
                        If i > Me.mainMenuItems.Count - 1 OrElse Not hovering Then Continue For
                        If i <> Cursor + Scroll Then
                            Cursor = i - Scroll
                            Continue For
                        End If
                        SelectFilter()
                        SoundManager.PlaySound("select")
                    Next
                End If

                If Controls.Accept(False, True, True) Then
                    SelectFilter()
                    SoundManager.PlaySound("select")
                End If
            End If

            If Controls.Dismiss(True, True, True) Then
                ApplyFilters()
                Core.SetScreen(Me._storageSystemScreen)
                SoundManager.PlaySound("select")
            End If
        End If

        CalculateResults()

        StorageSystemScreen.TileOffset = (StorageSystemScreen.TileOffset + 1) Mod 64
    End Sub

    Private Sub CalculateResults()
        Dim s = ""
        Dim s1 = ""
        Me.CalculatedFilters.ForEach(Sub(f) s &= $"{f.FilterType}|{f.FilterValue}")
        Me.Filters.ForEach(Sub(f) s1 &= $"{f.FilterType}|{f.FilterValue}")

        If s1 = s Then Return
        Me.CalculatedFilters.Clear()
        Me.CalculatedFilters.AddRange(Me.Filters)
        ApplyFilters()
        Me.Results = Me._storageSystemScreen.GetPokemonList(True, True).Count
    End Sub

    Private Sub SelectFilter()
        Dim filterType = Me.mainMenuItems(Me.Scroll + Me.Cursor).ToLower()
        Dim menus = New Dictionary(Of String, Action)
        menus.Add(Localization.GetString("storage_screen_filter_Pokemon", "Pokémon").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Pokémon, True))
        menus.Add(Localization.GetString("storage_screen_filter_Type1", "Type1").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Type1))
        menus.Add(Localization.GetString("storage_screen_filter_Type2", "Type2").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Type2))
        menus.Add(Localization.GetString("storage_screen_filter_Move", "Move").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Move, True))
        menus.Add(Localization.GetString("storage_screen_filter_Ability", "Ability").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Ability, True))
        menus.Add(Localization.GetString("storage_screen_filter_Nature", "Nature").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Nature))
        menus.Add(Localization.GetString("storage_screen_filter_Gender", "Gender").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.Gender))
        menus.Add(Localization.GetString("storage_screen_filter_HeldItem", "Held Item").ToLower, Sub() Me.OpenMenu(StorageSystemScreen.FilterTypes.HeldItem))
        If menus.ContainsKey(filterType) Then menus(filterType)()
    End Sub

#Region "Filtering"
    Private Sub OpenMenu(filterType As StorageSystemScreen.FilterTypes, Optional letterFiltering As Boolean = False)
        Dim l = Me._storageSystemScreen.GetPokemonList(True, False)
        Dim GetNames = New Dictionary(Of StorageSystemScreen.FilterTypes, Func(Of IEnumerable(Of String)))
        GetNames.Add(StorageSystemScreen.FilterTypes.Pokémon, Function() l.Select(Function(pokemon) pokemon.GetName()))
        GetNames.Add(StorageSystemScreen.FilterTypes.Type1, Function() l.Select(Function(p) $"{p.Type1}"))
        GetNames.Add(StorageSystemScreen.FilterTypes.Type2, Function() l.Select(Function(p) $"{p.Type2}"))
        GetNames.Add(StorageSystemScreen.FilterTypes.Move, Function() l.SelectMany(Function(p) p.Attacks).Select(Function(a) a.Name))
        GetNames.Add(StorageSystemScreen.FilterTypes.Ability, Function() l.Select(Function(p) p.Ability.Name))
        GetNames.Add(StorageSystemScreen.FilterTypes.Nature, Function() l.Select(Function(p) $"{p.Nature}"))
        GetNames.Add(StorageSystemScreen.FilterTypes.Gender, Function() l.Select(Function(p) $"{p.Gender}"))
        GetNames.Add(StorageSystemScreen.FilterTypes.HeldItem, Function() {Localization.GetString("storage_screen_filter_HeldItem_Yes", "Has a Held Item"), Localization.GetString("storage_screen_filter_HeldItem_No", "Has no Held Item")})
        Dim names = GetNames(filterType)().Distinct().ToList()
        names.Sort()
        Dim letters = If(letterFiltering, names.Select(Function(name) $"{name(0)}".ToUpper()).Distinct.ToList(), Nothing)
        Dim items = If(letterFiltering, letters, names)
        Dim OnClick = If(letterFiltering, Sub(s As SelectMenu) Me.SelectLetter(s, names, filterType), Sub(s) Me.SelectType(s, filterType))
        items.Add(Localization.GetString("global_back", Localization.GetString("global_back", "Back")))
        If GetFilterText($"{filterType}") <> "" Then items.Insert(0, Localization.GetString("global_clear", "Clear"))
        Me.Menu = New SelectMenu(items, 0, OnClick, -1)
    End Sub

    Private Sub SelectLetter(ByVal s As SelectMenu, names As List(Of String), filterType As StorageSystemScreen.FilterTypes)
        If s.SelectedItem = Localization.GetString("global_back", "Back") Then Return
        If s.SelectedItem = Localization.GetString("global_clear", "Clear") Then
            Me.Filters.RemoveAll(Function(filter) filter.FilterType = filterType)
            Return
        End If
        Dim chosenLetter = s.SelectedItem
        Dim buttonNames = names.Where(Function(x) x.ToUpper().StartsWith(chosenLetter)).Distinct().ToList()
        buttonNames.Sort()
        buttonNames.Add(Localization.GetString("global_back", "Back"))
        Me.Menu = New SelectMenu(buttonNames, 0, Sub(x) Me.SelectType(x, filterType, False), -1)
    End Sub

    Private Sub SelectType(ByVal s As SelectMenu, filterType As StorageSystemScreen.FilterTypes, Optional backIsRoot As Boolean = True)
        If s.SelectedItem = Localization.GetString("global_back", "Back") Then
            If Not backIsRoot Then Me.OpenMenu(filterType, True)
            Return
        End If
        Me.Filters.RemoveAll(Function(filter) filter.FilterType = filterType)
        If s.SelectedItem = Localization.GetString("global_clear", "Clear") Then Return
        Me.Filters.Add(New StorageSystemScreen.Filter() With {.FilterType = filterType, .FilterValue = s.SelectedItem})
    End Sub
#End Region

End Class