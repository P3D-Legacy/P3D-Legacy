Namespace Items.Mail

    <Item(333, "RSVP Mail")>
    Public Class RSVPMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("RSVP Mail", 50, 333, New Rectangle(288, 480, 24, 24), "Stationary designed for invitations. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
