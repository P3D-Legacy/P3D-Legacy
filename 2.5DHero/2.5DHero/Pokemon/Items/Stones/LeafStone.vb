Namespace Items.Stones

    <Item(34, "Leaf Stone")>
    Public Class LeafStone

        Inherits StoneItem

        Public Sub New()
            MyBase.New("Leaf Stone", 2100, ItemTypes.Standard, 34, 1, 0, New Rectangle(240, 24, 24, 24), "A peculiar stone that can make certain species of Pokémon evolve. It has an unmistakable leaf pattern.")
        End Sub

    End Class

End Namespace
