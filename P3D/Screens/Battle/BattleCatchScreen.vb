Public Class BattleCatchScreen

    Inherits Screen

    Dim Ball As Item

    Dim PokemonScale As Vector3

    Dim AnimationIndex As Integer = 0
    Dim InBall As Boolean = False

    Dim textboxStart As Boolean = False
    Dim showPokedexEntry As Boolean = False

    Dim boxData As String = ""
    Dim sentToBox As Boolean = False

    Dim p As Pokemon

    Dim SpriteVisible As Boolean = False

    Dim BattleScreen As BattleSystem.BattleScreen
    Dim AnimationHasStarted As Boolean = False
    Dim AnimationList As New List(Of BattleSystem.AnimationQueryObject)

    Public Sub New(ByVal BattleScreen As BattleSystem.BattleScreen, ByVal Ball As Item)
        Me.Identification = Identifications.BattleCatchScreen

        Me.Ball = Ball
        Me.PreScreen = BattleScreen
        Me.UpdateFadeIn = True

        Me.BattleScreen = BattleScreen
        p = BattleScreen.OppPokemon

        Me.SpriteVisible = BattleScreen.OppPokemonNPC.Visible

        SetCamera()
    End Sub

    Public Overrides Sub Draw()
        SkyDome.Draw(45.0F)

        Level.Draw()

        Dim RenderObjects As New List(Of Entity)

        RenderObjects.Add(BattleScreen.OppPokemonNPC)

        If RenderObjects.Count > 0 Then
            RenderObjects = (From r In RenderObjects Order By r.CameraDistance Descending).ToList()
        End If

        For Each [Object] As Entity In RenderObjects
            [Object].Render()
        Next

        If AnimationList.Count > 0 Then
            Dim cIndex As Integer = 0
            Dim cQuery As New List(Of BattleSystem.AnimationQueryObject)
nextIndex:
            If AnimationList.Count > cIndex Then
                Dim cQueryObject As BattleSystem.AnimationQueryObject = AnimationList(cIndex)
                cQuery.Add(cQueryObject)

                If cQueryObject.PassThis = True Then
                    cIndex += 1
                    GoTo nextIndex
                End If
            End If

            cQuery.Reverse()

            For Each cQueryObject As BattleSystem.AnimationQueryObject In cQuery
                cQueryObject.Draw(BattleScreen)
            Next
        End If

        World.DrawWeather(Screen.Level.World.CurrentMapWeather)

        TextBox.Draw()
    End Sub

    Public Sub UpdateAnimations()
        Dim cIndex As Integer = 0
nextIndex:
        If AnimationList.Count > cIndex Then
            Dim cQueryObject As BattleSystem.QueryObject = AnimationList(cIndex)

            cQueryObject.Update(BattleScreen)

            If cQueryObject.IsReady = True Then
                AnimationList.RemoveAt(cIndex)

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

        BattleScreen.OppPokemonNPC.UpdateEntity()

        CType(Camera, BattleSystem.BattleCamera).UpdateMatrices()
        CType(Camera, BattleSystem.BattleCamera).UpdateFrustum()
        If TextBox.Showing = False Then
            If Me.IsCurrentScreen() = True Then
                UpdateAnimations()
                Select Case AnimationIndex
                    Case 0
                        If AnimationHasStarted = False Then
                            Dim Shakes As List(Of Boolean) = New List(Of Boolean)
                            For i = 0 To 3
                                If StayInBall() = True Then
                                    Select Case i
                                        Case 0
                                            Shakes.Add(False)
                                        Case 1
                                            Shakes.Add(True)
                                        Case 2
                                            Shakes.Add(False)
                                        Case 3
                                            InBall = True
                                    End Select
                                Else
                                    Exit For
                                    InBall = False
                                End If
                            Next
                            If Core.Player.ShowBattleAnimations <> 0 AndAlso BattleScreen.IsPVPBattle = False Then


                                PokemonScale = BattleScreen.OppPokemonNPC.Scale
                                'Ball is thrown
                                Dim CatchAnimation = New BattleSystem.AnimationQueryObject(Nothing, False, Nothing)
                                CatchAnimation.AnimationPlaySound("Battle\Pokeball\Throw", 0, 0)

                                Dim BallPosition As Vector3 = New Vector3(BattleScreen.OppPokemonNPC.Position.X - 3, BattleScreen.OppPokemonNPC.Position.Y + 0.15F, BattleScreen.OppPokemonNPC.Position.Z)
                                Dim BallEntity As Entity = CatchAnimation.SpawnEntity(BallPosition, Ball.Texture, New Vector3(0.3F), 1.0F, 0, 0)

                                CatchAnimation.AnimationMove(BallEntity, False, 3, 0.1F, 0, 0.075, False, False, 0F, 0F,,,, 0.025)
                                CatchAnimation.AnimationRotate(BallEntity, False, 0, 0, -0.5, 0, 0, -6 * MathHelper.Pi, 0, 0, False, False, True, False)
                                CatchAnimation.AnimationRotate(BallEntity, False, 0, 0, 6 * MathHelper.Pi, 0, 0, 0, 4, 0, False, False, True, False)

                                ' Ball closes
                                CatchAnimation.AnimationPlaySound("Battle\Pokeball\Open", 3, 0)
                                Dim SmokeParticlesClose As Integer = 0
                                Do
                                    Dim SmokePosition = New Vector3(BattleScreen.OppPokemonNPC.Position.X + CSng(Random.Next(-10, 10) / 10), BattleScreen.OppPokemonNPC.Position.Y - 0.35F, BattleScreen.OppPokemonNPC.Position.Z + CSng(Random.Next(-10, 10) / 10))


                                    Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                                    Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                                    Dim SmokeSpeed = CSng(Random.Next(1, 3) / 25.0F)
                                    Dim SmokeEntity = CatchAnimation.SpawnEntity(SmokePosition, SmokeTexture, SmokeScale, 1, 3, 0)
                                    Dim SmokeDestination = New Vector3(BallEntity.Position.X - SmokePosition.X + 3, BallEntity.Position.Y - SmokePosition.Y, BallEntity.Position.Z - SmokePosition.Z - 0.05F)
                                    CatchAnimation.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 3, 0)

                                    Threading.Interlocked.Increment(SmokeParticlesClose)
                                Loop While SmokeParticlesClose <= 38
                                ' Pokémon Shrinks
                                CatchAnimation.AnimationScale(BattleScreen.OppPokemonNPC, False, False, 0.0F, 0.0F, 0.0F, 0.035F, 3, 0)

                                ' Ball falls
                                CatchAnimation.AnimationMove(BallEntity, False, 3, -0.35, 0, 0.1F, False, False, 8, 0)
                                CatchAnimation.AnimationPlaySound("Battle\Pokeball\Land", 9, 0)


                                For i = 0 To Shakes.Count - 1
                                    CatchAnimation.AnimationPlaySound("Battle\Pokeball\Shake", 12 + i * 10, 0)
                                    If Shakes(i) = False Then
                                        CatchAnimation.AnimationRotate(BallEntity, False, 0, 0, 0.15F, 0, 0, MathHelper.PiOver4, 12 + i * 10, 0, False, False, True, True)
                                    Else
                                        CatchAnimation.AnimationRotate(BallEntity, False, 0, 0, -0.15F, 0, 0, 0 - MathHelper.PiOver4, 12 + i * 10, 0, False, False, True, True)
                                    End If
                                Next

                                If InBall = True Then
                                    For i = 0 To 2
                                        Dim StarPosition As Vector3 = New Vector3(BattleScreen.OppPokemonNPC.Position.X + 0.05F, BattleScreen.OppPokemonNPC.Position.Y, BattleScreen.OppPokemonNPC.Position.Z)
                                        Dim StarDestination As Vector3 = New Vector3(0.05F, 0.65F, 0 - ((1 - i) * 0.4F))
                                        Dim StarEntity As Entity = CatchAnimation.SpawnEntity(StarPosition, TextureManager.GetTexture("Textures\Battle\BallCatchStar"), New Vector3(0.35F), 1.0F, 12 + Shakes.Count * 10)
                                        CatchAnimation.AnimationMove(StarEntity, True, StarDestination.X, StarDestination.Y, StarDestination.Z, 0.01F, False, False, 12 + Shakes.Count * 10, 0.0F,,, 3, 0.015F)
                                        CatchAnimation.AnimationPlaySound("Battle\Pokeball\Catch", 12 + Shakes.Count * 10, 4)
                                        CatchAnimation.AnimationFade(BallEntity, True, 0.01F, False, 0.0F, 12 + Shakes.Count * 10 + 3, 2)
                                    Next
                                Else
                                    CatchAnimation.AnimationFade(BallEntity, True, 1.0F, False, 0.0F, 12 + Shakes.Count * 10, 0)
                                    CatchAnimation.AnimationPlaySound("Battle\Pokeball\Break", 12 + Shakes.Count * 10, 0)
                                    ' Ball Opens
                                    Dim SmokeParticlesOpen As Integer = 0
                                    Do
                                        Dim SmokePosition = BattleScreen.OppPokemonNPC.Position
                                        Dim SmokeDestination = New Vector3(CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10))

                                        Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                                        Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                                        Dim SmokeSpeed = CSng(Random.Next(1, 3) / 25.0F)

                                        Dim SmokeEntity As Entity = CatchAnimation.SpawnEntity(SmokePosition, SmokeTexture, SmokeScale, 1.0F, 12 + Shakes.Count * 10, 0)

                                        CatchAnimation.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 12 + Shakes.Count * 10, 0)

                                        Threading.Interlocked.Increment(SmokeParticlesOpen)
                                    Loop While SmokeParticlesOpen <= 38

                                    ' Pokemon appears
                                    CatchAnimation.AnimationScale(BattleScreen.OppPokemonNPC, False, True, PokemonScale.X, PokemonScale.Y, PokemonScale.Z, 0.035F, 12 + Shakes.Count * 10, 0)

                                End If

                                AnimationList.Add(CatchAnimation)
                            End If
                            AnimationHasStarted = True
                        Else
                            If AnimationList.Count = 0 Then
                                AnimationIndex = 1
                            End If
                        End If
                    Case 1
                        ' After animation
                        If InBall = True Then
                            'Caught Pokémon
                            CatchPokemon()
                            BattleSystem.Battle.Caught = True
                            AnimationIndex = 2
                        Else
                            'Pokémon broke free
                            Core.SetScreen(Me.PreScreen)
                            CType(Core.CurrentScreen, BattleSystem.BattleScreen).Battle.InitializeRound(CType(Core.CurrentScreen, BattleSystem.BattleScreen), New BattleSystem.Battle.RoundConst() With {.StepType = BattleSystem.Battle.RoundConst.StepTypes.Text, .Argument = "It broke free!"})
                        End If
                    Case 2
                        If showPokedexEntry = True Then
                            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New PokedexViewScreen(Core.CurrentScreen, p, True), Color.White, False))
                        End If
                        AnimationIndex = 3
                    Case 3
                        Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, p))
                        AnimationIndex = 4
                    Case 4
                        If p.CatchBall.ID = 186 Then
                            p.FullRestore() ' Heal Ball
                        End If

                        PlayerStatistics.Track("Caught Pokemon", 1)
                        StorePokemon()
                        AnimationIndex = 5
                    Case 5
                        Core.SetScreen(Me.PreScreen)
                        BattleSystem.Battle.Won = True
                        CType(Core.CurrentScreen, BattleSystem.BattleScreen).EndBattle(False)

                End Select
            End If
        End If
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

            s = "It was transfered to Box~""" & boxName & """~on the PC."
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
