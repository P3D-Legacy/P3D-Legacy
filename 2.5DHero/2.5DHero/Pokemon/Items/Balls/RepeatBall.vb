Namespace Items.Balls

    Public Class RepeatBall

        Inherits Item

        Public Sub New()
            MyBase.New("Repeat Ball", 1000, ItemTypes.Pokéballs, 168, 1, 0, New Rectangle(384, 216, 24, 24), "A somewhat different Pokéball that works especially well on Pokémon species that have been caught before.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace