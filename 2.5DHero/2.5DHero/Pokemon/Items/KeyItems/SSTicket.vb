Namespace Items.KeyItems

    <Item(41, "S.S. Ticket")>
    Public Class SSTicket

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "The ticket required for sailing on the ferry S.S. Aqua. It has a drawing of a ship on it. "

        Public Sub New()
            _textureRectangle = New Rectangle(240, 216, 24, 24)
        End Sub

    End Class

End Namespace
