Namespace Items.Standard

    <Item(653, "Black Augurite")>
    Public Class BlackAugurite

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A glassy black stone that produces a sharp cutting edge when split. It’s loved by a certain Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(336, 192, 24, 24)
        End Sub

    End Class

End Namespace
