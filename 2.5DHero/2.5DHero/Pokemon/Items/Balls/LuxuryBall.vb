Namespace Items.Balls

    Public Class LuxuryBall

        Inherits Item

        Public Sub New()
            MyBase.New("Luxury Ball", 1000, ItemTypes.Pokéballs, 174, 1, 0, New Rectangle(432, 216, 24, 24), "A particularly comfortable Pokéball that makes a wild Pokémon quickly grow friendlier after being caught.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace