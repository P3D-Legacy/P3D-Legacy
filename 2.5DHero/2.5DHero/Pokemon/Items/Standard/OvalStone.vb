Namespace Items.Standard

    Public Class OvalStone

        Inherits Item

        Public Sub New()
            MyBase.New("Oval Stone", 2100, ItemTypes.Standard, 179, 1, 1, New Rectangle(192, 216, 24, 24), "A peculiar stone that makes certain species of Pokémon evolve. It is shaped like an egg.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 80
        End Sub

    End Class

End Namespace