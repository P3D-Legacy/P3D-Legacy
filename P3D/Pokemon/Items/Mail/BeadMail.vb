Namespace Items.Mail

    <Item(301, "Bead Mail")>
    Public Class BeadMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Mail featuring a sketch of the holding Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(0, 456, 24, 24)
        End Sub

    End Class

End Namespace
