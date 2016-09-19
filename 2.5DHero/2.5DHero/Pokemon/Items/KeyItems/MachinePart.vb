Namespace Items.KeyItems

    <Item(128, "Machine Part")>
    Public Class MachinePart

        Inherits Item

        Public Sub New()
            MyBase.New("Machine Part", 100, ItemTypes.KeyItems, 128, 1, 1, New Rectangle(168, 120, 24, 24), "An important machine part for the Power Plant that was stolen.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
