Namespace Items.Standard

    Public Class BigPearl

        Inherits Item

        Public Sub New()
            MyBase.New("Big Pearl", 7500, ItemTypes.Standard, 111, 1, 0, New Rectangle(288, 96, 24, 24), "A rather large pearl that has a very nice silvery sheen. It can be sold to shops for a high price.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace