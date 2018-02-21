Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class Encryption

    Public Shared Function EncryptString(ByVal s As String, ByVal password As String) As String
        Dim rd As New RijndaelManaged
        Dim out As String = ""

        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(StringObfuscation.DeObfuscate(password)))

        md5.Clear()
        rd.Key = key
        rd.GenerateIV()

        Dim iv() As Byte = rd.IV
        Dim ms As New MemoryStream

        ms.Write(iv, 0, iv.Length)

        Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
        Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(s)

        cs.Write(data, 0, data.Length)
        cs.FlushFinalBlock()

        Dim encdata() As Byte = ms.ToArray()
        out = Convert.ToBase64String(encdata)
        cs.Close()
        rd.Clear()

        Return out
    End Function

    Public Shared Function DecryptString(ByVal s As String, ByVal password As String) As String
        Dim rd As New RijndaelManaged
        Dim rijndaelIvLength As Integer = 16
        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(StringObfuscation.DeObfuscate(password)))

        md5.Clear()

        Dim encdata() As Byte = Convert.FromBase64String(s)
        Dim ms As New MemoryStream(encdata)
        Dim iv(15) As Byte

        ms.Read(iv, 0, rijndaelIvLength)
        rd.IV = iv
        rd.Key = key

        Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)

        Dim data(CInt(ms.Length - rijndaelIvLength)) As Byte
        Dim i As Integer = cs.Read(data, 0, data.Length)

        cs.Close()
        rd.Clear()
        Return System.Text.Encoding.UTF8.GetString(data, 0, i)
    End Function

End Class