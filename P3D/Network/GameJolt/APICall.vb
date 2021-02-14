Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography

Namespace GameJolt

    Public Class APICall

        Public Structure JoltParameter
            Dim Name As String
            Dim Value As String
        End Structure

        Public Enum RequestMethod
            [GET]
            POST
        End Enum

        Private Class APIURL

            Dim Values As New Dictionary(Of String, String)
            Dim BaseURL As String = ""

            Public Sub New(ByVal baseURL As String)
                Me.BaseURL = baseURL

                If Me.BaseURL.StartsWith("/") = False Then
                    Me.BaseURL = "/" & baseURL
                End If
                If Me.BaseURL.EndsWith("/") = False Then
                    Me.BaseURL &= "/"
                End If
            End Sub

            Public Sub AddKeyValuePair(ByVal Key As String, ByVal Value As String)
                Me.Values.Add(Key, Value)
            End Sub

            Public ReadOnly Property GetURL() As String
                Get
                    Dim url As String = HOST & API.API_VERSION & Me.BaseURL

                    For i = 0 To Me.Values.Count - 1
                        Dim appendString As String = ""
                        If i = 0 Then
                            appendString &= "?"
                        Else
                            appendString &= "&"
                        End If
                        appendString &= Me.Values.Keys(i) & "="

                        appendString &= UrlEncoder.Encode(Me.Values.Values(i))

                        url &= appendString
                    Next

                    Return url
                End Get
            End Property

        End Class

        Public Delegate Sub DelegateCallSub(ByVal result As String)

        Public CallSub As DelegateCallSub

        Dim username As String
        Dim token As String

        Dim loggedIn As Boolean

        Const CONST_GAMEID As String = Classified.GameJolt_Game_ID
        Const CONST_GAMEKEY As String = Classified.GameJolt_Game_Key
        Const HOST As String = "http://api.gamejolt.com/api/game/"

        Dim Exception As System.Exception = Nothing

        Public Event CallFails(ByVal ex As Exception)
        Public Event CallSucceeded(ByVal returnData As String)

        Private ReadOnly Property GameID() As String
            Get
                Return StringObfuscation.DeObfuscate(CONST_GAMEID)
            End Get
        End Property

        Private ReadOnly Property GameKey() As String
            Get
                Return StringObfuscation.DeObfuscate(CONST_GAMEKEY)
            End Get
        End Property

        Public Sub New(ByVal CallSub As DelegateCallSub)
            Me.CallSub = CallSub

            Me.username = API.username
            Me.token = API.token

            Me.loggedIn = API.LoggedIn
        End Sub

        Public Sub New()
            Me.username = API.username
            Me.token = API.token

            Me.loggedIn = API.LoggedIn
        End Sub

        Public Sub VerifyUser(ByVal newUsername As String, ByVal newToken As String)
            API.username = newUsername
            API.token = newToken
            username = newUsername
            token = newToken

            Dim url As New APIURL("/users/auth/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

#Region "Storage"

        Public Sub SetStorageData(ByVal key As String, ByVal data As String, ByVal useUsername As Boolean)
            If useUsername = True Then
                If loggedIn = False Then
                    Dim up As New Exception("User not logged in!") 'Happens when a user tries to send an API call but is not logged in.

                    Throw up
                Else
                    Dim url As New APIURL("/data-store/set/")
                    url.AddKeyValuePair("game_id", GameID)
                    url.AddKeyValuePair("username", username)
                    url.AddKeyValuePair("user_token", token)
                    url.AddKeyValuePair("key", key)

                    Initialize(url.GetURL(), RequestMethod.POST, data)
                End If
            Else
                Dim url As New APIURL("/data-store/set/")
                url.AddKeyValuePair("game_id", GameID)
                url.AddKeyValuePair("key", key)

                Initialize(url.GetURL(), RequestMethod.POST, data)
            End If
        End Sub

        Public Sub UpdateStorageData(ByVal key As String, ByVal value As String, ByVal operation As String, ByVal useUsername As Boolean)
            If useUsername = True Then
                If loggedIn = False Then
                    Dim up As New Exception("User not logged in!") 'Happens when a user tries to send an API call but is not logged in.

                    Throw up
                Else
                    Dim url As New APIURL("/data-store/update/")
                    url.AddKeyValuePair("game_id", GameID)
                    url.AddKeyValuePair("username", username)
                    url.AddKeyValuePair("user_token", token)
                    url.AddKeyValuePair("key", key)
                    url.AddKeyValuePair("operation", operation)
                    url.AddKeyValuePair("value", value)

                    Initialize(url.GetURL(), RequestMethod.GET)
                End If
            Else
                Dim url As New APIURL("/data-store/update/")
                url.AddKeyValuePair("game_id", GameID)
                url.AddKeyValuePair("key", key)
                url.AddKeyValuePair("operation", operation)
                url.AddKeyValuePair("value", value)

                Initialize(url.GetURL(), RequestMethod.GET)
            End If
        End Sub

        Public Sub SetStorageData(ByVal keys() As String, ByVal dataItems() As String, ByVal useUsernames() As Boolean)
            If keys.Length <> dataItems.Length Or keys.Length <> useUsernames.Length Then
                Dim ex As New Exception("The data arrays do not have the same lengths.")
                ex.Data.Add("Keys Length", keys.Length)
                ex.Data.Add("Data Length", dataItems.Length)
                ex.Data.Add("Username permission Length", useUsernames.Length)
                Throw ex
            End If

            Dim url As String = HOST & API.API_VERSION & "/batch/" & "?game_id=" & GameID & "&parallel=true"
            Dim postDataURL As String = ""

            For i = 0 To keys.Length - 1
                Dim key As String = keys(i)
                Dim data As String = dataItems(i)
                Dim useUsername As Boolean = useUsernames(i)

                If useUsername = True And loggedIn = False Then
                    Throw New Exception("User not logged in!")
                End If

                If useUsername = True Then
                    postDataURL &= "&requests[]=" & UrlEncoder.Encode(GetHashedURL("/data-store/set/" & "?game_id=" & GameID & "&username=" & username & "&user_token=" & token & "&key=" & UrlEncoder.Encode(key) & "&data=" & UrlEncoder.Encode(data)))
                Else
                    postDataURL &= "&requests[]=" & UrlEncoder.Encode(GetHashedURL("/data-store/set/" & "?game_id=" & GameID & "&key=" & UrlEncoder.Encode(key) & "&data=" & UrlEncoder.Encode(data)))
                End If
            Next

            Initialize(url, RequestMethod.POST, postDataURL)
        End Sub

        Public Sub SetStorageDataRestricted(ByVal key As String, ByVal data As String)
            Dim url As String = HOST & API.API_VERSION & "/data-store/set/" & "?game_id=" & GameID & "&key=" & key & "&restriction_username=" & API.username & "&restriction_user_token=" & API.token

            Initialize(url, RequestMethod.POST, data)
        End Sub

        Public Sub GetStorageData(ByVal key As String, ByVal useUsername As Boolean)
            If useUsername = True Then
                If loggedIn = False Then
                    Throw New Exception("User not logged in!")
                Else
                    Dim url As New APIURL("/data-store/")
                    url.AddKeyValuePair("game_id", GameID)
                    url.AddKeyValuePair("username", username)
                    url.AddKeyValuePair("user_token", token)
                    url.AddKeyValuePair("key", key)

                    Initialize(url.GetURL(), RequestMethod.GET)
                End If
            Else
                Dim url As New APIURL("/data-store/")
                url.AddKeyValuePair("game_id", GameID)
                url.AddKeyValuePair("key", key)

                Initialize(url.GetURL(), RequestMethod.GET)
            End If
        End Sub

        Public Sub GetStorageData(ByVal keys() As String, ByVal useUsername As Boolean)
            If useUsername = True Then
                If loggedIn = False Then
                    Throw New Exception("User not logged in!")
                Else
                    Dim url As String = HOST & API.API_VERSION & "/batch/"

                    Dim firstURL As Boolean = True

                    For Each key As String In keys
                        Dim keyURL As String = "?"
                        If firstURL = False Then
                            keyURL = "&"
                        End If

                        keyURL &= "requests[]=" & UrlEncoder.Encode(GetHashedURL(HOST & API.API_VERSION & "/data-store/" & "?game_id=" & GameID & "&username=" & username & "&user_token=" & token & "&key=" & key))

                        url &= keyURL

                        firstURL = False
                    Next

                    url &= "&game_id=" & GameID

                    Initialize(url, RequestMethod.GET)
                End If
            Else
                Dim url As String = HOST & API.API_VERSION & "/batch/"

                Dim firstURL As Boolean = True

                For Each key As String In keys
                    Dim keyURL As String = "?"
                    If firstURL = False Then
                        keyURL = "&"
                    End If

                    keyURL &= "requests[]=" & UrlEncoder.Encode(GetHashedURL(HOST & API.API_VERSION & "/data-store/" & "?game_id=" & GameID & "&key=" & key))

                    url &= keyURL

                    firstURL = False
                Next

                url &= "&game_id=" & GameID

                Initialize(url, RequestMethod.GET)
            End If
        End Sub

        Public Sub FetchUserdata(ByVal username As String)
            Dim url As New APIURL("/users/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub FetchUserdataByID(ByVal user_id As String)
            Dim url As New APIURL("/users/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("user_id", user_id)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub GetKeys(ByVal useUsername As Boolean, ByVal pattern As String)
            If useUsername = True Then
                If loggedIn = False Then
                    Throw New Exception("User not logged in!")
                Else
                    Dim url As New APIURL("/data-store/get-keys/")
                    url.AddKeyValuePair("game_id", GameID)
                    url.AddKeyValuePair("username", username)
                    url.AddKeyValuePair("user_token", token)
                    url.AddKeyValuePair("pattern", pattern)

                    Initialize(url.GetURL(), RequestMethod.GET)
                End If
            Else
                Dim url As New APIURL("/data-store/get-keys/")
                url.AddKeyValuePair("game_id", GameID)
                url.AddKeyValuePair("pattern", pattern)

                Initialize(url.GetURL(), RequestMethod.GET)
            End If
        End Sub

        Public Sub RemoveKey(ByVal key As String, ByVal useUsername As Boolean)
            If useUsername = True Then
                If loggedIn = False Then
                    Throw New Exception("User Not logged in!")
                Else
                    Dim url As New APIURL("/data-store/remove/")
                    url.AddKeyValuePair("game_id", GameID)
                    url.AddKeyValuePair("username", username)
                    url.AddKeyValuePair("user_token", token)
                    url.AddKeyValuePair("key", key)

                    Initialize(url.GetURL(), RequestMethod.POST)
                End If
            Else
                Dim url As New APIURL("/data-store/remove/")
                url.AddKeyValuePair("game_id", GameID)
                url.AddKeyValuePair("key", key)

                Initialize(url.GetURL(), RequestMethod.POST)
            End If
        End Sub

#End Region

#Region "Sessions"

        Public Sub OpenSession()
            Dim url As New APIURL("/sessions/open/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub CheckSession()
            Dim url As New APIURL("/sessions/ping/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub PingSession()
            Dim url As New APIURL("/sessions/ping/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub CloseSession()
            Dim url As New APIURL("/sessions/close/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

#End Region

#Region "Trophy"

        Public Sub FetchAllTrophies()
            Dim url As New APIURL("/trophies/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub FetchAllAchievedTrophies()
            Dim url As New APIURL("/trophies/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)
            url.AddKeyValuePair("achieved", "true")

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub FetchTrophy(ByVal trophy_id As Integer)
            Dim url As New APIURL("/trophies/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)
            url.AddKeyValuePair("trophy_id", trophy_id.ToString())

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub TrophyAchieved(ByVal trophy_id As Integer)
            Dim url As New APIURL("/trophies/add-achieved/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)
            url.AddKeyValuePair("trophy_id", trophy_id.ToString())

            Initialize(url.GetURL(), RequestMethod.POST)
        End Sub

        Public Sub RemoveTrophyAchieved(ByVal trophy_id As Integer)
            Dim url As New APIURL("/trophies/remove-achieved/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)
            url.AddKeyValuePair("trophy_id", trophy_id.ToString())

            Initialize(url.GetURL(), RequestMethod.POST)
        End Sub

#End Region

#Region "ScoreTable"

        Public Sub FetchTable(ByVal score_count As Integer, ByVal table_id As String)
            Dim url As New APIURL("/scores/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("limit", score_count.ToString())
            url.AddKeyValuePair("table_id", table_id)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub FetchUserRank(ByVal table_id As String, ByVal sort As Integer)
            Dim url As New APIURL("/scores/get-rank/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("sort", sort.ToString())
            url.AddKeyValuePair("table_id", table_id)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

        Public Sub AddScore(ByVal score As String, ByVal sort As Integer, ByVal table_id As String)
            Dim url As New APIURL("/scores/add/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)
            url.AddKeyValuePair("score", score)
            url.AddKeyValuePair("sort", sort.ToString())
            url.AddKeyValuePair("table_id", table_id)

            Initialize(url.GetURL(), RequestMethod.POST)
        End Sub

#End Region

#Region "Friends"

        Public Sub FetchFriendList(ByVal user_id As String)
            Dim url As New APIURL("/friends/")
            url.AddKeyValuePair("game_id", GameID)
            url.AddKeyValuePair("username", username)
            url.AddKeyValuePair("user_token", token)

            Initialize(url.GetURL(), RequestMethod.GET)
        End Sub

#End Region

        Private url As String = ""
        Private PostData As String = ""

        Private Function GetHashedURL(ByVal url As String) As String
            Dim m As MD5 = MD5.Create()

            Dim data() As Byte = m.ComputeHash(Encoding.UTF8.GetBytes(url & GameKey))

            Dim sBuild As New StringBuilder()

            For i = 0 To data.Length - 1
                sBuild.Append(data(i).ToString("x2"))
            Next

            Dim newurl As String = url & "&signature=" & sBuild.ToString()

            Return newurl
        End Function

        Private Sub Initialize(ByVal url As String, ByVal method As RequestMethod, Optional ByVal PostData As String = "")
            Exception = Nothing

            Dim newurl As String = GetHashedURL(url & "&format=keypair")

            Debug.Print(newurl) 'Intentional

            If method = RequestMethod.POST Then
                Me.url = newurl
                Me.PostData = PostData

                Dim t As New Threading.Thread(AddressOf POSTRequst)
                t.IsBackground = True
                t.Start()
            Else
                Try
                    Dim request As HttpWebRequest = CType(WebRequest.Create(newurl), HttpWebRequest)
                    request.Method = method.ToString()

                    request.BeginGetResponse(AddressOf EndResult, request)
                Catch ex As Exception
                    API.APICallCount -= 1
                    RaiseEvent CallFails(ex)
                End Try
            End If

            API.APICallCount += 1
        End Sub

        Private Sub POSTRequst()
            Dim gotData As String = ""
            Dim gotDataSuccess As Boolean = False

            Try
                Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                request.AllowWriteStreamBuffering = True
                request.Method = "POST"
                Dim post As String = "data=" & PostData
                request.ContentLength = post.Length
                request.ContentType = "application/x-www-form-urlencoded"
                request.ServicePoint.Expect100Continue = False

                Dim writer As StreamWriterLock = New StreamWriterLock(request.GetRequestStream())
                writer.Write(post)
                writer.Close()
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

                gotData = New StreamReader(response.GetResponseStream()).ReadToEnd()
                gotDataSuccess = True
            Catch ex As Exception
                RaiseEvent CallFails(ex)
            Finally
                API.APICallCount -= 1
            End Try

            'Handle data outside of the try...catch because the result function could throw an error:
            If gotDataSuccess = True Then
                If Not CallSub Is Nothing Then
                    CallSub(gotData)
                    RaiseEvent CallSucceeded(gotData)
                End If
            End If
        End Sub

        Private Sub EndResult(ByVal result As IAsyncResult)
            Dim data As String = ""

            Try
                If result.IsCompleted Then
                    Dim request As HttpWebRequest = CType(result.AsyncState, HttpWebRequest)

                    data = New StreamReader(request.EndGetResponse(result).GetResponseStream()).ReadToEnd()
                End If
            Catch ex As Exception
                RaiseEvent CallFails(ex)
            Finally
                API.APICallCount -= 1
            End Try

            'Handle data outside of the try...catch because the result function could throw an error:
            If data <> "" Then
                If Not CallSub Is Nothing Then
                    RaiseEvent CallSucceeded(data)
                    CallSub(data)
                End If
            End If
        End Sub

    End Class

End Namespace