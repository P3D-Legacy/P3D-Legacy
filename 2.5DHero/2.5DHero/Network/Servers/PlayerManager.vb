Option Strict On

Namespace Servers

    ''' <summary>
    ''' Manages all local player related operations.
    ''' </summary>
    Public Class PlayerManager

        Private _receivedWorldData As Boolean = False 'If this client received the server's world data.
        Private _receivedID As Boolean = False 'if the client received the server's ID.

        Private _needsUpdate As Boolean = False 'If the player entities need to be updated.

        Public Sub Reset()
            Logger.Debug("PlayerManager.vb: Reset PlayerManager")

            Me._receivedID = False
            Me._receivedWorldData = False

            Me._needsUpdate = False
            Me._lastPackage = Nothing
            Me._lastPackageSent = Nothing
        End Sub

        Public WriteOnly Property ReceivedWorldData() As Boolean
            Set(value As Boolean)
                Me._receivedWorldData = value
            End Set
        End Property

        Public WriteOnly Property ReceivedID() As Boolean
            Set(value As Boolean)
                Me._receivedID = value
            End Set
        End Property

        Public WriteOnly Property NeedsUpdate() As Boolean
            Set(value As Boolean)
                Me._needsUpdate = value
            End Set
        End Property

        Public ReadOnly Property ReceivedIniData() As Boolean
            Get
                Return Me._receivedWorldData = True And Me._receivedID = True
            End Get
        End Property

        Public ReadOnly Property HasNewPlayerData() As Boolean
            Get
                If Me._lastPackage Is Nothing Then
                    Return True
                Else
                    If Me._lastPackage.DataItems.Count <> Player.PLAYERDATAITEMSCOUNT Then
                        Return True
                    End If
                End If

                'position
                If Me._lastPackage.DataItems(6) <> Me.GetPlayerPositionString() Then
                    Return True
                End If

                'facing
                If Me._lastPackage.DataItems(7) <> Me.GetFacing().ToString() Then
                    Return True
                End If

                'moving
                If Me._lastPackage.DataItems(8) <> Me.GetMoving() Then
                    Return True
                End If

                'levelfile
                If Me._lastPackage.DataItems(5) <> Me.GetLevelFile() Then
                    Return True
                End If

                'busytype
                If Me._lastPackage.DataItems(10) <> Me.GetBusyType() Then
                    Return True
                End If

                If Not Screen.Level.OverworldPokemon Is Nothing And Not Core.Player.GetWalkPokemon() Is Nothing Then
                    'pokemon visible
                    If Me._lastPackage.DataItems(11) <> Me.GetPokemonVisible() Then
                        Return True
                    End If

                    'pokemon skin
                    If Me._lastPackage.DataItems(13) <> Me.GetPokemonSkin() Then
                        Return True
                    End If

                    'pokemon facing
                    If Me._lastPackage.DataItems(14) <> Me.GetPokemonFacing() Then
                        Return True
                    End If
                End If

                'playername:
                If Me._lastPackage.DataItems(4) <> Core.Player.Name Then
                    Return True
                End If

                'skin
                If Me._lastPackage.DataItems(9) <> Core.Player.Skin Then
                    Return True
                End If

                Return False
            End Get
        End Property

        Public Sub UpdatePlayers()
            'For some reason, have Try Catch here.
            Try
                If _needsUpdate = True Then
                    Me._needsUpdate = False

                    'Remove old entities from the level:
                    If Screen.Level.NetworkPlayers.Count > 0 Then
                        For i = 0 To Screen.Level.NetworkPlayers.Count - 1
                            If i <= Screen.Level.NetworkPlayers.Count - 1 Then
                                Screen.Level.Entities.Remove(Screen.Level.NetworkPlayers(i))
                            End If
                        Next
                    End If
                    If Screen.Level.NetworkPokemon.Count > 0 Then
                        For i = 0 To Screen.Level.NetworkPokemon.Count - 1
                            If i <= Screen.Level.NetworkPokemon.Count - 1 Then
                                Screen.Level.Entities.Remove(Screen.Level.NetworkPokemon(i))
                            End If
                        Next
                    End If

                    'Remove players from list that quit the game:
                    Dim removeList As New List(Of NetworkPlayer)
                    For i = 0 To Screen.Level.NetworkPlayers.Count - 1
                        If i <= Screen.Level.NetworkPlayers.Count - 1 Then
                            Dim netPlayer As NetworkPlayer = Screen.Level.NetworkPlayers(i)

                            Dim exists As Boolean = False
                            For j = 0 To Core.ServersManager.PlayerCollection.Count - 1
                                If j <= Core.ServersManager.PlayerCollection.Count - 1 Then
                                    Dim p As Player = Core.ServersManager.PlayerCollection(j)
                                    If p.ServersID = netPlayer.NetworkID Then
                                        exists = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If exists = False Then
                                removeList.Add(netPlayer)
                            End If
                        End If
                    Next
                    For i = 0 To removeList.Count - 1
                        If i <= removeList.Count - 1 Then
                            Screen.Level.NetworkPlayers.Remove(removeList(i))
                        End If
                    Next

                    'Remove pokemon from list that quit the game:
                    Dim removePokeList As New List(Of NetworkPokemon)
                    For i = 0 To Screen.Level.NetworkPokemon.Count - 1
                        If i <= Screen.Level.NetworkPokemon.Count - 1 Then
                            Dim netPokemon As NetworkPokemon = Screen.Level.NetworkPokemon(i)

                            Dim exists As Boolean = False
                            For j = 0 To Core.ServersManager.PlayerCollection.Count - 1
                                If j <= Core.ServersManager.PlayerCollection.Count - 1 Then
                                    Dim p As Player = Core.ServersManager.PlayerCollection(j)
                                    If p.ServersID = netPokemon.PlayerID Then
                                        exists = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If exists = False Then
                                removePokeList.Add(netPokemon)
                            End If
                        End If
                    Next
                    For i = 0 To removePokeList.Count - 1
                        If i <= removePokeList.Count - 1 Then
                            Screen.Level.NetworkPokemon.Remove(Screen.Level.NetworkPokemon(i))
                        End If
                    Next

                    'Create new players and pokemon and add to the level.
                    For i = 0 To Core.ServersManager.PlayerCollection.Count - 1
                        If i <= Core.ServersManager.PlayerCollection.Count - 1 Then
                            Dim p As Player = Core.ServersManager.PlayerCollection(i)

                            If p.Initialized = True Then
                                If p.ServersID <> Core.ServersManager.ID Then
                                    Dim exists As Boolean = False
                                    For j = 0 To Screen.Level.NetworkPlayers.Count - 1
                                        If j <= Screen.Level.NetworkPlayers.Count - 1 Then
                                            Dim netPlayer As NetworkPlayer = Screen.Level.NetworkPlayers(j)
                                            If netPlayer.NetworkID = p.ServersID Then
                                                netPlayer.ApplyPlayerData(p)
                                                exists = True
                                                Exit For
                                            End If
                                        End If
                                    Next

                                    If exists = False Then
                                        Dim netPlayer As New NetworkPlayer(p.Position.X, p.Position.Y, p.Position.Z, {Nothing}, p.Skin, p.Facing, 0, "", New Vector3(1), p.Name, p.ServersID)
                                        netPlayer.ApplyPlayerData(p)
                                        Screen.Level.NetworkPlayers.Add(netPlayer)

                                        Dim netPokemon As New NetworkPokemon(p.PokemonPosition, p.PokemonSkin, p.PokemonVisible)
                                        netPokemon.faceRotation = p.PokemonFacing
                                        netPokemon.FaceDirection = p.PokemonFacing
                                        netPokemon.PlayerID = p.ServersID

                                        Screen.Level.NetworkPokemon.Add(netPokemon)
                                    Else
                                        For j = 0 To Screen.Level.NetworkPokemon.Count - 1
                                            If j <= Screen.Level.NetworkPokemon.Count - 1 Then
                                                Dim netPokemon As NetworkPokemon = Screen.Level.NetworkPokemon(j)
                                                If netPokemon.PlayerID = p.ServersID Then
                                                    netPokemon.ApplyPlayerData(p)
                                                    Exit For
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    Next

                    'Add the newly added entities to the level entity enumeration.
                    If Screen.Level.NetworkPlayers.Count > 0 Then
                        For i = 0 To Screen.Level.NetworkPlayers.Count - 1
                            If i <= Screen.Level.NetworkPlayers.Count - 1 Then
                                Screen.Level.Entities.Add(Screen.Level.NetworkPlayers(i))
                            End If
                        Next
                    End If
                    If Screen.Level.NetworkPokemon.Count > 0 Then
                        For i = 0 To Screen.Level.NetworkPokemon.Count - 1
                            If i <= Screen.Level.NetworkPokemon.Count - 1 Then
                                Screen.Level.Entities.Add(Screen.Level.NetworkPokemon(i))
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception

            End Try
        End Sub

#Region "PlayerDataPackage"

        Private _lastPackage As Package = Nothing 'This holds the current data items that was sent to the server. No empty data.
        Private _lastPackageSent As Package = Nothing 'This holds the package that was sent to the server.

        Public Function CreatePlayerDataPackage() As Package
            Dim dataItems As New List(Of String)

            '---General information---
            '0: Active gamemode
            '1: isgamejoltsave
            '2: GameJoltID
            '3: DecimalSeparator

            Dim GameJoltID As String = ""
            If Core.Player.IsGamejoltSave = True Then
                GameJoltID = Core.GameJoltSave.GameJoltID
            End If

            AddToDataItems(dataItems, GameModeManager.ActiveGameMode.Name.ToLower(), 0)
            AddToDataItems(dataItems, Core.Player.IsGamejoltSave.ToNumberString(), 1)
            AddToDataItems(dataItems, GameJoltID, 2)
            AddToDataItems(dataItems, GameController.DecSeparator, 3)

            '---Player Information---
            '4: playername
            '5: levelfile
            '6: position
            '7: facing
            '8: moving
            '9: skin
            '10: busytype

            Dim positionString As String = Me.GetPlayerPositionString()
            Dim levelFile As String = Me.GetLevelFile()
            Dim facing As Integer = Me.GetFacing()
            Dim moving As String = Me.GetMoving()
            Dim busyType As String = Me.GetBusyType()

            AddToDataItems(dataItems, Core.Player.Name, 4)
            AddToDataItems(dataItems, levelFile, 5)
            AddToDataItems(dataItems, positionString, 6)
            AddToDataItems(dataItems, facing.ToString(), 7)
            AddToDataItems(dataItems, moving, 8)
            AddToDataItems(dataItems, Core.Player.Skin, 9)
            AddToDataItems(dataItems, busyType, 10)

            '---OverworldPokemon---
            '11: Visible
            '12: Position
            '13: Skin
            '14: facing

            Dim PokemonVisible As String = "0"
            Dim PokemonPosition As String = "0|-10|0"
            Dim PokemonSkin As String = "[POKEMON|N]10"
            Dim PokemonFacing As String = "0"

            If Not Screen.Level.OverworldPokemon Is Nothing And Not Core.Player.GetWalkPokemon() Is Nothing Then
                PokemonVisible = Me.GetPokemonVisible()
                PokemonPosition = Me.GetPokemonPosition()
                PokemonSkin = Me.GetPokemonSkin()
                PokemonFacing = Me.GetPokemonFacing()
            End If

            AddToDataItems(dataItems, PokemonVisible, 11)
            AddToDataItems(dataItems, PokemonPosition, 12)
            AddToDataItems(dataItems, PokemonSkin, 13)
            AddToDataItems(dataItems, PokemonFacing, 14)

            Return New Package(Package.PackageTypes.GameData, Core.ServersManager.ID, Servers.Package.ProtocolTypes.UDP, dataItems)
        End Function

        Private Sub AddToDataItems(ByRef l As List(Of String), ByVal value As String, ByVal listIndex As Integer)
            Dim insertValue As String = value

            If Not Me._lastPackage Is Nothing Then
                If Me._lastPackage.DataItems.Count - 1 >= listIndex Then
                    If Me._lastPackage.DataItems(listIndex) = value Then
                        insertValue = ""
                    End If
                End If
            End If

            l.Add(insertValue)
        End Sub

        Public Sub ApplyLastPackage(ByVal newP As Package)
            If Me._lastPackage Is Nothing Then
                Me._lastPackage = newP
            Else
                For i = 0 To newP.DataItems.Count - 1
                    If newP.DataItems(i) <> "" Then
                        Me._lastPackage.DataItems(i) = newP.DataItems(i)
                    End If
                Next
            End If

            Me._lastPackageSent = newP
        End Sub

        Private ReadOnly Property GetPlayerPositionString() As String
            Get
                Dim PositionString As String = ""
                If net.Pokemon3D.Game.Player.Temp.IsInBattle = True Then
                    PositionString = net.Pokemon3D.Game.Player.Temp.BeforeBattlePosition.X.ToString() & "|" & (net.Pokemon3D.Game.Player.Temp.BeforeBattlePosition.Y - 0.1F).ToString() & "|" & net.Pokemon3D.Game.Player.Temp.BeforeBattlePosition.Z.ToString()
                Else
                    PositionString = Screen.Camera.Position.X.ToString() & "|" & (Screen.Camera.Position.Y - 0.1F).ToString() & "|" & Screen.Camera.Position.Z.ToString()
                End If
                Return PositionString
            End Get
        End Property

        Private ReadOnly Property GetLevelFile() As String
            Get
                Dim levelFile As String = Screen.Level.LevelFile
                If net.Pokemon3D.Game.Player.Temp.IsInBattle = True Then
                    levelFile = net.Pokemon3D.Game.Player.Temp.BeforeBattleLevelFile
                End If
                Return levelFile
            End Get
        End Property

        Private ReadOnly Property GetFacing() As Integer
            Get
                Dim facing As Integer = 0
                If net.Pokemon3D.Game.Player.Temp.IsInBattle = True Then
                    facing = net.Pokemon3D.Game.Player.Temp.BeforeBattleFacing
                Else
                    facing = Screen.Camera.GetPlayerFacingDirection() 
                End If
                Return facing
            End Get
        End Property

        Private ReadOnly Property GetMoving() As String
            Get
                Dim moving As String = "0"
                If Screen.Camera.IsMoving() = True Then
                    moving = "1"
                End If
                Return moving
            End Get
        End Property

        Private ReadOnly Property GetBusyType() As String
            Get
                Dim busyType As String = "0" 'Normal state
                If Core.CurrentScreen.Identification = Screen.Identifications.ChatScreen Then
                    busyType = "2" 'In chat
                End If
                If net.Pokemon3D.Game.Player.Temp.IsInBattle = True Then
                    busyType = "1" 'in battle
                End If
                If Core.CurrentScreen.Identification = Screen.Identifications.PauseScreen Then
                    busyType = "3" 'AFK
                End If
                Return busyType
            End Get
        End Property

        Private ReadOnly Property GetPokemonVisible() As String
            Get
                If Screen.Level.OverworldPokemon.Visible = True And Screen.Level.Surfing = False And Screen.Level.Riding = False And net.Pokemon3D.Game.Player.Temp.IsInBattle = False Then
                    Return "1"
                End If
                Return "0"
            End Get
        End Property

        Private ReadOnly Property GetPokemonPosition() As String
            Get
                If net.Pokemon3D.Game.Player.Temp.IsInBattle = False Then
                    Return Screen.Level.OverworldPokemon.Position.X.ToString() & "|" & Math.Floor(Screen.Level.OverworldPokemon.Position.Y).ToString() & "|" & Screen.Level.OverworldPokemon.Position.Z.ToString()
                End If
                Return "0|-10|0"
            End Get
        End Property

        Private ReadOnly Property GetPokemonSkin() As String
            Get
                Dim shinyString As String = "N"
                If Core.Player.GetWalkPokemon().IsShiny = True Then
                    shinyString = "S"
                End If
                Return "[POKEMON|" & shinyString & "]" & Core.Player.GetWalkPokemon().Number.ToString() & PokemonForms.GetOverworldAddition(Core.Player.GetWalkPokemon())
            End Get
        End Property

        Private ReadOnly Property GetPokemonFacing() As String
            Get
                Return Screen.Level.OverworldPokemon.faceRotation.ToString()
            End Get
        End Property

#End Region

    End Class

End Namespace