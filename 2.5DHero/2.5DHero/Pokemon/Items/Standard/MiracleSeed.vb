Namespace Items.Standard

    Public Class MiracleSeed

        Inherits Item

        Public Sub New()
            MyBase.New("Miracle Seed", 100, ItemTypes.Standard, 117, 1, 0, New Rectangle(432, 96, 24, 24), "An item to be held by a Pokémon. It is a seed imbued with life force that boosts the power of Grass-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace