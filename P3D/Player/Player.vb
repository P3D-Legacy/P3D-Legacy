﻿Public Class Player

    Inherits Security.HashSecureBase

#Region "Properties"

    Public Property Name() As String
        Get
            Assert("_name", _name)
            Return _name
        End Get
        Set(value As String)
            Assert("_name", _name, value)
            _name = value
        End Set
    End Property

    Public Property RivalName() As String
        Get
            Return _rivalName
        End Get
        Set(value As String)
            _rivalName = value
        End Set
    End Property
    Public Property RivalSkin() As String
        Get
            Return _rivalSkin
        End Get
        Set(value As String)
            _rivalSkin = value
        End Set
    End Property

    Public Property Gender() As String
        Get
            Return _gender
        End Get
        Set(value As String)
            _gender = value
        End Set
    End Property

    Public Property Money() As Integer
        Get
            Return _money
        End Get
        Set(value As Integer)
            _money = value
        End Set
    End Property

    Public Property OT() As String
        Get
            Assert("_ot", _OT)
            Return _OT
        End Get
        Set(value As String)
            Assert("_ot", _OT, value)
            _OT = value
        End Set
    End Property

    Public Property Points() As Integer
        Get
            Return _points
        End Get
        Set(value As Integer)
            _points = value
        End Set
    End Property

    Public Property BP() As Integer
        Get
            Return _BP
        End Get
        Set(value As Integer)
            _BP = value
        End Set
    End Property

    Public Property Coins() As Integer
        Get
            Return _coins
        End Get
        Set(value As Integer)
            _coins = value
        End Set
    End Property

    Public Property HasPokedex() As Boolean
        Get
            Return _hasPokedex
        End Get
        Set(value As Boolean)
            _hasPokedex = value
        End Set
    End Property

    Public Property HasPokegear() As Boolean
        Get
            Return _hasPokegear
        End Get
        Set(value As Boolean)
            _hasPokegear = value
        End Set
    End Property

    Public Property LastRestPlace() As String
        Get
            Return _lastRestPlace
        End Get
        Set(value As String)
            _lastRestPlace = value
        End Set
    End Property

    Public Property LastRestPlacePosition() As String
        Get
            Return _lastRestPlacePosition
        End Get
        Set(value As String)
            _lastRestPlacePosition = value
        End Set
    End Property

    Public Property LastSavePlace() As String
        Get
            Return _lastSavePlace
        End Get
        Set(value As String)
            _lastSavePlace = value
        End Set
    End Property

    Public Property LastSavePlacePosition() As String
        Get
            Return _lastSavePlacePosition
        End Get
        Set(value As String)
            _lastSavePlacePosition = value
        End Set
    End Property

    Public Property RepelSteps() As Integer
        Get
            Return _repelSteps
        End Get
        Set(value As Integer)
            _repelSteps = value
        End Set
    End Property

    Public Property ScriptDelayItems() As String
        Get
            Return _ScriptDelayItems
        End Get
        Set(value As String)
            _ScriptDelayItems = value
        End Set
    End Property
    Public Property ScriptDelaySteps() As Integer
        Get
            Return _ScriptDelaySteps
        End Get
        Set(value As Integer)
            _ScriptDelaySteps = value
        End Set
    End Property
    Public Property ScriptDelayDisplaySteps() As Boolean
        Get
            Return _ScriptDelayDisplaySteps
        End Get
        Set(value As Boolean)
            _ScriptDelayDisplaySteps = value
        End Set
    End Property

    Public Property SaveCreated() As String
        Get
            Return _saveCreated
        End Get
        Set(value As String)
            _saveCreated = value
        End Set
    End Property

    Public Property DaycareSteps() As Integer
        Get
            Return _daycareSteps
        End Get
        Set(value As Integer)
            _daycareSteps = value
        End Set
    End Property
    Public Property PoisonSteps() As Integer
        Get
            Return _poisonSteps
        End Get
        Set(value As Integer)
            _poisonSteps = value
        End Set
    End Property

    Public Property GameMode() As String
        Get
            Return _gameMode
        End Get
        Set(value As String)
            _gameMode = value
        End Set
    End Property

    Public Property Skin() As String
        Get
            Return _skin
        End Get
        Set(value As String)
            _skin = value
        End Set
    End Property

    Public Property VisitedMaps() As String
        Get
            Return _visitedMaps
        End Get
        Set(value As String)
            _visitedMaps = value
        End Set
    End Property

    Public Property GTSStars() As Integer
        Get
            Return _GTSStars
        End Get
        Set(value As Integer)
            _GTSStars = value
        End Set
    End Property

    Public Property SandBoxMode() As Boolean
        Get
            Assert("_sandboxmode", _sandBoxMode)
            Return _sandBoxMode
        End Get
        Set(value As Boolean)
            Assert("_sandboxmode", _sandBoxMode, value)
            _sandBoxMode = value
        End Set
    End Property

    Public Property RegisterData() As String
        Get
            Return _registerData
        End Get
        Set(value As String)
            _registerData = value
        End Set
    End Property

    Public Property BerryData() As String
        Get
            Return _berryData
        End Get
        Set(value As String)
            _berryData = value
        End Set
    End Property

    Public Property PokedexData() As String
        Get
            Return _pokedexData
        End Get
        Set(value As String)
            _pokedexData = value
        End Set
    End Property

    Public Property ItemData() As String
        Get
            Return _itemData
        End Get
        Set(value As String)
            _itemData = value
        End Set
    End Property

    Public Property BoxData() As String
        Get
            Return _boxData
        End Get
        Set(value As String)
            _boxData = value
        End Set
    End Property

    Public Property NPCData() As String
        Get
            Return _NPCData
        End Get
        Set(value As String)
            _NPCData = value
        End Set
    End Property

    Public Property ApricornData() As String
        Get
            Return _apricornData
        End Get
        Set(value As String)
            _apricornData = value
        End Set
    End Property

    Public Property SecretBaseData() As String
        Get
            Return _secretBaseData
        End Get
        Set(value As String)
            _secretBaseData = value
        End Set
    End Property

    Public Property DaycareData() As String
        Get
            Return _daycareData
        End Get
        Set(value As String)
            _daycareData = value
        End Set
    End Property

    Public Property HallOfFameData() As String
        Get
            Return _hallOfFameData
        End Get
        Set(value As String)
            _hallOfFameData = value
        End Set
    End Property

    Public Property RoamingPokemonData() As String
        Get
            Return _roamingPokemonData
        End Get
        Set(value As String)
            _roamingPokemonData = value
        End Set
    End Property

    Public Property HistoryData() As String
        Get
            Return _historyData
        End Get
        Set(value As String)
            _historyData = value
        End Set
    End Property

    Public Property IsGameJoltSave() As Boolean
        Get
            Assert("_isgamejoltsave", _isGamejoltSave)
            Return _isGamejoltSave
        End Get
        Set(value As Boolean)
            Assert("_isgamejoltsave", _isGamejoltSave, value)
            _isGamejoltSave = value
        End Set
    End Property

    Public Property EmblemBackground() As String
        Get
            Assert("_emblembackground", _emblemBackground)
            Return _emblemBackground
        End Get
        Set(value As String)
            Assert("_emblembackground", _emblemBackground, value)
            _emblemBackground = value
        End Set
    End Property

