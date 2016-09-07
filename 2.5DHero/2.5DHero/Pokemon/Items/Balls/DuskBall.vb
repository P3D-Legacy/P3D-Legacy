Namespace Items.Balls

    Public Class DuskBall

        Inherits Item

        Public Sub New()
            MyBase.New("Dusk Ball", 1000, ItemTypes.Pokéballs, 158, 1, 0, New Rectangle(360, 216, 24, 24), "A somewhat different Pokéball that makes it easier to catch wild Pokémon at night or in dark places like caves.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace