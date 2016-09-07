Namespace Items.Stones

    Public Class StickyRock

        Inherits Item

        Public Sub New()
            MyBase.New("Sticky Rock", 300, ItemTypes.Standard, 262, 1.0F, 0, New Rectangle(480, 240, 24, 24), "It's a stone that sticks to other stones, but it's just a regular rock and has no effect on Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace