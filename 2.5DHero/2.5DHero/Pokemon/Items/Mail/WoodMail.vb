Namespace Items.Mail

    <Item(312, "Wood Mail")>
    Public Class WoodMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Wood Mail", 50, 312, New Rectangle(264, 456, 24, 24), "A Slakoth-print Mail to be held by a Pokémon.")
        End Sub

    End Class

End Namespace
