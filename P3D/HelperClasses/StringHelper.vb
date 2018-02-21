Public Class StringHelper

    Private Sub New()
        Throw New InvalidOperationException("Cannot initialize static class.")
    End Sub

    Public Shared Function GetChar(charCode As Integer) As Char
        Return Char.ConvertFromUtf32(charCode)(0)
    End Function

    Public Shared ReadOnly Property Tab() As Char
        Get
            Return GetChar(9)
        End Get
    End Property

    Public Shared ReadOnly Property LineFeed() As Char
        Get
            Return GetChar(10)
        End Get
    End Property

    Public Shared ReadOnly Property CrLf() As String
        Get
            Return GetChar(13) + LineFeed
        End Get
    End Property

    Public Shared Function IsNumeric(obj As Object) As Boolean
        If TypeOf obj Is String Then
            Return IsNumeric(CStr(obj))
        End If

        Return Microsoft.VisualBasic.IsNumeric(obj)
    End Function

    Public Shared Function IsNumeric(str As String) As Boolean
        Dim discard As Decimal
        Return Decimal.TryParse(str, Globalization.NumberStyles.Float, Globalization.NumberFormatInfo.CurrentInfo, discard)
    End Function

End Class
