Namespace Items.Standard

    Public Class ZoomLens

        Inherits Item

        Public Sub New()
            MyBase.New("Zoom Lens", 200, ItemTypes.Standard, 74, 1, 1, New Rectangle(264, 144, 24, 24), "An item to be held by a Pokémon. If the holder moves after its target moves, its accuracy will be boosted.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace