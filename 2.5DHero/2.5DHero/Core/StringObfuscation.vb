Public Class StringObfuscation

    Public Shared Function Obfuscate(ByVal s As String) As String
        Return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s))
    End Function

    Public Shared Function DeObfuscate(ByVal s As String) As String
        Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(s))
    End Function

End Class