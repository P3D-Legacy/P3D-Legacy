Imports System.IO
Imports System.Net.Sockets
Imports System.Net

Public Class JoinServerScreen

    Inherits Screen

    Friend Shared BarAnimationState As Integer = 0

    Dim mainTexture As Texture2D

    Private _dataModel As DataModel.Json.PlayerData.ServerListModel
    Dim ServerList As New List(Of Server)

    Dim selectIndex As Integer = 0
    Dim scrollIndex As Integer = 0
    Dim buttonIndex As Integer = 0

    Dim LoadOnlineServers As Boolean = True

    Public Shared SelectedServer As Server = Nothing
    Public Shared Online As Boolean = False

    Public Shared ClearThreadList As New List(Of Threading.Thread)

    Public Sub New(ByVal currentScreeen As Screen)
        mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        PreScreen = currentScreeen
        Identification = Identifications.JoinServerScreen
        MouseVisible = True
        CanBePaused = False
        CanChat = False
    End Sub

    Private Sub LoadServers()
        ServerList.Clear()

        Dim localServer As New Server("Local", "127.0.0.1")
        localServer.IsLocal = True
        ServerList.Add(localServer)

        If File.Exists(GameController.GamePath & "\Save\server_list.dat") = False Then
            CreateDefaultServerList()
        End If

        If LoadOnlineServers = True Then
            LoadServerList()
        End If

        For Each s As Server In ServerList
            s.Ping()
        Next
    End Sub

    Private Sub LoadServerList()
        Dim jsonData As String = File.ReadAllText(GameController.GamePath & "\Save\server_list.dat")

        Try
            _dataModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.PlayerData.ServerListModel)(jsonData)
            For Each serverModel In _dataModel.Servers
                ServerList.Add(New Server(serverModel.ListName, serverModel.IpAddress & ":" & serverModel.Port.ToString()))
            Next
        Catch ex As Exception
            Logger.Log("300", Logger.LogTypes.Message, "Failed to load server_list.dat. Create default content.")
            CreateDefaultServerList()
        End Try
    End Sub

    Private Sub CreateDefaultServerList()
        Dim officialServer As New Server("Official Pokémon3D Server", "karp.pokemon3d.net:15124")
        Dim AGNServer As New Server("AGN Server", "p3d.aggressivegaming.org:15124")

        ServerList.Add(officialServer)
        ServerList.Add(AGNServer)

        _dataModel = New DataModel.Json.PlayerData.ServerListModel()

        SaveServerlist()
    End Sub

    Public Overrides Sub Draw()
        Dim Tx As Integer = CInt(World.CurrentSeason)
        Dim Ty As Integer = 0
        If Tx > 1 Then
            Tx -= 2
            Ty += 1
        End If

        Dim ServersToDisplay As Integer = GetServersToDisplay()

        Dim pattern As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(160 + Tx * 16, Ty * 16, 16, 16), "")
        For Dx = 0 To ScreenSize.Width Step 128
            For Dy = 0 To ScreenSize.Height Step 128
                Dim c As Color = Color.White

                SpriteBatch.DrawInterface(pattern, New Rectangle(Dx, Dy, 128, 128), c)
            Next
        Next

        Canvas.DrawRectangle(New Rectangle(0, 75, ScreenSize.Width, ScreenSize.Height - 240), New Color(0, 0, 0, 128), True)

        SpriteBatch.DrawInterfaceString(FontManager.MainFont, "Join a server", New Vector2(CSng(ScreenSize.Width / 2 - FontManager.MainFont.MeasureString("Join a server").X), 14), Color.White, 0.0F, New Vector2(0), 2.0F, SpriteEffects.None, 0.0F)

        Dim endX As Integer = ServerList.Count - 1
        endX = CInt(MathHelper.Clamp(endX, 0, ServersToDisplay - 1))

        If ServerList.Count > ServersToDisplay Then
            Canvas.DrawScrollBar(New Vector2(CSng(ScreenSize.Width / 2 + 266), 100), ServerList.Count, 1, selectIndex, New Size(8, ScreenSize.Height - 300), False, Color.Black, Color.Gray, True)
        End If

        'Draw default first:
        For i = 0 To endX
            Dim index As Integer = i + scrollIndex

            If ServerList.Count - 1 >= index Then
                ServerList(index).Draw(New Vector2(0, i * 100 + 100), index = selectIndex)
            End If
        Next

        Dim CanvasTexture As Texture2D
        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        For i = 0 To 5
            If i = buttonIndex Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = "Join"
                    Dim s As Server = ServerList(selectIndex)

                    If s.IsLocal = True Then
                        Text = "Play"
                    Else
                        If s.CanJoin() = False Then
                            CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
                        End If
                    End If
                Case 1
                    Text = "Refresh"
                Case 2
                    Text = "Add"
                Case 3
                    Text = "Edit"
                    Dim s As Server = ServerList(selectIndex)

                    If s.IsLocal = True Then
                        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
                    End If
                Case 4
                    Text = "Remove"
                    Dim s As Server = ServerList(selectIndex)

                    If s.IsLocal = True Then
                        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
                    End If
                Case 5
                    Text = "Back"
            End Select

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(ScreenSize.Width / 2) - 560 + i * 192, ScreenSize.Height - 136, 128, 64), True)
            SpriteBatch.DrawInterfaceString(FontManager.InGameFont, Text, New Vector2(CInt(ScreenSize.Width / 2) - 542 + i * 192, ScreenSize.Height - 106), Color.Black)
        Next

        Dim vS As String = "Protocol version: " & Servers.ServersManager.PROTOCOLVERSION
        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, vS, New Vector2(ScreenSize.Width - FontManager.MiniFont.MeasureString(vS).X - 4, ScreenSize.Height - FontManager.MiniFont.MeasureString(vS).Y - 1), Color.White)

        'Draw player list tooltip after everything else:
        For i = 0 To endX
            Dim index As Integer = i + scrollIndex

            If ServerList.Count - 1 >= index Then
                ServerList(index).DrawPlayerListToolTip(New Vector2(0, i * 100 + 100))
            End If
        Next
    End Sub

    Public Overrides Sub Update()
        If LoadOnlineServers = False Then
            JoinButton()
        End If

        Dim ServersToDisplay As Integer = GetServersToDisplay()

        If Controls.Up(True, True, True) = True Then
            selectIndex -= 1
            If selectIndex - scrollIndex < 0 Then
                scrollIndex -= 1
            End If
        End If
        If Controls.Down(True, True, True) = True Then
            selectIndex += 1
            If selectIndex + scrollIndex > ServersToDisplay - 1 Then
                scrollIndex += 1
            End If
        End If

        If GameInstance.IsMouseVisible = True Then
            For i = 0 To 5
                If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 560 + i * 192, ScreenSize.Height - 138, 128 + 32, 64 + 32)).Contains(MouseHandler.MousePosition) = True Then
                    buttonIndex = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True AndAlso MouseHandler.ButtonPressed(MouseHandler.MouseButtons.RightButton) = False Then
                        Select Case buttonIndex
                            Case 0
                                JoinButton()
                            Case 1
                                RefreshButton()
                            Case 2
                                AddServerButton()
                            Case 3
                                EditServerButton()
                            Case 4
                                RemoveServerButton()
                            Case 5
                                CancelButton()
                        End Select
                    End If
                End If
            Next
        End If

        For i = 0 To ServersToDisplay - 1
            If ScaleScreenRec(New Rectangle(CInt(ScreenSize.Width / 2) - 354, i * 100 + 100, 500, 80)).Contains(MouseHandler.MousePosition) = True Then
                If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                    selectIndex = i + scrollIndex
                End If
            End If
        Next

        If Controls.Right(True, True, False) = True Then
            buttonIndex += 1
        End If
        If Controls.Left(True, True, False) = True Then
            buttonIndex -= 1
        End If

        buttonIndex = CInt(MathHelper.Clamp(buttonIndex, 0, 5))

        If Controls.Accept(False, True) = True Then
            Select Case buttonIndex
                Case 0
                    JoinButton()
                Case 1
                    RefreshButton()
                Case 2
                    AddServerButton()
                Case 3
                    EditServerButton()
                Case 4
                    RemoveServerButton()
                Case 5
                    CancelButton()
            End Select
        End If

        If Controls.Dismiss() = True Then
            'CType(PreScreen, MainMenuScreen).menuIndex = 0
            SetScreen(PreScreen)
        End If

        BarAnimationState += 1
        If BarAnimationState > 49 Then
            BarAnimationState = 0
        End If

        selectIndex = CInt(MathHelper.Clamp(selectIndex, 0, ServerList.Count - 1))
        scrollIndex = CInt(MathHelper.Clamp(scrollIndex, 0, ServerList.Count - ServersToDisplay))
    End Sub

#Region "Buttons"

    Private Sub JoinButton()
        If selectIndex = 0 Then
            Online = False
            SelectedServer = Nothing
            SetScreen(New OverworldScreen())
        Else
            If ServerList(selectIndex).CanJoin() = True Then
                SelectedServer = ServerList(selectIndex)
                ServerList.Move(selectIndex, 1)
                SaveServerlist()
                SetScreen(New ConnectScreen(ConnectScreen.Modes.Connect, "Connecting to server", "Please wait...", CurrentScreen))
            End If
        End If
    End Sub

    Private Sub AddServerButton()
        SetScreen(New AddServerScreen(Me, ServerList, True, Nothing))
    End Sub

    Private Sub EditServerButton()
        Dim s As Server = ServerList(selectIndex)

        If s.IsLocal = False Then
            ServerList.RemoveAt(selectIndex)
            SaveServerlist()
            SetScreen(New AddServerScreen(Me, ServerList, False, s))
        End If
    End Sub

    Private Sub RemoveServerButton()
        Dim s As Server = ServerList(selectIndex)

        If s.IsLocal = False Then
            ServerList.RemoveAt(selectIndex)
            SaveServerlist()
            LoadServers()
        End If
    End Sub

    Private Sub CancelButton()
        SetScreen(PreScreen)
    End Sub

    Private Sub RefreshButton()
        For Each Server As Server In ServerList
            Server.Refresh()
        Next
    End Sub

