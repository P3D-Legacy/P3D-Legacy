Namespace Items.Standard

    Public Class BlackBelt

        Inherits Item

        Public Sub New()
            MyBase.New("Black Belt", 100, ItemTypes.Standard, 98, 1, 0, New Rectangle(24, 96, 24, 24), "An item to be held by a Pokémon. This belt helps the wearer to focus and boosts the power of Fighting-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace