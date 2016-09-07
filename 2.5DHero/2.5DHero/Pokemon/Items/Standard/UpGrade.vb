Namespace Items.Standard

    Public Class UpGrade

        Inherits Item

        Public Sub New()
            MyBase.New("Up-Grade", 2100, ItemTypes.Standard, 172, 1, 0, New Rectangle(336, 144, 24, 24), "A transparent device somehow filled with all sorts of data. It was produced by Silph Co.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace