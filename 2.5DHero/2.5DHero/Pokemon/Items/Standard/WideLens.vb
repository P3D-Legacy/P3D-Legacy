Namespace Items.Standard

    Public Class WideLens

        Inherits Item

        Public Sub New()
            MyBase.New("Wide Lens", 200, ItemTypes.Standard, 171, 1, 1, New Rectangle(144, 216, 24, 24), "An item to be held by a Pokémon. It's a magnifying lens that slightly boosts the accuracy of moves.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace