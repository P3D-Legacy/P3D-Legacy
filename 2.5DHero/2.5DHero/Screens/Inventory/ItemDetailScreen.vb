Public Class ItemDetailScreen

    Inherits Screen

    Dim Item As Item
    Dim canUse As Boolean = True

    Dim index As Integer = 0
    Dim trashValue As Integer = 1
    Dim mainTexture As Texture2D

    Dim MenuItems As New List(Of String)

    Public Sub New(ByVal currentScreen As Screen, ByVal Item As Item, ByVal canUse As Boolean)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.ItemDetailScreen

        Me.Item = Item
        Me.canUse = canUse

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.ScreenSize.Width, Core.ScreenSize.Height), New Color(0, 0, 0, 150))

        DrawItem()
        If MenuItems.Count > 0 Then
            DrawMenu()
        End If

        TextBox.Draw()
    End Sub

    Private Sub DrawItem()
        Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 84, 64, 128, 128))

        Core.SpriteBatch.Draw(Item.Texture, New Rectangle(CInt(Core.windowSize.Width / 2) - 56, 96, 96, 96), Color.White)
    End Sub

    Private Sub DrawMenu()
        Dim CanvasTexture As Texture2D

        For i = 0 To MenuItems.Count - 1
            Dim Text As String = MenuItems(i)

            If i = index Then
                CanvasTexture = TextureManager.GetTexture(mainTexture, New Rectangle(0, 48, 48, 48))
            Else
                CanvasTexture = TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48))
            End If

            Dim offSetX As Integer = 0
            If MenuItems.Count > 3 Then
                If i < 2 Then
                    offSetX = -180
                Else
                    offSetX = 180
                End If
            End If
            Dim offSetY As Integer = i * 128
            If MenuItems.Count > 3 Then
                If i > 1 Then
                    offSetY -= 2 * 128
                End If
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 180 + offSetX, 240 + offSetY, 320, 64))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10 + offSetX, 276 + offSetY), Color.Black)

            If MenuItems(i) = Localization.GetString("item_detail_screen_trash") Then
                Dim trashText As String = trashValue & "/" & Core.Player.Inventory.GetItemAmount(Me.Item.ID)

                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) + 180 + offSetX, 240 + offSetY, 128, 64))
                Core.SpriteBatch.DrawString(FontManager.InGameFont, trashText, New Vector2(CInt(Core.windowSize.Width / 2) - (FontManager.InGameFont.MeasureString(trashText).X / 2) + 256 + offSetX, 276 + offSetY), Color.Black)
            End If
        Next
    End Sub

    Public Overrides Sub Update()
        TextBox.Update()

        If TextBox.Showing = False Then
            If Core.Player.Inventory.GetItemAmount(Me.Item.ID) = 0 Then
                Core.SetScreen(Me.PreScreen)
            End If

            If MenuItems.Count = 0 Then
                CreateMenuItems()
            End If

            If Controls.Down(True, True, False, True) = True Then
                If Controls.ShiftDown() = True Then
                    If MenuItems(index) = Localization.GetString("item_detail_screen_trash") Then
                        trashValue += 1
                    End If
                Else
                    index += 1
                End If
            End If
            If Controls.Up(True, True, False, True) = True Then
                If Controls.ShiftDown() = True Then
                    If MenuItems(index) = Localization.GetString("item_detail_screen_trash") Then
                        trashValue -= 1
                    End If
                Else
                    index -= 1
                End If
            End If

            Dim increment As Integer = 1
            If MenuItems.Count > 3 Then
                increment = 2
            End If
            If Controls.Right(True, True, False, True) = True Then
                index += increment
            End If
            If Controls.Left(True, True, False, True) = True Then
                index -= increment
            End If

            index = CInt(MathHelper.Clamp(index, 0, MenuItems.Count - 1))

            If MenuItems(index) = Localization.GetString("item_detail_screen_trash") Then
                If Controls.Right(True, False, True, False) = True Then
                    trashValue += 1
                End If
                If Controls.Left(True, False, True, False) = True Then
                    trashValue -= 1
                End If

                trashValue = CInt(MathHelper.Clamp(trashValue, 1, Core.Player.Inventory.GetItemAmount(Me.Item.ID)))
            End If

            If Controls.Accept() = True Then
                Select Case MenuItems(index)
                    Case Localization.GetString("item_detail_screen_use")
                        Item.Use()
                    Case Localization.GetString("item_detail_screen_give")
                        Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me.Item, AddressOf GiveItem, Localization.GetString("item_detail_screen_give_item") & Me.Item.Name, True))
                    Case Localization.GetString("item_detail_screen_trash")
                        Core.Player.Inventory.RemoveItem(Me.Item.ID, trashValue)
                    Case Localization.GetString("item_detail_screen_back")
                        Core.SetScreen(Me.PreScreen)
                End Select
            End If

            If Controls.Dismiss() = True Then
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub

    Private Sub GiveItem(ByVal PokeIndex As Integer)
        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.IsEgg() = False Then
            Core.Player.Inventory.RemoveItem(Me.Item.ID, 1)

            Dim reItem As Item = Nothing
            If Not Pokemon.Item Is Nothing Then
                reItem = Pokemon.Item
                Core.Player.Inventory.AddItem(reItem.ID, 1)
            End If

            Pokemon.Item = Me.Item

            TextBox.reDelay = 0.0F

            Dim t As String = Localization.GetString("pokemon_screen_give_item_1") & Item.Name & Localization.GetString("pokemon_screen_give_item_2") & Pokemon.GetDisplayName() & Localization.GetString("pokemon_screen_give_item_3")
            If Not reItem Is Nothing Then
                t &= Localization.GetString("pokemon_screen_give_item_4") & reItem.Name & Localization.GetString("pokemon_screen_give_item_5")
            Else
                t &= "."
            End If
            TextBox.Show(t, {})
        Else
            TextBox.Show("Eggs cannot hold items.")
        End If
    End Sub

    Private Sub CreateMenuItems()
        If Item.CanBeUsed = True And canUse = True Then
            MenuItems.Add(Localization.GetString("item_detail_screen_use"))
        End If
        If Item.CanBeHold = True Then
            MenuItems.Add(Localization.GetString("item_detail_screen_give"))
        End If
        If Item.ItemType <> Game.Items.ItemTypes.KeyItems And Item.CanBeTossed = True Then
            MenuItems.Add(Localization.GetString("item_detail_screen_trash"))
        End If
        MenuItems.Add(Localization.GetString("item_detail_screen_back"))
    End Sub
End Class