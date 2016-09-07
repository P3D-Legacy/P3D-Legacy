Namespace Items.Standard

    Public Class StickyBarb

        Inherits Item

        Public Sub New()
            MyBase.New("Sticky Barb", 200, ItemTypes.Standard, 70, 1, 0, New Rectangle(24, 168, 24, 24), "An item to be held by a Pokémon. It damages the holder every turn and may latch on to Pokémon that touch the holder.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 80
        End Sub

    End Class

End Namespace