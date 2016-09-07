Namespace Items.Standard

    Public Class QuickPowder

        Inherits Item

        Public Sub New()
            MyBase.New("Quick Powder", 10, ItemTypes.Standard, 155, 1, 1, New Rectangle(96, 216, 24, 24), "An item to be held by Ditto. Extremely fine yet hard, this odd powder boosts the Speed stat.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace