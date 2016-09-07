Namespace Items.Standard

    Public Class FocusBand

        Inherits Item

        Public Sub New()
            MyBase.New("Focus Band", 200, ItemTypes.Standard, 119, 1, 1, New Rectangle(480, 96, 24, 24), "An item to be held by a Pokémon. The holder may endure a potential KO attack, leaving it with just 1 HP.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10

            Me._battlePointsPrice = 64
        End Sub

    End Class

End Namespace