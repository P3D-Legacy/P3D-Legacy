Namespace Items.Mail

    <Item(327, "BridgeMail V")>
    Public Class BridgeMailV

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of a brick bridge. Let a Pokémon hold it for use."

        Public Sub New()
            _textureRectangle = New Rectangle(144, 480, 24, 24)
        End Sub

    End Class

End Namespace
