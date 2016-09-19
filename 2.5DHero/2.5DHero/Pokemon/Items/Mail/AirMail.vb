Namespace Items.Mail

    <Item(313, "Air Mail")>
    Public Class AirMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Air Mail", 50, 313, New Rectangle(288, 456, 24, 24), "Stationery featuring a print of colorful letter sets. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
