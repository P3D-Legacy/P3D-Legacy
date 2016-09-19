Namespace Items.Mail

    <Item(320, "Snow Mail")>
    Public Class SnowMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Snow Mail", 50, 320, New Rectangle(480, 456, 24, 24), "Stationery featuring a print of a chilly, snow-covered world. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
