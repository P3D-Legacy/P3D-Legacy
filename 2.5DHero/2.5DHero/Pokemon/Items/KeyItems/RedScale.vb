Namespace Items.KeyItems

    Public Class RedScale

        Inherits Item

        Public Sub New()
            MyBase.New("Red Scale", 9800, ItemTypes.KeyItems, 66, 1, 0, New Rectangle(432, 48, 24, 24), "A scale from the red Gyarados. It glows red like a flame.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace