Namespace Items.Balls

    Public Class NetBall

        Inherits Item

        Public Sub New()
            MyBase.New("Net Ball", 1000, ItemTypes.Pokéballs, 80, 1, 0, New Rectangle(48, 168, 24, 24), "A somewhat different Pokéball that is more effective when attempting to catch Water- or Bug-type Pokémon.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace