Namespace Items.Standard

    Public Class PearlString

        Inherits Item

        Public Sub New()
            MyBase.New("Pearl String", 50000, ItemTypes.Standard, 173, 1, 0, New Rectangle(408, 216, 24, 24), "Very large pearls that sparkle in a pretty silver color. They can be sold at a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace