Namespace Items.Standard

    Public Class BigNugget

        Inherits Item

        Public Sub New()
            MyBase.New("Big Nugget", 20000, ItemTypes.Standard, 189, 1, 1, New Rectangle(48, 240, 24, 24), "A big nugget of pure gold that gives off a lustrous gleam. It can be sold at a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace