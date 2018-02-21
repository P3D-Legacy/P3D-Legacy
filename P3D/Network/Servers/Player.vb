Namespace Servers

    Public Class Player

        Public Const PLAYERDATAITEMSCOUNT As Integer = 15

        Private _gameVersion As String = ""
        Private _isGameJoltPlayer As Boolean = False

        Private _serversID As Integer = 0
        Private _initialized As Boolean = False

        Private _gameJoltID As String = ""

        Private _name As String = ""
        Private _position As Vector3 = New Vector3(0)
        Private _skin As String = ""
        Private _facing As Integer = 0
        Private _moving As Boolean = False
        Private _levelFile As String = ""
        Private _decimalSeparator As String = ","
        Private _busyType As Integer = 0
        Private _gameMode As String = ""

        Private _pokemonPosition As Vector3 = New Vector3(0)
        Private _pokemonFacing As Integer = 0
        Private _pokemonSkin As String = ""
        Private _pokemonVisible As Boolean = False

        Public ReadOnly Property Moving() As Boolean
            Get
                Return Me._moving
            End Get
        End Property

        Public ReadOnly Property LevelFile() As String
            Get
                Return Me._levelFile
            End Get
        End Property

        Public ReadOnly Property BusyType() As Integer
            Get
                Return Me._busyType
            End Get
        End Property

        Public Property ServersID() As Integer
            Get
                Return Me._serversID
            End Get
            Set(value As Integer)
                Me._serversID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property

        Public ReadOnly Property GameJoltId() As String
            Get
                Return Me._gameJoltID
            End Get
        End Property

        Public ReadOnly Property Initialized() As Boolean
            Get
                Return Me._initialized
            End Get
        End Property

        Public ReadOnly Property Position() As Vector3
            Get
                Return Me._position
            End Get
        End Property

        Public ReadOnly Property Skin() As String
            Get
                Return Me._skin
            End Get
        End Property

        Public ReadOnly Property Facing() As Integer
            Get
                Return Me._facing
            End Get
        End Property

        Public ReadOnly Property PokemonPosition() As Vector3
            Get
                Return Me._pokemonPosition
            End Get
        End Property

        Public ReadOnly Property PokemonFacing() As Integer
            Get
                Return Me._pokemonFacing
            End Get
        End Property

        Public ReadOnly Property PokemonSkin() As String
            Get
                Return Me._pokemonSkin
            End Get
        End Property

        Public ReadOnly Property PokemonVisible() As Boolean
            Get
                Return Me._pokemonVisible
            End Get
        End Property

        Public Sub ApplyNewData(ByVal p As Package)
            '---General information---
            '0: Active gamemode
            '1: isgamejoltsave
            '2: GameJoltID
            '3: DecimalSeparator

            '---Player Information---
            '4: playername
            '5: levelfile
            '6: position
            '7: facing
            '8: moving
            '9: skin
            '10: busytype

            '---OverworldPokemon---
            '11: Visible
            '12: Position
            '13: Skin
            '14: facing

            Dim d() As String = p.DataItems.ToArray()

            For i = 0 To PLAYERDATAITEMSCOUNT - 1
                Dim value As String = d(i)
                If value <> "" Then
                    Select Case i
                        Case 0 '0: Active gamemode
                            Me._gameMode = value
                        Case 1 '1: isgamejoltsave
                            Me._isGameJoltPlayer = CBool(value)
                        Case 2 '2: GameJoltID
                            Me._gameJoltID = value
                        Case 3 '3: DecimalSeparator
                            Me._decimalSeparator = value
                        Case 4 '4: playername
                            Me._name = value
                        Case 5 '5: levelfile
                            Me._levelFile = value
                        Case 6 '6: position
                            Dim posString As String = value.Replace(Me._decimalSeparator, GameController.DecSeparator)
                            Dim posList() As String = posString.Split(CChar("|"))

                            Me._position = New Vector3(CSng(posList(0)), CSng(posList(1)), CSng(posList(2)))
                        Case 7 '7: facing
                            Me._facing = CInt(value)
                        Case 8 '8: moving
                            Me._moving = CBool(value)
                        Case 9 '9: skin
                            Me._skin = CStr(value)
                        Case 10 '10: busytype
                            Me._busyType = CInt(value)
                        Case 11 '11: Visible
                            Me._pokemonVisible = CBool(value)
                        Case 12 '12: Position
                            Dim posString As String = value.Replace(Me._decimalSeparator, GameController.DecSeparator)
                            Dim posList() As String = posString.Split(CChar("|"))

                            Me._pokemonPosition = New Vector3(CSng(posList(0)), CSng(posList(1)), CSng(posList(2)))
                        Case 13 '13: Skin
                            Me._pokemonSkin = value
                        Case 14 '14: facing
                            Me._pokemonFacing = CInt(value)
                    End Select
                End If
            Next

            Me._initialized = True
            Core.ServersManager.PlayerManager.NeedsUpdate = True
        End Sub

    End Class

End Namespace