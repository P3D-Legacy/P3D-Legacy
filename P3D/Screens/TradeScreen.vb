Public Class TradeScreen

    Inherits Screen

#Region "Navigation"

    Private Enum MenuStates
        MainPage
        BuyItems
        SellItems
        BuyItemsCategory
        SellItemsCategory
        SellItemsConfirmation
    End Enum

    Private MenuState As MenuStates = MenuStates.MainPage
    Private CurrentCategory As Items.ItemTypes = Items.ItemTypes.Medicine

    Private MainCursor As Integer = 0
    Private CategoryCursor As Integer = 0
    Private BuySellCursor As Integer = 0

    Private BuySellScroll As Integer = 0
    Private CategoryScroll As Integer = 0

    Private Property Scroll() As Integer
        Get
            Select Case Me.MenuState
                Case MenuStates.MainPage
                    Return 0
                Case MenuStates.BuyItems, MenuStates.SellItems, MenuStates.SellItemsConfirmation
                    Return BuySellScroll
                Case MenuStates.BuyItemsCategory, MenuStates.SellItemsCategory
                    Return CategoryScroll
            End Select
            Return 0
        End Get
        Set(value As Integer)
            Select Case Me.MenuState
                Case MenuStates.BuyItems, MenuStates.SellItems, MenuStates.SellItemsConfirmation
                    Me.BuySellScroll = value
                Case MenuStates.BuyItemsCategory, MenuStates.SellItemsCategory
                    Me.CategoryScroll = value
            End Select
        End Set
    End Property

    Private Property Cursor() As Integer
        Get
            Select Case Me.MenuState
                Case MenuStates.MainPage
                    Return MainCursor
                Case MenuStates.BuyItems, MenuStates.SellItems, MenuStates.SellItemsConfirmation
                    Return BuySellCursor
                Case MenuStates.BuyItemsCategory, MenuStates.SellItemsCategory
                    Return CategoryCursor
            End Select

            Return 0
        End Get
        Set(value As Integer)
            Select Case Me.MenuState
                Case MenuStates.MainPage
                    MainCursor = value
                Case MenuStates.BuyItems, MenuStates.SellItems, MenuStates.SellItemsConfirmation
                    BuySellCursor = value
                Case MenuStates.BuyItemsCategory, MenuStates.SellItemsCategory
                    CategoryCursor = value
            End Select
        End Set
    End Property

#End Region

    Public Enum Currencies
        Pokédollar
        BattlePoints
        Coins
    End Enum

    ''' <summary>
    ''' Contains definitions for an item that can be traded in a store.
    ''' </summary>
    Public Structure TradeItem

        ''' <summary>
        ''' Creates a new instance of the TradeItem structure.
        ''' </summary>
        ''' <param name="ItemID">The ID of the Item.</param>
        ''' <param name="Price">The Price of the Item. Leave -1 for automatic Price.</param>
        ''' <param name="Amount">The Amount of the Item available in the store. Leave -1 for infinite.</param>
        Public Sub New(ByVal ItemID As String, ByVal Amount As Integer, ByVal Price As Integer, ByVal Currency As Currencies)
            Me.ItemID = ItemID
            Me.Amount = Amount

            If Price = -1 Then
                Dim i As Item = Me.GetItem()

                If Currency = Currencies.BattlePoints Then
                    Me.Price = i.BattlePointsPrice
                ElseIf Currency = Currencies.Pokédollar Then
                    Me.Price = i.PokeDollarPrice
                End If
            Else
                Me.Price = Price
            End If
        End Sub

        Public ItemID As String
        Public Price As Integer
        Public Amount As Integer

        ''' <summary>
        ''' Returns the Item that is associated with this TradeItem instance.
        ''' </summary>
        Public Function GetItem() As Item
            Return Item.GetItemByID(Me.ItemID)
        End Function

        Public Function SellPrice() As Integer
            Return CInt(Math.Ceiling(Me.Price / 2))
        End Function

    End Structure

    Private TradeItems As New List(Of TradeItem)
    Private PossibleStoreItems As New List(Of TradeItem)
    Private CanBuyItems As Boolean = True
    Private CanSellItems As Boolean = True
    Private Currency As Currencies = Currencies.Pokédollar
    Private ShopIdentifier As String = ""

    Dim texture As Texture2D
    Dim TileOffset As Integer = 0
    Dim Title As String = ""

    ''' <summary>
    ''' Creates a new instance of the TradeScreen class.
    ''' </summary>
    ''' <param name="currentScreen">The screen that is the current active screen.</param>
    ''' <param name="storeString">The string defining what the store sells. Format: {ItemID|Amount|Price}</param>
    Public Sub New(ByVal currentScreen As Screen, ByVal storeString As String, ByVal canBuy As Boolean, ByVal canSell As Boolean, ByVal currencyIndicator As String, ByVal shopIdentifier As String)
        Me.PreScreen = currentScreen

        Dim itemArr = storeString.Split({"}"}, StringSplitOptions.RemoveEmptyEntries)

        Me.SetCurrency(currencyIndicator)

        For Each lItem As String In itemArr
            lItem = lItem.Remove(0, 1)

            Dim itemData = lItem.Split(CChar("|"))

            If shopIdentifier <> "" Then
                Me.ShopIdentifier = shopIdentifier
                If ActionScript.IsRegistered(Me.ShopIdentifier & "_" & itemData(0)) = True Then
                    If ScriptConversion.ToInteger(itemData(1)) <> -1 Then
                        Dim registerContent() As Object = ActionScript.GetRegisterValue(shopIdentifier & "_" & itemData(0))

                        If CInt(registerContent(0)) < ScriptConversion.ToInteger(itemData(1)) Then
                            Dim ResultAmount As Integer = ScriptConversion.ToInteger(itemData(1))
                            If CInt(registerContent(0)) > 0 Then
                                ResultAmount = ScriptConversion.ToInteger(itemData(1)) - CInt(registerContent(0))
                            End If
                            Me.TradeItems.Add(New TradeItem(itemData(0), ResultAmount, ScriptConversion.ToInteger(itemData(2)), Me.Currency))
                            Me.PossibleStoreItems.Add(New TradeItem(itemData(0), -1, ScriptConversion.ToInteger(itemData(2)), Me.Currency))
                        End If
                    End If
                Else
                    Me.TradeItems.Add(New TradeItem(itemData(0), ScriptConversion.ToInteger(itemData(1)), ScriptConversion.ToInteger(itemData(2)), Me.Currency))
                    Me.PossibleStoreItems.Add(New TradeItem(itemData(0), -1, ScriptConversion.ToInteger(itemData(2)), Me.Currency))
                End If
            Else
                Me.TradeItems.Add(New TradeItem(itemData(0), ScriptConversion.ToInteger(itemData(1)), ScriptConversion.ToInteger(itemData(2)), Me.Currency))
                Me.PossibleStoreItems.Add(New TradeItem(itemData(0), -1, ScriptConversion.ToInteger(itemData(2)), Me.Currency))
            End If

        Next

        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        Me.MouseVisible = True
        Me.CanMuteAudio = True
        Me.CanBePaused = True

        Me.CanBuyItems = canBuy
        Me.CanSellItems = canSell

        Me.Title = Localization.GetString("shop_screen_title_store", "Store")

        Me.CreateMainMenuButtons()
    End Sub

    Private Sub CreateMainMenuButtons()
        If mainMenuButtons.Count = 0 Then
            If CanBuyItems = True Then
                mainMenuButtons.Add(Localization.GetString("shop_screen_button_buy", "Buy"))
            End If
            If CanSellItems = True Then
                mainMenuButtons.Add(Localization.GetString("shop_screen_button_sell", "Sell"))
            End If
            mainMenuButtons.Add(Localization.GetString("shop_screen_button_exit", "Exit"))
        End If
    End Sub

    ''' <summary>
    ''' Sets the currency from the currency indicator string.
    ''' </summary>
    ''' <param name="currencyIndicator">The currency indicator.</param>
    Private Sub SetCurrency(ByVal currencyIndicator As String)
        Select Case currencyIndicator.ToLower()
            Case "p", "pokedollar", "pokédollar", "poke", "poké", "poke dollar", "poké dollar", "money"
                Me.Currency = Currencies.Pokédollar
            Case "bp", "battlepoints", "battle points"
                Me.Currency = Currencies.BattlePoints
            Case "c", "coins"
                Me.Currency = Currencies.Coins
        End Select
    End Sub

    Public Overrides Sub Update()
        Select Case Me.MenuState
            Case MenuStates.MainPage
                Me.UpdateMain()
            Case MenuStates.BuyItemsCategory
                Me.UpdateBuyCategory()
            Case MenuStates.BuyItems
                Me.UpdateBuyItems()
            Case MenuStates.SellItemsCategory
                Me.UpdateSellCategory()
            Case MenuStates.SellItems
                Me.UpdateSellItems()
            Case MenuStates.SellItemsConfirmation
                Me.UpdateSellConfirmation()
        End Select

        Me.TileOffset += 1
        If Me.TileOffset >= 64 Then
            Me.TileOffset = 0
        End If
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(84, 198, 216))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + Me.TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), Color.White)
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198), False, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Title, New Vector2(100, 24), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        Select Case Me.MenuState
            Case MenuStates.MainPage
                Me.DrawMain()
            Case MenuStates.BuyItemsCategory
                Me.DrawBuyCategory()
            Case MenuStates.BuyItems
                Me.DrawBuyItems()
            Case MenuStates.SellItemsCategory
                Me.DrawSellCategory()
            Case MenuStates.SellItems
                Me.DrawSellItems()
            Case MenuStates.SellItemsConfirmation
                Me.DrawSellConfirmation()
        End Select
    End Sub

#Region "Mainscreen"

    Dim mainMenuButtons As New List(Of String)

    ''' <summary>
    ''' Updates the main screen.
    ''' </summary>
    Private Sub UpdateMain()
        Me.Title = Localization.GetString("shop_screen_title_store", "Store")

        If Controls.Up(True, True, True, True, True, True) = True Then
            Me.Cursor -= 1
            If Controls.ShiftDown() = True Then
                Me.Cursor -= 4
            End If
        End If
        If Controls.Down(True, True, True, True, True, True) = True Then
            Me.Cursor += 1
            If Controls.ShiftDown() = True Then
                Me.Cursor += 4
            End If
        End If
        Me.Cursor = Me.Cursor.Clamp(0, mainMenuButtons.Count - 1)

        If Controls.Accept(True, False, False) = True Then
            For i = 0 To mainMenuButtons.Count - 1
                If New Rectangle(100, 100 + i * 96, 64 * 7, 64).Contains(MouseHandler.MousePosition) = True Then
                    If i = Me.Cursor Then
                        SoundManager.PlaySound("select")
                        Me.ClickMainButton()
                    Else
                        Cursor = i
                    End If
                End If
            Next
        End If

        If Controls.Accept(False, True, True) = True Then
            SoundManager.PlaySound("select")
            Me.ClickMainButton()
        End If

        If Controls.Dismiss(True, True, True) = True Then
            SoundManager.PlaySound("select")
            Me.ButtonMainExit()
        End If
    End Sub

    Private Sub ClickMainButton()
        Select Case mainMenuButtons(Me.Cursor)
            Case Localization.GetString("shop_screen_button_buy", "Buy")
                Me.ButtonMainBuy()
            Case Localization.GetString("shop_screen_button_sell", "Sell")
                Me.ButtonMainSell()
            Case Localization.GetString("shop_screen_button_exit", "Exit")
                Me.ButtonMainExit()
        End Select
    End Sub

    ''' <summary>
    ''' Event when clicking on the Buy button.
    ''' </summary>
    Private Sub ButtonMainBuy()
        Me.MenuState = MenuStates.BuyItemsCategory
        Me.Cursor = 0
        Me.Scroll = 0
        Me.LoadBuyCategoriesItems()
    End Sub

    ''' <summary>
    ''' Event when clicking on the Sell button.
    ''' </summary>
    Private Sub ButtonMainSell()
        Me.MenuState = MenuStates.SellItemsCategory
        Me.Cursor = 0
        Me.Scroll = 0
        Me.LoadSellCategoryItems()
    End Sub

    ''' <summary>
    ''' Event when clicking on the Exit button.
    ''' </summary>
    Private Sub ButtonMainExit()
        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, Me.PreScreen, Color.White, False))
    End Sub

    ''' <summary>
    ''' Draw the main screen.
    ''' </summary>
    Private Sub DrawMain()
        Dim y As Integer = 100
        For Each b As String In mainMenuButtons
            DrawButton(New Vector2(100, y), 5, b, 16)
            y += 96
        Next

        Me.DrawMainCursor()
    End Sub

    ''' <summary>
    ''' Draw the cursor on the main screen.
    ''' </summary>
    Private Sub DrawMainCursor()
        Dim cPosition As Vector2 = New Vector2(380, 100 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

#End Region

#Region "BuyCategoryScreen"

    Private loadedBuyCategories As New List(Of Items.ItemTypes)

    Private Sub LoadBuyCategoriesItems()
        Me.loadedBuyCategories.Clear()
        For Each i As TradeItem In Me.TradeItems
            Dim item As Item = i.GetItem()

            If loadedBuyCategories.Contains(item.ItemType) = False And item.CanBeTraded = True Then
                loadedBuyCategories.Add(item.ItemType)
            End If
        Next
        Me.loadedBuyCategories = (From c As Items.ItemTypes In Me.loadedBuyCategories Order By CInt(c)).ToList()
    End Sub

    Private Sub UpdateBuyCategory()
        Me.Title = Localization.GetString("shop_screen_title_BuyItems", "Buy Items")

        If loadedBuyCategories.Count > 0 Then
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

            If Me.loadedBuyCategories.Count < 7 Then
                Me.Scroll = 0
            Else
                Me.Scroll = Me.Scroll.Clamp(0, Me.loadedBuyCategories.Count - 6)
            End If
            If Me.loadedBuyCategories.Count < 6 Then
                Me.Cursor = Me.Cursor.Clamp(0, Me.loadedBuyCategories.Count - 1)
            Else
                Me.Cursor = Me.Cursor.Clamp(0, 5)
            End If

            If Controls.Accept(True, False, False) = True Then
                For i = Scroll To Scroll + 5
                    If i <= Me.loadedBuyCategories.Count - 1 Then
                        If New Rectangle(100, 100 + (i - Me.Scroll) * 96, 64 * 7, 64).Contains(MouseHandler.MousePosition) = True Then
                            If i = Scroll + Cursor Then
                                SoundManager.PlaySound("select")
                                Me.ButtonBuyCategoriesAccept()
                            Else
                                Cursor = i - Scroll
                            End If
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                SoundManager.PlaySound("select")
                Me.ButtonBuyCategoriesAccept()
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            SoundManager.PlaySound("select")
            Me.MenuState = MenuStates.MainPage
        End If
    End Sub

    Private Sub ButtonBuyCategoriesAccept()
        Me.CurrentCategory = Me.loadedBuyCategories(Me.Cursor + Me.Scroll)
        Me.MenuState = MenuStates.BuyItems
        Me.Cursor = 0
        Me.Scroll = 0
        Me.LoadBuyItemsList()
    End Sub

    Private Sub DrawBuyCategory()
        If Me.loadedBuyCategories.Count > 0 Then
            For i = Scroll To Scroll + 5
                If i <= Me.loadedBuyCategories.Count - 1 Then
                    Dim p As Integer = i - Scroll

                    DrawButton(New Vector2(100, 100 + p * 96), 5, Localization.GetString("item_category_" & Me.loadedBuyCategories(i).ToString(), Me.loadedBuyCategories(i).ToString()), 16, GetItemTypeTexture(Me.loadedBuyCategories(i)))
                End If
            Next

            Canvas.DrawRectangle(New Rectangle(580, 100, 240, 48), New Color(255, 255, 255, 127))

            If Me.loadedBuyCategories.Count > 0 Then
                Dim x As Integer = 0
                Dim y As Integer = 0

                For Each i As TradeItem In Me.TradeItems
                    Dim item As Item = i.GetItem()
                    If item.ItemType = Me.loadedBuyCategories(Me.Cursor + Me.Scroll) Then
                        SpriteBatch.Draw(item.Texture, New Rectangle(580 + x * 48, 100 + y * 48, 48, 48), Color.White)

                        x += 1
                        If x = 5 Then
                            x = 0
                            y += 1
                            Canvas.DrawRectangle(New Rectangle(580, 100 + y * 48, 240, 48), New Color(255, 255, 255, 127))
                        End If
                    End If
                Next
            End If

            Me.DrawMainCursor()
        Else
            DrawBanner(New Vector2(CSng(Core.windowSize.Width / 2 - 250), CSng(Core.windowSize.Height / 2 - 50)), 100, Localization.GetString("shop_screen_buy_NoItemsToBuy", "There are no items to buy."), FontManager.MainFont, 500)
        End If
    End Sub

#End Region

#Region "BuyItemsScreen"

    Private BuySellSparkleRotation As Single = 0.0F
    Private BuySellItemSize As Single = 192.0F
    Private BuySellItemShrinking As Boolean = True

    Private BuyItemsList As New List(Of TradeItem)
    Private BuyItemsAmount As Integer = 1
    Private BuyItemsShowDescription As Boolean = False

    Private Sub LoadBuyItemsList()
        Me.BuyItemsList.Clear()
        For Each i As TradeItem In Me.TradeItems
            Dim item As Item = i.GetItem()
            If item.ItemType = Me.CurrentCategory Then
                BuyItemsList.Add(i)
            End If
        Next
        Me.BuyItemsList = (From i As TradeItem In BuyItemsList Order By i.GetItem().OneLineName).ToList()
    End Sub

    Private Sub UpdateBuyItems()
        Me.Title = Localization.GetString("shop_screen_title_BuyCategory", "Buy [CATEGORY]").Replace("[CATEGORY]", Localization.GetString("item_category_" & Me.CurrentCategory.ToString(), Me.CurrentCategory.ToString()))

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
        If Controls.Right(True, True, False, True, True, True) = True Then
            Me.BuyItemsAmount += 1
            If Controls.ShiftDown() = True Then
                Me.BuyItemsAmount += 4
            End If
        End If
        If Controls.Left(True, True, False, True, True, True) = True Then
            Me.BuyItemsAmount -= 1
            If Controls.ShiftDown() = True Then
                Me.BuyItemsAmount -= 4
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

        If Me.BuyItemsList.Count < 7 Then
            Me.Scroll = 0
        Else
            Me.Scroll = Me.Scroll.Clamp(0, Me.BuyItemsList.Count - 6)
        End If
        If Me.BuyItemsList.Count < 6 Then
            Me.Cursor = Me.Cursor.Clamp(0, Me.BuyItemsList.Count - 1)
        Else
            Me.Cursor = Me.Cursor.Clamp(0, 5)
        End If

        If Controls.Accept(True, False, False) = True Then
            For i = Scroll To Scroll + 5
                If i <= Me.BuyItemsList.Count - 1 Then
                    If New Rectangle(100, 100 + (i - Me.Scroll) * 96, 64 * 7, 64).Contains(MouseHandler.MousePosition) = True Then
                        Cursor = i - Scroll
                    End If
                End If
            Next
            ' Item description:
            If New Rectangle(736, 160, 256, 256).Contains(MouseHandler.MousePosition) = True Then
                Me.BuyItemsShowDescription = Not Me.BuyItemsShowDescription
            End If

            ' - button:
            If New Rectangle(664, 484, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                Me.ButtonBuyItemsMinus()
            End If
            ' + button:
            If New Rectangle(856, 484, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                Me.ButtonBuyItemsPlus()
            End If

            ' Buy button:
            If New Rectangle(664 + 32, 484 + 64 + 22, 64 * 3, 64).Contains(MouseHandler.MousePosition) = True Then
                Me.ButtonBuyItemsBuy()
            End If
        End If

        If Me.Cursor > Me.BuyItemsList.Count - 1 Then
            Me.Cursor = Me.BuyItemsList.Count - 1
        End If

        If ControllerHandler.ButtonPressed(Buttons.Back) = True Or KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Then
            Me.BuyItemsShowDescription = Not Me.BuyItemsShowDescription
        End If

        If Me.BuyItemsList.Count > 0 Then
            Me.BuyItemsAmount = Me.BuyItemsAmount.Clamp(0, Me.GetMaxBuyItemAmount(Me.BuyItemsList(Me.Scroll + Me.Cursor)))
        End If

        If Controls.Accept(False, True, True) = True Then
            Me.ButtonBuyItemsBuy()
        End If

        If Controls.Dismiss(True, True, True) = True Then
            If Me.BuyItemsShowDescription = True Then
                SoundManager.PlaySound("select")
                Me.BuyItemsShowDescription = False
            Else
                SoundManager.PlaySound("select")
                Me.MenuState = MenuStates.BuyItemsCategory
            End If
        End If

        Me.BuySellSparkleRotation += 0.005F

        If BuySellItemShrinking = True Then
            BuySellItemSize -= 0.5F
            If BuySellItemSize <= 160.0F Then
                BuySellItemShrinking = False
            End If
        Else
            BuySellItemSize += 0.5F
            If BuySellItemSize >= 192.0F Then
                BuySellItemShrinking = True
            End If
        End If
    End Sub

    Private Sub ButtonBuyItemsMinus()
        If Controls.ShiftDown() = True Then
            Me.BuyItemsAmount -= 5
        Else
            Me.BuyItemsAmount -= 1
        End If
    End Sub

    Private Sub ButtonBuyItemsPlus()
        If Controls.ShiftDown() = True Then
            Me.BuyItemsAmount += 5
        Else
            Me.BuyItemsAmount += 1
        End If
    End Sub

    Private Sub ButtonBuyItemsBuy()
        If BuyItemsAmount > 0 Then
            Dim tradeItem As TradeItem = Me.BuyItemsList(Me.Scroll + Me.Cursor)

            Me.ChangeCurrencyAmount(-(tradeItem.Price * Me.BuyItemsAmount))
            Core.Player.Inventory.AddItem(tradeItem.ItemID, Me.BuyItemsAmount)

            ' Add a Premier Ball (ID=3) if the player bought 10 or more Poké Balls (ID=5):
            If tradeItem.ItemID = 5.ToString And Me.BuyItemsAmount >= 10 Then
                Core.Player.Inventory.AddItem(3.ToString, 1)
            End If

            ' Remove trade item from seller's side if the rest amount is smaller than 0:
            If tradeItem.Amount > -1 Then
                For i = 0 To Me.TradeItems.Count - 1
                    If Me.TradeItems(i).ItemID = tradeItem.ItemID And tradeItem.Amount = Me.TradeItems(i).Amount Then
                        If ShopIdentifier <> "" Then
                            If ActionScript.IsRegistered(Me.ShopIdentifier & "_" & tradeItem.ItemID) = False Then
                                ActionScript.RegisterID(Me.ShopIdentifier & "_" & tradeItem.ItemID, "int", "0")
                            End If

                            Dim registerContent() As Object = ActionScript.GetRegisterValue(ShopIdentifier & "_" & tradeItem.ItemID)
                            ActionScript.ChangeRegister(Me.ShopIdentifier & "_" & tradeItem.ItemID, CStr(CInt(registerContent(0)) + BuyItemsAmount))
                        End If

                        Dim t As TradeItem = Me.TradeItems(i)
                        t.Amount -= Me.BuyItemsAmount

                        If t.Amount < 1 Then
                            Me.TradeItems.RemoveAt(i)
                        Else
                            Me.TradeItems(i) = t
                        End If
                        Exit For
                    End If
                Next
            End If

            Me.LoadBuyItemsList()

            SoundManager.PlaySound("buy")

            If Me.BuyItemsList.Count = 0 Then
                Me.MenuState = MenuStates.BuyItemsCategory
                Me.Cursor -= 1
                Me.LoadBuyCategoriesItems()
            End If
        End If
    End Sub

    Private Function GetMaxBuyItemAmount(ByVal tradeItem As TradeItem) As Integer
        Dim item As Item = tradeItem.GetItem()
        Dim ItemID As String
        If item.IsGameModeItem Then
            ItemID = item.gmID
        Else
            ItemID = item.ID.ToString
        End If
        Dim maxAmount As Integer = item.MaxStack - Core.Player.Inventory.GetItemAmount(ItemID)

        If maxAmount > tradeItem.Amount And tradeItem.Amount > -1 Then
            maxAmount = tradeItem.Amount
        End If

        If tradeItem.Price = 0 Then
            Return maxAmount
        End If

        Dim money As Integer = Me.GetCurrencyAmount()
        Dim amount As Integer = CInt(Math.Floor(money / tradeItem.Price))
        Return amount.Clamp(0, maxAmount)
    End Function

    Private Sub DrawBuyItems()
        ' Item selection menu:
        For i = Scroll To Scroll + 5
            If i <= Me.BuyItemsList.Count - 1 Then
                Dim p As Integer = i - Scroll

                DrawButton(New Vector2(100, 100 + p * 96), 5, Me.BuyItemsList(i).GetItem().OneLineName, 16, Me.BuyItemsList(i).GetItem().Texture)
            End If
        Next

        If Me.BuyItemsList.Count > 0 Then
            Dim DescriptionHint As String = ScriptVersion2.ScriptCommander.Parse(Localization.GetString("shop_screen_buysell_DescriptionHint", "Press [<system.button(special)>] or Select to view the item's description.")).ToString
            Core.SpriteBatch.DrawString(FontManager.InGameFont, DescriptionHint, New Vector2(128 + 2, Core.windowSize.Width - 128 + 2), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, DescriptionHint, New Vector2(128, Core.windowSize.Width - 128), Color.White)

            While BuyItemsList.Count <= Scroll + Cursor
                Cursor -= 1
            End While
            Dim selectedItem As TradeItem = Me.BuyItemsList(Scroll + Cursor)

            ' Item preview:
            Core.SpriteBatch.EndBatch()
            Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise)
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Box\Sparkle"), New Rectangle(736 + 128, 160 + 128, 256, 256), Nothing, Color.White, BuySellSparkleRotation, New Vector2(128, 128), SpriteEffects.None, 0.0F)
            Core.SpriteBatch.End()
            Core.SpriteBatch.BeginBatch()

            Dim itemOffset As Single = (256 - BuySellItemSize) / 2.0F
            Core.SpriteBatch.Draw(selectedItem.GetItem().Texture, New Rectangle(CInt(736 + itemOffset), CInt(160 + itemOffset), CInt(BuySellItemSize), CInt(BuySellItemSize)), Color.White)

            If BuyItemsShowDescription = True Then
                Canvas.DrawRectangle(New Rectangle(736 + 28, 160 + 28, 200, 200), New Color(0, 0, 0, 200))
                Dim t As String = selectedItem.GetItem().GetDescription().CropStringToWidth(FontManager.MiniFont, 180)
                SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(736 + 30, 160 + 30), Color.White)
            End If

            ' Amount of the selected item in the player's Bag:
            Dim amount As String = Core.Player.Inventory.GetItemAmount(selectedItem.ItemID).ToString()
            While amount.Length < 3
                amount = "0" & amount
            End While
            Dim bannerText As String = ""
            If selectedItem.Amount > -1 Then
                bannerText = " | " & Localization.GetString("shop_screen_buy_InStock", "In Stock:") & " " & selectedItem.Amount
            End If
            Me.DrawBanner(New Vector2(664, 430), 30, Localization.GetString("shop_screen_buysell_InInventory", "In Inventory:") & " " & amount & bannerText, FontManager.MainFont, 400)

            ' - button:
            Core.SpriteBatch.Draw(texture, New Rectangle(664, 484, 64, 64), New Rectangle(16, 32, 16, 16), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "-", New Vector2(664 + 23, 484 + 2), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

            ' Amount field:
            Canvas.DrawRectangle(New Rectangle(740, 492, 104, 48), New Color(77, 147, 198))
            Canvas.DrawRectangle(New Rectangle(744, 496, 96, 40), New Color(232, 240, 248))
            Dim amountString As String = Me.BuyItemsAmount.ToString()
            While amountString.Length < 3
                amountString = "0" & amountString
            End While
            amountString = "x" & amountString
            Core.SpriteBatch.DrawString(FontManager.MainFont, amountString, New Vector2(792 - FontManager.MainFont.MeasureString(amountString).X / 2.0F, 504), Color.Black)

            ' + button:
            Core.SpriteBatch.Draw(texture, New Rectangle(856, 484, 64, 64), New Rectangle(16, 32, 16, 16), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "+", New Vector2(856 + 19, 484 + 6), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("shop_screen_buysell_PricePerItem", "Per Item:") & " " & selectedItem.Price.ToString() & GetCurrencyShort() & Environment.NewLine &
                                                       Localization.GetString("shop_screen_buysell_PriceTotal", "Total:") & " " & (BuyItemsAmount * selectedItem.Price).ToString() & GetCurrencyShort(), New Vector2(930, 490), Color.White)

            ' Buy button:
            If Me.BuyItemsAmount > 0 Then
                If ControllerHandler.IsConnected() = True Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\GamePad\xboxControllerButtonA"), New Rectangle(664 + 12, 484 + 64 + 34, 40, 40), Color.White)
                End If

                Me.DrawButton(New Vector2(664 + 32, 484 + 64 + 22), 1, Localization.GetString("shop_screen_button_buy", "Buy"), 64)
            End If
        End If

        ' Current balance:
        Me.DrawBanner(New Vector2(664, 110), 30, Localization.GetString("shop_screen_buysell_CurrentBalance", "Current balance:") & " " & GetCurrencyDisplay(), FontManager.MainFont, 400)

        ' Cursor draw:
        Me.DrawMainCursor()
    End Sub

#End Region

#Region "SellCatetoryScreen"

    Private loadedSellCategories As New List(Of Items.ItemTypes)

    Private Sub LoadSellCategoryItems()
        Me.loadedSellCategories.Clear()

        For Each c In Core.Player.Inventory
            Dim i = Item.GetItemByID(c.ItemID)
            If loadedSellCategories.Contains(i.ItemType) = False And i.CanBeTraded = True Then
                loadedSellCategories.Add(i.ItemType)
            End If
        Next
        Me.loadedSellCategories = (From c As Items.ItemTypes In Me.loadedSellCategories Order By CInt(c)).ToList()
    End Sub

    Private Sub UpdateSellCategory()
        Me.Title = Localization.GetString("shop_screen_title_SellItems", "Sell Items")

        If Me.loadedSellCategories.Count > 0 Then
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

            If Me.loadedSellCategories.Count < 7 Then
                Me.Scroll = 0
            Else
                Me.Scroll = Me.Scroll.Clamp(0, Me.loadedSellCategories.Count - 6)
            End If
            If Me.loadedSellCategories.Count < 6 Then
                Me.Cursor = Me.Cursor.Clamp(0, Me.loadedSellCategories.Count - 1)
            Else
                Me.Cursor = Me.Cursor.Clamp(0, 5)
            End If

            If Controls.Accept(True, False, False) = True Then
                For i = Scroll To Scroll + 5
                    If i <= Me.loadedSellCategories.Count - 1 Then
                        If New Rectangle(100, 100 + (i - Me.Scroll) * 96, 64 * 7, 64).Contains(MouseHandler.MousePosition) = True Then
                            If i = Scroll + Cursor Then
                                SoundManager.PlaySound("select")
                                Me.ButtonSellCategoriesAccept()
                            Else
                                Cursor = i - Scroll
                            End If
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                SoundManager.PlaySound("select")
                Me.ButtonSellCategoriesAccept()
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            SoundManager.PlaySound("select")
            Me.MenuState = MenuStates.MainPage
        End If
    End Sub

    Private Sub ButtonSellCategoriesAccept()
        Me.CurrentCategory = Me.loadedSellCategories(Me.Cursor + Me.Scroll)
        Me.MenuState = MenuStates.SellItems
        Me.Cursor = 0
        Me.Scroll = 0
        Me.LoadSellItemsList()
    End Sub

    Private Sub DrawSellCategory()
        If Me.loadedSellCategories.Count > 0 Then
            For i = Scroll To Scroll + 5
                If i <= Me.loadedSellCategories.Count - 1 Then
                    Dim p As Integer = i - Scroll

                    DrawButton(New Vector2(100, 100 + p * 96), 5, Localization.GetString("item_category_" & Me.loadedSellCategories(i).ToString(), Me.loadedSellCategories(i).ToString()), 16, GetItemTypeTexture(Me.loadedSellCategories(i)))
                End If
            Next

            Me.DrawMainCursor()
        Else
            DrawBanner(New Vector2(CSng(Core.windowSize.Width / 2 - 250), CSng(Core.windowSize.Height / 2 - 50)), 100, Localization.GetString("shop_screen_sell_NoItemsToSell", "You have no items to sell."), FontManager.MainFont, 500)
        End If
    End Sub

#End Region

#Region "SellItemsScreen"

    Private SellItemsList As New List(Of TradeItem)
    Private SellItemsAmount As Integer = 1
    Private SellItemsShowDescription As Boolean = False

    Private Sub LoadSellItemsList()
        Me.SellItemsList.Clear()
        For Each c In Core.Player.Inventory
            Dim i = Item.GetItemByID(c.ItemID)
            Dim ItemID As String
            If i.IsGameModeItem Then
                ItemID = i.gmID
            Else
                ItemID = i.ID.ToString
            End If

            If i.CanBeTraded = True Then
                If i.ItemType = Me.CurrentCategory Then
                    Dim price As Integer = -1
                    For Each sellItem As TradeItem In Me.PossibleStoreItems
                        If sellItem.ItemID = ItemID Then
                            price = sellItem.Price
                        End If
                    Next
                    SellItemsList.Add(New TradeItem(ItemID, c.Amount, price, Me.Currency))
                End If
            End If
        Next
        Me.SellItemsList = (From i As TradeItem In SellItemsList Order By i.GetItem().OneLineName).ToList()
    End Sub

    Private Sub UpdateSellItems()
        Me.Title = Localization.GetString("shop_screen_title_SellCategory", "Sell [CATEGORY]").Replace("[CATEGORY]", Localization.GetString("item_category_" & Me.CurrentCategory.ToString(), Me.CurrentCategory.ToString()))

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
        If Controls.Right(True, True, False, True, True, True) = True Then
            Me.SellItemsAmount += 1
            If Controls.ShiftDown() = True Then
                Me.SellItemsAmount += 4
            End If
        End If
        If Controls.Left(True, True, False, True, True, True) = True Then
            Me.SellItemsAmount -= 1
            If Controls.ShiftDown() = True Then
                Me.SellItemsAmount -= 4
            End If
        End If

        Me.SellItemsClampCursor()

        If Controls.Accept(True, False, False) = True Then
            For i = Scroll To Scroll + 5
                If i <= Me.SellItemsList.Count - 1 Then
                    If New Rectangle(100, 100 + (i - Me.Scroll) * 96, 64 * 7, 64).Contains(MouseHandler.MousePosition) = True Then
                        Cursor = i - Scroll
                    End If
                End If
            Next

            ' Item description:
            If New Rectangle(736, 160, 256, 256).Contains(MouseHandler.MousePosition) = True Then
                Me.SellItemsShowDescription = Not Me.SellItemsShowDescription
            End If

            ' - button:
            If New Rectangle(664, 484, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                Me.ButtonSellItemsMinus()
            End If
            ' + button:
            If New Rectangle(856, 484, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                Me.ButtonSellItemsPlus()
            End If

            ' Buy button:
            If New Rectangle(664 + 32, 484 + 64 + 22, 64 * 3, 64).Contains(MouseHandler.MousePosition) = True Then
                SoundManager.PlaySound("select")
                Me.ButtonSellItemsSell()
            End If
        End If

        If ControllerHandler.ButtonPressed(Buttons.Back) = True Or KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Then
            Me.SellItemsShowDescription = Not Me.SellItemsShowDescription
        End If

        If Me.SellItemsList.Count > 0 Then
            Me.SellItemsAmount = Me.SellItemsAmount.Clamp(0, Me.SellItemsList(Me.Scroll + Me.Cursor).Amount)
        End If

        If Controls.Accept(False, True, True) = True Then
            SoundManager.PlaySound("select")
            Me.ButtonSellItemsSell()
        End If

        If Controls.Dismiss(True, True, True) = True Then
            If Me.SellItemsShowDescription = True Then
                Me.SellItemsShowDescription = False
                SoundManager.PlaySound("select")
            Else
                Me.MenuState = MenuStates.SellItemsCategory
                SoundManager.PlaySound("select")
            End If
        End If

        Me.BuySellSparkleRotation += 0.005F

        If BuySellItemShrinking = True Then
            BuySellItemSize -= 0.5F
            If BuySellItemSize <= 160.0F Then
                BuySellItemShrinking = False
            End If
        Else
            BuySellItemSize += 0.5F
            If BuySellItemSize >= 192.0F Then
                BuySellItemShrinking = True
            End If
        End If
    End Sub

    Private Sub SellItemsClampCursor()
        While Me.Cursor > 5
            Me.Cursor -= 1
            Me.Scroll += 1
        End While
        While Me.Cursor < 0
            Me.Cursor += 1
            Me.Scroll -= 1
        End While

        If Me.SellItemsList.Count < 7 Then
            Me.Scroll = 0
        Else
            Me.Scroll = Me.Scroll.Clamp(0, Me.SellItemsList.Count - 6)
        End If
        If Me.SellItemsList.Count < 6 Then
            Me.Cursor = Me.Cursor.Clamp(0, Me.SellItemsList.Count - 1)
        Else
            Me.Cursor = Me.Cursor.Clamp(0, 5)
        End If
    End Sub

    Private Sub ButtonSellItemsMinus()
        If Controls.ShiftDown() = True Then
            Me.SellItemsAmount -= 5
        Else
            Me.SellItemsAmount -= 1
        End If
    End Sub

    Private Sub ButtonSellItemsPlus()
        If Controls.ShiftDown() = True Then
            Me.SellItemsAmount += 5
        Else
            Me.SellItemsAmount += 1
        End If
    End Sub

    Private Sub ButtonSellItemsSell()
        If SellItemsAmount > 0 Then
            sellItemsConfirmationCursor = 0
            Me.MenuState = MenuStates.SellItemsConfirmation
        End If
    End Sub

    Private Sub DrawSellItems()
        ' Item selection menu:
        For i = Scroll To Scroll + 5
            If i <= Me.SellItemsList.Count - 1 Then
                Dim p As Integer = i - Scroll

                DrawButton(New Vector2(100, 100 + p * 96), 5, Me.SellItemsList(i).GetItem().OneLineName, 16, Me.SellItemsList(i).GetItem().Texture)
            End If
        Next

        If Me.SellItemsList.Count > 0 Then
            Dim DescriptionHint As String = ScriptVersion2.ScriptCommander.Parse(Localization.GetString("shop_screen_buysell_DescriptionHint", "Press [<system.button(special)>] or Select to view the item's description.")).ToString
            Core.SpriteBatch.DrawString(FontManager.InGameFont, DescriptionHint, New Vector2(128 + 2, Core.windowSize.Width - 128 + 2), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, DescriptionHint, New Vector2(128, Core.windowSize.Width - 128), Color.White)

            Dim selectedItem As TradeItem = Me.SellItemsList(Scroll + Cursor)

            ' Item preview:
            Core.SpriteBatch.EndBatch()
            Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise)
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Box\Sparkle"), New Rectangle(736 + 128, 160 + 128, 256, 256), Nothing, Color.White, BuySellSparkleRotation, New Vector2(128, 128), SpriteEffects.None, 0.0F)
            Core.SpriteBatch.End()
            Core.SpriteBatch.BeginBatch()

            Dim itemOffset As Single = (256 - BuySellItemSize) / 2.0F
            Core.SpriteBatch.Draw(selectedItem.GetItem().Texture, New Rectangle(CInt(736 + itemOffset), CInt(160 + itemOffset), CInt(BuySellItemSize), CInt(BuySellItemSize)), Color.White)

            If Me.SellItemsShowDescription = True Then
                Canvas.DrawRectangle(New Rectangle(736 + 28, 160 + 28, 200, 200), New Color(0, 0, 0, 200))
                Dim t As String = selectedItem.GetItem().GetDescription().CropStringToWidth(FontManager.MiniFont, 180)
                SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(736 + 30, 160 + 30), Color.White)
            End If

            ' Amount of the selected item in the player's Bag:
            Dim amount As String = Core.Player.Inventory.GetItemAmount(selectedItem.ItemID).ToString()
            While amount.Length < 3
                amount = "0" & amount
            End While
            Me.DrawBanner(New Vector2(664, 430), 30, Localization.GetString("shop_screen_buysell_InInventory", "In Inventory:") & " " & amount, FontManager.MainFont, 400)

            ' - button:
            Core.SpriteBatch.Draw(texture, New Rectangle(664, 484, 64, 64), New Rectangle(16, 32, 16, 16), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "-", New Vector2(664 + 23, 484 + 2), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

            ' Amount field:
            Canvas.DrawRectangle(New Rectangle(740, 492, 104, 48), New Color(77, 147, 198))
            Canvas.DrawRectangle(New Rectangle(744, 496, 96, 40), New Color(232, 240, 248))
            Dim amountString As String = Me.SellItemsAmount.ToString()
            While amountString.Length < 3
                amountString = "0" & amountString
            End While
            amountString = "x" & amountString
            Core.SpriteBatch.DrawString(FontManager.MainFont, amountString, New Vector2(792 - FontManager.MainFont.MeasureString(amountString).X / 2.0F, 504), Color.Black)

            ' + button:
            Core.SpriteBatch.Draw(texture, New Rectangle(856, 484, 64, 64), New Rectangle(16, 32, 16, 16), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "+", New Vector2(856 + 19, 484 + 6), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("shop_screen_buysell_PricePerItem", "Per Item:") & " " & selectedItem.SellPrice().ToString() & GetCurrencyShort() & Environment.NewLine &
                                                      Localization.GetString("shop_screen_buysell_PriceTotal", "Total:") & " " & (SellItemsAmount * selectedItem.SellPrice()).ToString() & GetCurrencyShort(), New Vector2(930, 490), Color.White)

            ' Sell button:
            If Me.SellItemsAmount > 0 Then
                If ControllerHandler.IsConnected() = True Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\GamePad\xboxControllerButtonA"), New Rectangle(664 + 12, 484 + 64 + 34, 40, 40), Color.White)
                End If

                Me.DrawButton(New Vector2(664 + 32, 484 + 64 + 22), 1, Localization.GetString("shop_screen_button_sell", "Sell"), 64)
            End If
        End If

        ' Current balance:
        Me.DrawBanner(New Vector2(664, 110), 30, Localization.GetString("shop_screen_buysell_CurrentBalance", "Current balance:") & " " & GetCurrencyDisplay(), FontManager.MainFont, 400)

        ' Cursor draw:
        Me.DrawMainCursor()
    End Sub

#End Region

#Region "SellItemsConfirmationScreen"

    Private sellItemsConfirmationCursor As Integer = 0

    Private Sub UpdateSellConfirmation()
        If Controls.Down(True, True, True, True, True, True) = True Then
            Me.sellItemsConfirmationCursor += 1
        End If
        If Controls.Up(True, True, True, True, True, True) = True Then
            Me.sellItemsConfirmationCursor -= 1
        End If

        Me.sellItemsConfirmationCursor = Me.sellItemsConfirmationCursor.Clamp(0, 1)

        If Controls.Accept(True, False, False) = True Then
            For i = 0 To 1
                If New Rectangle(CInt(Core.windowSize.Width / 2.0F - 192), CInt(Core.windowSize.Height / 2.0F - 60 + (i * 96)), 64 * 5, 64).Contains(MouseHandler.MousePosition) = True Then
                    If sellItemsConfirmationCursor = i Then
                        Select Case i
                            Case 0
                                ButtonSellConfirmationSell()
                            Case 1
                                ButtonSellConfirmationCancel()
                        End Select
                    Else
                        sellItemsConfirmationCursor = i
                    End If
                End If
            Next
        End If

        If Controls.Accept(False, True, True) = True Then
            Select Case sellItemsConfirmationCursor
                Case 0
                    ButtonSellConfirmationSell()
                Case 1
                    ButtonSellConfirmationCancel()
            End Select
        End If
    End Sub

    Private Sub DrawSellConfirmation()
        Me.DrawSellItems()

        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), CInt(Core.windowSize.Height / 2 - 200), 600, 400), New Color(0, 0, 0, 150))

        Dim tradeItem As TradeItem = Me.SellItemsList(Me.Scroll + Me.Cursor)

        Dim itemName As String = tradeItem.GetItem().OneLineName()
        If Me.SellItemsAmount > 1 Then
            itemName = tradeItem.GetItem().OneLinePluralName()
        End If
        Dim text As String = Localization.GetString("shop_screen_sell_confirmation", "Do you want to sell~[AMOUNT] [ITEM]?").Replace("~", Environment.NewLine).Replace("*", Environment.NewLine).Replace("[AMOUNT]", Me.SellItemsAmount.ToString).Replace("[ITEM]", itemName)

        Core.SpriteBatch.DrawString(FontManager.MainFont,
                                    text,
                                    New Vector2(Core.windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(text).X, Core.windowSize.Height / 2.0F - 170),
                                    Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        DrawButton(New Vector2(Core.windowSize.Width / 2.0F - 192, Core.windowSize.Height / 2.0F - 60), 4, Localization.GetString("shop_screen_button_sell", "Sell"), 16, Nothing)
        DrawButton(New Vector2(Core.windowSize.Width / 2.0F - 192, Core.windowSize.Height / 2.0F + 36), 4, Localization.GetString("global_cancel", "Cancel"), 16, Nothing)

        ' Cursor:
        Dim cPosition As Vector2 = New Vector2(Core.windowSize.Width / 2.0F - 192 + 280, Core.windowSize.Height / 2.0F - 60 + Me.sellItemsConfirmationCursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Private Sub ButtonSellConfirmationSell()
        Dim tradeItem As TradeItem = Me.SellItemsList(Me.Scroll + Me.Cursor)

        Me.ChangeCurrencyAmount(tradeItem.SellPrice() * Me.SellItemsAmount)
        Core.Player.Inventory.RemoveItem(tradeItem.ItemID, Me.SellItemsAmount)
        Me.LoadSellItemsList()
        Me.SellItemsClampCursor()
        SoundManager.PlaySound("buy")

        If Me.SellItemsList.Count = 0 Then
            Me.MenuState = MenuStates.SellItemsCategory
            Me.LoadSellCategoryItems()
        Else
            Me.MenuState = MenuStates.SellItems
        End If
    End Sub

    Private Sub ButtonSellConfirmationCancel()
        Me.MenuState = MenuStates.SellItems
    End Sub

#End Region

    ''' <summary>
    ''' Draws a button on the screen.
    ''' </summary>
    ''' <param name="Position">The position of the button in absolute screen coordinates.</param>
    ''' <param name="Width">The width of the button in 64px steps. 5 is default.</param>
    ''' <param name="Text">The text to be displayed on the button.</param>
    ''' <param name="TextOffset">The offset on the text on the button. 16 is default.</param>
    Private Sub DrawButton(ByVal Position As Vector2, ByVal Width As Integer, ByVal Text As String, ByVal TextOffset As Integer, Optional ByVal Image As Texture2D = Nothing)
        Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Position.X), CInt(Position.Y), 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
        Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Position.X) + 64, CInt(Position.Y), 64 * Width, 64), New Rectangle(32, 16, 16, 16), Color.White)
        Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Position.X) + 64 * (Width + 1), CInt(Position.Y), 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

        If Image Is Nothing Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(TextOffset + CInt(Position.X), CInt(Position.Y) + 16), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        Else
            Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(4 + 16 + Image.Width + TextOffset + CInt(Position.X), CInt(Position.Y) + 16), Color.Black, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.Draw(Image, New Rectangle(CInt(Position.X) + TextOffset + 8, CInt(Position.Y) + 19, Image.Width, Image.Height), Color.White)
        End If
    End Sub

    Private Sub DrawBanner(ByVal Position As Vector2, ByVal Height As Integer, ByVal Text As String, ByVal Font As SpriteFont, Optional ByVal FixedWidth As Integer? = Nothing)
        Dim textWidth As Single = Font.MeasureString(Text).X
        Dim textHeight As Single = Font.MeasureString(Text).Y
        Dim textY As Single = (Height / 2.0F) - (textHeight / 2.0F)
        Dim Width As Integer = CInt((Height + 10) * 2 + textWidth)

        If FixedWidth.HasValue = True Then
            Width = FixedWidth.GetValueOrDefault()
        End If

        Canvas.DrawGradient(New Rectangle(CInt(Position.X), CInt(Position.Y), Height, Height), New Color(42, 167, 198, 0), New Color(42, 167, 198, 150), True, -1)
        Canvas.DrawRectangle(New Rectangle(CInt(Position.X) + Height, CInt(Position.Y), Width - (Height * 2), Height), New Color(42, 167, 198, 150))
        Canvas.DrawGradient(New Rectangle(CInt(Position.X) + (Width - Height), CInt(Position.Y), Height, Height), New Color(42, 167, 198, 150), New Color(42, 167, 198, 0), True, -1)

        Core.SpriteBatch.DrawString(Font, Text, New Vector2(CInt(Position.X + (Width / 2) - Font.MeasureString(Text).X / 2), Position.Y + textY), Color.White)
    End Sub

    Private Function GetItemTypeTexture(ByVal itemType As Items.ItemTypes) As Texture2D
        Dim i As Integer = 0
        Select Case itemType
            Case Items.ItemTypes.Standard
                i = 0
            Case Items.ItemTypes.Medicine
                i = 1
            Case Items.ItemTypes.Machines
                i = 2
            Case Items.ItemTypes.Pokéballs
                i = 3
            Case Items.ItemTypes.Plants
                i = 4
            Case Items.ItemTypes.Mail
                i = 5
            Case Items.ItemTypes.BattleItems
                i = 6
            Case Items.ItemTypes.KeyItems
                i = 7
        End Select

        Return TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\BagPack"), New Rectangle(i * 24, 150, 24, 24))
    End Function

    Private Function GetCurrencyAmount() As Integer
        Select Case Me.Currency
            Case Currencies.BattlePoints
                Return Core.Player.BP
            Case Currencies.Coins
                Return Core.Player.Coins
            Case Currencies.Pokédollar
                Return Core.Player.Money
        End Select
        Return 0
    End Function

    Private Function GetCurrencyDisplay() As String
        Select Case Me.Currency
            Case Currencies.BattlePoints
                Return GetCurrencyAmount().ToString() & " " & Localization.GetString("shop_screen_currency_BattlePoints", "Battle Points")
            Case Currencies.Coins
                Return GetCurrencyAmount().ToString() & " " & Localization.GetString("shop_screen_currency_Coins", "Coins")
            Case Currencies.Pokédollar
                Return GetCurrencyAmount().ToString() & " " & Localization.GetString("shop_screen_currency_Pokédollars", "$")
        End Select
        Return ""
    End Function

    Private Function GetCurrencyShort() As String
        Select Case Me.Currency
            Case Currencies.BattlePoints
                Return Localization.GetString("shop_screen_currency_short_BattlePoints", "BP")
            Case Currencies.Coins
                Return Localization.GetString("shop_screen_currency_short_Coins", "C")
            Case Currencies.Pokédollar
                Return Localization.GetString("shop_screen_currency_short_Pokédollars", "$")
        End Select
        Return ""
    End Function

    Private Sub ChangeCurrencyAmount(ByVal change As Integer)
        Select Case Me.Currency
            Case Currencies.BattlePoints
                Core.Player.BP = (Core.Player.BP + change).Clamp(0, Integer.MaxValue)
            Case Currencies.Coins
                Core.Player.Coins = (Core.Player.Coins + change).Clamp(0, Integer.MaxValue)
            Case Currencies.Pokédollar
                Core.Player.Money = (Core.Player.Money + change).Clamp(0, Integer.MaxValue)
        End Select
    End Sub

End Class