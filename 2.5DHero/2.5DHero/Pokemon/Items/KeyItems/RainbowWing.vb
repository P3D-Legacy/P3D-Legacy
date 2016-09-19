Namespace Items.KeyItems

    <Item(178, "Rainbow Wing")>
    Public Class RainbowWing

        Inherits Item

        Public Sub New()
            MyBase.New("Rainbow Wing", 9800, ItemTypes.KeyItems, 178, 1, 0, New Rectangle(408, 144, 24, 24), "A mystical rainbow feather that sparkles.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False

            Me._flingDamage = 20
        End Sub

    End Class

End Namespace
