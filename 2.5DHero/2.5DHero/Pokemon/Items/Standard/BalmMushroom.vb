Namespace Items.Standard

    Public Class BalmMushroom

        Inherits Item

        Public Sub New()
            MyBase.New("Balm Mushroom", 50000, ItemTypes.Standard, 153, 1, 0, New Rectangle(48, 216, 24, 24), "A rare mushroom which gives off a nice fragrance. A maniac will buy it for a high price.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace