Namespace Items.Apricorns

    Public Class BluApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("Blue Apricorn", 100, ItemTypes.Plants, 89, 1, 70, New Rectangle(336, 72, 24, 24), "A blue Apricorn. It smells a bit like grass.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace