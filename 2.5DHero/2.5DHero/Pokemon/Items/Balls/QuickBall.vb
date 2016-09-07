Namespace Items.Balls

    Public Class QuickBall

        Inherits Item

        Public Sub New()
            MyBase.New("Quick Ball", 1000, ItemTypes.Pokéballs, 129, 1, 0, New Rectangle(120, 168, 24, 24), "A somewhat different Pokéball that has a more successful catch rate if used at the start of a wild encounter.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace