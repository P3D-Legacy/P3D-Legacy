Namespace Items.Standard

    Public Class NeverMeltIce

        Inherits Item

        Public Sub New()
            MyBase.New("Never-Melt Ice", 100, ItemTypes.Standard, 107, 1, 0, New Rectangle(216, 96, 24, 24), "An item to be held by a Pokémon. It's a piece of ice that repels heat effects and boosts Ice-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace