Public Class InventoryScreen

    Inherits Screen

    Dim index(8) As Integer
    Dim scrollIndex(8) As Integer
    Dim bagIndex As Integer = 0
    Dim bagIdentifier As Items.ItemTypes

    Dim Items As New Dictionary(Of Item, Integer)
    Dim cItems As New Dictionary(Of Item, Integer)

    Dim mainTexture As Texture2D
    Dim texture As Texture2D

    Public Delegate Sub DoStuff(ByVal ItemID As Integer)
    Dim ReturnItem As DoStuff
    Dim AllowedPages() As Integer

    Private Sub AchieveMailman()
        Dim hasAllMail As Boolean = True
        For i = 300 To 335
            If Core.Player.Inventory.GetItemAmount(i) <= 0 Then
                hasAllMail = False
            End If
        Next
        If hasAllMail = True Then
            GameJolt.Emblem.AchieveEmblem("mailman")
        End If
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal AllowedPages() As Integer, ByVal StartPageIndex As Integer, ByVal DoStuff As DoStuff)
        Me.Identification = Identifications.InventoryScreen
        Me.PreScreen = currentScreen
        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
        Me.texture = TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48))

        Me.ReturnItem = DoStuff
        Me.AllowedPages = AllowedPages

        For i = 0 To 7
            scrollIndex(i) = 0
        Next
        For i = 0 To 7
            index(i) = 0
        Next

        For Each c In Core.Player.Inventory
            Me.Items.Add(Item.GetItemByID(c.ItemID), c.Amount)
        Next

        ChangeBag()

        bagIndex = StartPageIndex
        index(bagIndex) = Player.Temp.BagSelectIndex
        If index(bagIndex) > cItems.Count - 1 Then
            index(bagIndex) = 0
        End If

        AchieveMailman()
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal AllowedPages() As Integer, ByVal DoStuff As DoStuff)
        Me.New(currentScreen, AllowedPages, Player.Temp.BagIndex, DoStuff)
    End Sub

    Public Sub New(ByVal currentScreen As Screen)
        Me.New(currentScreen, {}, Player.Temp.BagIndex, Nothing)
    End Sub

    Public Overrides Sub Update()
        If Controls.Down(True, True, True) Then
            If Controls.ShiftDown() = True Then
                Me.index(bagIndex) += 3
            Else
                Me.index(bagIndex) += 1
            End If
        End If
        If Controls.Up(True, True, True) Then
            If Controls.ShiftDown() = True Then
                Me.index(bagIndex) -= 3
            Else
                Me.index(bagIndex) -= 1
            End If
        End If
        If KeyBoardHandler.KeyPressed(Keys.End) = True Then
            Me.index(bagIndex) = cItems.Keys.Count - 1
        End If
        If KeyBoardHandler.KeyPressed(Keys.Home) = True Then
            Me.index(bagIndex) = 0
        End If

        If Controls.Right(True, True, False) = True Then
            bagIndex += 1
            If AllowedPages.Count > 0 And AllowedPages.Contains(bagIndex) = False Then
                While AllowedPages.Contains(bagIndex) = False
                    bagIndex += 1

                    If bagIndex < 0 Then
                        bagIndex = 7
                    ElseIf bagIndex > 7 Then
                        bagIndex = 0
                    End If
                End While
            End If
        End If
        If Controls.Left(True, True, False) = True Then
            bagIndex -= 1
            If AllowedPages.Count > 0 And AllowedPages.Contains(bagIndex) = False Then
                While AllowedPages.Contains(bagIndex) = False
                    bagIndex -= 1

                    If bagIndex < 0 Then
                        bagIndex = 7
                    ElseIf bagIndex > 7 Then
                        bagIndex = 0
                    End If
                End While
            End If
        End If

        If bagIndex < 0 Then
            bagIndex = 7
        ElseIf bagIndex > 7 Then
            bagIndex = 0
        End If

        If cItems.Keys.Count - 1 > 0 Then
            index(bagIndex) = CInt(MathHelper.Clamp(index(bagIndex), 0, cItems.Keys.Count - 1))
        Else
            index(bagIndex) = 0
        End If

        While Me.index(bagIndex) + scrollIndex(bagIndex) > 5
            Me.scrollIndex(bagIndex) -= 1
        End While
        While Me.index(bagIndex) + scrollIndex(bagIndex) < 0
            Me.scrollIndex(bagIndex) += 1
        End While

        Me.ChangeBag()

        If KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Or ControllerHandler.ButtonPressed(Buttons.Y) = True Then
            Me.SortItems()
        End If

        Player.Temp.BagIndex = Me.bagIndex
        Player.Temp.BagSelectIndex = Me.index(bagIndex)

        If Controls.Accept() = True Then
            Me.ChooseItem()
        End If

        If Controls.Dismiss() Then
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

    Private Sub ChooseItem()
        If cItems.Count > 0 Then
            If Not ReturnItem Is Nothing Then
                Core.SetScreen(Me.PreScreen)
                ReturnItem(cItems.Keys(index(bagIndex)).ID)
            Else
                Dim Item As Item = cItems.Keys(index(bagIndex))

                Core.SetScreen(New ItemDetailScreen(Me, Item, True))
            End If
        End If
    End Sub

    Private Sub ChangeBag()
        Select Case bagIndex
            Case 0
                bagIdentifier = Game.Items.ItemTypes.Standard
            Case 1
                bagIdentifier = Game.Items.ItemTypes.Medicine
            Case 2
                bagIdentifier = Game.Items.ItemTypes.Machines
            Case 3
                bagIdentifier = Game.Items.ItemTypes.Pokéballs
            Case 4
                bagIdentifier = Game.Items.ItemTypes.Plants
            Case 5
                bagIdentifier = Game.Items.ItemTypes.Mail
            Case 6
                bagIdentifier = Game.Items.ItemTypes.BattleItems
            Case 7
                bagIdentifier = Game.Items.ItemTypes.KeyItems
        End Select

        cItems.Clear()
        For i As Integer = 0 To Items.Keys.Count - 1
            If Items.Keys(i).ItemType = Me.bagIdentifier Then
                cItems.Add(Items.Keys(i), Items.Values(i))
            End If
        Next
    End Sub

    Private SortMode As String = "Name"
    Private sorted As Boolean = False

    Private Sub SortItems()
        Dim newItems As New Dictionary(Of Item, Integer)

        Select Case SortMode
            Case "Name"
                For Each k As KeyValuePair(Of Item, Integer) In (From i In Items Order By i.Key.Name Ascending)
                    newItems.Add(k.Key, k.Value)
                Next

                SortMode = "ID"
            Case "ID"
                For Each k As KeyValuePair(Of Item, Integer) In (From i In Items Order By i.Key.ID Ascending)
                    newItems.Add(k.Key, k.Value)
                Next

                SortMode = "SortValue"
            Case "SortValue"
                For Each k As KeyValuePair(Of Item, Integer) In (From i In Items Order By i.Key.SortValue Ascending)
                    newItems.Add(k.Key, k.Value)
                Next

                SortMode = "Name"
        End Select

        Items = newItems

        sorted = True

        ChangeBag()
    End Sub


    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawImageBorder(texture, 2, New Rectangle(60, 100, 800, 480))
        Canvas.DrawImageBorder(texture, 2, New Rectangle(572, 100, 288, 64))
        Canvas.DrawImageBorder(texture, 2, New Rectangle(60, 516, 480, 64))
        Canvas.DrawImageBorder(texture, 2, New Rectangle(572, 196, 288, 384))
        Canvas.DrawImageBorder(texture, 2, New Rectangle(620, 420, 192, 64))

        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\BagPack"), New Rectangle(592, 126, 48, 48), New Rectangle(24 * bagIndex, 150, 24, 24), Color.White)

        Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("inventory_menu_bag"), New Vector2(646, 134), Color.Black)
        Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("inventory_menu_backadvice"), New Vector2(1200 - FontManager.MiniFont.MeasureString(Localization.GetString("inventory_menu_backadvice")).X - 330, 580), Color.DarkGray)
        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("inventory_menu_items") & ":" & vbNewLine & Localization.GetString("item_category_" & Me.bagIdentifier.ToString()), New Vector2(640, 446), Color.Black)

        Canvas.DrawScrollBar(New Vector2(555, 120), cItems.Count, 6, scrollIndex(bagIndex), New Size(4, 390), False, TextureManager.GetTexture(mainTexture, New Rectangle(112, 12, 1, 1)), TextureManager.GetTexture(mainTexture, New Rectangle(113, 12, 1, 1)))

        For i As Integer = 0 To cItems.Keys.Count - 1
            Dim Item As Item = cItems.Keys(i)
            If i + scrollIndex(bagIndex) <= 5 And i + scrollIndex(bagIndex) >= 0 Then
                Dim p As New Vector2(100, 114)
                With Core.SpriteBatch

                    Dim BorderTexture As Texture2D
                    If i = index(bagIndex) Then
                        BorderTexture = TextureManager.GetTexture(mainTexture, New Rectangle(48, 0, 48, 48))
                    Else
                        BorderTexture = texture
                    End If

                    Canvas.DrawImageBorder(BorderTexture, 1, New Rectangle(CInt(p.X), CInt(p.Y + (i + scrollIndex(bagIndex)) * 70), 320, 32))

                    .Draw(Item.Texture, New Rectangle(CInt(p.X), CInt(p.Y + (i + scrollIndex(bagIndex)) * 70), 64, 64), Color.White)
                    .DrawString(FontManager.MiniFont, Item.Name, New Vector2(CInt(p.X + 74), CInt(p.Y + (i + scrollIndex(bagIndex)) * 70) + 13), Color.Black)

                    If Me.bagIndex <> 7 Then
                        Dim lenght As String = ""
                        If cItems.Values(i).ToString().Length < 3 Then
                            For n = 1 To 3 - cItems.Values(i).ToString().Length
                                lenght &= " "
                            Next
                        End If
                        .DrawString(FontManager.MiniFont, "x" & lenght & cItems.Values(i).ToString(), New Vector2(CInt(p.X + 280), CInt(p.Y + (i + scrollIndex(bagIndex)) * 70) + 13), Color.Black)
                    End If

                    If i = index(bagIndex) Then
                        .DrawString(FontManager.MiniFont, Item.Description.CropStringToWidth(FontManager.MiniFont, 450), New Vector2(80, 534), Color.Black)
                    End If
                End With
            End If
        Next

        Dim x As Integer = bagIndex * 58
        Dim y As Integer = 0
        If bagIndex > 3 Then
            y = 58
            x = (bagIndex - 4) * 58
        End If

        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\BagPack"), New Rectangle(646, 220, 174, 174), New Rectangle(x, y, 58, 58), Color.White)

        If sorted = True Then
            Dim displayMode As String = "Name"
            Select Case SortMode
                Case "Name"
                    displayMode = "SortValue"
                Case "ID"
                    displayMode = "Name"
                Case "SortValue"
                    displayMode = "ID"
            End Select

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Sortmode: """ & displayMode & """", New Vector2(638, 522), Color.Gray)
        End If

        Canvas.DrawScrollBar(New Vector2(630, 405), 8, 1, bagIndex, New Size(200, 4), True, TextureManager.GetTexture(mainTexture, New Rectangle(112, 12, 1, 1)), TextureManager.GetTexture(mainTexture, New Rectangle(113, 12, 1, 1)))
    End Sub

    Public Overrides Sub ChangeTo()
        Me.Items.Clear()
        For Each c In Core.Player.Inventory
            Me.Items.Add(Item.GetItemByID(c.ItemID), c.Amount)
        Next

        ChangeBag()
    End Sub

End Class