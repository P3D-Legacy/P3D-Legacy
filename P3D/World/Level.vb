Imports System.Threading
Imports System.Timers

''' <summary>
''' A class that manages the collection of entities to represent a map.
''' </summary>
Public NotInheritable Class Level

    Implements IDisposable

#Region "Fields"

    ''' <summary>
    ''' Stores warp data for warping to a new map.
    ''' </summary>
    Public WarpData As WarpDataStruct

    ''' <summary>
    ''' Stores temporary Pokémon encounter data.
    ''' </summary>
    Public PokemonEncounterData As PokemonEncounterDataStruct

    ' Level states:

    Private _offsetMapUpdateDelay As Integer = 50 ' Ticks until the next Offset Map update occurs.

    ' Map properties:
    Private _dayTime As World.DayTimes = World.GetTime

    Private _offsetTimer As New Timers.Timer()
    Private _isUpdatingOffsetMaps As Boolean = False

#End Region

#Region "Entity Related"
    ''' <summary>
    ''' Entity insert order. (This is to ensure drawing can be done properly if two entities are of the same distance)
    ''' </summary>
    Private _insertEntityCount As Integer = 0

    ''' <summary>
    ''' Entity Sync Point, using this will ensure that the entity is safe to read and write.
    ''' </summary>
    Public ReadOnly Property EntityReadWriteSync As Object = New Object()

    ''' <summary>
    ''' The array of all possible entities.
    ''' </summary>
    Public ReadOnly Property DrawingEntities As List(Of Entity) = New List(Of Entity)

    ''' <summary>
    ''' The array of floors the player can move on.
    ''' </summary>
    Public ReadOnly Property Floors As List(Of Entity) = New List(Of Entity)

    ''' <summary>
    ''' The array of entities composing the map.
    ''' </summary>
    Public ReadOnly Property Entities As List(Of Entity) = New List(Of Entity)

    ''' <summary>
    ''' The array of npc in the map.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Npcs As List(Of NPC) = New List(Of NPC)

    ''' <summary>
    ''' The array of particles in the map.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Particles As List(Of Particle) = New List(Of Particle)

    ''' <summary>
    ''' The array of shaders that add specific lighting to the map.
    ''' </summary>
    Public ReadOnly Property Shaders As List(Of Shader) = New List(Of Shader)

    ''' <summary>
    ''' The array of floors the offset maps are composed of.
    ''' </summary>
    Public ReadOnly Property OffsetmapFloors As List(Of Entity) = New List(Of Entity)

    ''' <summary>
    ''' The array of entities the offset maps are composed of.
    ''' </summary>
    Public ReadOnly Property OffsetmapEntities As List(Of Entity) = New List(Of Entity)

    ''' <summary>
    ''' The reference to the active OwnPlayer instance.
    ''' </summary>
    Public Property OwnPlayer As OwnPlayer

    ''' <summary>
    ''' The reference to the active OverworldPokemon instance.
    ''' </summary>
    Public Property OverworldPokemon As OverworldPokemon

    ''' <summary>
    ''' The array of players on the server to render.
    ''' </summary>
    Public ReadOnly Property NetworkPlayers As List(Of NetworkPlayer) = New List(Of NetworkPlayer)

    ''' <summary>
    ''' The array of Pokémon on the server to render.
    ''' </summary>
    Public ReadOnly Property NetworkPokemon As List(Of NetworkPokemon) = New List(Of NetworkPokemon)

    Public Sub AddEntity(e As Entity)
        SyncLock EntityReadWriteSync
            If TypeOf e Is Floor Then
                Floors.Add(e)
            ElseIf TypeOf e Is NPC Then
                Npcs.Add(CType(e, NPC))
                Entities.Add(e)
            ElseIf TypeOf e Is Particle Then
                Particles.Add(CType(e, Particle))
                Entities.Add(e)
            ElseIf TypeOf e Is NetworkPlayer Then
                NetworkPlayers.Add(CType(e, NetworkPlayer))
                Entities.Add(e)
            ElseIf TypeOf e Is NetworkPokemon Then
                NetworkPokemon.Add(CType(e, NetworkPokemon))
                Entities.Add(e)
            Else
                Entities.Add(e)
            End If
            
            e.InsertOrder = _insertEntityCount
            _insertEntityCount += 1
            
            DrawingEntities.Add(e)
        End SyncLock
    End Sub

    Public Sub AddOffsetMapEntity(e As Entity)
        SyncLock EntityReadWriteSync
            If TypeOf e Is Floor Then
                OffsetmapFloors.Add(e)
            Else
                OffsetmapEntities.Add(e)
            End If

            e.InsertOrder = _insertEntityCount
            _insertEntityCount += 1

            DrawingEntities.Add(e)
        End SyncLock
    End Sub

    Public Sub ForEachEntity(entityType As List(Of Entity), action As Action(Of Entity))
        SyncLock EntityReadWriteSync
            For Each entity As Entity In entityType
                action.Invoke(entity)
            Next
        End SyncLock
    End Sub

    Public Sub ClearEntity()
        SyncLock EntityReadWriteSync
            _insertEntityCount = 0
            DrawingEntities.Clear()
            Floors.Clear()
            Entities.Clear()
            Npcs.Clear()
            Particles.Clear()
            Shaders.Clear()
            OffsetmapFloors.Clear()
            OffsetmapEntities.Clear()
            Entity.EntityDictionary.Clear()
        End SyncLock
    End Sub

    Private Shared Function SortByDrawOrder(entity As Entity, entity2 As Entity) As Integer
        ' For drawing to be perfect, opaque object needed to be drawn first then transparent by furthest to nearest.
        If TypeOf entity Is Floor AndAlso TypeOf entity2 IsNot Floor Then
            Return 1
        ElseIf TypeOf entity IsNot Floor AndAlso TypeOf entity2 Is Floor Then
            Return -1
        ElseIf TypeOf entity Is Floor AndAlso TypeOf entity2 Is Floor Then
            If entity.CameraDistance < entity2.CameraDistance Then
                Return 1
            ElseIf entity.CameraDistance > entity2.CameraDistance Then
                Return -1
            Else
                If entity.InsertOrder < entity2.InsertOrder Then
                    Return 1
                ElseIf entity.InsertOrder > entity2.InsertOrder Then
                    Return -1
                Else
                    Return 0
                End If
            End If
        Else
            If entity.CameraDistance < entity2.CameraDistance Then
                Return -1
            ElseIf entity.CameraDistance > entity2.CameraDistance Then
                Return 1
            Else
                If entity.InsertOrder < entity2.InsertOrder Then
                    Return 1
                ElseIf entity.InsertOrder > entity2.InsertOrder Then
                    Return -1
                Else
                    Return 0
                End If
            End If
        End If
    End Function
#End Region

#Region "Properties"

    ''' <summary>
    ''' The Terrain of this level.
    ''' </summary>
    Public ReadOnly Property Terrain As Terrain = New Terrain(Terrain.TerrainTypes.Plain)

    ''' <summary>
    ''' A RouteSign on the top left corner of the screen to display the map's name.
    ''' </summary>
    Public ReadOnly Property RouteSign As RouteSign = New RouteSign()

    ''' <summary>
    ''' Indicates whether the player is Surfing.
    ''' </summary>
    Public Property Surfing As Boolean = False

    ''' <summary>
    ''' Indicates whether the player is Riding.
    ''' </summary>
    Public Property Riding As Boolean = False

    ''' <summary>
    ''' Indicates whether the player used Strength already.
    ''' </summary>
    Public Property UsedStrength As Boolean = False

    ''' <summary>
    ''' The name of the current map.
    ''' </summary>
    ''' <remarks>This name gets displayed on the RouteSign.</remarks>
    Public Property MapName As String = ""

    ''' <summary>
    ''' The default background music for this level.
    ''' </summary>
    ''' <remarks>Doesn't play for Surfing, Riding and Radio.</remarks>
    Public Property MusicLoop As String = ""

    ''' <summary>
    ''' The file this level got loaded from.
    ''' </summary>
    ''' <remarks>The path is relative to the \maps\ or \GameMode\[gamemode]\maps\ path.</remarks>
    Public Property LevelFile As String = ""

    ''' <summary>
    ''' Whether the player can use the move Teleport.
    ''' </summary>
    Public Property CanTeleport As Boolean = True

    ''' <summary>
    ''' Whether the player can use the move Dig or an Escape Rope.
    ''' </summary>
    Public Property CanDig As Boolean = False

    ''' <summary>
    ''' Whether the player can use the move Fly.
    ''' </summary>
    Public Property CanFly As Boolean = False

    ''' <summary>
    ''' The type of Ride the player can use on this map.
    ''' </summary>
    ''' <remarks>0 = Depends on CanDig and CanFly, 1 = True, 2 = False</remarks>
    Public Property RideType As Integer = 0

    ''' <summary>
    ''' The Weather on this map.
    ''' </summary>
    ''' <remarks>For the weather, look at the WeatherTypes enumeration in World.vb</remarks>
    Public Property WeatherType As Integer = 0

    ''' <summary>
    ''' The DayTime on this map.
    ''' </summary>
    ''' <remarks>For the day time, look at the DayTime enumeration in World.vb</remarks>
    Public Property DayTime As Integer
        Get
            Select Case Me._dayTime
                Case World.DayTimes.Night
                    Return 1
                Case World.DayTimes.Morning
                    Return 2
                Case World.DayTimes.Day
                    Return 3
                Case World.DayTimes.Evening
                    Return 4
                Case Else
                    Return World.GetTime
            End Select
        End Get
        Set(value As Integer)
            Select Case value
                Case 1
                    Me._dayTime = World.DayTimes.Night
                Case 2
                    Me._dayTime = World.DayTimes.Morning
                Case 3
                    Me._dayTime = World.DayTimes.Day
                Case 4
                    Me._dayTime = World.DayTimes.Evening
                Case Else
                    Me._dayTime = World.GetTime
            End Select
        End Set
    End Property

    ''' <summary>
    ''' If only the Save option should be available in the menu for this map.
    ''' </summary>
    Public Property SaveOnly As Boolean

    ''' <summary>
    ''' The environment type for this map.
    ''' </summary>
    Public Property EnvironmentType As Integer = 0

    ''' <summary>
    ''' Whether the player can encounter wild Pokémon in the Grass entities.
    ''' </summary>
    Public Property WildPokemonGrass As Boolean = True

    ''' <summary>
    ''' Whether the player can encounter wild Pokémon on every floor tile.
    ''' </summary>
    Public Property WildPokemonFloor As Boolean = False

    ''' <summary>
    ''' Whether the player can encounter wild Pokémon while Surfing.
    ''' </summary>
    Public Property WildPokemonWater As Boolean = True

    ''' <summary>
    ''' Whether the map is dark, and needs to be lightened up by Flash.
    ''' </summary>
    Public Property IsDark As Boolean = False

    ''' <summary>
    ''' Whether the Overworld Pokémon is visible.
    ''' </summary>
    Public Property ShowOverworldPokemon As Boolean = True

    ''' <summary>
    ''' The amount of walked steps on this map.
    ''' </summary>
    Public Property WalkedSteps As Integer = 0

    ''' <summary>
    ''' The region this map is assigned to.
    ''' </summary>
    ''' <remarks>The default is "Johto".</remarks>
    Public Property CurrentRegion As String = "Johto"

    ''' <summary>
    ''' Regional forms available on this level.
    ''' </summary>
    Public Property RegionalForm As String = ""

    ''' <summary>
    ''' Chance of a Hidden Ability being on a wild Pokémon.
    ''' </summary>
    Public Property HiddenAbilityChance As Integer = 0

    ''' <summary>
    ''' The LightingType of this map. More information in the Lighting\UpdateLighting.
    ''' </summary>
    Public Property LightingType As Integer = 0

    ''' <summary>
    ''' Whether the map is a part of the Safari Zone. This changes the Battle Menu and the Menu Screen.
    ''' </summary>
    Public Property IsSafariZone As Boolean = False

    ''' <summary>
    ''' Whether the map is a part of the Bug Catching Contest. This changes the Battle Menu and the Menu Screen.
    ''' </summary>
    Public Property IsBugCatchingContest As Boolean = False

    ''' <summary>
    ''' Holds data for the Bug Catching Contest.
    ''' </summary>
    ''' <remarks>Composed of 3 values, separated by ",": 0 = script location for ending the contest, 1 = script location for selecting the remaining balls item, 2 = Menu Item name for the remaining balls item.</remarks>
    Public Property BugCatchingContestData As String = ""

    ''' <summary>
    ''' Used to modify the Battle Map camera position.
    ''' </summary>
    ''' <remarks>Data: MapName,x,y,z OR Mapname OR x,y,z OR empty</remarks>
    Public Property BattleMapData As String = ""

    ''' <summary>
    ''' Used to modify the Battle Map.
    ''' </summary>
    ''' <remarks>Data: MapName,x,y,z OR Mapname OR empty</remarks>
    Public Property SurfingBattleMapData As String

    ''' <summary>
    ''' The instance of the World class, handling time, season and weather based operations.
    ''' </summary>
    Public Property World As World = Nothing

    ''' <summary>
    ''' Whether the Radio is currently activated.
    ''' </summary>
    Public Property IsRadioOn As Boolean = False

    ''' <summary>
    ''' The currently selected Radio station. If possible, this will replace the Music Loop.
    ''' </summary>
    Public Property SelectedRadioStation As GameJolt.PokegearScreen.RadioStation = Nothing

    ''' <summary>
    ''' Allowed Radio channels on this map.
    ''' </summary>
    Public Property AllowedRadioChannels As List(Of Decimal) = New List(Of Decimal)

    ''' <summary>
    ''' Handles wild Pokémon encounters.
    ''' </summary>
    Public ReadOnly Property PokemonEncounter As PokemonEncounter = Nothing

    ''' <summary>
    ''' The backdrop renderer of this level.
    ''' </summary>
    Public ReadOnly Property BackdropRenderer As BackdropRenderer

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
        ''' Amount of 90° rotations counterclockwise.
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
    ''' A structure to store wild Pokémon encounter data in.
    ''' </summary>
    Public Structure PokemonEncounterDataStruct
        ''' <summary>
        ''' The assumed position the player will be in when encounterning the Pokémon.
        ''' </summary>
        Public Position As Vector3

        ''' <summary>
        ''' Whether the player encountered a Pokémon.
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
        WarpData = New WarpDataStruct()
        PokemonEncounterData = New PokemonEncounterDataStruct()
        PokemonEncounter = New PokemonEncounter(Me)

        StartOffsetMapUpdate()

        BackdropRenderer = New BackdropRenderer()
        BackdropRenderer.Initialize()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub

    ''' <summary>
    ''' Initializes the offset map update cycle.
    ''' </summary>
    Public Sub StartOffsetMapUpdate()
        If Not Me._offsetTimer Is Nothing Then
            Me._offsetTimer.Stop()
        End If

        Me._offsetTimer = New Timers.Timer()
        Me._offsetTimer.Interval = Core.GameInstance.GetGame().TargetElapsedTime.TotalMilliseconds
        Me._offsetTimer.AutoReset = True
        AddHandler _offsetTimer.Elapsed, AddressOf UpdateOffsetMap
        Me._offsetTimer.Start()

        Logger.Debug("Started Offset map update")
    End Sub

    Public Sub StopOffsetMapUpdate()
        Me._offsetTimer.Stop()
        While Me._isUpdatingOffsetMaps
            Thread.Sleep(1)
        End While

        Logger.Debug("Stopped Offset map update")
    End Sub

    ''' <summary>
    ''' Loads a level from a levelfile.
    ''' </summary>
    ''' <param name="Levelpath">The path to load the level from. Start with "|" to prevent loading a levelfile.</param>
    Public Sub Load(ByVal Levelpath As String, Optional Reload As Boolean = False)
        ' copy all changed files
        If GameController.IS_DEBUG_ACTIVE Then
            DebugFileWatcher.TriggerReload()
        End If

        ' Create the world and load the level:
        World = New World(0, 0)

        If Levelpath.StartsWith("|") = False Then
            StopOffsetMapUpdate()
            Dim levelLoader As New LevelLoader()
            levelLoader.LoadLevel({Levelpath, False, New Vector3(0, 0, 0), 0, New List(Of String)}, Reload)
        Else
            Logger.Debug("Don't attempt to load a levelfile.")
        End If

        ' Create own player entity and OverworldPokémon entity and add them to the entity enumeration:
        OwnPlayer = New OwnPlayer(0, 0, 0, {TextureManager.DefaultTexture}, Core.Player.Skin, 0, 0, "", "Gold", 0)
        OverworldPokemon = New OverworldPokemon(Screen.Camera.Position.X, Screen.Camera.Position.Y, Screen.Camera.Position.Z + 1)
        OverworldPokemon.ChangeRotation()

        AddEntity(OwnPlayer)
        AddEntity(OverworldPokemon)

        Surfing = Core.Player.startSurfing
        StartOffsetMapUpdate()
    End Sub

    ''' <summary>
    ''' Renders the level.
    ''' </summary>
    Public Sub Draw()
        Me.BackdropRenderer.Draw()

        ' Set the effect's View and Projection matrices:
        Screen.Effect.View = Screen.Camera.View
        Screen.Effect.Projection = Screen.Camera.Projection

        ' Reset the Debug values:
        DebugDisplay.DrawnVertices = 0
        DebugDisplay.MaxVertices = 0
        DebugDisplay.MaxDistance = 0

        SyncLock EntityReadWriteSync
            DrawingEntities.Sort(AddressOf SortByDrawOrder)

            For i As Integer = DrawingEntities.Count - 1 To 0 Step -1
                Dim entity As Entity = DrawingEntities(i)

                If entity.CanBeRemoved Then
                    DrawingEntities.RemoveAt(i)
                Else
                    entity.Render()
                    DebugDisplay.MaxVertices += entity.VertexCount
                End If
            Next
        End SyncLock

        If IsDark = True Then
            DrawFlashOverlay()
        End If
    End Sub

    ''' <summary>
    ''' Updates the level's logic.
    ''' </summary>
    Public Sub Update()
        BackdropRenderer.Update()

        UpdatePlayerWarp()
        PokemonEncounter.TriggerBattle()

        ' Reload map from file (Debug or Sandbox Mode):
        If GameController.IS_DEBUG_ACTIVE OrElse Core.Player.SandBoxMode = True Then
            If KeyBoardHandler.KeyPressed(Keys.R) = True AndAlso Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Core.OffsetMaps.Clear()
                Logger.Debug(String.Format("Reload map file: {0}", LevelFile))
                Load(LevelFile, True)
            End If
        End If

        ' Update all network players and Pokémon:
        If JoinServerScreen.Online = True Then
            Core.ServersManager.PlayerManager.UpdatePlayers()
        End If

        ' Call Update and UpdateEntity methods of all entities:
        UpdateEntities()
    End Sub

    ''' <summary>
    ''' Updates all entities on the map and offset map and sorts the enumarations.
    ''' </summary>
    Public Sub UpdateEntities()
        SyncLock EntityReadWriteSync
            For i As Integer = Entities.Count - 1 To 0 Step -1
                Dim entity As Entity = Entities(i)

                If entity.NeedsUpdate Then
                    entity.Update()
                End If

                entity.UpdateEntity()

                If entity.CanBeRemoved Then
                    Entities.RemoveAt(i)
                End If
            Next

            For i As Integer = Floors.Count - 1 To 0 Step -1
                Dim entity As Entity = Floors(i)

                If entity.NeedsUpdate Then
                    entity.Update()
                End If

                entity.UpdateEntity()

                If entity.CanBeRemoved Then
                    Floors.RemoveAt(i)
                End If
            Next

            For i As Integer = Npcs.Count - 1 To 0 Step -1
                Dim entity As NPC = Npcs(i)

                If entity.CanBeRemoved Then
                    Npcs.RemoveAt(i)
                End If
            Next

            For i As Integer = NetworkPlayers.Count - 1 To 0 Step -1
                Dim entity As NetworkPlayer = NetworkPlayers(i)

                If entity.CanBeRemoved Then
                    NetworkPlayers.RemoveAt(i)
                End If
            Next

            For i As Integer = NetworkPokemon.Count - 1 To 0 Step -1
                Dim entity As NetworkPokemon = NetworkPokemon(i)

                If entity.CanBeRemoved Then
                    NetworkPokemon.RemoveAt(i)
                End If
            Next
        End SyncLock
    End Sub

    ''' <summary>
    ''' Sorts the entity enumerations.
    ''' </summary>
    <Obsolete("This function does nothing and will be removed in future version.")>
    Public Sub SortEntities()
    End Sub

    ''' <summary>
    ''' Sorts and updates offset map entities.
    ''' </summary>
    Public Sub UpdateOffsetMap(sender As Object, e As ElapsedEventArgs)
        Me._isUpdatingOffsetMaps = True
        If Core.GameOptions.LoadOffsetMaps > 0 Then
            ' The Update function of entities on offset maps are not getting called.

            If Me._offsetMapUpdateDelay <= 0 Then ' Only when the delay is 0, update.
                Me._offsetMapUpdateDelay = Core.GameOptions.LoadOffsetMaps - 1 'Set the new delay

                SyncLock EntityReadWriteSync
                    For i As Integer = OffsetmapEntities.Count - 1 To 0 Step -1
                        Dim entity As Entity = OffsetmapEntities(i)

                        If entity.NeedsUpdate Then
                            entity.Update()
                        End If

                        entity.UpdateEntity()

                        If entity.CanBeRemoved Then
                            OffsetmapEntities.RemoveAt(i)
                        End If
                    Next

                    For i As Integer = OffsetmapFloors.Count - 1 To 0 Step -1
                        Dim entity As Entity = OffsetmapFloors(i)

                        If entity.NeedsUpdate Then
                            entity.Update()
                        End If

                        entity.UpdateEntity()

                        If entity.CanBeRemoved Then
                            OffsetmapFloors.RemoveAt(i)
                        End If
                    Next
                End SyncLock
            Else
                Me._offsetMapUpdateDelay -= 1
            End If
        End If
        Me._isUpdatingOffsetMaps = False
    End Sub

    ''' <summary>
    ''' Sorts the entity enumerations.
    ''' </summary>
    <Obsolete("This function does nothing and will be removed in future version.")>
    Public Sub SortOffsetmapEntities()
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
        If WarpData.DoWarpInNextTick = True Then ' If a warp event got scheduled.
            ' Disable wild Pokémon:
            Me.WildPokemonFloor = False
            Me.PokemonEncounterData.EncounteredPokemon = False

            ' Set the Surfing flag for the next map:
            Core.Player.startSurfing = Surfing

            ' Change the player position:
            Screen.Camera.Position = WarpData.WarpPosition

            Dim tempProperties As String = Me.CanDig.ToString() & "," & Me.CanFly.ToString() ' Store properties to determine if the "enter" sound should be played.

            ' Store skin values:
            Dim usingGameJoltTexture As Boolean = OwnPlayer.UsingGameJoltTexture
            Core.Player.Skin = OwnPlayer.SkinName

            ' Load the new level:
            Dim params As New List(Of Object)
            params.AddRange({WarpData.WarpDestination, False, New Vector3(0, 0, 0), 0, New List(Of String)})

            World = New World(0, 0)

            Me.StopOffsetMapUpdate()
            Dim levelLoader As New LevelLoader()
            levelLoader.LoadLevel(params.ToArray())

            Core.Player.AddVisitedMap(Me.LevelFile) ' Add new map to visited maps list.
            UsedStrength = False ' Disable Strength usuage upon map switch.
            Me.Surfing = Core.Player.startSurfing ' Set the Surfing property after map switch.

            ' Create player and Pokémon entities:
            OwnPlayer = New OwnPlayer(0, 0, 0, {TextureManager.DefaultTexture}, Core.Player.Skin, 0, 0, "", "Gold", 0)
            OwnPlayer.SetTexture(Core.Player.Skin, usingGameJoltTexture)

            OverworldPokemon = New OverworldPokemon(Screen.Camera.Position.X, Screen.Camera.Position.Y, Screen.Camera.Position.Z + 1)
            OverworldPokemon.Visible = False
            OverworldPokemon.warped = True

            AddEntity(OwnPlayer)
            AddEntity(OverworldPokemon)

            ' Set Ride skin, if needed:
            If Riding = True And CanRide() = False Then
                Riding = False
                OwnPlayer.SetTexture(Core.Player.TempRideSkin, True)
                Core.Player.Skin = Core.Player.TempRideSkin
            End If

            ' If any turns after the warp are defined, apply them:
            Screen.Camera.InstantTurn(WarpData.WarpRotations)

            ' Make the RouteSign appear:
            RouteSign.Setup(MapName)

            ' Play the correct music track:
            If IsRadioOn = True AndAlso GameJolt.PokegearScreen.StationCanPlay(Me.SelectedRadioStation) = True Then
                MusicManager.Play(SelectedRadioStation.Music, True)
            Else
                IsRadioOn = False
                If Me.Surfing = True Then
                    MusicManager.Play("surf", True)
                Else
                    If Me.Riding = True Then
                        MusicManager.Play("ride", True)
                    Else
                        MusicManager.Play(MusicLoop, True)
                    End If
                End If
            End If

            ' Initialize the world with newly loaded environment variables:
            World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

            ' If this map is on the restplaces list, set the player's last restplace to this map:
            Dim restplaces As List(Of String) = System.IO.File.ReadAllLines(GameModeManager.GetMapPath("restplaces.dat")).ToList()

            For Each line As String In restplaces
                Dim place As String = line.GetSplit(0, "|")
                If place = LevelFile Then
                    Core.Player.LastRestPlace = place
                    Core.Player.LastRestPlacePosition = line.GetSplit(1, "|")
                End If
            Next

            ' If the warp happened through a warp block, make the player walk one step forward after switching to the new map:
            If WarpData.IsWarpBlock = True Then
                Screen.Camera.StopMovement()
                Screen.Camera.Move(1.0F)
            End If

            ' Because of the map change, Roaming Pokémon are moving to their next location on the world map:
            RoamingPokemon.ShiftRoamingPokemon(-1)

            ' Check if the enter sound should be played by checking if CanDig or CanFly properties are different from the last map:
            If tempProperties <> Me.CanDig.ToString() & "," & Me.CanFly.ToString() Then
                SoundManager.PlaySound("enter", False)
            ElseIf tempProperties = "True,False" And Me.CanDig = True And Me.CanFly = False Then
                SoundManager.PlaySound("enter", False)
            ElseIf tempProperties = "False,False" And Me.CanDig = False And Me.CanFly = False Then
                SoundManager.PlaySound("enter", False)
            End If

            ' Unlock the yaw on the camera:
            CType(Screen.Camera, OverworldCamera).YawLocked = False
            NetworkPlayer.ScreenRegionChanged()

            ' If a warp occured, update the camera:
            Screen.Camera.Update()

            ' Disable the warp check:
            Me.WarpData.DoWarpInNextTick = False
            WarpData.IsWarpBlock = False

            If Core.ServersManager.ServerConnection.Connected = True Then
                ' Update network players:
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
    ''' <param name="id">The ID of the NPC to return from the level.</param>
    ''' <returns>Returns either a matching NPC or Nothing.</returns>
    Public Function GetNPC(ByVal id As Integer) As NPC
        SyncLock EntityReadWriteSync
            For Each npc As NPC In Npcs
                If npc.NPCID = id Then
                    Return npc
                End If
            Next

            Return Nothing
        End SyncLock
    End Function

    ''' <summary>
    ''' Returns an NPC based on the entity ID.
    ''' </summary>
    Public Function GetEntity(id As Integer) As Entity
        If id = -1 Then
            Throw New Exception("-1 is the default value for NOT having an ID, therefore is not a valid ID.")
        End If

        SyncLock EntityReadWriteSync
            For Each entity As Entity In Entities
                If entity.ID = id Then
                    Return entity
                End If
            Next
        End SyncLock

        Return Nothing
    End Function

    ''' <summary>
    ''' Checks all NPCs on the map for if the player is in their line of sight.
    ''' </summary>
    Public Sub CheckTrainerSights()
        SyncLock EntityReadWriteSync
            For Each npc As NPC In Npcs
                If npc.IsTrainer Then
                    npc.CheckInSight()
                End If
            Next
        End SyncLock
    End Sub

    ''' <summary>
    ''' Determines whether the player can use Ride on this map.
    ''' </summary>
    Public Function CanRide() As Boolean
        If GameController.IS_DEBUG_ACTIVE = True OrElse Core.Player.SandBoxMode = True Then 'Always true for Sandboxmode and Debug mode.
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
        If Screen.Level.CanDig = False AndAlso Screen.Level.CanFly = False Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Whether the player can move based on the entity around him.
    ''' </summary>
    Public Function CanMove() As Boolean
        SyncLock EntityReadWriteSync
            For Each entity As Entity In Entities
                If entity.Position.X = Screen.Camera.Position.X AndAlso entity.Position.Z = Screen.Camera.Position.Z AndAlso CInt(entity.Position.Y) = CInt(Screen.Camera.Position.Y) Then
                    Return entity.LetPlayerMove()
                End If
            Next

            Return True
        End SyncLock
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        RemoveHandler _offsetTimer.Elapsed, AddressOf UpdateOffsetMap
        _offsetTimer.Dispose()
    End Sub
End Class