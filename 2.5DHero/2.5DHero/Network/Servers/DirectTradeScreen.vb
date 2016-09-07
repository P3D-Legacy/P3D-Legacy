Public Class DirectTradeScreen

    Inherits Screen

    Enum ScreenStates
        Idle
        Stopped
        Trading
    End Enum

    Shared ScreenState As ScreenStates = ScreenStates.Idle

    Dim OfferPokemon As Pokemon = Nothing 'The Pokémon you offer.
    Dim OfferPokemonIndex As Integer = 0

    Shared TradePokemon As Pokemon = Nothing 'The Pokémon your trade partner wants to trade.

    Shared PartnerNetworkID As Integer = 0
    Shared WaitingForPlayer As Boolean = False

    Shared SentTradeOffer As Boolean = False
    Shared ReceivedTradeOffer As Boolean = False

    Shared DisconnectMessage As String = ""

    Dim Cursor As Integer = 0
    Dim menuItems As New List(Of String)
    Dim texture As Texture2D = Nothing

    Public Sub New(ByVal currentScreen As Screen, ByVal connectPlayerID As Integer, ByVal isHost As Boolean)
        Me.Identification = Identifications.DirectTradeScreen
        Me.PreScreen = currentScreen
        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        MusicManager.PlayMusic("gts", True)

        Me.MouseVisible = True
        Me.CanBePaused = False

        PartnerNetworkID = connectPlayerID
        WaitingForPlayer = isHost
        SentTradeOffer = False
        ReceivedTradeOffer = False
        TradePokemon = Nothing
        StartTradeRemote = False

        DisconnectMessage = ""
        ScreenState = ScreenStates.Idle

        If isHost = False Then
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.TradeJoin, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
        Else
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.TradeRequest, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
        End If

        Me.menuItems = {"Trade", "Choose Pokémon", "Cancel Trade"}.ToList()
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
        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

        Select Case ScreenState
            Case ScreenStates.Idle
                If WaitingForPlayer = True Then
                    Core.SpriteBatch.DrawString(FontManager.MainFont, "Waiting for other player" & LoadingDots.Dots, New Vector2(100), Color.White)
                Else
                    Me.DrawIdle()
                End If
            Case ScreenStates.Stopped
                Core.SpriteBatch.DrawString(FontManager.MainFont, DisconnectMessage, New Vector2(100), Color.White)
            Case ScreenStates.Trading
                Me.DrawTrading()
        End Select
    End Sub

    Dim offerYOffset As Integer = 0
    Dim tradeYOffset As Integer = 0

    Private Sub DrawIdle()
        'Own Side:
        Core.SpriteBatch.Draw(Screen.Level.OwnPlayer.Texture, New Rectangle(100, 32, 64, 64), New Rectangle(0, 64, 32, 32), Color.White)
        Core.SpriteBatch.DrawString(FontManager.MainFont, Core.Player.Name, New Vector2(170, 54), Color.White, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)

        Canvas.DrawRectangle(New Rectangle(100, 100, 256, 256), New Color(255, 255, 255, 150))

        If Not Me.OfferPokemon Is Nothing Then
            GetYOffset(offerYOffset, OfferPokemon)
            Core.SpriteBatch.Draw(Me.OfferPokemon.GetTexture(True), New Rectangle(100, 100 - offerYOffset, 256, 256), Color.White)

            Canvas.DrawRectangle(New Rectangle(100, 400, 256, 128), New Color(255, 255, 255, 150))

            Dim itemString As String = "none"
            If Not Me.OfferPokemon.Item Is Nothing Then
                itemString = OfferPokemon.Item.Name
            End If

            Core.SpriteBatch.DrawString(FontManager.MiniFont, OfferPokemon.GetDisplayName() & vbNewLine & "Level: " & OfferPokemon.Level & vbNewLine & "OT: " & OfferPokemon.OT & " / " & OfferPokemon.CatchTrainerName & vbNewLine & "Item: " & itemString, New Vector2(114, 414), Color.Black)
        End If

        'Menu:
        If ReceivedTradeOffer = True Then
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 100, 64 * 4, 64), New Color(255, 255, 255, 150))
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Your trade partner" & vbNewLine & "accepts this trade.", New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 4, 104), Color.Black)
        Else
            If SentTradeOffer = True Then
                Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 100, 64 * 4, 64), New Color(255, 255, 255, 150))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, "You accepted" & vbNewLine & "this trade.", New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 4, 104), Color.Black)
            End If
        End If

        For i = 0 To Me.menuItems.Count - 1
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2), 200 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 64, 200 + i * 96, 64 * 2, 64), New Rectangle(32, 16, 16, 16), Color.White)
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 64 * 3, 200 + i * 96, 64, 64), New Rectangle(16, 16, 16, 16), Color.White, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Me.menuItems(i), New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 20, 216 + i * 96), Color.Black, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
        Next

        DrawCursor()

        'Trade Side:
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
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(Core.windowSize.Width - 356), 32, 64, 64), New Rectangle(0, 64, 32, 32), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, tempPlayer.Name, New Vector2(CInt(Core.windowSize.Width - 356) + 70, 54), Color.White, 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)
        End If

        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 356), 100, 256, 256), New Color(255, 255, 255, 150))

        If Not TradePokemon Is Nothing Then
            GetYOffset(tradeYOffset, TradePokemon)
            Core.SpriteBatch.Draw(TradePokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width - 356), 100 - tradeYOffset, 256, 256), Color.White)

            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 356), 400, 256, 128), New Color(255, 255, 255, 150))

            Dim itemString As String = "none"
            If Not TradePokemon.Item Is Nothing Then
                itemString = TradePokemon.Item.Name
            End If

            Core.SpriteBatch.DrawString(FontManager.MiniFont, TradePokemon.GetDisplayName() & vbNewLine & "Level: " & TradePokemon.Level & vbNewLine & "OT: " & TradePokemon.OT & " / " & TradePokemon.CatchTrainerName & vbNewLine & "Item: " & itemString, New Vector2(CInt(Core.windowSize.Width - 356) + 14, 414), Color.Black)
        End If
    End Sub

    Private Sub GetYOffset(ByRef offset As Integer, ByVal p As Pokemon)
        Dim t As Texture2D = p.GetTexture(True)
        offset = -1

        Dim cArr(t.Width * t.Height - 1) As Color
        t.GetData(cArr)

        For y = 0 To t.Height - 1
            For x = 0 To t.Width - 1
                If cArr(x + y * t.Height) <> Color.Transparent Then
                    offset = y
                    Exit For
                End If
            Next

            If offset <> -1 Then
                Exit For
            End If
        Next
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - (64 * 4) / 2) + 160, 200 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
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
            Case ScreenStates.Trading
                Me.UpdateTrading()
        End Select
    End Sub

