Namespace Items.Mail

    <Item(309, "Shadow Mail")>
    Public Class ShadowMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "A Duskull-print Mail to be held by a Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(192, 456, 24, 24)
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
