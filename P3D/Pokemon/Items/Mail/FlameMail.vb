Namespace Items.Mail

    <Item(317, "Flame Mail")>
    Public Class FlameMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of flames in blazing red. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(384, 456, 24, 24)
        End Sub

    End Class

End Namespace
