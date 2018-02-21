Namespace Items.Mail

    <Item(319, "Mosaic Mail")>
    Public Class MosaicMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of a vivid rainbow pattern. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(456, 456, 24, 24)
        End Sub

    End Class

End Namespace
