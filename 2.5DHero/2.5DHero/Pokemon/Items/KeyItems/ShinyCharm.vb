Namespace Items.KeyItems

    Public Class ShinyCharm

        Inherits Item

        Public Sub New()
            MyBase.New("Shiny Charm", 9800, ItemTypes.KeyItems, 242, 1, 0, New Rectangle(120, 264, 24, 24), "A shiny charm said to increase the chance of finding a Shiny Pokémon in the wild.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace