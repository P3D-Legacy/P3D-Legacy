Namespace Items.Standard

    Public Class OddIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Odd Incense", 9800, ItemTypes.Standard, 263, 1, 0, New Rectangle(0, 264, 24, 24), "An item to be held by a Pokémon. This exotic-smelling incense boosts the power of Psychic-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace