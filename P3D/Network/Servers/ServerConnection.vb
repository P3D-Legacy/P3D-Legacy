Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text

Namespace Servers

    ''' <summary>
    ''' Manages all Servers connection related operations.
    ''' </summary>
    Public Class ServerConnection

        Private _client As TcpClient

        Private _stream As NetworkStream
        Private _streamReader As StreamReader
        Private _streamWriter As StreamWriterLock

        Private _receiveThread As Threading.Thread

        Private _ConnectionOpen As Boolean = False 'Checks if a connection is started.

        ''' <summary>
        ''' Returns the current connection status.
        ''' </summary>
        Public ReadOnly Property Connected() As Boolean
            Get
                If _client Is Nothing Then
                    Return False
                End If
                Return _ConnectionOpen = True And _client.Connected = True
            End Get
        End Property

        ''' <summary>
        ''' Connects to a server.
        ''' </summary>
        ''' <param name="Server">The server to connect to.</param>
        Public Sub Connect(ByVal Server As Server)
            Dim t As New Threading.Thread(AddressOf InternalConnect)
            t.IsBackground = True
            t.Start(Server)
        End Sub

        Private Sub InternalConnect(ByVal ServerObject As Object)
            Dim Server As Server = CType(ServerObject, Server)

            Me._client = New TcpClient()

            Me._ConnectionOpen = True

            Try
                Dim connectIP As IPAddress = Nothing

                If Not IPAddress.TryParse(Server.IP, connectIP) Then
                    For Each IPAddress In Dns.GetHostEntry(Server.IP).AddressList
                        If IPAddress.AddressFamily = AddressFamily.InterNetwork Then
                            connectIP = IPAddress
                            Exit For
                        End If
                    Next
                End If


                Me._client.Connect(connectIP.ToString, CInt(Server.Port))

                If Me._client.Connected = True Then
                    'Send GameData package first:
                    Me._stream = Me._client.GetStream()
                    Me._streamReader = New StreamReader(Me._stream)
                    Me._streamWriter = New StreamWriterLock(Me._stream)

                    Core.ServersManager.PlayerManager.Reset()
                    Dim initialPlayerData As String = Core.ServersManager.PlayerManager.CreatePlayerDataPackage().ToString()

                    Me._streamWriter.WriteLine(initialPlayerData)
                    Me._streamWriter.Flush()

                    Logger.Debug("Sent initial data to server.")

                    'Start background activities:
                    Me.StartPing()
                    Me.StartListen()
                Else
                    Logger.Log(Logger.LogTypes.Warning, "ServerConnection.vb: Server connection error.")
                    Me.Disconnect("Cannot connect to server!", "Connection error!")
                End If
            Catch ex As Exception
                Logger.Log(Logger.LogTypes.Warning, "ServerConnection.vb: Server connection exception.")
                Me.Disconnect("Cannot connect to server!", ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Aborts the threads and closes any open streams.
        ''' </summary>
        Public Sub Abort()
            Logger.Debug("ServerConnection.vb: Aborting threads and streams...")
            Me._ConnectionOpen = False
            JoinServerScreen.Online = False
            Try
                If Not Me._client Is Nothing Then
                    Me._client.Close()
                    Logger.Debug(" - - Closed TCP Client")
                End If
            Catch : End Try
            Try
                Me.StopPing()
                Me.StopListen()
            Catch : End Try
            If Not Me._stream Is Nothing Then
                Try
                    Me._streamReader.Close()
                    Logger.Debug(" - - Closed stream reader")
                Catch : End Try
                Try
                    Me._streamWriter.Close()
                    Logger.Debug(" - - Closed stream writer")
                Catch : End Try
                Try
                    Me._stream.Close()
                    Logger.Debug(" - - Closed stream")
                Catch : End Try
            End If
            Logger.Debug("ServerConnection.vb: Aborted threads and streams.")
        End Sub

        ''' <summary>
        ''' Disconnects the player from the server and opens the main menu.
        ''' </summary>
        Public Sub Disconnect()
            Me.Disconnect("", "")
        End Sub

        ''' <summary>
        ''' Disconnects the player from the server and opens the DisconnectScreen (if there's a value for Header or Message).
        ''' </summary>
        ''' <param name="Header">The header to display on the ConnectScreen.</param>
        ''' <param name="Message">The Message to display on the ConnectScreen.</param>
        Public Sub Disconnect(ByVal Header As String, ByVal Message As String)
            If Me._ConnectionOpen = True Then
                Logger.Debug("Disconnect; Header: " & Header & "; Message: " & Message)

                If Message <> "" Or Header <> "" Then
                    ConnectScreen.Setup(New ConnectScreen(ConnectScreen.Modes.Disconnect, Header, Message, Core.CurrentScreen))
                End If

                Me.Abort()
            End If
        End Sub

#Region "Ping"

        Dim LastPingTime As Date = Date.Now
        Dim PingTimer As New Timers.Timer

        ''' <summary>
        ''' Start the ping thread.
        ''' </summary>
        Public Sub StartPing()
            Me.StopPing()
            Me.LastPingTime = Date.Now

            Me.PingTimer = New Timers.Timer()
            Me.PingTimer.Interval = 1000
            Me.PingTimer.AutoReset = True
            AddHandler Me.PingTimer.Elapsed, AddressOf InternalPing
            Me.PingTimer.Start()

            Logger.Debug("Starting Servers ping thread.")
        End Sub

        ''' <summary>
        ''' Stopping the ping thread.
        ''' </summary>
        Public Sub StopPing()
            Try
                Me.PingTimer.Stop()
            Catch : End Try
        End Sub

        ''' <summary>
        ''' Sends a ping package to the connected server.
        ''' </summary>
        Private Sub InternalPing()
            Try
                If (Date.Now - Me.LastPingTime).Seconds >= 10 Then
                    Me.SendPackage(New Package(Package.PackageTypes.Ping, Core.ServersManager.ID, Package.ProtocolTypes.UDP))
                    Me.LastPingTime = Date.Now
                End If
            Catch ex As Exception
                Debug.Print("Error while sending ping to server: " & ex.Message)
                Me.Disconnect("Disconnected from Server", "Error trying to ping the server.")
            End Try
        End Sub

#End Region

#Region "Send Data"

        Public Sub SendGameData()
            Dim p As Package = Core.ServersManager.PlayerManager.CreatePlayerDataPackage()
            Core.ServersManager.PlayerManager.ApplyLastPackage(p)

            Me.SendPackage(p)
        End Sub

        Public Sub SendChatMessage(ByVal message As String)
            If message.ToLower().StartsWith("/login ") = True Then
                Dim password As String = message.Remove(0, 7)
                Dim hashedPassword As String = BitConverter.ToString(New SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "").ToLower()
                message = "/login " + hashedPassword
                SendPackage(New Package(Package.PackageTypes.ChatMessage, Core.ServersManager.ID, Package.ProtocolTypes.TCP, message))
            ElseIf message.ToLower().StartsWith("/pm ") = True Then
                message = message.Remove(0, 4)
                Dim playerName As String = message
                While Core.ServersManager.PlayerCollection.HasPlayer(playerName) = False And playerName.Contains(" ") = True
                    playerName = playerName.Remove(playerName.LastIndexOf(" "))
                End While
                If playerName <> "" And Core.ServersManager.PlayerCollection.HasPlayer(playerName) = True And playerName.ToLower() <> Core.Player.Name.ToLower() Then
                    message = message.Remove(0, playerName.Length + 1)

                    SendPackage(New Package(Package.PackageTypes.PrivateMessage, Core.ServersManager.ID, Package.ProtocolTypes.TCP, {playerName, message}.ToList()))
                End If
            Else
                SendPackage(New Package(Package.PackageTypes.ChatMessage, Core.ServersManager.ID, Package.ProtocolTypes.TCP, message))
            End If
        End Sub

        Public Sub SendGameStateMessage(ByVal message As String)
            SendPackage(New Package(Package.PackageTypes.GamestateMessage, Core.ServersManager.ID, Package.ProtocolTypes.TCP, message))
        End Sub

        ''' <summary>
        ''' Send a package object to the server.
        ''' </summary>
        Public Sub SendPackage(ByVal Package As Package)
            If _ConnectionOpen = True And Me._client.Connected = True Then
                Threading.ThreadPool.QueueUserWorkItem(New Threading.WaitCallback(AddressOf InternalSendPackage), Package)
            End If
        End Sub

        Private Sub InternalSendPackage(ByVal packageObject As Object)
            'Because this might get executed from the Thread pool even after
            'the player disconnected from the server, we check explicitly if
            'they are still connected to the server:
            If Me._ConnectionOpen = True And Me._client.Connected = True Then
                Dim package As Package = CType(packageObject, Package)

                Try
                    Me._streamWriter.WriteLine(package.ToString())
                    Me._streamWriter.Flush()
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Warning, "ServerConnection.vb: Error while sending data to server (TCP): " & ex.Message)
                    Me.Disconnect("Disconnected from Server", "Error trying to send data to the server (TCP)." & Environment.NewLine & ex.Message)
                End Try

                Me.LastPingTime = Date.Now
            End If
        End Sub

#End Region

#Region "Receive Data"

        ''' <summary>
        ''' Start the listen thread that receives packages from the server.
        ''' </summary>
        Private Sub StartListen()
            If Not Me._receiveThread Is Nothing AndAlso Me._receiveThread.IsAlive = True Then
                Try
                    Me._receiveThread.Abort()
                Catch : End Try
            End If
            Me._receiveThread = New Threading.Thread(AddressOf InternalListen)
            Me._receiveThread.IsBackground = True
            Me._receiveThread.Start()
        End Sub

        ''' <summary>
        ''' Listen to the server (TCP).
        ''' </summary>
        Private Sub InternalListen()
            While Me._client.Connected
                Try
                    Dim receivedData As String = Me._streamReader.ReadLine()
                    Dim p As New Package(receivedData)

                    If p.IsValid = True Then
                        Try
                            p.Handle()
                        Catch ex As Exception
                            Logger.Debug("Error trying to handle server data package.")
                        End Try
                    Else
                        Logger.Debug("Received invalid server package.")
                    End If
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Warning, "ServerConnection.vb: Error while receiving server data (TCP): " & Environment.NewLine & ex.Message)
                    Me.Disconnect("Disconnected from Server", "Error while receiving server data (TCP).")
                End Try
            End While
        End Sub

        ''' <summary>
        ''' Stops listening to the TCP and UDP ports.
        ''' </summary>
        Private Sub StopListen()
            Try
                If Not Me._receiveThread Is Nothing Then
                    Me._receiveThread.Abort()
                    Logger.Debug(" - - Aborted listen thread")
                End If
            Catch : End Try
        End Sub

#End Region

    End Class

End Namespace