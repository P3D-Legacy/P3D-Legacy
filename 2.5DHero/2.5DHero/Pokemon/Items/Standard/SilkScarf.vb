Namespace Items.Standard

    Public Class SilkScarf

        Inherits Item

        Public Sub New()
            MyBase.New("Silk Scarf", 100, ItemTypes.Standard, 90, 1, 0, New Rectangle(264, 192, 24, 24), "An item to be held by a Pokémon. It's a sumptuous scarf that boosts the power of Normal-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace