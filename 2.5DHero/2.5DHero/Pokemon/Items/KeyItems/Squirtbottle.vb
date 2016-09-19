Namespace Items.KeyItems

    <Item(175, "SquirtBottle")>
    Public Class Squirtbottle

        Inherits Item

        Public Sub New()
            MyBase.New("SquirtBottle", 9800, ItemTypes.KeyItems, 175, 1, 0, New Rectangle(360, 144, 24, 24), "A bottle used for watering plants in Loamy Soil.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace
