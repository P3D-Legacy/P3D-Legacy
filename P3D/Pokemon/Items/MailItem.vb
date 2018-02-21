Namespace Items

    ''' <summary>
    ''' The basic item that represents a Mail Item.
    ''' </summary>
    Public MustInherit Class MailItem

        Inherits Item

        Public Structure MailData
            Dim MailID As Integer
            Dim MailHeader As String
            Dim MailText As String
            Dim MailSender As String
            Dim MailAttachment As Integer
            Dim MailSignature As String
            Dim MailOriginalTrainerOT As String
            Dim MailRead As Boolean
        End Structure

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Mail
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 50

        Public Shared Function GetMailDataFromString(ByVal s As String) As MailData
            If s.Contains("|") = True Then
                s = s.Replace("|", "\,")
            End If

            Dim data() As String = s.Split("\,")

            Dim mail As New MailData
            mail.MailID = CInt(data(0))
            mail.MailSender = data(1)
            mail.MailHeader = data(2)
            mail.MailText = data(3)
            mail.MailSignature = data(4)
            mail.MailAttachment = CInt(data(5))
            mail.MailOriginalTrainerOT = data(6)
            mail.MailRead = CBool(data(7))

            Return mail
        End Function

        Public Shared Function GetStringFromMail(ByVal mail As MailData) As String
            Dim s As String = mail.MailID.ToString() & "\," &
                mail.MailSender & "\," &
                mail.MailHeader & "\," &
                mail.MailText & "\," &
                mail.MailSignature & "\," &
                mail.MailAttachment.ToString() & "\," &
                mail.MailOriginalTrainerOT & "\," &
                mail.MailRead.ToNumberString()

            Return s
        End Function

    End Class

End Namespace
