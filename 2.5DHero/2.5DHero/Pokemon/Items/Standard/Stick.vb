Namespace Items.Standard

    Public Class Stick

        Inherits Item

        Public Sub New()
            MyBase.New("Stick", 200, ItemTypes.Standard, 105, 1, 0, New Rectangle(168, 96, 24, 24), "An item to be held by Farfetch'd. It is a very long and stiff stalk of leek that boosts the critical-hit ratio.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 60
        End Sub

    End Class

End Namespace