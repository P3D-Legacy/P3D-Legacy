Namespace Items.KeyItems

    Public Class SSTicket

        Inherits Item

        Public Sub New()
            MyBase.New("S.S. Ticket", 9800, ItemTypes.KeyItems, 41, 1, 0, New Rectangle(240, 216, 24, 24), "The ticket required for sailing on the ferry S.S. Aqua. It has a drawing of a ship on it. ")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace