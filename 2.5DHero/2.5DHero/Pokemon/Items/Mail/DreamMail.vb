Namespace Items.Mail

    <Item(302, "Dream Mail")>
    Public Class DreamMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Mail featuring a sketch of the holding Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(24, 456, 24, 24)
        End Sub

    End Class

End Namespace
