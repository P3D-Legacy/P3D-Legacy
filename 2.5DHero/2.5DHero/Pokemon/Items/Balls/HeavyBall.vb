Namespace Items.Balls

    Public Class HeavyBall

        Inherits Item

        Public Sub New()
            MyBase.New("Heavy Ball", 150, ItemTypes.Pokéballs, 157, 1.0F, 1, New Rectangle(48, 144, 24, 24), "A Pokéball for catching very heavy Pokémon. ")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace