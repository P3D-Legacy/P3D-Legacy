Namespace Items.Standard

    <Item(137, "Dawn Stone")>
    Public Class DawnStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that makes certain species of Pokémon evolve. It sparkles like eyes."

        Public Sub New()
            _textureRectangle = New Rectangle(384, 192, 24, 24)
        End Sub

    End Class

End Namespace
