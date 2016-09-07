Namespace Items.Standard

    Public Class GripClaw

        Inherits Item

        Public Sub New()
            MyBase.New("Grip Claw", 200, ItemTypes.Standard, 176, 1, 1, New Rectangle(168, 216, 24, 24), "A Pokémon hold item that extends the duration of multiturn attacks like Bind and Wrap.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 90
        End Sub

    End Class

End Namespace