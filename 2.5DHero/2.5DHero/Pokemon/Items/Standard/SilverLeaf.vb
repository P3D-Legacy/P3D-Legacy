Namespace Items.Standard

    Public Class SilverLeaf

        Inherits Item

        Public Sub New()
            MyBase.New("Silver Leaf", 1000, ItemTypes.Standard, 60, 1, 0, New Rectangle(288, 48, 24, 24), "A strange, silver-colored leaf.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace