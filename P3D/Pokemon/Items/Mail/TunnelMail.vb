Namespace Items.Mail

    <Item(323, "Tunnel Mail")>
    Public Class TunnelMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of a dimly lit coal mine. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(48, 480, 24, 24)
        End Sub

    End Class

End Namespace