#Region "Idle"

    Private Sub UpdateIdle()
        If WaitingForPlayer = False Then
            If StartTradeRemote = True Then
                StartTradeRemote = False
                InitializeTrade()
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
                QuitTrade()
            End If
        End If
    End Sub

    Private Sub SelectMenuEntry()
        Select Case Me.Cursor
            Case 0
                Me.StartTrade()
            Case 1
                Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf Me.SelectPokemonForTrade, "Choose Pokémon for Trade", True, False, False))
                CType(Core.CurrentScreen, ChoosePokemonScreen).CanChooseHMPokemon = False
            Case 2
                Me.QuitTrade()
        End Select
    End Sub

    Private Sub QuitTrade()
        Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.TradeQuit, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
        Core.SetScreen(Me.PreScreen)
    End Sub

    Shared StartTradeRemote As Boolean = False

    Private Sub StartTrade()
        If Not Me.OfferPokemon Is Nothing And Not TradePokemon Is Nothing Then
            Me.CanChat = False
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.TradeStart, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, PartnerNetworkID.ToString()))
            If ReceivedTradeOffer = True Then
                InitializeTrade()
            Else
                SentTradeOffer = True
            End If
        End If
    End Sub

    Private Sub SelectPokemonForTrade(ByVal pokeIndex As Integer)
        If pokeIndex > -1 Then
            Me.OfferPokemon = Core.Player.Pokemons(pokeIndex)
            Me.OfferPokemonIndex = pokeIndex
            SentTradeOffer = False
            ReceivedTradeOffer = False
            Me.CanChat = True
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.TradeOffer, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, {PartnerNetworkID.ToString(), Me.OfferPokemon.GetSaveData()}.ToList()))
        End If
    End Sub

