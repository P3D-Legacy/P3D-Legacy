Namespace Items.Standard

    <Item(602, "Dome Fossil")>
    Public Class DomeFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived in the sea. It could be a shell or carapace."

        Public Sub New()
            _textureRectangle = New Rectangle(24, 0, 24, 24)
        End Sub

    End Class

End Namespace
