Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="battle")>
    Friend NotInheritable Class Battle

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="reset")>
        Public Shared Function Reset(This As Object, parameters As Object()) As Object

            BattleSystem.BattleScreen.ResetVars()
            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="canRun")>
        Public Shared Function SetCanRun(This As Object, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Boolean)}) Then
                BattleSystem.BattleScreen.CanRun = CType(parameters(0), Boolean)
            End If
            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="battleWildData")>
        Public Shared Function BattleWildData(This As Object, parameters As Object()) As Object

            'optional:
            '{pokemondata},             [musicloop],    [introtype]

            If TypeContract.Ensure(parameters, {GetType(String), GetType(String), GetType(Integer)}, 2) Then

                Dim helper = New ParamHelper(parameters)

                Dim data = helper.Pop(Of String)

                Dim musicLoop As String = helper.Pop("")
                Dim introType As Integer = helper.Pop(Core.Random.Next(0, 10))

                Dim p As Pokemon = Nothing

                p = Pokemon.GetPokemonByData(data)

                Dim method As Integer = 0
                If Screen.Level.Surfing = True Then
                    method = 2
                End If

                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 1)
                Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType, musicLoop))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="battleWild")>
        Public Shared Function BattleWild(This As Object, parameters As Object()) As Object

            'ID,    Level,  [shiny],    [musicloop],    [introtype]
            'int    int     -1 or bool  string          int

            If TypeContract.Ensure(parameters, {GetType(Integer), GetType(Integer), GetType(Boolean), GetType(String), GetType(Integer)}, 3) Then

                Dim helper = New ParamHelper(parameters)

                Dim id = helper.Pop(Of Integer)
                Dim level = helper.Pop(Of Integer)

                Dim p As Pokemon = Nothing

                Dim musicLoop As String
                Dim introType As Integer

                p = Pokemon.GetPokemonByID(ID)
                p.Generate(level, True)

                If Not helper.HasEnded() Then

                    Dim shiny = helper.Pop(Of Boolean)
                    p.IsShiny = shiny

                End If

                musicLoop = helper.Pop("")
                introType = helper.Pop(Core.Random.Next(0, 10))

                Dim method As Integer = 0
                If Screen.Level.Surfing = True Then
                    method = 2
                End If

                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 1)
                Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType, musicLoop))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing
        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="battleTrainer")>
        Public Shared Function BattleTrainer(This As Object, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String)}) Then

                Dim trainerFile = CType(parameters(0), String)
                Dim t As New Trainer(trainerFile)

                Dim method As Integer = 0
                If Screen.Level.Surfing = True Then
                    method = 2
                End If

                Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="encounterTrainer")>
        Public Shared Function EncounterTrainer(This As Object, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String)}) Then

                Dim trainerFile = CType(parameters(0), String)
                Dim t = New Trainer(trainerFile)

                If Not t.IsBeaten() Then

                    CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = True

                    If t.GetInSightMusic() <> "" And t.GetInSightMusic() <> "nomusic" Then
                        MusicManager.PlayMusic(t.GetInSightMusic(), True, 0.0F, 0.0F)
                    End If

                    If t.IntroMessage <> "" Then
                        Screen.TextBox.reDelay = 0.0F
                        Screen.TextBox.Show(t.IntroMessage, {})
                    End If

                    ScriptManager.WaitUntil(Function()
                                                Return Not Screen.TextBox.Showing
                                            End Function)

                    CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = False

                    Dim method As Integer = 0
                    If Screen.Level.Surfing = True Then
                        method = 2
                    End If

                    Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, method)
                    Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))

                Else
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(t.DefeatMessage, {})

                    ScriptManager.WaitUntil(Function()
                                                Return Not Screen.TextBox.Showing
                                            End Function)

                End If

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing

        End Function

    End Class

End Namespace
