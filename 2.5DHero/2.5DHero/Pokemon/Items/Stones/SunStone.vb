Namespace Items.Stones

    <Item(169, "Sun Stone")>
    Public Class SunStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that can make certain species of Pokémon evolve. It burns as red as the evening sun."

        Public Sub New()
            _textureRectangle = New Rectangle(312, 144, 24, 24)
        End Sub

    End Class

End Namespace
