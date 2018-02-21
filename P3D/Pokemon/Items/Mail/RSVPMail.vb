Namespace Items.Mail

    <Item(333, "RSVP Mail")>
    Public Class RSVPMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationary designed for invitations. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(288, 480, 24, 24)
        End Sub

    End Class

End Namespace
