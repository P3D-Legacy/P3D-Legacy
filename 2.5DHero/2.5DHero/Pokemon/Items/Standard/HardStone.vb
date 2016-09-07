Namespace Items.Standard

    Public Class HardStone

        Inherits Item

        Public Sub New()
            MyBase.New("HardStone", 100, ItemTypes.Standard, 125, 1, 0, New Rectangle(96, 120, 24, 24), "An item to be held by a Pokémon. It is a durable stone that boosts the power of Rock-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 100
        End Sub

    End Class

End Namespace