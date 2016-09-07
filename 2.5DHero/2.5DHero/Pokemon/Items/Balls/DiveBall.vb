Namespace Items.Balls

    Public Class DiveBall

        Inherits Item

        Public Sub New()
            MyBase.New("Dive Ball", 1000, ItemTypes.Pokéballs, 79, 1, 0, New Rectangle(288, 144, 24, 24), "A somewhat different Pokéball that works especially well when catching Pokémon that live underwater.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace