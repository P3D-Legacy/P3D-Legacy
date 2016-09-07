Namespace Items.Standard

    Public Class SoftSand

        Inherits Item

        Public Sub New()
            MyBase.New("Soft Sand", 100, ItemTypes.Standard, 76, 1, 0, New Rectangle(144, 72, 24, 24), "An item to be held by a Pokémon. It is a loose, silky sand that boosts the power of Ground-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace