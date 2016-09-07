Namespace Items.Standard

    Public Class LaggingTail

        Inherits Item

        Public Sub New()
            MyBase.New("Lagging Tail", 200, ItemTypes.Standard, 142, 1, 0, New Rectangle(432, 192, 24, 24), "An item to be held by a Pokémon. It is tremendously heavy and makes the holder move slower than usual.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace