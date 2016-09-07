Namespace Items.Standard

    Public Class SeaIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Sea Incense", 9800, ItemTypes.Standard, 264, 1, 0, New Rectangle(24, 264, 24, 24), "An item to be held by a Pokémon. This incense has a curious aroma that boosts the power of Water-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace