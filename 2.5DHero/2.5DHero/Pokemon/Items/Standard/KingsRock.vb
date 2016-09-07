Namespace Items.Standard

    Public Class KingsRock

        Inherits Item

        Public Sub New()
            MyBase.New("King's Rock", 100, ItemTypes.Standard, 82, 1, 0, New Rectangle(216, 72, 24, 24), "An item to be held by a Pokémon. When the holder successfully inflicts damage, the target may also flinch.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._battlePointsPrice = 64
        End Sub

    End Class

End Namespace