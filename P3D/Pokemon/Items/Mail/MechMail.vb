Namespace Items.Mail

    <Item(306, "Mech Mail")>
    Public Class MechMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "A Magnemite-print Mail to be held by a Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(120, 456, 24, 24)
        End Sub

    End Class

End Namespace
