Namespace Items.KeyItems

    <Item(284, "Rainbow Pass")>
    Public Class RainbowPass

        Inherits Item

        Public Sub New()
            MyBase.New("Rainbow Pass", 100, ItemTypes.KeyItems, 284, 1, 1, New Rectangle(144, 264, 24, 24), "A pass for ferries between Vermilion and the Sevii Islands. It features a drawing of a rainbow.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
