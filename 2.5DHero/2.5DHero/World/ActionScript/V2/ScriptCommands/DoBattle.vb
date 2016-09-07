Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @battle commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoBattle(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "starttrainer"
                    Dim t As New Trainer(argument)
                    If t.IsBeaten() = False Then
                        If ScriptV2.started = False Then
                            CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = True

                            If t.GetInSightMusic() <> "" And t.GetInSightMusic() <> "nomusic" Then
                                MusicManager.PlayMusic(t.GetInSightMusic(), True, 0.0F, 0.0F)
                            End If

                            If t.IntroMessage <> "" Then
                                Screen.TextBox.reDelay = 0.0F
                                Screen.TextBox.Show(t.IntroMessage, {})
                            End If
                            ScriptV2.started = True
                        End If
                        If Screen.TextBox.Showing = False Then
                            CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = False

                            Dim method As Integer = 0
                            If Screen.Level.Surfing = True Then
                                method = 2
                            End If

                            Dim b As New BattleSystem.BattleScreen(New Trainer(argument), Core.CurrentScreen, method)
                            Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))
                        End If
                    Else
                        Screen.TextBox.reDelay = 0.0F
                        Screen.TextBox.Show(t.DefeatMessage, {})

                        IsReady = True
                    End If

                    If Screen.TextBox.Showing = False Then
                        IsReady = True
                    End If

                    CanContinue = False
                Case "trainer"
                    Dim ID As String = argument

                    If argument.CountSeperators(",") > 0 Then
                        ID = argument.GetSplit(0)

                        For Each v As String In argument.Split(CChar(","))
                            Select Case v
                                Case "generate_pokemon_tower"
                                    Dim level As Integer = 0
                                    For Each p As Pokemon In Core.Player.Pokemons
                                        If p.Level > level Then
                                            level = p.Level
                                        End If
                                    Next

                                    While CStr(level)(CStr(level).Length - 1) <> "0"
                                        level += 1
                                    End While
                            End Select
                        Next
                    End If

                    Dim t As New Trainer(ID)

                    Dim method As Integer = 0
                    If Screen.Level.Surfing = True Then
                        method = 2
                    End If

                    Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, method)
                    Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))

                    IsReady = True

                    CanContinue = False
                Case "wild"
                    'ID,    Level,  [shiny],    [musicloop],    [introtype]
                    'int    int     -1 or bool  string          int

                    'optional:
                    '{pokemondata},             [musicloop],    [introtype]

                    Dim p As Pokemon = Nothing
                    Dim musicLoop As String = ""
                    Dim introType As Integer = Core.Random.Next(0, 10)

                    Dim method As Integer = 0
                    If Screen.Level.Surfing = True Then
                        method = 2
                    End If

                    If argument.StartsWith("{") = True And argument.Contains("}") = True Then
                        If argument.EndsWith("}") = True Then
                            p = Pokemon.GetPokemonByData(argument)
                        Else
                            Dim pokemonData As String = argument.Remove(argument.LastIndexOf("}") + 1)
                            p = Pokemon.GetPokemonByData(pokemonData)

                            argument = argument.Remove(0, argument.LastIndexOf("}") + 1)

                            If argument.Length > 1 And argument.StartsWith(",") = True Then
                                argument = argument.Remove(0, 1)
                                Dim args() As String = argument.Split(CChar(","))

                                For i = 0 To args.Length - 1
                                    Select Case i
                                        Case 0
                                            musicLoop = args(i)
                                        Case 1
                                            introType = int(args(i))
                                    End Select
                                Next
                            End If
                        End If
                    Else
                        If argument.Length > 0 Then
                            Dim ID As Integer = int(argument.GetSplit(0))
                            Dim Level As Integer = int(argument.GetSplit(1))

                            p = Pokemon.GetPokemonByID(ID)
                            p.Generate(Level, True)

                            Dim args() As String = argument.Split(CChar(","))

                            For i = 0 To args.Length - 1
                                Select Case i
                                    Case 2
                                        If args(i) <> "-1" Then
                                            p.IsShiny = CBool(args(i))
                                        End If
                                    Case 3
                                        musicLoop = args(i)
                                    Case 4
                                        introType = int(args(i))
                                End Select
                            Next
                        End If
                    End If

                    Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 1)
                    Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, method)
                    Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType, musicLoop))

                    IsReady = True

                    CanContinue = False
                Case "setvar"
                    Dim varname As String = argument.GetSplit(0)
                    Dim varvalue As String = argument.GetSplit(1)

                    Select Case varname.ToLower()
                        Case "canrun"
                            BattleSystem.BattleScreen.CanRun = CBool(varvalue)
                        Case "cancatch"
                            BattleSystem.BattleScreen.CanCatch = CBool(varvalue)
                        Case "canblackout"
                            BattleSystem.BattleScreen.CanBlackout = CBool(varvalue)
                        Case "canreceiveexp"
                            BattleSystem.BattleScreen.CanReceiveEXP = CBool(varvalue)
                        Case "canuseitems"
                            BattleSystem.BattleScreen.CanUseItems = CBool(varvalue)
                        Case "frontiertrainer"
                            Trainer.FrontierTrainer = int(varvalue)
                        Case "divebattle"
                            BattleSystem.BattleScreen.DiveBattle = CBool(varvalue)
                        Case "inversebattle"
                            BattleSystem.BattleScreen.IsInverseBattle = CBool(varvalue)
                        Case "custombattlemusic"
                            BattleSystem.BattleScreen.CustomBattleMusic = varvalue
                        Case "hiddenabilitychance"
                            Screen.Level.HiddenAbilityChance = int(varvalue)

                    End Select

                    IsReady = True
                Case "resetvars"
                    BattleSystem.BattleScreen.ResetVars()

                    IsReady = True
                Case Else
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace