Namespace Items.Balls

    Public Class LevelBall

        Inherits Item

        Public Sub New()
            MyBase.New("Level Ball", 150, ItemTypes.Pokéballs, 159, 1, 4, New Rectangle(96, 144, 24, 24), "A Pokéball for catching Pokémon that are a lower level than your own. ")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace