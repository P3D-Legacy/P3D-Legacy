Namespace Items.Apricorns

    Public Class BlkApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("Black Apricorn", 100, ItemTypes.Plants, 99, 1, 71, New Rectangle(48, 96, 24, 24), "A black Apricorn It has an indescribable scent.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace