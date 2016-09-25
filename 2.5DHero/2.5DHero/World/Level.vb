''' <summary>
''' A class that manages the collection of entities to represent a map.
''' </summary>
Public Class Level

#Region "Fields"

    Private _routeSign As RouteSign = Nothing
    Private _world As World = Nothing
    Private _pokemonEncounter As PokemonEncounter = Nothing

    ''' <summary>
    ''' Stores warp data for warping to a new map.
    ''' </summary>
    Public WarpData As WarpDataStruct

    ''' <summary>
    ''' Stores temporary Pokémon encounter data.
    ''' </summary>
    Public PokemonEncounterData As PokemonEcounterDataStruct

    'Level states:
    Private _isSurfing As Boolean = False
    Private _isRiding As Boolean = False
    Private _usedStrength As Boolean = False
    Private _isDark As Boolean = False
    Private _walkedSteps As Integer = 0

    Private _offsetMapUpdateDelay As Integer = 50 'Ticks until the next Offset Map update occurs.

    'Map properties:
    Private _terrain As Terrain = New Terrain(net.Pokemon3D.Game.Terrain.TerrainTypes.Plain)
    Private _mapName As String = ""
    Private _musicLoop As String = ""
    Private _levelFile As String = ""
    Private _canTeleport As Boolean = True
    Private _canDig As Boolean = False
    Private _canFly As Boolean = False
    Private _rideType As Integer = 0
    Private _weatherType As Integer = 0
    Private _environmentType As Integer = 0
    Private _wildPokemonGrass As Boolean = True
    Private _wildPokemonFloor As Boolean = False
    Private _wildPokemonWater As Boolean = True
    Private _showOverworldPokemon As Boolean = True
    Private _currentRegion As String = "Johto"
    Private _hiddenabilitychance As Integer = 0
    Private _lightingType As Integer = 0
    Private _isSafariZone As Boolean = False
    Private _isBugCatchingContest As Boolean = False

    Private _bugCatchingContestData As String = ""
    Private _battleMapData As String = ""

    'Entity enumerations:
    Private _ownPlayer As OwnPlayer
    Private _ownOverworldPokemon As OverworldPokemon

    Private _entities As New List(Of Entity)
    Private _floors As New List(Of Entity)
    Private _shaders As New List(Of Shader)
    Private _backdropRenderer As BackdropRenderer

    Private _networkPlayers As New List(Of NetworkPlayer)
    Private _networkPokemon As New List(Of NetworkPokemon)

    Private _offsetMapEntities As New List(Of Entity)
    Private _offsetMapFloors As New List(Of Entity)

    'Radio:
    Private _isRadioOn As Boolean = False
    Private _selectedRadioStation As GameJolt.PokegearScreen.RadioStation = Nothing
    Private _radioChannels As New List(Of Decimal)

    Private _offsetTimer As New Timers.Timer()
    Private _isUpdatingOffsetMaps As Boolean = False

#End Region

#Region "Properties"

    ''' <summary>
    ''' The Terrain of this level.
    ''' </summary>
    Public ReadOnly Property Terrain() As Terrain
        Get
            Return Me._terrain
        End Get
    End Property

    ''' <summary>
    ''' A RouteSign on the top left corner of the screen to display the map's name.
    ''' </summary>
    Public ReadOnly Property RouteSign() As RouteSign
        Get
            Return Me._routeSign
        End Get
    End Property

    ''' <summary>
    ''' Indicates wether the player is surfing.
    ''' </summary>
    Public Property Surfing() As Boolean
        Get
            Return _isSurfing
        End Get
        Set(value As Boolean)
            Me._isSurfing = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates wether the player is riding.
    ''' </summary>
    Public Property Riding() As Boolean
        Get
            Return Me._isRiding
        End Get
        Set(value As Boolean)
            Me._isRiding = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates wether the player used Strength already.
    ''' </summary>
    Public Property UsedStrength() As Boolean
        Get
            Return Me._usedStrength
        End Get
        Set(value As Boolean)
            Me._usedStrength = value
        End Set
    End Property

    ''' <summary>
    ''' The reference to the active OwnPlayer instance.
    ''' </summary>
    Public Property OwnPlayer() As OwnPlayer
        Get
            Return Me._ownPlayer
        End Get
        Set(value As OwnPlayer)
            Me._ownPlayer = value
        End Set
    End Property

    ''' <summary>
    ''' The reference to the active OverworldPokemon instance.
    ''' </summary>
    Public Property OverworldPokemon() As OverworldPokemon
        Get
            Return Me._ownOverworldPokemon
        End Get
        Set(value As OverworldPokemon)
            Me._ownOverworldPokemon = value
        End Set
    End Property

    ''' <summary>
    ''' The array of entities composing the map.
    ''' </summary>
    Public Property Entities() As List(Of Entity)
        Get
            Return Me._entities
        End Get
        Set(value As List(Of Entity))
            Me._entities = value
        End Set
    End Property

    ''' <summary>
    ''' The array of floors the player can move on.
    ''' </summary>
    Public Property Floors() As List(Of Entity)
        Get
            Return Me._floors
        End Get
        Set(value As List(Of Entity))
            Me._floors = value
        End Set
    End Property

    ''' <summary>
    ''' The array of shaders that add specific lighting to the map.
    ''' </summary>
    Public Property Shaders() As List(Of Shader)
        Get
            Return Me._shaders
        End Get
        Set(value As List(Of Shader))
            Me._shaders = value
        End Set
    End Property

    ''' <summary>
    ''' The array of players on the server to render.
    ''' </summary>
    Public Property NetworkPlayers() As List(Of NetworkPlayer)
        Get
            Return Me._networkPlayers
        End Get
        Set(value As List(Of NetworkPlayer))
            Me._networkPlayers = value
        End Set
    End Property

    ''' <summary>
    ''' The array of Pokémon on the server to render.
    ''' </summary>
    Public Property NetworkPokemon() As List(Of NetworkPokemon)
        Get
            Return Me._networkPokemon
        End Get
        Set(value As List(Of NetworkPokemon))
            Me._networkPokemon = value
        End Set
    End Property

    ''' <summary>
    ''' The array of entities the offset maps are composed of.
    ''' </summary>
    Public Property OffsetmapEntities() As List(Of Entity)
        Get
            Return Me._offsetMapEntities
        End Get
        Set(value As List(Of Entity))
            Me._offsetMapEntities = value
        End Set
    End Property

    ''' <summary>
    ''' The array of floors the offset maps are composed of.
    ''' </summary>
    Public Property OffsetmapFloors() As List(Of Entity)
        Get
            Return Me._offsetMapFloors
        End Get
        Set(value As List(Of Entity))
            Me._offsetMapFloors = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the current map.
    ''' </summary>
    ''' <remarks>This name gets displayed on the RouteSign.</remarks>
    Public Property MapName() As String
        Get
            Return Me._mapName
        End Get
        Set(value As String)
            Me._mapName = value
        End Set
    End Property

    ''' <summary>
    ''' The default background music for this level.
    ''' </summary>
    ''' <remarks>Doesn't play for surfing, riding and radio.</remarks>
    Public Property MusicLoop() As String
        Get
            Return Me._musicLoop
        End Get
        Set(value As String)
            Me._musicLoop = value
        End Set
    End Property

    ''' <summary>
    ''' The file this level got loaded from.
    ''' </summary>
    ''' <remarks>The path is relative to the \maps\ or \GameMode\[gamemode]\maps\ path.</remarks>
    Public Property LevelFile() As String
        Get
            Return Me._levelFile
        End Get
        Set(value As String)
            Me._levelFile = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the player can use the move Teleport.
    ''' </summary>
    Public Property CanTeleport As Boolean
        Get
            Return Me._canTeleport
        End Get
        Set(value As Boolean)
            Me._canTeleport = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the player can use the move Dig or an Escape Rope.
    ''' </summary>
    Public Property CanDig As Boolean
        Get
            Return Me._canDig
        End Get
        Set(value As Boolean)
            Me._canDig = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the player can use the move Fly.
    ''' </summary>
    Public Property CanFly As Boolean
        Get
            Return Me._canFly
        End Get
        Set(value As Boolean)
            Me._canFly = value
        End Set
    End Property

    ''' <summary>
    ''' The type of Ride the player can use on this map.
    ''' </summary>
    ''' <remarks>0 = Depends on CanDig and CanFly, 1 = True, 2 = False</remarks>
    Public Property RideType As Integer
        Get
            Return Me._rideType
        End Get
        Set(value As Integer)
            Me._rideType = value
        End Set
    End Property

    ''' <summary>
    ''' The Weather on this map.
    ''' </summary>
    ''' <remarks>For the weather, look at the WeatherTypes enumeration in World.vb</remarks>
    Public Property WeatherType As Integer
        Get
            Return Me._weatherType
        End Get
        Set(value As Integer)
            Me._weatherType = value
        End Set
    End Property

    ''' <summary>
    ''' The environment type for this map.
    ''' </summary>
    Public Property EnvironmentType As Integer
        Get
            Return Me._environmentType
        End Get
        Set(value As Integer)
            Me._environmentType = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the player can encounter wild Pokémon in the Grass entities.
    ''' </summary>
    Public Property WildPokemonGrass As Boolean
        Get
            Return Me._wildPokemonGrass
        End Get
        Set(value As Boolean)
            _wildPokemonGrass = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the player can encounter wild Pokémon on every floor tile.
    ''' </summary>
    Public Property WildPokemonFloor As Boolean
        Get
            Return Me._wildPokemonFloor
        End Get
        Set(value As Boolean)
            Me._wildPokemonFloor = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the player can encounter wild Pokémon while surfing.
    ''' </summary>
    Public Property WildPokemonWater As Boolean
        Get
            Return Me._wildPokemonWater
        End Get
        Set(value As Boolean)
            Me._wildPokemonWater = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the map is dark, and needs to be lightened up by Flash.
    ''' </summary>
    Public Property IsDark As Boolean
        Get
            Return Me._isDark
        End Get
        Set(value As Boolean)
            Me._isDark = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the Overworld Pokémon is visible.
    ''' </summary>
    Public Property ShowOverworldPokemon As Boolean
        Get
            Return Me._showOverworldPokemon
        End Get
        Set(value As Boolean)
            Me._showOverworldPokemon = value
        End Set
    End Property

    ''' <summary>
    ''' The amount of walked steps on this map.
    ''' </summary>
    Public Property WalkedSteps As Integer
        Get
            Return Me._walkedSteps
        End Get
        Set(value As Integer)
            Me._walkedSteps = value
        End Set
    End Property

    ''' <summary>
    ''' The region this map is assigned to.
    ''' </summary>
    ''' <remarks>The default is "Johto".</remarks>
    Public Property CurrentRegion As String
        Get
            Return Me._currentRegion
        End Get
        Set(value As String)
            Me._currentRegion = value
        End Set
    End Property

    ''' <summary>
    ''' Chance of a Hidden Ability being on a wild pokemon.
    ''' </summary>
    Public Property HiddenAbilityChance As Integer
        Get
            Return Me._hiddenabilitychance
        End Get
        Set(value As Integer)
            Me._hiddenabilitychance = value
        End Set
    End Property

    ''' <summary>
    ''' The LightingType of this map. More information in the Level\UpdateLighting.
    ''' </summary>
    Public Property LightingType As Integer
        Get
            Return Me._lightingType
        End Get
        Set(value As Integer)
            Me._lightingType = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the map is a part of the SafariZone. This changes the Battle Menu and the MenuScreen.
    ''' </summary>
    Public Property IsSafariZone As Boolean
        Get
            Return Me._isSafariZone
        End Get
        Set(value As Boolean)
            Me._isSafariZone = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the map is a part of the BugCatchingContest. This changes the Battle Menu and the MenuScreen.
    ''' </summary>
    Public Property IsBugCatchingContest As Boolean
        Get
            Return Me._isBugCatchingContest
        End Get
        Set(value As Boolean)
            Me._isBugCatchingContest = value
        End Set
    End Property

    ''' <summary>
    ''' Holds data for the Bug Catching Contest.
    ''' </summary>
    ''' <remarks>Composed of 3 values, separated by ",": 0 = script location for ending the contest, 1 = script location for selecting the remaining balls item, 2 = Menu Item name for the remaining balls item.</remarks>
    Public Property BugCatchingContestData As String
        Get
            Return Me._bugCatchingContestData
        End Get
        Set(value As String)
            Me._bugCatchingContestData = value
        End Set
    End Property

    ''' <summary>
    ''' Used to modify the Battle Map camera position.
    ''' </summary>
    ''' <remarks>Data: MapName,x,y,z OR Mapname OR x,y,z OR empty</remarks>
    Public Property BattleMapData() As String
        Get
            Return Me._battleMapData
        End Get
        Set(value As String)
            Me._battleMapData = value
        End Set
    End Property

    ''' <summary>
    ''' The instance of the World class, handling time, season and weather based operations.
    ''' </summary>
    Public Property World() As World
        Get
            Return Me._world
        End Get
        Set(value As World)
            Me._world = value
        End Set
    End Property

    ''' <summary>
    ''' Wether the radio is currently activated.
    ''' </summary>
    Public Property IsRadioOn() As Boolean
        Get
            Return Me._isRadioOn
        End Get
        Set(value As Boolean)
            Me._isRadioOn = value
        End Set
    End Property

    ''' <summary>
    ''' The currently selected radio station. If possible, this will replace the MusicLoop.
    ''' </summary>
    Public Property SelectedRadioStation() As GameJolt.PokegearScreen.RadioStation
        Get
            Return Me._selectedRadioStation
        End Get
        Set(value As GameJolt.PokegearScreen.RadioStation)
            Me._selectedRadioStation = value
        End Set
    End Property

    ''' <summary>
    ''' Allowed radio channels on this map.
    ''' </summary>
    Public Property AllowedRadioChannels() As List(Of Decimal)
        Get
            Return Me._radioChannels
        End Get
        Set(value As List(Of Decimal))
            Me._radioChannels = value
        End Set
    End Property

    ''' <summary>
    ''' Handles wild Pokémon encounters.
    ''' </summary>
    Public ReadOnly Property PokemonEncounter() As PokemonEncounter
        Get
            Return Me._pokemonEncounter
        End Get
    End Property

    ''' <summary>
    ''' The backdrop renderer of this level.
    ''' </summary>
    Public ReadOnly Property BackdropRenderer() As BackdropRenderer
        Get
            Return _backdropRenderer
        End Get
    End Property

#End Region

#Region "Structures"

    ''' <summary>
    ''' A structure to store warp data in.
    ''' </summary>
    Public Structure WarpDataStruct
        ''' <summary>
        ''' The destination map file.
        ''' </summary>
        Public WarpDestination As String

        ''' <summary>
        ''' The position to warp the player to.
        ''' </summary>
        Public WarpPosition As Vector3

        ''' <summary>
        ''' The check to see if the player should get warped next tick.
        ''' </summary>
        Public DoWarpInNextTick As Boolean

        ''' <summary>
        ''' Amount of 90° rotations counter clockwise.
        ''' </summary>
        Public WarpRotations As Integer

        ''' <summary>
        ''' The correct camera yaw to set the camera to after the warping.
        ''' </summary>
        Public CorrectCameraYaw As Single

        ''' <summary>
        ''' If the warp action got triggered by a warp block.
        ''' </summary>
        Public IsWarpBlock As Boolean
    End Structure

    ''' <summary>
    ''' A structure to store wild pokémon encounter data in.
    ''' </summary>
    Public Structure PokemonEcounterDataStruct
        ''' <summary>
        ''' The assumed position the player will be in when encounterning the Pokémon.
        ''' </summary>
        Public Position As Vector3

        ''' <summary>
        ''' Wether the player encountered a Pokémon.
        ''' </summary>
        Public EncounteredPokemon As Boolean

        ''' <summary>
        ''' The encounter method.
        ''' </summary>
        Public Method As Spawner.EncounterMethods

        ''' <summary>
        ''' The link to the .poke file used to spawn the Pokémon in.
        ''' </summary>
        Public PokeFile As String
    End Structure

#End Region

    ''' <summary>
    ''' Creates a new instance of the Level class.
    ''' </summary>
    Public Sub New()
        Me._routeSign = New RouteSign()
        Me.WarpData = New WarpDataStruct()
        Me.PokemonEncounterData = New PokemonEcounterDataStruct()
        Me._pokemonEncounter = New PokemonEncounter(Me)

        Me.StartOffsetMapUpdate()

        Me._backdropRenderer = New BackdropRenderer()
        Me._backdropRenderer.Initialize()
    End Sub

    ''' <summary>
    ''' Initializes the offset map update cycle.
    ''' </summary>
    Public Sub StartOffsetMapUpdate()
        If Not Me._offsetTimer Is Nothing Then
            Me._offsetTimer.Stop()
        End If

        Me._offsetTimer = New Timers.Timer()
        Me._offsetTimer.Interval = 16
        Me._offsetTimer.AutoReset = True
        AddHandler Me._offsetTimer.Elapsed, AddressOf Me.UpdateOffsetMap
        Me._offsetTimer.Start()

        Logger.Debug("Started Offset map update")
    End Sub

    Public Sub StopOffsetMapUpdate()
        Me._offsetTimer.Stop()
        While Me._isUpdatingOffsetMaps : End While

        Logger.Debug("Stopped Offset map update")
    End Sub

    ''' <summary>
    ''' Loads a level from a levelfile.
    ''' </summary>
    ''' <param name="Levelpath">The path to load the level from. Start with "|" to prevent loading a levelfile.</param>
    Public Sub Load(ByVal Levelpath As String)
        'Create a parameter array to pass over to the LevelLoader.
        Dim params As New List(Of Object)
        params.AddRange({Levelpath, False, New Vector3(0, 0, 0), 0, New List(Of String)})

        'Create the world and load the level.
        World = New World(0, 0)

        If Levelpath.StartsWith("|") = False Then
            Me.StopOffsetMapUpdate()
            Dim levelLoader As New LevelLoader()
            levelLoader.LoadLevel(params.ToArray())
        Else
            Logger.Debug("Don't attempt to load a levelfile.")
        End If

        'Create own player entity and OverworldPokémon entity and add them to the entity enumeration.
        OwnPlayer = New OwnPlayer(0, 0, 0, {net.Pokemon3D.Game.TextureManager.DefaultTexture}, Core.Player.Skin, 0, 0, "", "Gold", 0)
        OverworldPokemon = New OverworldPokemon(Screen.Camera.Position.X, Screen.Camera.Position.Y, Screen.Camera.Position.Z + 1)
        OverworldPokemon.ChangeRotation()
        Entities.AddRange({OwnPlayer, OverworldPokemon})

        Me.Surfing = Core.Player.startSurfing
        Me.StartOffsetMapUpdate()
    End Sub

    ''' <summary>
    ''' Renders the level.
    ''' </summary>
    Public Sub Draw()
        Me._backdropRenderer.Draw()

        'Set the effect's View and Projection matrices.
        Screen.Effect.View = Screen.Camera.View
        Screen.Effect.Projection = Screen.Camera.Projection

        'Reset the Debug values.
        DebugDisplay.DrawnVertices = 0
        DebugDisplay.MaxVertices = 0
        DebugDisplay.MaxDistance = 0

        If Core.GameOptions.LoadOffsetMaps > 0 Then
            Me.RenderOffsetMap() 'Only render offset maps if they are activated.
        End If

        'Render floors.
        For i = 0 To Me.Floors.Count - 1
            If i <= Me.Floors.Count - 1 Then
                Me.Floors(i).Render()
                DebugDisplay.MaxVertices += Me.Floors(i).VertexCount
            End If
        Next

        'Render all other entities.
        For i = 0 To Me.Entities.Count - 1
            If i <= Me.Entities.Count - 1 Then
                Me.Entities(i).Render()
                DebugDisplay.MaxVertices += Me.Entities(i).VertexCount
            End If
        Next

        If IsDark = True Then
            DrawFlashOverlay()
        End If
    End Sub

    ''' <summary>
    ''' Updates the level's logic.
    ''' </summary>
    Public Sub Update()
        Me._backdropRenderer.Update()

        Me.UpdatePlayerWarp()
        Me._pokemonEncounter.TriggerBattle()

        'Reload map from file (debug/sandbox):
        If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            If KeyBoardHandler.KeyPressed(Keys.R) = True And Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Core.OffsetMaps.Clear()
                Logger.Debug(String.Format("Reload map file: {0}", Me._levelFile))
                Me.Load(LevelFile)
            End If
        End If

        'Update all network players and Pokémon:
        If JoinServerScreen.Online = True Then
            Core.ServersManager.PlayerManager.UpdatePlayers()
        End If

        'Call Update and UpdateEntity methods of all entities:
        Me.UpdateEntities()
    End Sub

    ''' <summary>
    ''' Updates all entities on the map and offset map and sorts the enumarations.
    ''' </summary>
    Public Sub UpdateEntities()
        'Update and remove entities:
        If LevelLoader.IsBusy = False Then
            For i = 0 To Entities.Count - 1
                If i <= Entities.Count - 1 Then
                    If Entities.Count - 1 >= i AndAlso Entities(i).CanBeRemoved = True Then
                        Entities.RemoveAt(i)
                        i -= 1
                    Else
                        If Entities(i).NeedsUpdate = True Then
                            Entities(i).Update()
                        End If
                    End If
                Else
                    Exit For
                End If
            Next
        End If

        'UpdateEntity for all entities:
        For i = 0 To Me.Entities.Count - 1
            If i <= Me.Entities.Count - 1 Then
                Me.Entities(i).UpdateEntity()
            End If
        Next

        'UpdateEntity for all floors:
        For i = 0 To Me.Floors.Count - 1
            If i <= Me.Floors.Count - 1 Then
                Me.Floors(i).UpdateEntity()
            End If
        Next

        Me.SortEntities()
    End Sub

    ''' <summary>
    ''' Sorts the entity enumerations.
    ''' </summary>
    Public Sub SortEntities()
        If LevelLoader.IsBusy = False Then
            Entities = (From f In Entities Order By f.CameraDistance Descending).ToList()
        End If
    End Sub

    ''' <summary>
    ''' Sorts and updates offset map entities.
    ''' </summary>
    Public Sub UpdateOffsetMap()
        Me._isUpdatingOffsetMaps = True
        If Core.GameOptions.LoadOffsetMaps > 0 Then
            'The Update function of entities on offset maps are not getting called.

            If Me._offsetMapUpdateDelay <= 0 Then 'Only when the delay is 0, update.
                'Sort the list:
                If LevelLoader.IsBusy = False Then
                    OffsetmapEntities = (From e In OffsetmapEntities Order By e.CameraDistance Descending).ToList()
                End If

                Me._offsetMapUpdateDelay = Core.GameOptions.LoadOffsetMaps - 1 'Set the new delay

                'Remove entities that CanBeRemoved (see what I did there?)
                For i = 0 To OffsetmapEntities.Count - 1
                    If i <= OffsetmapEntities.Count - 1 Then
                        If OffsetmapEntities(i).CanBeRemoved = True Then
                            OffsetmapEntities.RemoveAt(i)
                            i -= 1
                        End If
                    Else
                        Exit For
                    End If
                Next

                'Call UpdateEntity on all offset map entities.
                For i = Me.OffsetmapEntities.Count - 1 To 0 Step -1
                    If i <= Me.OffsetmapEntities.Count - 1 Then
                        Me.OffsetmapEntities(i).UpdateEntity()
                    End If
                Next

                'Call UpdateEntity on all offset map floors.
                For i = Me.OffsetmapFloors.Count - 1 To 0 Step -1
                    If i <= Me.OffsetmapFloors.Count - 1 Then
                        Me.OffsetmapFloors(i).UpdateEntity()
                    End If
                Next
            Else
                Me._offsetMapUpdateDelay -= 1
            End If
        End If
        Me._isUpdatingOffsetMaps = False
    End Sub

    ''' <summary>
    ''' Renders offset map entities.
    ''' </summary>
    Private Sub RenderOffsetMap()
        'Render floors:
        For i = 0 To Me.OffsetmapFloors.Count - 1
            If i <= Me.OffsetmapFloors.Count - 1 Then
                If Not Me.OffsetmapFloors(i) Is Nothing Then
                    Me.OffsetmapFloors(i).Render()
                    DebugDisplay.MaxVertices += Me.OffsetmapFloors(i).VertexCount
                End If
            End If
        Next

        'Render entities:
        For i = 0 To Me.OffsetmapEntities.Count - 1
            If i <= Me.OffsetmapEntities.Count - 1 Then
                If Not Me.OffsetmapEntities(i) Is Nothing Then
                    Me.OffsetmapEntities(i).Render()
                    DebugDisplay.MaxVertices += Me.OffsetmapEntities(i).VertexCount
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Draws the flash overlay to the screen.
    ''' </summary>
    Private Sub DrawFlashOverlay()
        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Overworld\flash_overlay"), New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), Color.White)
    End Sub

    ''' <summary>
    ''' Handles warp events for the player.
    ''' </summary>
    Private Sub UpdatePlayerWarp()
        If WarpData.DoWarpInNextTick = True Then 'If a warp event got scheduled.
            'Disable wild Pokémon:
            Me._wildPokemonFloor = False
            Me.PokemonEncounterData.EncounteredPokemon = False

            'Set the surfing flag for the next map:
            Core.Player.startSurfing = Surfing

            'Change the player position:
            Screen.Camera.Position = WarpData.WarpPosition

            Dim tempProperties As String = Me.CanDig.ToString() & "," & Me.CanFly.ToString() 'Store properties to determine if the "enter" sound should be played.

            'Store skin values:
            Dim usingGameJoltTexture As Boolean = OwnPlayer.UsingGameJoltTexture
            Core.Player.Skin = OwnPlayer.SkinName

            'Load the new level:
            Dim params As New List(Of Object)
            params.AddRange({WarpData.WarpDestination, False, New Vector3(0, 0, 0), 0, New List(Of String)})

            World = New World(0, 0)

            Me.StopOffsetMapUpdate()
            Dim levelLoader As New LevelLoader()
            levelLoader.LoadLevel(params.ToArray())

            Core.Player.AddVisitedMap(Me.LevelFile) 'Add new map to visited maps list.
            UsedStrength = False 'Disable strength usuage upon map switch.
            Me.Surfing = Core.Player.startSurfing 'Set the surfing property after map switch.

            'Create player and Pokémon entities.
            OwnPlayer = New OwnPlayer(0, 0, 0, {net.Pokemon3D.Game.TextureManager.DefaultTexture}, Core.Player.Skin, 0, 0, "", "Gold", 0)
            OwnPlayer.SetTexture(Core.Player.Skin, usingGameJoltTexture)

            OverworldPokemon = New OverworldPokemon(Screen.Camera.Position.X, Screen.Camera.Position.Y, Screen.Camera.Position.Z + 1)
            OverworldPokemon.Visible = False
            OverworldPokemon.warped = True
            Entities.AddRange({OwnPlayer, OverworldPokemon})

            'Set ride skin, if needed.
            If Riding = True And CanRide() = False Then
                Riding = False
                OwnPlayer.SetTexture(Core.Player.TempRideSkin, True)
                Core.Player.Skin = Core.Player.TempRideSkin
            End If

            'If any turns after the warp are defined, apply them:
            Screen.Camera.InstantTurn(WarpData.WarpRotations)

            'Make the routesign appear:
            Me._routeSign.Setup(MapName)

            'Play the correct music track.
            If IsRadioOn = True AndAlso GameJolt.PokegearScreen.StationCanPlay(Me.SelectedRadioStation) = True Then
                MusicManager.PlayMusic(SelectedRadioStation.Music, True)
            Else
                IsRadioOn = False
                If Me.Surfing = True Then
                    MusicManager.PlayMusic("surf", True)
                Else
                    If Me.Riding = True Then
                        MusicManager.PlayMusic("ride", True)
                    Else
                        MusicManager.PlayMusic(MusicLoop, True)
                    End If
                End If
            End If

            'Initialize the world with newly loaded environment variables:
            World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            'If this map is on the restplaces list, set the player's last restplace to this map.
            Dim restplaces As List(Of String) = System.IO.File.ReadAllLines(GameModeManager.GetMapPath("restplaces.dat")).ToList()

            For Each line As String In restplaces
                Dim place As String = line.GetSplit(0, "|")
                If place = LevelFile Then
                    Core.Player.LastRestPlace = place
                    Core.Player.LastRestPlacePosition = line.GetSplit(1, "|")
                End If
            Next

            'If the warp happened through a warp block, make the player walk one step forward after switching to the new map.
            If Screen.Camera.IsMoving = True And WarpData.IsWarpBlock = True Then
                Screen.Camera.StopMovement()
                Screen.Camera.Move(1.0F)
            End If

            'Because of the map change, Roaming Pokémon are moving to their next location on the world map.
            RoamingPokemon.ShiftRoamingPokemon(-1)

            'Check if the enter sound should be played by checking if CanDig or CanFly properties are different from the last map.
            If tempProperties <> Me.CanDig.ToString() & "," & Me.CanFly.ToString() Then
                SoundManager.PlaySound("enter", False)
            End If

            'Unlock the yaw on the camera.
            CType(Screen.Camera, OverworldCamera).YawLocked = False
            NetworkPlayer.ScreenRegionChanged()

            'If a warp occured, update the camera:
            Screen.Camera.Update()

            'Disable the warp check:
            Me.WarpData.DoWarpInNextTick = False
            WarpData.IsWarpBlock = False

            If Core.ServersManager.ServerConnection.Connected = True Then
                'Update network players:
                Core.ServersManager.PlayerManager.NeedsUpdate = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns a list of all NPCs on the map.
    ''' </summary>
    Public Function GetNPCs() As List(Of NPC)
        Dim reList As New List(Of NPC)

        For Each Entity As Entity In Me.Entities
            If Entity.EntityID = "NPC" Then
                reList.Add(CType(Entity, NPC))
            End If
        Next

        Return reList
    End Function

    ''' <summary>
    ''' Returns an NPC based on their ID.
    ''' </summary>
    ''' <param name="ID">The ID of the NPC to return from the level.</param>
    ''' <returns>Returns either a matching NPC or Nothing.</returns>
    Public Function GetNPC(ByVal ID As Integer) As NPC
        For Each NPC As NPC In GetNPCs()
            If NPC.NPCID = ID Then
                Return NPC
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns an NPC based on the entity ID.
    ''' </summary>
    Public Function GetEntity(ByVal ID As Integer) As Entity
        If ID = -1 Then
            Throw New Exception("-1 is the default value for NOT having an ID, therefore is not a valid ID.")
        Else
            For Each ent As Entity In Me.Entities
                If ent.ID = ID Then
                    Return ent
                End If
            Next
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Checks all NPCs on the map for if the player is in their line of sight.
    ''' </summary>
    Public Sub CheckTrainerSights()
        For Each Entity As Entity In Entities
            If Entity.EntityID = "NPC" Then
                Dim NPC As NPC = CType(Entity, NPC)
                If NPC.IsTrainer = True Then
                    NPC.CheckInSight()
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Determines wether the player can use Ride on this map.
    ''' </summary>
    Public Function CanRide() As Boolean
        If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then 'Always true for Sandboxmode and Debug mode.
            Return True
        End If
        If RideType > 0 Then
            Select Case RideType
                Case 1
                    Return True
                Case 2
                    Return False
            End Select
        End If
        If Screen.Level.CanDig = False And Screen.Level.CanFly = False Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Wether the player can move based on the entity around him.
    ''' </summary>
    Public Function CanMove() As Boolean
        For Each e As Entity In Me.Entities
            If e.Position.X = Screen.Camera.Position.X And e.Position.Z = Screen.Camera.Position.Z And CInt(e.Position.Y) = CInt(Screen.Camera.Position.Y) Then
                Return e.LetPlayerMove()
            End If
        Next
        Return True
    End Function

End Class
