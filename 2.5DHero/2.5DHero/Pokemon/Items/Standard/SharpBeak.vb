Namespace Items.Standard

    Public Class SharpBeak

        Inherits Item

        Public Sub New()
            MyBase.New("Sharp Beak", 100, ItemTypes.Standard, 77, 1, 0, New Rectangle(168, 72, 24, 24), "An item to be held by a Pokémon. It's a long, sharp beak that boosts the power of Flying-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 50
        End Sub

    End Class

End Namespace