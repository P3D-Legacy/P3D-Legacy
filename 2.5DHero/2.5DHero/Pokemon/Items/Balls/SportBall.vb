Namespace Items.Balls

    Public Class SportBall

        Inherits Item

        Public Sub New()
            MyBase.New("Sport Ball", 1000, ItemTypes.Pokéballs, 177, 1.5F, 0, New Rectangle(384, 144, 24, 24), "A special Pokéball for the Bug-Catching Contest.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace