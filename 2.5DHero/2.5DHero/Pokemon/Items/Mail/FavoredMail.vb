Namespace Items.Mail

    <Item(329, "Favored Mail")>
    Public Class FavoredMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Favored Mail", 50, 329, New Rectangle(192, 480, 24, 24), "Stationary designed for writing about your favorite things. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
