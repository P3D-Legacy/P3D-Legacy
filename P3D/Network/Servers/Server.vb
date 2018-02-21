Namespace Servers

    Public Class Server

        Public IP As String = ""
        Public Port As String = ""

        Public Sub New(ByVal Address As String)
            If Address.Contains(":") = True Then
                Me.IP = Address.Split(CChar(":"))(0)
                Me.Port = Address.Split(CChar(":"))(1)
            Else
                Me.IP = Address
                Me.Port = "15124"
            End If
        End Sub

    End Class

End Namespace