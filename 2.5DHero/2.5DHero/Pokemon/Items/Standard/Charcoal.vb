Namespace Items.Standard

    Public Class Charcoal

        Inherits Item

        Public Sub New()
            MyBase.New("Charcoal", 4900, ItemTypes.Standard, 138, 1, 0, New Rectangle(336, 120, 24, 24), "An item to be held by a Pokémon. It is a combustible fuel that boosts the power of Fire-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace