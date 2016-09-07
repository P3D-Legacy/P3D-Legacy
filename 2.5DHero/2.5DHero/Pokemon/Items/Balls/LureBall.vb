Namespace Items.Balls

    Public Class LureBall

        Inherits Item

        Public Sub New()
            MyBase.New("Lure Ball", 150, ItemTypes.Pokéballs, 160, 1, 5, New Rectangle(120, 144, 24, 24), "A Pokéball for catching Pokémon hooked by a Rod when fishing. ")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace