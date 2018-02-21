Namespace Items.Mail

    <Item(307, "Orange Mail")>
    Public Class OrangeMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "A Zigzagoon-print Mail to be held by a Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(144, 456, 24, 24)
        End Sub

    End Class

End Namespace
