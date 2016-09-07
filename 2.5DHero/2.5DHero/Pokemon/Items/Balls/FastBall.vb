Namespace Items.Balls

    Public Class FastBall

        Inherits Item

        Public Sub New()
            MyBase.New("Fast Ball", 150, ItemTypes.Pokéballs, 161, 1, 10, New Rectangle(144, 144, 24, 24), "A Pokéball that makes it easier to catch Pokémon which are quick to run away.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace