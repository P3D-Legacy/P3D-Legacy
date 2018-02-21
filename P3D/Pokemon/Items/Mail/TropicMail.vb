Namespace Items.Mail

    <Item(310, "Tropic Mail")>
    Public Class TropicMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "A Bellossom-print Mail to be held by a Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(216, 456, 24, 24)
        End Sub

    End Class

End Namespace