#End Region

    'Non-base datatypes:
    Public Pokemons As New List(Of Pokemon)
    Public Pokedexes As New List(Of Pokedex)
    Public Inventory As New PlayerInventory
    Public Badges As New List(Of Integer)
    Public PlayTime As TimeSpan
    Public GameStart As Date
    Public LastPokemonPosition As Vector3 = New Vector3(999, 999, 999)
    Public PokeFiles As New List(Of String)
    Public EarnedAchievements As New List(Of String)
    Public PokegearModules As New List(Of Integer)
    Public PhoneContacts As New List(Of String)
    Public Mails As New List(Of Items.MailItem.MailData)
    Public Trophies As New List(Of Integer)

    'Non-secure fields:
    Public ShowBattleAnimations As Integer = 1
    Public BoxAmount As Integer = 10
    Public DiagonalMovement As Boolean = False
    Public DifficultyMode As Integer = 0
    Public BattleStyle As Integer = 0
    Public ShowModelsInBattle As Boolean = True
    Public TempSurfSkin As String = "Hilbert"
    Public TempRideSkin As String = ""
    Public Statistics As String = ""
    Public CheckForTrainersLater As Boolean = False
    Public UsedItemsToCheckScriptDelayFor As New List(Of String)

    'Secure fields:
    Private _name As String = "<player.name>"
    Private _rivalName As String = "???"
    Private _rivalSkin As String = "Silver"
    Private _gender As String = "Male"
    Private _money As Integer = 0
    Private _OT As String = "00000"
    Private _points As Integer = 0
    Private _BP As Integer = 0
    Private _coins As Integer = 0
    Private _hasPokedex As Boolean = False
    Private _hasPokegear As Boolean = False
    Private _lastRestPlace As String = "yourroom.dat"
    Private _lastRestPlacePosition As String = "1,0.1,3"
    Private _lastSavePlace As String = "yourroom.dat"
    Private _lastSavePlacePosition As String = "1,0.1,3"
    Private _repelSteps As Integer = 0
    Private _ScriptDelaySteps As Integer = 0
    Private _ScriptDelayItems As String = ""
    Private _ScriptDelayDisplaySteps As Boolean = False
    Private _saveCreated As String = "Pre 0.21"
    Private _daycareSteps As Integer = 0
    Private _poisonSteps As Integer = 0
    Private _gameMode As String = "Kolben"
    Private _skin As String = "Hilbert"
    Private _visitedMaps As String = ""
    Private _GTSStars As Integer = 8
    Private _sandBoxMode As Boolean = False

    Private _registerData As String = ""
    Private _berryData As String = ""
    Private _pokedexData As String = ""
    Private _itemData As String = ""
    Private _boxData As String = ""
    Private _NPCData As String = ""
    Private _apricornData As String = ""
    Private _secretBaseData As String = ""
    Private _daycareData As String = ""
    Private _hallOfFameData As String = ""
    Private _roamingPokemonData As String = ""
    Private _historyData As String = ""
    Private _isGamejoltSave As Boolean = False
    Private _emblemBackground As String = "standard"

    Public startPosition As Vector3 = New Vector3(14, 0.1, 10)
    Public startRotation As Single = 0
    Public startFreeCameraMode As Boolean = False
    Public startMap As String = "barktown.dat"
    Public startFOV As Single = 45.0F
    Public startRotationSpeed As Integer = 12
    Public startThirdPerson As Boolean = False
    Public startSurfing As Boolean = False
    Public startRiding As Boolean = False
    Public EnableExpAll As Boolean = False

    Public filePrefix As String = "nilllzz"
    Public newFilePrefix As String = ""
    Public AutosaveUsed As Boolean = False
    Public loadedSave As Boolean = False

    Public PlayerTemp As New PlayerTemp()

    Public RunMode As Boolean = True
    Public RunToggled As Boolean = False

    Public DoWalkAnimation As Boolean = True

    Public Structure Temp
        Public Shared PokemonScreenIndex As Integer = 0
        Public Shared PokemonStatusPageIndex As Integer = 0
        Public Shared BagIndex As Integer = 0
        Public Shared BagPageIndex As Integer() = {0, 0, 0, 0, 0, 0, 0, 0}
        Public Shared BagItemIndex As Integer() = {0, 0, 0, 0, 0, 0, 0, 0}
        Public Shared BagSelectIndex As Integer = 0
        Public Shared MenuIndex As Integer = 0
        Public Shared PokedexIndex As Integer = 0
        Public Shared PokemonSummaryPageIndex As Integer = 0
        Public Shared PCBoxIndex As Integer = 0
        Public Shared StorageSystemCursorPosition As New Vector2(1, 0)
        Public Shared OptionScreenIndex As Integer = 0
        Public Shared MapSwitch(3) As Boolean
        Public Shared LastPosition As Vector3
        Public Shared IsInBattle As Boolean = False
        Public Shared BeforeBattlePosition As Vector3 = New Vector3(0)
        Public Shared BeforeBattleLevelFile As String = "yourroom.dat"
        Public Shared BeforeBattleFacing As Integer = 0
        Public Shared PokedexModeIndex As Integer = 0
        Public Shared PokedexHabitatIndex As Integer = 0
        Public Shared PokegearPage As Integer = 0
        Public Shared LastCall As Integer = 32
        Public Shared LastUsedRepel As Integer = -1
        Public Shared MapSteps As Integer = 0
        Public Shared HallOfFameIndex As Integer = 0
        Public Shared PCBoxChooseMode As Boolean = False
        Public Shared PCSelectionType As StorageSystemScreen.SelectionModes = StorageSystemScreen.SelectionModes.SingleMove
        Public Shared RadioStation As Decimal = 0D
        Public Shared LastPokegearPage As GameJolt.PokegearScreen.MenuScreens = GameJolt.PokegearScreen.MenuScreens.Main
    End Structure

    Private Sub ResetTemp()
        Temp.PokemonScreenIndex = 0
        Temp.PokemonStatusPageIndex = 0
        Temp.BagIndex = 0
        Temp.BagPageIndex = {0, 0, 0, 0, 0, 0, 0, 0}
        Temp.BagItemIndex = {0, 0, 0, 0, 0, 0, 0, 0}
        Temp.BagSelectIndex = 0
        Temp.MenuIndex = 0
        Temp.PokedexIndex = 0
        Temp.PCBoxIndex = 0
        Temp.OptionScreenIndex = 0
        Temp.IsInBattle = False
        For i = 0 To 3
            Temp.MapSwitch(i) = True
        Next
        Temp.PokedexModeIndex = 0
        Temp.PokedexHabitatIndex = 0
        Temp.PokegearPage = 0
        Temp.LastCall = 32
        Temp.LastUsedRepel = -1
        Temp.MapSteps = 0
        Temp.HallOfFameIndex = 0
        Temp.PCBoxChooseMode = False
        Temp.StorageSystemCursorPosition = New Vector2(1, 0)
        Temp.PCSelectionType = StorageSystemScreen.SelectionModes.SingleMove
        Temp.RadioStation = 0D
        Temp.LastPokegearPage = GameJolt.PokegearScreen.MenuScreens.Main
    End Sub

#Region "Load"

    Public Sub LoadGame(ByVal filePrefix As String)
        For Each s As String In Core.GameOptions.ContentPackNames
            ContentPackManager.Load(GameController.GamePath & "\ContentPacks\" & s & "\exceptions.dat")
        Next

        GameModeManager.CreateGameModesFolder()
        GameModeManager.CreateKolbenMode()

        ScriptStorage.Clear()
        ScriptBlock.TriggeredScriptBlock = False
        MysteryEventScreen.ClearActivatedEvents()
        Pokedex.AutoDetect = True
        LevelLoader.ClearTempStructures()
        BattleSystem.BattleScreen.ResetVars()
        World.RegionWeatherSet = False

        Me.filePrefix = filePrefix
        PokeFiles.Clear()
        GameMode = "Kolben"

        LoadPlayer()
        GameJolt.Emblem.GetAchievedEmblems()

        BattleSystem.GameModeElementLoader.Load()
        BattleSystem.GameModeAttackLoader.Load()
        GameModeItemLoader.Load()

        If IsGameJoltSave = True Then
            SandBoxMode = False
        End If

        SmashRock.Load()
        Badge.Load()
        Pokedex.Load()
        PokemonInteractions.Load()
        PokemonForms.Initialize()

        LoadPokedex()
        LoadParty()
        LoadItems()
        LoadBerries()
        LoadApricorns()
        LoadDaycare()
        LoadOptions()
        LoadRegister()
        LoadItemData()
        LoadBoxData()
        LoadNPCData()
        LoadHallOfFameData()
        LoadSecretBaseData()
        LoadRoamingPokemonData()
        LoadStatistics()

        PlayerTemp.Reset()
        ResetTemp()
        Chat.ClearChat()

        If AutosaveUsed = True Then
            IO.Directory.Delete(GameController.GamePath & "\Save\" & Me.filePrefix, True)

            Me.filePrefix = newFilePrefix
            AutosaveUsed = False

            Dim outputString As String = newFilePrefix

            Core.GameMessage.ShowMessage(Localization.GetString("game_message_continue_autosave") & " """ & outputString & """", 12, FontManager.MainFont, Color.White)

            newFilePrefix = ""
        End If

        If IsGameJoltSave = True Then
            lastLevel = GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points)
            OT = GameJoltSave.GameJoltID
        End If

        Entity.MakeShake = Name.ToLower() = "drunknilllzz"

        loadedSave = True
    End Sub

    Private Sub LoadParty()
        Pokemons.Clear()

        Dim PokeData() As String
        If IsGameJoltSave = True Then
            PokeData = GameJoltSave.Party.SplitAtNewline()
        Else
            PokeData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Party.dat").SplitAtNewline()
        End If

        If PokeData.Count > 0 AndAlso PokeData(0) <> "" Then
            For Each Line As String In PokeData
                If Line.StartsWith("{") = True And Line.EndsWith("}") = True Then
                    Dim p As Pokemon = Pokemon.GetPokemonByData(Line)

                    Dim dexID As String = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)

                    If p.IsEgg() = False Then
                        If p.IsShiny = True Then
                            PokedexData = Pokedex.ChangeEntry(PokedexData, dexID, 3)
                        Else
                            PokedexData = Pokedex.ChangeEntry(PokedexData, dexID, 2)
                        End If
                    End If

                    Pokemons.Add(p)
                End If
            Next
        End If
    End Sub

    Private Sub LoadPlayer()
        Dim Data() As String
        If IsGameJoltSave = True Then
            Data = GameJoltSave.Player.SplitAtNewline()
        Else
            Data = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Player.dat").SplitAtNewline()
        End If

        If Not Screen.Level Is Nothing Then
            Screen.Level.Riding = False
        End If

        For Each Line As String In Data
            If Line <> "" And Line.Contains("|") = True Then
                Dim ID As String = Line.Remove(Line.IndexOf("|"))
                Dim Value As String = Line.Remove(0, Line.IndexOf("|") + 1)
                Select Case ID.ToLower()
                    Case "name"
                        Name = Value

                        If IsGameJoltSave = True Then
                            If Name.ToLower() <> GameJolt.API.username.ToLower() Then
                                Name = GameJolt.API.username
                            End If
                        End If
                    Case "position"
                        Dim v() As String = Value.Split(CChar(","))
                        startPosition.X = CSng(v(0).Replace(".", GameController.DecSeparator))
                        startPosition.Y = CSng(v(1).Replace(".", GameController.DecSeparator))
                        startPosition.Z = CSng(v(2).Replace(".", GameController.DecSeparator))
                    Case "lastpokemonposition"
                        Dim v() As String = Value.Split(CChar(","))
                        LastPokemonPosition.X = CSng(v(0).Replace(".", GameController.DecSeparator))
                        LastPokemonPosition.Y = CSng(v(1).Replace(".", GameController.DecSeparator))
                        LastPokemonPosition.Z = CSng(v(2).Replace(".", GameController.DecSeparator))
                    Case "mapfile"
                        startMap = Value
                    Case "rivalname"
                        RivalName = Value
                    Case "rivalskin"
                        RivalSkin = Value
                    Case "money"
                        Money = CInt(Value)
                    Case "badges"
                        Badges.Clear()

                        If Value = "0" Then
                            Badges = New List(Of Integer)
                        Else
                            If Value.Contains(",") = False Then
                                Badges = {CInt(Value)}.ToList()
                            Else
                                Dim l As List(Of String) = Value.Split(CChar(",")).ToList()

                                For i = 0 To l.Count - 1
                                    Badges.Add(CInt(l(i)))
                                Next
                            End If
                        End If
                    Case "rotation"
                        startRotation = CSng(Value.Replace(".", GameController.DecSeparator))
                    Case "gender"
                        If Value.ToLower = "male" Or Value = "0" Then
                            Gender = "Male"
                        ElseIf Value.ToLower = "female" Or Value = "1" Then
                            Gender = "Female"
                        Else
                            Gender = "Other"
                        End If
                    Case "playtime"
                        Dim dd() As String = Value.Split(CChar(","))
                        If dd.Count >= 4 Then
                            PlayTime = New TimeSpan(CInt(dd(3)), CInt(dd(0)), CInt(dd(1)), CInt(dd(2)))
                        Else
                            PlayTime = New TimeSpan(CInt(dd(0)), CInt(dd(1)), CInt(dd(2)))
                        End If
                    Case "ot"
                        OT = CStr(CInt(Value).Clamp(0, 999999))
                    Case "points"
                        Points = CInt(Value)
                    Case "haspokedex"
                        HasPokedex = CBool(Value)
                    Case "haspokegear"
                        HasPokegear = CBool(Value)
                    Case "freecamera"
                        startFreeCameraMode = CBool(Value)
                    Case "thirdperson"
                        startThirdPerson = CBool(Value)
                    Case "skin"
                        Skin = Value
                    Case "battleanimations"
                        ShowBattleAnimations = CInt(Value)
                    Case "runmode"
                        RunMode = CBool(Value)
                    Case "runtoggled"
                        RunToggled = CBool(Value)
                    Case "boxamount"
                        BoxAmount = CInt(Value)
                    Case "lastrestplace"
                        LastRestPlace = Value
                    Case "lastrestplaceposition"
                        LastRestPlacePosition = Value
                    Case "diagonalmovement"
                        If GameController.IS_DEBUG_ACTIVE = True Then
                            DiagonalMovement = CBool(Value)
                        Else
                            DiagonalMovement = False
                        End If
                    Case "repelsteps"
                        RepelSteps = CInt(Value)
                    Case "scriptdelayitems"
                        ScriptDelayItems = Value
                    Case "scriptdelaysteps"
                        ScriptDelaySteps = CInt(Value)
                    Case "scriptdelaydisplaysteps"
                        ScriptDelayDisplaySteps = CBool(Value)
                    Case "lastsaveplace"
                        LastSavePlace = Value
                    Case "lastsaveplaceposition"
                        LastSavePlacePosition = Value
                    Case "difficulty"
                        DifficultyMode = CInt(Value)
                    Case "battlestyle"
                        BattleStyle = CInt(Value)
                    Case "savecreated"
                        SaveCreated = Value
                    Case "autosave"
                        If IsGameJoltSave = False Then
                            newFilePrefix = Value
                            AutosaveUsed = True
                        End If
                    Case "daycaresteps"
                        DaycareSteps = CInt(Value)
                    Case "gamemode"
                        GameMode = Value
                    Case "pokefiles"
                        If Value <> "" Then
                            If Value.Contains(",") = True Then
                                PokeFiles.AddRange(Value.Split(CChar(",")))
                            Else
                                PokeFiles.Add(Value)
                            End If
                        End If
                    Case "visitedmaps"
                        VisitedMaps = Value
                    Case "tempsurfskin"
                        TempSurfSkin = Value
                    Case "surfing"
                        startSurfing = CBool(Value)
                    Case "bp"
                        BP = CInt(Value)
                    Case "coins"
                        Coins = CInt(Value)
                    Case "gtsstars"
                        GTSStars = CInt(Value)
                    Case "showmodels"
                        ShowModelsInBattle = CBool(Value)
                    Case "sandboxmode"
                        SandBoxMode = CBool(Value)
                    Case "earnedachievements"
                        If Value <> "" Then
                            EarnedAchievements = Value.ToLower.Split(CChar(",")).ToList()
                        End If
                    Case "expall"
                        EnableExpAll = CBool(Value)
                End Select
            Else
                Logger.Log(Logger.LogTypes.Warning, "Player.vb: The line """ & Line & """ is either empty or does not conform the player.dat file rules.")
            End If
        Next

        If IsGameJoltSave = True And startSurfing = False Then
            Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender)
            Select Case GameJoltSave.Gender
                Case "Male"
                    Gender = "Male"
                Case "Female"
                    Gender = "Female"
                Case Else
                    Gender = "Other"
            End Select
        End If

        GameStart = Date.Now
    End Sub

    Private Sub LoadOptions()
        Dim Data() As String
        If IsGameJoltSave = True Then
            Data = GameJoltSave.Options.SplitAtNewline()
        Else
            Data = IO.File.ReadAllLines(GameController.GamePath & "\Save\" & filePrefix & "\Options.dat")
        End If

        For Each Line As String In Data
            If Line.Contains("|") = True Then
                Dim ID As String = Line.Remove(Line.IndexOf("|"))
                Dim Value As String = Line.Remove(0, Line.IndexOf("|") + 1)
                Select Case ID.ToLower()
                    Case "fov"
                        startFOV = CSng(Value.Replace(".", GameController.DecSeparator)).Clamp(1, 179)
                    Case "textspeed"
                        TextBox.TextSpeed = CInt(Value)
                    Case "mousespeed"
                        startRotationSpeed = CInt(Value)
                End Select
            End If
        Next
    End Sub

    Private Sub LoadItems()
        Inventory.Clear()
        Mails.Clear()

        Dim Data As String
        If IsGameJoltSave = True Then
            Data = GameJoltSave.Items
        Else
            Data = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Items.dat")
        End If

        If Data <> "" Then
            Dim ItemData() As String = Data.SplitAtNewline()

            For Each ItemDat As String In ItemData
                If ItemDat <> "" And ItemDat.StartsWith("{") = True And ItemDat.EndsWith("}") = True And ItemDat.Contains("|") = True Then
                    Dim ItemID As String = ItemDat.Remove(0, ItemDat.IndexOf("{") + 1)
                    ItemID = ItemID.Remove(ItemID.IndexOf("}"))

                    Dim amount As Integer = CInt(ItemID.Remove(0, ItemID.IndexOf("|") + 1))
                    ItemID = ItemID.Remove(ItemID.IndexOf("|"))

                    Inventory.AddItem(ItemID, amount)
                Else
                    If ItemDat <> "" And ItemDat.StartsWith("Mail|") = True Then
                        Dim mailData As String = ItemDat.Remove(0, 5)
                        Mails.Add(Items.MailItem.GetMailDataFromString(mailData))
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub LoadBerries()
        If IsGameJoltSave = True Then
            Core.Player.BerryData = GameJoltSave.Berries
        Else
            Core.Player.BerryData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Berries.dat")
        End If
    End Sub

    Private Sub LoadApricorns()
        If IsGameJoltSave = True Then
            Core.Player.ApricornData = GameJoltSave.Apricorns
        Else
            Core.Player.ApricornData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Apricorns.dat")
        End If
    End Sub

    Private Sub LoadDaycare()
        Core.Player.DaycareData = ""
        If IsGameJoltSave = True Then
            Core.Player.DaycareData = GameJoltSave.Daycare
        Else
            If IO.File.Exists(GameController.GamePath & "\Save\" & filePrefix & "\Daycare.dat") = True Then
                Core.Player.DaycareData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Daycare.dat")
            End If
        End If
    End Sub

    Private Sub LoadPokedex()
        If IsGameJoltSave = True Then
            PokedexData = GameJoltSave.Pokedex
        Else
            PokedexData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Pokedex.dat")
        End If

        If PokedexData = "" Then
            PokedexData = Pokedex.NewPokedex()
        End If
    End Sub

    Private Sub LoadRegister()
        If IsGameJoltSave = True Then
            RegisterData = GameJoltSave.Register
        Else
            RegisterData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Register.dat")
        End If
    End Sub

    Private Sub LoadItemData()
        If IsGameJoltSave = True Then
            ItemData = GameJoltSave.ItemData
        Else
            ItemData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\ItemData.dat")
        End If
    End Sub

    Private Sub LoadBoxData()
        If IsGameJoltSave = True Then
            BoxData = GameJoltSave.Box
        Else
            BoxData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Box.dat")
        End If
    End Sub

    Private Sub LoadNPCData()
        If IsGameJoltSave = True Then
            NPCData = GameJoltSave.NPC
        Else
            NPCData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\NPC.dat")
        End If
    End Sub

    Private Sub LoadHallOfFameData()
        If IsGameJoltSave = True Then
            HallOfFameData = GameJoltSave.HallOfFame
        Else
            If IO.File.Exists(GameController.GamePath & "\Save\" & filePrefix & "\HallOfFame.dat") = True Then
                HallOfFameData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\HallOfFame.dat")
            Else
                HallOfFameData = ""
            End If
        End If
    End Sub

    Private Sub LoadSecretBaseData()
        If IsGameJoltSave = True Then
            SecretBaseData = GameJoltSave.SecretBase
        Else
            If IO.File.Exists(GameController.GamePath & "\Save\" & filePrefix & "\SecretBase.dat") = True Then
                SecretBaseData = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\SecretBase.dat")
            Else
                SecretBaseData = ""
            End If
        End If
    End Sub

    Private Sub LoadRoamingPokemonData()
        RoamingPokemonData = ""
        If IsGameJoltSave = True Then
            RoamingPokemonData = GameJoltSave.RoamingPokemon
        Else
            If IO.File.Exists(GameController.GamePath & "\Save\" & filePrefix & "\RoamingPokemon.dat") = True Then
                For Each line As String In IO.File.ReadAllLines(GameController.GamePath & "\Save\" & filePrefix & "\RoamingPokemon.dat")
                    If RoamingPokemonData <> "" Then
                        RoamingPokemonData &= Environment.NewLine
                    End If
                    If line.CountSeperators("|") < 6 Then
                        'Convert potential old data:
                        Dim data() As String = line.Split(CChar("|"))
                        Dim newP As Pokemon = Pokemon.GetPokemonByID(CInt(data(0)))
                        newP.Generate(CInt(data(1)), True)

                        RoamingPokemonData &= Random.Next(100, 1001).ToString & "|" & newP.Number.ToString() & "|" & newP.Level.ToString() & "|" & data(2) & "|" & data(3) & "||" & newP.IsShiny.ToNumberString() & "|" & newP.GetSaveData()
                    ElseIf line.CountSeperators("|") < 7 Then
                        'Update to include shiny (and identifier)
                        Dim data() As String = line.Split(CChar("|"))
                        Dim newP As Pokemon = Pokemon.GetPokemonByData(data(5))

                        RoamingPokemonData &= Random.Next(100, 1001).ToString & "|" & data(0) & "|" & data(1) & "|" & data(2) & "|" & data(3) & "|" & data(4) & "|" & newP.IsShiny.ToNumberString() & "|" & data(5)
                    ElseIf line.CountSeperators("|") < 8 Then
                        'Update to include (and identifier)
                        Dim data() As String = line.Split(CChar("|"))
                        RoamingPokemonData &= Random.Next(100, 1001).ToString & "|" & data(0) & "|" & data(1) & "|" & data(2) & "|" & data(3) & "|" & data(4) & "|" & "|" & data(5) & "|" & data(6)
                    Else
                        RoamingPokemonData &= line
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub LoadStatistics()
        If IsGameJoltSave = True Then
            Statistics = GameJoltSave.Statistics
        Else
            If IO.File.Exists(GameController.GamePath & "\Save\" & filePrefix & "\Statistics.dat") = True Then
                Statistics = IO.File.ReadAllText(GameController.GamePath & "\Save\" & filePrefix & "\Statistics.dat")
            Else
                Statistics = ""
            End If
        End If
        PlayerStatistics.Load(Statistics)
    End Sub

#End Region

#Region "Save"

    Dim GameJoltTempStoreString As New Dictionary(Of String, String)

    Public Sub SaveGame(ByVal IsAutosave As Boolean)
        SaveGameHelpers.ResetSaveCounter()

        If IsAutosave = True Then
            newFilePrefix = filePrefix
            filePrefix = "autosave"

            If IO.Directory.Exists(GameController.GamePath & "\Save\autosave") = False Then
                IO.Directory.CreateDirectory(GameController.GamePath & "\Save\autosave")
            End If
        Else
            newFilePrefix = filePrefix
        End If

        GameJoltTempStoreString.Clear()

        If Not IsGameJoltSave Then
            If IO.Directory.Exists(GameController.GamePath & "\Save\" & filePrefix) = False Then
                IO.Directory.CreateDirectory(GameController.GamePath & "\Save\" & filePrefix)
            End If
        End If

        SavePlayer(IsAutosave)
        SaveParty()
        SaveItems()
        SaveBerries()
        SaveApricorns()
        SaveDaycare()
        SaveOptions()
        SavePokedex()
        SaveRegister()
        SaveItemData()
        SaveBoxData()
        SaveNPCData()
        SaveHallOfFameData()
        SaveSecretBaseData()
        SaveRoamingPokemonData()
        SaveStatistics()

        filePrefix = newFilePrefix

        If IsGameJoltSave = True Then
            If Core.GameOptions.Extras.Contains("Backup Save Feature") Then
                If Not Directory.Exists(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID.ToString() & "/Encrypted") Then
                    Directory.CreateDirectory(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID.ToString() & "/Encrypted")
                End If

                Dim OriginalHASH As String =
                Encryption.EncryptString(GetApricornsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetBerriesData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetBoxData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetDaycareData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetHallOfFameData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetItemDataData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetItemsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetNPCDataData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetOptionsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetPartyData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetPlayerData(False), StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetPokedexData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetRegisterData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetRoamingPokemonData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetSecretBaseData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) & "|" &
                Encryption.EncryptString(GetStatisticsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID))

                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID.ToString() & "/Encrypted/Encrypted.dat",
                                  OriginalHASH & "|" & Encryption.EncryptString(OriginalHASH, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)))

                Dim timestamp As String = Date.Now.ToString("yyyy-MM-dd_HH.mm.ss")

                If Not Directory.Exists(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp) Then
                    Directory.CreateDirectory(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp)
                End If

                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Apricorns.dat", GetApricornsData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Berries.dat", GetBerriesData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Box.dat", GetBoxData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Daycare.dat", GetDaycareData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/HallOfFame.dat", GetHallOfFameData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/ItemData.dat", GetItemDataData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Items.dat", GetItemsData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/NPC.dat", GetNPCDataData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Options.dat", GetOptionsData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Party.dat", GetPartyData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Player.dat", GetPlayerData(False))
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Pokedex.dat", GetPokedexData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Register.dat", GetRegisterData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/RoamingPokemon.dat", GetRoamingPokemonData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/SecretBase.dat", GetSecretBaseData)
                File.WriteAllText(GameController.GamePath & "/Backup Save/" & GameJoltSave.GameJoltID & "/" & timestamp & "/Statistics.dat", GetStatisticsData)
            End If

            Dim APICallSave As New GameJolt.APICall(AddressOf SaveGameHelpers.CompleteGameJoltSave)
            AddHandler APICallSave.CallFails, Sub(ByVal ex As Exception)
                                                  SaveGameHelpers.CompleteGameJoltSave("false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false" & Environment.NewLine & "false")
                                              End Sub

            Dim keys As New List(Of String)
            Dim dataItems As New List(Of String)
            Dim useUsername As New List(Of Boolean)

            For i = 0 To GameJoltTempStoreString.Count - 1
                keys.Add(GameJoltTempStoreString.Keys(i))
                dataItems.Add(GameJoltTempStoreString.Values(i))
                useUsername.Add(True)
            Next
            APICallSave.SetStorageData(keys.ToArray(), dataItems.ToArray(), useUsername.ToArray())

            SavePublicVars()

            GameJoltSave.UpdatePlayerScore()
        End If
    End Sub

    Private Sub SavePublicVars()
        If GameJolt.LogInScreen.UserBanned(GameJoltSave.GameJoltID) = False Then
            Dim APICallPoints As New GameJolt.APICall(AddressOf SaveGameHelpers.AddGameJoltSaveCounter)
            APICallPoints.SetStorageData("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|points", GameJoltSave.Points.ToString(), False)

            Dim APICallEmblem As New GameJolt.APICall(AddressOf SaveGameHelpers.AddGameJoltSaveCounter)
            APICallEmblem.SetStorageData("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|emblem", GameJoltSave.Emblem, False)

            Dim APICallGender As New GameJolt.APICall(AddressOf SaveGameHelpers.AddGameJoltSaveCounter)
            APICallGender.SetStorageData("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|gender", GameJoltSave.Gender, False)
        End If
    End Sub

    Public Function GetPartyData() As String
        Dim Data As String = ""
        For i = 0 To Pokemons.Count - 1
            If Data <> "" Then
                Data &= Environment.NewLine
            End If
            Data &= Pokemons(i).GetSaveData()
        Next
        Return Data
    End Function

    Public Function GetPlayerData(ByVal IsAutosave As Boolean) As String
        Dim GenderString As String = ""
        If Gender = "Male" Then
            GenderString = "Male"
        ElseIf Gender = "Female" Then
            GenderString = "Female"
        Else
            GenderString = "Other"
        End If

        Dim badgeString As String = ""
        If Badges.Count > 0 Then
            For i = 0 To Badges.Count - 1
                If i <> 0 Then
                    badgeString &= ","
                End If
                badgeString &= Badges(i).ToString()
            Next
        Else
            badgeString = "0"
        End If

        Dim hasPokedexString As String = HasPokedex.ToNumberString()

        Dim c As OverworldCamera = GetOverworldCamera()
        Dim freeCameraString As String = c.FreeCameraMode.ToNumberString()

        Dim diff As TimeSpan = (Date.Now - GameStart)
        Dim p As TimeSpan = PlayTime + diff
        Dim PlayTimeString As String = p.Hours & "," & p.Minutes & "," & p.Seconds & "," & p.Days

        Dim lastPokemonPosition As String = "999,999,999"
        If Screen.Level.OverworldPokemon.Visible = True Then
            lastPokemonPosition = (Screen.Level.OverworldPokemon.Position.X.ToString().Replace(GameController.DecSeparator, ".") & "," & Screen.Level.OverworldPokemon.Position.Y.ToString().Replace(GameController.DecSeparator, ".") & "," & Screen.Level.OverworldPokemon.Position.Z.ToString().Replace(GameController.DecSeparator, "."))
        End If

        Dim PokeFilesString As String = ""
        If PokeFiles.Count > 0 Then
            For Each pokefile As String In PokeFiles
                If PokeFilesString <> "" Then
                    PokeFilesString &= ","
                End If

                PokeFilesString &= pokefile
            Next
        End If

        Dim EarnedAchievementsString As String = ""
        If EarnedAchievements.Count > 0 Then
            For Each ea As String In EarnedAchievements
                If EarnedAchievementsString <> "" Then
                    EarnedAchievementsString &= ","
                End If

                EarnedAchievementsString &= ea
            Next
        End If

        Dim skin As String = Screen.Level.OwnPlayer.SkinName
        If Screen.Level.Riding = True Then
            skin = TempRideSkin
        End If

        Dim runToggle As Boolean = RunToggled
        If RunMode = False Then
            runToggle = False
        End If

        Dim Data As String = "Name|" & Name & Environment.NewLine &
            "Position|" & c.Position.X.ToString().Replace(GameController.DecSeparator, ".") & "," & c.Position.Y.ToString.Replace(GameController.DecSeparator, ".") & "," & c.Position.Z.ToString().Replace(GameController.DecSeparator, ".") & Environment.NewLine &
            "MapFile|" & Screen.Level.LevelFile & Environment.NewLine &
            "Rotation|" & c.Yaw.ToString.Replace(GameController.DecSeparator, ".") & Environment.NewLine &
            "RivalName|" & RivalName & Environment.NewLine &
            "RivalSkin|" & RivalSkin & Environment.NewLine &
            "Money|" & Money & Environment.NewLine &
            "Badges|" & badgeString & Environment.NewLine &
            "Gender|" & GenderString & Environment.NewLine &
            "PlayTime|" & PlayTimeString & Environment.NewLine &
            "OT|" & OT & Environment.NewLine &
            "Points|" & Points.ToString() & Environment.NewLine &
            "HasPokedex|" & hasPokedexString & Environment.NewLine &
            "HasPokegear|" & HasPokegear.ToNumberString() & Environment.NewLine &
            "FreeCamera|" & freeCameraString & Environment.NewLine &
            "ThirdPerson|" & c.ThirdPerson.ToNumberString() & Environment.NewLine &
            "Skin|" & skin & Environment.NewLine &
            "Location|" & Screen.Level.MapName & Environment.NewLine &
            "BattleAnimations|" & ShowBattleAnimations.ToString() & Environment.NewLine &
            "RunMode|" & RunMode.ToNumberString() & Environment.NewLine &
            "RunToggled|" & runToggle.ToNumberString & Environment.NewLine &
            "BoxAmount|" & BoxAmount.ToString() & Environment.NewLine &
            "LastRestPlace|" & LastRestPlace & Environment.NewLine &
            "LastRestPlacePosition|" & LastRestPlacePosition & Environment.NewLine &
            "DiagonalMovement|" & DiagonalMovement.ToNumberString() & Environment.NewLine &
            "RepelSteps|" & RepelSteps.ToString() & Environment.NewLine &
            "ScriptDelayItems|" & ScriptDelayItems.ToString() & Environment.NewLine &
            "ScriptDelaySteps|" & ScriptDelaySteps.ToString() & Environment.NewLine &
            "ScriptDelayDisplaySteps|" & ScriptDelayDisplaySteps.ToNumberString() & Environment.NewLine &
            "LastSavePlace|" & LastSavePlace & Environment.NewLine &
            "LastSavePlacePosition|" & LastSavePlacePosition & Environment.NewLine &
            "Difficulty|" & DifficultyMode.ToString() & Environment.NewLine &
            "BattleStyle|" & BattleStyle.ToString() & Environment.NewLine &
            "SaveCreated|" & SaveCreated & Environment.NewLine &
            "LastPokemonPosition|" & lastPokemonPosition & Environment.NewLine &
            "DaycareSteps|" & DaycareSteps.ToString() & Environment.NewLine &
            "GameMode|" & GameMode & Environment.NewLine &
            "PokeFiles|" & PokeFilesString & Environment.NewLine &
            "VisitedMaps|" & VisitedMaps & Environment.NewLine &
            "TempSurfSkin|" & TempSurfSkin & Environment.NewLine &
            "Surfing|" & Screen.Level.Surfing.ToNumberString() & Environment.NewLine &
            "BP|" & BP & Environment.NewLine &
            "Coins|" & Coins & Environment.NewLine &
            "ExpAll|" & EnableExpAll & Environment.NewLine &
            "ShowModels|" & ShowModelsInBattle.ToNumberString() & Environment.NewLine &
            "GTSStars|" & GTSStars & Environment.NewLine &
            "SandBoxMode|" & SandBoxMode.ToNumberString() & Environment.NewLine &
            "EarnedAchievements|" & EarnedAchievementsString

        If IsAutosave = True Then
            Data &= Environment.NewLine & "AutoSave|" & newFilePrefix
        End If

        Return Data
    End Function

    Public Function GetOptionsData() As String
        Dim c As OverworldCamera = GetOverworldCamera()

        Dim FOVstring As String = c.FOV.ToString.Replace(",", ".")
        Dim MouseSpeedString As String = CStr(c.RotationSpeed * 10000)
        Dim TextSpeedString As String = CStr(TextBox.TextSpeed)

        Dim Data As String = "FOV|" & FOVstring & Environment.NewLine &
            "TextSpeed|" & TextSpeedString & Environment.NewLine &
            "MouseSpeed|" & MouseSpeedString

        Return Data
    End Function

    Public Function GetItemsData() As String
        Dim Data As String = ""

        For Each c In Inventory
            If Data <> "" Then
                Data &= Environment.NewLine
            End If

            Data &= "{" & c.ItemID & "|" & c.Amount & "}"
        Next

        For Each mail As Items.MailItem.MailData In Mails
            If Data <> "" Then
                Data &= Environment.NewLine
            End If
            Data &= "Mail|" & Items.MailItem.GetStringFromMail(mail)
        Next

        Return Data
    End Function

    Public Function GetBerriesData() As String
        Return BerryData
    End Function

    Public Function GetApricornsData() As String
        Return ApricornData
    End Function

    Public Function GetDaycareData() As String
        Return DaycareData
    End Function

    Public Function GetPokedexData() As String
        Return PokedexData
    End Function

    Public Function GetRegisterData() As String
        Return RegisterData
    End Function

    Public Function GetItemDataData() As String
        Return ItemData
    End Function

    Public Function GetBoxData() As String
        Return BoxData
    End Function

    Public Function GetNPCDataData() As String
        Return NPCData
    End Function

    Public Function GetHallOfFameData() As String
        Return HallOfFameData
    End Function

    Public Function GetSecretBaseData() As String
        Return SecretBaseData
    End Function

    Public Function GetRoamingPokemonData() As String
        Return RoamingPokemonData
    End Function

    Public Function GetStatisticsData() As String
        Return Statistics
    End Function

    Private Function GetOverworldCamera() As OverworldCamera
        Dim baseScreen As Screen = CurrentScreen
        While Not baseScreen.PreScreen Is Nothing
            baseScreen = baseScreen.PreScreen
        End While

        If baseScreen.Identification = Screen.Identifications.BattleScreen Then
            Return CType(CType(baseScreen, BattleSystem.BattleScreen).SavedOverworld.Camera, OverworldCamera)
        ElseIf baseScreen.Identification = Screen.Identifications.CreditsScreen Then
            Return CType(CType(baseScreen, CreditsScreen).SavedOverworld.Camera, OverworldCamera)
        ElseIf baseScreen.Identification = Screen.Identifications.HallofFameScreen Then
            Return CType(CType(baseScreen, HallOfFameScreen).SavedOverworld.Camera, OverworldCamera)
        End If

        Return CType(Screen.Camera, OverworldCamera)
    End Function

    Private Sub SaveParty()
        Dim Data As String = GetPartyData()

        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|party", Data)
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Party.dat", Data)
        End If
    End Sub

    Private Sub SavePlayer(ByVal IsAutosave As Boolean)
        Dim Data As String = GetPlayerData(IsAutosave)

        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|player", Data)
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Player.dat", Data)
        End If
    End Sub

    Private Sub SaveOptions()
        Dim Data As String = GetOptionsData()

        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|options", Data)
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Options.dat", Data)
        End If
    End Sub

    Private Sub SaveItems()
        Inventory.RemoveItem(177.ToString) 'Removing Sport Balls if player has those.

        Dim Data As String = GetItemsData()

        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|items", Data)
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Items.dat", Data)
        End If
    End Sub

    Private Sub SaveBerries()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|berries", GetBerriesData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Berries.dat", GetBerriesData())
        End If
    End Sub

    Private Sub SaveApricorns()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|apricorns", GetApricornsData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Apricorns.dat", GetApricornsData())
        End If
    End Sub

    Private Sub SaveDaycare()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|daycare", GetDaycareData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Daycare.dat", GetDaycareData())
        End If
    End Sub

    Private Sub SavePokedex()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|pokedex", GetPokedexData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Pokedex.dat", GetPokedexData())
        End If
    End Sub

    Private Sub SaveRegister()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|register", GetRegisterData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Register.dat", GetRegisterData())
        End If
    End Sub

    Private Sub SaveItemData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|itemdata", GetItemDataData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\ItemData.dat", GetItemDataData())
        End If
    End Sub

    Private Sub SaveBoxData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|box", GetBoxData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Box.dat", GetBoxData())
        End If
    End Sub

    Private Sub SaveNPCData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|npc", GetNPCDataData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\NPC.dat", GetNPCDataData())
        End If
    End Sub

    Private Sub SaveHallOfFameData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|halloffame", GetHallOfFameData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\HallOfFame.dat", GetHallOfFameData())
        End If
    End Sub

    Private Sub SaveSecretBaseData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|secretbase", GetSecretBaseData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\SecretBase.dat", GetSecretBaseData())
        End If
    End Sub

    Private Sub SaveRoamingPokemonData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|roamingpokemon", GetRoamingPokemonData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\RoamingPokemon.dat", GetRoamingPokemonData())
        End If
    End Sub

    Private Sub SaveStatistics()
        Statistics = PlayerStatistics.GetData()
        If IsGameJoltSave = True Then
            GameJoltTempStoreString.Add("saveStorageV" & GameJolt.GamejoltSave.VERSION & "|" & GameJoltSave.GameJoltID & "|statistics", GetStatisticsData())
        Else
            IO.File.WriteAllText(GameController.GamePath & "\Save\" & filePrefix & "\Statistics.dat", GetStatisticsData())
        End If
    End Sub

#End Region

#Region "Heal"

    Public Sub HealParty()
        For i = 0 To Pokemons.Count - 1
            Pokemons(i).FullRestore()
        Next
    End Sub

    Public Sub HealParty(ByVal Members() As Integer)
        For Each member As Integer In Members
            If Pokemons.Count - 1 >= member Then
                Pokemons(member).FullRestore()
            End If
        Next
    End Sub

#End Region

#Region "Pokemon"

    Public ReadOnly Property CountFightablePokemon() As Integer
        Get
            Dim i As Integer = 0

            For Each Pokemon As Pokemon In Pokemons
                If Pokemon.Status <> Pokemon.StatusProblems.Fainted And Pokemon.EggSteps = 0 And Pokemon.HP > 0 Then
                    i += 1
                End If
            Next
            Return i
        End Get
    End Property

    Public ReadOnly Property CanCatchPokémon() As Boolean
        Get
            Dim data() As String = BoxData.ToArray("§")
            If data.Count >= BoxAmount * 30 Then
                Return False
            End If
            Return True
        End Get
    End Property

    Public ReadOnly Property SurfPokemon() As Integer
        Get
            For i = 0 To Pokemons.Count - 1
                Dim p As Pokemon = Pokemons(i)

                If p.IsEgg() = False Then
                    For Each a As BattleSystem.Attack In p.Attacks
                        If a.Name.ToLower() = "surf" Then
                            Return i
                        End If
                    Next
                End If
            Next
            If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                Return 0
            Else
                Return -1
            End If
        End Get
    End Property

    Public Function GetWalkPokemon() As Pokemon
        If Pokemons.Count = 0 Then
            Return Nothing
        End If

        For i = 0 To Pokemons.Count - 1
            If Pokemons(i).Status <> Pokemon.StatusProblems.Fainted And Pokemons(i).IsEgg() = False Then
                Return Pokemons(i)
            End If
        Next
        Return Nothing
    End Function

    Public Function GetValidPokemonCount() As Integer
        Dim c As Integer = 0
        For Each p As Pokemon In Core.Player.Pokemons
            If p.Status <> Pokemon.StatusProblems.Fainted And p.EggSteps = 0 Then
                c += 1
            End If
        Next
        Return c
    End Function

#End Region

#Region "Steps"

    Public IsFlying As Boolean = False

    '===STEP EVENT INFORMATION===
    'Events when taking a step	| Priority	| Event Type    | Resolution if Not fired
    '---------------------------|-----------|---------------|--------------------------------------------------------------------------------
    'ScriptBlock trigger		| 0		    | ScriptBlock	| Always fire!
    'Trainer Is in sight		| 1		    | Script		| Ignore, will be activated when walked by on a different tile. Design failure.
    'Egg hatches			    | 2		    | Screen change	| Will happen On Next Step automatically.
    'Repel wears out			| 3		    | Script		| Add one Step To the repel counter, so the Event happens On the Next Step.
    'Wild Pokémon appears		| 4		    | WildPokemon	| Just ignore, random Event
    'Pokegear call			    | 5		    | Script		| Just ignore, Not too important
    '----------------------------------------------------------------------------------------------------------------------------------------
    'All Script Events need a special check condition set.
    'Script Blocks are handled externally.
    '
    'Additional things to do that always fire:
    ' - Set the player's LastPosition
    ' - Add to the daycare cycle, if it finishes, do daycare events, add to the friendship value of Pokémon, add points and check or following pokemon pickup.
    ' - Apply shaders to following pokemon and player, and make following pokemon visible
    ' - make wild Pokémon noises
    ' - add to the Temp map step count
    ' - track the statistic for walked steps.

    Public _stepEventStartedTrainer As Boolean = False
    Private _stepEventRepelMessage As Boolean = False
    Private _stepEventEggHatched As Boolean = False

    Public Sub TakeStep(ByVal stepAmount As Integer)
        _stepEventEggHatched = False
        _stepEventRepelMessage = False
        _stepEventStartedTrainer = False

        If IsFlying = False Then
            'Set the last position:
            Temp.LastPosition = Screen.Camera.Position

            'Increment step counters:
            Screen.Level.WalkedSteps += 1
            Temp.MapSteps += 1
            DaycareSteps += stepAmount
            StepEventPoison(stepAmount)

            PlayerStatistics.Track("Steps taken", stepAmount)

            'Daycare cycle:
            PlayerTemp.DayCareCycle -= stepAmount
            If PlayerTemp.DayCareCycle <= 0 Then
                Daycare.EggCircle()

                'Every 256 steps, add friendship to the Pokémon in the player's team.
                For Each p As Pokemon In Pokemons
                    If p.Status <> Pokemon.StatusProblems.Fainted And p.IsEgg() = False Then
                        p.ChangeFriendShip(Pokemon.FriendShipCauses.Walking)
                    End If
                Next

                AddPoints(1, "Completed an Egg Circle.")

                PokemonInteractions.CheckForRandomPickup()
            End If

            'Apply shaders and set following pokemon:
            Screen.Level.OwnPlayer.ApplyShaders()
            Screen.Level.OverworldPokemon.ApplyShaders()

            Screen.Level.OverworldPokemon.ChangeRotation()
            Screen.Level.OverworldPokemon.MakeVisible()

            'Make wild pokemon noises:
            MakeWildPokemonNoise()

            StepEventCheckTrainers()
            StepEventCheckEggHatching(stepAmount)
            StepEventCheckRepel(stepAmount)
            StepEventCheckScript(stepAmount)
            StepEventWildPokemon()
            StepEventPokegearCall()
        Else
            IsFlying = False
        End If
    End Sub

    Public Sub StepEventCheckTrainers()
        If CanFireStepEvent() = True Then
            Screen.Level.CheckTrainerSights()
            If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                If CType(CurrentScreen, OverworldScreen).ActionScript.IsReady = False Then
                    _stepEventStartedTrainer = True
                End If
            End If
        Else
            If Screen.Level.PokemonEncounterData.EncounteredPokemon = True OrElse (CurrentScreen.Identification = Screen.Identifications.OverworldScreen AndAlso CType(CurrentScreen, OverworldScreen).TrainerEncountered = True) Then
                CheckForTrainersLater = True
            End If
        End If
    End Sub

    Private Sub StepEventCheckEggHatching(ByVal stepAmount As Integer)
        If CanFireStepEvent() = True Then
            Dim addEggSteps As Integer = stepAmount
            For Each p As Pokemon In Pokemons
                If p.Ability IsNot Nothing Then
                    If p.Ability.Name.ToLower() = "magma armor" OrElse p.Ability.Name.ToLower() = "flame body" Then
                        addEggSteps *= Random.Next(1, 4)
                        Exit For
                    End If
                End If
            Next

            Dim eggsReady As New List(Of Pokemon)
            For Each p As Pokemon In Pokemons
                If p.EggSteps > 0 Then
                    p.EggSteps += addEggSteps
                    If p.EggSteps >= p.BaseEggSteps Then
                        eggsReady.Add(p)
                    End If
                End If
            Next

            If eggsReady.Count > 0 Then
                For Each p As Pokemon In eggsReady
                    Pokemons.Remove(p)
                Next

                Screen.TextBox.Show("Huh?")
                SetScreen(New TransitionScreen(CurrentScreen, New HatchEggScreen(CurrentScreen, eggsReady), Color.White, False))

                _stepEventEggHatched = True
            End If
        End If
    End Sub

    Private Sub StepEventCheckRepel(ByVal stepAmount As Integer)
        If RepelSteps > 0 Then
            RepelSteps -= stepAmount

            If RepelSteps <= 0 Then
                If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    If CanFireStepEvent() = True Then
                        Screen.Level.WalkedSteps = 0

                        Dim s As String = "version=2" & Environment.NewLine &
                                        "@Text.Show(Your repel effect wore off.)" & Environment.NewLine &
                                        ":end"

                        If Temp.LastUsedRepel > -1 Then
                            Dim haveItemLeft As Boolean = Inventory.GetItemAmount(Temp.LastUsedRepel.ToString) > 0

                            If haveItemLeft = True Then
                                s = "version=2" & Environment.NewLine &
                                    "@Text.Show(Your repel effect wore off.*Do you want to use~another <inventory.name(" & Temp.LastUsedRepel & ")>?)" & Environment.NewLine &
                                    "@Options.Show(Yes,No)" & Environment.NewLine &
                                    ":when:Yes" & Environment.NewLine &
                                    "@sound.play(Use_Repel)" & Environment.NewLine &
                                    "@Text.Show(<player.name> used~a <inventory.name(" & Temp.LastUsedRepel & ")>.)" & Environment.NewLine &
                                    "@item.repel(" & Temp.LastUsedRepel & ")" & Environment.NewLine &
                                    "@item.remove(" & Temp.LastUsedRepel & ",1,0)" & Environment.NewLine &
                                    ":endwhen" & Environment.NewLine &
                                    ":end"
                            End If
                        End If

                        CType(CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
                        _stepEventRepelMessage = True
                    Else
                        _repelSteps = 1
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub StepEventCheckScript(ByVal stepAmount As Integer)
        If ScriptDelaySteps > 0 Then
            ScriptDelaySteps -= stepAmount

            If ScriptDelaySteps <= 0 Then
                If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    If CanFireStepEvent() = True Then
                        Dim Data() As String = Core.Player.RegisterData.Split(CChar(","))
                        Dim StepDelayRegisterName As String = ""
                        For Each line As String In Data
                            If line.StartsWith("[") = True And line.Contains("]") = True And line.EndsWith("]") = False Then
                                Dim lineName As String = line.Remove(0, line.IndexOf("]") + 1)
                                Dim delayID As String = ""
                                If lineName.StartsWith("SCRIPTDELAY_") Then
                                    Dim registerContent() As Object = ActionScript.GetRegisterValue(lineName)
                                    Dim delayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                                    If delayType.ToLower = "steps" Then
                                        StepDelayRegisterName = lineName
                                    End If
                                End If
                            End If
                        Next
                        If StepDelayRegisterName <> "" Then
                            ScriptDelayDisplaySteps = False
                            Dim registerContent() As Object = ActionScript.GetRegisterValue(StepDelayRegisterName)

                            If registerContent(0) Is Nothing Or registerContent(1) Is Nothing Then
                                Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: No valid script has been set to be executed.")
                                ActionScript.UnregisterID(StepDelayRegisterName, "str")
                                ActionScript.UnregisterID(StepDelayRegisterName)
                                Exit Sub
                            End If

                            Dim DelayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                            If DelayType.ToLower = "steps" Then
                                Dim DelayValue As String = CStr(registerContent(0)).GetSplit(1, ";")

                                CType(CurrentScreen, OverworldScreen).ActionScript.StartScript(DelayValue, 0, False)
                                ActionScript.UnregisterID(StepDelayRegisterName, "str")
                                ActionScript.UnregisterID(StepDelayRegisterName)
                            End If
                        End If
                    Else
                        ScriptDelaySteps = 1
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub StepEventWildPokemon()
        If CanFireStepEvent() = True Then
            If Screen.Level.WildPokemonFloor = True And Screen.Level.Surfing = False Then
                Screen.Level.PokemonEncounter.TryEncounterWildPokemon(Screen.Camera.Position, Spawner.EncounterMethods.Land, "")
            End If
        End If
    End Sub
    Private Sub StepEventPoison(ByVal stepAmount As Integer)
        If CanFireStepEvent() = True Then
            If CInt(GameModeManager.GetGameRuleValue("OverworldPoison", "0")) > 0 Then
                Dim PoisonAmount As Integer = 0
                PoisonSteps += stepAmount
                If PoisonSteps >= 3 Then
                    PoisonAmount += 1
                    PoisonSteps -= 4
                End If
                If PoisonAmount > 0 Then
                    Dim PlayPoisonSound As Boolean = False
                    For i = 0 To Core.Player.Pokemons.Count - 1
                        If Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Poison OrElse Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.BadPoison Then
                            PlayPoisonSound = True
                            Exit For
                        End If
                    Next
                    If PlayPoisonSound = True Then
                        SoundManager.PlaySound("OverworldPoison")
                    End If
                    For i = 0 To Core.Player.Pokemons.Count - 1
                        If Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Poison OrElse Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.BadPoison Then
                            If Core.Player.Pokemons(i).Ability IsNot Ability.GetAbilityByID(17) Then
                                Core.Player.Pokemons(i).HP -= 1 * PoisonAmount
                            End If
                        End If
                    Next
                    If CInt(GameModeManager.GetGameRuleValue("OverworldPoison", "0")) = 1 Then
                        For i = 0 To Core.Player.Pokemons.Count - 1
                            If Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Poison OrElse Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.BadPoison Then
                                If Core.Player.Pokemons(i).HP <= 1 Then
                                    Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.None
                                    Core.Player.Pokemons(i).HP = 1
                                    Screen.TextBox.Show(Core.Player.Pokemons(i).GetDisplayName & " was cured~of Poison.")
                                End If
                            End If
                        Next
                    ElseIf CInt(GameModeManager.GetGameRuleValue("OverworldPoison", "0")) = 2 Then
                        For i = 0 To Core.Player.Pokemons.Count - 1
                            If Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Poison OrElse Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.BadPoison Then
                                If Core.Player.Pokemons(i).HP <= 0 Then
                                    Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted
                                    Screen.TextBox.Show(Core.Player.Pokemons(i).GetDisplayName & "Fainted.")

                                    'Remove fainted Pokémon from player's team if the DeathInsteadOfFaint GameRule is activated.
                                    If CBool(GameModeManager.GetGameRuleValue("DeathInsteadOfFaint", "0")) = True Then
                                        If i <= Core.Player.Pokemons.Count - 1 Then
                                            If Core.Player.Pokemons(i).HP <= 0 Or Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted Then
                                                Core.Player.Pokemons.RemoveAt(i)
                                                i -= 1
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            If Core.Player.CountFightablePokemon = 0 Then
                                If Screen.Level.BlackOutScript <> "" Then
                                    CType(CurrentScreen, OverworldScreen).ActionScript.StartScript(Screen.Level.BlackOutScript, 0, False)
                                Else
                                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New BlackOutScreen(Core.CurrentScreen), Color.Black, False))
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub StepEventPokegearCall()
        If CanFireStepEvent() = True Then
            If Temp.MapSteps > 0 Then
                If Temp.LastCall < 256 Then
                    Temp.LastCall += 1
                Else
                    If Random.Next(0, 700) = 0 Then
                        GameJolt.PokegearScreen.RandomCall()
                        Temp.LastCall = 0
                    End If
                End If
            End If
        End If
    End Sub

    Private Function CanFireStepEvent() As Boolean
        If ScriptBlock.TriggeredScriptBlock = False Then
            If _stepEventStartedTrainer = False Then
                If _stepEventEggHatched = False Then
                    If _stepEventRepelMessage = False Then
                        If Screen.Level.PokemonEncounterData.EncounteredPokemon = False Then
                            Return True
                        End If
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Private Sub MakeWildPokemonNoise()
        If Screen.Level.WildPokemonGrass = True Then
            If Random.Next(0, 193) = 0 Then
                Dim p As Pokemon = Spawner.GetPokemon(Screen.Level.LevelFile, Spawner.EncounterMethods.Land, False, "")

                If Not p Is Nothing Then
                    PlayWildPokemonNoise(p.Number)
                End If
            End If
        End If
        If Screen.Level.WildPokemonFloor = True Then
            If Random.Next(0, 193) = 0 Then
                Dim p As Pokemon = Spawner.GetPokemon(Screen.Level.LevelFile, Spawner.EncounterMethods.Land, False, "")

                If Not p Is Nothing Then
                    PlayWildPokemonNoise(p.Number)
                    Exit Sub
                End If
            End If
        End If
        If Screen.Level.WildPokemonWater = True Then
            If Random.Next(0, 193) = 0 Then
                Dim p As Pokemon = Spawner.GetPokemon(Screen.Level.LevelFile, Spawner.EncounterMethods.Surfing, False, "")

                If Not p Is Nothing Then
                    PlayWildPokemonNoise(p.Number)
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub PlayWildPokemonNoise(ByVal number As Integer)
        SoundManager.PlayPokemonCry(number, Random.Next(0, 6) / 10.0F, Random.Next(0, 20) / 10.0F - 1, SoundManager.Volume * 0.5F)
    End Sub

#End Region

    Public Sub CheckItemCountScriptDelay(itemID As String)
        Dim ItemDelayList As List(Of String) = Core.Player.ScriptDelayItems.Split(";").ToList
        For Each entry As String In ItemDelayList
            If entry.GetSplit(1, ",").ToLower = itemID.ToLower Then
                Dim inventoryAmount As Integer = Core.Player.Inventory.GetItemAmount(itemID)
                Dim Match As Boolean = False
                Select Case entry.GetSplit(2, ",")
                    Case "=", "equal"
                        If inventoryAmount = CInt(entry.GetSplit(3, ",")) Then
                            Match = True
                        End If
                    Case "<", "below"
                        If inventoryAmount < CInt(entry.GetSplit(3, ",")) Then
                            Match = True
                        End If
                    Case "<=", "equalorbelow"
                        If inventoryAmount <= CInt(entry.GetSplit(3, ",")) Then
                            Match = True
                        End If
                    Case ">", "above"
                        If inventoryAmount > CInt(entry.GetSplit(3, ",")) Then
                            Match = True
                        End If
                    Case ">=", "equalorabove"
                        If inventoryAmount >= CInt(entry.GetSplit(3, ",")) Then
                            Match = True
                        End If
                End Select

                If Match = True Then
                    Dim DelayRegisterName As String = "SCRIPTDELAY_" & entry.GetSplit(0, ",")

                    Dim Data() As String = Core.Player.RegisterData.Split(CChar(","))
                    ScriptDelayDisplaySteps = False
                    Dim registerContent() As Object = ActionScript.GetRegisterValue(DelayRegisterName)

                    If registerContent(0) Is Nothing Or registerContent(1) Is Nothing Then
                        Logger.Log(Logger.LogTypes.Warning, "ScriptComparer.vb: No valid script has been set to be executed.")
                        ActionScript.UnregisterID(DelayRegisterName, "str")
                        ActionScript.UnregisterID(DelayRegisterName)
                    End If

                    Dim DelayType As String = CStr(registerContent(0)).GetSplit(0, ";")
                    If DelayType.ToLower = "itemcount" Then
                        Dim DelayValue As String = CStr(registerContent(0)).GetSplit(1, ";")

                        Dim s As Screen = CurrentScreen
                        While s.Identification <> Screen.Identifications.OverworldScreen And Not s.PreScreen Is Nothing
                            s = s.PreScreen
                        End While
                        If s.Identification = Screen.Identifications.OverworldScreen Then
                            CType(CurrentScreen, OverworldScreen).ActionScript.StartScript(DelayValue, 0, False)

                            ActionScript.UnregisterID(DelayRegisterName, "str")
                            ActionScript.UnregisterID(DelayRegisterName)
                            ItemDelayList.Remove(entry)
                            Exit For
                        End If
                    End If
                End If
            End If
        Next
        Core.Player.ScriptDelayItems = String.Join(";", ItemDelayList)

    End Sub

    Public Sub AddVisitedMap(ByVal mapFile As String)
        Dim maps As List(Of String) = VisitedMaps.Split(CChar(",")).ToList()

        If maps.Contains(mapFile) = False Then
            maps.Add(mapFile)
            VisitedMaps = ""
            For Each map As String In maps
                If VisitedMaps <> "" Then
                    VisitedMaps &= ","
                End If
                VisitedMaps &= map
            Next
        End If
    End Sub

    Public Sub AddPoints(ByVal amount As Integer, ByVal reason As String)
        Dim addPoints As Integer = amount

        For Each mysteryEvent As MysteryEventScreen.MysteryEvent In MysteryEventScreen.ActivatedMysteryEvents
            If mysteryEvent.EventType = MysteryEventScreen.EventTypes.PointsMultiplier Then
                addPoints = CInt(addPoints * CSng(mysteryEvent.Value.Replace(".", GameController.DecSeparator)))
            End If
        Next

        If IsGameJoltSave = True Then
            If GameJolt.LogInScreen.UserBanned(GameJoltSave.GameJoltID) = False Then
                GameJoltSave.Points += addPoints
            End If
        Else
            Points += addPoints
        End If

        HistoryScreen.HistoryHandler.AddHistoryItem("Obtained game points", "Amount: " & addPoints.ToString() & "; Reason: " & reason, False, False)
    End Sub

    Public Sub ResetNewLevel()
        lastLevel = 0
        displayEmblemDelay = 0.0F
        emblemPositionX = windowSize.Width
    End Sub

    Dim lastLevel As Integer = 0
    Dim displayEmblemDelay As Single = 0.0F
    Dim emblemPositionX As Integer = windowSize.Width

    Public Sub DrawLevelUp()
        If IsGameJoltSave = True Then
            If lastLevel <> GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points) And lastLevel <> 0 Then
                lastLevel = GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points)
                displayEmblemDelay = 35.0F
                Skin = GameJolt.Emblem.GetPlayerSpriteFile(lastLevel, GameJoltSave.GameJoltID, GameJoltSave.Gender)
            End If

            If displayEmblemDelay > 0.0F Then
                displayEmblemDelay -= 0.1F
                If displayEmblemDelay <= 6.4F Then
                    If emblemPositionX < windowSize.Width Then
                        emblemPositionX += 8
                    End If
                Else
                    If emblemPositionX > windowSize.Width - 512 Then
                        emblemPositionX -= 8
                    End If
                End If

                GameJolt.Emblem.Draw(GameJolt.API.username, GameJoltSave.GameJoltID, GameJoltSave.Points, GameJoltSave.Gender, GameJoltSave.Emblem, New Vector2(emblemPositionX, 0), 4, GameJoltSave.DownloadedSprite)

                If displayEmblemDelay <= 0.0F Then
                    displayEmblemDelay = 0.0F
                    emblemPositionX = windowSize.Width
                End If
            End If
        End If
    End Sub

    Public Shared Function IsSaveGameFolder(ByVal folder As String) As Boolean
        If IO.Directory.Exists(folder) = True Then
            Dim files() As String = {"Apricorns", "Berries", "Box", "Daycare", "HallOfFame", "ItemData", "Items", "NPC", "Options", "Party", "Player", "Pokedex", "Register", "RoamingPokemon", "SecretBase"}
            For Each file As String In files
                If IO.File.Exists(folder & "\" & file & ".dat") = False Then
                    Return False
                End If
            Next
            Return True
        End If
        Return False
    End Function

    Public Function IsRunning() As Boolean
        If RunMode = True Then
            Return RunToggled
        ElseIf KeyBoardHandler.KeyDown(Keys.LeftShift) = True Or ControllerHandler.ButtonDown(Buttons.B) = True Then
            If Screen.Level.Riding = False And Screen.Level.Surfing = False And Inventory.HasRunningShoes = True Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Sub Unload()
        'This function clears all data from the loaded player and restores the default values.

        'Clearning lists:
        Pokemons.Clear()
        Pokedexes.Clear()
        Inventory.Clear()
        Badges.Clear()
        PokeFiles.Clear()
        EarnedAchievements.Clear()
        PokegearModules.Clear()
        PhoneContacts.Clear()
        Mails.Clear()
        Trophies.Clear()

        'Restore default values:
        Name = "<player.name>"
        RivalName = ""
        RivalSkin = ""
        Money = 0
        PlayTime = TimeSpan.Zero
        GameStart = Date.Now
        OT = "00000"
        Points = 0
        BP = 0
        Coins = 0
        HasPokedex = False
        HasPokegear = False
        ShowBattleAnimations = 1
        BoxAmount = 10
        LastRestPlace = "yourroom.dat"
        LastRestPlacePosition = "1,0.1,3"
        LastSavePlace = "yourroom.dat"
        LastSavePlacePosition = "1,0.1,3"
        DiagonalMovement = False
        RepelSteps = 0
        DifficultyMode = 0
        BattleStyle = 0
        ShowModelsInBattle = True
        SaveCreated = "Pre 0.21"
        LastPokemonPosition = New Vector3(999)
        DaycareSteps = 0
        GameMode = "Kolben"
        VisitedMaps = ""
        TempSurfSkin = "Hilbert"
        TempRideSkin = ""
        GTSStars = 8
        SandBoxMode = False
        Statistics = ""
        startPosition = New Vector3(14, 0.1, 10)
        startRotation = 0
        startFreeCameraMode = False
        startMap = "barktown.dat"
        startFOV = 45.0F
        startRotationSpeed = 12
        startThirdPerson = False
        startSurfing = False
        startRiding = False
        Skin = "Hilbert"

        'Clear temp save data:
        RegisterData = ""
        BerryData = ""
        PokedexData = ""
        ItemData = ""
        BoxData = ""
        NPCData = ""
        ApricornData = ""
        SecretBaseData = ""
        DaycareData = ""
        HallOfFameData = ""
        RoamingPokemonData = ""
        UsedItemsToCheckScriptDelayFor.Clear()

        filePrefix = "nilllzz"
        newFilePrefix = ""
        AutosaveUsed = False
        loadedSave = False

        IsGameJoltSave = False
        EmblemBackground = "standard"

        ResetNewLevel()
    End Sub

End Class