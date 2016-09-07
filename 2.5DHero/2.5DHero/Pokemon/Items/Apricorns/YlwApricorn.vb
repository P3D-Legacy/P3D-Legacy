Namespace Items.Apricorns

    Public Class YlwApricorn

        Inherits Item

        Public Sub New()
            MyBase.New("Yellow Apricorn", 100, ItemTypes.Plants, 92, 1, 65, New Rectangle(384, 72, 24, 24), "A yellow Apricorn. It has an invigorating scent.")

            Me._canBeUsedInBattle = False
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace