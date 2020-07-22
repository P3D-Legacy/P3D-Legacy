Namespace Items.Standard

    <Item(605, "Claw Fossil")>
    Public Class ClawFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived in the sea. It appears to be a fragment of a claw."

        Public Sub New()
            _textureRectangle = New Rectangle(96, 0, 24, 24)
        End Sub

    End Class

End Namespace
