Public Class BattleCatchScreen

    Inherits Screen

    Dim Ball As Item

    Dim Animations As BattleSystem.AnimationQueryObject = New BattleSystem.AnimationQueryObject(BattleScreen.OppPokemonNPC, Nothing, BattleScreen.OppPokemonModel)
    Dim BallStartPosition As Vector3 = New Vector3(Camera.Position.X - 1.0F, Camera.Position.Y, Camera.Position.Z - 1.0F) + BattleScreen.BattleMapOffset
    Dim BallEntity As Entity = Nothing

    Dim PokemonScale As Vector3

    Dim AnimationStarted As Boolean = False
    Dim caught As Boolean = False
    Dim InBall As Boolean = False
    Dim AnimationIndex As Integer = 0
    Dim renamed As Boolean = False

    Dim textboxStart As Boolean = False
    Dim showPokedexEntry As Boolean = False

    Dim boxData As String = ""
    Dim sentToBox As Boolean = False

    Dim p As Pokemon

    Dim ModelVisible As Boolean = True
    Dim SpriteVisible As Boolean = False

    Dim BattleScreen As BattleSystem.BattleScreen

    Public Sub New(ByVal BattleScreen As BattleSystem.BattleScreen, ByVal Ball As Item)
        Me.Identification = Identifications.BattleCatchScreen

        Me.Ball = Ball
        Me.PreScreen = BattleScreen
        Me.UpdateFadeIn = True

        Me.BattleScreen = BattleScreen
        p = BattleScreen.OppPokemon

        Me.ModelVisible = BattleScreen.OppPokemonModel.Visible
        Me.SpriteVisible = BattleScreen.OppPokemonNPC.Visible

        BattleScreen.OppPokemonModel.Visible = False
        BattleScreen.OppPokemonNPC.Visible = True

        SetCamera()
    End Sub

    Public Overrides Sub Draw()
        SkyDome.Draw(45.0F)

        Level.Draw()

        Dim RenderObjects As New List(Of Entity)

        If InBall = False Then
            RenderObjects.Add(BattleScreen.OppPokemonNPC)
        End If

        If RenderObjects.Count > 0 Then
            RenderObjects = (From r In RenderObjects Order By r.CameraDistance Descending).ToList()
        End If

        For Each [Object] As Entity In RenderObjects
            [Object].Render()
        Next

        Animations.Draw(CType(Me.PreScreen, BattleSystem.BattleScreen))

        World.DrawWeather(Screen.Level.World.CurrentMapWeather)

        TextBox.Draw()
    End Sub

    Private Sub UpdateAnimations()
        Animations.Update(CType(Me.PreScreen, BattleSystem.BattleScreen))
    End Sub

    Private Sub SetCamera()
        Camera.Position = New Vector3(BattleScreen.OppPokemonNPC.Position.X - 2.5F, BattleScreen.OppPokemonNPC.Position.Y + 0.25F, BattleScreen.OppPokemonNPC.Position.Z + 0.5F) - BattleScreen.BattleMapOffset
        Camera.Pitch = -0.25F
        Camera.Yaw = MathHelper.Pi * 1.5F + 0.25F
    End Sub

    Dim _playIntroSound As Boolean = False

    Public Overrides Sub Update()
        Lighting.UpdateLighting(Screen.Effect)
        If textboxStart = False Then
            textboxStart = True
            TextBox.Show(Core.Player.Name & " used a " & Ball.Name & "!", {}, False, False)
        End If
        TextBox.Update()

        SkyDome.Update()

        Level.Update()
        SetCamera()

        BattleScreen.OppPokemonNPC.UpdateEntity()

        CType(Camera, BattleSystem.BattleCamera).UpdateMatrices()
        CType(Camera, BattleSystem.BattleCamera).UpdateFrustum()

        If TextBox.Showing = False Then

            UpdateAnimations()

            If Me.IsCurrentScreen() = True Then
                If AnimationStarted = False Then
                    If Me._playIntroSound = False Then
                        Me._playIntroSound = True
                        SoundManager.PlaySound("Battle\Pokeball\throw")
                    End If
                    BallEntity = Animations.SpawnEntity(BallStartPosition, Ball.Texture, New Vector3(0.3F), 1.0F)
                    SetupAnimation()
                Else
                    Select Case Me.AnimationIndex
                        Case 0
                            SoundManager.PlaySound("Battle\Pokeball\open")
                            InBall = True
                            AnimationIndex = 1
                            AnimationStarted = False
                            SetupAnimation()
                        Case 1
                            AnimationIndex = 2
                            AnimationStarted = False
                            SetupAnimation()
                        Case 2, 3, 4, 5
                            If StayInBall() = True Then
                                SoundManager.PlaySound("Battle\Pokeball\shake")
                                AnimationIndex += 1
                            Else
                                SoundManager.PlaySound("Battle\Pokeball\break")
                                AnimationIndex = 21
                                InBall = False
                            End If
                            AnimationStarted = False
                            SetupAnimation()
                        Case 6
                            AnimationIndex = 7
                            AnimationStarted = False
                            SetupAnimation()
                        Case 7
                            AnimationIndex = 8
                            AnimationStarted = False
                            SetupAnimation()
                            CatchPokemon()
                            BattleSystem.Battle.Caught = True
                        Case 8
                            AnimationIndex = 9
                            If showPokedexEntry = True Then
                                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New PokedexViewScreen(Core.CurrentScreen, p, True), Color.White, False))
                            End If
                        Case 9
                            AnimationIndex = 10
                            Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, p))
                        Case 10 ' After Catch
                            If p.CatchBall.ID = 186 Then
                                p.FullRestore() ' Heal Ball
                            End If

                            PlayerStatistics.Track("Caught Pokemon", 1)
                            StorePokemon()
                            AnimationIndex = 11
                        Case 11
                            Core.SetScreen(Me.PreScreen)
                            BattleSystem.Battle.Won = True
                            CType(Core.CurrentScreen, BattleSystem.BattleScreen).EndBattle(False)
                        Case 20 ' Failed
                            If Core.Player.Pokemons.Count < 6 Then
                                Dim p As Pokemon = BattleScreen.OppPokemon
                                p.SetCatchInfos(Me.Ball, "Illegally caught!")

                                Core.Player.Pokemons.Add(p)
                            End If
                            ResetVisibility()
                            Core.SetScreen(Me.PreScreen)
                        Case 21 ' After Break
                            SetupAnimation()
                            Core.SetScreen(Me.PreScreen)
                            CType(Core.CurrentScreen, BattleSystem.BattleScreen).Battle.InitializeRound(CType(Core.CurrentScreen, BattleSystem.BattleScreen), New BattleSystem.Battle.RoundConst() With {.StepType = BattleSystem.Battle.RoundConst.StepTypes.Text, .Argument = "It broke free!"})
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub ResetVisibility()
        BattleScreen.OppPokemonModel.Visible = ModelVisible
        BattleScreen.OppPokemonNPC.Visible = SpriteVisible
    End Sub

    Private Sub CatchPokemon()
        p.ResetTemp()

        Dim s As String = "Gotcha!~" & p.GetName() & " was caught!"

        If Core.Player.HasPokedex = True Then
            If Pokedex.GetEntryType(Core.Player.PokedexData, p.Number) < 2 Then
                s &= "*" & p.GetName() & "'s data was~added to the Pokédex."
                showPokedexEntry = True
            End If
        End If

        If p.IsShiny = True Then
            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 3)
        Else
            If Pokedex.GetEntryType(Core.Player.PokedexData, p.Number) < 3 Then
                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 2)
            End If
        End If

        p.SetCatchInfos(Me.Ball, "caught at")

        MusicManager.Pause()
        MusicManager.Play("wild_defeat", False, 0.0F)
        SoundManager.PlaySound("success_catch", True)
        TextBox.Show(s, {}, False, False)

    End Sub

    Private Sub StorePokemon()
        Dim s As String = ""

        If Core.Player.Pokemons.Count < 6 Then
            If BattleScreen.BattleMode = BattleSystem.BattleScreen.BattleModes.BugContest And Core.Player.Pokemons(Core.Player.Pokemons.Count - 1).CatchBall.ID = 177 And Core.Player.Pokemons.Count > 1 Then
                Core.Player.Pokemons.RemoveAt(Core.Player.Pokemons.Count - 1)
            ElseIf BattleScreen.BattleMode = BattleSystem.BattleScreen.BattleModes.BugContest And Core.Player.Pokemons(0).CatchBall.ID = 177 And Core.Player.Pokemons.Count > 1 Then
                Core.Player.Pokemons.RemoveAt(0)
            End If

            Core.Player.Pokemons.Add(p)
        Else
            Dim boxName As String = StorageSystemScreen.GetBoxName(StorageSystemScreen.DepositPokemon(p, Player.Temp.PCBoxIndex))

            s = "It was transfered to Box~""" & boxName & """~on your PC."
        End If

        If p.IsShiny = True Then
            If p.Number <> 130 Then
                GameJolt.Emblem.AchieveEmblem("stars")
            End If
        End If

        Core.Player.AddPoints(3, "Caught Pokémon.")

        If s <> "" Then
            TextBox.Show(s)
        End If
    End Sub

    Private Sub SetupAnimation()
        Me.AnimationStarted = True

        Select Case Me.AnimationIndex
            Case 0
                PokemonScale = BattleScreen.OppPokemonNPC.Scale
                Animations.AnimationMove(BallEntity, False, BattleScreen.OppPokemonNPC.Position.X - 0.05F, 0.0F, BattleScreen.OppPokemonNPC.Position.Z, 0.1, False, True, 0F, 0F,, 0.3)
            Case 1
                Dim SmokeParticles As Integer = 0
                Do
                    Dim SmokePosition = BattleScreen.OwnPokemonNPC.Position + New Vector3(CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10))
                    Dim SmokeDestination = BattleScreen.OwnPokemonNPC.Position

                    Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                    Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                    Dim SmokeSpeed = CSng(Random.Next(1, 3) / 10.0F)

                    Dim SmokeEntity As Entity = Animations.SpawnEntity(SmokePosition, SmokeTexture, SmokeScale, 1.0F)

                    Animations.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 0.0F, 0.0F)

                    Threading.Interlocked.Increment(SmokeParticles)
                Loop While SmokeParticles <= 38

                Animations.AnimationMove(BallEntity, False, BattleScreen.OppPokemonNPC.Position.X, BattleScreen.OwnPokemonNPC.Position.Y, BattleScreen.OppPokemonNPC.Position.Z, 0.01F, False, False, 0.0F, 6.0F,,, 3)

                Animations.AnimationScale(Nothing, False, False, 0.05F, 0.05F, 0.05F, 0.02F, 0.0F, 0.0F, "1")
                Animations.AnimationFade(Nothing, False, 1, False, 0.0F, 0.0F, 0.0F)
            Case 2
                Animations.AnimationMove(BallEntity, False, BattleScreen.OppPokemonNPC.Position.X, BattleScreen.OwnPokemonNPC.Position.Y - 0.35F, BattleScreen.OppPokemonNPC.Position.Z, 0.02F, False, False, 0.0F, 6.0F,,, 3)
            Case 3, 5
                Animations.AnimationRotate(BallEntity, False, 0, 0, 0.05F, 0, 0, 1.0F, 0.0F, 4.0F, False, False, True, True)
            Case 4, 6
                Animations.AnimationRotate(BallEntity, False, 0, 0, -0.05F, 0, 0, -1.0F, 0.0F, 4.0F, False, False, True, True)
            Case 7 ' Catch Animation
                For i = 0 To 2
                    Dim StarPosition As Vector3 = New Vector3(BattleScreen.OppPokemonNPC.Position.X - 0.05F, -0.35F, BattleScreen.OppPokemonNPC.Position.Z)
                    Dim StarDestination As Vector3 = New Vector3(StarPosition.X, StarPosition.Y + 0.4F, StarPosition.Z - ((1 - i) * 0.4F))
                    Dim StarEntity As Entity = Animations.SpawnEntity(StarPosition, TextureManager.GetTexture("Textures\Battle\BallCatchStar"), New Vector3(0.1F), 1.0F)
                    Animations.AnimationMove(StarEntity, True, StarDestination.X, StarDestination.Y, StarDestination.Z, 0.01F, False, False, 0.0F, 0.0F,,, 3)
                Next
                Animations.AnimationMove(BallEntity, False, BattleScreen.OppPokemonNPC.Position.X, BattleScreen.OwnPokemonNPC.Position.Y - 0.35F, BattleScreen.OppPokemonNPC.Position.Z, 0.02F, False, False, 0.0F, 6.0F,,, 3)
            Case 8
                Animations.AnimationFade(BallEntity, True, 0.01F, False, 0.0F, 0.0F, 0.0F)
            Case 21 ' Break Animation
                ' Ball Opens
                Dim SmokeParticles As Integer = 0
                Do
                    Dim SmokePosition = BattleScreen.OwnPokemonNPC.Position
                    Dim SmokeDestination = BattleScreen.OwnPokemonNPC.Position + New Vector3(CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10))

                    Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                    Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                    Dim SmokeSpeed = CSng(Random.Next(1, 3) / 10.0F)

                    Dim SmokeEntity As Entity = Animations.SpawnEntity(SmokePosition, SmokeTexture, SmokeScale, 1.0F)

                    Animations.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 0.0F, 0.0F)

                    Threading.Interlocked.Increment(SmokeParticles)
                Loop While SmokeParticles <= 38

                ' Pokemon appears
                Animations.AnimationFade(Nothing, False, 1, False, 1, 0.0F, 0.0F)
                Animations.AnimationScale(Nothing, False, True, PokemonScale.X, PokemonScale.Y, PokemonScale.Z, 0.02F, 0.0F, 0.0F, "1")

        End Select
    End Sub

    Private Function StayInBall() As Boolean
        Dim cp As Pokemon = p
        Dim MaxHP As Integer = cp.MaxHP
        Dim CurrentHP As Integer = cp.HP
        Dim CatchRate As Integer = cp.CatchRate
        Dim BallRate As Single = Ball.CatchMultiplier
        Dim PokemonStartFriendship As Integer = cp.Friendship

        Select Case Ball.Name.ToLower()
            Case "repeat ball"
                If Pokedex.GetEntryType(Core.Player.PokedexData, cp.Number) > 1 Then
                    BallRate = 2.5F
                End If
            Case "nest ball"
                BallRate = CSng((41 - cp.Level) / 10)
                BallRate = CInt(MathHelper.Clamp(BallRate, 1, 4))
            Case "net ball"
                If cp.IsType(Element.Types.Bug) = True Or cp.IsType(Element.Types.Water) = True Then
                    BallRate = 3.5F
                End If
            Case "dive ball"
                If BattleSystem.BattleScreen.DiveBattle = True Then
                    BallRate = 3.5F
                End If
			Case "lure ball"
                If BattleSystem.BattleScreen.DiveBattle = True Then
                    BallRate = 5.0F
                End If
            Case "dusk ball"
                If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Cave Or Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Dark Then
                    BallRate = 3.5F
                ElseIf Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside And World.GetTime() = 0 Then
                    BallRate = 3.5F
                End If
            Case "fast ball"
                If cp.BaseSpeed >= 100 Then
                    BallRate = 4.0F
                End If
            Case "level ball"
                If CInt(Math.Floor(BattleScreen.OwnPokemon.Level / 4)) > cp.Level Then
                    BallRate = 8.0F
                ElseIf CInt(Math.Floor(BattleScreen.OwnPokemon.Level / 2)) > cp.Level Then
                    BallRate = 4.0F
                ElseIf BattleScreen.OwnPokemon.Level > cp.Level Then
                    BallRate = 2.0F
                End If
            Case "love ball"
                If BattleScreen.OwnPokemon.Number = cp.Number And BattleScreen.OwnPokemon.Gender <> cp.Gender Then
                    BallRate = 8.0F
                End If
            Case "moon ball"
                For Each ev As EvolutionCondition In cp.EvolutionConditions
                    For Each con As EvolutionCondition.Condition In ev.Conditions
                        If con.ConditionType = EvolutionCondition.ConditionTypes.Item And con.Argument = "8" And ev.Trigger = EvolutionCondition.EvolutionTrigger.ItemUse Then
                            BallRate = 4.0F
                            Exit For
                        End If
                    Next

                    If BallRate = 4.0F Then
                        Exit For
                    End If
                Next
            Case "heavy ball"
                Dim weight As Single = cp.PokedexEntry.Weight

                If weight > 451.5F And weight < 677.3F Then
                    BallRate = 2.0F
                ElseIf weight > 677.3F And weight < 903.0F Then
                    BallRate = 3.0F
                ElseIf weight > 903.0F Then
                    BallRate = 4.0F
                End If
            Case "friend ball"
                cp.Friendship = 200
            Case "quick ball"
                If BattleScreen.FieldEffects.Rounds < 2 Then
                    BallRate = 5.0F
                End If
            Case "timer ball"
                BallRate = CInt(1 + BattleScreen.FieldEffects.Rounds * 0.3).Clamp(1, 4)
            Case "dream ball"
                If cp.Status = Pokemon.StatusProblems.Sleep Then
                    BallRate = 3.0F
                End If
        End Select

        Dim Status As Single = 1.0F
        Select Case cp.Status
            Case Pokemon.StatusProblems.Poison, Pokemon.StatusProblems.BadPoison, Pokemon.StatusProblems.Burn, Pokemon.StatusProblems.Paralyzed
                Status = 1.5F
            Case Pokemon.StatusProblems.Sleep, Pokemon.StatusProblems.Freeze
                Status = 2.5F
        End Select

        Dim CaptureRate As Integer = CInt(Math.Floor(((1 + (MaxHP * 3 - CurrentHP * 2) * CatchRate * BallRate * Status) / (MaxHP * 3))))

        If BattleScreen.PokemonSafariStatus < 0 Then
            For i = 1 To BattleScreen.PokemonSafariStatus
                CaptureRate *= 2
            Next
        ElseIf BattleScreen.PokemonSafariStatus > 0 Then
            For i = 1 To BattleScreen.PokemonSafariStatus.ToPositive()
                CaptureRate = CInt(CaptureRate / 2)
            Next
        End If

        If CaptureRate <= 0 Then
            CaptureRate = 1
        End If

        Dim B As Integer = CInt(1048560 / Math.Sqrt(Math.Sqrt(16711680 / CaptureRate)))
        Dim R As Integer = Core.Random.Next(0, 65535 + 1)

        If R > B Then
            cp.Friendship = PokemonStartFriendship
            Return False
        Else
            Return True
        End If
    End Function

End Class