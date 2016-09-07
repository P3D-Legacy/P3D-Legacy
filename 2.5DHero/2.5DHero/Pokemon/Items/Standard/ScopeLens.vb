Namespace Items.Standard

    Public Class ScopeLens

        Inherits Item

        Public Sub New()
            MyBase.New("Scope Lens", 200, ItemTypes.Standard, 140, 1, 1, New Rectangle(384, 120, 24, 24), "An item to be held by a Pokémon. It is a lens that boosts the holder's critical-hit ratio.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._battlePointsPrice = 64
        End Sub

    End Class

End Namespace