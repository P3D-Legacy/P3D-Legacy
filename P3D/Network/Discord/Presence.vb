
Public Class Presence

    ReadOnly APP_ID As String = Classified.Discord_Application_ID
    Private Handlers As DiscordEventHandlers = New DiscordEventHandlers With {.Ready = AddressOf IsReady}

    Private PreviousScreen As String = ""
    Private PreviousMapLevel As String = ""
    Private APP_StartTimestamp As Long = Nothing

    Public Sub Initialize()
        If APP_ID IsNot Nothing And Environment.Is64BitProcess = False Then
            Discord_Initialize(APP_ID, Handlers, 1, "")
            Update()
            APP_StartTimestamp = DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds
        End If
    End Sub

    Private Sub IsReady(ByRef Request As DiscordUser)
        Logger.Log(Logger.LogTypes.Message, "Presence.vb: Discord initialized successfully!")
    End Sub

    Public Sub Update()

        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: Checking Discord Presence.")

        ' Description of fields is found here
        ' https://discord.com/developers/docs/rich-presence/how-to#updating-presence-update-presence-payload-fields

        Dim APP_Details As String = "" ' Defaults - DO NOT CHANGE
        Dim APP_State As String = "Playing Solo" ' Defaults - DO NOT CHANGE
        Dim APP_LargeImageName As String = "logoxl" ' Defaults - DO NOT CHANGE
        Dim APP_LargeImageText As String = "Pokemon 3D" ' Defaults - DO NOT CHANGE
        Dim APP_SmallImageName As String = "" ' Defaults - DO NOT CHANGE
        Dim APP_SmallImageText As String = "" ' Defaults - DO NOT CHANGE
        Dim APP_Party_Size As Integer = 0 ' Defaults - DO NOT CHANGE
        Dim APP_Party_Size_Max As Integer = 0 ' Defaults - DO NOT CHANGE

        ' WHAT SHOULD THE FIELDS BE USED FOR?
        ' 
        ' APP_Details: Should be used for current state the user is in the game
        ' Default should be: ""
        ' Example: "Walking in Goldenrod City"
        ' 
        ' APP_State: Show if the user is in solo or multiplayer
        ' Default should be: "Playing Solo"
        ' Example: "Playing Solo" or "Playing Multiplayer"
        ' 
        ' APP_LargeImageName: Visible image in discord
        ' Default should be: "logoxl"
        ' Example: "logoxl" or when in a specific place "goldenrod_city"
        ' 
        ' APP_LargeImageText: Visible name of logo in discord
        ' Default should be: "Pokemon 3D"
        ' Example: "Pokemon 3D" or when in a specific place "Goldenrod City"
        ' 
        ' APP_SmallImageName: Visible image in discord
        ' Default should be: ""
        ' Example: "logoxl" only when user is in a specific place else default
        ' 
        ' APP_SmallImageText: Visible name of logo in discord
        ' Default should be: ""
        ' Example: "Pokemon 3D" only when user is in a specific place else default
        ' 
        ' APP_Party_Size: Visible name of logo in discord
        ' Default should be: ""
        ' Example: "Pokemon 3D" only when user is in a specific place else default
        ' 
        ' APP_Party_Size_Max: Visible name of logo in discord
        ' Default should be: ""
        ' Example: "Pokemon 3D" only when user is in a specific place else default
        ' 

        Dim ShouldUpdate As Boolean = False

        Dim CurrentScreen As String = Core.CurrentScreen.ToString().Replace("P3D.", "")
        Dim MenuScreens() As String = {"NewMainMenuScreen", "PauseScreen", "PressStartScreen", "JoinServerScreen", "NewMenuScreen", "AddServerScreen", "ConnectScreen", "NewTrainerScreen", "GameJolt.LogInScreen", "PartyScreen", "NewOptionScreen", "SaveScreen", "TransitionScreen"}
        Dim PokedexScreens() As String = {"PokedexSelectScreen", "PokedexScreen", "PokedexViewScreen", "PokedexHabitatScreen"}
        Dim BattleScreens() As String = {"BattleIntroScreen", "BattleSystem.BattleScreen"}
        Dim InventoryScreens() As String = {"NewInventoryScreen"}
        Dim CurrentMapLevel As String = ""
        Dim CurrentMapLevelFileName As String = ""
        Dim CurrentMapLevelFileNames() As String = {"goldenrod_city"}

        'Dim APP_Details As String = "Daniel is in Goldenrod City" ' Example: "Daniel is in Goldenrod City"
        'Dim APP_State As String = "Playing Solo" ' Example: "Playing Solo" or "Playing Multiplayer"
        'Dim APP_LargeImageName As String = "goldenrod_city" ' Image file name that has been uploaded to Discord. Example: "goldenrod_city"
        'Dim APP_LargeImageText As String = "Goldenrod City" ' An explanation of what the image is of. Example: "Goldenrod City"
        'Dim APP_SmallImageName As String = "logoxl" ' Image file name that has been uploaded to Discord. Example: "logoxl"
        'Dim APP_SmallImageText As String = "Pokémon 3D" ' An explanation of what the image is of. Example: "Pokémon 3D"

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
            APP_Details = "Walking in " & CurrentMapLevel
            Logger.Log(Logger.LogTypes.Debug, "Presence.vb: CurrentMapLevelFileName: " & CurrentMapLevelFileName)
            If CurrentMapLevelFileNames.Contains(CurrentMapLevelFileName) Then
                APP_LargeImageName = CurrentMapLevel.ToLower.Replace(" ", "_")
                APP_LargeImageText = CurrentMapLevel
                APP_SmallImageName = "logoxl" ' Defaults - DO NOT CHANGE
                APP_SmallImageText = "Pokemon 3D" ' Defaults - DO NOT CHANGE
            End If
            ShouldUpdate = True
        ElseIf CurrentScreen IsNot PreviousScreen Then
            If MenuScreens.Contains(CurrentScreen) Then
                APP_Details = "In the menus"
            ElseIf PokedexScreens.Contains(CurrentScreen) Then
                APP_Details = "In the Pokedex"
            ElseIf BattleScreens.Contains(CurrentScreen) Then
                APP_Details = "In a battle"
            ElseIf InventoryScreens.Contains(CurrentScreen) Then
                APP_Details = "In the inventory"
            Else
                APP_Details = CurrentScreen ' This is just for debug purposes
            End If
            PreviousScreen = CurrentScreen
            ShouldUpdate = True
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
