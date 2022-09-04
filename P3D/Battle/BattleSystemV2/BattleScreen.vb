﻿Namespace BattleSystem

    Public Class BattleScreen

        Inherits Screen

        'Used for after fainting switching
        Public OwnFaint As Boolean = False
        Public OppFaint As Boolean = False

        'Used for moves like U-turn (self-switch)
        Public OwnSelfSwitch As Boolean = False
        Public OppSelfSwitch As Boolean = False

        'Used for lead picking in PvP Battles
        Public Shared OwnLeadIndex As Integer = 0
        Public Shared OppLeadIndex As Integer = 0

#Region "BattleValues"

        Public IsChoiced As Boolean = False
        Public ClearMainMenuTime As Boolean = False
        Public ClearMoveMenuTime As Boolean = False
        Public Shared CanCatch As Boolean = True
        Public Shared CanRun As Boolean = True
        Public Shared CanBlackout As Boolean = True
        Public Shared CanReceiveEXP As Boolean = True
        Public Shared RoamingBattle As Boolean = False
        Public Shared RoamingPokemonStorage As RoamingPokemon = Nothing
        Public Shared CanUseItems As Boolean = True
        Public Shared DiveBattle As Boolean = False
        Public Shared TempPokeFile As String = ""
        Public Shared IsInverseBattle As Boolean = False
        Public Shared CustomBattleMusic As String = ""

#End Region

        Public Enum BattleModes
            Standard
            Safari
            BugContest
            PVP
        End Enum

        Public Battle As Battle
        Public FieldEffects As FieldEffects
        Public SavedOverworld As OverworldStorage
        Public BattleMenu As BattleMenu
        Public BattleQuery As New List(Of QueryObject)

        'Remove when new system gets put in place:
        Public OwnPokemon As Pokemon
        Public OppPokemon As Pokemon

        Public IsMegaEvolvingOwn As Boolean = False
        Public IsMegaEvolvingOpp As Boolean = False

        Public OppPokemonNPC As NPC
        Public OwnPokemonNPC As NPC
        Public OwnTrainerNPC As NPC
        Public OppTrainerNPC As NPC

        Public OwnPokemonModel As Entity
        Public OppPokemonModel As Entity

        Public OwnPokemonIndex As Integer = 0
        Public OppPokemonIndex As Integer = 0

        Public ParticipatedPokemon As New List(Of Integer)

        'New multi pokemon system:
        Public PokemonOnSide As Integer = 1 'How many Pokémon are present on one side of the field.
        Public Profiles As New List(Of PokemonProfile) 'The collection of Pokémon Profiles representing the Pokémon on the field.

        'Battle settings:
        Public IsTrainerBattle As Boolean = False
        Public BattleMode As BattleModes = BattleModes.Standard
        Public PokemonSafariStatus As Integer = 0

        Public WildPokemon As Pokemon
        Public OverworldScreen As Screen
        Public defaultMapType As Integer
        Public Trainer As Trainer

        Public DrawColoredScreen As Boolean = True
        Public ColorOverlay As Color = Color.Black

        Public BattleMapOffset As New Vector3(0)

        Public Overrides Function GetScreenStatus() As String
            Dim pokemonString As String = "OwnPokemon=OWNEMPTY" & Environment.NewLine &
                "OppPokemon=OPPEMPTY"

            If Not Me.OwnPokemon Is Nothing Then
                pokemonString = pokemonString.Replace("OWNEMPTY", Me.OwnPokemon.GetSaveData())
            End If
            If Not Me.OppPokemon Is Nothing Then
                pokemonString = pokemonString.Replace("OPPEMPTY", Me.OppPokemon.GetSaveData())
            End If

            Dim values As String = "Values=; CanCatch=" & CanCatch.ToString() & "; CanRun=" & CanRun.ToString() & "; CanBlackout=" & CanBlackout.ToString() &
                "; CanReceiveEXP=" & CanReceiveEXP.ToString() & "; RoamingBattle=" & RoamingBattle.ToString() & "; CanUseItems=" & CanUseItems.ToString() &
                "; DiveBattle=" & DiveBattle.ToString() & "; TempPokeFile=" & TempPokeFile & "; IsInverseBattle=" & IsInverseBattle.ToString()

            Dim s As String = "BattleMode=" & Me.BattleMode.ToString() & Environment.NewLine &
                "IsTrainerBattle=" & Me.IsTrainerBattle.ToString() & Environment.NewLine &
                "IsPVPBattle=" & Me.IsPVPBattle.ToString() & Environment.NewLine &
                "LoadedBattleMap=" & Level.LevelFile & Environment.NewLine &
                pokemonString & Environment.NewLine &
                values & Environment.NewLine &
                "IsRemoteBattle=" & Me.IsRemoteBattle.ToString() & Environment.NewLine &
                "IsHost=" & Me.IsHost.ToString() & Environment.NewLine &
                "MenuVisible=" & BattleMenu.Visible.ToString()

            Return s
        End Function

        Public Sub New(ByVal WildPokemon As Pokemon, ByVal OverworldScreen As Screen, ByVal defaultMapType As Integer)
            Me.WildPokemon = WildPokemon
            Me.OverworldScreen = OverworldScreen
            Me.defaultMapType = defaultMapType
            Me.IsTrainerBattle = False
            Me.MouseVisible = False
            Me.PVPGameJoltID = ""
            'Reset variable when new battle starts
            BattleSystem.Battle.Caught = False
        End Sub

        Public Sub New(ByVal Trainer As Trainer, ByVal OverworldScreen As Screen, ByVal defaultMapType As Integer)
            Me.Trainer = Trainer
            Me.OverworldScreen = OverworldScreen
            Me.defaultMapType = defaultMapType
            Me.IsTrainerBattle = True
            Me.MouseVisible = False
            Me.PVPGameJoltID = ""
        End Sub

#Region "Initialize"

        Private Sub InitializeScreen()
            Me.Identification = Identifications.BattleScreen

            Me.CanBePaused = True
            Me.MouseVisible = True
            Me.CanChat = True
            Me.CanDrawDebug = True
            Me.CanMuteAudio = True
            Me.CanTakeScreenshot = True

            Screen.TextBox.Showing = False
            Screen.PokemonImageView.Showing = False
            Screen.ImageView.Showing = False
            Screen.ChooseBox.Showing = False

            Effect = New BasicEffect(Core.GraphicsDevice)
            Effect.FogEnabled = True
            SkyDome = New SkyDome()
            Camera = New BattleCamera()

            Battle = New Battle()
            FieldEffects = New FieldEffects()

            Level = New Level()
            LoadBattleMap()

            If Core.Player.Badges.Count > 0 Then 'Only have weather effects carry over from the Overworld if the player has at least one badge.
                FieldEffects.Weather = BattleWeather.GetBattleWeather(SavedOverworld.Level.World.CurrentMapWeather)
            End If

            Me.UpdateFadeIn = True

            ReceivedInput = ""
            ReceivedQuery = ""

            Me.BattleMenu = New BattleMenu()
            BattleMenu.Reset()
        End Sub

        Public Sub InitializeWild(ByVal WildPokemon As Pokemon, ByVal OverworldScreen As Screen, ByVal defaultMapType As Integer)
            SavedOverworld = New OverworldStorage()

            SavedOverworld.OverworldScreen = OverworldScreen
            SavedOverworld.Camera = Screen.Camera
            SavedOverworld.Level = Screen.Level
            SavedOverworld.Effect = Screen.Effect
            SavedOverworld.SkyDome = Screen.SkyDome

            InitializeScreen()

            PlayerStatistics.Track("Wild battles", 1)

            Me.defaultMapType = defaultMapType

            Me.OppPokemon = WildPokemon

            If Core.Player.Pokemons.Count = 0 Then
                Dim p1 As Pokemon = Pokemon.GetPokemonByID(247)
                p1.Generate(15, True)
                Core.Player.Pokemons.Add(p1)
            End If

            Dim meIndex As Integer = 0
            For i = 0 To Core.Player.Pokemons.Count - 1
                If Core.Player.Pokemons(i).IsEgg() = False And Core.Player.Pokemons(i).HP > 0 And Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted Then
                    meIndex = i
                    Exit For
                End If
            Next
            Me.OwnPokemon = Core.Player.Pokemons(meIndex)
            OwnPokemonIndex = meIndex

            Me.IsTrainerBattle = False
            Me.ParticipatedPokemon.Add(meIndex)

            Dim ownShiny As String = "N"
            If OwnPokemon.IsShiny = True Then
                ownShiny = "S"
            End If

            Dim oppShiny As String = "N"
            If OppPokemon.IsShiny = True Then
                oppShiny = "S"
            End If

            Dim ownModel As String = GetModelName(True)
            Dim oppModel As String = GetModelName(False)

            If ownModel = "" Then
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, WildPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}, 1), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 13) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Models\Pokemon\Bulbasaur\Normal"), WallBlock)
            Else
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, WildPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 13) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing, 1,,, ownModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OwnPokemonNPC)
            Screen.Level.Entities.Add(OwnPokemonModel)

            If oppModel = "" Then
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(WildPokemon), 1, WildPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 13) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Models\Pokemon\Bulbasaur\Normal"), WallBlock)
            Else
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(WildPokemon), 1, WildPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 13) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, oppModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OppPokemonNPC)
            Screen.Level.Entities.Add(OppPokemonModel)

            Dim ownSkin As String = Core.Player.Skin
            If SavedOverworld.Level.Surfing = True Then
                ownSkin = Core.Player.TempSurfSkin
            End If
            If SavedOverworld.Level.Riding = True Then
                ownSkin = Core.Player.TempRideSkin
            End If

            OwnTrainerNPC = CType(Entity.GetNewEntity("NPC", New Vector3(10, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 0, "", "", New Vector3(0), {ownSkin, 3, "Player", 2, False, "Still", New List(Of Rectangle)}), NPC)
            Screen.Level.Entities.Add(OwnTrainerNPC)

            Dim cq As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 5)
            cq.PassThis = True

            Dim q As CameraQueryObject = New CameraQueryObject(New Vector3(13, 0, 15), New Vector3(21, 0, 15), 0.05F, 0.05F, -0.8F, 1.4F, 0.0F, 0.0F, 0.016F, 0.016F)
            q.PassThis = True

            Dim q1 As New PlaySoundQueryObject(OppPokemon.Number.ToString(), True, 5.0F)
            If OppPokemon.IsShiny = True Then
                q1 = New PlaySoundQueryObject("Battle\shiny", False, 5.0F)
            End If

            Dim q2 As TextQueryObject = New TextQueryObject("Wild " & OppPokemon.GetDisplayName() & " appeared!")

            Dim q22 As CameraQueryObject = New CameraQueryObject(New Vector3(14, 0, 15), New Vector3(13, 0, 15), 0.05F, 0.05F, MathHelper.PiOver2, -0.8F, 0.0F, 0.0F, 0.05F, 0.05F)

            Dim q3 As CameraQueryObject = New CameraQueryObject(New Vector3(14, 0, 11), New Vector3(14, 0, 15), 0.01F, 0.01F, MathHelper.PiOver2, MathHelper.PiOver2, 0.0F, 0.0F)
            q3.PassThis = True

            Dim q31 As New PlaySoundQueryObject(OwnPokemon.Number.ToString(), True, 3.0F)
            Dim q4 As TextQueryObject = New TextQueryObject("Go, " & Me.OwnPokemon.GetDisplayName() & "!")

            Me.BattleQuery.AddRange({cq, q1, q, q2, q22, q3, q31, q4})

            Dim q5 As ToggleMenuQueryObject = New ToggleMenuQueryObject(Me.BattleMenu.Visible)

            Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
            Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)

            cq2.PassThis = True

            Battle.SwitchInOwn(Me, meIndex, True, -1)
            Battle.SwitchInOpp(Me, True, 0)

            Me.BattleQuery.AddRange({cq1, q5, cq2})

            For i = 0 To 99
                InsertCasualCameramove()
            Next

            Me.BattleMode = BattleModes.Standard
            BattleMenu.Reset()
            Me.DownloadOnlineSprites()
        End Sub

        Public Sub InitializeTrainer(ByVal Trainer As Trainer, ByVal OverworldScreen As Screen, ByVal defaultMapType As Integer)
            SavedOverworld = New OverworldStorage()

            SavedOverworld.OverworldScreen = OverworldScreen
            SavedOverworld.Camera = Screen.Camera
            SavedOverworld.Level = Screen.Level
            SavedOverworld.Effect = Screen.Effect
            SavedOverworld.SkyDome = Screen.SkyDome

            InitializeScreen()

            If IsPVPBattle = False And IsRemoteBattle = False Then
                PlayerStatistics.Track("Trainer battles", 1)
            End If

            Me.defaultMapType = defaultMapType

            Me.OppPokemon = Trainer.Pokemons(0)

            If Core.Player.Pokemons.Count = 0 Then
                Dim p1 As Pokemon = Pokemon.GetPokemonByID(247)
                p1.Generate(15, True)
                Core.Player.Pokemons.Add(p1)
            End If

            Dim meIndex As Integer = 0
            For i = 0 To Core.Player.Pokemons.Count - 1
                If Core.Player.Pokemons(i).IsEgg() = False And Core.Player.Pokemons(i).HP > 0 And Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted Then
                    meIndex = i
                    Exit For
                End If
            Next
            Me.OwnPokemon = Core.Player.Pokemons(meIndex)
            OwnPokemonIndex = meIndex
            If IsPVPBattle Then
                OwnPokemon = Core.Player.Pokemons(OwnLeadIndex)
                OwnPokemonIndex = OwnLeadIndex
                OppPokemon = Trainer.Pokemons(OppLeadIndex)
                OppPokemonIndex = OppLeadIndex
            End If

            Me.IsTrainerBattle = True
            Me.ParticipatedPokemon.Add(meIndex)

            Dim ownShiny As String = "N"
            If OwnPokemon.IsShiny = True Then
                ownShiny = "S"
            End If

            Dim oppShiny As String = "N"
            If OppPokemon.IsShiny = True Then
                oppShiny = "S"
            End If

            Dim ownModel As String = GetModelName(True)
            Dim oppModel As String = GetModelName(False)

            Dim InitiallyVisibleOwn As Integer = 1
            If IsPVPBattle = True AndAlso Core.Player.ShowBattleAnimations <> 0 Then
                InitiallyVisibleOwn = 0
            End If

            If ownModel = "" Then
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, OwnPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}, InitiallyVisibleOwn), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Pokemon\Models\Bulbasaur\Normal"), WallBlock)
            Else
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, OwnPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing, InitiallyVisibleOwn,,, ownModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OwnPokemonNPC)
            Screen.Level.Entities.Add(OwnPokemonModel)

            Dim InitiallyVisibleOpp As Integer = 1
            If Core.Player.ShowBattleAnimations <> 0 Then
                InitiallyVisibleOpp = 0
            End If

            If oppModel = "" Then
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OppPokemon), 1, OppPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}, InitiallyVisibleOpp), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Models\Bulbasaur\Normal"), WallBlock)
            Else
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OppPokemon), 1, OppPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing, InitiallyVisibleOpp,,, oppModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OppPokemonNPC)
            Screen.Level.Entities.Add(OppPokemonModel)

            Dim ownSkin As String = Core.Player.Skin
            If SavedOverworld.Level.Surfing = True Then
                ownSkin = Core.Player.TempSurfSkin
            End If
            If SavedOverworld.Level.Riding = True Then
                ownSkin = Core.Player.TempRideSkin
            End If

            OwnTrainerNPC = CType(Entity.GetNewEntity("NPC", New Vector3(10, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 0, "", "", New Vector3(0), {ownSkin, 3, "Player", 2, False, "Still", New List(Of Rectangle)}), NPC)
            Screen.Level.Entities.Add(OwnTrainerNPC)

            OppTrainerNPC = CType(Entity.GetNewEntity("NPC", New Vector3(17, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 0, "", "", New Vector3(0), {Trainer.SpriteName, 1, "Player", 3, False, "Still", New List(Of Rectangle)}), NPC)
            Screen.Level.Entities.Add(OppTrainerNPC)

            Dim cq As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 5)
            cq.PassThis = True

            Dim q As CameraQueryObject = New CameraQueryObject(New Vector3(13, 0, 15), New Vector3(21, 0, 15), 0.05F, 0.05F, -0.8F, 1.4F, 0.0F, 0.0F, 0.016F, 0.016F)
            q.PassThis = True

            Dim q1 As TextQueryObject = New TextQueryObject(Trainer.Name & " " & "wants to battle!")
            Dim q11 As TextQueryObject = New TextQueryObject(Trainer.Name & ": """ & "Go," & " " & OppPokemon.GetDisplayName() & "!""")

            ' Ball is thrown
            Dim BallThrowOpp As AnimationQueryObject = New AnimationQueryObject(OppPokemonNPC, False, OppPokemonModel)

            If Core.Player.ShowBattleAnimations <> 0 Then
                BallThrowOpp.AnimationPlaySound("Battle\Pokeball\Throw", 0, 0)
                BallThrowOpp.AnimationMove(Nothing, False, 0, 0.5, 0, 0.5, False, False, 2, 0,,, 3)
                Dim BallThrowEntity As Entity = BallThrowOpp.SpawnEntity(New Vector3(2, -0.15, 0), Me.OppPokemon.CatchBall.Texture, New Vector3(0.3F), 1.0F)
                BallThrowOpp.AnimationMove(BallThrowEntity, True, 0, 0.35, 0, 0.1, False, True, 0F, 0.5F,, 0.3,, 0.025F)

                ' Ball Opens
                BallThrowOpp.AnimationPlaySound("Battle\Pokeball\Open", 3, 0)
                Dim SmokeSpawnedOpp As Integer = 0
                Do
                    Dim SmokeDestination = New Vector3(CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10))

                    Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                    Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                    Dim SmokeSpeed = CSng(Random.Next(1, 3) / 20.0F)

                    Dim SmokeEntity As Entity = BallThrowOpp.SpawnEntity(Nothing, SmokeTexture, SmokeScale, 1.0F, 3)
                    BallThrowOpp.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 3.0F, 0.0F)

                    Threading.Interlocked.Increment(SmokeSpawnedOpp)
                Loop While SmokeSpawnedOpp <= 38

                ' Pokemon appears
                BallThrowOpp.AnimationFade(Nothing, False, 1, True, 1, 3, 0)
                BallThrowOpp.AnimationPlaySound(CStr(Me.OppPokemon.Number), 4, 0,, True)
            Else
                BallThrowOpp.AnimationPlaySound(CStr(Me.OppPokemon.Number), 0, 0,, True)
            End If


            '  Pokémon falls down
            If Core.Player.ShowBattleAnimations <> 0 Then
                '  Pokémon falls down
                BallThrowOpp.AnimationMove(Nothing, False, 0, 0, 0, 0.05F, False, False, 4, 0,,, 3)
            End If

            Dim q2 As CameraQueryObject = New CameraQueryObject(New Vector3(14, 0, 15), New Vector3(13, 0, 15), 0.05F, 0.05F, MathHelper.PiOver2, -0.8F, 0.0F, 0.0F, 0.05F, 0.05F)

            Dim q3 As CameraQueryObject = New CameraQueryObject(New Vector3(14, 0, 11), New Vector3(14, 0, 15), 0.01F, 0.01F, MathHelper.PiOver2, MathHelper.PiOver2, 0.0F, 0.0F)
            q3.PassThis = True
            Dim q31 As New PlaySoundQueryObject(OwnPokemon.Number.ToString(), True, 3.0F)
            Dim q4 As TextQueryObject = New TextQueryObject("Go," & " " & Me.OwnPokemon.GetDisplayName() & "!")


            Me.BattleQuery.AddRange({cq, q, q1, q11, BallThrowOpp, q2, q3, q31, q4})

            If IsPVPBattle = True AndAlso Core.Player.ShowBattleAnimations <> 0 Then
                ' Ball is thrown
                Dim BallThrowOwn As AnimationQueryObject = New AnimationQueryObject(Me.OwnPokemonNPC, False, Me.OwnPokemonModel)

                BallThrowOwn.AnimationPlaySound("Battle\Pokeball\Throw", 0, 0)
                BallThrowOwn.AnimationMove(Nothing, False, 0, 0.5, 0, 0.5, False, False, 2, 0,,, 3)

                Dim BallThrowEntity As Entity = BallThrowOwn.SpawnEntity(New Vector3(-2, -0.15, 0), Me.OwnPokemon.CatchBall.Texture, New Vector3(0.3F), 1.0F)
                BallThrowOwn.AnimationMove(BallThrowEntity, True, 0, 0.35, 0, 0.1, False, True, 0F, 0.5F,, 0.3,, 0.025F)

                ' Ball Opens
                BallThrowOwn.AnimationPlaySound("Battle\Pokeball\Open", 3, 0)

                Dim SmokeSpawned As Integer = 0
                Do
                    Dim SmokeDestination = New Vector3(CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10))

                    Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                    Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                    Dim SmokeSpeed = CSng(Random.Next(1, 3) / 20.0F)

                    Dim SmokeEntity As Entity = BallThrowOwn.SpawnEntity(Nothing, SmokeTexture, SmokeScale, 1.0F, 3)
                    BallThrowOwn.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 3.0F, 0.0F)

                    Threading.Interlocked.Increment(SmokeSpawned)
                Loop While SmokeSpawned <= 38


                ' Pokemon appears
                BallThrowOwn.AnimationFade(Nothing, False, 1, True, 1, 3, 0)
                BallThrowOwn.AnimationPlaySound(CStr(Me.OwnPokemon.Number), 4, 0,, True)

                '  Pokémon falls down
                BallThrowOwn.AnimationMove(Nothing, False, 0, 0, 0, 0.05F, False, False, 5, 0,,, 3)
                Me.BattleQuery.Add(BallThrowOwn)
            End If

            Dim q5 As ToggleMenuQueryObject = New ToggleMenuQueryObject(Me.BattleMenu.Visible)

            Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
            Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)

            cq2.PassThis = True

            Battle.SwitchInOwn(Me, meIndex, True, OwnPokemonIndex)
            Battle.SwitchInOpp(Me, True, OppPokemonIndex)
            TempPVPBattleQuery.Clear()

            Me.BattleQuery.AddRange({cq1, q5, cq2})

            For i = 0 To 99
                InsertCasualCameramove()
            Next

            If Pokedex.GetEntryType(Core.Player.PokedexData, OppPokemon.Number) = 0 Then
                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, OppPokemon.Number, 1)
            End If

            Me.BattleMode = BattleModes.Standard
            BattleMenu.Reset()
            Me.DownloadOnlineSprites()
        End Sub

        Public Sub InitializeSafari(ByVal WildPokemon As Pokemon, ByVal OverworldScreen As Screen, ByVal defaultMapType As Integer)
            SavedOverworld = New OverworldStorage()

            SavedOverworld.OverworldScreen = OverworldScreen
            SavedOverworld.Camera = Screen.Camera
            SavedOverworld.Level = Screen.Level
            SavedOverworld.Effect = Screen.Effect
            SavedOverworld.SkyDome = Screen.SkyDome

            InitializeScreen()
            FieldEffects.Weather = BattleWeather.WeatherTypes.Clear

            PlayerStatistics.Track("Safari battles", 1)

            Me.defaultMapType = defaultMapType

            Me.OppPokemon = WildPokemon

            If Core.Player.Pokemons.Count = 0 Then
                Dim p1 As Pokemon = Pokemon.GetPokemonByID(247)
                p1.Generate(15, True)
                Core.Player.Pokemons.Add(p1)
            End If

            Dim meIndex As Integer = 0
            For i = 0 To Core.Player.Pokemons.Count - 1
                If Core.Player.Pokemons(i).IsEgg() = False And Core.Player.Pokemons(i).HP > 0 And Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted Then
                    meIndex = i
                    Exit For
                End If
            Next
            Me.OwnPokemon = Core.Player.Pokemons(meIndex)
            OwnPokemonIndex = meIndex

            Me.IsTrainerBattle = False
            Me.ParticipatedPokemon.Add(meIndex)

            Dim ownShiny As String = "N"
            If OwnPokemon.IsShiny = True Then
                ownShiny = "S"
            End If

            Dim oppShiny As String = "N"
            If OppPokemon.IsShiny = True Then
                oppShiny = "S"
            End If

            Dim ownModel As String = GetModelName(True)
            Dim oppModel As String = GetModelName(False)

            If ownModel = "" Then
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, WildPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Models\Bulbasaur\Normal"), WallBlock)
            Else
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, WildPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, ownModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OwnPokemonNPC)
            Screen.Level.Entities.Add(OwnPokemonModel)

            If oppModel = "" Then
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(WildPokemon), 1, WildPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Models\Bulbasaur\Normal"), WallBlock)
            Else
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(WildPokemon), 1, WildPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, oppModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OppPokemonNPC)
            Screen.Level.Entities.Add(OppPokemonModel)

            Dim ownSkin As String = Core.Player.Skin
            If SavedOverworld.Level.Surfing = True Then
                ownSkin = Core.Player.TempSurfSkin
            End If
            If SavedOverworld.Level.Riding = True Then
                ownSkin = Core.Player.TempRideSkin
            End If

            OwnTrainerNPC = CType(Entity.GetNewEntity("NPC", New Vector3(10, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 0, "", "", New Vector3(0), {ownSkin, 3, "Player", 2, False, "Still", New List(Of Rectangle)}), NPC)
            Screen.Level.Entities.Add(OwnTrainerNPC)

            Dim cq As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 5)
            cq.PassThis = True

            Dim q As CameraQueryObject = New CameraQueryObject(New Vector3(13, 0, 15), New Vector3(21, 0, 15), 0.05F, 0.05F, -0.8F, 1.4F, 0.0F, 0.0F, 0.016F, 0.016F)
            q.PassThis = True

            Dim q1 As New PlaySoundQueryObject(OppPokemon.Number.ToString(), True, 5.0F)
            Dim q2 As TextQueryObject = New TextQueryObject("Wild " & OppPokemon.GetDisplayName() & " appeared!")

            Dim q5 As ToggleMenuQueryObject = New ToggleMenuQueryObject(Me.BattleMenu.Visible)

            Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
            Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)

            cq2.PassThis = True

            Me.BattleQuery.AddRange({cq, q, q1, q2})

            Me.BattleQuery.AddRange({cq1, q5, cq2})

            For i = 0 To 99
                InsertCasualCameramove()
            Next

            Me.BattleMode = BattleModes.Safari
            BattleMenu.Reset()
            Me.DownloadOnlineSprites()
        End Sub

        Public Sub InitializeBugCatch(ByVal WildPokemon As Pokemon, ByVal OverworldScreen As Screen, ByVal defaultMapType As Integer)
            SavedOverworld = New OverworldStorage()

            SavedOverworld.OverworldScreen = OverworldScreen
            SavedOverworld.Camera = Screen.Camera
            SavedOverworld.Level = Screen.Level
            SavedOverworld.Effect = Screen.Effect
            SavedOverworld.SkyDome = Screen.SkyDome

            InitializeScreen()

            PlayerStatistics.Track("Bug-Catching contest battles", 1)

            Me.defaultMapType = defaultMapType

            Me.OppPokemon = WildPokemon

            If Core.Player.Pokemons.Count = 0 Then
                Dim p1 As Pokemon = Pokemon.GetPokemonByID(10)
                p1.Generate(15, True)
                Core.Player.Pokemons.Add(p1)
            End If

            Dim meIndex As Integer = 0
            For i = 0 To Core.Player.Pokemons.Count - 1
                If Core.Player.Pokemons(i).IsEgg() = False And Core.Player.Pokemons(i).HP > 0 And Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted Then
                    meIndex = i
                    Exit For
                End If
            Next
            Me.OwnPokemon = Core.Player.Pokemons(meIndex)
            OwnPokemonIndex = meIndex


            Me.IsTrainerBattle = False
            Me.ParticipatedPokemon.Add(meIndex)

            Dim ownShiny As String = "N"
            If OwnPokemon.IsShiny = True Then
                ownShiny = "S"
            End If

            Dim oppShiny As String = "N"
            If OppPokemon.IsShiny = True Then
                oppShiny = "S"
            End If

            Dim ownModel As String = GetModelName(True)
            Dim oppModel As String = GetModelName(False)

            If ownModel = "" Then
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, OwnPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}, 1), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Pokemon\Models\Bulbasaur\Normal"), WallBlock)
            Else
                OwnPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(12, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(OwnPokemon), 3, OwnPokemon.GetDisplayName(), 0, True, "Still", New List(Of Rectangle)}), NPC)
                OwnPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(12, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 0.5F, 0), New Vector3(OwnPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing, 1,,, ownModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OwnPokemonNPC)
            Screen.Level.Entities.Add(OwnPokemonModel)

            If oppModel = "" Then
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(WildPokemon), 1, WildPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, "Pokemon\Models\Bulbasaur\Normal"), WallBlock)
            Else
                OppPokemonNPC = CType(Entity.GetNewEntity("NPC", New Vector3(15, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", False, New Vector3(1), 0, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(WildPokemon), 1, WildPokemon.GetDisplayName(), 1, True, "Still", New List(Of Rectangle)}), NPC)
                OppPokemonModel = CType(Entity.GetNewEntity("WallBlock", New Vector3(15, -0.5F, 12.5F) + BattleMapOffset, {}, {}, False, New Vector3(0, MathHelper.Pi * 1.5F, 0), New Vector3(OppPokemon.GetModelProperties().Item1), BaseModel.BlockModel, 1, "", True, New Vector3(1), 0, "", "", New Vector3(0), Nothing,,,, oppModel), WallBlock)
            End If

            Screen.Level.Entities.Add(OppPokemonNPC)
            Screen.Level.Entities.Add(OppPokemonModel)

            Dim ownSkin As String = Core.Player.Skin
            If SavedOverworld.Level.Surfing = True Then
                ownSkin = Core.Player.TempSurfSkin
            End If
            If SavedOverworld.Level.Riding = True Then
                ownSkin = Core.Player.TempRideSkin
            End If

            OwnTrainerNPC = CType(Entity.GetNewEntity("NPC", New Vector3(10, 0, 13) + BattleMapOffset, {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 0, "", "", New Vector3(0), {ownSkin, 3, "Player", 2, False, "Still", New List(Of Rectangle)}), NPC)
            Screen.Level.Entities.Add(OwnTrainerNPC)

            Dim cq As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 5)
            cq.PassThis = True

            Dim q As CameraQueryObject = New CameraQueryObject(New Vector3(13, 0, 15), New Vector3(21, 0, 15), 0.05F, 0.05F, -0.8F, 1.4F, 0.0F, 0.0F, 0.016F, 0.016F)
            q.PassThis = True

            Dim q1 As New PlaySoundQueryObject(OppPokemon.Number.ToString(), True, 5.0F)
            Dim q2 As TextQueryObject = New TextQueryObject("Wild " & OppPokemon.GetDisplayName() & " appeared!")

            Dim q22 As CameraQueryObject = New CameraQueryObject(New Vector3(14, 0, 15), New Vector3(13, 0, 15), 0.05F, 0.05F, MathHelper.PiOver2, -0.8F, 0.0F, 0.0F, 0.05F, 0.05F)

            Dim q3 As CameraQueryObject = New CameraQueryObject(New Vector3(14, 0, 11), New Vector3(14, 0, 15), 0.01F, 0.01F, MathHelper.PiOver2, MathHelper.PiOver2, 0.0F, 0.0F)
            q3.PassThis = True

            Dim q31 As New PlaySoundQueryObject(OwnPokemon.Number.ToString(), True, 3.0F)
            Dim q4 As TextQueryObject = New TextQueryObject("Go, " & Me.OwnPokemon.GetDisplayName() & "!")

            Me.BattleQuery.AddRange({cq, q1, q, q2, q22, q3, q4})

            Dim q5 As ToggleMenuQueryObject = New ToggleMenuQueryObject(Me.BattleMenu.Visible)

            Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
            Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)

            cq2.PassThis = True

            Battle.SwitchInOwn(Me, meIndex, True, -1)
            Battle.SwitchInOpp(Me, True, 0)

            Me.BattleQuery.AddRange({cq1, q5, cq2})

            For i = 0 To 99
                InsertCasualCameramove()
            Next

            Me.BattleMode = BattleModes.BugContest
            BattleMenu.Reset()
            Me.DownloadOnlineSprites()
        End Sub

        Public Sub InitializePVP(ByVal PVPTrainer As Trainer, ByVal OverworldScreen As Screen)
            Me.IsPVPBattle = True
            Me.BattleMode = BattleModes.PVP
            BattleScreen.CanReceiveEXP = False
            BattleScreen.CanBlackout = False
            BattleScreen.CanRun = False
            BattleScreen.CanUseItems = False
            PVPLobbyScreen.StoppedBattle = True
            PVPLobbyScreen.DisconnectMessage = "The battle has ended." & Environment.NewLine & Environment.NewLine & "Press any key to exit."
            PVPLobbyScreen.ScreenState = PVPLobbyScreen.ScreenStates.Stopped
            InitializeTrainer(PVPTrainer, OverworldScreen, 0)
            Me.CanBePaused = False
        End Sub

        Public Sub LoadBattleMap()
            Dim levelfile As String = SavedOverworld.Level.LevelFile
            Dim cRegion As String = SavedOverworld.Level.CurrentRegion.Split(CChar(","))(0)
            Dim battleMapData() As String = SavedOverworld.Level.BattleMapData.Split(CChar(","))
            Dim surfingBattleMapData() As String = SavedOverworld.Level.SurfingBattleMapData.Split(CChar(","))

            If Me.IsPVPBattle = True Then
                levelfile = "pvp.dat"
            Else
                If SavedOverworld.Level.BattleMapData <> "" Then
                    Select Case battleMapData.Length
                        Case 1
                            levelfile = battleMapData(0)
                        Case 3
                            BattleMapOffset = New Vector3(CSng(battleMapData(0).Replace(".", GameController.DecSeparator)), CSng(battleMapData(1).Replace(".", GameController.DecSeparator)), CSng(battleMapData(2).Replace(".", GameController.DecSeparator)))
                        Case 4
                            levelfile = battleMapData(0)
                            BattleMapOffset = New Vector3(CSng(battleMapData(1).Replace(".", GameController.DecSeparator)), CSng(battleMapData(2).Replace(".", GameController.DecSeparator)), CSng(battleMapData(3).Replace(".", GameController.DecSeparator)))
                    End Select
                End If

                If File.Exists(GameController.GamePath & "\Content\Data\maps\battle\" & levelfile) = False And File.Exists(GameController.GamePath & GameModeManager.ActiveGameMode.MapPath & "battle\" & levelfile) = False Then
                    Select Case Me.defaultMapType
                        Case 0
                            levelfile = cRegion & "0.dat"
                        Case 2
                            levelfile = cRegion & "1.dat"
                        Case Else
                            levelfile = cRegion & "0.dat"
                    End Select
                    BattleMapOffset = New Vector3(0)
                End If

                If SavedOverworld.Level.Surfing = True Then
                    If SavedOverworld.Level.SurfingBattleMapData <> "" Then
                        Select Case surfingBattleMapData.Length
                            Case 1
                                levelfile = surfingBattleMapData(0)
                            Case 4
                                levelfile = surfingBattleMapData(0)
                                BattleMapOffset = New Vector3(CSng(surfingBattleMapData(1).Replace(".", GameController.DecSeparator)), CSng(surfingBattleMapData(2).Replace(".", GameController.DecSeparator)), CSng(surfingBattleMapData(3).Replace(".", GameController.DecSeparator)))
                            Case Else
                                levelfile = cRegion & "1.dat"
                                BattleMapOffset = New Vector3(0)
                        End Select
                        DiveBattle = True
                    Else
                        levelfile = cRegion & "1.dat"
                        DiveBattle = True
                        BattleMapOffset = New Vector3(0)
                    End If
                End If
            End If

            If File.Exists(GameController.GamePath & "\Content\Data\maps\battle\" & levelfile) = False And File.Exists(GameController.GamePath & GameModeManager.ActiveGameMode.MapPath & "battle\" & levelfile) = False Then
                Select Case Me.defaultMapType
                    Case 0
                        levelfile = "battle0.dat"
                    Case 2
                        levelfile = "battle1.dat"
                    Case Else
                        levelfile = "battle0.dat"
                End Select
                BattleMapOffset = New Vector3(0)
            End If

            Level.Load("battle\" & levelfile)
            Level.MapName = SavedOverworld.Level.MapName
        End Sub

#End Region

        Public Overrides Sub Draw()
            SkyDome.Draw(45.0F)

            Dim ForegroundEntities As New List(Of Entity)
            For Each e As Entity In Level.Entities
                If e Is OwnPokemonNPC Then
                    ForegroundEntities.Add(e)
                End If
                If e Is OppPokemonNPC Then
                    ForegroundEntities.Add(e)
                End If
                If e Is OwnTrainerNPC Then
                    ForegroundEntities.Add(e)
                End If
                If e Is OppTrainerNPC Then
                    ForegroundEntities.Add(e)
                End If
                If e Is OwnPokemonModel Then
                    ForegroundEntities.Add(e)
                End If
                If e Is OppPokemonModel Then
                    ForegroundEntities.Add(e)
                End If
            Next
            If ForegroundEntities.Count > 0 Then
                ForegroundEntities = (From f In ForegroundEntities Order By f.CameraDistance Descending).ToList()
            End If
            Level.Draw()

            World.DrawWeather(Screen.Level.World.CurrentMapWeather)

            If HasToWaitPVP() = True Then
                Canvas.DrawRectangle(New Rectangle(0, CInt(Core.windowSize.Height / 2 - 60), CInt(Core.windowSize.Width), 120), New Color(0, 0, 0, 150))
                Dim t As String = "Waiting for the other player  "
                Core.SpriteBatch.DrawString(FontManager.MainFont, t.Remove(t.Length - 2, 2) & LoadingDots.Dots, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), CSng(Core.windowSize.Height / 2 - FontManager.MainFont.MeasureString(t).Y / 2)), Color.White)
            Else
                If BattleMenu.Visible = True Then
                    BattleMenu.Draw(Me)
                End If
            End If

            Dim ForegroundAnimationList As New List(Of AnimationQueryObject)
            Dim BackgroundAnimationList As New List(Of AnimationQueryObject)
            If BattleQuery.Count > 0 Then
                Dim cIndex As Integer = 0
                Dim cQuery As New List(Of QueryObject)
nextIndex:
                If BattleQuery.Count > cIndex Then
                    Dim cQueryObject As QueryObject = BattleQuery(cIndex)
                    If cQueryObject.QueryType = QueryObject.QueryTypes.MoveAnimation Then
                        If CType(cQueryObject, AnimationQueryObject).DrawBeforeEntities = True Then
                            BackgroundAnimationList.Add(CType(cQueryObject, AnimationQueryObject))
                            cIndex += 1
                            GoTo nextIndex
                        Else
                            ForegroundAnimationList.Add(CType(cQueryObject, AnimationQueryObject))
                            cIndex += 1
                            GoTo nextIndex
                        End If
                    Else
                        cQuery.Add(cQueryObject)
                    End If


                    If cQueryObject.PassThis = True Then
                        cIndex += 1
                        GoTo nextIndex
                    End If
                End If

                cQuery.Reverse()
                If cQuery.Count > 0 Then
                    For Each cQueryObject As QueryObject In cQuery
                        cQueryObject.Draw(Me)
                    Next
                End If
            End If
            If BackgroundAnimationList.Count > 0 Then
                Dim cIndex As Integer = 0
                Dim cQuery As New List(Of QueryObject)
nextIndexBackground:
                If BackgroundAnimationList.Count > cIndex Then
                    Dim cQueryObject As QueryObject = BackgroundAnimationList(cIndex)
                    cQuery.Add(cQueryObject)

                    If cQueryObject.PassThis = True Then
                        cIndex += 1
                        GoTo nextIndexBackground
                    End If
                End If

                cQuery.Reverse()

                For Each cQueryObject As QueryObject In cQuery
                    cQueryObject.Draw(Me)
                Next

                For i = 0 To ForegroundEntities.Count - 1
                    ForegroundEntities(i).Render()
                    DebugDisplay.MaxVertices += ForegroundEntities(i).VertexCount
                Next
            End If
            If ForegroundAnimationList.Count > 0 Then
                Dim cIndex As Integer = 0
                Dim cQuery As New List(Of QueryObject)
nextIndexForeground:
                If ForegroundAnimationList.Count > cIndex Then
                    Dim cQueryObject As QueryObject = ForegroundAnimationList(cIndex)
                    cQuery.Add(cQueryObject)

                    If cQueryObject.PassThis = True Then
                        cIndex += 1
                        GoTo nextIndexForeground
                    End If
                End If

                cQuery.Reverse()

                For Each cQueryObject As QueryObject In cQuery
                    cQueryObject.Draw(Me)
                Next
            End If
            'Core.SpriteBatch.DrawString(FontManager.MiniFont, "Battle system not final!", New Vector2(0, Core.windowSize.Height - 20), Color.White)

            TextBox.Draw()

            If DrawColoredScreen = True Then
                Canvas.DrawRectangle(Core.windowSize, Me.ColorOverlay)
            End If
        End Sub

        Public Overrides Sub Update()
            If CheckNetworkPlayer() = False Then
                Exit Sub
            End If

            If IsRemoteBattle = True And IsHost = False And SentInput = True And ReceivedQuery <> "" Then
                BattleMenu.Visible = False
            End If
            If IsRemoteBattle = True And IsHost = False Then
                If ReceivedPokemonData = True And ClientWaitForData = True Then
                    ClientWaitForData = False
                    ReceivedPokemonData = False
                    BattleMenu.Reset()
                    ClearMainMenuTime = True
                    ClearMoveMenuTime = True
                    BattleMenu.Update(Me)
                End If
            End If
            If Me.IsHost = False And Me.LockData <> "{}" And ReceivedPokemonData = False And ClientWaitForData = False And IsRemoteBattle = True Then
                Dim lockArgument As String = LockData.Remove(LockData.Length - 1, 1).Remove(0, 1)

                BattleQuery.Clear()
                BattleQuery.Add(FocusBattle())
                BattleQuery.Insert(0, New ToggleMenuQueryObject(True))

                If StringHelper.IsNumeric(lockArgument) = True Then
                    SendClientCommand("MOVE|" & CStr(CInt(lockArgument)))
                Else
                    SendClientCommand("TEXT|" & lockArgument)
                End If
                LockData = "{}"
            End If

            Lighting.UpdateLighting(Screen.Effect)
            Camera.Update()
            Level.Update()

            SkyDome.Update()

            TextBox.Update()
            If TextBox.Showing = False Then
                Dim cIndex As Integer = 0
nextIndex:
                If BattleQuery.Count > cIndex Then
                    Dim cQueryObject As QueryObject = BattleQuery(cIndex)

                    cQueryObject.Update(Me)

                    If cQueryObject.IsReady = True Then
                        BattleQuery.RemoveAt(cIndex)

                        If cQueryObject.PassThis = True Then
                            GoTo nextIndex
                        End If
                    Else
                        If cQueryObject.PassThis = True Then
                            cIndex += 1
                            GoTo nextIndex
                        End If
                    End If
                End If

                If HasToWaitPVP() = False Then
                    If BattleMenu.Visible = True Then
                        BattleMenu.Update(Me)
                    End If
                End If
            End If

            If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                If KeyBoardHandler.KeyPressed(Keys.K) = True Then
                    Battle.Won = True
                    EndBattle(False)
                End If
            End If

            'Update the world:
            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, World.GetWeatherTypeFromWeather(Screen.Level.World.CurrentMapWeather))
        End Sub

#Region "CameraStuffs"

        Private lastCameraSettings As New List(Of Integer)
        Private lastCamera As Integer = 2
        Private cameraSettingCount As Integer = 4

        Public Sub InsertCasualCameramove()
            If lastCameraSettings.Count = cameraSettingCount Then
                lastCameraSettings.Clear()
            End If

            Dim r As Integer = Core.Random.Next(0, cameraSettingCount)
            While lastCameraSettings.Contains(r) = True Or lastCamera = r
                r = Core.Random.Next(0, cameraSettingCount)
            End While
            lastCameraSettings.Add(r)
            lastCamera = r

            Dim q As CameraQueryObject = Nothing

            Select Case r
                Case 0
                    q = New CameraQueryObject(New Vector3(17, 1, 15), New Vector3(9, 1, 15), 0.01F, 0.01F, 1.2F, -1.2F, -0.3F, -0.3F, 0.003F, 0.003F)
                Case 1
                    q = New CameraQueryObject(New Vector3(10.3, 0.5F, 10), New Vector3(17, 0.5F, 10), 0.01F, 0.01F, MathHelper.Pi + 0.5F, MathHelper.Pi - 0.5F, -0.1F, -0.1F, 0.0015F, 0.0015F)
                Case 2
                    q = New CameraQueryObject(New Vector3(14, 0, 11), New Vector3(14, 0, 15), 0.01F, 0.01F, MathHelper.PiOver2, MathHelper.PiOver2, 0.0F, 0.0F)
                Case 3
                    q = New CameraQueryObject(New Vector3(13, 0, 12), New Vector3(17, 0, 12), 0.01F, 0.01F, MathHelper.PiOver2 + 0.4F, MathHelper.PiOver2, 0.0F, 0.0F, 0.001F, 0.001F)
            End Select

            If Not q Is Nothing Then
                BattleQuery.Add(q)
            End If
        End Sub

        Public Function FocusOwnPokemon() As QueryObject
            Dim q As New CameraQueryObject(New Vector3(Me.OwnPokemonNPC.Position.X + 1.0F, Me.OwnPokemonNPC.Position.Y + 0.5F, Me.OwnPokemonNPC.Position.Z + 1.0F) - BattleMapOffset, Screen.Camera.Position, 0.06F, 0.06F, CSng(MathHelper.PiOver4) + 0.05F, Screen.Camera.Yaw, -0.3F, Screen.Camera.Pitch, 0.04F, 0.04F)
            Return q
        End Function

        Public Function FocusOppPokemon() As QueryObject
            Dim q As New CameraQueryObject(New Vector3(Me.OppPokemonNPC.Position.X - 1.0F, Me.OppPokemonNPC.Position.Y + 0.5F, Me.OppPokemonNPC.Position.Z + 1.0F) - BattleMapOffset, Screen.Camera.Position, 0.06F, 0.06F, -CSng(MathHelper.PiOver4) - 0.05F, Screen.Camera.Yaw, -0.3F, Screen.Camera.Pitch, 0.04F, 0.04F)
            Return q
        End Function

        Public Function FocusOwnPlayer() As QueryObject
            Dim q As New CameraQueryObject(New Vector3(11, 0.0F, 13.5F), Screen.Camera.Position, 0.1F, 0.1F, CSng(MathHelper.PiOver4), Screen.Camera.Yaw, -0.1F, Screen.Camera.Pitch, 0.04F, 0.04F)
            Return q
        End Function

        Public Function FocusBattle() As QueryObject
            Dim q As New CameraQueryObject(New Vector3(13.5F, 0.5F, 15.0F), Screen.Camera.Position, 0.1F, 0.1F, 0, Screen.Camera.Yaw, -0.1F, Screen.Camera.Pitch, 0.04F, 0.04F)
            Return q
        End Function

#End Region

        Public Sub EndBattle(ByVal blackout As Boolean)
            'Level.StopOffsetMapUpdate()

            Dim str As String = ""
            'Call the EndBattle function of the abilities and Reverts battle only Pokemon formes.
            For Each p As Pokemon In Core.Player.Pokemons
                str = p.AdditionalData.ToLower()
                Select Case str
                    Case "mega", "mega_x", "mega_y", "primal", "blade"
                        p.AdditionalData = PokemonForms.GetInitialAdditionalData(p)
                        p.ReloadDefinitions()
                        p.CalculateStats()
                        If str <> "blade" Then
                            p.RestoreAbility() 'currently only used for mega evolutions
                        End If
                End Select
                If Not p.Ability Is Nothing Then
                    p.Ability.EndBattle(p)
                End If
            Next

            'Remove fainted Pokémon from player's team if the DeathInsteadOfFaint GameRule is activated.
            If CBool(GameModeManager.GetGameRuleValue("DeathInsteadOfFaint", "0")) = True Then
                For i = 0 To Core.Player.Pokemons.Count - 1
                    If i <= Core.Player.Pokemons.Count - 1 Then
                        If Core.Player.Pokemons(i).HP <= 0 Or Core.Player.Pokemons(i).Status = Pokemon.StatusProblems.Fainted Then
                            Core.Player.Pokemons.RemoveAt(i)
                            i -= 1
                        End If
                    End If
                Next
            End If

            'Shift the Roaming Pokemon.
            If RoamingBattle = True Then
                If FieldEffects.RoamingFled = False AndAlso Battle.Fled = False Then
                    Core.Player.RoamingPokemonData = RoamingPokemon.RemoveRoamingPokemon(RoamingPokemonStorage)
                Else
                    Core.Player.RoamingPokemonData = RoamingPokemon.ReplaceRoamingPokemon(RoamingPokemonStorage)
                End If
                RoamingPokemon.ShiftRoamingPokemon(RoamingPokemonStorage.WorldID)
            End If

            'Reverse this variable temp >
            Battle.Fled = False

            'Add the Pokefile to the visited pokefiles list.
            If IsTrainerBattle = False Then
                If TempPokeFile <> "" Then
                    If Core.Player.PokeFiles.Contains(TempPokeFile) = False Then
                        Core.Player.PokeFiles.Add(TempPokeFile)
                    End If
                End If
            End If
            TempPokeFile = ""

            If IsRemoteBattle = False Then
                If ConnectScreen.Connected = True Then
                    If Battle.Won = False Then
                        If IsTrainerBattle = True Then
                            Core.ServersManager.ServerConnection.SendGameStateMessage("got defeated by " & Trainer.TrainerType & " " & Trainer.Name & ".")
                        Else
                            Core.ServersManager.ServerConnection.SendGameStateMessage("got defeated by a wild " & OppPokemon.GetDisplayName() & ".")
                        End If
                    End If
                End If
            Else
                If IsHost = True Then
                    If Battle.Won = False Then
                        Core.ServersManager.ServerConnection.SendGameStateMessage("hosted a battle: ""Player " & Core.Player.Name & " got defeated by Player " & Trainer.Name & """.")
                    Else
                        Core.ServersManager.ServerConnection.SendGameStateMessage("hosted a battle: ""Player " & Trainer.Name & " got defeated by Player " & Core.Player.Name & """.")
                    End If
                Else
                    Battle.Won = ClientWonBattle
                End If
                PVPLobbyScreen.SetupBattleResults(Me)
            End If

            If CanBlackout = False Then
                blackout = False
            End If
            If blackout = False Then
                ResetVars()

                If IsTrainerBattle = True Then
                    ActionScript.RegisterID("trainer_" & Trainer.TrainerFile)
                End If

                If Me.BattleMode <> BattleModes.PVP Then
                    Abilities.HoneyGather.GatherHoney()
                    Abilities.Pickup.Pickup()
                End If

                Dim hasLevelUp As Boolean = False
                For Each p As Pokemon In Core.Player.Pokemons
                    If p.hasLeveledUp = True Then
                        hasLevelUp = True
                    End If
                    If IsRemoteBattle = True Then
                        If p.OriginalItem IsNot Nothing Then
                            p.Item = P3D.Item.GetItemByID(p.OriginalItem.ID)
                            p.Item.AdditionalData = p.OriginalItem.AdditionalData
                            Screen.TextBox.Show(Core.Player.Name & " received~" & p.OriginalItem.Name & "and gave it back to~" & p.GetDisplayName)
                        End If
                    Else
                        If Not p.OriginalItem Is Nothing Then
                            If p.Item IsNot Nothing Then
                                Core.Player.Inventory.AddItem(p.OriginalItem.ID, 1)
                                SoundManager.PlaySound("item_found", True)
                                Screen.TextBox.Show(Core.Player.Name & " found~" & p.OriginalItem.Name & "!*" & Core.Player.Inventory.GetMessageReceive(p.OriginalItem, 1))
                            Else
                                p.Item = P3D.Item.GetItemByID(p.OriginalItem.ID)
                                p.Item.AdditionalData = p.OriginalItem.AdditionalData
                                Screen.TextBox.Show(Core.Player.Name & " found~" & p.OriginalItem.Name & "*and gave it back to~" & p.GetDisplayName)
                            End If
                            p.OriginalItem = Nothing
                        End If
                    End If
                    p.ResetTemp()
                Next

                If hasLevelUp = False Then
                    MusicManager._afterBattleIntroSong = Nothing
                    MusicManager.Play(SavedOverworld.Level.MusicLoop)
                    Core.SetScreen(New TransitionScreen(Me, SavedOverworld.OverworldScreen, New Color(255, 255, 254), False, AddressOf ChangeSavedScreen))
                Else
                    Dim EvolvePokeList As New List(Of Integer)

                    For i = 0 To Core.Player.Pokemons.Count - 1
                        Dim p As Pokemon = Core.Player.Pokemons(i)

                        If p.hasLeveledUp = True And p.EvolutionConditions.Count > 0 Then
                            p.hasLeveledUp = False
                            If p.CanEvolve(EvolutionCondition.EvolutionTrigger.LevelUp, "") = True Then
                                EvolvePokeList.Add(i)
                            End If
                        End If
                    Next

                    If EvolvePokeList.Count = 0 Then
                        MusicManager._afterBattleIntroSong = Nothing
                        MusicManager.Play(SavedOverworld.Level.MusicLoop)
                        Core.SetScreen(New TransitionScreen(Me, SavedOverworld.OverworldScreen, New Color(255, 255, 254), False, AddressOf ChangeSavedScreen))
                    Else
                        Core.SetScreen(New TransitionScreen(Me, New EvolutionScreen(Core.CurrentScreen, EvolvePokeList, "", EvolutionCondition.EvolutionTrigger.LevelUp, True), Color.Black, False))
                    End If
                End If

                For Each p As Pokemon In Core.Player.Pokemons
                    If p.Number = 213 Then
                        If Not p.Item Is Nothing Then
                            If p.Item.IsBerry = True Then
                                If Core.Random.Next(0, 3) = 0 Then
                                    p.Item = Item.GetItemByID(139)
                                End If
                            End If
                        End If
                    End If
                Next
            Else
                For Each p As Pokemon In Core.Player.Pokemons
                    If IsRemoteBattle = True Then
                        If p.OriginalItem IsNot Nothing Then
                            p.Item = P3D.Item.GetItemByID(p.OriginalItem.ID)
                            p.Item.AdditionalData = p.OriginalItem.AdditionalData
                            Screen.TextBox.Show(Core.Player.Name & " received~" & p.OriginalItem.Name & "and gave it back to~" & p.GetDisplayName)
                        End If
                    Else
                        If Not p.OriginalItem Is Nothing Then
                            If p.Item IsNot Nothing Then
                                Core.Player.Inventory.AddItem(p.OriginalItem.ID, 1)
                                SoundManager.PlaySound("item_found", True)
                                Screen.TextBox.Show(Core.Player.Name & " found~" & p.originalItem.Name & "!*" & Core.Player.Inventory.GetMessageReceive(p.originalItem, 1))
                            Else
                                p.Item = P3D.Item.GetItemByID(p.OriginalItem.ID)
                                p.Item.AdditionalData = p.OriginalItem.AdditionalData
                                Screen.TextBox.Show(Core.Player.Name & " found~" & p.OriginalItem.Name & "*and gave it back to~" & p.GetDisplayName)
                            End If
                            p.OriginalItem = Nothing
                        End If
                    End If
                    p.ResetTemp()
                Next
                ResetVars()
                Core.SetScreen(New TransitionScreen(Me, New BlackOutScreen(Me), Color.Black, False))
            End If
            OwnLeadIndex = 0
            OppLeadIndex = 0
        End Sub

        Public Sub ChangeSavedScreen()
            Screen.Level = SavedOverworld.Level
            Screen.Camera = SavedOverworld.Camera
            Screen.Effect = SavedOverworld.Effect
            Screen.SkyDome = SavedOverworld.SkyDome
            Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
        End Sub

        Public Function TrainerHasFightablePokemon() As Boolean
            For Each p As Pokemon In Trainer.Pokemons
                If p.Status <> Pokemon.StatusProblems.Fainted Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub SendInNewTrainerPokemon(ByVal index As Integer)
            Dim i As Integer = index

            If i = -1 Then
                If IsPVPBattle Then
                    i = 0
                    While Trainer.Pokemons(i).Status = Pokemon.StatusProblems.Fainted OrElse OppPokemonIndex = i OrElse Trainer.Pokemons(i).HP <= 0
                        i += 1
                    End While

                Else
                    i = Core.Random.Next(0, Trainer.Pokemons.Count)
                    While Trainer.Pokemons(i).Status = Pokemon.StatusProblems.Fainted OrElse OppPokemonIndex = i OrElse Trainer.Pokemons(i).HP <= 0
                        i = Core.Random.Next(0, Trainer.Pokemons.Count)
                    End While
                End If
            End If

            OppPokemonIndex = i
            OppPokemon = Trainer.Pokemons(i)

            If Pokedex.GetEntryType(Core.Player.PokedexData, OppPokemon.Number) = 0 Then
                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, OppPokemon.Number, 1)
            End If
        End Sub

        Public Function GetModelName(ByVal own As Boolean) As String
            If Core.Player.ShowModelsInBattle = False Or Me.IsRemoteBattle = True Then
                Return ""
            End If

            Dim poke As Pokemon = OwnPokemon
            If own = False Then
                poke = OppPokemon
            End If

            Dim n As String = PokemonForms.GetAnimationName(poke)

            Dim s As String = "Normal"
            If poke.IsShiny = True Then
                s = "Shiny"
            End If

            Dim p As String = "Models\Pokemon\" & n & "\" & s

            If ModelManager.ModelExist(p) = True Then
                Return p
            End If

            Return ""
        End Function

        Public Shared Sub ResetVars()
            CanCatch = True
            CanRun = True
            CanBlackout = True
            CanReceiveEXP = True
            RoamingBattle = False
            CanUseItems = True
            DiveBattle = False
            IsInverseBattle = False
            CustomBattleMusic = ""
            RoamingPokemonStorage = Nothing
        End Sub

        Public Function GetTrainerMoney() As Integer
            Dim money As Integer = Trainer.Money

            If FieldEffects.AmuletCoin > 0 Then
                money *= 2
            End If

            money += FieldEffects.OwnPayDayCounter

            For Each mysteryEvent As MysteryEventScreen.MysteryEvent In MysteryEventScreen.ActivatedMysteryEvents
                If mysteryEvent.EventType = MysteryEventScreen.EventTypes.MoneyMultiplier Then
                    money = CInt(money * CSng(mysteryEvent.Value.Replace(".", GameController.DecSeparator)))
                End If
            Next

            Return money
        End Function

        Public Sub AddToQuery(ByVal index As Integer, ByVal o As QueryObject)
            If index = -1 Then
                BattleQuery.Add(o)
            Else
                BattleQuery.Insert(index, o)
            End If
        End Sub

#Region "Networking"

        Public IsPVPBattle As Boolean = False
        Public IsRemoteBattle As Boolean = False
        Public IsHost As Boolean = False
        Public PartnerNetworkID As Integer = 0
        Public OwnStatistics As New NetworkPlayerStatistics()
        Public OppStatistics As New NetworkPlayerStatistics()
        Public PVPGameJoltID As String = ""

        Class NetworkPlayerStatistics

            Public Critical As Integer = 0
            Public SuperEffective As Integer = 0
            Public NotVeryEffective As Integer = 0
            Public NoEffect As Integer = 0
            Public Turns As Integer = 0
            Public Switches As Integer = 0
            Public Moves As Integer = 0

            Public Overrides Function ToString() As String
                Return "{" & Critical & "|" & SuperEffective & "|" & NotVeryEffective & "|" & NoEffect & "|" & Turns & "|" & Switches & "|" & Moves & "}"
            End Function

            Public Sub FromString(ByVal s As String)
                s = s.Remove(s.Length - 1, 1).Remove(0, 1)
                Dim data() As String = s.Split(CChar("|"))

                Me.Critical = CInt(data(0))
                Me.SuperEffective = CInt(data(1))
                Me.NotVeryEffective = CInt(data(2))
                Me.NoEffect = CInt(data(3))
                Me.Turns = CInt(data(4))
                Me.Switches = CInt(data(5))
                Me.Moves = CInt(data(6))
            End Sub

        End Class

        Public Function HasToWaitPVP() As Boolean
            If IsPVPBattle = True And IsRemoteBattle = True Then
                If IsHost = True Then
                    If ReceivedInput = "" Then
                        Return True
                    End If
                Else
                    If ClientWaitForData = True Then
                        Return True
                    End If
                    If ReceivedQuery = "" And SentInput = True Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        Private Function CheckNetworkPlayer() As Boolean
            If Me.IsRemoteBattle = True Then
                If Core.ServersManager.ServerConnection.Connected = True Then
                    Dim partnerOnServer As Boolean = False
                    For Each p As Servers.Player In Core.ServersManager.PlayerCollection
                        If p.ServersID = PartnerNetworkID Then
                            partnerOnServer = True
                            Exit For
                        End If
                    Next
                    If partnerOnServer = False Then
                        PVPLobbyScreen.StoppedBattle = True
                        PVPLobbyScreen.DisconnectMessage = "The other player disconnected." & Environment.NewLine & Environment.NewLine & "Press any key to exit."
                        PVPLobbyScreen.ScreenState = PVPLobbyScreen.ScreenStates.Stopped
                        Battle.Won = True
                        EndBattle(False)
                        PVPLobbyScreen.BattleSuccessful = False
                        Return False
                    End If
                Else
                    PVPLobbyScreen.StoppedBattle = True
                    PVPLobbyScreen.DisconnectMessage = "You got disconnected from the server." & Environment.NewLine & Environment.NewLine & "Press any key to exit."
                    PVPLobbyScreen.ScreenState = PVPLobbyScreen.ScreenStates.Stopped
                    Battle.Won = False
                    EndBattle(False)
                    PVPLobbyScreen.BattleSuccessful = False
                    Return False
                End If
            End If
            Return True
        End Function

#Region "Client"

        'Client:
        Public SentInput As Boolean = False
        Public Shared ReceivedQuery As String = ""
        Public ClientWaitForData As Boolean = False
        Public ReceivedPokemonData As Boolean = False
        Public TempPVPBattleQuery As New Dictionary(Of Integer, QueryObject)
        Public LockData As String = "{}"
        Public ClientWonBattle As Boolean = True

        'Sends the decided step to the host
        Public Sub SendClientCommand(ByVal c As String)
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleClientData, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, {PartnerNetworkID.ToString(), c}.ToList()))
            Me.SentInput = True
            Logger.Debug("[Battle]: Sent Client command")
        End Sub

        'Receives the current status of the battle from the host
        Public Shared Sub ReceiveHostEndRoundData(ByVal data As String)
            Dim newQueries As New List(Of String)
            Dim tempData As String = ""

            Dim cData As String = data
            If GameController.IS_DEBUG_ACTIVE Then
                If Directory.Exists(GameController.GamePath & "\PvP Log\") = False Then
                    Directory.CreateDirectory(GameController.GamePath & "\PvP Log\")
                End If
                Dim shownData As String = data.Replace("}{", "}" & Environment.NewLine & "{").Replace("}|{", "}|" & Environment.NewLine & Environment.NewLine & "{")
                IO.File.WriteAllText(GameController.GamePath & "\PvP Log\HostEndRoundData.dat", shownData)
            End If
            'Converts the single string received as data into a list of string 
            While cData.Length > 0
                If cData(0).ToString() = "|" AndAlso tempData(tempData.Length - 1).ToString() = "}" Then
                    newQueries.Add(tempData)
                    tempData = ""
                Else
                    tempData &= cData(0).ToString()
                End If
                cData = cData.Remove(0, 1)
            End While

            If tempData.StartsWith("{") = True And tempData.EndsWith("}") = True Then
                newQueries.Add(tempData)
                tempData = ""
            End If

            Dim s As Screen = Core.CurrentScreen
            While Not s.PreScreen Is Nothing And s.Identification <> Identifications.BattleScreen
                s = s.PreScreen
            End While

            If s.Identification = Identifications.BattleScreen Then

                'First set of queries are read and converted into BattleScreen values for the client side.
                CType(s, BattleScreen).LockData = newQueries(0) 'when locked into certain situations that do not allow the client to take actions (like multi turn moves)
                CType(s, BattleScreen).OppStatistics.FromString(newQueries(1))
                CType(s, BattleScreen).OwnStatistics.FromString(newQueries(2))
                CType(s, BattleScreen).OppPokemon = Pokemon.GetPokemonByData(newQueries(3))
                CType(s, BattleScreen).OwnPokemon = Pokemon.GetPokemonByData(newQueries(4))

                Dim weatherInfo As String = newQueries(5)
                weatherInfo = weatherInfo.Remove(weatherInfo.Length - 1, 1).Remove(0, 1)
                CType(s, BattleScreen).FieldEffects.Weather = CType(CInt(weatherInfo), BattleWeather.WeatherTypes)

                Dim CanSwitchInfo As String = newQueries(6)
                CanSwitchInfo = CanSwitchInfo.Remove(CanSwitchInfo.Length - 1, 1).Remove(0, 1)
                CType(s, BattleScreen).FieldEffects.ClientCanSwitch = CType(CanSwitchInfo, Boolean)

                For i = 0 To 6
                    newQueries.RemoveAt(0)
                Next

                'Next queries contain the data from the party of the host and the client.
                Dim ownCount As Integer = Core.Player.Pokemons.Count
                Dim oppCount As Integer = CType(s, BattleScreen).Trainer.Pokemons.Count

                CType(s, BattleScreen).Trainer.Pokemons.Clear()
                Core.Player.Pokemons.Clear()

                For i = 0 To oppCount - 1
                    CType(s, BattleScreen).Trainer.Pokemons.Add(Pokemon.GetPokemonByData(newQueries(i)))
                    If CType(s, BattleScreen).Trainer.Pokemons.Last().GetSaveData() = CType(s, BattleScreen).OppPokemon.GetSaveData() Then
                        CType(s, BattleScreen).OppPokemonIndex = CType(s, BattleScreen).Trainer.Pokemons.Count - 1
                    End If
                Next

                For i = oppCount To oppCount + ownCount - 1
                    Core.Player.Pokemons.Add(Pokemon.GetPokemonByData(newQueries(i)))
                    If Core.Player.Pokemons.Last().GetSaveData() = CType(s, BattleScreen).OwnPokemon.GetSaveData() Then
                        CType(s, BattleScreen).OwnPokemonIndex = Core.Player.Pokemons.Count - 1
                    End If
                Next

                Logger.Debug("[Battle]: Received Host End Round data")
                CType(s, BattleScreen).ReceivedPokemonData = True
            End If
        End Sub

        'Receives the "movie" from the host, and stores it in the BattleQuery list. Also checks for After Fainting Switch conditions.
        Public Shared Sub ReceiveHostData(ByVal data As String)
            Dim newQueries As New List(Of String)
            Dim tempData As String = ""
            Dim cData As String = data
            Dim s As Screen = Core.CurrentScreen
            While Not s.PreScreen Is Nothing And s.Identification <> Identifications.BattleScreen
                s = s.PreScreen
            End While
            If s.Identification = Identifications.BattleScreen Then
                If GameController.IS_DEBUG_ACTIVE Then
                    If Directory.Exists(GameController.GamePath & "\PvP Log\") = False Then
                        Directory.CreateDirectory(GameController.GamePath & "\PvP Log\")
                    End If
                    Dim shownData As String = data.Replace("}{", "}" & Environment.NewLine & "{").Replace("}|{", "}|" & Environment.NewLine & Environment.NewLine & "{")
                    IO.File.WriteAllText(GameController.GamePath & "\PvP Log\HostData.dat", shownData)
                End If
            End If

            While cData.Length > 0
                If cData(0).ToString() = "|" AndAlso tempData(tempData.Length - 1).ToString() = "}" Then
                    newQueries.Add(tempData)
                    tempData = ""
                Else
                    tempData &= cData(0).ToString()
                End If
                cData = cData.Remove(0, 1)
            End While

            If tempData.StartsWith("{") = True And tempData.EndsWith("}") = True Then
                newQueries.Add(tempData)
                tempData = ""
            End If

            If s.Identification = Identifications.BattleScreen Then
                CType(s, BattleScreen).BattleQuery.Clear()
                For Each q As String In newQueries
                    Dim Query As QueryObject = QueryObject.FromString(q)
                    If Query IsNot Nothing Then
                        CType(s, BattleScreen).BattleQuery.Add(Query)
                    End If
                Next
                For i = 0 To 99
                    CType(s, BattleScreen).InsertCasualCameramove()
                Next

                For Each q As QueryObject In CType(s, BattleScreen).BattleQuery
                    If q.QueryType = QueryObject.QueryTypes.Textbox Then
                        If CType(q, TextQueryObject).Text = "You lost the battle!" Then
                            CType(s, BattleScreen).ClientWonBattle = False
                        End If
                    End If
                Next
            End If
            Logger.Debug("[Battle]: Received Host data (movie)")
            ReceivedQuery = data
        End Sub

#End Region

#Region "Host"

        'Host:
        Public SentHostData As Boolean = False
        Public Shared ReceivedInput As String = ""

        'After the client has decided its next step, the host receives the information about this step, so it can now decide his own.
        Public Shared Sub ReceiveClientData(ByVal data As String)
            Logger.Debug("[Battle]: Received Client data")
            ReceivedInput = data

            If GameController.IS_DEBUG_ACTIVE Then
                If Directory.Exists(GameController.GamePath & "\PvP Log\") = False Then
                    Directory.CreateDirectory(GameController.GamePath & "\PvP Log\")
                End If
                Dim shownData As String = data.Replace("}{", "}" & Environment.NewLine & "{").Replace("}|{", "}|" & Environment.NewLine & Environment.NewLine & "{")
                IO.File.WriteAllText(GameController.GamePath & "\PvP Log\ClientCommand.dat", shownData)
            End If

            Dim s As Screen = Core.CurrentScreen
            While Not s.PreScreen Is Nothing And s.Identification <> Identifications.BattleScreen
                s = s.PreScreen
            End While

            Dim BV2Screen As BattleScreen = CType(s, BattleScreen)
            BV2Screen.BattleMenu.Visible = False

            'prevents multi turn action to take place in an after fainting switching turn
            If Not (BV2Screen.OppFaint And BV2Screen.IsRemoteBattle) Then
                BV2Screen.Battle.StartMultiTurnAction(BV2Screen)
            Else
                BV2Screen.BattleMenu.Visible = True
            End If
        End Sub

        'Sends some variables that let the client know the current state of the battle
        Public Sub SendEndRoundData()
            Dim lockData As String = "{}"
            Dim oppStep As Battle.RoundConst = Battle.GetOppStep(Me, Battle.OwnStep)
            If Battle.SelectedMoveOpp = False Then
                If oppStep.StepType = BattleSystem.Battle.RoundConst.StepTypes.Move Then
                    lockData = "{" & CType(oppStep.Argument, Attack).ID.ToString() & "}"
                Else
                    lockData = "{" & CStr(oppStep.Argument) & "}"
                End If
            End If

            Dim d As String = lockData & "|" &
                              OwnStatistics.ToString() & "|" & OppStatistics.ToString() & "|" &
                              OwnPokemon.GetSaveData() & "|" & OppPokemon.GetSaveData() & "|" &
                              "{" & CInt(FieldEffects.Weather).ToString() & "}" & "|" &
                              "{" & BattleCalculation.CanSwitch(Me, False).ToString & "}"

            For Each p As Pokemon In Core.Player.Pokemons
                If d <> "" Then
                    d &= "|"
                End If
                d &= p.GetSaveData()
            Next
            For Each p As Pokemon In Trainer.Pokemons
                If d <> "" Then
                    d &= "|"
                End If
                d &= p.GetSaveData()
            Next
            If GameController.IS_DEBUG_ACTIVE Then
                If Directory.Exists(GameController.GamePath & "\PvP Log\") = False Then
                    Directory.CreateDirectory(GameController.GamePath & "\PvP Log\")
                End If
                Dim shownData As String = d.Replace("}{", "}" & Environment.NewLine & "{").Replace("}|{", "}|" & Environment.NewLine & Environment.NewLine & "{")
                IO.File.WriteAllText(GameController.GamePath & "\PvP Log\SentEndRoundData.dat", shownData)
            End If
            Logger.Debug("[Battle]: Sent End Round data")
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattlePokemonData, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, {PartnerNetworkID.ToString(), d}.ToList()))
        End Sub

        'Sends the "movie" to the client
        Public Sub SendHostQuery()
            Dim d As String = ""

            Dim sendQuery As New List(Of QueryObject)
            For i = 0 To Me.BattleQuery.Count - 1
                If Me.TempPVPBattleQuery.ContainsKey(i) = False Then
                    sendQuery.Add(Me.BattleQuery(i))
                Else
                    sendQuery.Add(Me.TempPVPBattleQuery(i))
                End If
            Next

            For Each q As QueryObject In sendQuery
                If d <> "" Then
                    d &= "|"
                End If
                d &= q.ToString()
            Next
            Me.TempPVPBattleQuery.Clear()

            Logger.Debug("[Battle]: Sent Host Query")
            If GameController.IS_DEBUG_ACTIVE Then
                If Directory.Exists(GameController.GamePath & "\PvP Log\") = False Then
                    Directory.CreateDirectory(GameController.GamePath & "\PvP Log\")
                End If
                Dim shownData As String = d.Replace("}{", "}" & Environment.NewLine & "{").Replace("}|{", "}|" & Environment.NewLine & Environment.NewLine & "{")
                IO.File.WriteAllText(GameController.GamePath & "\PvP Log\SentHostQuery.dat", shownData)
            End If
            Core.ServersManager.ServerConnection.SendPackage(New Servers.Package(Servers.Package.PackageTypes.BattleHostData, Core.ServersManager.ID, Servers.Package.ProtocolTypes.TCP, {PartnerNetworkID.ToString(), d}.ToList()))
            SentHostData = True
        End Sub

#End Region

#Region "GameJolt"

        ''' <summary>
        ''' Use this to download the sprites for the players.
        ''' </summary>
        Private Sub DownloadOnlineSprites()
            If Core.Player.IsGameJoltSave = True Then
                'Dim t As New Threading.Thread(AddressOf DownloadSprites)
                't.IsBackground = True
                't.Start()
                DownloadSprites()
            End If
        End Sub

        Private Sub DownloadSprites()
            OwnTrainerNPC.SetupSprite(OwnTrainerNPC.TextureID, Core.GameJoltSave.GameJoltID, True)
            If PVPGameJoltID <> "" Then
                OppTrainerNPC.SetupSprite(OppTrainerNPC.TextureID, PVPGameJoltID, True)
            End If
        End Sub

#End Region

#End Region

#Region "Profiles and Targets"

        Public Function GetProfile(ByVal Target As PokemonTarget) As PokemonProfile
            For Each p As PokemonProfile In Me.Profiles
                If p.FieldPosition = Target Then
                    Return p
                End If
            Next
            Return Nothing
        End Function

#End Region

    End Class

End Namespace