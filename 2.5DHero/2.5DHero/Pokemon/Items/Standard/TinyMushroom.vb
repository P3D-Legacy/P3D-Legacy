Namespace Items.Plants

    Public Class TinyMushroom

        Inherits Item

        Public Sub New()
            MyBase.New("Tiny Mushroom", 500, ItemTypes.Plants, 86, 1, 72, New Rectangle(264, 72, 24, 24), "A very small and rare mushroom. It's popular with a certain class of collectors and sought out by them.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace