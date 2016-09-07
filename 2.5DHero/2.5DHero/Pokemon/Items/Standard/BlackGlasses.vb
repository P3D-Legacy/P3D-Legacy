Namespace Items.Standard

    Public Class BlackGlasses

        Inherits Item

        Public Sub New()
            MyBase.New("Black Glasses", 100, ItemTypes.Standard, 102, 1, 0, New Rectangle(96, 96, 24, 24), "An item to be held by a Pokemon. A pair of shady-looking glasses that boost the power of Dark-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace