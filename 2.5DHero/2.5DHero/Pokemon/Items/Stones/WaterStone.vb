Namespace Items.Stones

    Public Class WaterStone

        Inherits Items.StoneItem

        Public Sub New()
            MyBase.New("Water Stone", 2100, ItemTypes.Standard, 24, 1, 0, New Rectangle(24, 24, 24, 24), "A peculiar stone that can make certain species of Pokémon evolve. It is the blue of a pool of clear water.")
        End Sub

    End Class

End Namespace