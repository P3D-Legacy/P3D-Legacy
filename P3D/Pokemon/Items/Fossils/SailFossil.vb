Namespace Items.Standard

    <Item(611, "Sail Fossil")>
    Public Class SailFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived on land. It looks like the impression from a skin sail."

        Public Sub New()
            _textureRectangle = New Rectangle(24, 48, 24, 24)
        End Sub

    End Class

End Namespace
