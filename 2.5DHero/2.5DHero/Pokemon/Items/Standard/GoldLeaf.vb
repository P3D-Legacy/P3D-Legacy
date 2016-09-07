Namespace Items.Standard

    Public Class GoldLeaf

        Inherits Item

        Public Sub New()
            MyBase.New("Gold Leaf", 2000, ItemTypes.Standard, 75, 1, 0, New Rectangle(120, 72, 24, 24), "A strange, gold-colored leaf.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace