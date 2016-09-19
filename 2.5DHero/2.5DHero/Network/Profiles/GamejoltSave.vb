Namespace GameJolt

    Public Class GamejoltSave

        Inherits Security.HashSecureBase

        'Offset in Fields array:
        Private Const ID_APRICORNS As Integer = 0
        Private Const ID_BERRIES As Integer = 1
        Private Const ID_BOX As Integer = 2
        Private Const ID_DAYCARE As Integer = 3
        Private Const ID_ITEMDATA As Integer = 4
        Private Const ID_ITEMS As Integer = 5
        Private Const ID_NPC As Integer = 6
        Private Const ID_OPTIONS As Integer = 7
        Private Const ID_PARTY As Integer = 8
        Private Const ID_PLAYER As Integer = 9
        Private Const ID_POKEDEX As Integer = 10
        Private Const ID_REGISTER As Integer = 11
        Private Const ID_HALLOFFAME As Integer = 12
        Private Const ID_SECRETBASE As Integer = 13
        Private Const ID_ROAMINGPOKEMON As Integer = 14
        Private Const ID_STATISTICS As Integer = 15
        'Extra, SAVEFILECOUNT will get added.
        Private Const ID_PLAYERPOINTS As Integer = 0
        Private Const ID_PLAYEREMBLEM As Integer = 1
        Private Const ID_PLAYERGENDER As Integer = 2
        Private Const ID_FRIENDS As Integer = 3
        Private Const ID_SENTREQUESTS As Integer = 4
        Private Const ID_RECEIVEDREQUESTS As Integer = 5

        'The amount of files to download.
        'SAVEFILEs represent the amount of files that would get stored in an offline save.
        'EXTRADATA is stuff that does get saved extra and that offline profiles don't have, like global points, friends etc.
        Public Const SAVEFILECOUNT As Integer = 16
        Public Const EXTRADATADOWNLOADCOUNT As Integer = 6
        Public Const EXTRADATAUPLOADCOUNT As Integer = 3

        'The current version of the save files.
        'WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING
        '   Changing this will break all current online saves!
        'WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING
        Public Const VERSION As String = "1"

        ''' <summary>
        ''' Apricorn data
        ''' </summary>
        Public ReadOnly Property Apricorns() As String
            Get
                Return Me._apricorns
            End Get
        End Property

        ''' <summary>
        ''' Berry data
        ''' </summary>
        Public ReadOnly Property Berries() As String
            Get
                Return Me._berries
            End Get
        End Property

        ''' <summary>
        ''' Box data
        ''' </summary>
        Public ReadOnly Property Box() As String
            Get
                Return Me._box
            End Get
        End Property

        ''' <summary>
        ''' Daycare data
        ''' </summary>
        Public ReadOnly Property Daycare() As String
            Get
                Return Me._daycare
            End Get
        End Property

        ''' <summary>
        ''' ItemData data
        ''' </summary>
        Public ReadOnly Property ItemData() As String
            Get
                Return Me._itemData
            End Get
        End Property

        ''' <summary>
        ''' Item data
        ''' </summary>
        Public ReadOnly Property Items() As String
            Get
                Return Me._items
            End Get
        End Property

        ''' <summary>
        ''' NPC data
        ''' </summary>
        Public ReadOnly Property NPC() As String
            Get
                Return Me._NPC
            End Get
        End Property

        ''' <summary>
        ''' Option data
        ''' </summary>
        Public ReadOnly Property Options() As String
            Get
                Return Me._options
            End Get
        End Property

        ''' <summary>
        ''' Party data
        ''' </summary>
        Public ReadOnly Property Party() As String
            Get
                Return Me._party
            End Get
        End Property

        ''' <summary>
        ''' Player data
        ''' </summary>
        Public ReadOnly Property Player() As String
            Get
                Return Me._player
            End Get
        End Property

        ''' <summary>
        ''' Pokedex data
        ''' </summary>
        Public ReadOnly Property Pokedex() As String
            Get
                Return Me._pokedex
            End Get
        End Property

        ''' <summary>
        ''' Register data
        ''' </summary>
        Public ReadOnly Property Register() As String
            Get
                Return Me._register
            End Get
        End Property

        ''' <summary>
        ''' HallOfFame data
        ''' </summary>
        Public ReadOnly Property HallOfFame() As String
            Get
                Return Me._hallOfFame
            End Get
        End Property

        ''' <summary>
        ''' SecretBase data
        ''' </summary>
        Public ReadOnly Property SecretBase() As String
            Get
                Return Me._secretBase
            End Get
        End Property

        ''' <summary>
        ''' RoamingPokemon data
        ''' </summary>
        Public ReadOnly Property RoamingPokemon() As String
            Get
                Return Me._roamingPokemon
            End Get
        End Property

        ''' <summary>
        ''' Statistics data
        ''' </summary>
        Public ReadOnly Property Statistics() As String
            Get
                Return Me._statistics
            End Get
        End Property

        Private _apricorns As String = ""
        Private _berries As String = ""
        Private _box As String = ""
        Private _daycare As String = ""
        Private _itemData As String = ""
        Private _items As String = ""
        Private _NPC As String = ""
        Private _options As String = ""
        Private _party As String = ""
        Private _player As String = ""
        Private _pokedex As String = ""
        Private _register As String = ""
        Private _hallOfFame As String = ""
        Private _secretBase As String = ""
        Private _roamingPokemon As String = ""
        Private _statistics As String = ""

        Public AchievedEmblems As New List(Of String)
        Public DownloadedSprite As Texture2D = Nothing

        Public Friends As String = ""
        Public SentRequests As String = ""
        Public ReceivedRequests As String = ""

        Public Emblem As String = "trainer"

        Public Property Points() As Integer
            Get
                Assert("_points", _points)
                Return _points
            End Get
            Set(value As Integer)
                Assert("_points", _points, value)
                _points = value
            End Set
        End Property

        Public Property Gender() As String
            Get
                Assert("_gender", _gender)
                Return _gender
            End Get
            Set(value As String)
                Assert("_gender", _gender, value)
                _gender = value
            End Set
        End Property

        Public ReadOnly Property GameJoltID() As String
            Get
                Return _gameJoltID
            End Get
        End Property

        Private _points As Integer = 0
        Private _gender As String = "0"
        Private _gameJoltID As String = ""

        Private _downloadedFlags As New List(Of Boolean)
        Private _downloadFailed As Boolean = False

        ''' <summary>
        ''' Indicates if the download finished.
        ''' </summary>
        Public ReadOnly Property DownloadFinished() As Boolean
            Get
                Return DownloadProgress() = TotalDownloadItems()
            End Get
        End Property

        ''' <summary>
        ''' Returns the amount of downloaded items.
        ''' </summary>
        Public ReadOnly Property DownloadProgress() As Integer
            Get
                Dim c As Integer = 0
                For i = 0 To _downloadedFlags.Count - 1
                    If i <= _downloadedFlags.Count - 1 Then
                        Dim b As Boolean = _downloadedFlags(i)
                        If b = True Then
                            c += 1
                        End If
                    End If
                Next
                Return c
            End Get
        End Property

        ''' <summary>
        ''' The total files to download from the server.
        ''' </summary>
        Public ReadOnly Property TotalDownloadItems() As Integer
            Get
                Return SAVEFILECOUNT + EXTRADATADOWNLOADCOUNT
            End Get
        End Property

        ''' <summary>
        ''' If the download of this save failed at some point.
        ''' </summary>
        Public ReadOnly Property DownloadFailed() As Boolean
            Get
                Return Me._downloadFailed
            End Get
        End Property

        ''' <summary>
        ''' Handles the download failing.
        ''' </summary>
        ''' <param name="ex">The exception getting thrown.</param>
        Private Sub DownloadFailedHandler(ByVal ex As Exception)
            Me._downloadFailed = True

            Logger.Log(Logger.LogTypes.Warning, "The download of a GameJolt save failed with this message: " & ex.Message)
        End Sub

        ''' <summary>
        ''' Starts to download a GameJolt save.
        ''' </summary>
        ''' <param name="GameJoltID">The GameJolt ID of the save to download.</param>
        ''' <param name="MainSave">If this save is a main save download. Disable this flag to also download additional data.</param>
        Public Sub DownloadSave(ByVal GameJoltID As String, ByVal MainSave As Boolean)
            Me._gameJoltID = GameJoltID
            Me._downloadFailed = False

            'Fill fields to contain as many items as items to download.
            _downloadedFlags.Clear()
            For i = 1 To SAVEFILECOUNT + EXTRADATADOWNLOADCOUNT
                _downloadedFlags.Add(False)
            Next

            AchievedEmblems.Clear()

            _apricorns = ""
            _berries = ""
            _box = ""
            _daycare = ""
            _itemData = ""
            _items = ""
            _NPC = ""
            _options = ""
            _party = ""
            _player = ""
            _pokedex = ""
            _register = ""
            _hallOfFame = ""
            _secretBase = ""
            _roamingPokemon = ""
            _statistics = ""

            Friends = ""
            If MainSave = True Then
                GTSSetupScreen.GTSEditTradeScreen.SelectFriendScreen.Clear() 'Clear temp friends
            End If

            Dim APIPublicCall As New APICall(AddressOf GotPublicKeys)
            APIPublicCall.GetKeys(False, "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|*")

            Dim APIPrivateCall As New APICall(AddressOf GotPrivateKeys)
            APIPrivateCall.GetKeys(True, "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|*")

            If MainSave = True Then
                GameJolt.Emblem.GetAchievedEmblems()
            End If

            Dim APIFriendsCall As New APICall(AddressOf SaveFriends)
            APIFriendsCall.FetchFriendList(GameJoltID)

            Dim APISentCall As New APICall(AddressOf SaveSentRequests)
            APISentCall.FetchSentFriendRequest()

            Dim APIReceivedCall As New APICall(AddressOf SaveReceivedRequests)
            APIReceivedCall.FetchReceivedFriendRequests()

            If MainSave = True Then
                Dim t As New Threading.Thread(AddressOf DownloadSpriteSub)
                t.IsBackground = True
                t.Start()
            End If
        End Sub

        Private Sub DownloadSpriteSub()
            DownloadedSprite = GameJolt.Emblem.GetOnlineSprite(GameJoltID)
        End Sub

        Private Sub GotPublicKeys(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            Dim existsPoints As Boolean = False
            Dim existsEmblem As Boolean = False
            Dim existsGender As Boolean = False

            For Each Item As API.JoltValue In list
                If Item.Value = "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|points" Then
                    Dim APICall As New APICall(AddressOf SavePlayerPoints)
                    AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                    APICall.GetStorageData(Item.Value, False)

                    existsPoints = True
                End If
                If Item.Value = "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|emblem" Then
                    Dim APICall As New APICall(AddressOf SavePlayerEmblem)
                    AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                    APICall.GetStorageData(Item.Value, False)

                    existsEmblem = True
                End If
                If Item.Value = "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|gender" Then
                    Dim APICall As New APICall(AddressOf SavePlayerGender)
                    AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                    APICall.GetStorageData(Item.Value, False)

                    existsGender = True
                End If
            Next

            If existsPoints = False Then
                Points = 0
                _downloadedFlags(SAVEFILECOUNT + ID_PLAYERPOINTS) = True

                UpdatePlayerScore()
            End If
            If existsEmblem = False Then
                Emblem = "trainer"
                _downloadedFlags(SAVEFILECOUNT + ID_PLAYEREMBLEM) = True
            End If
            If existsGender = False Then
                Gender = "0"
                _downloadedFlags(SAVEFILECOUNT + ID_PLAYERGENDER) = True
            End If
        End Sub

        Public Sub UpdatePlayerScore()
            Dim APICall As New APICall()

            APICall.AddScore(Points & " Points", Points, "14908")
        End Sub

        Private Sub GotPrivateKeys(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            Dim exists() As Boolean = {}
            For i = 0 To SAVEFILECOUNT - 1
                Dim l As List(Of Boolean) = exists.ToList()
                l.Add(False)
                exists = l.ToArray()
            Next

            For Each Item As API.JoltValue In list
                Select Case Item.Value
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|apricorns"
                        Dim APICall As New APICall(AddressOf SaveApricorns)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_APRICORNS) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|berries"
                        Dim APICall As New APICall(AddressOf SaveBerries)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_BERRIES) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|box"
                        Dim APICall As New APICall(AddressOf SaveBox)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_BOX) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|daycare"
                        Dim APICall As New APICall(AddressOf SaveDaycare)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_DAYCARE) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|itemdata"
                        Dim APICall As New APICall(AddressOf SaveItemData)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_ITEMDATA) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|items"
                        Dim APICall As New APICall(AddressOf SaveItems)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_ITEMS) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|npc"
                        Dim APICall As New APICall(AddressOf SaveNPC)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_NPC) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|options"
                        Dim APICall As New APICall(AddressOf SaveOptions)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_OPTIONS) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|party"
                        Dim APICall As New APICall(AddressOf SaveParty)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_PARTY) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|player"
                        Dim APICall As New APICall(AddressOf SavePlayer)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_PLAYER) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|pokedex"
                        Dim APICall As New APICall(AddressOf SavePokedex)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_POKEDEX) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|register"
                        Dim APICall As New APICall(AddressOf SaveRegister)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_REGISTER) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|halloffame"
                        Dim APICall As New APICall(AddressOf SaveHallOfFame)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_HALLOFFAME) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|secretbase"
                        Dim APICall As New APICall(AddressOf SaveSecretBase)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_SECRETBASE) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|roamingpokemon"
                        Dim APICall As New APICall(AddressOf SaveRoamingPokemon)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_ROAMINGPOKEMON) = True
                    Case "saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltID & "|statistics"
                        Dim APICall As New APICall(AddressOf SaveStatistics)
                        AddHandler APICall.CallFails, AddressOf DownloadFailedHandler
                        APICall.GetStorageData(Item.Value, True)

                        exists(ID_STATISTICS) = True
                End Select
            Next

            If exists(ID_BERRIES) = False Then
                _berries = NewGameScreen.GetBerryData()
            End If
            If exists(ID_OPTIONS) = False Then
                _options = NewGameScreen.GetOptionsData()
            End If
            If exists(ID_PLAYER) = False Then
                _player = GetPlayerData()
            End If

            For i = 0 To SAVEFILECOUNT - 1
                If exists(i) = False Then
                    _downloadedFlags(i) = True
                End If
            Next
        End Sub

        Private Sub SavePlayerPoints(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Points = CInt(data.Replace("\""", """"))
            Else
                Points = 0
            End If

            UpdatePlayerScore()

            _downloadedFlags(SAVEFILECOUNT + ID_PLAYERPOINTS) = True
        End Sub

        Private Sub SavePlayerEmblem(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Emblem = data.Replace("\""", """")
            Else
                Emblem = "trainer"
            End If

            _downloadedFlags(SAVEFILECOUNT + ID_PLAYEREMBLEM) = True
        End Sub

        Private Sub SavePlayerGender(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Gender = data.Replace("\""", """")
            Else
                Gender = "0"
            End If

            _downloadedFlags(SAVEFILECOUNT + ID_PLAYERGENDER) = True
        End Sub

        Private Sub SaveApricorns(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _apricorns = data.Replace("\""", """")
            Else
                _apricorns = ""
            End If

            _downloadedFlags(ID_APRICORNS) = True
        End Sub

        Private Sub SaveBerries(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _berries = data.Replace("\""", """")
            Else
                _berries = NewGameScreen.GetBerryData()
            End If

            _downloadedFlags(ID_BERRIES) = True
        End Sub

        Private Sub SaveBox(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _box = data.Replace("\""", """")
            Else
                _box = ""
            End If

            _downloadedFlags(ID_BOX) = True
        End Sub

        Private Sub SaveDaycare(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _daycare = data.Replace("\""", """")
            Else
                _daycare = ""
            End If

            _downloadedFlags(ID_DAYCARE) = True
        End Sub

        Private Sub SaveItemData(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _itemData = data.Replace("\""", """")
            Else
                _itemData = ""
            End If

            _downloadedFlags(ID_ITEMDATA) = True
        End Sub

        Private Sub SaveItems(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _items = data.Replace("\""", """")
            Else
                _items = ""
            End If

            _downloadedFlags(ID_ITEMS) = True
        End Sub

        Private Sub SaveNPC(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _NPC = data.Replace("\""", """")
            Else
                _NPC = ""
            End If

            _downloadedFlags(ID_NPC) = True
        End Sub

        Private Sub SaveOptions(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _options = data.Replace("\""", """")
            Else
                _options = NewGameScreen.GetOptionsData()
            End If

            _downloadedFlags(ID_OPTIONS) = True
        End Sub

        Private Sub SaveParty(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _party = data.Replace("\""", """")
            Else
                _party = ""
            End If

            _downloadedFlags(ID_PARTY) = True
        End Sub

        Private Sub SavePlayer(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _player = data.Replace("\""", """")
            Else
                _player = GetPlayerData()
            End If

            _downloadedFlags(ID_PLAYER) = True
        End Sub

        Private Sub SavePokedex(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _pokedex = data.Replace("\""", """")
            Else
                _pokedex = ""
            End If

            _downloadedFlags(ID_POKEDEX) = True
        End Sub

        Private Sub SaveRegister(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _register = data.Replace("\""", """")
            Else
                _register = ""
            End If

            _downloadedFlags(ID_REGISTER) = True
        End Sub

        Private Sub SaveHallOfFame(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _hallOfFame = data.Replace("\""", """")
            Else
                _hallOfFame = ""
            End If

            _downloadedFlags(ID_HALLOFFAME) = True
        End Sub

        Private Sub SaveSecretBase(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _secretBase = data.Replace("\""", """")
            Else
                _secretBase = ""
            End If

            _downloadedFlags(ID_SECRETBASE) = True
        End Sub

        Private Sub SaveRoamingPokemon(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _roamingPokemon = data.Replace("\""", """")
            Else
                _roamingPokemon = ""
            End If

            _downloadedFlags(ID_ROAMINGPOKEMON) = True
        End Sub

        Private Sub SaveStatistics(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                _statistics = data.Replace("\""", """")
            Else
                _statistics = ""
            End If

            _downloadedFlags(ID_STATISTICS) = True
        End Sub

        Private Sub SaveFriends(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list.Count > 1 Then
                Friends = ""

                For Each Item As API.JoltValue In list
                    If Item.Name = "friend_id" Then
                        If Friends <> "" Then
                            Friends &= ","
                        End If
                        Friends &= Item.Value
                    End If
                Next
            Else
                Friends = ""
            End If

            _downloadedFlags(SAVEFILECOUNT + ID_FRIENDS) = True
        End Sub

        Private Sub SaveSentRequests(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list.Count > 1 Then
                SentRequests = ""

                For Each Item As API.JoltValue In list
                    If Item.Name = "user_id" Then
                        If SentRequests <> "" Then
                            SentRequests &= ","
                        End If
                        SentRequests &= Item.Value
                    End If
                Next
            Else
                SentRequests = ""
            End If

            _downloadedFlags(SAVEFILECOUNT + ID_SENTREQUESTS) = True
        End Sub

        Private Sub SaveReceivedRequests(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If list.Count > 1 Then
                ReceivedRequests = ""

                For Each Item As API.JoltValue In list
                    If Item.Name = "user_id" Then
                        If ReceivedRequests <> "" Then
                            ReceivedRequests &= ","
                        End If
                        ReceivedRequests &= Item.Value
                    End If
                Next
            Else
                ReceivedRequests = ""
            End If

            _downloadedFlags(SAVEFILECOUNT + ID_RECEIVEDREQUESTS) = True
        End Sub

#Region "DefaultData"

        Private Function GetPlayerData() As String
            Dim GameMode As GameMode = GameModeManager.ActiveGameMode

            Dim ot As String = GameJoltID
            While ot.Length < 5
                ot = "0" & ot
            End While

            Dim s As String = "Name|" & GameJolt.API.username & vbNewLine &
                "Position|1,0.1,3" & vbNewLine &
                "MapFile|yourroom.dat" & vbNewLine &
                "Rotation|1.570796" & vbNewLine &
                "RivalName|???" & vbNewLine &
                "Money|3000" & vbNewLine &
                "Badges|0" & vbNewLine &
                "Gender|Male" & vbNewLine &
                "PlayTime|0,0,0" & vbNewLine &
                "OT|" & ot & vbNewLine &
                "Points|0" & vbNewLine &
                "hasPokedex|0" & vbNewLine &
                "hasPokegear|0" & vbNewLine &
                "freeCamera|1" & vbNewLine &
                "thirdPerson|0" & vbNewLine &
                "skin|" & GameJolt.Emblem.GetPlayerSpriteFile(1, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Gender) & vbNewLine &
                "location|Your Room" & vbNewLine &
                "battleAnimations|2" & vbNewLine &
                "BoxAmount|5" & vbNewLine &
                "LastRestPlace|yourroom.dat" & vbNewLine &
                "LastRestPlacePosition|1,0.1,3" & vbNewLine &
                "DiagonalMovement|0" & vbNewLine &
                "RepelSteps|0" & vbNewLine &
                "LastSavePlace|yourroom.dat" & vbNewLine &
                "LastSavePlacePosition|1,0.1,3" & vbNewLine &
                "Difficulty|" & GameModeManager.GetGameRuleValue("Difficulty", "0") & vbNewLine &
                "BattleStyle|0" & vbNewLine &
                "saveCreated|" & GameController.GAMEDEVELOPMENTSTAGE & " " & GameController.GAMEVERSION & vbNewLine &
                "LastPokemonPosition|999,999,999" & vbNewLine &
                "DaycareSteps|0" & vbNewLine &
                "GameMode|Kolben" & vbNewLine &
                "PokeFiles|" & vbNewLine &
                "VisitedMaps|yourroom.dat" & vbNewLine &
                "TempSurfSkin|Hilbert" & vbNewLine &
                "Surfing|0" & vbNewLine &
                "ShowModels|1" & vbNewLine &
                "GTSStars|4" & vbNewLine &
                "SandBoxMode|0" & vbNewLine &
                "EarnedAchievements|"

            Return s
        End Function

#End Region

        Public Sub ResetSave()
            Points = 0
            Emblem = "trainer"
            Gender = "0"

            _apricorns = ""
            _berries = NewGameScreen.GetBerryData()
            _box = ""
            _daycare = ""
            _itemData = ""
            _items = ""
            _NPC = ""
            _options = NewGameScreen.GetOptionsData()
            _party = ""
            _player = GetPlayerData()
            _pokedex = ""
            _register = ""
            _hallOfFame = ""
            _secretBase = ""
            _roamingPokemon = ""
            _statistics = ""
        End Sub

    End Class

    Public Class StaffProfile

        Public Shared Staff As New List(Of StaffProfile)

        Public Shared Sub SetupStaff()
            Staff.Add(New StaffProfile("17441", "Creator", "nilllzz", {StaffArea.GlobalAdmin, StaffArea.GTSAdmin, StaffArea.GTSDaily, StaffArea.MailManagement}))
            Staff.Add(New StaffProfile("32943", "Programmator", "dracohouston", {StaffArea.GlobalAdmin}))
            Staff.Add(New StaffProfile("32349", "Dark", "darkfire", {StaffArea.GlobalAdmin, StaffArea.GTSAdmin, StaffArea.GTSDaily, StaffArea.MailManagement}))
            Staff.Add(New StaffProfile("33742", "Prince", "princevade", {StaffArea.GlobalAdmin, StaffArea.GTSAdmin, StaffArea.GTSDaily, StaffArea.MailManagement}))
            Staff.Add(New StaffProfile("1", "GameJolt", "cros", {}))
            Staff.Add(New StaffProfile("35947", "", "", {StaffArea.GTSDaily}))
        End Sub

        Public Enum StaffArea
            GTSAdmin
            GTSDaily
            MailManagement
            GlobalAdmin
        End Enum

        Public StaffAreas As New List(Of StaffArea)
        Public RankName As String = ""
        Public Sprite As String = ""
        Public GameJoltID As String = ""

        Public Sub New(ByVal GameJoltID As String, ByVal RankName As String, ByVal Sprite As String, ByVal StaffAreas() As StaffArea)
            Me.RankName = RankName
            Me.GameJoltID = GameJoltID
            Me.Sprite = Sprite
            Me.StaffAreas = StaffAreas.ToList()
        End Sub

    End Class

End Namespace
