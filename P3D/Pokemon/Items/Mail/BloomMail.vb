Namespace Items.Mail

    <Item(314, "Bloom Mail")>
    Public Class BloomMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of pretty floral patterns. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(312, 456, 24, 24)
        End Sub

    End Class

End Namespace
