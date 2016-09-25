Namespace Items.Standard

    <Item(180, "Brick Piece")>
    Public Class BrickPiece

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A rare chunk of brick."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(72, 240, 24, 24)
        End Sub

    End Class

End Namespace
