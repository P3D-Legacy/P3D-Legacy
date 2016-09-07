Namespace Items.Balls

    Public Class UltraBall

        Inherits Item

        Public Sub New()
            MyBase.New("Ultra Ball", 1200, ItemTypes.Pokéballs, 2, 2, 2, New Rectangle(24, 0, 24, 24), "An ultra-high performance Pokéball that provides a higher success rate for catching Pokémon than a Great Ball.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace