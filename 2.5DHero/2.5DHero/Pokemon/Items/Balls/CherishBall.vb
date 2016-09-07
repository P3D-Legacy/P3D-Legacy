Namespace Items.Balls

    Public Class CherishBall

        Inherits Item

        Public Sub New()
            MyBase.New("Cherish Ball", 1000, ItemTypes.Pokéballs, 45, 1, 0, New Rectangle(216, 192, 24, 24), "A quite rare Pokéball that has been specially crafted to commemorate an occasion of some sort.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace