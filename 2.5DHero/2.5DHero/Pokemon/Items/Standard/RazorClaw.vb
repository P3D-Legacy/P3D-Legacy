Namespace Items.Standard

    Public Class RazorClaw

        Inherits Item

        Public Sub New()
            MyBase.New("Razor Claw", 2100, ItemTypes.Standard, 184, 1, 1, New Rectangle(480, 144, 24, 24), "An item to be held by a Pokémon. It is a sharply hooked claw that ups the holder's critical-hit ratio.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 80

            Me._battlePointsPrice = 48
        End Sub

    End Class

End Namespace