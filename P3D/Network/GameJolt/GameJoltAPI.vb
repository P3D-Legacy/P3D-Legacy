Imports System.Globalization
Imports System.Net
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json

Public NotInheritable Class GameJoltAPI
    Private NotInheritable Class SnakeCaseNamingPolicy
        Inherits JsonNamingPolicy

        Public Overrides Function ConvertName(name As String) As String
            Return String.Join("", name.Select(Function(c, i) IIf(i > 0 AndAlso Char.IsUpper(c), $"_{Char.ToLowerInvariant(c)}", Char.ToLowerInvariant(c))))
        End Function
    End Class

    Private NotInheritable Class GameJoltObject (Of T As GameJoltResponseObject)
        Public Property Response As T
    End Class

    Public MustInherit Class GameJoltResponseObject
        Public Property Success As String
        Public Property Message As String
    End Class

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

    Private Shared ReadOnly Property JsonSerializerOptions As JsonSerializerOptions = new JsonSerializerOptions() With { .PropertyNamingPolicy = new SnakeCaseNamingPolicy() }

#Region "Users"

    Public NotInheritable Class FetchUsersObject
        Inherits GameJoltResponseObject

        Public Property Users As UserObject()
    End Class

    Public NotInheritable Class UserObject
        Public Property Id As String
        Public Property Type As String
        Public Property Username As String
        Public Property AvatarUrl As String
        Public Property SignedUp As String
        Public Property SignedUpTimestamp As Long
        Public Property LastLoggedIn As String
        Public Property LastLoggedInTimestamp As Long
        Public Property Status As String
        Public Property DeveloperName As String
        Public Property DeveloperWebsite As String
        Public Property DeveloperDescription As String
    End Class

    Public Shared Sub FetchUser(username As String, Optional onSuccess As Action(Of UserObject) = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)

        ExecuteRequestsAsync({("/users/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of FetchUsersObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke(result.Response.Users(0))
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    onFailed?.Invoke(ex)
                End Try
            End If
        End Sub)
    End Sub

    Public Shared Sub FetchUser(userId As Long, Optional onSuccess As Action(Of UserObject) = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("user_id", userId.ToString())

        ExecuteRequestsAsync({("/users/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of FetchUsersObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke(result.Response.Users(0))
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    onFailed?.Invoke(ex)
                End Try
            End If
        End Sub)
    End Sub

    Public Shared Sub FetchUsers(userIds As Long(), Optional onSuccess As Action(Of UserObject()) = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("user_id", String.Join(",", userIds.Select(Function(l) l.ToString())))

        ExecuteRequestsAsync({("/users/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of FetchUsersObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke(result.Response.Users)
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
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
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        ExecuteRequestsAsync({("/users/auth/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of GameJoltResponseObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke()
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    onFailed?.Invoke(ex)
                End Try
            End If
        End Sub)
    End Sub

#End Region

#Region "Sessions"

    Public Shared Sub OpenSession(username As String, userToken As String, Optional onSuccess As Action = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        ExecuteRequestsAsync({("/sessions/open/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of GameJoltResponseObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke()
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
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
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        ExecuteRequestsAsync({("/sessions/ping/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of GameJoltResponseObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke()
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
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
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        ExecuteRequestsAsync({("/sessions/check/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of GameJoltResponseObject))(reader.ReadToEnd(), JsonSerializerOptions)
                            onSuccess?.Invoke(result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase))
                        End Using
                    End Using
                Catch ex As Exception
                    onFailed?.Invoke(ex)
                End Try
            End If
        End Sub)
    End Sub

    Public Shared Sub CloseSession(username As String, userToken As String, Optional onSuccess As Action = Nothing, Optional onFailed As Action(Of Exception) = Nothing)
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        ExecuteRequestsAsync({("/sessions/close/", parameters)}).ContinueWith(Sub(task As Task(Of Stream))
            If task.IsFaulted Then
                onFailed?.Invoke(task.Exception)
            Else
                Try
                    Using responseStream As Stream = task.Result
                        Using reader = New StreamReader(responseStream)
                            Dim result = JsonSerializer.Deserialize (Of GameJoltObject(Of GameJoltResponseObject))(reader.ReadToEnd(), JsonSerializerOptions)

                            If result.Response.Success.Equals("true", StringComparison.OrdinalIgnoreCase) Then
                                onSuccess?.Invoke()
                            Else
                                onFailed?.Invoke(New Exception(result.Response.Message))
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    onFailed?.Invoke(ex)
                End Try
            End If
        End Sub)
    End Sub

#End Region
    
#Region "Scores"
#End Region
    
#Region "Trophies"
#End Region
    
#Region "Data Store"
#End Region
    
#Region "Friends"
#End Region
    
#Region "Time"
#End Region

    Public Async Shared Function ExecuteRequestsAsync(requests As (endpoint As String, parameters As SortedDictionary(Of String, String))(), Optional batchParameters as SortedDictionary(Of String, String) = Nothing) As Task(Of Stream)
        Dim postData = new StringBuilder()
        Dim additionalData = new StringBuilder()

        If requests.Length = 1 Then
            For Each keyValuePair As KeyValuePair(Of String, String) In requests(0).parameters
                postData.AppendFormat("{0}={1}&", WebUtility.UrlEncode(keyValuePair.Key), WebUtility.UrlEncode(keyValuePair.Value))
                additionalData.Append(WebUtility.UrlEncode(keyValuePair.Key))
                additionalData.Append(WebUtility.UrlEncode(keyValuePair.Value))
            Next

            If postData.Chars(postData.Length - 1) = "&"c Then
                postData.Remove(postData.Length - 1, 1)
            End If

            Dim postDataContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded")
            Dim requestUrl As String = BuildHashedRequest(requests(0).endpoint, New SortedDictionary(Of String,String)(), additionalData.ToString())
            Debug.Print(requestUrl)

            Dim response = Await HttpClient.PostAsync(requestUrl, postDataContent)
            response.EnsureSuccessStatusCode()
            Return response.Content.ReadAsStream()
        Else
            If batchParameters Is Nothing Then
                Throw New ArgumentNullException(NameOf(batchParameters))
            End If

            For Each request In requests
                Dim subRequestUrl = BuildHashedRequest(request.endpoint, request.parameters, String.Empty, False)
                postData.AppendFormat("requests[]={0}&", WebUtility.UrlEncode(subRequestUrl))
                additionalData.AppendFormat("requests{0}", subRequestUrl)
            Next

            If postData.Chars(postData.Length - 1) = "&"c Then
                postData.Remove(postData.Length - 1, 1)
            End If

            Dim postDataContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded")
            Dim requestUrl = BuildHashedRequest("/batch/", batchParameters, additionalData.ToString())
            Debug.Print(requestUrl)

            Dim response = Await HttpClient.PostAsync(requestUrl, postDataContent)
            response.EnsureSuccessStatusCode()
            Return response.Content.ReadAsStream()
        End If
    End Function

    Private Shared Function BuildHashedRequest(endpoint As String, parameters As SortedDictionary(Of String, String), Optional additionalData As String = "", Optional includeHost As Boolean = True) As String
        Dim fullUrl = New StringBuilder()

        If includeHost Then
            fullUrl.Append(Host)
        End If

        fullUrl.AppendFormat("{0}?", endpoint)

        If Not parameters.ContainsKey("game_id") Then
            parameters.Add("game_id", GameId)
        End If

        For Each keyValuePair As KeyValuePair(Of String, String) In parameters
            fullUrl.AppendFormat("{0}={1}&", WebUtility.UrlEncode(keyValuePair.Key), WebUtility.UrlEncode(keyValuePair.Value))
        Next

        If fullUrl.Chars(fullUrl.Length - 1) = "&"c Then
            fullUrl.Remove(fullUrl.Length - 1, 1)
        End If

        fullUrl.Append(additionalData)
        fullUrl.Append(GameKey)

        Using hashObj = MD5.Create()
            Dim hash = hashObj.ComputeHash(Encoding.UTF8.GetBytes(fullUrl.ToString()))
            fullUrl.Remove(fullUrl.Length - GameKey.Length - additionalData.Length, GameKey.Length + additionalData.Length)

            If fullUrl.Chars(fullUrl.Length - 1) = "?"c Then
                fullUrl.Append("signature=")
            Else
                fullUrl.Append("&signature=")
            End If

            For Each b As Byte In hash
                fullUrl.Append(b.ToString("x2"))
            Next
        End Using

        Return fullUrl.ToString()
    End Function
End Class

Public NotInheritable Class GameJoltBatchAPIBuilder
    Private ReadOnly _requests As List(Of (endpoint As String, parameters As SortedDictionary(Of String, String))) = New List(Of (endpoint as String, parameters as SortedDictionary(Of String,String)))()

    Private _isParallel As Boolean
    Private _isBreakOnError As Boolean

    Public Async Function ExecuteRequestsAsync() As Task(Of Stream())
        Dim parameters = New SortedDictionary(Of String, String)()

        If _isParallel Then
            parameters.Add("parallel", "true")
            
            If _requests.Count > 50 Then
                Dim responses = New List(Of Task(Of Stream))
                
                For Each chunk In _requests.Chunk(50)
                    responses.Add(GameJoltAPI.ExecuteRequestsAsync(chunk, parameters))
                Next
                
                Return Await Task.WhenAll(responses)
            Else 
                Return { Await GameJoltAPI.ExecuteRequestsAsync(_requests.ToArray(), parameters) }
            End If
        Else If _isBreakOnError Then
            parameters.Add("break_on_error", "true")
        End If
        
        If _requests.Count > 50 Then
            Dim responses = New List(Of Stream)
                
            For Each chunk In _requests.Chunk(50)
                responses.Add(Await GameJoltAPI.ExecuteRequestsAsync(chunk, parameters))
            Next
                
            Return responses.ToArray()
        Else 
            Return { Await GameJoltAPI.ExecuteRequestsAsync(_requests.ToArray(), parameters) }
        End If
    End Function

    Public Function AsParallel(Optional value as Boolean = True) As GameJoltBatchAPIBuilder
        _isParallel = value
        Return Me
    End Function

    Public Function BreakOnError(Optional value as Boolean = True) As GameJoltBatchAPIBuilder
        _isBreakOnError = value
        Return Me
    End Function

#Region "Users"

    Public Function FetchUser(username As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)

        _requests.Add(("/users/", parameters))
        Return Me
    End Function

    Public Function FetchUser(userId As Long) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("user_id", userId.ToString(CultureInfo.InvariantCulture))

        _requests.Add(("/users/", parameters))
        Return Me
    End Function

    Public Function FetchUsers(userIds As Long()) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("user_id", String.Join(",", userIds.Select(Function(l) l.ToString(CultureInfo.InvariantCulture))))

        _requests.Add(("/users/", parameters))
        Return Me
    End Function

    Public Function AuthUser(username As String, userToken As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        _requests.Add(("/users/auth/", parameters))
        Return Me
    End Function

#End Region

#Region "Sessions"

    Public Function OpenSession(username As String, userToken As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        _requests.Add(("/sessions/open/", parameters))
        Return Me
    End Function

    Public Function PingSession(username As String, userToken As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        _requests.Add(("/sessions/ping/", parameters))
        Return Me
    End Function

    Public Function CheckSession(username As String, userToken As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        _requests.Add(("/sessions/check/", parameters))
        Return Me
    End Function

    Public Function CloseSession(username As String, userToken As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        _requests.Add(("/sessions/close/", parameters))
        Return Me
    End Function

#End Region

#Region "Scores"

    Public Function FetchScore(Optional limit As Integer? = Nothing,
                               Optional tableId As Integer? = Nothing,
                               Optional username As String = Nothing,
                               Optional userToken As String = Nothing,
                               Optional guest As String = Nothing,
                               Optional betterThan As Integer? = Nothing,
                               Optional worseThan As Integer? = Nothing) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()

        If limit.HasValue Then
            parameters.Add("limit", limit.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If tableId.HasValue Then
            parameters.Add("table_id", tableId.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If username IsNot Nothing Then
            parameters.Add("username", username.ToString())
        End If

        If userToken IsNot Nothing Then
            parameters.Add("user_token", userToken.ToString())
        End If

        If guest IsNot Nothing Then
            parameters.Add("guest", guest.ToString())
        End If

        If betterThan.HasValue Then
            parameters.Add("better_than", betterThan.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If worseThan.HasValue Then
            parameters.Add("worse_than", worseThan.Value.ToString(CultureInfo.InvariantCulture))
        End If

        _requests.Add(("/scores/", parameters))
        Return Me
    End Function

    Public Function FetchScoreTable() As GameJoltBatchAPIBuilder
        _requests.Add(("/scores/tables/", New SortedDictionary(Of String,String)()))
        Return Me
    End Function

    Public Function AddScore(score As String, sortValue As Integer,
                             Optional username As String = Nothing,
                             Optional userToken As String = Nothing,
                             Optional guest As String = Nothing,
                             Optional extraData As String = Nothing,
                             Optional tableId As Integer? = Nothing) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("score", score)
        parameters.Add("sort", sortValue.ToString(CultureInfo.InvariantCulture))

        If username IsNot Nothing Then
            parameters.Add("username", username.ToString())
        End If

        If userToken IsNot Nothing Then
            parameters.Add("user_token", userToken.ToString())
        End If

        If guest IsNot Nothing Then
            parameters.Add("guest", guest.ToString())
        End If

        If extraData IsNot Nothing Then
            parameters.Add("extra_data", extraData.ToString())
        End If

        If tableId.HasValue Then
            parameters.Add("table_id", tableId.Value.ToString(CultureInfo.InvariantCulture))
        End If

        _requests.Add(("/scores/add/", parameters))
        Return Me
    End Function

    Public Function GetRank(sortValue As Integer, Optional tableId As Integer? = Nothing) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("sort", sortValue)

        If tableId.HasValue Then
            parameters.Add("table_id", tableId.Value.ToString(CultureInfo.InvariantCulture))
        End If

        _requests.Add(("/scores/get-rank/", parameters))
        Return Me
    End Function

#End Region

#Region "Trophies"

    Public Function FetchTrophies(username As String, userToken As String,
                                  Optional achieved As Boolean? = Nothing,
                                  Optional trophyId As Integer? = Nothing) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        If achieved.HasValue Then
            parameters.Add("achieved", achieved.Value.ToString().ToLowerInvariant())
        End If

        If trophyId.HasValue Then
            parameters.Add("trophy_id", trophyId.Value.ToString(CultureInfo.InvariantCulture))
        End If

        _requests.Add(("/trophies/", parameters))
        Return Me
    End Function

    Public Function FetchTrophies(username As String, userToken As String,
                                  Optional achieved As Boolean? = Nothing,
                                  Optional trophyIds As Integer() = Nothing) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        If achieved.HasValue Then
            parameters.Add("achieved", achieved.Value.ToString().ToLowerInvariant())
        End If

        If trophyIds IsNot Nothing Then
            parameters.Add("trophy_id", String.Join(",", trophyIds.Select(Function(i) i.ToString(CultureInfo.InvariantCulture))))
        End If

        _requests.Add(("/trophies/", parameters))
        Return Me
    End Function

    Public Function AddAchievedTrophy(username As String, userToken As String, trophyId As Integer) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)
        parameters.Add("trophy_id", trophyId.ToString(CultureInfo.InvariantCulture))

        _requests.Add(("/trophies/add-achieved/", parameters))
        Return Me
    End Function

    Public Function RemoveAchievedTrophy(username As String, userToken As String, trophyId As Integer) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)
        parameters.Add("trophy_id", trophyId.ToString(CultureInfo.InvariantCulture))

        _requests.Add(("/trophies/remove-achieved/", parameters))
        Return Me
    End Function

#End Region

#Region "Data Store"

#End Region

#Region "Friends"

    Public Function FetchFriends(username As String, userToken As String) As GameJoltBatchAPIBuilder
        Dim parameters = New SortedDictionary(Of String, String)()
        parameters.Add("username", username)
        parameters.Add("user_token", userToken)

        _requests.Add(("/friends/", parameters))
        Return Me
    End Function

#End Region

#Region "Time"

    Public Function FetchTime() As GameJoltBatchAPIBuilder
        _requests.Add(("/time/", New SortedDictionary(Of String, String)()))
        Return Me
    End Function

#End Region
End Class