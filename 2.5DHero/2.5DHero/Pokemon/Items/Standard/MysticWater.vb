Namespace Items.Standard

    <Item(95, "Mystic Water")>
    Public Class MysticWater

        Inherits Item

        Public Sub New()
            MyBase.New("Mystic Water", 100, ItemTypes.Standard, 95, 1, 0, New Rectangle(456, 72, 24, 24), "An item to be held by a Pokémon. This teardrop-shaped gem boosts the power of Water-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace
