﻿
Public Class Presence

    ReadOnly Discord_App_ID As String = Classified.Discord_Application_ID
    ReadOnly Default_Details As String = "" ' Defaults - DO NOT CHANGE
    ReadOnly Default_State As String = "Playing Solo" ' Defaults - DO NOT CHANGE
    ReadOnly Default_LargeImageName As String = "default" ' Defaults - DO NOT CHANGE
    ReadOnly Default_LargeImageText As String = "Pokemon 3D" ' Defaults - DO NOT CHANGE
    ReadOnly Default_SmallImageName As String = "" ' Defaults - DO NOT CHANGE
    ReadOnly Default_SmallImageText As String = "" ' Defaults - DO NOT CHANGE
    ReadOnly Default_Party_Size As Integer = 0 ' Defaults - DO NOT CHANGE
    ReadOnly Default_Party_Size_Max As Integer = 0 ' Defaults - DO NOT CHANGE
    Private Handlers As DiscordEventHandlers = New DiscordEventHandlers With {.Ready = AddressOf IsReady}

    Private PreviousScreen As String = ""
    Private PreviousMapLevel As String = ""
    Private StartTimestamp As Long = Nothing
    Dim MapLevelFiles As New List(Of String)

    Public Sub Initialize()

        ' Checking valid filenames, in directory. These files also needs to be uploaded to the application
        ' https://discord.com/developers/applications/*APPLICATION_ID_HERE*/rich-presence/assets
        ' DanielRTRD has access here, ask if you need access!
        For Each RPC_Image In Directory.GetFiles(GameController.GamePath & "\Content\DiscordRPC\")
            MapLevelFiles.Add(Path.GetFileName(RPC_Image).Replace(".png", "").Replace(".jpg", "").Replace(".jpeg", ""))
            'Logger.Log(Logger.LogTypes.Debug, "Presence.vb: Adding file to MapLevelFiles: '" & RPC_Image & "'")
        Next

        ' Start Discord
        If Discord_App_ID IsNot Nothing And Environment.Is64BitProcess = False Then
            Discord_Initialize(Discord_App_ID, Handlers, 1, "")
            Update()
            StartTimestamp = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
            Logger.Log(Logger.LogTypes.Message, "Presence.vb: Discord RPC Initialized!")
        End If
    End Sub

    Private Sub IsReady(ByRef Request As DiscordUser)
        ' Do nothing
    End Sub

    Public Sub Update()

        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: Checking Discord Presence.")

        If Core.GameOptions.DiscordRPCEnabled = False Then
            ' Shutdown Discord if user decides to disable it in game
            ' Restart of game is needed after enabling again to make it initialize again
            Discord_Shutdown()
        End If

        ' Description of fields is found here
        ' https://discord.com/developers/docs/rich-presence/how-to#updating-presence-update-presence-payload-fields

        ' Reset the local variables
        Dim Presence_Details As String = Default_Details
        Dim Presence_State As String = Default_State
        Dim Presence_LargeImageName As String = Default_LargeImageName
        Dim Presence_LargeImageText As String = Default_LargeImageText
        Dim Presence_SmallImageName As String = Default_SmallImageName
        Dim Presence_SmallImageText As String = Default_SmallImageText
        Dim Presence_Party_Size As Integer = Default_Party_Size
        Dim Presence_Party_Size_Max As Integer = Default_Party_Size_Max

        ' WHAT SHOULD THE FIELDS BE USED FOR?
        ' 
        ' Presence_Details: Should be used for current state the user is in the game
        ' Default should be: ""
        ' Example: "In Goldenrod City"
        ' 
        ' Presence_State: Show if the user is in solo or multiplayer
        ' Default should be: "Playing Solo"
        ' Example: "Playing Solo" or "Playing Multiplayer"
        ' 
        ' Presence_LargeImageName: Visible image in discord
        ' Default should be: "default"
        ' Example: "default" or when in a specific place "goldenrod_city"
        ' 
        ' Presence_LargeImageText: Visible name of logo in discord
        ' Default should be: "Pokemon 3D"
        ' Example: "Pokemon 3D" or when in a specific place "Goldenrod City"
        ' 
        ' Presence_SmallImageName: Visible image in discord
        ' Default should be: ""
        ' Example: "default" only when user is in a specific place else default
        ' 
        ' Presence_SmallImageText: Visible name of logo in discord
        ' Default should be: ""
        ' Example: "Pokemon 3D" only when user is in a specific place else default
        ' 
        ' Presence_Party_Size: Visible name of logo in discord
        ' Default should be: "0"
        ' Example: Will be used for servers in the future
        ' 
        ' Presence_Party_Size_Max: Visible name of logo in discord
        ' Default should be: "0"
        ' Example: Will be used for servers in the future
        ' 

        Dim ShouldUpdate As Boolean = False

        Dim CurrentScreen As String = Core.CurrentScreen.ToString().Replace("P3D.", "")
        Dim MenuScreens() As String = {"NewMainMenuScreen", "PauseScreen", "PressStartScreen", "JoinServerScreen", "NewMenuScreen", "AddServerScreen", "ConnectScreen", "NewTrainerScreen", "GameJolt.LogInScreen", "PartyScreen", "NewOptionScreen", "SaveScreen", "TransitionScreen", "SplashScreen"}
        Dim PokedexScreens() As String = {"PokedexSelectScreen", "PokedexScreen", "PokedexViewScreen", "PokedexHabitatScreen"}
        Dim BattleScreens() As String = {"BattleIntroScreen", "BattleSystem.BattleScreen"}
        Dim InventoryScreens() As String = {"NewInventoryScreen"}
        Dim CurrentMapLevel As String = ""
        Dim CurrentMapLevelFileName As String = ""

        CurrentMapLevel = GetCurrentMapLevel()

        'Logger.Log(Logger.LogTypes.Debug, "Presence.vb: PreviousScreen: " & PreviousScreen)
        'Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentScreen: " & CurrentScreen)

        'Logger.Log(Logger.LogTypes.Debug, "Presence.vb: PreviousMapLevel: " & PreviousMapLevel)
        'Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentMapLevel: " & CurrentMapLevel)

        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            If CurrentMapLevel <> PreviousMapLevel Then
                PreviousMapLevel = CurrentMapLevel
            End If
            CurrentMapLevelFileName = GetCurrentMapLevelFileName()
            If CurrentMapLevel = "Pokemon Center" Or CurrentMapLevel = "Pokemon Mart" Then
                CurrentMapLevel = "a " & CurrentMapLevel
            End If
            Presence_Details = "In " & CurrentMapLevel
            Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentMapLevelFileName: '" & CurrentMapLevelFileName & "'")
            If MapLevelFiles.Contains(CurrentMapLevelFileName) Then
                Presence_LargeImageName = CurrentMapLevelFileName
                Presence_LargeImageText = CurrentMapLevel
                Presence_SmallImageName = Default_LargeImageName ' When a map is show, icon should be moved to small
                Presence_SmallImageText = Default_LargeImageText ' When a map is show, icon should be moved to small
            End If
            ShouldUpdate = True
        ElseIf CurrentScreen IsNot PreviousScreen Then
            PreviousScreen = CurrentScreen
            ShouldUpdate = True
            If MenuScreens.Contains(CurrentScreen) Then
                Presence_Details = "In the menus"
            ElseIf PokedexScreens.Contains(CurrentScreen) Then
                Presence_Details = "In the Pokedex"
            ElseIf BattleScreens.Contains(CurrentScreen) Then
                Presence_Details = "In a battle"
            ElseIf InventoryScreens.Contains(CurrentScreen) Then
                Presence_Details = "In the inventory"
            ElseIf GameController.IS_DEBUG_ACTIVE = True Then
                Presence_Details = "Debuging: " & CurrentScreen ' This is just for debug purposes
                ShouldUpdate = True ' This is just for debug purposes
            Else
                ShouldUpdate = False
            End If
        End If

        If ServersManager.ServerConnection.Connected = True Then
            Presence_State = "Playing Multiplayer"
        Else
            Presence_State = "Playing Solo"
        End If

        Dim Presence As DiscordRichPresence = New DiscordRichPresence With {
            .Details = Presence_Details,
            .State = Presence_State,
            .LargeImageKey = Presence_LargeImageName,
            .LargeImageText = Presence_LargeImageText,
            .SmallImageKey = Presence_SmallImageName,
            .SmallImageText = Presence_SmallImageText,
            .PartySize = Presence_Party_Size,
            .PartyMax = Presence_Party_Size_Max,
            .StartTimestamp = StartTimestamp
        }

        If ShouldUpdate And Environment.Is64BitProcess = False Then
            Logger.Log(Logger.LogTypes.Debug, "Presence.vb: Updating Discord RPC.")
            Discord_UpdatePresence(Presence)
        End If

    End Sub

    Public Function GetCurrentMapLevel() As String
        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            Return Screen.Level.MapName
        End If
        Return ""
    End Function

    Public Function GetCurrentMapLevelFileName() As String
        Return GetCurrentMapLevel.ToLower.Replace(" ", "_").Replace(".", "").Replace("'", "")
    End Function

End Class
