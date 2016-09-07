Namespace Items.Standard

    Public Class BrickPiece

        Inherits Item

        Public Sub New()
            MyBase.New("Brick Piece", 100, ItemTypes.Standard, 180, 1, 1, New Rectangle(72, 240, 24, 24), "A rare chunk of brick.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace