Namespace Items.KeyItems

    Public Class OldSeaMap

        Inherits Item

        Public Sub New()
            MyBase.New("Old Sea Map", 100, ItemTypes.KeyItems, 285, 1, 1, New Rectangle(168, 264, 24, 24), "A faded sea chart that shows the way to a certain island.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace