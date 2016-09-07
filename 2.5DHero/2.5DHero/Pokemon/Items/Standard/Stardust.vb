Namespace Items.Standard

    Public Class Stardust

        Inherits Item

        Public Sub New()
            MyBase.New("Stardust", 2000, ItemTypes.Standard, 131, 1, 0, New Rectangle(240, 120, 24, 24), "Lovely, red sand that flows between the fingers with a loose, silky feel. It can be sold at a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace