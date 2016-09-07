Namespace Items.Apricorns

    Public Class PnkApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("Pink Apricorn", 100, ItemTypes.Plants, 101, 1, 68, New Rectangle(72, 96, 24, 24), "A pink Apricorn. It has a nice, sweet scent.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace