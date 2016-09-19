Namespace Items.Mail

    <Item(301, "Bead Mail")>
    Public Class BeadMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Bead Mail", 50, 301, New Rectangle(0, 456, 24, 24), "Mail featuring a sketch of the holding Pokémon.")
        End Sub

    End Class

End Namespace
