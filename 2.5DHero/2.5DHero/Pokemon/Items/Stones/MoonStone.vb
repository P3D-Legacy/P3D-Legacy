Namespace Items.Stones

    Public Class MoonStone

        Inherits Items.StoneItem

        Public Sub New()
            MyBase.New("Moon Stone", 2100, ItemTypes.Standard, 8, 1, 10, New Rectangle(144, 0, 24, 24), "A peculiar stone that can make certain species of Pokémon evolve. It is as black as the night sky.")
        End Sub

    End Class

End Namespace