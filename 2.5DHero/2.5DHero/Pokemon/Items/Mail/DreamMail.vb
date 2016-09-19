Namespace Items.Mail

    <Item(302, "Dream Mail")>
    Public Class DreamMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Dream Mail", 50, 302, New Rectangle(24, 456, 24, 24), "Mail featuring a sketch of the holding Pokémon.")
        End Sub

    End Class

End Namespace
