Namespace Items.Mail

    <Item(331, "Inquiry Mail")>
    Public Class InquiryMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationary designed for writing questions. Let a Pokémon hold it for delivery."

        Public Sub New()
            _textureRectangle = New Rectangle(240, 480, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Dim MailID As String
            If Me.IsGameModeItem = True Then
                MailID = Me.gmID
            Else
                MailID = Me.ID.ToString
            End If
            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MailSystemScreen(Core.CurrentScreen, MailID), Color.Black, False))
        End Sub

    End Class

End Namespace
