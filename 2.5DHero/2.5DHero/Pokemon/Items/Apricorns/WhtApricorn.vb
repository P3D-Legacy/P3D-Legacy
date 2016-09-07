Namespace Items.Apricorns

    Public Class WhtApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("White Apricorn", 100, ItemTypes.Plants, 97, 1, 66, New Rectangle(0, 96, 24, 24), "A white Apricorn. It doesn't smell like anything.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace