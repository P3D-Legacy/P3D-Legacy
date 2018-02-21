Namespace Items.Stones

    <Item(8, "Moon Stone")>
    Public Class MoonStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that can make certain species of Pokémon evolve. It is as black as the night sky."

        Public Sub New()
            _textureRectangle = New Rectangle(144, 0, 24, 24)
        End Sub

    End Class

End Namespace