#End Region

    Private Function GetServersToDisplay() As Integer
        Dim contentHeight As Integer = ScreenSize.Height - 300
        Dim serverHeight As Integer = 80
        Dim serverCount As Integer = 1

        While contentHeight > serverHeight + 80
            serverHeight += 100
            serverCount += 1
        End While

        Return serverCount
    End Function

    Public Overrides Sub ChangeTo()
        If Controls.ShiftDown() = True Then
            LoadOnlineServers = False
        Else
            LoadOnlineServers = True
        End If
        LoadServers()
    End Sub

    Class Server

        Inherits Servers.Server

        Public IsLocal As Boolean = False
        Public IdentifierName As String = ""
        Private Name As String = ""
        Public PingResult As Integer = 0
        Public Pinged As Boolean = False
        Public StartedPing As Boolean = False
        Public ServerMessage As String = ""
        Public PlayerList As New List(Of String)
        Public CurrentPlayersOnline As Integer = 0
        Public MaxPlayersOnline As Integer = 0
        Public ServerProtocolVersion As String = ""

        Dim ReceivedError As Boolean = False

        Public Sub Refresh()
            Name = IdentifierName
            Pinged = False
            StartedPing = False
            ReceivedError = False
            CurrentPlayersOnline = 0
            MaxPlayersOnline = 0
            ServerProtocolVersion = ""
            ServerMessage = ""

            Ping()
        End Sub

        Public Function GetName() As String
            If Name = "" Then
                Return IdentifierName
            End If
            Return Name
        End Function

        Public Sub New(ByVal name As String, ByVal Address As String)
            MyBase.New(Address)

            IdentifierName = name

            Pinged = False
            StartedPing = False
        End Sub

        Public Sub Ping()
            If IsLocal = True Then
                Name = "Local"
                PingResult = 0
                StartedPing = True
                Pinged = True
                CurrentPlayersOnline = 0
                MaxPlayersOnline = 1
                IP = "127.0.0.1"
                Port = "15124"
                ServerMessage = "Play on your local computer."
                ServerProtocolVersion = Servers.ServersManager.PROTOCOLVERSION
            Else
                Dim t As New Threading.Thread(AddressOf StartPing)
                t.IsBackground = True
                t.Start()
                ClearThreadList.Add(t)
                StartedPing = True
                Dim cT As New Threading.Thread(AddressOf CheckServerConnectTimeout)
                cT.IsBackground = True
                cT.Start(t)
                ClearThreadList.Add(cT)
            End If
        End Sub

        Private Sub CheckServerConnectTimeout(ByVal t As Object)
            Dim sw As New Stopwatch()
            sw.Start()

            'TTL: 10000 ticks, usually at 60 Hz => 10000/60 seconds
            While sw.ElapsedMilliseconds < 10000 And Pinged = False
                'wait for server connection in main thread.
                Threading.Thread.Sleep(1)
            End While

            sw.Stop()

            If Pinged = False Then
                Try
                    CType(t, Threading.Thread).Abort()
                Catch : End Try
                Pinged = True
                ReceivedError = True
            End If
        End Sub

        Public Function CanJoin() As Boolean
            If IsLocal = True Then
                Return True
            End If
            If StartedPing = True And Pinged = True Then
                If ServerProtocolVersion = Servers.ServersManager.PROTOCOLVERSION Then
                    If CurrentPlayersOnline < MaxPlayersOnline Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        Private Sub StartPing()
            Dim sw As New Stopwatch()

            Try
                Dim client As New TcpClient()

                Dim connectIP As IPAddress = Nothing

                For Each ipaddress In Dns.GetHostEntry(IP).AddressList
                    If ipaddress.AddressFamily = AddressFamily.InterNetwork Then
                        connectIP = ipaddress
                        Exit For
                    End If
                Next

                sw.Start()
                client.Connect(connectIP, CInt(Port))

                If client.Connected = True Then
                    Dim Stream As NetworkStream = client.GetStream()

                    Dim streamr As StreamReader = New StreamReader(Stream)
                    Dim streamw As StreamWriterLock = New StreamWriterLock(Stream)

                    streamw.WriteLine(New Servers.Package(Servers.Package.PackageTypes.ServerDataRequest, -1, Servers.Package.ProtocolTypes.TCP, "r").ToString())
                    streamw.Flush()

                    Dim p As New Servers.Package(streamr.ReadLine())
                    If p.IsValid = True Then
                        If p.PackageType = Servers.Package.PackageTypes.ServerInfoData Then
                            sw.Stop()
                            CurrentPlayersOnline = CInt(p.DataItems(0))
                            MaxPlayersOnline = CInt(p.DataItems(1))
                            Name = p.DataItems(2)
                            ServerMessage = p.DataItems(3)

                            PlayerList.Clear()
                            If p.DataItems.Count > 4 Then
                                For i = 4 To p.DataItems.Count - 1
                                    PlayerList.Add(p.DataItems(i))
                                Next
                            End If

                            PlayerList = (From playerName As String In PlayerList Order By playerName Ascending).ToList()

                            ServerProtocolVersion = p.ProtocolVersion
                            Logger.Debug("171", "Received server data.")
                        Else
                            ReceivedError = True
                        End If
                    Else
                        ReceivedError = True
                    End If
                Else
                    ReceivedError = True
                End If
            Catch ex As Exception
                ReceivedError = True

                Logger.Debug("172", "JoinServerScreen.vb: Exception trying to ping server: " & ex.Message)
            End Try
            Pinged = True

            PingResult = CInt(sw.ElapsedMilliseconds)
        End Sub

        Public Function GetPingTime() As Integer
            If Pinged = True And StartedPing = True Then
                If ReceivedError = True Then
                    Return 0
                Else
                    Return PingResult
                End If
            Else
                Return 0
            End If
        End Function

        Public Function GetServerStatus() As String
            If ReceivedError = True Then
                Return "Cannot reach server."
            End If
            If Pinged = True Then
                Return "Server online"
            Else
                Return "Polling" & LoadingDots.Dots
            End If
        End Function

        Public Sub Draw(ByVal startPos As Vector2, ByVal selected As Boolean)
            Dim width As Integer = 500
            startPos.X = CInt(ScreenSize.Width / 2 - width / 2)
            If selected = True Then
                Canvas.DrawRectangle(New Rectangle(CInt(startPos.X), CInt(startPos.Y), width, 80), New Color(0, 0, 0, 200), True)
                Canvas.DrawBorder(2, New Rectangle(CInt(startPos.X), CInt(startPos.Y), width, 80), Color.LightGray, True)
            End If
            SpriteBatch.DrawInterfaceString(FontManager.MiniFont, GetName(), New Vector2(CInt(startPos.X) + 4, CInt(startPos.Y) + 3), Color.White, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

            If ReceivedError = True Then
                SpriteBatch.DrawInterfaceString(FontManager.MiniFont, GetServerStatus(), New Vector2(CInt(startPos.X) + 4, CInt(startPos.Y) + 30), New Color(190, 0, 0, 255), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
                SpriteBatch.DrawInterface(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(150, 224, 14, 14), ""), New Rectangle(CInt(startPos.X) + width - 32, CInt(startPos.Y) + 3, 28, 28), Color.White)

                If New Rectangle(CInt(startPos.X) + width - 32, CInt(startPos.Y) + 3, 28, 28).Contains(MouseHandler.MousePosition) = True Then
                    Canvas.DrawRectangle(New Rectangle(MouseHandler.MousePosition.X + 10, MouseHandler.MousePosition.Y + 10, 160, 32), Color.Black)
                    Canvas.DrawBorder(3, New Rectangle(MouseHandler.MousePosition.X + 10, MouseHandler.MousePosition.Y + 10, 160, 32), Color.Gray)
                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "(no connection)", New Vector2(MouseHandler.MousePosition.X + 14, MouseHandler.MousePosition.Y + 16), Color.White)
                End If
            Else
                If Pinged = True Then
                    Dim message As String = ServerMessage
                    Dim color As Color = New Color(180, 180, 180, 255)

                    If CanJoin() = False Then
                        If CurrentPlayersOnline >= MaxPlayersOnline Then
                            message = "The server is full."
                        End If
                        If ServerProtocolVersion <> Servers.ServersManager.PROTOCOLVERSION Then
                            message = "Version doesn't match the server's version."
                        End If

                        color = New Color(190, 0, 0, 255)
                    End If

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, message, New Vector2(CInt(startPos.X) + 4, CInt(startPos.Y) + 30), color, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, CurrentPlayersOnline & "/" & MaxPlayersOnline, New Vector2(CInt(startPos.X) + width - 36 - FontManager.MiniFont.MeasureString(CurrentPlayersOnline & "/" & MaxPlayersOnline).X, CInt(startPos.Y) + 7), Color.LightGray)
                    SpriteBatch.DrawInterface(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(80 + 14 * (4 - GetPingLevel()), 238, 14, 14), ""), New Rectangle(CInt(startPos.X) + width - 32, CInt(startPos.Y) + 3, 28, 28), Color.White)

                    'Ping result tool tip:
                    If New Rectangle(CInt(startPos.X) + width - 32, CInt(startPos.Y) + 3, 28, 28).Contains(MouseHandler.MousePosition) = True Then
                        Canvas.DrawRectangle(New Rectangle(MouseHandler.MousePosition.X + 10, MouseHandler.MousePosition.Y + 10, 160, 32), Color.Black)
                        Canvas.DrawBorder(3, New Rectangle(MouseHandler.MousePosition.X + 10, MouseHandler.MousePosition.Y + 10, 160, 32), Color.Gray)
                        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Ping: " & PingResult & " ms", New Vector2(MouseHandler.MousePosition.X + 14, MouseHandler.MousePosition.Y + 16), Color.White)
                    End If
                Else
                    SpriteBatch.DrawInterface(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(80 + 14 * CInt(Math.Floor(BarAnimationState / 10)), 224, 14, 14), ""), New Rectangle(CInt(startPos.X) + width - 32, CInt(startPos.Y) + 3, 28, 28), Color.White)
                    If New Rectangle(CInt(startPos.X) + width - 32, CInt(startPos.Y) + 3, 28, 28).Contains(MouseHandler.MousePosition) = True Then
                        Canvas.DrawRectangle(New Rectangle(MouseHandler.MousePosition.X + 10, MouseHandler.MousePosition.Y + 10, 160, 32), Color.Black)
                        Canvas.DrawBorder(3, New Rectangle(MouseHandler.MousePosition.X + 10, MouseHandler.MousePosition.Y + 10, 160, 32), Color.Gray)
                        SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Polling" & LoadingDots.Dots, New Vector2(MouseHandler.MousePosition.X + 14, MouseHandler.MousePosition.Y + 16), Color.White)
                    End If

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Polling" & LoadingDots.Dots, New Vector2(CInt(startPos.X) + 4, CInt(startPos.Y) + 30), New Color(180, 180, 180, 255), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
                End If
            End If

            SpriteBatch.DrawInterfaceString(FontManager.MiniFont, GetAddressString(), New Vector2(CInt(startPos.X) + 4, CInt(startPos.Y) + 53), New Color(180, 180, 180, 255), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        End Sub

        Public Sub DrawPlayerListToolTip(ByVal startPos As Vector2)
            If ReceivedError = False And Pinged = True And IsLocal = False Then
                Dim width As Integer = 500
                startPos.X = CInt(ScreenSize.Width / 2 - width / 2)

                If ScaleScreenRec(New Rectangle(CInt(startPos.X) + width - 36 - FontManager.MiniFont.MeasureString(CurrentPlayersOnline & "/" & MaxPlayersOnline).X.ToInteger(), CInt(startPos.Y) + 3, FontManager.MiniFont.MeasureString(CurrentPlayersOnline & "/" & MaxPlayersOnline).X.ToInteger(), 28)).Contains(MouseHandler.MousePosition) = True Then
                    Dim tooltipText As String = "No players on the server."

                    If PlayerList.Count > 0 Then
                        tooltipText = PlayerList.ToArray().ArrayToString(True)
                    End If

                    Dim v = FontManager.MiniFont.MeasureString("Player list:" & vbNewLine & tooltipText)

                    Dim drawY As Integer = MouseHandler.MousePosition.Y + 10
                    If drawY + v.Y + 12 > windowSize.Height Then
                        drawY = CInt(windowSize.Height - v.Y - 22)
                    End If
                    If drawY < 0 Then
                        drawY = 0
                    End If

                    Canvas.DrawRectangle(New Rectangle(MouseHandler.MousePosition.X + 10, drawY, CInt(v.X + 10), CInt(v.Y + 22)), Color.Black, True)
                    Canvas.DrawBorder(3, New Rectangle(MouseHandler.MousePosition.X + 10, drawY, CInt(v.X + 10), CInt(v.Y + 22)), Color.Gray, True)

                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Player list:", New Vector2(MouseHandler.MousePosition.X + 14, drawY + 6), Color.LightBlue)
                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, tooltipText, New Vector2(MouseHandler.MousePosition.X + 14, drawY + 6 + 34), Color.White)
                End If
            End If
        End Sub

        Public Function GetAddressString() As String
            If IsLocal = True Then
                Return ""
            Else
                Return IP & ":" & Port
            End If
        End Function

        Private Function GetPingLevel() As Integer
            If PingResult < 500 Then
                Return 0
            ElseIf PingResult >= 500 And PingResult < 1000 Then
                Return 1
            ElseIf PingResult >= 1000 And PingResult < 2000 Then
                Return 2
            ElseIf PingResult >= 2000 And PingResult < 5000 Then
                Return 3
            Else
                Return 4
            End If
        End Function

        Public Overrides Function ToString() As String
            Return IdentifierName & "," & GetAddressString()
        End Function

    End Class

    Public Shared Sub AddServerMessage(ByVal m As String, ByVal server_name As String)
        If File.Exists(GameController.GamePath & "\Save\server_list.dat") = False Then
            File.WriteAllText(GameController.GamePath & "\Save\server_list.dat", "")
        End If

        Dim newData As String = ""

        Dim data() As String = File.ReadAllLines(GameController.GamePath & "\Save\server_list.dat")
        For Each line As String In data
            If newData <> "" Then
                newData &= vbNewLine
            End If
            If line.StartsWith(server_name & ",") = True Then
                Dim lineData() As String = line.Split(CChar(","))
                newData &= lineData(0) & "," & lineData(1) & "," & m
            Else
                newData &= line
            End If
        Next

        File.WriteAllText(GameController.GamePath & "\Save\server_list.dat", newData)
    End Sub

    Public Overrides Sub ChangeFrom()
        For Each t As Threading.Thread In ClearThreadList
            Try
                If t.IsAlive = True Then
                    t.Abort()
                End If
            Catch : End Try
        Next
        ClearThreadList.Clear()
    End Sub

    Private Sub SaveServerlist()
        Dim serverModels As New List(Of DataModel.Json.PlayerData.ServerListModel.ServerModel)
        For Each server In ServerList
            serverModels.Add(New DataModel.Json.PlayerData.ServerListModel.ServerModel() With {
                             .IpAddress = server.IP,
                             .ListName = server.IdentifierName,
                             .Port = CInt(server.Port)})
        Next

        _dataModel.Servers = serverModels.ToArray()

        Dim jsonData As String = _dataModel.ToString("    ")
        File.WriteAllText(GameController.GamePath & "\Save\server_list.dat", jsonData)
    End Sub

End Class