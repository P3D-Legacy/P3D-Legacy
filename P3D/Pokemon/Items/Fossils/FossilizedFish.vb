Namespace Items.Standard

    <Item(613, "Fossilized Fish")>
    Public Class FossilizedFish

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "The fossil of an ancient Pokémon that once lived in the sea. What it looked like is a mystery."

        Public Sub New()
            _textureRectangle = New Rectangle(48, 72, 24, 24)
        End Sub

    End Class

End Namespace
