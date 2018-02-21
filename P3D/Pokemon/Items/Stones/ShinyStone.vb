Namespace Items.Standard

    <Item(135, "Shiny Stone")>
    Public Class ShinyStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that makes certain species of Pokémon evolve. It shines with a dazzling light."

        Public Sub New()
            _textureRectangle = New Rectangle(336, 192, 24, 24)
        End Sub

    End Class

End Namespace
