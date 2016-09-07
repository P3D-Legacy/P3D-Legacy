Namespace Items.Standard

    Public Class Leftovers

        Inherits Item

        Public Sub New()
            MyBase.New("Leftovers", 200, ItemTypes.Standard, 146, 1, 1, New Rectangle(456, 120, 24, 24), "An item to be held by a Pokémon. The holder's HP is slowly but steadily restored throughout every battle.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10

            Me._battlePointsPrice = 64
        End Sub

    End Class

End Namespace