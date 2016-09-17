Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="battle")>
    Friend NotInheritable Class Battle

        <ScriptVariable>
        Public canRun As Boolean = True

        <ScriptVariable>
        Public canCatch As Boolean = True

        <ScriptVariable>
        Public canBlackout As Boolean = True

        <ScriptVariable>
        Public canReceiveExp As Boolean = True

        <ScriptVariable>
        Public canUseItems As Boolean = True

        <ScriptVariable>
        Public frontierTrainer As Integer = -1

        <ScriptVariable>
        Public isDiveBattle As Boolean = False

        <ScriptVariable>
        Public isInverseBattle As Boolean = False

        <ScriptVariable>
        Public customBattleMusic As String = ""

        <ScriptVariable>
        Public hiddenAbilityChance As Integer = 0

        Private Shared Sub ApplyValues(This As Battle)
            BattleSystem.BattleScreen.CanRun = This.canRun
            BattleSystem.BattleScreen.CanCatch = This.canCatch
            BattleSystem.BattleScreen.CanBlackout = This.canBlackout
            BattleSystem.BattleScreen.CanReceiveEXP = This.canReceiveExp
            BattleSystem.BattleScreen.CanUseItems = This.canUseItems
            BattleSystem.BattleScreen.DiveBattle = This.isDiveBattle
            BattleSystem.BattleScreen.IsInverseBattle = This.isInverseBattle
            BattleSystem.BattleScreen.CustomBattleMusic = This.customBattleMusic

            Trainer.FrontierTrainer = This.frontierTrainer
            Screen.Level.HiddenAbilityChance = This.hiddenAbilityChance
        End Sub

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="reset")>
        Public Shared Function Reset(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            objLink.SetMember("canRun", True)
            objLink.SetMember("canCatch", True)
            objLink.SetMember("canBlackout", True)
            objLink.SetMember("canReceiveExp", True)
            objLink.SetMember("canUseItems", True)
            objLink.SetMember("frontierTrainer", -1)
            objLink.SetMember("isDiveBattle", False)
            objLink.SetMember("isInverseBattle", False)
            objLink.SetMember("customBattleMusic", "")
            objLink.SetMember("hiddenAbilityChance", 0)

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="startWildData")>
        Public Shared Function StartWildData(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

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

                ApplyValues(CType(This, Battle))

                Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType, musicLoop))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="startWild")>
        Public Shared Function StartWild(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            'ID,    Level,  [shiny],    [musicloop],    [introtype]
            'int    int     -1 or bool  string          int

            If TypeContract.Ensure(parameters, {GetType(Integer), GetType(Integer), GetType(Boolean), GetType(String), GetType(Integer)}, 3) Then

                Dim helper = New ParamHelper(parameters)

                Dim id = helper.Pop(Of Integer)
                Dim level = helper.Pop(Of Integer)

                Dim p As Pokemon = Nothing

                Dim musicLoop As String
                Dim introType As Integer

                p = Pokemon.GetPokemonByID(id)
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

                ApplyValues(CType(This, Battle))

                Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType, musicLoop))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing
        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="startTrainer")>
        Public Shared Function StartTrainer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String)}) Then

                Dim trainerFile = CType(parameters(0), String)
                Dim t As New Trainer(trainerFile)

                Dim method As Integer = 0
                If Screen.Level.Surfing = True Then
                    method = 2
                End If

                ApplyValues(CType(This, Battle))

                Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="encounterTrainer")>
        Public Shared Function EncounterTrainer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

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

                    ApplyValues(CType(This, Battle))

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
