Namespace Items.Balls

    Public Class LoveBall

        Inherits Item

        Public Sub New()
            MyBase.New("Love Ball", 150, ItemTypes.Pokéballs, 166, 1, 8, New Rectangle(240, 144, 24, 24), "Pokéball for catching Pokémon that are the opposite gender of your Pokémon.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace