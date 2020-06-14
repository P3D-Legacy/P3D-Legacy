Namespace Items.Standard

    <Item(601, "Helix Fossil")>
    Public Class HelixFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived in the sea. It might be a piece of a seashell."

        Public Sub New()
            _textureRectangle = New Rectangle(0, 0, 24, 24)
        End Sub

    End Class

End Namespace
