Namespace Items.Standard

    Public Class RazorFang

        Inherits Item

        Public Sub New()
            MyBase.New("Razor Fang", 2100, ItemTypes.Standard, 183, 1, 1, New Rectangle(456, 144, 24, 24), "An item to be held by a Pokémon. It may make foes and allies flinch when the holder inflicts damage.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._battlePointsPrice = 48
        End Sub

    End Class

End Namespace