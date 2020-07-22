Namespace Items.Standard

    <Item(604, "Root Fossil")>
    Public Class RootFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived in the sea. It looks as if it could be part of a plant's root."

        Public Sub New()
            _textureRectangle = New Rectangle(72, 0, 24, 24)
        End Sub

    End Class

End Namespace