#End Region

#Region "InputFunctions"

    Public Shared Sub OtherPlayerJoins()
        If IsTradeScreen() = True Then
            WaitingForPlayer = False
        End If
    End Sub

    Public Shared Sub OtherPlayerQuits()
        If IsTradeScreen() = True Then
            ScreenState = ScreenStates.Stopped
            DisconnectMessage = "The other player quit the trade." & vbNewLine & vbNewLine & "Press any key to exit."
        End If
    End Sub

    Public Shared Sub ReceiveOfferPokemon(ByVal data As String)
        If IsTradeScreen() = True Then
            If data = "none" Then
                TradePokemon = Nothing
            Else
                TradePokemon = Pokemon.GetPokemonByData(data)
            End If
            SentTradeOffer = False
            ReceivedTradeOffer = False
            GetTradeScreen().CanChat = True
        End If
    End Sub

    Public Shared Sub ReceiveTradeStart()
        If IsTradeScreen() = True Then
            If SentTradeOffer = True Then
                StartTradeRemote = True
            Else
                ReceivedTradeOffer = True
            End If
        End If
    End Sub

    Private Shared Function IsTradeScreen() As Boolean
        Return (GetTradeScreen() IsNot Nothing)
    End Function

    Private Shared Function GetTradeScreen() As DirectTradeScreen
        Dim s As Screen = Core.CurrentScreen
        While s.Identification <> Identifications.DirectTradeScreen And s.PreScreen IsNot Nothing
            s = s.PreScreen
        End While
        If s.Identification = Identifications.DirectTradeScreen Then
            Return CType(s, DirectTradeScreen)
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Trade"

    Private Sub InitializeTrade()
        Core.Player.Pokemons.RemoveAt(OfferPokemonIndex)
        Core.Player.Pokemons.Add(Pokemon.GetPokemonByData(TradePokemon.GetSaveData()))
        Core.Player.PokedexData = Pokedex.RegisterPokemon(Core.Player.PokedexData, Core.Player.Pokemons(Core.Player.Pokemons.Count - 1))
        Core.Player.SaveGame(False)

        SentTradeOffer = False
        ReceivedTradeOffer = False
        ScreenState = ScreenStates.Trading

        ownPokemonPosition = Core.windowSize.Height
        tState = 0
        messageDelay = 220
        MusicManager.PlayMusic("evolution", True)
        Me.CanChat = False
        PlayerStatistics.Track("Trades", 1)
    End Sub

    Dim ownPokemonPosition As Integer = 0
    Dim oppPokemonPosition As Integer = 0
    Dim tState As Integer = 0
    Dim messageDelay As Integer = 220

    Private Sub DrawTrading()
        Select Case tState
            Case 0
                Core.SpriteBatch.Draw(Me.OfferPokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), ownPokemonPosition, 256, 256), Color.White)
            Case 1
                Core.SpriteBatch.Draw(Me.OfferPokemon.GetTexture(False), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), CInt(Core.windowSize.Height / 2 - 128), 256, 256), Color.White)

                Dim p As Servers.Player = Nothing
                For Each pp As Servers.Player In Core.ServersManager.PlayerCollection
                    If pp.ServersID = PartnerNetworkID Then
                        p = pp
                        Exit For
                    End If
                Next

                Dim t As String = "Sending " & OfferPokemon.GetDisplayName() & " to " & p.Name & "." & vbNewLine & "Good-bye, " & OfferPokemon.GetDisplayName() & "!"

                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CInt(Core.windowSize.Height / 2 + 130)), Color.White)
            Case 2
                Core.SpriteBatch.Draw(Me.OfferPokemon.GetTexture(False), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), ownPokemonPosition, 256, 256), Color.White)
            Case 3
                Core.SpriteBatch.Draw(TradePokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), oppPokemonPosition, 256, 256), Color.White)
            Case 4
                Core.SpriteBatch.Draw(TradePokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - 128), CInt(Core.windowSize.Height / 2 - 128), 256, 256), Color.White)

                Dim p As Servers.Player = Nothing
                For Each pp As Servers.Player In Core.ServersManager.PlayerCollection
                    If pp.ServersID = PartnerNetworkID Then
                        p = pp
                        Exit For
                    End If
                Next

                Dim t As String = p.Name & " sent over " & TradePokemon.GetDisplayName() & "."

                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CInt(Core.windowSize.Height / 2 + 130)), Color.White)
        End Select
    End Sub

    Private Sub UpdateTrading()
        Select Case tState
            Case 0
                If ownPokemonPosition > CInt(Core.windowSize.Height / 2 - 128) Then
                    ownPokemonPosition -= 4
                    If ownPokemonPosition <= CInt(Core.windowSize.Height / 2 - 128) Then
                        ownPokemonPosition = CInt(Core.windowSize.Height / 2 - 128)
                        tState = 1
                        SoundManager.PlayPokemonCry(OfferPokemon.Number)
                    End If
                End If
            Case 1
                If messageDelay > 0 Then
                    messageDelay -= 1
                    If messageDelay <= 0 Then
                        messageDelay = 220
                        tState = 2
                    End If
                End If
            Case 2
                If ownPokemonPosition > -256 Then
                    ownPokemonPosition -= 4
                    If ownPokemonPosition <= -256 Then
                        ownPokemonPosition = -256
                        tState = 3
                        oppPokemonPosition = -256
                    End If
                End If
            Case 3
                If oppPokemonPosition < CInt(Core.windowSize.Height / 2 - 128) Then
                    oppPokemonPosition += 4
                    If oppPokemonPosition >= CInt(Core.windowSize.Height / 2 - 128) Then
                        oppPokemonPosition = CInt(Core.windowSize.Height / 2 - 128)
                        tState = 4
                        SoundManager.PlayPokemonCry(TradePokemon.Number)
                    End If
                End If
            Case 4
                If messageDelay > 0 Then
                    messageDelay -= 1
                    If messageDelay = 180 Then
                        SoundManager.PlaySound("success", True)
                    End If
                    If messageDelay <= 0 Then
                        messageDelay = 220
                        EndTrade()
                    End If
                End If
        End Select
    End Sub

    Private Sub EndTrade()
        Dim offeredPokemonID As Integer = OfferPokemon.Number

        ScreenState = ScreenStates.Idle
        OfferPokemon = Nothing
        TradePokemon = Nothing
        SentTradeOffer = False
        ReceivedTradeOffer = False

        MusicManager.PlayMusic("gts", True)

        If Core.Player.Pokemons(Core.Player.Pokemons.Count - 1).CanEvolve(EvolutionCondition.EvolutionTrigger.Trading, "") = True Then
            Core.SetScreen(New EvolutionScreen(Me, {Core.Player.Pokemons.Count - 1}.ToList(), offeredPokemonID.ToString(), EvolutionCondition.EvolutionTrigger.Trading))
        End If
        Me.CanChat = True
    End Sub

#End Region

End Class