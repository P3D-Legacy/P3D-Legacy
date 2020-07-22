Namespace Items.Standard

    <Item(612, "Fossilized Bird")>
    Public Class FossilizedBird

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "The fossil of an ancient Pokémon that once soared through the sky. What it looked like is a mystery."

        Public Sub New()
            _textureRectangle = New Rectangle(0, 72, 24, 24)
        End Sub

    End Class

End Namespace
