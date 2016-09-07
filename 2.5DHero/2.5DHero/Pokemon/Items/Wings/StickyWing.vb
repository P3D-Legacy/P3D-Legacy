Namespace Items.Wings

    Public Class StickyWing

        Inherits Item

        Public Sub New()
            MyBase.New("Sticky Wing", 200, ItemTypes.Standard, 261, 1.0F, 0, New Rectangle(456, 240, 24, 24), "It's a feather that sticks to other feathers, but it's just a regular feather and has no effect on Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 20
        End Sub

    End Class

End Namespace