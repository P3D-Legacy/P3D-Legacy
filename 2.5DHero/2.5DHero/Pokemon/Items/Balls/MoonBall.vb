Namespace Items.Balls

    Public Class MoonBall

        Inherits Item

        Public Sub New()
            MyBase.New("Moon Ball", 150, ItemTypes.Pokéballs, 165, 1, 6, New Rectangle(216, 144, 24, 24), "A Pokéball for catching Pokémon that evolve using the Moon Stone. ")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace