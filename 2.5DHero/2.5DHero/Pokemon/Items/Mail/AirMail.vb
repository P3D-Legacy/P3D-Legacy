Namespace Items.Mail

    <Item(313, "Air Mail")>
    Public Class AirMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of colorful letter sets. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(288, 456, 24, 24)
        End Sub

    End Class

End Namespace
