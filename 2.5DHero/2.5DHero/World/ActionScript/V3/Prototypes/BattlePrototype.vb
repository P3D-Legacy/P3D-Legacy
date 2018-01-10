Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Battle")>
    Friend NotInheritable Class BattlePrototype

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

        Private Shared Sub ApplyValues(This As BattlePrototype)
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
        <ApiMethodSignature()>
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

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="startWild")>
        <ApiMethodSignature({"pokemon", "music", "introType"}, {GetType(PokemonPrototype), GetType(String), GetType(Integer)}, 2)>
        Public Shared Function StartWild(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(PokemonPrototype), GetType(String), GetType(Integer)}, 2) Then

                Dim helper = New ParamHelper(parameters)

                Dim wrapper = helper.Pop(Of PokemonPrototype)

                Dim musicLoop As String = helper.Pop("")
                Dim introType As Integer = helper.Pop(Core.Random.Next(0, 10))

                Dim p = PokemonPrototype.GetPokemon(wrapper)

                Dim method As Integer = 0
                If Screen.Level.Surfing = True Then
                    method = 2
                End If

                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 1)

                ApplyValues(CType(This, BattlePrototype))

                Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, introType, musicLoop))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="startTrainer")>
        <ApiMethodSignature("trainer", GetType(TrainerPrototype))>
        Public Shared Function StartTrainer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(TrainerPrototype)) Then

                Dim wrapper = CType(parameters(0), TrainerPrototype)
                Dim t = New Trainer(wrapper.file)

                Dim method As Integer = 0
                If Screen.Level.Surfing = True Then
                    method = 2
                End If

                ApplyValues(CType(This, BattlePrototype))

                Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, method)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))

                ScriptManager.Instance.WaitFrames(1)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="encounterTrainer")>
        <ApiMethodSignature("trainer", GetType(TrainerPrototype))>
        Public Shared Function EncounterTrainer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(TrainerPrototype)) Then

                Dim wrapper = CType(parameters(0), TrainerPrototype)
                Dim t = New Trainer(wrapper.file)

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

                    ApplyValues(CType(This, BattlePrototype))

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

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="wonLast", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function WonLast(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return BattleSystem.Battle.Won

        End Function

    End Class

End Namespace
