Namespace GameJolt

    ''' <summary>
    ''' A class to handle emblem rendering and management.
    ''' </summary>
    Public Class Emblem

#Region "Enumrations"

        ''' <summary>
        ''' The names of male trainer types.
        ''' </summary>
        Public Enum MaleEmblemSpriteType
            Preschooler = 0
            SchoolKid = 1
            Youngster = 2
            Waiter = 3
            Backpacker = 4
            Pokefan = 5
            Butler = 6
            Cheerleader = 7
            Clerk = 8
            PokemonBreeder = 9
            Drummer = 10
            Cyclist = 11
            RichBoy = 12
            Ranger = 13
            Athlete = 14
            Scientist = 15
            Doctor = 16
            Gentleman = 17
            AceTrainer = 18
            Veteran = 19
        End Enum

        ''' <summary>
        ''' The names of female trainer types.
        ''' </summary>
        Public Enum FemaleEmblemSpriteType
            Preschooler = 0
            SchoolKid = 1
            Lass = 2
            Waitress = 3
            Backpacker = 4
            Pokefan = 5
            Maid = 6
            Cheerleader = 7
            Clerk = 8
            PokemonBreeder = 9
            Guitarist = 10
            Cyclist = 11
            Lady = 12
            Ranger = 13
            Athlete = 14
            Scientist = 15
            Nurse = 16
            Socialite = 17
            AceTrainer = 18
            Veteran = 19
        End Enum

        ''' <summary>
        ''' The sprites of male trainers.
        ''' </summary>
        Public Enum MaleEmblemSprites
            NN40 = 0
            NN42 = 1
            NN44 = 2
            NN46 = 3
            NN48 = 4
            NN51 = 5
            NN74 = 6
            NN76 = 7
            NN53 = 8
            NN55 = 9
            NN80 = 10
            NN72 = 11
            NN57 = 12
            NN59 = 13
            NN78 = 14
            NN64 = 15
            NN66 = 16
            NN68 = 17
            NN70 = 18
            NN38 = 19
        End Enum

        ''' <summary>
        ''' The sprites of female trainers.
        ''' </summary>
        Public Enum FemaleEmblemSprites
            NN41 = 0
            NN43 = 1
            NN45 = 2
            NN47 = 3
            NN50 = 4
            NN52 = 5
            NN75 = 6
            NN77 = 7
            NN54 = 8
            NN56 = 9
            NN81 = 10
            NN73 = 11
            NN58 = 12
            NN60 = 13
            NN79 = 14
            NN65 = 15
            NN67 = 16
            NN69 = 17
            NN71 = 18
            NN39 = 19
        End Enum

#End Region

        ''' <summary>
        ''' Renders an emblem to the screen.
        ''' </summary>
        ''' <param name="Name">The name of the player.</param>
        ''' <param name="ID">The GameJolt ID.</param>
        ''' <param name="Points">The points of the player.</param>
        ''' <param name="Gender">The gender of the player (0=male, 1=female).</param>
        ''' <param name="EmblemBackground">The emblem background name.</param>
        ''' <param name="Position">The position on the screen.</param>
        ''' <param name="Scale">The scale of the emblem.</param>
        ''' <param name="SpriteTexture">An alternative sprite to draw.</param>
        Public Shared Sub Draw(ByVal Name As String, ByVal ID As String, ByVal Points As Integer, ByVal Gender As String, ByVal EmblemBackground As String, ByVal Position As Vector2, ByVal Scale As Single, ByVal SpriteTexture As Texture2D)
            Draw(Name, ID, Points, Gender, EmblemBackground, Position, Scale, SpriteTexture, Nothing)
        End Sub

        ''' <summary>
        ''' Renders an emblem to the screen.
        ''' </summary>
        ''' <param name="Name">The name of the player.</param>
        ''' <param name="ID">The GameJolt ID.</param>
        ''' <param name="Points">The points of the player.</param>
        ''' <param name="Gender">The gender of the player (0=male, 1=female).</param>
        ''' <param name="EmblemBackground">The emblem background name.</param>
        ''' <param name="Position">The position on the screen.</param>
        ''' <param name="Scale">The scale of the emblem.</param>
        ''' <param name="SpriteTexture">An alternative sprite to draw.</param>
        ''' <param name="PokemonList">A list of 0-6 Pokémon to render below the player information.</param>
        Public Shared Sub Draw(ByVal Name As String, ByVal ID As String, ByVal Points As Integer, ByVal Gender As String, ByVal EmblemBackground As String, ByVal Position As Vector2, ByVal Scale As Single, ByVal SpriteTexture As Texture2D, ByVal PokemonList As List(Of Pokemon))
            'Generate OT:
            Dim OT As String = ID
            While OT.Length < 5
                OT = "0" & OT
            End While

            'Check if user is banned.
            Dim UserBanned As Boolean = LogInScreen.UserBanned(ID)

            Dim PlayerName As String = Name & " (" & OT & ")"

            Dim PlayerPoints As String = CStr(Points)
            Dim PlayerLevel As Integer = GetPlayerLevel(Points)

            Dim PlayerTexture As Texture2D = SpriteTexture
            If PlayerTexture Is Nothing Then
                PlayerTexture = GetPlayerSprite(PlayerLevel, ID, Gender)
            End If

            Dim frameSize As New Size(CInt(PlayerTexture.Width / 3), CInt(PlayerTexture.Height / 4))

            Dim PlayerTitle As String = GetPlayerTitle(PlayerLevel, ID, Gender)
            If UserBanned = True Then
                PlayerTitle = "Lonely"
            End If

            Dim EmblemBackgroundTexture As Texture2D = Nothing
            Dim EmblemFontColor As Color = GetEmblemFontColor(EmblemBackground)

            If UserBanned = True Then
                EmblemBackgroundTexture = GetEmblemBackgroundTexture("missingno")
                EmblemFontColor = Color.White
            Else
                EmblemBackgroundTexture = GetEmblemBackgroundTexture(EmblemBackground)
            End If

            Core.SpriteBatch.Draw(EmblemBackgroundTexture, New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(128 * Scale), CInt(32 * Scale)), Color.White)
            Core.SpriteBatch.Draw(PlayerTexture, New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(32 * Scale), CInt(32 * Scale)), New Rectangle(0, frameSize.Height * 2, frameSize.Width, frameSize.Height), Color.White)

            If PokemonList Is Nothing OrElse PokemonList.Count = 0 Then
                Core.SpriteBatch.DrawString(FontManager.MiniFont, PlayerName & vbNewLine & PlayerTitle & vbNewLine & vbNewLine & "Level: " & PlayerLevel & vbNewLine & "(Points: " & PlayerPoints & ")", New Vector2(32 * Scale + 10 + Position.X, 10 + Position.Y), EmblemFontColor, 0.0F, Vector2.Zero, CSng(Scale / 4), SpriteEffects.None, 0.0F)
            Else
                Core.SpriteBatch.DrawString(FontManager.MiniFont, PlayerName & vbNewLine & PlayerTitle & vbNewLine & "Level: " & PlayerLevel & vbNewLine & "(Points: " & PlayerPoints & ")", New Vector2(32 * Scale + 10 + Position.X, 6 + Position.Y), EmblemFontColor, 0.0F, Vector2.Zero, CSng(Scale / 4), SpriteEffects.None, 0.0F)

                For i = 0 To 5
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(192, 0, 32, 32), ""), New Rectangle(CInt(32 * Scale + (10 / 4) * Scale + Position.X + i * (10 * Scale)), CInt(Position.Y + 22.5F * Scale), CInt(Scale * 8), CInt(Scale * 8)), Color.White)

                    If PokemonList.Count - 1 >= i Then
                        Dim p As Pokemon = PokemonList(i)
                        Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(CInt(32 * Scale + (10 / 4) * Scale + Position.X + i * (10 * Scale)), CInt(Position.Y + 22.5F * Scale), CInt(Scale * 8), CInt(Scale * 8)), Color.White)
                    End If
                Next
            End If

            If UserBanned = False Then
                If IsFriend(ID) = True Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(80, 144, 32, 32), ""), New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(32 * CSng(Scale / 4)), CInt(32 * CSng(Scale / 4))), Color.White)
                End If
                If SentRequest(ID) = True Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(112, 176, 32, 32), ""), New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(32 * CSng(Scale / 4)), CInt(32 * CSng(Scale / 4))), Color.White)
                End If
                If ReceivedRequest(ID) = True Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(80, 176, 32, 32), ""), New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(32 * CSng(Scale / 4)), CInt(32 * CSng(Scale / 4))), Color.White)
                End If
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(144, 176, 32, 32), ""), New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(32 * CSng(Scale / 4)), CInt(32 * CSng(Scale / 4))), Color.White)
            End If
        End Sub

#Region "EmblemHelperFunctions"

        Public Shared Function GetPlayerLevel(ByVal playerPoints As Integer) As Integer
            Dim level As Integer = CInt(Math.Floor(xRoot(3, (4 / 5) * playerPoints * 10)))

            level = level.Clamp(1, 100)

            Return level
        End Function

        Public Shared Function GetPlayerTitle(ByVal level As Integer, ByVal id As String, ByVal gender As String) As String
            For Each staffMember As StaffProfile In StaffProfile.Staff
                If staffMember.GameJoltID.ToLower() = id.ToLower() And staffMember.RankName <> "" Then
                    Return staffMember.RankName
                End If
            Next

            Dim t As Integer = CInt(Math.Ceiling(level / 5)) - 1

            t = t.Clamp(0, 19)

            If gender = "1" Then
                Return CType(t, FemaleEmblemSpriteType).ToString()
            End If
            Return CType(t, MaleEmblemSpriteType).ToString()
        End Function

        Public Shared Function GetPlayerSpriteFile(ByVal level As Integer, ByVal id As String, ByVal gender As String) As String
            For Each staffMember As StaffProfile In StaffProfile.Staff
                If staffMember.GameJoltID.ToLower() = id.ToLower() And staffMember.Sprite <> "" Then
                    Return staffMember.Sprite
                End If
            Next

            Dim t As Integer = CInt(Math.Ceiling(level / 5)) - 1

            t = t.Clamp(0, 19)
            Dim tFile As String = CType(t, MaleEmblemSprites).ToString()
            If gender = "1" Then
                tFile = CType(t, FemaleEmblemSprites).ToString()
            End If
            If tFile.StartsWith("NN") = True Then
                tFile = tFile.Remove(0, 2)
            End If

            Return tFile
        End Function

        Public Shared Function GetPlayerSprite(ByVal level As Integer, ByVal id As String, ByVal gender As String) As Texture2D
            For Each staffMember As StaffProfile In StaffProfile.Staff
                If staffMember.GameJoltID.ToLower() = id.ToLower() And staffMember.Sprite <> "" Then
                    Return TextureManager.GetTexture("Textures\NPC\" & staffMember.Sprite)
                End If
            Next

            Dim t As Integer = CInt(Math.Ceiling(level / 5)) - 1

            t = t.Clamp(0, 19)
            Dim tFile As String = CType(t, MaleEmblemSprites).ToString()
            If gender = "1" Then
                tFile = CType(t, FemaleEmblemSprites).ToString()
            End If
            If tFile.StartsWith("NN") = True Then
                tFile = tFile.Remove(0, 2)
            End If

            Return TextureManager.GetTexture("Textures\NPC\" & tFile)
        End Function

        Private Shared Function GetEmblemBackgroundTexture(ByVal emblemName As String) As Texture2D
            'Don't load from TextureManager, because ContentPack emblems are not allowed.
            Return Content.Load(Of Texture2D)("Textures\Emblem\" & emblemName)
        End Function

        Public Shared Function GetEmblemFontColor(ByVal emblemName As String) As Color
            Select Case emblemName.ToLower()
                Case "alph", "genetics", "legendary", "stars", "champion", "overkill", "cyber", "glowing", "material", "fog", "mineral", "storm", "eggsplosion", "missingno", "thunder", "rainbow", "marsh", "volcano", "earth", "shooting star"
                    Return Color.White
                Case "eevee", "pokedex", "snow", "trainer", "kanto", "glacier", "hive", "plain", "zephyr", "rising", "mailman", "cascade", "boulder", "unodostres", "silver ability", "gold ability", "silver knowledge", "gold knowledge", "johto"
                    Return Color.Black
            End Select
            Return Color.White
        End Function

        Public Shared Function GetPointsForLevel(ByVal level As Integer) As Integer
            Dim points As Double = Math.Ceiling((5 / 4) * (Math.Pow(level, 3) / 10))
            Return CInt(points)
        End Function

#End Region

#Region "OnlineSprites"

        Shared TempDownloadedSprites As New Dictionary(Of String, Texture2D)

        Public Shared Function HasDownloadedSprite(ByVal GameJoltID As String) As Boolean
            Return TempDownloadedSprites.ContainsKey(GameJoltID)
        End Function

        Public Shared Function GetOnlineSprite(ByVal GameJoltID As String) As Texture2D
            If LogInScreen.UserBanned(GameJoltID) = True Then
                Return Nothing
            End If

            If TempDownloadedSprites.ContainsKey(GameJoltID) = True Then
                Dim tempT As Texture2D = TempDownloadedSprites(GameJoltID)
                Return tempT
            End If

            Dim t As Texture2D = DownloadTexture2D.n_Remote_Texture2D(Core.GraphicsDevice, "" & GameJoltID & ".png", False) ' CLASSIFIED

            If TempDownloadedSprites.ContainsKey(GameJoltID) = False Then
                TempDownloadedSprites.Add(GameJoltID, t)
            End If

            If Not t Is Nothing Then
                If t.Width >= 96 And t.Height >= 128 Then
                    If t.Width / 3 = t.Height / 4 Then
                        Return t
                    End If
                End If
            Else
                Logger.Debug("GetOnlineSprite.vb: Getting sprite for " & GameJoltID & " failed.")
            End If

            Return Nothing
        End Function

        Public Shared Sub ClearOnlineSpriteCache()
            Try
                TempDownloadedSprites.Clear()
            Catch : End Try
        End Sub

#End Region

#Region "UserEmblem"

        Public Emblem As String = "trainer"
        Public Points As Integer = 0
        Public Gender As String = "0"

        Public Username As String = ""
        Public GameJoltID As String = ""
        Public ValidProfile As Boolean = False
        Private loadedInstances As Integer = 0
        Public startedLoading As Boolean = False
        Public DownloadedSprite As Texture2D = Nothing
        Public OnlineTeam As List(Of Pokemon) = Nothing

        Public ReadOnly Property DoneLoading As Boolean
            Get
                If loadedInstances = 3 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property BackgroundTexture() As Texture2D
            Get
                Return GetEmblemBackgroundTexture(Emblem)
            End Get
        End Property

        Public ReadOnly Property SpriteTexture() As Texture2D
            Get
                If Not Me.DownloadedSprite Is Nothing Then
                    Return Me.DownloadedSprite
                End If
                Return GetPlayerSprite(GetPlayerLevel(Points), GameJoltID, Gender)
            End Get
        End Property

        Dim PublicKeys As String = ""

        Public Sub New(ByVal username As String)
            Me.StartLoading(username)
        End Sub

        Public Sub New(ByVal GameJoltID As String, ByVal unimportant As Integer)
            StartLoadingID(GameJoltID)
        End Sub

        Public Sub New(ByVal username As String, ByVal Keys As String)
            Me.New(username)
            PublicKeys = Keys
        End Sub

        Public Sub New(ByVal username As String, ByVal Keys As String, ByVal startLoading As Boolean)
            PublicKeys = Keys
            Me.Username = username
            If startLoading = True Then
                Me.StartLoading(username)
            End If
        End Sub

        Public Sub StartLoading(ByVal userName As String)
            Dim APICall As New APICall(AddressOf GotGameJoltID)
            APICall.FetchUserdata(Username)

            Me.Username = userName
            Me.startedLoading = True
        End Sub

        Public Sub StartLoadingID(ByVal GameJoltID As String)
            Me.GameJoltID = GameJoltID

            Dim APICall As New APICall(AddressOf GotGameJoltID)
            APICall.FetchUserdataByID(GameJoltID)

            Me.startedLoading = True
        End Sub

        Public Sub New(ByVal username As String, ByVal user_id As String, ByVal points As Integer, ByVal gender As String, ByVal emblem As String)
            Me.Username = username
            Me.GameJoltID = user_id
            Me.Points = points
            Me.Gender = gender
            Me.Emblem = emblem

            Dim t As Texture2D = GetOnlineSprite(Me.GameJoltID)
            DownloadedSprite = t

            loadedInstances = 3
            ValidProfile = True
        End Sub

        Private Sub GotGameJoltID(ByVal result As String)
            Dim list As List(Of GameJolt.API.JoltValue) = GameJolt.API.HandleData(result)
            Dim founduserid As Boolean = False

            For Each Item As GameJolt.API.JoltValue In list
                If Item.Name.ToLower() = "id" Then
                    Me.GameJoltID = Item.Value
                    ValidProfile = True

                    If PublicKeys <> "" Then
                        GotPublicKeys(PublicKeys)
                    Else
                        Dim APICall As New APICall(AddressOf GotPublicKeys)
                        APICall.GetKeys(False, "saveStorageV" & GameJolt.GamejoltSave.Version & "|" & GameJoltID & "|*")
                    End If

                    Dim APICall1 As New APICall(AddressOf GotOnlineTeamKey)
                    APICall1.GetKeys(False, "RegisterBattleV" & RegisterBattleScreen.REGISTERBATTLEVERSION & "|" & Me.GameJoltID & "|*")

                    Dim t As Texture2D = GetOnlineSprite(Me.GameJoltID)
                    DownloadedSprite = t

                    founduserid = True
                End If
                If Item.Name.ToLower() = "username" Then
                    Me.Username = Item.Value
                End If
            Next

            If founduserid = False Then loadedInstances = 3
        End Sub

        Private Sub GotPublicKeys(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            Dim exists(3) As Boolean

            For Each Item As API.JoltValue In list
                If Item.Value = "saveStorageV" & GameJolt.GamejoltSave.Version & "|" & GameJoltID & "|points" Then
                    If exists(0) = False Then
                        Dim APICall As New APICall(AddressOf GetPlayerPoints)
                        APICall.GetStorageData(Item.Value, False)
                        exists(0) = True
                    End If
                End If
                If Item.Value = "saveStorageV" & GameJolt.GamejoltSave.Version & "|" & GameJoltID & "|emblem" Then
                    If exists(1) = False Then
                        Dim APICall As New APICall(AddressOf GetPlayerEmblem)
                        APICall.GetStorageData(Item.Value, False)
                        exists(1) = True
                    End If
                End If
                If Item.Value = "saveStorageV" & GameJolt.GamejoltSave.Version & "|" & GameJoltID & "|gender" Then
                    If exists(2) = False Then
                        Dim APICall As New APICall(AddressOf GetPlayerGender)
                        APICall.GetStorageData(Item.Value, False)
                        exists(2) = True
                    End If
                End If
            Next

            If exists(0) = False Then
                Points = 0
                loadedInstances += 1
            End If
            If exists(1) = False Then
                Me.Emblem = "trainer"
                loadedInstances += 1
            End If
            If exists(2) = False Then
                Me.Gender = "0"
                loadedInstances += 1
            End If
        End Sub

        Private Sub GotOnlineTeamKey(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            For Each Item As API.JoltValue In list
                If Item.Value.StartsWith("RegisterBattleV" & RegisterBattleScreen.REGISTERBATTLEVERSION & "|" & Me.GameJoltID & "|") = True Then
                    If Me.OnlineTeam Is Nothing Then
                        Dim APICall As New APICall(AddressOf GetOnlineTeam)
                        APICall.GetStorageData(Item.Value, False)
                    End If
                End If
            Next
        End Sub

        Private Sub GetPlayerPoints(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Points = CInt(data.Replace("\""", """"))
            Else
                Points = 0
            End If

            loadedInstances += 1
        End Sub

        Private Sub GetPlayerEmblem(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Emblem = data.Replace("\""", """")
            Else
                Emblem = "trainer"
            End If

            loadedInstances += 1
        End Sub

        Private Sub GetPlayerGender(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))

                Gender = data.Replace("\""", """")
            Else
                Gender = "0"
            End If

            loadedInstances += 1
        End Sub

        Private Sub GetOnlineTeam(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Me.OnlineTeam = New List(Of Pokemon)
                Dim data As String = result.Remove(0, 22)
                data = data.Remove(data.LastIndexOf(""""))
                Dim dataArray() As String = data.SplitAtNewline()

                For Each line As String In dataArray
                    If line.StartsWith("{") = True And line.EndsWith("}") = True Then
                        Dim pokemonData As String = line.Replace("\""", """")

                        Me.OnlineTeam.Add(Pokemon.GetPokemonByData(pokemonData))
                    End If
                Next
            Else
                Me.OnlineTeam = Nothing
            End If
        End Sub

        Public Sub Draw(ByVal Position As Vector2, ByVal Size As Integer)
            Draw(Username, GameJoltID, Points, Gender, Emblem, Position, Size, Me.DownloadedSprite, Me.OnlineTeam)
        End Sub

        Public ReadOnly Property IsFriend() As Boolean
            Get
                If Me.DoneLoading = True Then
                    If Me.GameJoltID <> Core.GameJoltSave.GameJoltID Then
                        Dim Friends() As String = Core.GameJoltSave.Friends.Split(CChar(","))
                        If Friends.Count > 0 Then
                            If Friends.Contains(Me.GameJoltID) = True Then
                                Return True
                            End If
                        End If
                    End If
                End If

                Return False
            End Get
        End Property

        Public Shared ReadOnly Property IsFriend(ByVal GameJoltID As String) As Boolean
            Get
                If GameJoltID <> Core.GameJoltSave.GameJoltID Then
                    Dim Friends() As String = Core.GameJoltSave.Friends.Split(CChar(","))
                    If Friends.Count > 0 Then
                        If Friends.Contains(GameJoltID) = True Then
                            Return True
                        End If
                    End If
                End If

                Return False
            End Get
        End Property

        Public Shared ReadOnly Property SentRequest(ByVal GameJoltID As String) As Boolean
            Get
                If GameJoltID <> Core.GameJoltSave.GameJoltID Then
                    Dim Requests() As String = Core.GameJoltSave.SentRequests.Split(CChar(","))
                    If Requests.Count > 0 Then
                        If Requests.Contains(GameJoltID) = True Then
                            Return True
                        End If
                    End If
                End If

                Return False
            End Get
        End Property

        Public Shared ReadOnly Property ReceivedRequest(ByVal GameJoltID As String) As Boolean
            Get
                If GameJoltID <> Core.GameJoltSave.GameJoltID Then
                    Dim Requests() As String = Core.GameJoltSave.ReceivedRequests.Split(CChar(","))
                    If Requests.Count > 0 Then
                        If Requests.Contains(GameJoltID) = True Then
                            Return True
                        End If
                    End If
                End If

                Return False
            End Get
        End Property

        Public ReadOnly Property HasOnlineTeam() As Boolean
            Get
                If Not Me.OnlineTeam Is Nothing Then
                    If Me.OnlineTeam.Count > 0 Then
                        Return True
                    End If
                End If
                Return False
            End Get
        End Property

#End Region

#Region "Trophy"

        Public Shared Function EmblemToTrophyID(ByVal emblem As String) As Integer
            Select Case emblem.ToLower()
                Case "alph"
                    Return 1958
                Case "material"
                    Return 1960
                Case "cyber"
                    Return 1973
                Case "johto"
                    Return 1963
                Case "kanto"
                    Return 1962
                Case "legendary"
                    Return 1964
                Case "genetics"
                    Return 1972
                Case "unodostres"
                    Return 1974
                Case "champion"
                    Return 1959
                Case "snow"
                    Return 1967
                Case "eevee"
                    Return 1961
                Case "stars"
                    Return 1971
                Case "glowing"
                    Return 1968
                Case "overkill"
                    Return 1969
                Case "pokedex"
                    Return 1970
                Case "zephyr"
                    Return 1994
                Case "hive"
                    Return 1995
                Case "plain"
                    Return 1996
                Case "fog"
                    Return 1997
                Case "storm"
                    Return 1998
                Case "mineral"
                    Return 1999
                Case "glacier"
                    Return 2000
                Case "rising"
                    Return 2001
                Case "eggsplosion"
                    Return 2581
                Case "mailman"
                    Return 3746
                Case "silver ability"
                    Return 4765
                Case "silver knowledge"
                    Return 4767
                Case "gold ability"
                    Return 4766
                Case "gold knowledge"
                    Return 4768
                Case "boulder"
                    Return 5776
                Case "cascade"
                    Return 5777
                Case "thunder"
                    Return 5767
                Case "rainbow"
                    Return 8677
                Case "marsh"
                    Return 8678
                Case "soul"
                    Return 10829
                Case "volcano"
                    Return 8752
                Case "earth"
                    Return 17001
                Case "shooting star"
                    Return 17559
                Case Else
                    Return 0
            End Select
        End Function

        Public Shared Function TrophyIDToEmblem(ByVal trophy_id As Integer) As String
            Select Case trophy_id
                Case 1958
                    Return "alph"
                Case 1959
                    Return "champion"
                Case 1960
                    Return "material"
                Case 1961
                    Return "eevee"
                Case 1962
                    Return "kanto"
                Case 1963
                    Return "johto"
                Case 1964
                    Return "legendary"
                Case 1967
                    Return "snow"
                Case 1968
                    Return "glowing"
                Case 1969
                    Return "overkill"
                Case 1970
                    Return "pokedex"
                Case 1971
                    Return "stars"
                Case 1972
                    Return "genetics"
                Case 1973
                    Return "cyber"
                Case 1974
                    Return "unodostres"
                Case 1994
                    Return "zephyr"
                Case 1995
                    Return "hive"
                Case 1996
                    Return "plain"
                Case 1997
                    Return "fog"
                Case 1998
                    Return "storm"
                Case 1999
                    Return "mineral"
                Case 2000
                    Return "glacier"
                Case 2001
                    Return "rising"
                Case 2581
                    Return "eggsplosion"
                Case 3746
                    Return "mailman"
                Case 4765
                    Return "silver ability"
                Case 4766
                    Return "gold ability"
                Case 4767
                    Return "silver knowledge"
                Case 4768
                    Return "gold knowledge"
                Case 5767
                    Return "thunder"
                Case 5776
                    Return "boulder"
                Case 5777
                    Return "cascade"
                Case 8677
                    Return "rainbow"
                Case 8678
                    Return "marsh"
                Case 10829
                    Return "soul"
                Case 8752
                    Return "volcano"
                Case 17001
                    Return "earth"
                Case 17559
                    Return "shooting star"
                Case Else
                    Return "fail"
            End Select
        End Function

        Public Shared Sub GetAchievedEmblems()
            Dim APICall As New APICall(AddressOf AddAchievedEmblems)
            APICall.FetchAllAchievedTrophies()
        End Sub

        Private Shared Sub AddAchievedEmblems(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(list(0).Value) = True Then
                Dim currentTrophyID As Integer = 0

                For i = 0 To list.Count - 1
                    Select Case list(i).Name
                        Case "id"
                            currentTrophyID = CInt(list(i).Value)
                        Case "achieved"
                            If list(i).Value <> "false" Then
                                Dim newEmblem As String = TrophyIDToEmblem(currentTrophyID)
                                If newEmblem <> "fail" Then
                                    If Core.GameJoltSave.AchievedEmblems.Contains(newEmblem) = False Then
                                        Core.GameJoltSave.AchievedEmblems.Add(newEmblem)
                                    End If
                                End If
                            End If
                    End Select
                Next
            End If

            If Core.GameJoltSave.AchievedEmblems.Contains("trainer") = False Then
                Core.GameJoltSave.AchievedEmblems.Add("trainer")
            End If
        End Sub

        Public Shared Sub AchieveEmblem(ByVal emblem As String)
            If Core.Player.IsGamejoltSave = True Then
                If Core.GameJoltSave.AchievedEmblems.Contains(emblem.ToLower()) = False Then
                    Dim trophy_id As Integer = EmblemToTrophyID(emblem)

                    Dim APICall As New APICall(AddressOf AddedAchievement)
                    APICall.TrophyAchieved(trophy_id)

                    Dim APICallShow As New APICall(AddressOf ShowAchievedEmblem)
                    APICallShow.FetchTrophy(trophy_id)
                End If
            End If
            If Core.Player.EarnedAchievements.Contains(emblem.ToLower()) = False Then
                If ConnectScreen.Connected = True Then
                    Core.ServersManager.ServerConnection.SendGameStateMessage("achieved the emblem """ & emblem.ToUpper() & """!")
                End If
                Core.Player.EarnedAchievements.Add(emblem.ToLower())
            End If
        End Sub

        Private Shared Sub AddedAchievement(ByVal result As String)
            Dim APICall As New APICall(AddressOf AddAchievedEmblems)
            APICall.FetchAllAchievedTrophies()
        End Sub

        Private Shared Sub ShowAchievedEmblem(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            For Each line As API.JoltValue In list
                Select Case line.Name.ToLower()
                    Case "title"
                        achieved_emblem_title = line.Value
                    Case "description"
                        achieved_emblem_description = line.Value
                    Case "difficulty"
                        achieved_emblem_difficulty = line.Value
                    Case "image_url"
                        Dim t As New Threading.Thread(AddressOf DownloadAchievedEmblemTextrure)
                        t.IsBackground = True
                        t.Start(line.Value)
                End Select
            Next
        End Sub

        Private Shared Sub DownloadAchievedEmblemTextrure(ByVal url As Object)
            Dim t As Texture2D = DownloadTexture2D.n_Remote_Texture2D(Core.GraphicsDevice, url.ToString(), True)

            achieved_emblem_Texture = t

            displayEmblemDelay = 35.0F
        End Sub

        Shared displayEmblemDelay As Single = 0.0F
        Shared emblemPositionX As Integer = Core.windowSize.Width

        Shared achieved_emblem_description As String = ""
        Shared achieved_emblem_Texture As Texture2D = Nothing
        Shared achieved_emblem_title As String = ""
        Shared achieved_emblem_difficulty As String = ""

        Public Shared Sub SetDebugAchieve(ByVal emblem As String)
            If Core.GameJoltSave.AchievedEmblems.Contains(emblem) = False Then
                displayEmblemDelay = 35.0F

                Core.GameJoltSave.AchievedEmblems.Add(emblem)
            End If
        End Sub

        Public Shared Sub DrawNewEmblems()
            If displayEmblemDelay > 0.0F Then
                displayEmblemDelay -= 0.1F
                If displayEmblemDelay <= 6.4F Then
                    If emblemPositionX < Core.windowSize.Width Then
                        emblemPositionX += 8
                    End If
                Else
                    If emblemPositionX > Core.windowSize.Width - 512 Then
                        emblemPositionX -= 8
                    End If
                End If

                Canvas.DrawRectangle(New Rectangle(emblemPositionX + 10, 0, 512, 98), Color.Black)

                If Not achieved_emblem_Texture Is Nothing Then
                    Core.SpriteBatch.Draw(achieved_emblem_Texture, New Rectangle(emblemPositionX + 2, 2, 75, 75), Color.White)
                End If

                Core.SpriteBatch.Draw(TextureManager.GetTexture("Textures\Emblem\border"), New Rectangle(emblemPositionX, 0, 79, 98), Color.White)

                Dim fontColor As Color = Color.White
                Select Case achieved_emblem_difficulty.ToLower()
                    Case "bronze"
                        fontColor = New Color(220, 171, 117)
                    Case "silver"
                        fontColor = New Color(207, 207, 207)
                    Case "gold"
                        fontColor = New Color(255, 207, 39)
                    Case "platinum"
                        fontColor = New Color(172, 201, 202)
                End Select
                Core.SpriteBatch.DrawString(FontManager.MiniFont, achieved_emblem_difficulty, New Vector2(emblemPositionX + (38 - CInt(FontManager.MiniFont.MeasureString(achieved_emblem_difficulty).X / 2)), 77), fontColor)

                Core.SpriteBatch.DrawString(FontManager.MiniFont, "Achieved new emblem background: " & achieved_emblem_title, New Vector2(emblemPositionX + 88, 4), fontColor)

                Dim desText As String = achieved_emblem_description.CropStringToWidth(FontManager.MiniFont, 300)

                Core.SpriteBatch.DrawString(FontManager.MiniFont, desText, New Vector2(emblemPositionX + 94, 24), Color.White)

                If displayEmblemDelay <= 0.0F Then
                    displayEmblemDelay = 0.0F
                    emblemPositionX = Core.windowSize.Width
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace