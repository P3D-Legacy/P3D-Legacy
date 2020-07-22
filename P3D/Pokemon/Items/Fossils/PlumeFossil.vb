Namespace Items.Standard

    <Item(609, "Plume Fossil")>
    Public Class PlumeFossil

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A fossil from a prehistoric Pokémon that once lived in the sky. It looks as if it could come from part of its wing."

        Public Sub New()
            _textureRectangle = New Rectangle(96, 24, 24, 24)
        End Sub

    End Class

End Namespace
