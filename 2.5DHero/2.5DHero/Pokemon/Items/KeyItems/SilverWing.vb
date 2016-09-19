Namespace Items.KeyItems

    <Item(71, "Silver Wing")>
    Public Class SilverWing

        Inherits Item

        Public Sub New()
            MyBase.New("Silver Wing", 9800, ItemTypes.KeyItems, 71, 1, 0, New Rectangle(48, 72, 24, 24), "A strange, silvery feather that sparkles.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False

            Me._flingDamage = 20
        End Sub

    End Class

End Namespace
