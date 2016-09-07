Namespace Items.Balls

    Public Class TimerBall

        Inherits Item

        Public Sub New()
            MyBase.New("Timer Ball", 1000, ItemTypes.Pokéballs, 150, 1, 0, New Rectangle(336, 216, 24, 24), "A somewhat different Pokéball that becomes progressively more effective the more turns that are taken in battle.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace