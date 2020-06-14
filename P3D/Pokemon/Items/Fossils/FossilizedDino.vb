Namespace Items.Standard

    <Item(615, "Fossilized Dino")>
    Public Class FossilizedDino

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "The fossil of an ancient Pokémon that once lived in the sea. What it looked like is a mystery."

        Public Sub New()
            _textureRectangle = New Rectangle(24, 72, 24, 24)
        End Sub

    End Class

End Namespace
