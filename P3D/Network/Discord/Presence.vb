
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

    Public Sub Initialize()
        If Discord_App_ID IsNot Nothing And Environment.Is64BitProcess = False Then
            Discord_Initialize(Discord_App_ID, Handlers, 1, "")
            Update()
            StartTimestamp = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        End If
    End Sub

    Private Sub IsReady(ByRef Request As DiscordUser)
        ' Do nothing
    End Sub

    Public Sub Update()

        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: Checking Discord Presence.")

        If Core.GameOptions.DiscordRPCEnabled = False Then
            Discord_Shutdown()
        End If

        ' Description of fields is found here
        ' https://discord.com/developers/docs/rich-presence/how-to#updating-presence-update-presence-payload-fields

        Dim APP_Details As String = "" ' Defaults - DO NOT CHANGE
        Dim APP_State As String = "Playing Solo" ' Defaults - DO NOT CHANGE
        Dim APP_LargeImageName As String = "default" ' Defaults - DO NOT CHANGE
        Dim APP_LargeImageText As String = "Pokemon 3D" ' Defaults - DO NOT CHANGE
        Dim APP_SmallImageName As String = "" ' Defaults - DO NOT CHANGE
        Dim APP_SmallImageText As String = "" ' Defaults - DO NOT CHANGE
        Dim APP_Party_Size As Integer = 0 ' Defaults - DO NOT CHANGE
        Dim APP_Party_Size_Max As Integer = 0 ' Defaults - DO NOT CHANGE

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
        Dim CurrentMapLevelFileNames() As String = {"goldenrod_city"}

        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            CurrentMapLevel = Screen.Level.MapName
        End If

        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: PreviousScreen: " & PreviousScreen)
        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentScreen: " & CurrentScreen)

        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: PreviousMapLevel: " & PreviousMapLevel)
        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentMapLevel: " & CurrentMapLevel)

        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            If CurrentMapLevel <> PreviousMapLevel Then
                PreviousMapLevel = CurrentMapLevel
            End If
            CurrentMapLevelFileName = CurrentMapLevel.ToLower.Replace(" ", "_")
            APP_Details = "In " & CurrentMapLevel
            Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentMapLevelFileName: " & CurrentMapLevelFileName)
            If CurrentMapLevelFileNames.Contains(CurrentMapLevelFileName) Then
                APP_LargeImageName = CurrentMapLevel.ToLower.Replace(" ", "_").Replace(".", "")
                APP_LargeImageText = CurrentMapLevel
                APP_SmallImageName = "default" ' Defaults - DO NOT CHANGE
                APP_SmallImageText = "Pokemon 3D" ' Defaults - DO NOT CHANGE
            End If
            ShouldUpdate = True
        ElseIf CurrentScreen IsNot PreviousScreen Then
            PreviousScreen = CurrentScreen
            ShouldUpdate = True
            If MenuScreens.Contains(CurrentScreen) Then
                APP_Details = "In the menus"
            ElseIf PokedexScreens.Contains(CurrentScreen) Then
                APP_Details = "In the Pokedex"
            ElseIf BattleScreens.Contains(CurrentScreen) Then
                APP_Details = "In a battle"
            ElseIf InventoryScreens.Contains(CurrentScreen) Then
                APP_Details = "In the inventory"
            ElseIf GameController.IS_DEBUG_ACTIVE = True Then
                APP_Details = "Debuging: " & CurrentScreen ' This is just for debug purposes
                ShouldUpdate = True ' This is just for debug purposes
            Else
                ShouldUpdate = False
            End If
        End If

        If ServersManager.ServerConnection.Connected = True Then
            APP_State = "Playing Multiplayer"
        Else
            APP_State = "Playing Solo"
        End If

        Dim NewPresence As DiscordRichPresence = New DiscordRichPresence With {
            .Details = APP_Details,
            .State = APP_State,
            .LargeImageKey = APP_LargeImageName,
            .LargeImageText = APP_LargeImageText,
            .SmallImageKey = APP_SmallImageName,
            .SmallImageText = APP_SmallImageText,
            .PartySize = APP_Party_Size,
            .PartyMax = APP_Party_Size_Max,
            .StartTimestamp = APP_StartTimestamp
        }

        If ShouldUpdate And Environment.Is64BitProcess = False Then
            Logger.Log(Logger.LogTypes.Message, "Presence.vb: Updating Discord Presence.")
            Discord_UpdatePresence(NewPresence)
        End If
    End Sub

End Class
