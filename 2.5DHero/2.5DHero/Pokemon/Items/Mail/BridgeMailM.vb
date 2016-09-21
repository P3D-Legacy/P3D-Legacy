Namespace Items.Mail

    <Item(328, "BridgeMail M")>
    Public Class BridgeMailM

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of an arched bridge. Let a Pokémon hold it for use."

        Public Sub New()
            _textureRectangle = New Rectangle(168, 480, 24, 24)
        End Sub

    End Class

End Namespace
