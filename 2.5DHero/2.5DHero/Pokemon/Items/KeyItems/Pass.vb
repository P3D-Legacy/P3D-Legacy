Namespace Items.KeyItems

    Public Class Pass

        Inherits Item

        Public Sub New()
            MyBase.New("Pass", 100, ItemTypes.KeyItems, 134, 1, 1, New Rectangle(312, 120, 24, 24), "A ticket required for riding the Magnet Train. It allows you to ride whenever and however much you'd like.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace