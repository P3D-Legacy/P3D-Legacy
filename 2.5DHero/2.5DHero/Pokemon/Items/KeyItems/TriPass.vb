Namespace Items.KeyItems

    <Item(265, "Tri-Pass")>
    Public Class TriPass

        Inherits Item

        Public Sub New()
            MyBase.New("Tri-Pass", 100, ItemTypes.KeyItems, 265, 1, 1, New Rectangle(480, 48, 24, 24), "A pass for ferries between One, Two, and Three Island. It has a drawing of three islands.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
