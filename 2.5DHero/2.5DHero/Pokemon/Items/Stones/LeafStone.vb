Namespace Items.Stones

    Public Class LeafStone

        Inherits Items.StoneItem

        Public Sub New()
            MyBase.New("Leaf Stone", 2100, ItemTypes.Standard, 34, 1, 0, New Rectangle(240, 24, 24, 24), "A peculiar stone that can make certain species of Pokémon evolve. It has an unmistakable leaf pattern.")
        End Sub

    End Class

End Namespace