Namespace Items.Standard

    Public Class Nugget

        Inherits Item

        Public Sub New()
            MyBase.New("Nugget", 10000, ItemTypes.Standard, 36, 1, 0, New Rectangle(288, 24, 24, 24), "A nugget of the purest gold that gives off a lustrous gleam in direct light. It can be sold at a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace