''' <summary>
''' Represents the player's inventory.
''' </summary>
Public Class PlayerInventory

    Inherits List(Of ItemContainer)

    ''' <summary>
    ''' A secure class to contain ItemID and Amount.
    ''' </summary>
    Class ItemContainer

        Private _itemID As Integer
        Private _amount As Integer

        Public Property ItemID() As Integer
            Get
                Return Me._itemID
            End Get
            Set(value As Integer)
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

        Public Sub New(ByVal ItemID As Integer, ByVal Amount As Integer)
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
                Return ChrW(128)
            Case Items.ItemTypes.BattleItems
                Return ChrW(135)
            Case Items.ItemTypes.KeyItems
                Return ChrW(129)
            Case Items.ItemTypes.Machines
                Return ChrW(130)
            Case Items.ItemTypes.Mail
                Return ChrW(131)
            Case Items.ItemTypes.Medicine
                Return ChrW(132)
            Case Items.ItemTypes.Plants
                Return ChrW(133)
            Case Items.ItemTypes.Pokéballs
                Return ChrW(134)
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' Adds items to the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item.</param>
    ''' <param name="Amount">Amount of items to add.</param>
    Public Sub AddItem(ByVal ID As Integer, ByVal Amount As Integer)
        Dim newItem As Item = net.Pokemon3D.Game.Item.GetItemByID(ID)

        For Each c As ItemContainer In Me
            If c.ItemID = ID Then
                c.Amount += Amount
                Exit Sub
            End If
        Next

        Me.Add(New ItemContainer(ID, Amount))
    End Sub

    ''' <summary>
    ''' Removes items from the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item to remove.</param>
    ''' <param name="Amount">The amount of items to remove.</param>
    Public Sub RemoveItem(ByVal ID As Integer, ByVal Amount As Integer)
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
        End If
    End Sub

    ''' <summary>
    ''' Removes all items of an ID from the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item.</param>
    Public Sub RemoveItem(ByVal ID As Integer)
        Dim Amount As Integer = Me.GetItemAmount(ID)
        If Amount > 0 Then
            Me.RemoveItem(ID, Amount)
        End If
    End Sub

    ''' <summary>
    ''' Returns the count of the item in the inventory.
    ''' </summary>
    ''' <param name="ID">The ID of the item to be counted.</param>
    Public Function GetItemAmount(ByVal ID As Integer) As Integer
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
                If Me.GetItemAmount(78) > 0 Then
                    Return True
                End If
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
        Dim Message As String = ""
        If Amount = 1 Then
            Message = Core.Player.Name & " stored it in the~" & Core.Player.Inventory.GetItemPocketChar(Item) & Item.ItemType.ToString() & " pocket."
        Else
            Message = Core.Player.Name & " stored them~in the " & Core.Player.Inventory.GetItemPocketChar(Item) & Item.ItemType.ToString() & " pocket."
        End If
        Return Message
    End Function

End Class