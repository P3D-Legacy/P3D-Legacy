Namespace Items.Balls

    Public Class PremierBall

        Inherits Item

        Public Sub New()
            MyBase.New("Premier Ball", 200, ItemTypes.Pokéballs, 3, 1, 0, New Rectangle(216, 216, 24, 24), "A somewhat rare Pokéball that was made as a commemorative item used to celebrate an event of some sort.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace