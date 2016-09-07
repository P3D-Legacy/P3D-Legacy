Namespace Items.KeyItems

    Public Class BlueCard

        Inherits Item

        Public Sub New()
            MyBase.New("Blue Card", 100, ItemTypes.KeyItems, 116, 1, 1, New Rectangle(408, 96, 24, 24), "A card to save points for the Buena's Password show.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace