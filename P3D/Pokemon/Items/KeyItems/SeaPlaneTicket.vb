Namespace Items.KeyItems

    <Item(655, "Sea Plane Ticket")>
    Public Class SeaPlaneTicket

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "The ticket required for flying on the sea plane. It has a drawing of a sea plane on it. "

        Public Sub New()
            _textureRectangle = New Rectangle(432, 408, 24, 24)
        End Sub

    End Class

End Namespace
