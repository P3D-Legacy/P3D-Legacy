Namespace Items.Mail

    <Item(321, "Space Mail")>
    Public Class SpaceMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Space Mail", 50, 321, New Rectangle(0, 480, 24, 24), "Stationery featuring a print depicting the huge expanse of space. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
