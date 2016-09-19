Namespace Items.KeyItems

    <Item(6, "Bicycle")>
    Public Class Bicycle

        Inherits Item

        Public Sub New()
            MyBase.New("Bicycle", 9800, ItemTypes.KeyItems, 6, 1, 0, New Rectangle(120, 0, 24, 24), "A folding Bicycle that enables much faster movement than the Running Shoes.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace
