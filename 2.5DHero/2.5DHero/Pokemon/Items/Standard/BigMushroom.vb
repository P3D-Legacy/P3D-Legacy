Namespace Items.Plants

    Public Class BigMushroom

        Inherits Item

        Public Sub New()
            MyBase.New("Big Mushroom", 5000, ItemTypes.Plants, 87, 1, 73, New Rectangle(288, 72, 24, 24), "A very large and rare mushroom. It's popular with a certain class of collectors and sought out by them.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace