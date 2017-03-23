Imports net.Pokemon3D.Game.Screens.UI

''' <summary>
''' Displays the inventory and gives the player options to choose and use items.
''' </summary>
Public Class NewInventoryScreen

    Inherits Screen
    Implements ISelectionScreen

    Private _translation As Globalization.Classes.LOCAL_InventoryScreen

    Private target_1 As RenderTarget2D
    Private target_2 As RenderTarget2D
    Private itemBatch As CoreSpriteBatch
    Private infoBatch As CoreSpriteBatch

    ''' <summary>
    ''' Texture from file: GUI\Menus\General
    ''' </summary>
    Private _texture As Texture2D

    ''' <summary>
    ''' Texture from file: GUI\Menus\Inventory
    ''' </summary>
    Private _menuTexture As Texture2D

    'Stores the current tab index and if the player is controlling the tab list:
    Private _tabIndex As Integer = 0
    Private _tabInControl As Boolean = True

    'We have 8 indexes for the current page and item on that page (0-9) here, for each tab:
    Private _itemindex As Integer() = {0, 0, 0, 0, 0, 0, 0, 0}
    Private _pageIndex As Integer() = {0, 0, 0, 0, 0, 0, 0, 0}

    ''' <summary>
    ''' The item index of the current tab and page:
    ''' </summary>
    Private Property ItemIndex As Integer
        Get
            Return _itemindex(_tabIndex)
        End Get
        Set(value As Integer)
            _itemindex(_tabIndex) = value
        End Set
    End Property

    ''' <summary>
    ''' The page index for the current tab.
    ''' </summary>
    Private Property PageIndex As Integer
        Get
            Return _pageIndex(_tabIndex)
        End Get
        Set(value As Integer)
            _pageIndex(_tabIndex) = value
        End Set
    End Property

    'Stores a value from 0-255 for each tab that determines their light state when scrolling over it.
    Private _tabHighlight As Integer() = {0, 0, 0, 0, 0, 0, 0, 0}

    'Interface animation state values:
    Private _interfaceFade As Single = 0F
    Private _closing As Boolean = False
    Private _enrollY As Single = 0F
    Private _itemIntro As Single = 0F

    'Item animation:
    Private Class ItemAnimation
        Public _shakeV As Single
        Public _shakeLeft As Boolean
        Public _shakeCount As Integer
    End Class
    'The current state for the item animations:
    Private _itemAnimation As ItemAnimation

    'If the info popup is visible:
    Private _isInfoShowing As Boolean = False

    'Info popup state information:
    Private _infoSide As Integer = 0 '0 = left, 1 = right
    Private _infoSize As Integer = 0
    Private _infoSizeTarget As Integer = 0
    Private _infoPosition As Integer = 0
    Private _infoPositionTarget As Integer = 0

    Private _itemColumnLeft As Integer = 0 'Until which item column is considered left
    Private _itemColumnLeftOffset As Integer = 0
    Private _itemColumnLeftOffsetTarget As Integer = 0
    Private _itemColumnRightOffset As Integer = 0
    Private _itemColumnRightOffsetTarget As Integer = 0

    'Selectable options for items.
    ' - Use
    ' - Give (to pokémon)
    ' - Toss
    ' - Select

    '#EC_NO_TRANSLATION:
    Private Const INFO_ITEM_OPTION_USE As String = "USE"
    Private Const INFO_ITEM_OPTION_GIVE As String = "GIVE"
    Private Const INFO_ITEM_OPTION_TOSS As String = "TOSS"
    Private Const INFO_ITEM_OPTION_SELECT As String = "SELECT"

    Private _infoItemOptions As New List(Of String)
    Private _infoItemOptionsNormal As New List(Of String) 'Contains untranslated strings.
    Private _infoItemOptionSize As Integer() = {0, 0, 0}
    Private _infoItemOptionSelection As Integer = 0

    'Items for the current tab:
    Private _items As PlayerInventory.ItemContainer()

    'Displays a message box:
    Private _messageDelay As Single = 0F
    Private _messageText As String = ""

    'experiment
    Public Delegate Sub DoStuff(ByVal ItemID As Integer)
    Dim ReturnItem As DoStuff
    Dim AllowedPages() As Integer

    Public Sub New(ByVal currentScreen As Screen, ByVal AllowedPages As Integer(), ByVal StartPageIndex As Integer, ByVal DoStuff As DoStuff)

        _tabIndex = StartPageIndex
        Me.AllowedPages = AllowedPages
        ReturnItem = DoStuff

        _translation = New Globalization.Classes.LOCAL_InventoryScreen()
        target_1 = New RenderTarget2D(GraphicsDevice, 816, 400 - 32, False, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)
        target_2 = New RenderTarget2D(GraphicsDevice, 500, 368)
        itemBatch = New CoreSpriteBatch(GraphicsDevice)
        infoBatch = New CoreSpriteBatch(GraphicsDevice)

        Identification = Identifications.InventoryScreen
        PreScreen = currentScreen
        IsDrawingGradients = True

        _texture = TextureManager.GetTexture("GUI\Menus\General")
        _menuTexture = TextureManager.GetTexture("GUI\Menus\Inventory")

        ''DEBUG: Add all items in the game to the inventory:
        'For i = 1 To 2500
        '    Dim cItem As Item = Item.GetItemByID(i)

        '    If Not cItem Is Nothing Then
        '        Core.Player.Inventory.AddItem(cItem.ID, 1)
        '    End If
        'Next

        ResetAnimation()

        ' Set up the default visible item types configuration:
        _visibleItemTypes = New Item.ItemTypes() {Item.ItemTypes.Standard,
                                                  Item.ItemTypes.Medicine,
                                                  Item.ItemTypes.Plants,
                                                  Item.ItemTypes.Pokéballs,
                                                  Item.ItemTypes.Machines,
                                                  Item.ItemTypes.Mail,
                                                  Item.ItemTypes.BattleItems,
                                                  Item.ItemTypes.KeyItems}

        'TODO: Load state information from the PlayerTemp.

        _tabHighlight(_tabIndex) = 255

        'Load the items once when loading up the inventory screen:
        LoadItems()
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal AllowedPages() As Integer, ByVal DoStuff As DoStuff)
        Me.New(currentScreen, AllowedPages, Player.Temp.BagIndex, DoStuff)
    End Sub

    Public Sub New(ByVal currentScreen As Screen)
        Me.New(currentScreen, {}, Player.Temp.BagIndex, Nothing)
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        DrawGradients(CInt(255 * _interfaceFade))

        DrawTabs()
        DrawMain()

        DrawMessage()

        PokemonImageView.Draw()
        TextBox.Draw()
    End Sub

    ''' <summary>
    ''' Draws the temporary message.
    ''' </summary>
    Private Sub DrawMessage()
        If _messageDelay > 0F Then
            Dim textFade As Single = 1.0F
            If _messageDelay <= 1.0F Then
                textFade = _messageDelay
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 150), CInt(Core.windowSize.Height - 200), 300, 100), New Color(0, 0, 0, CInt(150 * textFade * _interfaceFade)))

            Dim text As String = _messageText.CropStringToWidth(FontManager.ChatFont, 250)
            Dim size As Vector2 = FontManager.ChatFont.MeasureString(text)

            SpriteBatch.DrawString(FontManager.ChatFont, text, New Vector2(CSng(Core.windowSize.Width / 2 - size.X / 2), CSng(Core.windowSize.Height - 150 - size.Y / 2)), New Color(255, 255, 255, CInt(255 * textFade * _interfaceFade)))
        End If
    End Sub

    ''' <summary>
    ''' Draws the tabs on the top of the UI.
    ''' </summary>
    Private Sub DrawTabs()
        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        For x = 0 To 368 Step 16
            Dim cTabIndex As Integer = CInt(Math.Floor(x / 48))
            Dim bgColor As Color = Color.White

            If cTabIndex <> _tabIndex And cTabIndex < _tabHighlight.Length Then
                Dim gC As Integer = 128 + CInt(128 * (_tabHighlight(cTabIndex) / 255))
                bgColor = New Color(gC, gC, gC)
            End If

            If _closing Then
                bgColor = New Color(bgColor.R, bgColor.G, bgColor.B, CInt(CInt(bgColor.A) * _interfaceFade))
            End If

            For y = 0 To 32 Step 16
                SpriteBatch.Draw(_menuTexture, New Rectangle(halfWidth - 400 + x, halfHeight - 200 + y, 16, 16), New Rectangle(0, 0, 4, 4), bgColor)
            Next
            If x Mod 48 = 32 Then
                SpriteBatch.Draw(_menuTexture, New Rectangle(halfWidth - 400 + x - 24, halfHeight - 200 + 8, 32, 32), GetTabImageRect(cTabIndex), bgColor)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Draws the main content.
    ''' </summary>
    Private Sub DrawMain()
        'Calculate the center of the screen:
        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        'When the interface is fading in/out, use a custom alpha:
        Dim mainBackgroundColor As Color = Color.White
        If _closing = True Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

        'Draw background pattern:
        For y = 48 To CInt(_enrollY) Step 16
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_menuTexture, New Rectangle(halfWidth - 400 + x, halfHeight - 200 + y, 16, 16), New Rectangle(0, 0, 4, 4), mainBackgroundColor)
            Next
        Next

        'This draws the lowest row so the background is not cut off:
        Dim modRes As Integer = CInt(_enrollY) Mod 16
        If modRes > 0 Then
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_menuTexture, New Rectangle(halfWidth - 400 + x, CInt(_enrollY + (halfHeight - 200)), 16, modRes), New Rectangle(0, 0, 4, 4), mainBackgroundColor)
            Next
        End If

        'Create a render target and render the items to it.
        'We do this because the items move to the left/right "outside" of the screen.
        'So we don't need to fiddle around with the offset, we just move them outside the render target.

        If CInt(_enrollY) - 32 > 0 Then 'Only draw, when the size is at least 1 pixel high.

            'Bring back when Monogame begins supporting this stuff
            '   Dim target As New RenderTarget2D(GraphicsDevice, 816, CInt(_enrollY) - 32, False, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents)
            GraphicsDevice.SetRenderTarget(target_1)
            GraphicsDevice.Clear(Color.Transparent)

            'Create a designated sprite batch:

            itemBatch.BeginBatch()

            Dim itemPanelAlpha As Integer = CInt(If(_tabInControl, 180, 255) * _interfaceFade)

            Dim iX, iY As Integer
            For i = 0 To 9
                iX = CInt(Math.Floor(i / 2))
                iY = i Mod 2

                If _items.Length > i + (PageIndex * 10) Then
                    Dim cItem = Item.GetItemByID(_items(i + (PageIndex * 10)).ItemID)

                    If _itemIntro >= i / 10 Then
                        Dim Yoffset As Integer = 0
                        If _itemIntro < (i + 1) / 10 Then
                            Yoffset = CInt((_itemIntro - (i / 10)) * 80)
                        End If

                        Dim XOffset As Integer = ComputeXOffset(iX)

                        Dim itemLoc = New Vector2(iX * 160 + 32 + XOffset, 48 + iY * 160 + 32)

                        Dim size As Integer = If(i = ItemIndex, 96, 72)

                        itemBatch.Draw(cItem.Texture, New Rectangle(CInt(itemLoc.X) + 48, CInt(itemLoc.Y) + Yoffset, size, size), Nothing, New Color(255, 255, 255, itemPanelAlpha),
                                       If(i = ItemIndex, _itemAnimation._shakeV, 0F), New Vector2(cItem.Texture.Width / 2.0F), SpriteEffects.None, 0F)

                        Canvas.DrawRectangle(itemBatch, New Rectangle(CInt(itemLoc.X) - 16, CInt(itemLoc.Y) + 48, 128, 24), New Color(0, 0, 0, CInt(If(_tabInControl, 64, 128) * _interfaceFade)))

                        Dim fontWidth As Integer = CInt(FontManager.MiniFont.MeasureString(cItem.Name).X)

                        itemBatch.DrawString(FontManager.MiniFont, cItem.Name, itemLoc + New Vector2(48 - fontWidth / 2.0F, 51), New Color(255, 255, 255, itemPanelAlpha))

                        itemBatch.DrawString(FontManager.MiniFont, "x" & _items(i + (PageIndex * 10)).Amount.ToString(), itemLoc + New Vector2(84, 26), New Color(40, 40, 40, itemPanelAlpha))
                    End If
                End If
            Next

            'When the info is visible, draw it:
            If _infoSize > 0 Then
                DrawInfo(itemBatch, target_1)
            End If

            itemBatch.EndBatch()

            'Reset to back buffer and render the result:
            GraphicsDevice.SetRenderTarget(Nothing)

            Dim drawheight As Integer = 368
            If _closing Then
                drawheight = CInt(_enrollY) - 32
            End If

            SpriteBatch.Draw(target_1, New Rectangle(halfWidth - 400, halfHeight - 200 + 48, 816, drawheight), mainBackgroundColor)

        End If
    End Sub

    ''' <summary>
    ''' Draws the info popup.
    ''' </summary>
    Private Sub DrawInfo(ByVal preBatch As SpriteBatch, ByVal preTarget As RenderTarget2D)
        'Create a new render target and set it.

        'Bring back when Monogame begins supporting this stuff
        '   Dim target As New RenderTarget2D(GraphicsDevice, _infoSize, 368, False, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents)
        GraphicsDevice.SetRenderTarget(target_2)

        'Render background:
        infoBatch.BeginBatch()
        For y = 0 To 368 Step 16
            For x = 0 To _infoSize + 16 Step 16
                infoBatch.Draw(_menuTexture, New Rectangle(x, y, 16, 16), New Rectangle(0, 0, 4, 4), New Color(128, 128, 128))
            Next
        Next

        Canvas.DrawGradient(infoBatch, New Rectangle(0, 0, 100, 368), New Color(0, 0, 0, 255), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawGradient(infoBatch, New Rectangle(_infoSize - 100, 0, 100, 368), New Color(0, 0, 0, 0), New Color(0, 0, 0, 255), True, -1)

        'Get item and gets its display texts based on the item category:
        Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)

        infoBatch.Draw(cItem.Texture, New Rectangle(24, 24, 48, 48), Color.White)

        Dim itemTitle As String = cItem.Name
        Dim itemSubTitle As String = cItem.ItemType.ToString()
        Dim itemDescription As String = cItem.Description

        Select Case cItem.ItemType
            Case Item.ItemTypes.Machines
                Dim techMachine = CType(cItem, Items.TechMachine)

                itemTitle = techMachine.Attack.Name

                If techMachine.IsTM Then
                    itemSubTitle = _translation.TECH_MACHINE_TITLE(cItem.ItemType.ToString())
                Else
                    itemSubTitle = _translation.HIDDEN_MACHINE_TITLE(cItem.ItemType.ToString())
                End If

                itemDescription &= vbNewLine & techMachine.Attack.Description
            Case Item.ItemTypes.Standard
                itemSubTitle = _translation.STANDARD_ITEM_TITLE(cItem.ItemType.ToString())
            Case Item.ItemTypes.KeyItems
                itemSubTitle = _translation.KEYITEM_TITLE(cItem.ItemType.ToString())
            Case Item.ItemTypes.Pokéballs
                itemSubTitle = _translation.POKEBALL_TITLE(cItem.ItemType.ToString())
            Case Item.ItemTypes.Plants
                itemSubTitle = _translation.PLANT_TITLE(cItem.ItemType.ToString())
            Case Item.ItemTypes.BattleItems
                itemSubTitle = _translation.BATTLEITEM_TITLE(cItem.ItemType.ToString())
        End Select

        infoBatch.DrawString(FontManager.TextFont, itemTitle, New Vector2(80, 20), Color.White, 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)
        infoBatch.DrawString(FontManager.TextFont, itemSubTitle, New Vector2(80, 46), Color.LightGray, 0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0F)
        infoBatch.DrawString(FontManager.TextFont, itemDescription.CropStringToWidth(FontManager.TextFont, 1.0F, 430), New Vector2(28, 84), Color.LightGray, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        For i = 0 To _infoItemOptions.Count - 1
            Canvas.DrawRectangle(infoBatch, New Rectangle(CInt(250 - _infoItemOptionSize(i) / 2), 158 + i * 64, _infoItemOptionSize(i), 48), New Color(255, 255, 255, 20))

            infoBatch.DrawString(FontManager.TextFont, _infoItemOptions(i), New Vector2(CInt(250 - FontManager.TextFont.MeasureString(_infoItemOptions(i)).X), 168 + i * 64), Color.White, 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)
        Next

        infoBatch.EndBatch()

        'Set the target that was previously active and render the new target on top of that:
        GraphicsDevice.SetRenderTarget(preTarget)
        preBatch.Draw(target_2, New Rectangle(_infoPosition + 80, 0, target_2.Width, target_2.Height), Color.White)
    End Sub

    ''' <summary>
    ''' Determines the x-offset of an item based on the <see cref="_itemColumnLeft"/> value.
    ''' </summary>
    Private Function ComputeXOffset(ByVal itemPositionId As Integer) As Integer
        Dim i As Integer = itemPositionId

        'Determine if we are in the left or right portion:
        Dim isLeft As Boolean = _itemColumnLeft >= i

        If isLeft Then
            Return _itemColumnLeftOffset
        Else
            Return _itemColumnRightOffset
        End If
    End Function

    ''' <summary>
    ''' Returns the texture rectangle that contains the tab image from GUI\Menus\Inventory.
    ''' </summary>
    ''' <param name="i">The tab index.</param>
    Private Function GetTabImageRect(ByVal i As Integer) As Rectangle
        Select Case _visibleItemTypes(i)
            Case Item.ItemTypes.Standard
                Return New Rectangle(0, 16, 16, 16)
            Case Item.ItemTypes.Medicine
                Return New Rectangle(16, 16, 16, 16)
            Case Item.ItemTypes.Plants
                Return New Rectangle(0, 32, 16, 16)
            Case Item.ItemTypes.Pokéballs
                Return New Rectangle(48, 16, 16, 16)
            Case Item.ItemTypes.Machines
                Return New Rectangle(32, 16, 16, 16)
            Case Item.ItemTypes.Mail
                Return New Rectangle(16, 32, 16, 16)
            Case Item.ItemTypes.BattleItems
                Return New Rectangle(48, 32, 16, 16)
            Case Item.ItemTypes.KeyItems
                Return New Rectangle(32, 32, 16, 16)
        End Select
    End Function

    Public Overrides Sub Update()
        'Updates the tab highlight:
        For index = 0 To _tabHighlight.Length - 1
            If index <> _tabIndex Then
                If _tabHighlight(index) > 0 Then
                    _tabHighlight(index) -= 15
                    If _tabHighlight(index) < 0 Then
                        _tabHighlight(index) = 0
                    End If
                End If
            Else
                _tabHighlight(index) = 255
            End If
        Next

        'Update the message:
        If _messageDelay > 0F Then
            _messageDelay -= 0.1F
            If _messageDelay <= 0F Then
                _messageDelay = 0F
            End If
        End If

        If _closing Then
            ' When the interface is closing, only update the closing animation
            ' Once the interface is completely closed, set to the previous screen.

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
                'TODO: Set the interface state to PlayerTemp.
                SetScreen(PreScreen)
            End If
        Else
            'Update intro animation:
            Dim maxWindowHeight As Integer = 400
            If _enrollY < maxWindowHeight Then
                _enrollY = MathHelper.Lerp(maxWindowHeight, _enrollY, 0.8F)
                If _enrollY >= maxWindowHeight Then
                    _enrollY = maxWindowHeight
                End If
            End If
            If _interfaceFade < 1.0F Then
                _interfaceFade = MathHelper.Lerp(1.0F, _interfaceFade, 0.95F)
                If _interfaceFade > 1.0F Then
                    _interfaceFade = 1.0F
                End If
            End If
            If _itemIntro < 1.0F Then
                _itemIntro += 0.05F
                If _itemIntro > 1.0F Then
                    _itemIntro = 1.0F
                End If
            End If

            'Control update:
            UpdateShakeAnimation()

            ' Input update:
            If TextBox.Showing = False And ChooseBox.Showing = False And PokemonImageView.Showing = False Then
                Dim isTabsSelected = _tabInControl

                UpdateTabs()

                If Not isTabsSelected Then
                    If _isInfoShowing Then
                        UpdateInfo()
                    Else
                        UpdateItems()
                    End If
                End If
            End If

            'Update the Dialogues:
            ChooseBox.Update()
            If ChooseBox.Showing = False Then
                TextBox.Update()
            End If
            If PokemonImageView.Showing = True Then
                PokemonImageView.Update()
            End If

            UpdateInfoAnimation()
        End If
    End Sub

    Private Sub UpdateTabs()
        If Controls.Left(True, True, True, True, True, True) And _tabInControl Or ControllerHandler.ButtonPressed(Buttons.LeftShoulder) Then
            _tabIndex -= 1
            If AllowedPages.Count > 0 And AllowedPages.Contains(_tabIndex) = False Then
                While AllowedPages.Contains(_tabIndex) = False
                    _tabIndex -= 1

                    If _tabIndex < 0 Then
                        _tabIndex = 7
                    ElseIf _tabIndex > 7 Then
                        _tabIndex = 0
                    End If
                End While
            End If
            If _tabIndex < 0 Then
                _tabIndex = 7
            ElseIf _tabIndex > 7 Then
                _tabIndex = 0
            End If
            _itemIntro = 0F
            ResetAnimation()
            LoadItems()
        End If
        If Controls.Right(True, True, True, True, True, True) And _tabInControl Or ControllerHandler.ButtonPressed(Buttons.RightShoulder) Then
            _tabIndex += 1
            If AllowedPages.Count > 0 And AllowedPages.Contains(_tabIndex) = False Then
                While AllowedPages.Contains(_tabIndex) = False
                    _tabIndex += 1

                    If _tabIndex < 0 Then
                        _tabIndex = 7
                    ElseIf _tabIndex > 7 Then
                        _tabIndex = 0
                    End If
                End While
            End If
            If _tabIndex < 0 Then
                _tabIndex = 7
            ElseIf _tabIndex > 7 Then
                _tabIndex = 0
            End If
            _itemIntro = 0F
            ResetAnimation()
            LoadItems()
        End If
        If _tabInControl Then
            If Controls.Dismiss() And CanExit Then
                _closing = True
            End If
            If Controls.Accept() Then
                _tabInControl = False
            End If
        End If
    End Sub

    Private Sub UpdateItems()
        If Controls.Left(True, True, True, True, True, True) Then
            ItemIndex -= 2
            If ItemIndex < 0 And PageIndex > 0 Then
                ItemIndex += 10
                PageIndex -= 1
                _itemIntro = 0F
                ResetAnimation()
            ElseIf ItemIndex < 0 And PageIndex = 0 Then
                If ItemIndex = -1 Then
                    ItemIndex = 1
                Else
                    ItemIndex = 0
                End If
            End If
        End If
        If Controls.Right(True, True, True, True, True, True) Then
            If ItemIndex + 2 + (PageIndex * 10) < _items.Length Then
                ItemIndex += 2
                If ItemIndex > 9 Then
                    ItemIndex -= 10
                    PageIndex += 1
                    _itemIntro = 0F
                    ResetAnimation()
                End If
            End If
        End If
        If Controls.Up(True, True, True, True, True, True) Then
            ItemIndex -= 1
            If ItemIndex < 0 And PageIndex > 0 Then
                ItemIndex += 10
                PageIndex -= 1
                _itemIntro = 0F
                ResetAnimation()
            ElseIf ItemIndex < 0 And PageIndex = 0 Then
                ItemIndex = 0
            End If
        End If
        If Controls.Down(True, True, True, True, True, True) Then
            If ItemIndex + 1 + (PageIndex * 10) < _items.Length Then
                ItemIndex += 1
                If ItemIndex > 9 Then
                    ItemIndex -= 10
                    PageIndex += 1
                    _itemIntro = 0F
                    ResetAnimation()
                End If
            End If
        End If

        If Controls.Accept() Then
            _infoItemOptionSelection = 0
            _isInfoShowing = True
            SetInfoSettings()
            SetItemOptions()
        End If

        If Controls.Dismiss() Then
            _tabInControl = True
        End If
    End Sub

    Private Sub UpdateInfo()
        For i = 0 To _infoItemOptionSize.Length - 1
            If i = _infoItemOptionSelection Then
                If _infoItemOptionSize(i) < 200 Then
                    _infoItemOptionSize(i) += 20
                    If _infoItemOptionSize(i) >= 200 Then
                        _infoItemOptionSize(i) = 200
                    End If
                End If
            Else
                If _infoItemOptionSize(i) > 0 Then
                    _infoItemOptionSize(i) -= 20
                    If _infoItemOptionSize(i) <= 0 Then
                        _infoItemOptionSize(i) = 0
                    End If
                End If
            End If
        Next

        If Controls.Up(True) Then
            _infoItemOptionSelection -= 1
            If _infoItemOptionSelection < 0 Then
                _infoItemOptionSelection = _infoItemOptions.Count - 1
            End If
        End If
        If Controls.Down(True) Then
            _infoItemOptionSelection += 1
            If _infoItemOptionSelection > _infoItemOptions.Count - 1 Then
                _infoItemOptionSelection = 0
            End If
        End If

        If Controls.Accept() Then
            SelectedItemOption()
        End If

        If Controls.Dismiss() Then
            CloseInfoScreen()
        End If
    End Sub

    Private Sub CloseInfoScreen()
        _infoSizeTarget = 0
        _infoPositionTarget = GetInfoTargetPositionRollback()
        _itemColumnRightOffsetTarget = 0
        _itemColumnLeftOffsetTarget = 0

        _isInfoShowing = False
    End Sub

    Private Sub SelectedItemOption()
        If _infoItemOptionsNormal.Count > 0 Then
            Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)

            Select Case _infoItemOptionsNormal(_infoItemOptionSelection)
                Case INFO_ITEM_OPTION_USE
                    cItem.Use()
                Case INFO_ITEM_OPTION_GIVE
                    Dim selScreen = New PartyScreen(Core.CurrentScreen) With {.Mode = ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                    AddHandler selScreen.SelectedObject, AddressOf GiveItemHandler

                    Core.SetScreen(selScreen)
                Case INFO_ITEM_OPTION_TOSS

                Case INFO_ITEM_OPTION_SELECT
                    FireSelectionEvent(cItem.ID)
                    CloseInfoScreen()
                    _closing = True
            End Select
        End If
    End Sub

    ''' <summary>
    ''' A handler method to convert the incoming object array.
    ''' </summary>
    Private Sub GiveItemHandler(ByVal params As Object())
        GiveItem(CInt(params(0)))
    End Sub

    Private Sub GiveItem(ByVal PokeIndex As Integer)
        Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)
        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.IsEgg() = False Then
            Core.Player.Inventory.RemoveItem(cItem.ID, 1)

            Dim reItem As Item = Nothing
            If Not Pokemon.Item Is Nothing Then
                reItem = Pokemon.Item
                Core.Player.Inventory.AddItem(reItem.ID, 1)
            End If

            Pokemon.Item = Item.GetItemByID(cItem.ID)

            If reItem Is Nothing Then
                ShowMessage(_translation.MESSAGE_GIVE_ITEM(Pokemon.GetDisplayName(), cItem.Name))
            Else
                ShowMessage(_translation.MESSAGE_SWITCH_ITEM(Pokemon.GetDisplayName(), reItem.Name, cItem.Name))
            End If

            LoadItems()
        Else
            ShowMessage(_translation.MESSAGE_EGG_ERROR)
        End If
    End Sub

    Private Sub ShowMessage(ByVal text As String)
        _messageDelay = CSng(text.Length / 1.75)
        _messageText = text
    End Sub

    ''' <summary>
    ''' Resets the item animation.
    ''' </summary>
    Private Sub ResetAnimation()
        _itemAnimation = New ItemAnimation()
    End Sub

    ''' <summary>
    ''' Updates the item shake animation.
    ''' </summary>
    Private Sub UpdateShakeAnimation()
        If _itemAnimation._shakeLeft = True Then
            _itemAnimation._shakeV -= 0.035F
            If _itemAnimation._shakeV <= -0.4F Then
                _itemAnimation._shakeCount -= 1
                _itemAnimation._shakeLeft = False
            End If
        Else
            _itemAnimation._shakeV += 0.035F
            If _itemAnimation._shakeV >= 0.4F Then
                _itemAnimation._shakeCount -= 1
                _itemAnimation._shakeLeft = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Reloads the temporary item list.
    ''' </summary>
    Private Sub LoadItems()
        _items = Core.Player.Inventory.Where(Function(x) Item.GetItemByID(x.ItemID).ItemType = _visibleItemTypes(_tabIndex)).ToArray()
    End Sub

    ''' <summary>
    ''' Sets the info interface state settings based on the selected item column.
    ''' </summary>
    Private Sub SetInfoSettings()
        Dim column As Integer = CInt(Math.Floor(ItemIndex / 2))

        _infoSize = 0
        _infoSizeTarget = 500 'Size is always 500.
        _itemColumnLeftOffset = 0
        _itemColumnRightOffset = 0
        Select Case column
            Case 0
                _infoSide = 0
                _infoPosition = column * 160 + 32 + 48
                _infoPositionTarget = _infoPosition
                _itemColumnLeft = 0
                _itemColumnLeftOffsetTarget = 0
                _itemColumnRightOffsetTarget = 500
            Case 1
                _infoSide = 0
                _infoPosition = column * 160 + 32 + 48
                _infoPositionTarget = _infoPosition - 160
                _itemColumnLeft = 1
                _itemColumnLeftOffsetTarget = -160
                _itemColumnRightOffsetTarget = 340
            Case 2
                _infoSide = 0
                _infoPosition = column * 160 + 32 + 48
                _infoPositionTarget = _infoPosition - 320
                _itemColumnLeft = 2
                _itemColumnLeftOffsetTarget = -320
                _itemColumnRightOffsetTarget = 180
            Case 3
                _infoSide = 1
                _infoPosition = column * 160 - 80
                _infoPositionTarget = _infoPosition - 320
                _itemColumnLeft = 2
                _itemColumnLeftOffsetTarget = -320
                _itemColumnRightOffsetTarget = 180
            Case 4
                _infoSide = 1
                _infoPosition = column * 160 - 80
                _infoPositionTarget = _infoPosition - 500
                _itemColumnLeft = 3
                _itemColumnLeftOffsetTarget = -500
                _itemColumnRightOffsetTarget = 0
        End Select
    End Sub

    Private Sub SetItemOptions()
        _infoItemOptions.Clear()
        _infoItemOptionsNormal.Clear()

        Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)

        If _mode = ISelectionScreen.ScreenMode.Default Then
            If cItem.CanBeUsed Then
                _infoItemOptions.Add(_translation.INFO_ITEM_OPTION_USE)
                _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_USE)
            End If
            If cItem.CanBeHold Then
                _infoItemOptions.Add(_translation.INFO_ITEM_OPTION_GIVE)
                _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_GIVE)
            End If
            If cItem.CanBeTossed Then
                _infoItemOptions.Add(_translation.INFO_ITEM_OPTION_TOSS)
                _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_TOSS)
            End If
        ElseIf _mode = ISelectionScreen.ScreenMode.Selection
            _infoItemOptions.Add(_translation.INFO_ITEM_OPTION_SELECT)
            _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_SELECT)
        End If
    End Sub

    ''' <summary>
    ''' Gets the target position of the info box for the rollback animation.
    ''' </summary>
    Private Function GetInfoTargetPositionRollback() As Integer
        Dim column As Integer = CInt(Math.Floor(ItemIndex / 2))

        Select Case column
            Case 0
                Return column * 160 + 32 + 48
            Case 1
                Return column * 160 + 32 + 48
            Case 2
                Return column * 160 + 32 + 48
            Case 3
                Return column * 160 - 80
            Case 4
                Return column * 160 - 80
        End Select

        Return 0
    End Function

    Private Sub UpdateInfoAnimation()
        'Make the size grow if needed:
        Dim tempInfoSize As Integer = _infoSize
        _infoSize = CInt(MathHelper.Lerp(_infoSize, _infoSizeTarget, 0.1F))
        If tempInfoSize = _infoSize Then
            _infoSize = _infoSizeTarget
        End If

        'Move the info position to its target:
        Dim tempInfoPosition As Integer = _infoPosition
        _infoPosition = CInt(MathHelper.Lerp(_infoPosition, _infoPositionTarget, 0.1F))
        If tempInfoPosition = _infoPosition Then
            _infoPosition = _infoPositionTarget
        End If

        'Move item offsets:
        Dim tempItemColumnLeftOffset As Integer = _itemColumnLeftOffset
        _itemColumnLeftOffset = CInt(MathHelper.Lerp(_itemColumnLeftOffset, _itemColumnLeftOffsetTarget, 0.1F))
        If tempItemColumnLeftOffset = _itemColumnLeftOffset Then
            _itemColumnLeftOffset = _itemColumnLeftOffsetTarget
        End If

        Dim tempItemColumnRightOffset As Integer = _itemColumnRightOffset
        _itemColumnRightOffset = CInt(MathHelper.Lerp(_itemColumnRightOffset, _itemColumnRightOffsetTarget, 0.1F))
        If tempItemColumnRightOffset = _itemColumnRightOffset Then
            _itemColumnRightOffset = _itemColumnRightOffsetTarget
        End If
    End Sub

    Private _mode As ISelectionScreen.ScreenMode = ISelectionScreen.ScreenMode.Default
    Private _canExit As Boolean = True
    Private _visibleItemTypes As Item.ItemTypes()

    Public Event SelectedObject(params() As Object) Implements ISelectionScreen.SelectedObject

    Private Sub FireSelectionEvent(ByVal itemId As Integer)
        RaiseEvent SelectedObject(New Object() {itemId})
    End Sub

    ''' <summary>
    ''' The current mode of this screen.
    ''' </summary>
    Public Property Mode As ISelectionScreen.ScreenMode Implements ISelectionScreen.Mode
        Get
            Return _mode
        End Get
        Set(value As ISelectionScreen.ScreenMode)
            _mode = value
        End Set
    End Property

    ''' <summary>
    ''' If the user can quit the screen in selection mode without choosing an item.
    ''' </summary>
    Public Property CanExit As Boolean Implements ISelectionScreen.CanExit
        Get
            Return _canExit
        End Get
        Set(value As Boolean)
            _canExit = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the visible item type tabs.
    ''' </summary>
    Public WriteOnly Property VisibleItemTypes As Item.ItemTypes()
        Set(value As Item.ItemTypes())
            _visibleItemTypes = value
        End Set
    End Property

End Class