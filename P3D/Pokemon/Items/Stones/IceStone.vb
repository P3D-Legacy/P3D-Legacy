Namespace Items.Stones

    <Item(593, "Ice Stone")>
    Public Class IceStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that can make certain species of Pokémon evolve. It has an unmistakable snowflake pattern."

        Public Sub New()
            _textureRectangle = New Rectangle(144, 312, 24, 24)
        End Sub

    End Class

End Namespace
