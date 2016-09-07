Namespace Items.Standard

    Public Class Electirizer

        Inherits Item

        Public Sub New()
            MyBase.New("Electirizer", 2100, ItemTypes.Standard, 120, 1, 0, New Rectangle(312, 192, 24, 24), "A box packed with a tremendous amount of electric energy. It is loved by a certain Pokémon.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 80
        End Sub

    End Class

End Namespace