Imports System.Linq
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Threading.Tasks
Imports System.IO
Imports System.Net
Imports System.ComponentModel
Imports System.Security.Cryptography

Namespace GameJolt

    Public Class ChannelClient

        Dim Prev As Integer = 0
        Dim Port As String = ""
        Dim Channel As String = ""
        Dim SID As String = ""

        Dim t As Threading.Thread

        'Public Sub New(ByVal Port As String, ByVal Channel As String)


        '    Using Client = New WebClient()
        '        Client.Credentials = New NetworkCredential("username", "password")
        '        AddHandler Client.OpenReadCompleted, Function(sender, e) As Object
        '                                                 Using reader As StreamReader = New StreamReader(e.result)
        '                                                     While True
        '                                                         MsgBox(reader.ReadLine())
        '                                                     End While
        '                                                 End Using
        '                                                 Return Nothing
        '                                             End Function
        '        Client.OpenReadAsync(New Uri("http://api-01.gamejolt.com:7001/subscribe?game_id=" & ChannelManager.GameID & "&channel=" & Channel, System.UriKind.Absolute))
        '    End Using

        '    System.Threading.Thread.Sleep(10000)
        'End Sub

        Public Sub New(ByVal Port As String, ByVal Channel As String)
            Me.Port = Port
            Me.Channel = Channel

            Dim request As HttpWebRequest = CType(WebRequest.Create(New Uri("http://api-01.gamejolt.com:7001/subscribe?game_id=" & ChannelManager.GameID & "&channel=" & Channel, System.UriKind.Absolute)), HttpWebRequest)
            request.Method = "POST"
            request.BeginGetRequestStream(AddressOf GetResponse, request)
            request.KeepAlive = True

            System.Threading.Thread.Sleep(10000)
        End Sub

        Private Sub GetResponse(ByVal result As IAsyncResult)
            Logger.Debug("LOL")
            If result.IsCompleted Then
                Dim request As HttpWebRequest = CType(result.AsyncState, HttpWebRequest)

                Dim data As String = New StreamReader(request.EndGetResponse(result).GetResponseStream()).ReadToEnd()

                'random data management
                MsgBox(data)
            End If
        End Sub

    End Class

End Namespace