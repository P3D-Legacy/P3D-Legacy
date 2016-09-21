Namespace Items.Mail

    <Item(315, "Brick Mail")>
    Public Class BrickMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of a tough-looking brick pattern. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(336, 456, 24, 24)
        End Sub

    End Class

End Namespace
