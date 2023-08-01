Namespace Items.Mail

    <Item(336, "Kolben Mail")>
    Public Class KolbenMail

        Inherits MailItem

        Public Overrides ReadOnly Property Description As String = "Stationery featuring a print of the Kolben Logo. It is given to Pokémon with a special meaning."
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeTraded As Boolean = False
        Public Overrides ReadOnly Property CanBeHeld As Boolean = False
        Public Overrides ReadOnly Property CanBeTossed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(360, 480, 24, 24)
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
