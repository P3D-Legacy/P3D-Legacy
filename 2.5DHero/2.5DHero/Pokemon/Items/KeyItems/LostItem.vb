Namespace Items.KeyItems

    <Item(130, "Lost Item")>
    Public Class LostItem

        Inherits Item

        Public Sub New()
            MyBase.New("Lost Item", 100, ItemTypes.KeyItems, 130, 1, 1, New Rectangle(216, 120, 24, 24), "The Poké Doll lost by the Copycat.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
