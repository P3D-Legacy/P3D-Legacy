Namespace Items.Mail

    <Item(325, "BridgeMail D")>
    Public Class BridgeMailD

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of a red drawbridge. Let a Pokémon hold it for use."

        Public Sub New()
            _textureRectangle = New Rectangle(96, 480, 24, 24)
        End Sub

    End Class

End Namespace
