Namespace Items.Stones

    <Item(34, "Leaf Stone")>
    Public Class LeafStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that can make certain species of Pokémon evolve. It has an unmistakable leaf pattern."

        Public Sub New()
            _textureRectangle = New Rectangle(240, 24, 24, 24)
        End Sub

    End Class

End Namespace
