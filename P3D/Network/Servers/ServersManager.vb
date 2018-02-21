Imports System.IO
Imports System.Net.Sockets
Imports System.Net

Namespace Servers

    Public Class ServersManager

        Public Const PROTOCOLVERSION As String = "0.5"

        Private _PlayerCollection As PlayerCollection = Nothing
        Private _ServerConnection As ServerConnection = Nothing
        Private _PlayerManager As PlayerManager = Nothing

        Private _OwnID As Integer = 0

        Public Sub New()
            Me._PlayerCollection = New PlayerCollection()
            Me._ServerConnection = New ServerConnection()
            Me._PlayerManager = New PlayerManager()
        End Sub

        Public ReadOnly Property PlayerCollection() As PlayerCollection
            Get
                Return Me._PlayerCollection
            End Get
        End Property

        Public ReadOnly Property ServerConnection() As ServerConnection
            Get
                Return Me._ServerConnection
            End Get
        End Property

        Public ReadOnly Property PlayerManager() As PlayerManager
            Get
                Return Me._PlayerManager
            End Get
        End Property

        Public Property ID() As Integer
            Get
                Return Me._OwnID
            End Get
            Set(value As Integer)
                Me._OwnID = value
                Me._PlayerManager.ReceivedID = True
            End Set
        End Property

        Private Sub Reset()
            Me._OwnID = 0

            Me._ServerConnection.Abort()
            Me._PlayerCollection.Clear()
            Me._PlayerManager.Reset()
        End Sub

        Public Sub Connect(ByVal ServerObject As Object)
            'Conver the ServerObject back to a Server instance and start the connection.
            Dim Server = CType(ServerObject, JoinServerScreen.Server)

            Logger.Debug("Try to connect to server: " & Server.IP & ":" & Server.Port)

            Me.Reset() 'Reset all prior connections.

            Logger.Debug("Start connection")
            Me._ServerConnection.Connect(Server)

            'Set online to true:
            JoinServerScreen.Online = True
        End Sub

        ''' <summary>
        ''' Updates the ServersManager and sends the player data package if needed.
        ''' </summary>
        Public Sub Update()
            If JoinServerScreen.Online = True And ConnectScreen.Connected = True Then
                If Me._PlayerManager.HasNewPlayerData() = True Then
                    Me._ServerConnection.SendGameData()
                End If
            End If
        End Sub

    End Class

End Namespace