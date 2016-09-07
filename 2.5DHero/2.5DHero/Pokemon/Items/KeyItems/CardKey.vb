Namespace Items.KeyItems

    Public Class CardKey

        Inherits Item

        Public Sub New()
            MyBase.New("Card Key", 9800, ItemTypes.KeyItems, 127, 1, 0, New Rectangle(144, 120, 24, 24), "A card key that opens a shutter in the Radio Tower.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace