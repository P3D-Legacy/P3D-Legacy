Namespace Items.Mail

    <Item(330, "Thanks Mail")>
    Public Class ThanksMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Thanks Mail", 50, 330, New Rectangle(216, 480, 24, 24), "Stationary designed for a thank-you note. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
