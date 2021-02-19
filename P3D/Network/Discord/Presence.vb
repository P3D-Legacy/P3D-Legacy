
Public Class Presence

    ReadOnly APP_ID As String = Classified.Discord_Application_ID
    Private Handlers As DiscordEventHandlers = New DiscordEventHandlers With {.Ready = AddressOf IsReady}

    Private PreviousScreen As String = ""
    Private PreviousMapLevel As String = ""

    Public Sub Initialize()
        If APP_ID IsNot Nothing Then
            Discord_Initialize(APP_ID, Handlers, 1, "")
            Update()
        End If
    End Sub

    Private Sub IsReady(ByRef Request As DiscordUser)
        Logger.Log(Logger.LogTypes.Message, "Presence.vb: Discord initialized successfully!")
    End Sub

    Public Sub Update()

        Logger.Log(Logger.LogTypes.Debug, "Presence.vb: Checking Discord Presence.")

        Dim APP_Details As String = "" ' Example: "Daniel is in Goldenrod City"
        Dim APP_State As String = "Playing Solo" ' Example: "Playing Solo" or "Playing Multiplayer"
        Dim APP_LargeImageName As String = "logoxl" ' Image file name that has been uploaded to Discord. Example: "goldenrod_city"
        Dim APP_LargeImageText As String = "Pokemon 3D" ' An explanation of what the image is of. Example: "Goldenrod City"
        Dim APP_SmallImageName As String = "" ' Image file name that has been uploaded to Discord. Example: "logoxl"
        Dim APP_SmallImageText As String = "" ' An explanation of what the image is of. Example: "Pokémon 3D"

        Dim ShouldUpdate As Boolean = False

        Dim CurrentScreen As String = Core.CurrentScreen.ToString().Replace("P3D.", "")
        Dim MenuScreens() As String = {"NewMainMenuScreen", "PauseScreen", "PressStartScreen", "JoinServerScreen", "NewMenuScreen", "AddServerScreen", "ConnectScreen", "NewTrainerScreen", "GameJolt.LogInScreen", "PartyScreen", "NewOptionScreen", "SaveScreen", "TransitionScreen"}
        Dim PokedexScreens() As String = {"PokedexSelectScreen", "PokedexScreen", "PokedexViewScreen", "PokedexHabitatScreen"}
        Dim BattleScreens() As String = {"BattleIntroScreen", "BattleSystem.BattleScreen"}
        Dim InventoryScreens() As String = {"NewInventoryScreen"}
        Dim CurrentMapLevel As String = ""

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
            APP_Details = "Walking in " & CurrentMapLevel
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
            .SmallImageText = APP_SmallImageText
        }

        If ShouldUpdate Then
            Logger.Log(Logger.LogTypes.Message, "Presence.vb: Updating Discord Presence.")
            Discord_UpdatePresence(NewPresence)
        End If
    End Sub

End Class
