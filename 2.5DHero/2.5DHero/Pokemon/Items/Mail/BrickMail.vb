Namespace Items.Mail

    <Item(315, "Brick Mail")>
    Public Class BrickMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Brick Mail", 50, 315, New Rectangle(336, 456, 24, 24), "Stationery featuring a print of a tough-looking brick pattern. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
