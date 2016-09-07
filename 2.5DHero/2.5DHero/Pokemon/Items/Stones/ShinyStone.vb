Namespace Items.Standard

    Public Class ShinyStone

        Inherits Items.StoneItem

        Public Sub New()
            MyBase.New("Shiny Stone", 2100, ItemTypes.Standard, 135, 1, 0, New Rectangle(336, 192, 24, 24), "A peculiar stone that makes certain species of Pokémon evolve. It shines with a dazzling light.")
        End Sub

    End Class

End Namespace