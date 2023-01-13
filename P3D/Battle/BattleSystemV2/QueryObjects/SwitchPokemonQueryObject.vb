Namespace BattleSystem

    Public Class SwitchPokemonQueryObject

        Inherits QueryObject

        Dim _ready As Boolean = False

#Region "TextStuff"

        Dim _text As String = ""
        Dim _textColor As Color = Color.White

        Dim _textIndex As Integer = 0
        Dim _textDelay As Single = 0.015F

        Private ReadOnly Property TextReady() As Boolean
            Get
                If _textIndex < Me._text.Length Then
                    Return False
                End If
                Return True
            End Get
        End Property

        Private Sub TransformText(ByVal text As String)
            Me._text = text

            Me._text = Me._text.Replace("*", " ")
            Me._text = Me._text.Replace("~", " ")
            Me._text = Me._text.Replace("<player.name>", Core.Player.Name)
            Me._text = Me._text.Replace("<playername>", Core.Player.Name)
            Me._text = Me._text.Replace("<rival.name>", Core.Player.RivalName)
            Me._text = Me._text.Replace("<rivalname>", Core.Player.RivalName)
            Me._text = Me._text.Replace("[POKE]", "Poké")
        End Sub

        Private Sub UpdateText()
            If Me.TextReady = False Then
                Me._textDelay -= 0.01F
                If Me._textDelay <= 0.0F Then
                    Me._textDelay = 0.015F
                    Me._textIndex += 1
                End If
                If Controls.Accept(True, True) = True And Me._textIndex > 2 Then
                    Me._textIndex = Me._text.Length
                End If
            End If
        End Sub

        Private Sub DrawText(BV2Screen As BattleScreen)
            Dim rec As New Rectangle(100, Core.windowSize.Height - 250, Core.windowSize.Width - 200, 200)

            Canvas.DrawRectangle(rec, New Color(0, 0, 0, 150))

            Dim text As String = Me._text.Substring(0, _textIndex)
            text = text.CropStringToWidth(FontManager.InGameFont, Core.windowSize.Width - 300)

            Core.SpriteBatch.DrawString(FontManager.InGameFont, text, New Vector2(rec.X + 20, rec.Y + 20), Color.White)

            If GamePad.GetState(PlayerIndex.One).IsConnected = True And Core.GameOptions.GamePadEnabled = True And BV2Screen.IsCurrentScreen() = True Then
                Dim d As New Dictionary(Of Buttons, String)
                d.Add(Buttons.A, "OK")
                BV2Screen.DrawGamePadControls(d, New Vector2(rec.X + rec.Width - 100, rec.Y + rec.Height - 40))
            Else
                If TextReady = True Then
                    Core.SpriteBatch.DrawString(FontManager.InGameFont, "OK", New Vector2(rec.X + rec.Width - FontManager.InGameFont.MeasureString("OK").X - 20, rec.Y + rec.Height - FontManager.InGameFont.MeasureString("OK").Y - 5), Color.White)
                End If
            End If
        End Sub

#End Region

#Region "ChooseStuff"

        Dim _chooseIndex As Integer = 0

        Private Sub UpdateChoose()
            If Controls.Down(True, True, True, True, True) = True Then
                _chooseIndex += 1
            End If
            If Controls.Up(True, True, True, True, True) = True Then
                _chooseIndex -= 1
            End If

            _chooseIndex = _chooseIndex.Clamp(0, 1)

            Dim rec As New Rectangle(Core.windowSize.Width - 250, Core.windowSize.Height - 450, 150, 150)
            If rec.Contains(MouseHandler.MousePosition) = False Then
                If Controls.Accept(True, True, True) = True Then
                    If _chooseIndex = 0 Then
                        Dim selScreen = New PartyScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf ChoosePokemon, Localization.GetString("battle_main_choose_pokemon"), False) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf ChoosePokemonHandler
                        SoundManager.PlaySound("select")
                        Core.SetScreen(selScreen)
                    Else
                        SoundManager.PlaySound("select")
                        _ready = True
                    End If
                End If
            Else
                If Controls.Accept(False, True, True) = True Then
                    If _chooseIndex = 0 Then
                        Dim selScreen = New PartyScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf ChoosePokemon, Localization.GetString("battle_main_choose_pokemon"), False) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf ChoosePokemonHandler
                        SoundManager.PlaySound("select")
                        Core.SetScreen(selScreen)
                    Else
                        SoundManager.PlaySound("select")
                        _ready = True
                    End If
                End If
                If Controls.Accept(True, False, False) Then
                    If New Rectangle(Core.windowSize.Width - 213, Core.windowSize.Height - 438, 80, 50).Contains(MouseHandler.MousePosition) Then
                        _chooseIndex = 0
                        Dim selScreen = New PartyScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf ChoosePokemon, Localization.GetString("battle_main_choose_pokemon"), False) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf ChoosePokemonHandler
                        SoundManager.PlaySound("select")
                        Core.SetScreen(selScreen)
                    End If
                    If New Rectangle(Core.windowSize.Width - 213, Core.windowSize.Height - 378, 80, 50).Contains(MouseHandler.MousePosition) Then
                        _chooseIndex = 1
                        SoundManager.PlaySound("select")
                        TempScreen.BattleQuery.Clear()
                        FinishOppSwitch(TempScreen)
                        Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
                        Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)
                        cq2.PassThis = True
                        TempScreen.BattleQuery.AddRange({cq1, cq2})
                        TempScreen.Battle.StartRound(TempScreen)
                        _ready = True
                    End If
                End If
                End If
            If Controls.Dismiss(True, True, True) = True Then
                SoundManager.PlaySound("select")
                TempScreen.BattleQuery.Clear()
                FinishOppSwitch(TempScreen)
                Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
                Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)
                cq2.PassThis = True
                TempScreen.BattleQuery.AddRange({cq1, cq2})
                TempScreen.Battle.StartRound(TempScreen)
                _ready = True
            End If
        End Sub

        Private Sub DrawChoose()
            Dim rec As New Rectangle(Core.windowSize.Width - 250, Core.windowSize.Height - 450, 150, 150)

            Canvas.DrawRectangle(rec, New Color(0, 0, 0, 150))

            If _chooseIndex = 0 Then
                Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 213, Core.windowSize.Height - 438, 80, 50), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("global_yes"), New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 430), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("global_no"), New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 370), Color.White)
            Else
                Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 213, Core.windowSize.Height - 378, 80, 50), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("global_yes"), New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 430), Color.White)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("global_no"), New Vector2(Core.windowSize.Width - 200, Core.windowSize.Height - 370), Color.Black)
            End If

        End Sub

        Dim TempScreen As BattleScreen

        Private Sub ChoosePokemonHandler(ByVal params As Object())
            ChoosePokemon(CInt(params(0)))
        End Sub

        Private Sub ChoosePokemon(ByVal PokeIndex As Integer)
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)
            If PokeIndex = TempScreen.OwnPokemonIndex Then
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is already~in battle!", {}, True, False)
            Else
                If Pokemon.IsEgg() = False Then
                    If Pokemon.Status <> P3D.Pokemon.StatusProblems.Fainted Then
                        If BattleCalculation.CanSwitch(TempScreen, True) = False Then
                            Screen.TextBox.Show(Localization.GetString("battle_main_cannot_switch", "Cannot switch out."), {}, True, False)
                        Else
                            Dim TempQuery = TempScreen.BattleQuery.ToArray
                            If TempScreen.OwnPokemonIndex <> PokeIndex Then
                                TempScreen.ParticipatedPokemon.Clear()
                                TempScreen.ParticipatedPokemon.Add(PokeIndex)
                                If TempScreen.IsRemoteBattle = True And TempScreen.IsHost = False Then
                                    TempScreen.OppFaint = False
                                    TempScreen.OwnStatistics.Switches += 1
                                    TempScreen.BattleQuery.Clear()
                                    TempScreen.Battle.SwitchOutOwn(TempScreen, PokeIndex, TempScreen.BattleQuery.Count)
                                    TempScreen.BattleQuery.Reverse()
                                    FinishOppSwitch(TempScreen)
                                    Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
                                    Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)
                                    cq2.PassThis = True
                                    TempScreen.BattleQuery.AddRange({cq1, cq2})
                                    TempScreen.HasSwitchedOwn = True
                                    TempScreen.Battle.StartRound(TempScreen)
                                Else
                                    TempScreen.BattleQuery.Clear()
                                    TempScreen.Battle.SwitchOutOwn(TempScreen, PokeIndex, TempScreen.BattleQuery.Count)
                                    TempScreen.BattleQuery.Reverse()
                                    FinishOppSwitch(TempScreen)
                                    Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
                                    Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)
                                    cq2.PassThis = True
                                    TempScreen.BattleQuery.AddRange({cq1, cq2})
                                    TempScreen.HasSwitchedOwn = True
                                    TempScreen.Battle.StartRound(TempScreen)
                                End If
                                Me._ready = True
                            End If
                        End If
                    Else
                        Screen.TextBox.Show(Pokemon.GetDisplayName() & " is fainted!", {}, True, False)
                    End If
                Else
                    Screen.TextBox.Show("Cannot switch in~the egg!", {}, True, False)
                End If
            End If
        End Sub

