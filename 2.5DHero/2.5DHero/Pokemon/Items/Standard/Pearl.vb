Namespace Items.Standard

    Public Class Pearl

        Inherits Item

        Public Sub New()
            MyBase.New("Pearl", 1400, ItemTypes.Standard, 110, 1, 0, New Rectangle(264, 96, 24, 24), "A rather small pearl that has a very nice silvery sheen to it. It can be sold cheaply to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace