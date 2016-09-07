Namespace Items.Balls

    Public Class NestBall

        Inherits Item

        Public Sub New()
            MyBase.New("Nest Ball", 1000, ItemTypes.Pokéballs, 188, 1, 0, New Rectangle(24, 240, 24, 24), "A somewhat different Pokéball that becomes more effective the lower the level of the wild Pokémon.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace