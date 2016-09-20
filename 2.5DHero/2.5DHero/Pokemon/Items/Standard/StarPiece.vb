Namespace Items.Standard

    <Item(132, "Star Piece")>
    Public Class StarPiece

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 120, 24, 24)
        End Sub

    End Class

End Namespace