#End Region

        Public Sub New(ByVal BattleScreen As BattleScreen, ByVal NewPokemon As Pokemon)
            MyBase.New(QueryTypes.SwitchPokemon)

            Me.TempScreen = BattleScreen
            TransformText(BattleScreen.Trainer.Name & " " & Localization.GetString("battle_main_trainer_sent_out_3") & " " & NewPokemon.GetDisplayName() & Localization.GetString("battle_main_trainer_sent_out_4"))
        End Sub

        Dim delay As Single = 2.0F

        Public Sub FinishOppSwitch(BattleScreen As BattleScreen)
            BattleScreen.OppPokemonNPC.Position.Y = 0
            BattleScreen.Battle.ChangeCameraAngle(1, False, BattleScreen)
            Dim oppModel As String = BattleScreen.GetModelName(False)
            If oppModel = "" Then
                BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, PokemonForms.GetOverworldSpriteName(BattleScreen.OppPokemon), -1, -1, 0, 1))
            Else
                BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(False, oppModel, -1, -1, 1, 0))
            End If

            BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, 1, -1, -1, -1, -1))
            BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.Name & ": ""Go, " & BattleScreen.OppPokemon.GetDisplayName() & "!"""))

            Dim BallThrow As AnimationQueryObject = New AnimationQueryObject(BattleScreen.OppPokemonNPC, False)
            If Core.Player.ShowBattleAnimations <> 0 Then
                ' Ball is thrown
                BallThrow.AnimationMove(Nothing, False, 0, 0.5, 0, 0.5, False, False, 0, 0,,, 3)

                BallThrow.AnimationPlaySound("Battle\Pokeball\Throw", 0, 0)
                Dim BallThrowEntity = BallThrow.SpawnEntity(New Vector3(2, -0.15, 0), BattleScreen.OppPokemon.CatchBall.Texture, New Vector3(0.3F), 1.0F)
                BallThrow.AnimationMove(BallThrowEntity, True, 0, 0.35, 0, 0.1, False, True, 0F, 0.5F,, 0.3,, 0.025F)

                ' Ball opens
                BallThrow.AnimationPlaySound("Battle\Pokeball\Open", 3, 0)
                Dim SmokeSpawned As Integer = 0
                Do
                    Dim SmokePosition = New Vector3(0, 0.35, 0)
                    Dim SmokeDestination = New Vector3(CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10), CSng(Random.Next(-10, 10) / 10))

                    Dim SmokeTexture As Texture2D = TextureManager.GetTexture("Textures\Battle\Smoke")

                    Dim SmokeScale = New Vector3(CSng(Random.Next(2, 6) / 10))
                    Dim SmokeSpeed = CSng(Random.Next(1, 3) / 20.0F)
                    Dim SmokeEntity = BallThrow.SpawnEntity(SmokePosition, SmokeTexture, SmokeScale, 1, 3)

                    BallThrow.AnimationMove(SmokeEntity, True, SmokeDestination.X, SmokeDestination.Y, SmokeDestination.Z, SmokeSpeed, False, False, 3.0F, 0.0F)
                    Threading.Interlocked.Increment(SmokeSpawned)
                Loop While SmokeSpawned <= 38
            End If

            If Core.Player.ShowBattleAnimations <> 0 Then
                ' Pokemon appears
                BallThrow.AnimationFade(Nothing, False, 1, True, 1, 3, 0)
                BallThrow.AnimationPlaySound(CStr(BattleScreen.OppPokemon.Number), 4, 0,, True)
                '  Pokémon falls down
                BallThrow.AnimationMove(Nothing, False, 0, 0, 0, 0.05F, False, False, 5, 0)
            Else
                ' Pokemon appears
                BallThrow.AnimationFade(Nothing, False, 1, True, 1, 0, 0)
                BallThrow.AnimationPlaySound(CStr(BattleScreen.OppPokemon.Number), 0, 0,, True)
            End If
            BattleScreen.BattleQuery.Add(BallThrow)

            With BattleScreen
                Dim p As Pokemon = .OppPokemon
                Dim op As Pokemon = .OwnPokemon

                Dim spikeAffected As Boolean = True
                Dim rockAffected As Boolean = True

                spikeAffected = BattleScreen.FieldEffects.IsGrounded(False, BattleScreen)

                If spikeAffected = True Then
                    If .FieldEffects.OwnSpikes > 0 And p.Ability.Name.ToLower() <> "magic guard" Then
                        Dim spikeDamage As Double = 1D
                        Select Case .FieldEffects.OwnSpikes
                            Case 1
                                spikeDamage = (p.MaxHP / 100) * 12.5D
                            Case 2
                                spikeDamage = (p.MaxHP / 100) * 16.7D
                            Case 3
                                spikeDamage = (p.MaxHP / 100) * 25D
                        End Select
                        BattleScreen.Battle.ReduceHP(CInt(spikeDamage), False, True, BattleScreen, "The Spikes hurt " & p.GetDisplayName() & "!", "spikes")
                    End If
                End If
                'Sticky Web
                If spikeAffected = True Then
                    If .FieldEffects.OwnStickyWeb > 0 Then
                        BattleScreen.Battle.LowerStat(False, False, BattleScreen, "Speed", 1, "The opposing pokemon was caught in a Sticky Web!", "stickyweb")
                    End If
                End If
                If spikeAffected = True Then
                    If .FieldEffects.OwnToxicSpikes > 0 And p.Status = Pokemon.StatusProblems.None And p.Type1.Type <> Element.Types.Poison And p.Type2.Type <> Element.Types.Poison Then
                        Select Case .FieldEffects.OwnToxicSpikes
                            Case 1
                                BattleScreen.Battle.InflictPoison(False, True, BattleScreen, False, "The Toxic Spikes hurt " & p.GetDisplayName() & "!", "toxicspikes")
                            Case 2
                                BattleScreen.Battle.InflictPoison(False, True, BattleScreen, True, "The Toxic Spikes hurt " & p.GetDisplayName() & "!", "toxicspikes")
                        End Select
                    End If
                    If .FieldEffects.OwnToxicSpikes > 0 Then
                        If p.Type1.Type = Element.Types.Poison Or p.Type2.Type = Element.Types.Poison Then
                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " removed the Toxic Spikes!"))
                            .FieldEffects.OwnToxicSpikes = 0
                        End If
                    End If
                End If

                If rockAffected = True Then
                    If .FieldEffects.OwnStealthRock > 0 And p.Ability.Name.ToLower() <> "magic guard" Then
                        Dim rocksDamage As Double = 1D

                        Dim effectiveness As Single = BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(New Element(Element.Types.Rock), p.Type1)) * BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(New Element(Element.Types.Rock), p.Type2))
                        Select Case effectiveness
                            Case 0.25F
                                rocksDamage = (p.MaxHP / 100) * 3.125D
                            Case 0.5F
                                rocksDamage = (p.MaxHP / 100) * 6.25D
                            Case 1.0F
                                rocksDamage = (p.MaxHP / 100) * 12.5D
                            Case 2.0F
                                rocksDamage = (p.MaxHP / 100) * 25D
                            Case 4.0F
                                rocksDamage = (p.MaxHP / 100) * 50D
                        End Select

                        BattleScreen.Battle.ReduceHP(CInt(rocksDamage), False, True, BattleScreen, "The Stealth Rocks hurt " & p.GetDisplayName() & "!", "stealthrocks")
                    End If
                End If

                BattleScreen.Battle.TriggerAbilityEffect(BattleScreen, False)
                BattleScreen.Battle.TriggerItemEffect(BattleScreen, False)

                If .OppPokemon.Status = Pokemon.StatusProblems.Sleep Then
                    .FieldEffects.OppSleepTurns = Core.Random.Next(1, 4)
                End If

                If BattleScreen.FieldEffects.OppHealingWish = True Then
                    BattleScreen.FieldEffects.OppHealingWish = False

                    If .OppPokemon.HP < .OppPokemon.MaxHP Or .OppPokemon.Status <> Pokemon.StatusProblems.None Then
                        BattleScreen.Battle.GainHP(.OppPokemon.MaxHP - .OppPokemon.HP, False, False, BattleScreen, "The Healing Wish came true for " & .OppPokemon.GetDisplayName() & "!", "move:healingwish")
                        BattleScreen.Battle.CureStatusProblem(False, False, BattleScreen, "", "move:healingwish")
                    End If
                End If
            End With
        End Sub
        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If TextReady = False Then
                UpdateText()
            Else
                delay -= 0.1F
                If delay <= 0.0F Then
                    delay = 0.0F
                End If

                If delay = 0.0F And BV2Screen.IsCurrentScreen() = True Then
                    UpdateChoose()
                End If
            End If
        End Sub

        Public Overrides Sub Draw(BV2Screen As BattleScreen)
            If Me._ready = False Then
                DrawText(BV2Screen)
                If TextReady = True Then
                    DrawChoose()
                End If
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return Me._ready
            End Get
        End Property

    End Class

End Namespace