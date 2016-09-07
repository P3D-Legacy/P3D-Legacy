Namespace Items.Standard

    Public Class HeartScale
        Inherits Item

        Public Sub New()
            MyBase.New("Heart Scale", 100, ItemTypes.Standard, 190, 1, 0, New Rectangle(264, 216, 24, 24), "A pretty, heart-shaped scale that is extremely rare. It glows faintly with all of the colors of the rainbow.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace