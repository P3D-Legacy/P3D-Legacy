Namespace Items.Mail

    <Item(300, "Grass Mail")>
    Public Class GrassMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Grass Mail", 50, 300, New Rectangle(408, 456, 24, 24), "Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
