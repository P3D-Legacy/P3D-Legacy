Namespace Items.Standard

    Public Class QuickClaw

        Inherits Item

        Public Sub New()
            MyBase.New("Quick Claw", 100, ItemTypes.Standard, 73, 1, 0, New Rectangle(96, 72, 24, 24), "An item to be held by a Pokémon. This light, sharp claw lets the bearer move first occasionally.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 80

            Me._battlePointsPrice = 64
        End Sub

    End Class

End Namespace