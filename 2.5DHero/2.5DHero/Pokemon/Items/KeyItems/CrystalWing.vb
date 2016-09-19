Namespace Items.KeyItems

    <Item(56, "Crystal Wing")>
    Public Class CrystalWing

        Inherits Item

        Public Sub New()
            MyBase.New("Crystal Wing", 9800, ItemTypes.KeyItems, 56, 1, 0, New Rectangle(240, 192, 24, 24), "A mystical feather entirely made out of crystal.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False

            Me._flingDamage = 20
        End Sub

    End Class

End Namespace
