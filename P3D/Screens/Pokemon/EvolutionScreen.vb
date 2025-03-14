﻿Public Class EvolutionScreen

    Inherits Screen

    ' Old Evolution System:

    'Private FromBattle As Boolean = False
    'Private EvolutionArgument As String = ""
    'Private EvolutionTrigger As EvolutionCondition.EvolutionTrigger

    'Private PokemonList As New List(Of Integer)

    'Dim currentPokemon As Pokemon
    'Dim evolvedPokemon As Pokemon

    'Dim PokemonModel As ModelEntity
    'Dim C As Camera

    'Public Sub New(ByVal currentScreen As Screen, ByVal EvolvePokemonIndices As List(Of Integer), ByVal EvolutionArgument As String, ByVal EvolutionTrigger As EvolutionCondition.EvolutionTrigger, Optional ByVal FromBattle As Boolean = False)
    '    Me.Identification = Identifications.EvolutionScreen
    '    Me.PreScreen = currentScreen
    '    Me.CanBePaused = True
    '    Me.CanChat = True
    '    Me.CanMuteAudio = True
    '    Me.MouseVisible = False

    '    Me.FromBattle = FromBattle
    '    Me.EvolutionArgument = EvolutionArgument
    '    Me.EvolutionTrigger = EvolutionTrigger
    '    Me.PokemonList = EvolvePokemonIndices

    '    Me.currentPokemon = Basic.Player.Pokemons(Me.PokemonList(0))
    '    Me.evolvedPokemon = EvolvePokemon(currentPokemon, EvolutionArgument, EvolutionTrigger)

    '    Dim shiny As String = "Normal"
    '    If currentPokemon.IsShiny = True Then
    '        shiny = "Shiny"
    '    End If
    '    Me.PokemonModel = CType(Entity.GetNewEntity("ModelEntity", New Vector3(0), {}, {}, False, New Vector3(MathHelper.Pi * 0.5F, 0, 0), New Vector3(0.1F), BaseModel.BlockModel, 0, "Models\" & Me.currentPokemon.AnimationName & "\" & shiny, True, New Vector3(1), 0, "", "", New Vector3(0), Nothing), ModelEntity)

    '    Me.C = Screen.Camera

    '    Screen.Camera = New EvolutionCamera()
    'End Sub

    'Public Overrides Sub Draw()
    '    Me.PokemonModel.Render()

    '    TextBox.Draw()
    'End Sub

    'Dim state As Integer = 0

    'Public Overrides Sub Update()
    '    Camera.Update()
    '    SkyDome.Update()
    '    TextBox.Update()

    '    If TextBox.Showing = False Then
    '        Select Case state
    '            Case 0
    '                state += 1
    '                TextBox.Show("What?*" & currentPokemon.GetDisplayName() & "~is evolving!", {}, False, False)
    '            Case 1

    '        End Select
    '    End If
    'End Sub

    'Public Shared Function EvolvePokemon(ByVal currentPokemon As Pokemon, ByVal EvolutionArgument As String, ByVal EvolutionTrigger As EvolutionCondition.EvolutionTrigger) As Pokemon
    '    Dim HPpercentage As Integer = CInt((currentPokemon.HP / currentPokemon.MaxHP) * 100)

    '    Dim evolvedPokemon As Pokemon = PokeGetter.GetPokemonByID(currentPokemon.GetEvolutionID(EvolutionTrigger, EvolutionArgument))
    '    evolvedPokemon.Status = currentPokemon.Status

    '    evolvedPokemon.EVHP = currentPokemon.EVHP
    '    evolvedPokemon.EVAttack = currentPokemon.EVAttack
    '    evolvedPokemon.EVDefense = currentPokemon.EVDefense
    '    evolvedPokemon.EVSpAttack = currentPokemon.EVSpAttack
    '    evolvedPokemon.EVSpDefense = currentPokemon.EVSpDefense
    '    evolvedPokemon.EVSpeed = currentPokemon.EVSpeed

    '    evolvedPokemon.Friendship = currentPokemon.Friendship
    '    evolvedPokemon.NickName = currentPokemon.NickName

    '    evolvedPokemon.IVHP = currentPokemon.IVHP
    '    evolvedPokemon.IVAttack = currentPokemon.IVAttack
    '    evolvedPokemon.IVDefense = currentPokemon.IVDefense
    '    evolvedPokemon.IVSpAttack = currentPokemon.IVSpAttack
    '    evolvedPokemon.IVSpDefense = currentPokemon.IVSpDefense
    '    evolvedPokemon.IVSpeed = currentPokemon.IVSpeed

    '    evolvedPokemon.Wild(currentPokemon.Level, False)

    '    evolvedPokemon.Attacks = currentPokemon.Attacks
    '    evolvedPokemon.Gender = currentPokemon.Gender
    '    evolvedPokemon.Nature = currentPokemon.Nature

    '    evolvedPokemon.CalculateStats()

    '    Dim hasOldAbility As Boolean = False
    '    For Each a As Ability In evolvedPokemon.NewAbilities
    '        If a.ID = currentPokemon.Ability.ID Then
    '            hasOldAbility = True
    '            Exit For
    '        End If
    '    Next

    '    If hasOldAbility = False Then
    '        evolvedPokemon.Ability = evolvedPokemon.NewAbilities(Basic.Random.Next(0, evolvedPokemon.NewAbilities.Count))
    '    Else
    '        evolvedPokemon.Ability = currentPokemon.Ability
    '    End If

    '    evolvedPokemon.IsShiny = currentPokemon.IsShiny
    '    evolvedPokemon.Item = currentPokemon.Item

    '    evolvedPokemon.CatchBall = currentPokemon.CatchBall
    '    evolvedPokemon.CatchLocation = currentPokemon.CatchLocation
    '    evolvedPokemon.CatchMethod = currentPokemon.CatchMethod
    '    evolvedPokemon.CatchTrainerName = currentPokemon.CatchTrainerName
    '    evolvedPokemon.OT = currentPokemon.OT
    '    evolvedPokemon.Experience = currentPokemon.Experience

    '    evolvedPokemon.HP = CInt(evolvedPokemon.MaxHP * (HPpercentage / 100))

    '    Return evolvedPokemon
    'End Function

    Public PokeList As New List(Of Integer)
    Dim currentPokemon As Pokemon
    Dim evolvedPokemon As Pokemon

    Dim evolutionReady As Boolean = False
    Dim evolutionStarted As Boolean = False
    Dim evolved As Boolean = False
    Dim brokeEvolution As Boolean = False
    Dim AttackLearnList As New List(Of BattleSystem.Attack)
    Dim CanEnd As Boolean = False

    Dim EvolutionArg As String = ""
    Dim EvolutionTrigger As EvolutionCondition.EvolutionTrigger
    Dim FromBattle As Boolean = False

    Private Sparks As New List(Of Spark)

    Public Sub New(ByVal currentScreen As Screen, ByVal EvolvePokemonIndices As List(Of Integer), ByVal EvolutionArg As String, ByVal EvolutionTrigger As EvolutionCondition.EvolutionTrigger, Optional ByVal FromBattle As Boolean = False)
        Me.Identification = Identifications.EvolutionScreen
        PlayerStatistics.Track("Evolutions", 1)

        Me.AttackLearnList.Clear()
        Me.PreScreen = currentScreen
        Me.FromBattle = FromBattle
        Me.CanEnd = False
        For Each i As Integer In EvolvePokemonIndices
            PokeList.Add(i)
        Next

        Me.EvolutionArg = EvolutionArg
        Me.EvolutionTrigger = EvolutionTrigger

        currentPokemon = Core.Player.Pokemons(PokeList(0))

        EvolvePokemon()

        Me.SavedMusic = MusicManager.CurrentSong.Name
        MusicManager.PlayNoMusic()
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), Color.Black)
        Dim T As Texture2D = currentPokemon.GetTexture(True)
        Dim Size As Vector2 = New Vector2(CInt(MathHelper.Min(T.Width * 3, 288)), CInt(MathHelper.Min(T.Height * 3, 288)))

        If evolved = True Then
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Evolution\Light"), New Rectangle(CInt((Core.windowSize.Width / 2) - Size.X), CInt((Core.windowSize.Height / 2) - Size.Y), CInt(Size.X * 2), CInt(Size.Y * 2)), Color.White)
        End If

        If evolved = True Then
            T = evolvedPokemon.GetTexture(True)
            Size = New Vector2(CInt(MathHelper.Min(T.Width * 3, 288)), CInt(MathHelper.Min(T.Height * 3, 288)))
        End If
        Core.SpriteBatch.Draw(T, New Rectangle(CInt((Core.windowSize.Width / 2) - Size.X / 2), CInt((Core.windowSize.Height / 2) - Size.Y / 2), CInt(Size.X), CInt(Size.Y)), Color.White)

        For Each Spark As Spark In Sparks
            Spark.Draw()
        Next

        TextBox.Draw()
    End Sub

    Public Overrides Sub Update()
        TextBox.Update()

        If evolutionStarted = False Then
            SoundManager.PlayPokemonCry(currentPokemon.Number, PokemonForms.GetCrySuffix(currentPokemon))
            TextBox.Show("What?*" & currentPokemon.GetDisplayName() & " is evolving!", {}, False, False)
            evolutionStarted = True
            For i = 0 To Core.Random.Next(200, 250)
                Sparks.Add(New Spark())
            Next
        Else

            If evolutionReady = False And TextBox.Showing = False Then
                MusicManager.Play("evolution", True)

                If evolved = False Then
                    Dim allReady As Boolean = True

                    For Each Spark As Spark In Sparks
                        Spark.Update()

                        If Spark.IsReady = False Then
                            allReady = False
                        End If
                    Next

                    If allReady = True Then
                        evolved = True
                        For Each Spark As Spark In Sparks
                            Spark.doGrow = True
                        Next
                    Else
                        If Controls.Dismiss(True, True) = True Then
                            Sparks.Clear()
                            evolutionReady = True
                            brokeEvolution = True
                            TextBox.Show("Huh? " & currentPokemon.GetDisplayName() & "~stopped evolving!", {}, False, False)
                        End If
                    End If
                Else
                    Dim allReady As Boolean = True

                    For Each Spark As Spark In Sparks
                        Spark.Update()

                        If Spark.IsReady = False Or Spark.doGrow = True Then
                            allReady = False
                        End If
                    Next

                    If allReady = True Then
                        Sparks.Clear()
                        Dim type As Integer = 2
                        If evolvedPokemon.IsShiny = True Then
                            type = 3
                        End If

                        Dim dexID As String = PokemonForms.GetPokemonDataFileName(evolvedPokemon.Number, evolvedPokemon.AdditionalData)
                        If dexID.Contains("_") = False Then
                            If PokemonForms.GetAdditionalDataForms(evolvedPokemon.Number) IsNot Nothing AndAlso PokemonForms.GetAdditionalDataForms(evolvedPokemon.Number).Contains(evolvedPokemon.AdditionalData) Then
                                dexID = evolvedPokemon.Number & ";" & evolvedPokemon.AdditionalData
                            Else
                                dexID = evolvedPokemon.Number.ToString
                            End If
                        End If

                        Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, type)

                        evolvedPokemon.PlayCry()
                        SoundManager.PlaySound("success", True)

                        evolutionReady = True
                        Dim t As String = "Congratulations!*Your " & currentPokemon.GetDisplayName() & "~evolved into " & evolvedPokemon.GetName() & "!"
                        If evolvedPokemon.AttackLearns.ContainsKey(evolvedPokemon.Level) = True Then
                            Dim aList As List(Of BattleSystem.Attack) = evolvedPokemon.AttackLearns(evolvedPokemon.Level)
                            For a = 0 To aList.Count - 1
                                If evolvedPokemon.KnowsMove(aList(a)) = False Then
                                    If evolvedPokemon.Attacks.Count = 4 Then
                                        Me.AttackLearnList.Add(aList(a))
                                    Else
                                        evolvedPokemon.Attacks.Add(aList(a))

                                        t &= "*" & evolvedPokemon.GetDisplayName() & " learned~" & aList(a).Name & "!"
                                        PlayerStatistics.Track("Moves learned", 1)
                                        CanEnd = True
                                    End If
                                End If
                            Next
                        Else
                            CanEnd = True
                        End If

                        If Me.EvolutionTrigger = EvolutionCondition.EvolutionTrigger.Trading Then
                            Dim econ As EvolutionCondition = EvolutionCondition.GetEvolutionCondition(currentPokemon, Me.EvolutionTrigger, Me.EvolutionArg)
                            Dim removeItem As Boolean = False
                            If econ.Trigger = EvolutionCondition.EvolutionTrigger.Trading Then
                                For i = 0 To econ.Conditions.Count - 1
                                    If econ.Conditions(i).ConditionType = EvolutionCondition.ConditionTypes.HoldItem Then
                                        removeItem = True
                                    End If
                                Next
                            End If
                            If removeItem Then
                                evolvedPokemon.Item = Nothing
                            End If
                        End If

                        Core.Player.AddPoints(10, "Evolved Pokémon.")

                        If ConnectScreen.Connected = True Then
                            Core.ServersManager.ServerConnection.SendGameStateMessage("evolved their " & currentPokemon.GetName() & " into a " & evolvedPokemon.GetName() & "!")
                        End If

                        TextBox.Show(t, {}, False, False)
                    End If
                End If
            Else
                If TextBox.Showing = False Then
                    If CanEnd = False Then
                        If AttackLearnList.Count > 0 Then
                            Core.SetScreen(New LearnAttackScreen(Core.CurrentScreen, evolvedPokemon, AttackLearnList))
                        End If
                        CanEnd = True
                    Else
                        If Core.CurrentScreen.Identification = Identifications.EvolutionScreen Then
                            Endscene()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Endscene()
        If brokeEvolution = False Then
            ' Nincada's evolution to Shedinja if a Poké Ball is in the player's Bag and free space is available in the player's party:
            If Shedinja.CanEvolveInto(Me.evolvedPokemon, Me.EvolutionTrigger) = True Then
                Core.Player.Pokemons.Add(Shedinja.GenerateNew(evolvedPokemon))
                Core.Player.Inventory.RemoveItem(5.ToString, 1)
            End If

            Core.Player.Pokemons(PokeList(0)) = evolvedPokemon
        End If
        PokeList.RemoveAt(0)
        If PokeList.Count = 0 Then
            If FromBattle = False Then
                Dim s As Screen = Core.CurrentScreen
                While s.PreScreen.Identification = Identifications.EvolutionScreen
                    s = s.PreScreen
                End While
                Core.SetScreen(New TransitionScreen(s, s.PreScreen, Color.Black, False))
                MusicManager.Play(SavedMusic, True, 0.01F)
            Else
                Dim s As Screen = Core.CurrentScreen
                While s.Identification <> Identifications.BattleScreen
                    s = s.PreScreen
                End While
                ChangeSavedScreen()
                Core.SetScreen(New TransitionScreen(Me, CType(s, BattleSystem.BattleScreen).SavedOverworld.OverworldScreen, Color.Black, False))
            End If
        Else
            Core.SetScreen(New TransitionScreen(Me, New EvolutionScreen(Me, PokeList, Me.EvolutionArg, Me.EvolutionTrigger, Me.FromBattle), Color.Black, False))
        End If
    End Sub

    Private Sub EvolvePokemon()
        Dim HPpercentage As Integer = CInt((currentPokemon.HP / currentPokemon.MaxHP) * 100)
        Dim ID As String = currentPokemon.GetEvolutionID(Me.EvolutionTrigger, Me.EvolutionArg)
        If ID.Contains(CChar("_")) Then
            evolvedPokemon = Pokemon.GetPokemonByID(CInt(ID.Split(CChar("_"))(0)), ID.Split(CChar("_"))(1), True)
        Else
            evolvedPokemon = Pokemon.GetPokemonByID(CInt(ID), "", True)
        End If

        If evolvedPokemon.AdditionalData = "" AndAlso currentPokemon.AdditionalData <> "" Then
            evolvedPokemon.AdditionalData = currentPokemon.AdditionalData
        End If

        evolvedPokemon.Status = currentPokemon.Status

        evolvedPokemon.EVHP = currentPokemon.EVHP
        evolvedPokemon.EVAttack = currentPokemon.EVAttack
        evolvedPokemon.EVDefense = currentPokemon.EVDefense
        evolvedPokemon.EVSpAttack = currentPokemon.EVSpAttack
        evolvedPokemon.EVSpDefense = currentPokemon.EVSpDefense
        evolvedPokemon.EVSpeed = currentPokemon.EVSpeed

        evolvedPokemon.Friendship = currentPokemon.Friendship
        evolvedPokemon.NickName = currentPokemon.NickName

        evolvedPokemon.IVHP = currentPokemon.IVHP
        evolvedPokemon.IVAttack = currentPokemon.IVAttack
        evolvedPokemon.IVDefense = currentPokemon.IVDefense
        evolvedPokemon.IVSpAttack = currentPokemon.IVSpAttack
        evolvedPokemon.IVSpDefense = currentPokemon.IVSpDefense
        evolvedPokemon.IVSpeed = currentPokemon.IVSpeed

        evolvedPokemon.Generate(currentPokemon.Level, False)

        evolvedPokemon.Attacks = currentPokemon.Attacks
        evolvedPokemon.Gender = currentPokemon.Gender
        evolvedPokemon.Nature = currentPokemon.Nature

        evolvedPokemon.ReloadDefinitions()
        evolvedPokemon.CalculateStats()

        Dim hasOldAbility As Boolean = False

        If currentPokemon.IsUsingHiddenAbility = True And evolvedPokemon.HasHiddenAbility = True Then
            evolvedPokemon.Ability = evolvedPokemon.HiddenAbility
        Else
            For Each a As Ability In evolvedPokemon.NewAbilities
                If a.ID = currentPokemon.Ability.ID Then
                    hasOldAbility = True
                    Exit For
                End If
            Next

            If hasOldAbility = False Then
                Dim AbilityNumber As Integer = -1
                For i = 0 To currentPokemon.NewAbilities.Count - 1
                    If currentPokemon.NewAbilities(i).ID = currentPokemon.Ability.ID Then
                        AbilityNumber = i
                        Exit For
                    End If
                Next
                If AbilityNumber > -1 Then
                    AbilityNumber = AbilityNumber.Clamp(0, evolvedPokemon.NewAbilities.Count - 1)
                    evolvedPokemon.Ability = evolvedPokemon.NewAbilities(AbilityNumber)
                Else
                    evolvedPokemon.Ability = evolvedPokemon.NewAbilities(Core.Random.Next(0, evolvedPokemon.NewAbilities.Count))
                End If
            Else
                evolvedPokemon.Ability = currentPokemon.Ability
            End If
        End If
        evolvedPokemon.SetOriginalAbility()
        evolvedPokemon.IsShiny = currentPokemon.IsShiny
        evolvedPokemon.Item = currentPokemon.Item

        evolvedPokemon.CatchBall = currentPokemon.CatchBall
        If currentPokemon.CatchMethod = "" Then
            currentPokemon.CatchMethod = Localization.GetString("CatchMethod_Empty", "Somehow obtained at")
        End If
        If currentPokemon.CatchLocation = "" Then
            currentPokemon.CatchLocation = Localization.GetString("CatchLocation_Empty", "an unknown place")
        End If
        evolvedPokemon.CatchLocation = currentPokemon.CatchLocation
        evolvedPokemon.CatchMethod = currentPokemon.CatchMethod
        evolvedPokemon.CatchTrainerName = currentPokemon.CatchTrainerName
        evolvedPokemon.OT = currentPokemon.OT
        evolvedPokemon.Experience = currentPokemon.Experience

        evolvedPokemon.HP = CInt(evolvedPokemon.MaxHP * (HPpercentage / 100))
    End Sub

    Dim SavedMusic As String = ""

    Public Sub ChangeSavedScreen()
        Dim s As Screen = Core.CurrentScreen
        While s.Identification <> Identifications.BattleScreen
            s = s.PreScreen
        End While
        Screen.Level = CType(s, BattleSystem.BattleScreen).SavedOverworld.Level
        Screen.Camera = CType(s, BattleSystem.BattleScreen).SavedOverworld.Camera
        Screen.Effect = CType(s, BattleSystem.BattleScreen).SavedOverworld.Effect
        Screen.SkyDome = CType(s, BattleSystem.BattleScreen).SavedOverworld.SkyDome
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
    End Sub

End Class

Public Class Spark

    Dim T As Texture2D
    Dim Position As New Vector2(0, 0)
    Dim Size As Integer = 0
    Dim Speed As Single = 0.0F
    Dim aim As Vector2
    Dim Delay As Single = 0.0F
    Dim C As Color = New Color(255, 255, 255)

    Dim xReady As Boolean = False
    Dim yReady As Boolean = False

    Public IsReady As Boolean = False

    Public doGrow As Boolean = False
    Dim grown As Integer = 50

    Public Sub New()
        Me.T = TextureManager.GetTexture("GUI\Evolution\EvolutionSpark")
        Me.Size = Core.Random.Next(12, 150)
        Me.Speed = CSng(Core.Random.Next(5, 35) / 10)
        Me.aim = New Vector2(CSng(Core.windowSize.Width / 2) - CSng(Size / 2), CSng(Core.windowSize.Height / 2) - CSng(Size / 2))
        Me.aim = New Vector2(Me.aim.X + (Core.Random.Next(0, 320) - 160), Me.aim.Y + (Core.Random.Next(0, 320) - 160))
        Me.Delay = CSng(Core.Random.Next(10, 250) / 10)
        Me.C = New Color(Core.Random.Next(200, 256), Core.Random.Next(200, 256), Core.Random.Next(200, 256))

        Select Case Core.Random.Next(0, 4)
            Case 0
                Me.Position.X = -Size
                Me.Position.Y = Core.Random.Next(0, Core.windowSize.Height - Size)
            Case 1
                Me.Position.X = Core.Random.Next(0, Core.windowSize.Height - Size)
                Me.Position.Y = -Size
            Case 2
                Me.Position.X = Core.windowSize.Width
                Me.Position.Y = Core.Random.Next(0, Core.windowSize.Height - Size)
            Case 3
                Me.Position.X = Core.Random.Next(0, Core.windowSize.Height - Size)
                Me.Position.Y = Core.windowSize.Height
        End Select
    End Sub

    Public Sub Draw()
        Core.SpriteBatch.Draw(Me.T, New Rectangle(CInt(Me.Position.X), CInt(Me.Position.Y), Me.Size, Me.Size), Me.C)
    End Sub

    Public Sub Update()
        If Me.IsReady = False Then
            If Me.Delay > 0.0F Then
                Me.Delay -= 0.1F
                If Me.Delay <= 0.0F Then
                    Me.Delay = 0.0F
                End If
            Else
                If xReady = False Then
                    If Me.Position.X = Me.aim.X Then
                        Me.xReady = True
                    End If
                    If Me.Position.X < aim.X Then
                        Me.Position.X += Speed
                        If Me.Position.X >= aim.X Then
                            Me.xReady = True
                        End If
                    End If
                    If Me.Position.X > aim.X Then
                        Me.Position.X -= Speed
                        If Me.Position.X <= aim.X Then
                            Me.xReady = True
                        End If
                    End If
                End If

                If yReady = False Then
                    If Me.Position.Y = Me.aim.Y Then
                        Me.yReady = True
                    End If
                    If Me.Position.Y < aim.Y Then
                        Me.Position.Y += Speed
                        If Me.Position.Y >= aim.Y Then
                            Me.yReady = True
                        End If
                    End If
                    If Me.Position.Y > aim.Y Then
                        Me.Position.Y -= Speed
                        If Me.Position.Y <= aim.Y Then
                            Me.yReady = True
                        End If
                    End If
                End If

                If Core.Random.Next(0, 3) = 0 Then
                    Me.Speed += 0.1F
                End If

                If Me.xReady = True And Me.yReady = True Then
                    Me.IsReady = True
                End If
            End If
        End If

        If doGrow = True Then
            If Me.grown > 0 Then
                Me.Size += 2
                Me.Position.X -= 1
                Me.Position.Y -= 1
                Me.grown -= 1

                If grown = 0 Then
                    Select Case Core.Random.Next(0, 4)
                        Case 0
                            Me.aim.X = -Size
                            Me.aim.Y = Core.Random.Next(0, Core.windowSize.Height - Size)
                        Case 1
                            Me.aim.X = Core.Random.Next(0, Core.windowSize.Height - Size)
                            Me.aim.Y = -Size
                        Case 2
                            Me.aim.X = Core.windowSize.Width
                            Me.aim.Y = Core.Random.Next(0, Core.windowSize.Height - Size)
                        Case 3
                            Me.aim.X = Core.Random.Next(0, Core.windowSize.Height - Size)
                            Me.aim.Y = Core.windowSize.Height
                    End Select

                    doGrow = False
                    Me.IsReady = False
                    Me.xReady = False
                    Me.yReady = False
                    Me.Speed *= 3
                End If
            End If
        End If
    End Sub

End Class