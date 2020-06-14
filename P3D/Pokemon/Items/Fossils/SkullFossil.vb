Namespace Items.Standard

    <Item(606, "Skull Fossil")>
    Public Class SkullFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived on the land. It appears as though it's part of a head."

        Public Sub New()
            _textureRectangle = New Rectangle(0, 24, 24, 24)
        End Sub

    End Class

End Namespace
