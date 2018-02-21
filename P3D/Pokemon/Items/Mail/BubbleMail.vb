Namespace Items.Mail

    <Item(316, "Bubble Mail")>
    Public Class BubbleMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of a blue world underwater. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(360, 456, 24, 24)
        End Sub

    End Class

End Namespace
