Namespace Items.Mail

    <Item(321, "Space Mail")>
    Public Class SpaceMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print depicting the huge expanse of space. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(0, 480, 24, 24)
        End Sub

    End Class

End Namespace
