Namespace Items.Apricorns

    Public Class RedApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("Red Apricorn", 100, ItemTypes.Plants, 85, 1, 67, New Rectangle(240, 72, 24, 24), "A red Apricorn. It assails your nostrils.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace