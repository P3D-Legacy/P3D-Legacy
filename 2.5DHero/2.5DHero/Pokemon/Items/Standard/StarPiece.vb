Namespace Items.Standard

    <Item(132, "Star Piece")>
    Public Class StarPiece

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A small shard of a beautiful gem that demonstrates a distinctly red sparkle. It can be sold at a high price to shops."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 120, 24, 24)
        End Sub

    End Class

End Namespace
