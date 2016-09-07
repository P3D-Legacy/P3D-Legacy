Namespace Items.Standard

    Public Class SlowPokeTail

        Inherits Item

        Public Sub New()
            MyBase.New("Slowpoketail", 9800, ItemTypes.Standard, 103, 1, 0, New Rectangle(120, 96, 24, 24), "A very tasty tail of something. It sells for a high price.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace