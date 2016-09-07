Namespace Items.Standard

    Public Class Magmarizer

        Inherits Item

        Public Sub New()
            MyBase.New("Magmarizer", 2100, ItemTypes.Standard, 100, 1, 0, New Rectangle(288, 192, 24, 24), "A box packed with a tremendous amount of magma energy. It is loved by a certain Pokémon.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 80
        End Sub

    End Class

End Namespace