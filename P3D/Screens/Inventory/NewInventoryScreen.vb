Imports P3D.Screens.UI

''' <summary>
''' Displays the inventory and gives the player options to choose and use items.
''' </summary>
Public Class NewInventoryScreen

    Inherits Screen
    Implements ISelectionScreen

    'Private _translation As Globalization.Classes.LOCAL_InventoryScreen

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

    'Shows amount window if tossing item when true
    Private _tossingItems As Boolean = False
    Private _tossValue As Integer = 1

    'Stuff related to blurred PreScreens
    Private _preScreenTexture As RenderTarget2D
    Private _preScreenTarget As RenderTarget2D
    Private _blurScreens As Identifications() = {Identifications.BattleScreen,
                                                 Identifications.OverworldScreen,
                                                 Identifications.MailSystemScreen}

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

    Public Shared SelectedItem As String = "-1"
    Private DoReturnItem As Boolean = False

    'experiment
    Public Delegate Sub DoStuff(ByVal ItemID As Integer)
    Dim ReturnItem As DoStuff

    Dim AllowedPages() As Integer
    Dim AllowedItems As List(Of String)

    Public Sub New(ByVal currentScreen As Screen, ByVal AllowedPages As Integer(), ByVal StartPageIndex As Integer, ByVal DoStuff As DoStuff, Optional ByVal AllowedItems As List(Of String) = Nothing, Optional ByVal DoReturnItem As Boolean = False)

        SelectedItem = "-1"
        _preScreenTarget = New RenderTarget2D(GraphicsDevice, windowSize.Width, windowSize.Height)
        _blur = New Resources.Blur.BlurHandler(windowSize.Width, windowSize.Height)

        If AllowedPages.Contains(StartPageIndex) = False Then
            _tabIndex = AllowedPages(0)
        Else
            _tabIndex = StartPageIndex
        End If

        _pageIndex = Player.Temp.BagPageIndex
        _itemindex = Player.Temp.BagItemIndex

        Me.AllowedPages = AllowedPages
        Me.AllowedItems = AllowedItems
        ReturnItem = DoStuff
        Me.DoReturnItem = DoReturnItem

        'JSON Stuff
        '_translation = New Globalization.Classes.LOCAL_InventoryScreen()
        target_1 = New RenderTarget2D(GraphicsDevice, 816, 400 - 32, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents)
        target_2 = New RenderTarget2D(GraphicsDevice, 500, 368, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
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
        _visibleItemTypes = New Items.ItemTypes() {Items.ItemTypes.Standard,
                                                  Items.ItemTypes.Medicine,
                                                  Items.ItemTypes.Plants,
                                                  Items.ItemTypes.Pokéballs,
                                                  Items.ItemTypes.Machines,
                                                  Items.ItemTypes.Mail,
                                                  Items.ItemTypes.BattleItems,
                                                  Items.ItemTypes.KeyItems}

        'TODO: Load state information from the PlayerTemp.

        _tabHighlight(_tabIndex) = 255

        'Load the items once when loading up the inventory screen:
        LoadItems()
    End Sub

    Public Sub ReturnSelectedItem(ByVal ItemID As String)
        SelectedItem = ItemID
        _closing = True
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal AllowedPages() As Integer, ByVal DoStuff As DoStuff, Optional ByVal AllowedItems As List(Of String) = Nothing, Optional ByVal DoReturnItem As Boolean = False)
        Me.New(currentScreen, AllowedPages, Player.Temp.BagIndex, DoStuff, AllowedItems, DoReturnItem)
    End Sub

    Public Sub New(ByVal currentScreen As Screen, Optional ByVal AllowedItems As List(Of String) = Nothing, Optional ByVal DoReturnItem As Boolean = False)
        Me.New(currentScreen, {0, 1, 2, 3, 4, 5, 6, 7}, Player.Temp.BagIndex, Nothing, AllowedItems, DoReturnItem)
    End Sub

    Public Overrides Sub Draw()
        If _blurScreens.Contains(PreScreen.Identification) Then
            DrawPrescreen()
        Else
            PreScreen.Draw()
        End If
        DrawGradients(CInt(255 * _interfaceFade))


        DrawMain()
        DrawTabs()

        DrawMessage()

        PokemonImageView.Draw()
        ImageView.Draw()
        TextBox.Draw()
        ChooseBox.Draw()

        DrawAmount()
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
        SpriteBatch.Draw(_blur.Perform(_preScreenTexture), windowSize, Color.White)
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
        Dim mainBackgroundColor As Color = Color.White
        If _closing = True Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

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

        Dim TabDescriptionWidth As Integer = 176
        Dim TbgColor As New Color(128, 128, 128)
        If _closing Then
            TbgColor = New Color(TbgColor.R, TbgColor.G, TbgColor.B, CInt(CInt(TbgColor.A) * _interfaceFade))
        End If
        For x = 0 To TabDescriptionWidth Step 16
            For y = 0 To 32 Step 16
                SpriteBatch.Draw(_menuTexture, New Rectangle(halfWidth - 400 + x + 384, halfHeight - 200 + y, 16, 16), New Rectangle(0, 0, 4, 4), TbgColor)
            Next
        Next
        Canvas.DrawGradient(Core.SpriteBatch, New Rectangle(halfWidth - 400 + 384 + TabDescriptionWidth + 16, halfHeight - 200, 800 - (384 + TabDescriptionWidth), 48), New Color(0, 0, 0, CInt(TbgColor.A * 0.5)), New Color(0, 0, 0, CInt(TbgColor.A * 0.00)), True, -1)
        Dim TabName As String = ""
        Select Case _tabIndex
            Case 0 : TabName = Localization.GetString("item_category_Standard", "Standard")
            Case 1 : TabName = Localization.GetString("item_category_Medicine", "Medicine")
            Case 2 : TabName = Localization.GetString("item_category_Plants", "Plants")
            Case 3 : TabName = Localization.GetString("item_category_Pokeballs", "Pokéballs")
            Case 4 : TabName = Localization.GetString("item_category_Machines", "TM/HM")
            Case 5 : TabName = Localization.GetString("item_category_Mail", "Mail")
            Case 6 : TabName = Localization.GetString("item_category_BattleItems", "Battle Items")
            Case 7 : TabName = Localization.GetString("item_category_KeyItems", "Key Items")
        End Select
        Dim gColor As New Color(164, 164, 164)
        If _closing Then
            gColor = New Color(gColor.R, gColor.G, gColor.B, CInt(CInt(gColor.A) * _interfaceFade))
        End If
        Dim fontWidth As Integer = CInt(FontManager.ChatFont.MeasureString(TabName).X)
        SpriteBatch.DrawString(FontManager.ChatFont, TabName, New Vector2(halfWidth - 400 + 384 + CInt((TabDescriptionWidth - fontWidth) * 0.5), halfHeight - 200 + 12), gColor)
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

        Canvas.DrawRectangle(New Rectangle(halfWidth - 400, halfHeight - 232, 260, 32), New Color(ColorProvider.MainColor(False).R, ColorProvider.MainColor(False).G, ColorProvider.MainColor(False).B, mainBackgroundColor.A))
        Canvas.DrawRectangle(New Rectangle(halfWidth - 140, halfHeight - 216, 16, 16), New Color(ColorProvider.MainColor(False).R, ColorProvider.MainColor(False).G, ColorProvider.MainColor(False).B, mainBackgroundColor.A))
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 140, halfHeight - 232, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 124, halfHeight - 216, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)

        SpriteBatch.DrawString(FontManager.ChatFont, Localization.GetString("inventory_screen_title", "Inventory"), New Vector2(halfWidth - 390, halfHeight - 228), mainBackgroundColor)

        'Draw background pattern:
        For y = 0 To CInt(_enrollY) Step 16
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + x, halfHeight - 200 + y, 16, 16), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        Next

        'This draws the lowest row so the background is not cut off:
        Dim modRes As Integer = CInt(_enrollY) Mod 16
        If modRes > 0 Then
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + x, CInt(_enrollY + (halfHeight - 200)), 16, modRes), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
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

                        Dim nameTextHeight As Integer = 24
                        If _tabIndex = 4 Then
                            nameTextHeight = 40
                        End If

                        Dim TextLine1 As String = cItem.Name.GetSplit(0, "~")
                        Dim TextLine2 As String = ""

                        Dim fontSizeOffset As Integer = 0
                        If cItem.Name.Contains("~") Then
                            fontSizeOffset = 16
                            TextLine2 = cItem.Name.GetSplit(1, "~")
                        End If
                        nameTextHeight += fontSizeOffset

                        Canvas.DrawRectangle(itemBatch, New Rectangle(CInt(itemLoc.X) - 16 - 9, CInt(itemLoc.Y) + 48, 128 + 18, nameTextHeight), New Color(0, 0, 0, CInt(If(_tabInControl, 64, 128) * _interfaceFade)))

                        Dim fontWidth1 As Integer = CInt(FontManager.MiniFont.MeasureString(TextLine1).X)
                        Dim fontWidth2 As Integer = CInt(FontManager.MiniFont.MeasureString(TextLine2).X)

                        itemBatch.DrawString(FontManager.MiniFont, TextLine1, itemLoc + New Vector2(48 - fontWidth1 / 2.0F, 51), New Color(255, 255, 255, itemPanelAlpha))
                        If TextLine2 <> "" Then
                            itemBatch.DrawString(FontManager.MiniFont, TextLine2, itemLoc + New Vector2(48 - fontWidth2 / 2.0F, 51 + 16), New Color(255, 255, 255, itemPanelAlpha))
                        End If
                        If _tabIndex <> 7 Then
                            itemBatch.DrawString(FontManager.MainFont, "x" & _items(i + (PageIndex * 10)).Amount.ToString(), itemLoc + New Vector2(84, 26), New Color(40, 40, 40, itemPanelAlpha))
                        End If

                        If _tabIndex = 4 Then
                            Dim AttackName As String
                            If cItem.IsGameModeItem = False Then
                                AttackName = CType(cItem, Items.TechMachine).Attack.Name
                            Else
                                AttackName = CType(cItem, GameModeItem).gmTeachMove.Name
                            End If
                            Dim TMfontWidth As Integer = CInt(FontManager.MiniFont.MeasureString(AttackName).X)
                            itemBatch.DrawString(FontManager.MiniFont, AttackName, itemLoc + New Vector2(48 - TMfontWidth / 2.0F, 51 + 16 + fontSizeOffset), New Color(255, 255, 255, itemPanelAlpha))
                        End If
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
        If _items.Count = 0 Then
            Exit Sub
        End If
        'Create a new render target and set it.

        'Bring back when Monogame begins supporting this stuff
        '   Dim target As New RenderTarget2D(GraphicsDevice, _infoSize, 368, False, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PreserveContents)
        GraphicsDevice.SetRenderTarget(target_2)
        GraphicsDevice.Clear(Color.Transparent)

        'Render background:
        infoBatch.BeginBatch()
        Dim alpha = CInt(CSng(_infoSize) / 500 * 255)

        For y = 0 To 368 Step 16
            For x = 0 To _infoSize + 16 Step 16
                If x < _infoSize - 16 Then
                    infoBatch.Draw(_menuTexture, New Rectangle(x, y, 16, 16), New Rectangle(0, 0, 4, 4), New Color(128, 128, 128, alpha))
                End If
            Next
        Next

        Canvas.DrawGradient(infoBatch, New Rectangle(0, 0, 100, 368), New Color(0, 0, 0, alpha), New Color(0, 0, 0, 0), True, -1)
        Canvas.DrawGradient(infoBatch, New Rectangle(_infoSize - 100, 0, 100, 368), New Color(0, 0, 0, 0), New Color(0, 0, 0, alpha), True, -1)

        'Get item and gets its display texts based on the item category:
        Dim getIndex As Integer = ItemIndex + PageIndex * 10
        Dim cItem As Item = Item.GetItemByID(_items(getIndex).ItemID)

        infoBatch.Draw(cItem.Texture, New Rectangle(24, 24, 48, 48), Color.White)

        Dim itemTitle As String = cItem.Name
        Dim itemSubTitle As String = cItem.ItemType.ToString()
        Dim itemDescription As String = cItem.GetDescription

        Select Case cItem.ItemType
            Case Items.ItemTypes.Machines

                If cItem.IsGameModeItem = True Then
                    itemTitle = CType(cItem, GameModeItem).gmTeachMove.Name

                    If CType(cItem, GameModeItem).gmTeachMove IsNot Nothing Then
                        If CType(cItem, GameModeItem).gmIsHM = True Then
                            'JSON stuff
                            'itemSubTitle = _translation.HIDDEN_MACHINE_TITLE(cItem.ItemType.ToString())
                            itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_HM", "Hidden Machine")
                        Else
                            'JSON stuff
                            'itemSubTitle = _translation.TECH_MACHINE_TITLE(cItem.ItemType.ToString())
                            itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_TM", "Technical Machine")
                        End If
                    End If

                    itemDescription &= Environment.NewLine & CType(cItem, GameModeItem).gmTeachMove.Description
                Else
                    Dim techMachine = CType(cItem, Items.TechMachine)

                    itemTitle = techMachine.Attack.Name

                    If techMachine.IsTM = False Then
                        'JSON stuff
                        'itemSubTitle = _translation.HIDDEN_MACHINE_TITLE(cItem.ItemType.ToString())
                        itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_HM", "Hidden Machine")
                    Else
                        'JSON stuff
                        'itemSubTitle = _translation.TECH_MACHINE_TITLE(cItem.ItemType.ToString())
                        itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_TM", "Technical Machine")

                    End If

                    itemDescription &= Environment.NewLine & techMachine.Attack.Description
                End If
            Case Items.ItemTypes.Standard
                'JSON stuff
                'itemSubTitle = _translation.STANDARD_ITEM_TITLE(cItem.ItemType.ToString())
                itemSubTitle = cItem.ItemType.ToString() & Localization.GetString("inventory_screen_ItemSubtitle_Standard_Suffix", "Item")
            Case Items.ItemTypes.KeyItems
                'JSON stuff
                'itemSubTitle = _translation.KEYITEM_TITLE(cItem.ItemType.ToString())
                itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_KeyItem", "Key Item")
            Case Items.ItemTypes.Pokéballs
                'JSON stuff
                'itemSubTitle = _translation.POKEBALL_TITLE(cItem.ItemType.ToString())
                itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_PokeBall", "Poké Ball")
            Case Items.ItemTypes.Plants
                'JSON stuff
                'itemSubTitle = _translation.PLANT_TITLE(cItem.ItemType.ToString())
                itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_Plant", "Plant")
            Case Items.ItemTypes.BattleItems
                'JSON stuff
                'itemSubTitle = _translation.BATTLEITEM_TITLE(cItem.ItemType.ToString())
                itemSubTitle = Localization.GetString("inventory_screen_ItemSubtitle_BattleItem", "Battle Item")
        End Select

        infoBatch.DrawString(FontManager.TextFont, itemTitle, New Vector2(80, 20), Color.White, 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)
        infoBatch.DrawString(FontManager.TextFont, itemSubTitle, New Vector2(80, 46), Color.LightGray, 0F, Vector2.Zero, 2.0f, SpriteEffects.None, 0F)
        infoBatch.DrawString(FontManager.TextFont, itemDescription.CropStringToWidth(FontManager.TextFont, 1.0F, 430), New Vector2(28, 84), Color.LightGray, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        For i = 0 To _infoItemOptions.Count - 1
            Canvas.DrawRectangle(infoBatch, New Rectangle(CInt(250 - _infoItemOptionSize(i) / 2), 158 + i * 64, _infoItemOptionSize(i), 48), New Color(255, 255, 255, 20))

            infoBatch.DrawString(FontManager.TextFont, _infoItemOptions(i), New Vector2(CInt(250 - FontManager.TextFont.MeasureString(_infoItemOptions(i)).X), 168 + i * 64), Color.White, 0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0F)
        Next

        infoBatch.EndBatch()

        'Set the target that was previously active and render the new target on top of that:
        GraphicsDevice.SetRenderTarget(preTarget)
        preBatch.Draw(target_2, New Rectangle(_infoPosition + 80, 0, target_2.Width, target_2.Height), New Color(255, 255, 255, alpha))
    End Sub

    ''' <summary>
    ''' Draws the amount of items to be tossed.
    ''' </summary>
    Private Sub DrawAmount()
        If _tossingItems Then
            Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)

            Dim CanvasTexture As Texture2D
            CanvasTexture = TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(0, 0, 48, 48))

            Dim ItemID As String
            If cItem.IsGameModeItem = True Then
                ItemID = cItem.gmID
            Else
                ItemID = cItem.ID.ToString
            End If
            Dim trashText As String = _tossValue & "/" & Core.Player.Inventory.GetItemAmount(ItemID)
            Dim offsetX As Integer = 100
            Dim offsetY As Integer = Core.windowSize.Height - 390

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) + 180 + offsetX, 240 + offsetY, 128, 64))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, trashText, New Vector2(CInt(Core.windowSize.Width / 2) - (FontManager.InGameFont.MeasureString(trashText).X / 2) + 256 + offsetX, 276 + offsetY), Color.Black)
        End If
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
            Case Items.ItemTypes.Standard
                Return New Rectangle(0, 16, 16, 16)
            Case Items.ItemTypes.Medicine
                Return New Rectangle(16, 16, 16, 16)
            Case Items.ItemTypes.Plants
                Return New Rectangle(0, 32, 16, 16)
            Case Items.ItemTypes.Pokéballs
                Return New Rectangle(48, 16, 16, 16)
            Case Items.ItemTypes.Machines
                Return New Rectangle(32, 16, 16, 16)
            Case Items.ItemTypes.Mail
                Return New Rectangle(16, 32, 16, 16)
            Case Items.ItemTypes.BattleItems
                Return New Rectangle(48, 32, 16, 16)
            Case Items.ItemTypes.KeyItems
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
            If TextBox.Showing = False And ChooseBox.Showing = False And PokemonImageView.Showing = False And ImageView.Showing = False Then
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
            If ImageView.Showing = True Then
                ImageView.Update()
            End If

            UpdateInfoAnimation()
        End If

        'Update the toss amount indicator:
        If _tossingItems Then
            Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)
            If Controls.Right(True, True, True, True) Then
                _tossValue += 1
            End If
            If Controls.Left(True, True, True, True) Then
                _tossValue -= 1
            End If

            Dim ItemID As String
            If cItem.IsGameModeItem = True Then
                ItemID = cItem.gmID
            Else
                ItemID = cItem.ID.ToString
            End If
            _tossValue = CInt(MathHelper.Clamp(_tossValue, 1, Core.Player.Inventory.GetItemAmount(ItemID)))

            If Not TextBox.Showing Then
                If Controls.Accept Then
                    SoundManager.PlaySound("select")
                    Core.Player.Inventory.RemoveItem(ItemID, _tossValue)
                    LoadItems()
                    _tossingItems = False
                ElseIf Controls.Dismiss Then
                    SoundManager.PlaySound("select")
                    _tossingItems = False
                End If
                _tossValue = 1
            End If
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
            If AllowedPages.Count = 1 Then
                _tabInControl = False
            Else
                If Controls.Dismiss() And CanExit Then
                    SoundManager.PlaySound("select")
                    SelectedItem = "-1"
                    _closing = True
                End If
                If Controls.Accept() And _items.Length > 0 Then
                    SoundManager.PlaySound("select")
                    _tabInControl = False
                End If
            End If
        End If
    End Sub

    Private Sub UpdateItems()
        If Controls.Left(True, True, False, True, True, True) Then
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
        If Controls.Right(True, True, False, True, True, True) Then
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

        If Controls.Accept() AndAlso _items.Length > 0 Then
            Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)
            SoundManager.PlaySound("select")
            If DoReturnItem = True Then
                If cItem.IsGameModeItem = True Then
                    ReturnSelectedItem(cItem.gmID)
                Else
                    ReturnSelectedItem(cItem.ID.ToString)
                End If

            Else
                If Me.PreScreen.Identification = Screen.Identifications.BattleScreen Then
                    If cItem.CanBeUsedInBattle = True Then
                        _infoItemOptionSelection = 0
                        _isInfoShowing = True
                        SetInfoSettings()
                        SetItemOptions()
                    Else
                        TextBox.Show(Localization.GetString("inventory_screen_ItemNotUsableInBattle", "This item can't~be used in Battle."))
                    End If
                Else
                    _infoItemOptionSelection = 0
                    _isInfoShowing = True
                    SetInfoSettings()
                    SetItemOptions()
                End If

            End If
        End If
        If Controls.Dismiss() Then
            If AllowedPages.Count > 1 Then
                SoundManager.PlaySound("select")
                _tabInControl = True
            Else
                _closing = True
            End If
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
            SoundManager.PlaySound("select")
            SelectedItemOption()
        End If

        If Controls.Dismiss() Then
            SoundManager.PlaySound("select")
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

    Private Sub SaveBagIndex()
        Player.Temp.BagIndex = _tabIndex
        Player.Temp.BagPageIndex = _pageIndex
        Player.Temp.BagItemIndex = _itemindex
    End Sub

    Private Sub SelectedItemOption()
        If _infoItemOptionsNormal.Count > 0 Then
            Dim cItem As Item = Item.GetItemByID(_items(ItemIndex + PageIndex * 10).ItemID)

            Select Case _infoItemOptionsNormal(_infoItemOptionSelection)
                Case INFO_ITEM_OPTION_USE
                    cItem.Use()
                    LoadItems()
                Case INFO_ITEM_OPTION_GIVE
                    Dim selScreen = New PartyScreen(Core.CurrentScreen) With {.Mode = ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                    AddHandler selScreen.SelectedObject, AddressOf GiveItemHandler

                    Core.SetScreen(selScreen)
                Case INFO_ITEM_OPTION_TOSS
                    TossItem(cItem)
                Case INFO_ITEM_OPTION_SELECT
                    If cItem.IsGameModeItem = True Then
                        FireSelectionEvent(cItem.gmID)
                    Else
                        FireSelectionEvent(cItem.ID.ToString)
                    End If
                    CloseInfoScreen()
                    _closing = True
            End Select
            SaveBagIndex()
        End If
    End Sub

    Private Sub TossItem(ByVal cItem As Item)
        Dim text As String = Localization.GetString("inventory_screen_TossConfirmation", "Are you sure you want to toss~this item?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
        TextBox.Show(text, AddressOf Me.TossManyItems, False, False, TextBox.DefaultColor)
    End Sub

    Private Sub TossManyItems(ByVal result As Integer)
        If result = 0 Then
            TextBox.Show(Localization.GetString("inventory_screen_TossAmount", "Select the amount to toss."), {})
            _tossingItems = True
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
            Dim ItemID As String
            If cItem.IsGameModeItem = True Then
                ItemID = cItem.gmID
            Else
                ItemID = cItem.ID.ToString
            End If

            Core.Player.Inventory.RemoveItem(ItemID, 1)

            Dim reItem As Item = Nothing
            If Not Pokemon.Item Is Nothing Then
                Dim ReItemID As String
                If Pokemon.Item.IsGameModeItem = True Then
                    ReItemID = Pokemon.Item.gmID
                Else
                    ReItemID = Pokemon.Item.ID.ToString
                End If
                reItem = Pokemon.Item
                Core.Player.Inventory.AddItem(ReItemID, 1)
            End If

            Pokemon.Item = Item.GetItemByID(ItemID)

            If reItem Is Nothing Then
                'JSON Stuff
                'ShowMessage(_translation.MESSAGE_GIVE_ITEM(Pokemon.GetDisplayName(), cItem.Name))
                Dim giveString As String = Localization.GetString("inventory_screen_GiveItem_Give", "Gave <name> the <newitem>.").Replace("<name>", Pokemon.GetDisplayName()).Replace("<newitem>", cItem.OneLineName())
                ShowMessage(giveString)
            Else
                'JSON Stuff
                'ShowMessage(_translation.MESSAGE_SWITCH_ITEM(Pokemon.GetDisplayName(), reItem.Name, cItem.Name))
                Dim switchString As String = Localization.GetString("inventory_screen_GiveItem_Switch", "Switched <name>'s <olditem> with the <newitem>.").Replace("<name>", Pokemon.GetDisplayName()).Replace("<olditem>", reItem.OneLineName()).Replace("<newitem>", cItem.OneLineName())
                ShowMessage(switchString)
            End If

            LoadItems()
            If ItemIndex + PageIndex * 10 > _items.Count - 1 Then
                ItemIndex = 0
                PageIndex = 0
                CloseInfoScreen()
            End If
        Else
            'JSON Stuff
            'ShowMessage(_translation.MESSAGE_EGG_ERROR)
            ShowMessage(Localization.GetString("inventory_screen_EggsCannotHold", "Eggs cannot hold items."))
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
            _itemAnimation._shakeV -= 0.0275F
            If _itemAnimation._shakeV <= -0.4F Then
                _itemAnimation._shakeCount -= 1
                _itemAnimation._shakeLeft = False
            End If
        Else
            _itemAnimation._shakeV += 0.0275F
            If _itemAnimation._shakeV >= 0.4F Then
                _itemAnimation._shakeCount -= 1
                _itemAnimation._shakeLeft = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Reloads the temporary item list.
    ''' </summary>
    Public Sub LoadItems()
        _items = Core.Player.Inventory.Where(Function(x) Item.GetItemByID(x.ItemID).ItemType = _visibleItemTypes(_tabIndex)).Where(Function(x) IsItemVisible(x.ItemID) = True).ToArray()
        If _tabIndex = 4 Then 'TM/HM
            _items = (From i In _items Order By Item.GetItemByID(i.ItemID).SortValue Ascending).ToArray()
        Else
            _items = (From i In _items Order By Item.GetItemByID(i.ItemID).Name Ascending).ToArray()
        End If
        If _items.Count <= ItemIndex + PageIndex * 10 Then
            ItemIndex -= 1
            If ItemIndex = -1 Then
                If PageIndex > 0 Then
                    PageIndex -= 1
                    ItemIndex = 9
                Else
                    ItemIndex = 0
                    PageIndex = 0
                    _tabInControl = True
                End If
            End If
            CloseInfoScreen()
        End If
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
                'JSON Stuff
                '_infoItemOptions.Add(_translation.INFO_ITEM_OPTION_USE)
                _infoItemOptions.Add(Localization.GetString("global_use", "Use"))
                _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_USE)
            End If
            If cItem.CanBeHeld Then
                'JSON Stuff
                ' _infoItemOptions.Add(_translation.INFO_ITEM_OPTION_GIVE)
                _infoItemOptions.Add(Localization.GetString("global_give", "Give"))
                _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_GIVE)
            End If
            If cItem.CanBeTossed Then
                'JSON Stuff
                '_infoItemOptions.Add(_translation.INFO_ITEM_OPTION_TOSS)
                _infoItemOptions.Add(Localization.GetString("global_toss", "Toss"))
                _infoItemOptionsNormal.Add(INFO_ITEM_OPTION_TOSS)
            End If
        ElseIf _mode = ISelectionScreen.ScreenMode.Selection Then
            'JSON Stuff
            '_infoItemOptions.Add(_translation.INFO_ITEM_OPTION_SELECT)
            _infoItemOptions.Add(Localization.GetString("global_select", "Select"))
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
    Private _visibleItemTypes As Items.ItemTypes()
    Private Function IsItemVisible(ItemID As String) As Boolean
        If AllowedItems Is Nothing OrElse AllowedItems.Contains("-1") Then
            Return True
        Else
            If AllowedItems.Contains(ItemID) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Event SelectedObject(params() As Object) Implements ISelectionScreen.SelectedObject

    Private Sub FireSelectionEvent(ByVal itemId As String)
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
    Public WriteOnly Property VisibleItemTypes As Items.ItemTypes()
        Set(value As Items.ItemTypes())
            _visibleItemTypes = value
        End Set
    End Property

End Class