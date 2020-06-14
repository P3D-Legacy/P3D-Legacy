Namespace Items.Standard

    <Item(610, "Jaw Fossil")>
    Public Class JawFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived on the land. It looks as if it could be a piece of a large jaw."

        Public Sub New()
            _textureRectangle = New Rectangle(0, 48, 24, 24)
        End Sub

    End Class

End Namespace
