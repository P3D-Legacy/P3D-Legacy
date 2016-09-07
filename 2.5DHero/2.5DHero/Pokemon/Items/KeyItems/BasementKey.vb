Namespace Items.KeyItems

    Public Class BasementKey

        Inherits Item

        Public Sub New()
            MyBase.New("Basement Key", 9800, ItemTypes.KeyItems, 133, 1, 0, New Rectangle(288, 120, 24, 24), "A key that opens a door in the Goldenrod Tunnel.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace