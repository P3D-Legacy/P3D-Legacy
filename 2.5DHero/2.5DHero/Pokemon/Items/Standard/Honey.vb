Namespace Items.Plants

    Public Class Honey

        Inherits Item

        Public Sub New()
            MyBase.New("Honey", 100, ItemTypes.Plants, 253, 1, 0, New Rectangle(264, 240, 24, 24), "Honey produced by a Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace