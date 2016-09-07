Namespace Items.Standard

    Public Class SilverPowder

        Inherits Item

        Public Sub New()
            MyBase.New("SilverPowder", 100, ItemTypes.Standard, 88, 1, 0, New Rectangle(312, 72, 24, 24), "An item to be held by a Pokémon. It is a shiny, silver powder that will boost the power of Bug-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace