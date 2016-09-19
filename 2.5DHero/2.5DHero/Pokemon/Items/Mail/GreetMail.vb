Namespace Items.Mail

    <Item(332, "Greet Mail")>
    Public Class GreetMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Greet Mail", 50, 332, New Rectangle(264, 480, 24, 24), "Stationary designed for introductory greetings. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
