Namespace Items.KeyItems

    Public Class RunningShoes

        Inherits Item

        Public Sub New()
            MyBase.New("Running Shoes", 9800, ItemTypes.KeyItems, 78, 1, 0, New Rectangle(288, 216, 24, 24), "Special high-quality shoes. Instructions: Hold SHIFT to run!")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace