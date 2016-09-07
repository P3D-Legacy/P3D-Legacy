Public Class PVPLobbyScreen

    Inherits Screen

    Enum ScreenStates
        Idle
        Stopped
        ChooseTeam
        StartBattle
        BattleResults
    End Enum

    Public Shared ScreenState As ScreenStates = ScreenStates.Idle

    Shared OppTeam As List(Of Pokemon) = Nothing 'The team the opponent is using.
    Dim OwnTeam As List(Of Pokemon) = Nothing 'The team you are using.

    Shared PartnerNetworkID As Integer = 0 'The NetworkID of your partner.
    Shared WaitingForPlayer As Boolean = False 'If you are the host and waiting for the other player to accept.

    Shared SentBattleOffer As Boolean = False 'If you sent a request.
    Shared ReceivedBattleOffer As Boolean = False 'If you idle and get a request.

    Shared StartBattleRemote As Boolean = False 'If you sent a request and got the accept.

    Public Shared DisconnectMessage As String = ""
    Shared IsHost As Boolean = True

    Dim Cursor As Integer = 0
    Dim menuItems As New List(Of String)
    Dim texture As Texture2D = Nothing

    Public Sub New(ByVal currentScreen As Screen, ByVal connectPlayerID As Integer, ByVal isHost As Boolean)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.PVPLobbyScreen
        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        MusicManager.PlayMusic("lobby", True)

        Me.MouseVisible = True
        Me.CanBePaused = False

        PartnerNetworkID = connectPlayerID
        WaitingForPlayer = isHost
        SentBattleOffer = False
        ReceivedBattleOffer = False
        OppTeam = Nothing
        StartBattleRemote = False
        PVPLobbyScreen.IsHost = isHost
        OwnTeam = Nothing
        OppTeam = Nothing
        StoppedBattle = False
        BattleSuccessful = False
        BattleResults = Nothing

        DisconnectMessage = ""
        ScreenState = ScreenStates.Idle

        If isHost = False Then
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleJoin, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
        Else
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleRequest, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
        End If

        Me.menuItems = {"Start Battle!", "Choose Team", "Quit"}.ToList()
        Me.BattleBoxPokemon = StorageSystemScreen.GetBattleBoxPokemon()
        Core.StartThreadedSub(AddressOf DownloadOnlineSprite)
    End Sub

    Private Sub DownloadOnlineSprite()
        Dim p As Servers.Player = Core.ServersManager.PlayerCollection.GetPlayer(PartnerNetworkID)
        If Not p Is Nothing Then
            If p.GamejoltID <> "" Then
                GameJolt.Emblem.GetOnlineSprite(p.GamejoltID)
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawGradient(Core.windowSize, New Color(10, 145, 227), New Color(6, 77, 139), False, -1)
        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 65), New Color(0, 24, 114), New Color(13, 138, 228), False, -1)
        Core.SpriteBatch.DrawString(FontManager.MainFont, "Versus Battle", New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Versus Battle").X / 2), 20), New Color(196, 231, 255))
        Canvas.DrawRectangle(New Rectangle(0, 65, Core.windowSize.Width, 1), New Color(0, 24, 114))

        Select Case ScreenState
            Case ScreenStates.Idle
                If WaitingForPlayer = True Then
                    Dim t As String = "Waiting for other player" & LoadingDots.Dots
                    Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CSng(Core.windowSize.Height / 2 - 10)), Color.White)
                Else
                    Me.DrawIdle()
                End If
            Case ScreenStates.Stopped
                Dim t As String = DisconnectMessage
                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CSng(Core.windowSize.Height / 2 - 10)), Color.White)
            Case ScreenStates.ChooseTeam
                Me.DrawChooseTeam()
            Case ScreenStates.BattleResults
                Me.DrawBattleResults()
        End Select
    End Sub

    Public Overrides Sub Update()
        If ScreenState = ScreenStates.Idle Then
            If ConnectScreen.Connected = True Then
                Dim partnerOnServer As Boolean = False
                For Each p As Servers.Player In Core.ServersManager.PlayerCollection
                    If p.ServersID = PartnerNetworkID Then
                        partnerOnServer = True
                        Exit For
                    End If
                Next
                If partnerOnServer = False Then
                    DisconnectMessage = "The other player disconnected." & vbNewLine & vbNewLine & "Press any key to exit."
                    ScreenState = ScreenStates.Stopped
                End If
            Else
                DisconnectMessage = "You got disconnected from the server." & vbNewLine & vbNewLine & "Press any key to exit."
                ScreenState = ScreenStates.Stopped
            End If
        End If

        Select Case ScreenState
            Case ScreenStates.Idle
                Me.UpdateIdle()
            Case ScreenStates.Stopped
                If KeyBoardHandler.GetPressedKeys().Count > 0 Or ControllerHandler.HasControlerInput() = True Or Controls.Accept() = True Or Controls.Dismiss() = True Then
                    Core.SetScreen(Me.PreScreen)
                End If
            Case ScreenStates.ChooseTeam
                Me.UpdateChooseTeam()
            Case ScreenStates.BattleResults
                Me.UpdateBattleResults()
        End Select
    End Sub

#Region "Idle"

    Private Sub DrawIdle()
        'Own side:
        Canvas.DrawRectangle(New Rectangle(100, 200, 300, 64), New Color(177, 228, 247, 200))
        Canvas.DrawGradient(New Rectangle(0, 200, 100, 64), New Color(255, 255, 255, 0), New Color(177, 228, 247, 200), True, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Core.Player.Name, New Vector2(140, 215), Color.Black, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
        Core.SpriteBatch.Draw(Screen.Level.OwnPlayer.Texture, New Rectangle(60, 200, 64, 64), New Rectangle(0, 64, 32, 32), Color.White)

        Canvas.DrawRectangle(New Rectangle(0, 264, 400, 32), New Color(6, 77, 139))

        If Not OwnTeam Is Nothing Then
            If OwnTeam.Count > 0 Then
                For i = 0 To OwnTeam.Count - 1
                    Dim p As Pokemon = OwnTeam(i)
                    Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(40 + i * 40, 264, 32, 32), Color.White)
                Next
            End If
        End If

        'Menu:
        If ReceivedBattleOffer = True Then
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 100, 64 * 4, 64), New Color(255, 255, 255, 150))
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Your opponent wants to" & vbNewLine & "battle with this setup.", New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 4, 104), Color.Black)
        Else
            If SentBattleOffer = True Then
                Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 100, 64 * 4, 64), New Color(255, 255, 255, 150))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, "You want to battle" & vbNewLine & "with this setup.", New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 4, 104), Color.Black)
            End If
        End If

        For i = 0 To Me.menuItems.Count - 1
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 200 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 64, 200 + i * 96, 64 * 2, 64), New Rectangle(32, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 64 * 3, 200 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Me.menuItems(i), New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 20, 216 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
        Next

        DrawCursor()

        'Opp side:
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 300), 200, 300, 64), New Color(177, 228, 247, 200))
        Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width - 400), 200, 100, 64), New Color(255, 255, 255, 0), New Color(177, 228, 247, 200), True, -1)

        Dim t As Texture2D = Nothing
        Dim tempPlayer As Servers.Player = Nothing
        For Each p As Servers.Player In Core.ServersManager.PlayerCollection
            If p.ServersID = PartnerNetworkID Then
                Dim tPath As String = NetworkPlayer.GetTexturePath(p.Skin)
                If TextureManager.TextureExist(tPath) = True Then
                    t = TextureManager.GetTexture(tPath)
                Else
                    t = TextureManager.GetTexture("Textures\NPC\0")
                End If
                tempPlayer = p

                If p.GameJoltId <> "" Then
                    If GameJolt.Emblem.HasDownloadedSprite(p.GameJoltId) = True Then
                        Dim newT As Texture2D = GameJolt.Emblem.GetOnlineSprite(p.GameJoltId)
                        If Not newT Is Nothing Then
                            t = newT
                        End If
                    End If
                End If

                Exit For
            End If
        Next

        If Not t Is Nothing And Not tempPlayer Is Nothing Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, tempPlayer.Name, New Vector2(Core.windowSize.Width - 260, 215), Color.Black, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(Core.windowSize.Width - 340), 200, 64, 64), New Rectangle(0, 64, 32, 32), Color.White)
        End If

        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 400), 264, 400, 32), New Color(6, 77, 139))

        If Not OppTeam Is Nothing Then
            If OppTeam.Count > 0 Then
                For i = 0 To OppTeam.Count - 1
                    Dim p As Pokemon = OppTeam(i)
                    Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(CInt(Core.windowSize.Width - 360) + i * 40, 264, 32, 32), Color.White)
                Next
            End If
        End If
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 160, 200 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Private Sub UpdateIdle()
        If WaitingForPlayer = False Then
            If StartBattleRemote = True Then
                StartBattleRemote = False
                InitializeBattle()
            Else
                If Controls.Up(True, True, True, True, True, True) = True Then
                    Me.Cursor -= 1
                    If Controls.ShiftDown() = True Then
                        Me.Cursor -= 4
                    End If
                End If
                If Controls.Down(True, True, True, True, True, True) = True Then
                    Me.Cursor += 1
                    If Controls.ShiftDown() = True Then
                        Me.Cursor += 4
                    End If
                End If

                Me.Cursor = Me.Cursor.Clamp(0, Me.menuItems.Count - 1)

                If Controls.Accept(True, False, False) = True Then
                    For i = 0 To Me.menuItems.Count - 1
                        If New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 200 + i * 96, 64 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            If i = Cursor Then
                                Me.SelectMenuEntry()
                            Else
                                Cursor = i
                            End If
                        End If
                    Next
                End If

                If Controls.Accept(False, True, True) = True Then
                    Me.SelectMenuEntry()
                End If
            End If
        Else
            If Controls.Dismiss() = True Then
                QuitBattle()
            End If
        End If
    End Sub

    Private Sub SelectMenuEntry()
        Select Case Me.Cursor
            Case 0
                Me.StartBattle()
            Case 1
                ScreenState = ScreenStates.ChooseTeam
            Case 2
                Me.QuitBattle()
        End Select
    End Sub

    Private Sub StartBattle()
        If Not Me.OwnTeam Is Nothing And Not OppTeam Is Nothing Then
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleStart, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
            If ReceivedBattleOffer = True Then
                InitializeBattle()
            Else
                SentBattleOffer = True
            End If
        End If
    End Sub

    Private Sub QuitBattle()
        Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleQuit, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
        Core.SetScreen(Me.PreScreen)
    End Sub

#End Region

#Region "ChooseTeam"

    Dim BattleBoxPokemon As New List(Of Pokemon)
    Dim ChooseTeamCursor As Integer = 0

    Private Sub DrawChooseTeam()
        Dim t As String = "Choose your team:"
        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 100), Color.White)

        Dim startPos As New Vector2(CSng(Core.windowSize.Width / 2) - 400, 300)
        Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) - 410, 230, 290, 360), New Color(203, 40, 41), New Color(238, 128, 128), False, -1)
        For i = 0 To 5
            Dim x As Integer = i
            Dim y As Integer = 0

            While x > 1
                x -= 2
                y += 1
            End While

            Canvas.DrawBorder(2, New Rectangle(CInt(startPos.X) + x * 140, y * 100 + CInt(startPos.Y), 128, 80), New Color(230, 230, 230))

            If BattleBoxPokemon.Count - 1 >= i Then
                Core.SpriteBatch.Draw(BattleBoxPokemon(i).GetMenuTexture(), New Rectangle(CInt(startPos.X) + x * 140 + 32, y * 100 + CInt(startPos.Y) + 10, 64, 64), Color.White)
            End If

            Core.SpriteBatch.DrawString(FontManager.MainFont, "Battle Box", New Vector2(CInt(startPos.X) + 80, CInt(startPos.Y) - 45), Color.White)
        Next

        startPos = New Vector2(CSng(Core.windowSize.Width / 2) + 130, 300)
        Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2) + 120, 230, 290, 360), New Color(84, 198, 216), New Color(42, 167, 198), False, -1)
        For i = 0 To 5
            Dim x As Integer = i
            Dim y As Integer = 0

            While x > 1
                x -= 2
                y += 1
            End While

            Canvas.DrawBorder(2, New Rectangle(CInt(startPos.X) + x * 140, y * 100 + CInt(startPos.Y), 128, 80), New Color(230, 230, 230))

            If Core.Player.Pokemons.Count - 1 >= i Then
                Core.SpriteBatch.Draw(Core.Player.Pokemons(i).GetMenuTexture(), New Rectangle(CInt(startPos.X) + x * 140 + 32, y * 100 + CInt(startPos.Y) + 10, 64, 64), Color.White)
            End If

            Core.SpriteBatch.DrawString(FontManager.MainFont, "Team", New Vector2(CInt(startPos.X) + 106, CInt(startPos.Y) - 45), Color.White)
        Next

        DrawChooseTeamCursor()
    End Sub

    Private Sub DrawChooseTeamCursor()
        Dim cPosition As Vector2 = New Vector2(CSng(Core.windowSize.Width / 2) - 280, 190)
        If ChooseTeamCursor = 1 Then
            cPosition = New Vector2(CSng(Core.windowSize.Width / 2) + 250, 190)
        End If

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
    End Sub

    Private Sub UpdateChooseTeam()
        If Controls.Left(True, True) = True Then
            Me.ChooseTeamCursor = 0
        End If
        If Controls.Right(True, True) = True Then
            Me.ChooseTeamCursor = 1
        End If

        Dim hasBattleBoxPokemon As Boolean = False
        For Each p As Pokemon In Me.BattleBoxPokemon
            If p.IsEgg() = False Then
                hasBattleBoxPokemon = True
                Exit For
            End If
        Next

        If Controls.Accept(True, False, False) = True Then
            If New Rectangle(CInt(Core.windowSize.Width / 2) + 120, 230, 290, 360).Contains(MouseHandler.MousePosition) = True Then
                If ChooseTeamCursor = 0 Then
                    ChooseTeamCursor = 1
                Else
                    SelectTeam()
                End If
            End If
            If New Rectangle(CInt(Core.windowSize.Width / 2) - 410, 230, 290, 360).Contains(MouseHandler.MousePosition) = True Then
                If ChooseTeamCursor = 1 Then
                    ChooseTeamCursor = 0
                Else
                    If hasBattleBoxPokemon = True Then
                        SelectTeam()
                    End If
                End If
            End If
        End If

        If hasBattleBoxPokemon = True Or ChooseTeamCursor = 1 Then
            If Controls.Accept(False, True, True) = True Then
                SelectTeam()
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            ScreenState = ScreenStates.Idle
        End If
    End Sub

    Private Sub SelectTeam()
        Me.OwnTeam = New List(Of Pokemon)

        If ChooseTeamCursor = 0 Then
            Me.OwnTeam.AddRange(BattleBoxPokemon.ToArray())
        Else
            Me.OwnTeam.AddRange(Core.Player.Pokemons.ToArray())
        End If

        For i = 0 To Me.OwnTeam.Count - 1
            If i <= Me.OwnTeam.Count - 1 Then
                If Me.OwnTeam(i).IsEgg() = True Then
                    Me.OwnTeam.RemoveAt(i)
                    i -= 1
                End If
            End If
        Next

        SentBattleOffer = False
        ReceivedBattleOffer = False

        Dim sendPokemonData As String = ""
        For Each p As Pokemon In Me.OwnTeam
            If p.IsEgg() = False Then
                If sendPokemonData <> "" Then
                    sendPokemonData &= "|"
                End If
                sendPokemonData &= p.GetSaveData()
            End If
        Next

        Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleOffer, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, {PartnerNetworkID.ToString(), sendPokemonData}.ToList()))
        ScreenState = ScreenStates.Idle
    End Sub

#End Region

#Region "InputFunctions"

    Public Shared Sub OtherPlayerJoins()
        If IsLobbyScreen() = True Then
            WaitingForPlayer = False
        End If
    End Sub

    Public Shared Sub OtherPlayerQuits()
        If IsLobbyScreen() = True Then
            ScreenState = ScreenStates.Stopped
            DisconnectMessage = "The other player quit the battle." & vbNewLine & vbNewLine & "Press any key to exit."
        End If
    End Sub

    Public Shared Sub ReceiveOppTeam(ByVal data As String)
        If IsLobbyScreen() = True Then
            If data = "none" Then
                OppTeam = Nothing
            Else
                OppTeam = New List(Of Pokemon)

                Dim tempData As String = ""
                Dim cData As String = data
                While cData.Length > 0
                    If cData(0).ToString() = "|" AndAlso tempData(tempData.Length - 1).ToString() = "}" Then
                        OppTeam.Add(Pokemon.GetPokemonByData(tempData))
                        tempData = ""
                    Else
                        tempData &= cData(0).ToString()
                    End If
                    cData = cData.Remove(0, 1)
                End While
                If tempData.StartsWith("{") = True And tempData.EndsWith("}") = True Then
                    OppTeam.Add(Pokemon.GetPokemonByData(tempData))
                    tempData = ""
                End If
            End If
            SentBattleOffer = False
            ReceivedBattleOffer = False
        End If
    End Sub

    Public Shared Sub ReceiveBattleStart()
        If IsLobbyScreen() = True Then
            If SentBattleOffer = True Then
                StartBattleRemote = True
            Else
                ReceivedBattleOffer = True
            End If
        End If
    End Sub

    Private Shared Function IsLobbyScreen() As Boolean
        Dim s As Screen = Core.CurrentScreen
        While s.Identification <> Identifications.PVPLobbyScreen And Not s.PreScreen Is Nothing
            s = s.PreScreen
        End While
        Return (s.Identification = Identifications.PVPLobbyScreen)
    End Function

#End Region

#Region "StartBattle"

    Dim TempOriginalTeam As New List(Of Pokemon)
    Public Shared StoppedBattle As Boolean = False 'This is true if changed to the screen from a battle.

    Private Sub InitializeBattle()
        Me.CanChat = False
        Me.CanBePaused = False
        ScreenState = ScreenStates.StartBattle

        Dim tPath As String = "Textures\NPC\0"
        Dim tempPlayer As Servers.Player = Nothing
        For Each p As Servers.Player In Core.ServersManager.PlayerCollection
            If p.ServersID = PartnerNetworkID Then
                tPath = NetworkPlayer.GetTexturePath(p.Skin)
                If TextureManager.TextureExist(tPath) = False Then
                    tPath = "Textures\NPC\0"
                End If
                tempPlayer = p
                Exit For
            End If
        Next

        tPath = tPath.Remove(0, ("Textures\NPC\").Length)

        Dim t As New Trainer()
        t.Pokemons = OppTeam
        t.TrainerType = "Pokémon Trainer"
        t.DoubleTrainer = False
        t.Name = tempPlayer.Name
        t.Money = 0
        t.SpriteName = tPath
        t.Region = "Johto"
        t.TrainerFile = ""
        t.Items = New List(Of Item)
        t.Gender = 0
        t.IntroType = 11
        t.OutroMessage = ". . ."
        t.GameJoltID = tempPlayer.GamejoltID

        For Each p As Pokemon In t.Pokemons
            p.Level = 50
            p.CalculateStats()
            p.FullRestore()
        Next

        TempOriginalTeam.Clear()
        For Each p As Pokemon In Core.Player.Pokemons
            TempOriginalTeam.Add(Pokemon.GetPokemonByData(p.GetSaveData()))
        Next

        Core.Player.Pokemons.Clear()

        For Each p As Pokemon In Me.OwnTeam
            If p.IsEgg() = False Then
                p.Level = 50
                p.CalculateStats()
                p.FullRestore()
                Core.Player.Pokemons.Add(p)
            End If
        Next

        Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, 0)
        b.IsPVPBattle = True
        b.IsHost = IsHost
        b.IsRemoteBattle = True
        b.PartnerNetworkID = PartnerNetworkID

        If tempPlayer.GamejoltID <> "" Then
            b.PVPGameJoltID = tempPlayer.GamejoltID
        End If

        Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))
        PlayerStatistics.Track("PVP battles", 1)
    End Sub

#End Region

    Public Overrides Sub ChangeTo()
        If PVPLobbyScreen.StoppedBattle = True Then
            If BattleSuccessful = True Then
                ScreenState = ScreenStates.BattleResults
            End If
            MusicManager.PlayMusic("lobby", False)
            Core.Player.Pokemons.Clear()
            Core.Player.Pokemons.AddRange(TempOriginalTeam.ToArray())
        End If
    End Sub

#Region "BattleResults"

    Shared BattleResults As BattleResult = Nothing
    Public Shared BattleSuccessful As Boolean = False

    Class BattleResult

        Public Won As Boolean = False
        Public OwnPokemon As New List(Of Pokemon)
        Public OppPokemon As New List(Of Pokemon)

        Public OwnStatistics As BattleSystem.BattleScreen.NetworkPlayerStatistics
        Public OppStatistics As BattleSystem.BattleScreen.NetworkPlayerStatistics

        Public Sub New(ByVal won As Boolean, ByVal own As List(Of Pokemon), ByVal opp As List(Of Pokemon), ByVal ownStatistics As BattleSystem.BattleScreen.NetworkPlayerStatistics, ByVal oppStatistics As BattleSystem.BattleScreen.NetworkPlayerStatistics)
            Me.Won = won

            For Each p As Pokemon In own
                Me.OwnPokemon.Add(Pokemon.GetPokemonByData(p.GetSaveData()))
            Next

            Me.OppPokemon = opp

            Me.OwnStatistics = ownStatistics
            Me.OppStatistics = oppStatistics

            If won = False Then
                For Each p As Pokemon In Me.OwnPokemon
                    p.HP = 0
                    p.Status = Pokemon.StatusProblems.Fainted
                Next
            Else
                For Each p As Pokemon In Me.OppPokemon
                    p.HP = 0
                    p.Status = Pokemon.StatusProblems.Fainted
                Next
            End If
        End Sub

    End Class

    Public Shared Sub SetupBattleResults(ByVal BattleScreen As BattleSystem.BattleScreen)
        BattleSuccessful = True
        BattleResults = New BattleResult(BattleSystem.Battle.Won, Core.Player.Pokemons, BattleScreen.Trainer.Pokemons, BattleScreen.OwnStatistics, BattleScreen.OppStatistics)
        If BattleResults.Won = True Then
            PlayerStatistics.Track("PVP Wins", 1)
        Else
            PlayerStatistics.Track("PVP Losses", 1)
        End If
    End Sub

    Private Sub DrawBattleResults()
        If Not BattleResults Is Nothing Then
            'Draw Result:
            Dim resultText As String = "You Won the Battle!"
            Dim resultStateText As String = "Won"
            Dim resultColor As Color = Color.Orange
            If BattleResults.Won = False Then
                resultText = "You Lost the Battle!"
                resultStateText = "Lost"
                resultColor = Color.LightBlue
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, resultText, New Vector2(Core.windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(resultText).X / 2.0F, 120), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, resultStateText, New Vector2(Core.windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(resultText).X / 2.0F + FontManager.MainFont.MeasureString("You ").X, 120), resultColor)

            'Own side:
            Canvas.DrawRectangle(New Rectangle(100, 200, 300, 64), New Color(177, 228, 247, 200))
            Canvas.DrawGradient(New Rectangle(0, 200, 100, 64), New Color(255, 255, 255, 0), New Color(177, 228, 247, 200), True, -1)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Core.Player.Name, New Vector2(140, 215), Color.Black, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.Draw(Screen.Level.OwnPlayer.Texture, New Rectangle(60, 200, 64, 64), New Rectangle(0, 64, 32, 32), Color.White)

            Canvas.DrawRectangle(New Rectangle(0, 264, 400, 32), New Color(6, 77, 139))

            For i = 0 To BattleResults.OwnPokemon.Count - 1
                Dim p As Pokemon = BattleResults.OwnPokemon(i)

                Dim c As Color = Color.White
                If p.Status = Pokemon.StatusProblems.Fainted Or p.HP <= 0 Then
                    c = New Color(65, 65, 65, 255)
                End If

                Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(40 + i * 40, 264, 32, 32), c)
            Next

            'Draw Own Statistics:
            With BattleResults.OwnStatistics
                Dim s1 As String = "Turns:" & vbNewLine &
                                   "     Moves:" & vbNewLine &
                                   "     Switches:" & vbNewLine & vbNewLine &
                                   "Super Effective:" & vbNewLine &
                                   "Not very Effective:" & vbNewLine &
                                   "No Effect:" & vbNewLine & vbNewLine &
                                   "Critical Hits:"
                Dim s2 As String = .Turns & vbNewLine &
                                   .Moves & vbNewLine &
                                   .Switches & vbNewLine & vbNewLine &
                                   .SuperEffective & vbNewLine &
                                   .NotVeryEffective & vbNewLine &
                                   .NoEffect & vbNewLine & vbNewLine &
                                   .Critical

                Core.SpriteBatch.DrawString(FontManager.MiniFont, s1, New Vector2(40, 340), Color.White, 0.0F, Vector2.Zero, 1.1F, SpriteEffects.None, 0.0F)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, s2, New Vector2(250, 340), Color.LightBlue, 0.0F, Vector2.Zero, 1.1F, SpriteEffects.None, 0.0F)
            End With

            'Draw Pokémon left:
            Dim countOwnPokemon As Integer = 0
            For Each p As Pokemon In BattleResults.OwnPokemon
                If p.IsEgg() = False And p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                    countOwnPokemon += 1
                End If
            Next
            Dim countOppPokemon As Integer = 0
            For Each p As Pokemon In BattleResults.OppPokemon
                If p.IsEgg() = False And p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                    countOppPokemon += 1
                End If
            Next

            Dim pokeLeft As String = countOwnPokemon.ToString & " VS " & countOppPokemon.ToString()
            Core.SpriteBatch.DrawString(FontManager.MainFont, pokeLeft, New Vector2(Core.windowSize.Width / 2.0F - FontManager.MainFont.MeasureString(pokeLeft).X * 2.0F, 240), Color.White, 0.0F, Vector2.Zero, 4.0F, SpriteEffects.None, 0.0F)

            'Opp Side:
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 300), 200, 300, 64), New Color(177, 228, 247, 200))
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width - 400), 200, 100, 64), New Color(255, 255, 255, 0), New Color(177, 228, 247, 200), True, -1)

            Dim t As Texture2D = Nothing
            Dim tempPlayer As Servers.Player = Nothing
            For Each p As Servers.Player In Core.ServersManager.PlayerCollection
                If p.ServersID = PartnerNetworkID Then
                    Dim tPath As String = NetworkPlayer.GetTexturePath(p.Skin)
                    If TextureManager.TextureExist(tPath) = True Then
                        t = TextureManager.GetTexture(tPath)
                    Else
                        t = TextureManager.GetTexture("Textures\NPC\0")
                    End If
                    tempPlayer = p
                    Exit For
                End If
            Next

            If Not t Is Nothing And Not tempPlayer Is Nothing Then
                Core.SpriteBatch.DrawString(FontManager.MainFont, tempPlayer.Name, New Vector2(Core.windowSize.Width - 260, 215), Color.Black, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
                Core.SpriteBatch.Draw(t, New Rectangle(CInt(Core.windowSize.Width - 340), 200, 64, 64), New Rectangle(0, 64, 32, 32), Color.White)
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 400), 264, 400, 32), New Color(6, 77, 139))

            For i = 0 To BattleResults.OppPokemon.Count - 1
                Dim p As Pokemon = BattleResults.OppPokemon(i)

                Dim c As Color = Color.White
                If p.Status = Pokemon.StatusProblems.Fainted Or p.HP <= 0 Then
                    c = New Color(65, 65, 65, 255)
                End If

                Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(CInt(Core.windowSize.Width - 360) + i * 40, 264, 32, 32), c)
            Next

            'Draw Opp Statistics:
            With BattleResults.OppStatistics
                Dim s1 As String = "Turns:" & vbNewLine &
                                   "     Moves:" & vbNewLine &
                                   "     Switches:" & vbNewLine & vbNewLine &
                                   "Super Effective:" & vbNewLine &
                                   "Not very Effective:" & vbNewLine &
                                   "No Effect:" & vbNewLine & vbNewLine &
                                   "Critical Hits:"
                Dim s2 As String = .Turns & vbNewLine &
                                   .Moves & vbNewLine &
                                   .Switches & vbNewLine & vbNewLine &
                                   .SuperEffective & vbNewLine &
                                   .NotVeryEffective & vbNewLine &
                                   .NoEffect & vbNewLine & vbNewLine &
                                   .Critical

                Core.SpriteBatch.DrawString(FontManager.MiniFont, s1, New Vector2(Core.windowSize.Width - 360, 340), Color.White, 0.0F, Vector2.Zero, 1.1F, SpriteEffects.None, 0.0F)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, s2, New Vector2(Core.windowSize.Width - 150, 340), Color.LightBlue, 0.0F, Vector2.Zero, 1.1F, SpriteEffects.None, 0.0F)
            End With

        End If
    End Sub

    Private Sub UpdateBattleResults()
        If Controls.Accept() = True Then
            ScreenState = ScreenStates.Stopped
        End If
    End Sub

#End Region

End Class