Namespace Items.Balls

    Public Class GreatBall

        Inherits Item

        Public Sub New()
            MyBase.New("Great Ball", 600, ItemTypes.Pokéballs, 4, 1.5F, 1, New Rectangle(72, 0, 24, 24), "A good, high-performance Pokéball that provides a higher Pokémon catch rate than a standard Pokéball can.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace