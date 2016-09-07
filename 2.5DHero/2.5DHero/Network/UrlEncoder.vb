Imports System.Text

''' <summary>
''' <para>Because the 4.0 client .net Framework does not contain the System.Web assembly, we need to implement the only function we
''' need from it ourselves, which is the URLEncode method.
''' This class is doing exactly that.</para>
''' </summary>
Public Class UrlEncoder

    ''' <summary>
    ''' Returns the URL encoded string of an URL that properly escapes special characters.
    ''' </summary>
    ''' <param name="str">The string to encode.</param>
    ''' <returns></returns>
    Public Shared Function Encode(ByVal str As String) As String
        If str Is Nothing Then
            Return Nothing
        End If

        Return UrlEncode(str, Encoding.UTF8)
    End Function

    Private Shared Function UrlEncode(ByVal str As String, ByVal e As Encoding) As String
        If str Is Nothing Then
            Return Nothing
        End If

        Return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e))
    End Function

    Private Shared Function UrlEncodeToBytes(ByVal str As String, ByVal e As Encoding) As Byte()
        If str Is Nothing Then
            Return Nothing
        End If

        Dim bytes As Byte() = e.GetBytes(str)
        Return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, False)
    End Function

    Private Shared Function UrlEncodeBytesToBytesInternal(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer, ByVal alwaysCreateReturnValue As Boolean) As Byte()
        Dim cSpaces As Integer = 0
        Dim cUnsafe As Integer = 0

        ' count them first
        For i As Integer = 0 To count - 1
            Dim ch As Char = Convert.ToChar(bytes(offset + i))

            If ch = " "c Then
                cSpaces += 1
            ElseIf Not IsSafe(ch) Then
                cUnsafe += 1
            End If
        Next

        ' nothing to expand?
        If Not alwaysCreateReturnValue AndAlso cSpaces = 0 AndAlso cUnsafe = 0 Then
            Return bytes
        End If

        ' expand not 'safe' characters into %XX, spaces to +s
        Dim expandedBytes As Byte() = New Byte(count + (cUnsafe * 2 - 1)) {}
        Dim pos As Integer = 0

        For i As Integer = 0 To count - 1
            Dim b As Byte = bytes(offset + i)
            Dim ch As Char = Convert.ToChar(b)

            If IsSafe(ch) Then
                expandedBytes(pos) = b
            ElseIf ch = " "c Then
                expandedBytes(pos) = Convert.ToByte("+"c)
            Else
                expandedBytes(pos) = Convert.ToByte("%"c)
                pos += 1

                expandedBytes(pos) = Convert.ToByte(IntToHex((b >> 4) And &HF))
                pos += 1

                expandedBytes(pos) = Convert.ToByte(IntToHex(b And &HF))
            End If

            pos += 1
        Next

        Return expandedBytes
    End Function

    Private Shared Function IntToHex(n As Integer) As Char
        If n <= 9 Then
            Return Convert.ToChar(n + Convert.ToInt32("0"c))
        Else
            Return Convert.ToChar(n - 10 + Convert.ToInt32("a"c))
        End If
    End Function

    'Determines if a character is a safe URL character.    
    '-_.!*\() and alphanumeric are safe characters.
    Private Shared Function IsSafe(ch As Char) As Boolean
        If ch >= "a"c AndAlso ch <= "z"c OrElse ch >= "A"c AndAlso ch <= "Z"c OrElse ch >= "0"c AndAlso ch <= "9"c Then
            Return True
        End If

        Return "-_.!*\()".ToCharArray().Contains(ch)
        '           ^ The array of safe characters, apart from alphanumeric.
    End Function

End Class