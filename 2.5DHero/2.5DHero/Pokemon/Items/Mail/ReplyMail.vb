Namespace Items.Mail

    <Item(335, "Reply Mail")>
    Public Class ReplyMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Reply Mail", 50, 335, New Rectangle(336, 480, 24, 24), "Stationary designed for writing a reply. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
