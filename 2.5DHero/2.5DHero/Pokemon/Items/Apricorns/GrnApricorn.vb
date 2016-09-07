Namespace Items.Apricorns

    Public Class GrnApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("Green Apricorn", 100, ItemTypes.Plants, 93, 1, 69, New Rectangle(408, 72, 24, 24), "A green Apricorn. It has a mysterious, aromatic scent.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace