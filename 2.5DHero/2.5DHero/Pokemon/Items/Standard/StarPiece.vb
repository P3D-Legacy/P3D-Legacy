Namespace Items.Standard

    Public Class StarPiece

        Inherits Item

        Public Sub New()
            MyBase.New("Star Piece", 9800, ItemTypes.Standard, 132, 1, 0, New Rectangle(264, 120, 24, 24), "A small shard of a beautiful gem that demonstrates a distinctly red sparkle. It can be sold at a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace