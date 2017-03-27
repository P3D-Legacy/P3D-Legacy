Namespace Construct.Framework.Classes

    <ScriptClass("Battle")>
    <ScriptDescription("Used to initiate battles.")>
    Public Class CL_Battle

        Inherits ScriptClass

        'Not defeated: Shows intro message, Starts battle
        'Defeated: Shows defeat message
        <ScriptCommand("StartTrainer", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Plays the default reaction of a trainer.")>
        Private Function M_StartTrainer(ByVal argument As String) As String
            ActiveLine.Preserve = True

            Dim t As New Trainer(argument)
            If t.IsBeaten() = False Then
                If WorkValues.Count = 0 Then
                    CType(CurrentScreen, OverworldScreen).TrainerEncountered = True

                    If t.GetInSightMusic() <> "" And t.GetInSightMusic() <> "nomusic" Then
                        MusicPlayer.GetInstance().Play(t.GetInSightMusic(), True, 0.0F, 0.0F)
                    End If

                    If t.IntroMessage <> "" Then
                        Screen.TextBox.reDelay = 0.0F
                        Screen.TextBox.Show(t.IntroMessage, {})
                    End If

                    'Set a flag that this script has been started.
                    WorkValues.Add("started")
                End If
                If Screen.TextBox.Showing = False Then
                    CType(CurrentScreen, OverworldScreen).TrainerEncountered = False

                    Dim method As Integer = 0
                    If Screen.Level.Surfing = True Then
                        method = 2
                    End If

                    Dim b As New BattleSystem.BattleScreen(New Trainer(argument), CurrentScreen, method)
                    SetScreen(New BattleIntroScreen(CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))
                End If
            Else
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(t.DefeatMessage, {})

                ActiveLine.Preserve = False
            End If

            If Screen.TextBox.Showing = False Then
                ActiveLine.Preserve = False
            End If

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Trainer", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Starts a battle with a trainer.")>
        Private Function M_Trainer(ByVal argument As String) As String
            Dim ID As String = argument

            If argument.CountSeperators(",") > 0 Then
                ID = argument.GetSplit(0)

                For Each v As String In argument.Split(CChar(","))
                    Select Case v
                        Case "generate_pokemon_tower"
                            Dim level As Integer = 0
                            For Each p As Pokemon In Game.Core.Player.Pokemons
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

            Dim b As New BattleSystem.BattleScreen(t, CurrentScreen, method)
            SetScreen(New BattleIntroScreen(CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Wild", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Starts a wild battle.")>
        Private Function M_Wild(ByVal argument As String) As String
            'ID,    Level,  [shiny],    [musicloop],    [introtype]
            'int    int     -1 or bool  string          int

            'optional:
            '{pokemondata},             [musicloop],    [introtype]

            Dim p As Pokemon = Nothing
            Dim musicLoop As String = ""
            Dim introType As Integer = Random.Next(0, 10)

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
                                    introType = Int(args(i))
                            End Select
                        Next
                    End If
                End If
            Else
                If argument.Length > 0 Then
                    Dim ID As Integer = Int(argument.GetSplit(0))
                    Dim Level As Integer = Int(argument.GetSplit(1))

                    p = Pokemon.GetPokemonByID(ID)
                    p.Generate(Level, True)

                    Dim args() As String = argument.Split(CChar(","))

                    For i = 0 To args.Length - 1
                        Select Case i
                            Case 2
                                If args(i) <> "-1" Then
                                    p.IsShiny = Bool(args(i))
                                End If
                            Case 3
                                musicLoop = args(i)
                            Case 4
                                introType = Int(args(i))
                        End Select
                    Next
                End If
            End If

            Game.Core.Player.PokedexData = Pokedex.ChangeEntry(Game.Core.Player.PokedexData, p.Number, 1)
            Dim b As New BattleSystem.BattleScreen(p, CurrentScreen, method)
            SetScreen(New BattleIntroScreen(CurrentScreen, b, introType, musicLoop))

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetVar")>
        <ScriptDescription("Sets a battle variable.")>
        Private Function M_Setvar(ByVal argument As String) As String
            Dim varname As String = argument.GetSplit(0)
            Dim varvalue As String = argument.GetSplit(1)

            Select Case varname.ToLower()
                Case "canrun"
                    BattleSystem.BattleScreen.CanRun = Bool(varvalue)
                Case "cancatch"
                    BattleSystem.BattleScreen.CanCatch = Bool(varvalue)
                Case "canblackout"
                    BattleSystem.BattleScreen.CanBlackout = Bool(varvalue)
                Case "canreceiveexp"
                    BattleSystem.BattleScreen.CanReceiveEXP = Bool(varvalue)
                Case "canuseitems"
                    BattleSystem.BattleScreen.CanUseItems = Bool(varvalue)
                Case "frontiertrainer"
                    Trainer.FrontierTrainer = Int(varvalue)
                Case "divebattle"
                    BattleSystem.BattleScreen.DiveBattle = Bool(varvalue)
                Case "inversebattle"
                    BattleSystem.BattleScreen.IsInverseBattle = Bool(varvalue)
                Case "custombattlemusic"
                    BattleSystem.BattleScreen.CustomBattleMusic = varvalue
                Case "hiddenabilitychance"
                    Screen.Level.HiddenAbilityChance = Int(varvalue)

            End Select

            Return Core.Null
        End Function

        <ScriptCommand("ResetVars")>
        <ScriptDescription("Resets all battle variables to their default.")>
        Private Function M_Resetvars(ByVal argument As String) As String
            BattleSystem.BattleScreen.ResetVars()

            Return Core.Null
        End Function

        <ScriptConstruct("DefeatMessage")>
        <ScriptDescription("Returns the defeatmessage for a trainer.")>
        Private Function F_Defeatmessage(ByVal argument As String) As String
            Dim t As New Trainer(argument)

            Return t.DefeatMessage
        End Function

        <ScriptConstruct("IntroMessage")>
        <ScriptDescription("Returns the intromessage for a trainer.")>
        Private Function F_Intromessage(ByVal argument As String) As String
            Dim t As New Trainer(argument)

            Return t.IntroMessage
        End Function

        <ScriptConstruct("OutroMessage")>
        <ScriptDescription("Returns the outromessage for a trainer.")>
        Private Function F_Outromessage(ByVal argument As String) As String
            Dim t As New Trainer(argument)

            Return t.OutroMessage
        End Function

        <ScriptConstruct("Won")>
        <ScriptDescription("Returns if the last battle has been won by the player.")>
        Private Function F_Won(ByVal argument As String) As String
            Return ToString(BattleSystem.Battle.Won)
        End Function

    End Class

End Namespace