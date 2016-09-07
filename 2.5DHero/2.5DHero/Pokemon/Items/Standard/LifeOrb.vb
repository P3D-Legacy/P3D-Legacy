Namespace Items.Standard

    Public Class LifeOrb

        Inherits Item

        Public Sub New()
            MyBase.New("Life Orb", 200, ItemTypes.Standard, 506, 1, 1, New Rectangle(240, 240, 24, 24), "An item to be held by a Pokémon. It boosts the power of moves, but at the cost of some HP on each hit.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace