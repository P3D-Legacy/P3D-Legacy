Namespace Items.Standard

    Public Class PinkBow

        Inherits Item

        Public Sub New()
            MyBase.New("Pink Bow", 100, ItemTypes.Standard, 104, 1, 0, New Rectangle(144, 96, 24, 24), "Powers up Fairy-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace