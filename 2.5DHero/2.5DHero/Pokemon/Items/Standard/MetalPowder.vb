Namespace Items.Standard

    Public Class MetalPowder

        Inherits Item

        Public Sub New()
            MyBase.New("Metal Powder", 10, ItemTypes.Standard, 35, 1, 1, New Rectangle(264, 24, 24, 24), "An item to be held by Ditto. Extremely fine yet hard, this odd powder boosts the Defense stat.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace