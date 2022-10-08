Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml

Public NotInheritable Class NewAPI

    Private Const Host As String = "https://api.gamejolt.com/api/game/v1_2"

    Private Shared _cachedGameId As String
    Private Shared _cachedGameKey As String

    Private Shared ReadOnly Property GameId As String
        Get
            If _cachedGameId Is Nothing Then
                _cachedGameId = StringObfuscation.DeObfuscate(Classified.GameJolt_Game_ID)
            End If
            Return _cachedGameId
        End Get
    End Property

    Private Shared ReadOnly Property GameKey As String
        Get
            If _cachedGameKey Is Nothing Then
                _cachedGameKey = StringObfuscation.DeObfuscate(Classified.GameJolt_Game_Key)
            End If
            Return _cachedGameKey
        End Get
    End Property

    Public Shared Sub FetchUser(username As String, Optional onSuccess As Action(Of Dictionary(Of String, String)) = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("username", username)

        Dim request = BuildGetRequestAsync("/users/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 If xml("response")("success").InnerText = "true" Then
                                                     Dim result = New Dictionary(Of String, String)

                                                     For Each childNode As XmlNode In xml("response")("users")("user").ChildNodes
                                                         result.Add(childNode.Name, childNode.InnerText)
                                                     Next

                                                     onSuccess?.Invoke(result)
                                                 Else
                                                     onFailed?.Invoke(New Exception(xml("response")("message").InnerText))
                                                 End If
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Public Shared Sub FetchUser(userId As Integer, Optional onSuccess As Action(Of Dictionary(Of String, String)) = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("user_id", userId.ToString())

        Dim request = BuildGetRequestAsync("/users/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 If xml("response")("success").InnerText = "true" Then
                                                     Dim result = New Dictionary(Of String, String)

                                                     For Each childNode As XmlNode In xml("response")("users")("user").ChildNodes
                                                         result.Add(childNode.Name, childNode.InnerText)
                                                     Next

                                                     onSuccess?.Invoke(result)
                                                 Else
                                                     onFailed?.Invoke(New Exception(xml("response")("message").InnerText))
                                                 End If
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Public Shared Sub AuthUser(username As String, userToken As String, Optional onSuccess As Action = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        Dim request = BuildGetRequestAsync("/users/auth/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 If xml("response")("success").InnerText = "true" Then
                                                     onSuccess?.Invoke()
                                                 Else
                                                     onFailed?.Invoke(New Exception(xml("response")("message").InnerText))
                                                 End If
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Public Shared Sub OpenSession(username As String, userToken As String, Optional onSuccess As Action = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        Dim request = BuildGetRequestAsync("/sessions/open/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 If xml("response")("success").InnerText = "true" Then
                                                     onSuccess?.Invoke()
                                                 Else
                                                     onFailed?.Invoke(New Exception(xml("response")("message").InnerText))
                                                 End If
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Public Shared Sub PingSession(username As String, userToken As String, Optional onSuccess As Action = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        Dim request = BuildGetRequestAsync("/sessions/ping/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 If xml("response")("success").InnerText = "true" Then
                                                     onSuccess?.Invoke()
                                                 Else
                                                     onFailed?.Invoke(New Exception(xml("response")("message").InnerText))
                                                 End If
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Public Shared Sub CheckSession(username As String, userToken As String, Optional onSuccess As Action(Of Boolean) = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        Dim request = BuildGetRequestAsync("/sessions/check/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 onSuccess?.Invoke(Boolean.Parse(xml("response")("success").InnerText))
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Public Shared Sub CloseSession(username As String, userToken As String, Optional onSuccess As Action = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New Dictionary(Of String, String)()
        parameters.Add("format", "xml")
        parameters.Add("game_id", GameId)
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        Dim request = BuildGetRequestAsync("/sessions/close/", parameters)
        request.ContinueWith(Sub(task As Task(Of WebResponse))
                                 If task.IsFaulted Then
                                     onFailed?.Invoke(task.Exception)
                                 Else
                                     Try
                                         Using responseStream As Stream = task.Result.GetResponseStream()
                                             Using reader = New StreamReader(responseStream)
                                                 Dim xml = New XmlDocument()
                                                 xml.LoadXml(reader.ReadToEnd())

                                                 If xml("response")("success").InnerText = "true" Then
                                                     onSuccess?.Invoke()
                                                 Else
                                                     onFailed?.Invoke(New Exception(xml("response")("message").InnerText))
                                                 End If
                                             End Using
                                         End Using
                                     Catch ex As Exception
                                         onFailed?.Invoke(ex)
                                     End Try
                                 End If
                             End Sub)
    End Sub

    Private Shared Function BuildGetRequestAsync(endpoint As String, parameters As Dictionary(Of String, String)) As Task(Of WebResponse)
        Dim fullUrl = New StringBuilder(Host)
        fullUrl.Append(endpoint)
        fullUrl.Append("?")

        For Each keyValuePair As KeyValuePair(Of String, String) In parameters
            fullUrl.Append(keyValuePair.Key)
            fullUrl.Append("=")
            fullUrl.Append(WebUtility.UrlEncode(keyValuePair.Value))
            fullUrl.Append("&")
        Next

        fullUrl.Remove(fullUrl.Length - 1, 1)
        fullUrl.Append(GameKey)

        Using hashObj = MD5.Create()
            Dim hash = hashObj.ComputeHash(Encoding.UTF8.GetBytes(fullUrl.ToString()))
            fullUrl.Remove(fullUrl.Length - GameKey.Length, GameKey.Length)
            fullUrl.Append("&signature=")

            For Each b As Byte In hash
                fullUrl.Append(b.ToString("x2"))
            Next
        End Using

        Dim requestUrl = fullUrl.ToString()

        Debug.Print(requestUrl)

        Dim request = WebRequest.CreateHttp(requestUrl)
        request.Method = "GET"
        Return request.GetResponseAsync()
    End Function

End Class
