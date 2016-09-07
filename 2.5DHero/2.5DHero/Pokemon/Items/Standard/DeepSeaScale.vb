Namespace Items.Standard

    Public Class DeepSeaScale

        Inherits Item

        Public Sub New()
            MyBase.New("DeepSeaScale", 200, ItemTypes.Standard, 162, 1, 1, New Rectangle(120, 216, 24, 24), "An item to be held by Clamperl. This scale shines with a faint pink and raises the holder's Sp. Def stat.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace