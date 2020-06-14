Namespace Items.Standard

    <Item(608, "Cover Fossil")>
    Public Class CoverFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived in the sea. It appears as though it could be part of its back."

        Public Sub New()
            _textureRectangle = New Rectangle(72, 24, 24, 24)
        End Sub

    End Class

End Namespace
