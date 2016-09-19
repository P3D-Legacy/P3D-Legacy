Namespace Items.Mail

    <Item(331, "Inquiry Mail")>
    Public Class InquiryMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Inquiry Mail", 50, 331, New Rectangle(240, 480, 24, 24), "Stationary designed for writing questions. Let a Pokémon hold it for delivery.")
        End Sub

    End Class

End Namespace
