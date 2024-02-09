''' <summary>
''' Represents the player's inventory.
''' </summary>
Public Class PlayerInventory

    Inherits List(Of ItemContainer)

    ''' <summary>
    ''' A secure class to contain ItemID and Amount.
    ''' </summary>
    Class ItemContainer

        Private _itemID As String
        Private _amount As Integer

        Public Property ItemID() As String
            Get
                Return Me._itemID
            End Get
            Set(value As String)
                Me._itemID = value
            End Set
        End Property

        Public Property Amount() As Integer
            Get
                Return Me._amount
            End Get
            Set(value As Integer)
                Me._amount = value
            End Set
        End Property

        Public Sub New(ByVal ItemID As String, ByVal Amount As Integer)
            Me.ItemID = ItemID
            Me.Amount = Amount
        End Sub

    End Class

    ''' <summary>
    ''' Returns a character that represents the item's pocket icon.
    ''' </summary>
    Public Function GetItemPocketChar(ByVal Item As Item) As String
        Select Case Item.ItemType
            Case Items.ItemTypes.Standard
                Return "€"
            Case Items.ItemTypes.KeyItems
                Return "№"
            Case Items.ItemTypes.Machines
                Return "™"
            Case Items.ItemTypes.Mail
                Return "←"
            Case Items.ItemTypes.Medicine
                Return "↑"
            Case Items.ItemTypes.Plants
                Return "→"
            Case Items.ItemTypes.Pokéballs
                Return "↓"
            Case Items.ItemTypes.BattleItems
                Return "↔"
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' Adds items to the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item.</param>
    ''' <param name="Amount">Amount of items to add.</param>
    Public Sub AddItem(ByVal ID As String, ByVal Amount As Integer)
        For Each c As ItemContainer In Me
            If c.ItemID = ID Then
                c.Amount += Amount
                Exit Sub
            End If
        Next

        Me.Add(New ItemContainer(ID, Amount))

        Dim item As Item = Item.GetItemByID(ID)
        If item.IsMail = True AndAlso item.IsGameModeItem = False Then
            Dim hasAllMail = True
            For m = 300 To 323
                If Core.Player.Inventory.GetItemAmount(m.ToString) = 0 Then
                    hasAllMail = False
                    Exit For
                End If
            Next
            If hasAllMail = True Then
                GameJolt.Emblem.AchieveEmblem("mailman")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Removes items from the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item to remove.</param>
    ''' <param name="Amount">The amount of items to remove.</param>
    Public Sub RemoveItem(ByVal ID As String, ByVal Amount As Integer)
        If Amount > 0 Then
            For Each c As ItemContainer In Me
                If c.ItemID = ID Then
                    If c.Amount - Amount <= 0 Then
                        Me.Remove(c)
                        Exit Sub
                    Else
                        c.Amount -= Amount
                    End If
                End If
            Next
        Else
            For Each c As ItemContainer In Me
                If c.ItemID = ID Then
                    Me.Remove(c)
                    Exit Sub
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Removes all items of an ID from the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item.</param>
    Public Sub RemoveItem(ByVal ID As String)
        Dim Amount As Integer = Me.GetItemAmount(ID)
        If Amount > 0 Then
            Me.RemoveItem(ID, Amount)
        End If
    End Sub

    ''' <summary>
    ''' Returns the count of the item in the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item to be counted.</param>
    Public Function GetItemAmount(ByVal ID As String) As Integer
        For Each c As ItemContainer In Me
            If c.ItemID = ID Then
                Return c.Amount
            End If
        Next

        Return 0
    End Function

    ''' <summary>
    ''' If the player has the Running Shoes in their inventory.
    ''' </summary>
    Public ReadOnly Property HasRunningShoes() As Boolean
        Get
            If Core.Player.SandBoxMode = True Or GameController.IS_DEBUG_ACTIVE = True Then
                Return True
            Else
                If Me.GetItemAmount(78.ToString) > 0 Then
                    Return True
                End If
            End If

            Return False
        End Get
    End Property

    ''' <summary>
    ''' If the player has the Mega Bracelet in their inventory.
    ''' </summary>
    Public ReadOnly Property HasMegaBracelet() As Boolean
        Get
            If Me.GetItemAmount(576.ToString) > 0 Then
                Return True
            End If

            Return False
        End Get
    End Property

    ''' <summary>
    ''' Returns a message that displays the event of putting an item into the inventory.
    ''' </summary>
    ''' <param name="Item">The Item to store in the inventory.</param>
    ''' <param name="Amount">The amount.</param>
    Public Function GetMessageReceive(ByVal Item As Item, ByVal Amount As Integer) As String
        Dim Message As String
        Dim SpaceAfterStart As String = ""
        If Amount = 1 Then
            If Localization.GetString("item_stored_in_pocket_single_start", "<player.name> stored it in the~").Replace("<player.name>", Core.Player.Name).EndsWith("~") = False Then
                SpaceAfterStart = " "
            End If
        Else
            If Localization.GetString("item_stored_in_pocket_multiple_start", "<player.name> stored them~in the").Replace("<player.name>", Core.Player.Name).EndsWith("~") = False Then
                SpaceAfterStart = " "
            End If
        End If
        If Amount = 1 Then
            Message = Localization.GetString("item_stored_in_pocket_single_start", "<player.name> stored it in the~").Replace("<player.name>", Core.Player.Name) & SpaceAfterStart & Core.Player.Inventory.GetItemPocketChar(Item) & Localization.GetString("item_category_" & Item.ItemType.ToString(), Item.ItemType.ToString()) & " " & Localization.GetString("item_stored_in_pocket_single_end", "pocket.").Replace("<player.name>", Core.Player.Name)
        Else
            Message = Localization.GetString("item_stored_in_pocket_multiple_start", "<player.name> stored them~in the").Replace("<player.name>", Core.Player.Name) & SpaceAfterStart & Core.Player.Inventory.GetItemPocketChar(Item) & Localization.GetString("item_category_" & Item.ItemType.ToString(), Item.ItemType.ToString()) & " " & Localization.GetString("item_stored_in_pocket_multiple_end", "pocket.").Replace("<player.name>", Core.Player.Name)
        End If
        Return Message
    End Function

End Class