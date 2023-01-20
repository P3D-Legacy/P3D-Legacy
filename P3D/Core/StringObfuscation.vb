Imports System.Text

Public Class StringObfuscation

    Public Shared Function Obfuscate(ByVal s As String) As String
        Return Convert.ToBase64String(Encoding.UTF8.GetBytes(s))
    End Function

    Public Shared Function DeObfuscate(ByVal s As String) As String
        Return Encoding.UTF8.GetString(Convert.FromBase64String(s))
    End Function

End Class