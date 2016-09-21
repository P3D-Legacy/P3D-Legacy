Namespace Items.Mail

    <Item(309, "Shadow Mail")>
    Public Class ShadowMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "A Duskull-print Mail to be held by a Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(192, 456, 24, 24)
        End Sub

    End Class

End Namespace
