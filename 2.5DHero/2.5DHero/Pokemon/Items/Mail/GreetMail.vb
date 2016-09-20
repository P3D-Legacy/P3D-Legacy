Namespace Items.Mail

    <Item(332, "Greet Mail")>
    Public Class GreetMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationary designed for introductory greetings. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(264, 480, 24, 24)
        End Sub

    End Class

End Namespace
