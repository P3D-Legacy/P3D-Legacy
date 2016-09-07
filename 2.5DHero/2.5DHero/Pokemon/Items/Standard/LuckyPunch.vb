Namespace Items.Standard

    Public Class LuckyPunch

        Inherits Item

        Public Sub New()
            MyBase.New("Lucky Punch", 10, ItemTypes.Standard, 30, 1, 0, New Rectangle(144, 24, 24, 24), "An item to be held by Chansey. This pair of lucky boxing gloves will boost Chansey's critical-hit ratio.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 40
        End Sub

    End Class

End Namespace