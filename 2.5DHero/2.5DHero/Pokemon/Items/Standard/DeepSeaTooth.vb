Namespace Items.Standard

    Public Class DeepSeaTooth

        Inherits Item

        Public Sub New()
            MyBase.New("DeepSeaTooth", 200, ItemTypes.Standard, 167, 1, 1, New Rectangle(312, 216, 24, 24), "An item to be held by Clamperl. This fang gleams a sharp silver and raises the holder's Sp. Atk stat.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 90
        End Sub

    End Class

End Namespace