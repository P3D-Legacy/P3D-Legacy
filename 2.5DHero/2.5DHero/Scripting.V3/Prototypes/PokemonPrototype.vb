Option Strict On
Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Pokemon")>
    Friend NotInheritable Class PokemonPrototype

        <Reference>
        Public ref As Pokemon

        Public Shared Function GetPokemon(This As Object) As Pokemon
            Return CType(This, PokemonPrototype).ref
        End Function

        Public Sub New() : End Sub

        Public Sub New(p As Pokemon)
            ref = p
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If parameters.Length = 1 AndAlso TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = Pokemon.GetPokemonByData(CType(parameters(0), String))

            ElseIf parameters.Length >= 2 AndAlso TypeContract.Ensure(parameters, {GetType(Integer), GetType(Integer), GetType(String)}, 1) Then

                Dim helper = New ParamHelper(parameters)

                Dim id = helper.Pop(Of Integer)
                Dim level = helper.Pop(Of Integer)
                Dim additionalData = helper.Pop("")

                Dim p = Pokemon.GetPokemonByID(id, additionalData)
                Dim exp = p.NeedExperience(level)
                p.GetExperience(exp, True)

                objLink.SetMember("data", p.GetSaveData())

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="getLegendaryIds", IsStatic:=True)>
        Public Shared Function GetLegendaryIds(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Pokemon.Legendaries

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="getSprite")>
        Public Shared Function GetOverworldSprite(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return $"[POKEMON|{If(p.IsShiny, "S", "N")}]{p.Number.ToString()}{PokemonForms.GetOverworldAddition(p)}"

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="id")>
        Public Shared Function GetId(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Number

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="name")>
        Public Shared Function GetName(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.OriginalName

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="translatedName")>
        Public Shared Function GetTranslatedName(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.GetName()

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="nickname")>
        Public Shared Function GetNickname(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.NickName

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="nickname")>
        Public Shared Function SetNickname(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                p.NickName = CType(parameters(0), String)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="displayName")>
        Public Shared Function GetDisplayName(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.GetDisplayName()

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="isShiny")>
        Public Shared Function GetIsShiny(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.IsShiny

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="isShiny")>
        Public Shared Function SetIsShiny(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Boolean)) Then

                Dim p = GetPokemon(This)
                p.IsShiny = CType(parameters(0), Boolean)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="canSwim")>
        Public Shared Function GetCanSwim(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.CanSwim

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="level")>
        Public Shared Function GetLevel(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Level

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="cry")>
        Public Shared Function PlayCry(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            p.PlayCry()
            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="additionalData")>
        Public Shared Function GetAdditionalData(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.AdditionalData

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="additionalData")>
        Public Shared Function SetAdditionalData(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                p.AdditionalData = CType(parameters(0), String)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="isEgg")>
        Public Shared Function GetIsEgg(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.IsEgg()

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="nature")>
        Public Shared Function GetNature(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Nature.ToString()

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="nature")>
        Public Shared Function SetNature(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                Dim p = GetPokemon(This)
                p.Nature = Pokemon.ConvertIDToNature(CType(parameters(0), Integer))

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="friendship")>
        Public Shared Function GetFriendship(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Friendship

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="friendship")>
        Public Shared Function SetFriendship(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                Dim p = GetPokemon(This)
                p.Friendship = CType(parameters(0), Integer)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="ot")>
        Public Shared Function GetOT(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.OT

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="ot")>
        Public Shared Function SetOT(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                p.OT = CType(parameters(0), String)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="catchTrainer")>
        Public Shared Function GetCatchTrainer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.CatchTrainerName

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="catchTrainer")>
        Public Shared Function SetCatchTrainer(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                p.CatchTrainerName = CType(parameters(0), String)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="catchMethod")>
        Public Shared Function GetCatchMethod(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.CatchMethod

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="catchMethod")>
        Public Shared Function SetCatchMethod(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                p.CatchMethod = CType(parameters(0), String)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="catchLocation")>
        Public Shared Function GetCatchLocation(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.CatchLocation

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="catchLocation")>
        Public Shared Function SetCatchLocation(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                p.CatchLocation = CType(parameters(0), String)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="catchBall")>
        Public Shared Function GetCatchBall(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return New ItemPrototype(p.CatchBall)

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="catchBall")>
        Public Shared Function SetCatchBall(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(ItemPrototype)) Then

                Dim p = GetPokemon(This)
                p.CatchBall = ItemPrototype.ToItem(parameters(0))

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="item")>
        Public Shared Function GetItem(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)

            If p.Item IsNot Nothing Then
                Return New ItemPrototype(p.Item)
            Else
                Return Nothing
            End If

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="item")>
        Public Shared Function SetItem(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(ItemPrototype)) Then

                Dim p = GetPokemon(This)

                If parameters(0) Is Nothing Then
                    p.Item = Nothing
                Else

                    Dim item = CType(parameters(0), ItemPrototype)
                    p.Item = ItemPrototype.ToItem(item)

                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="gender")>
        Public Shared Function GetGender(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Gender

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="gender")>
        Public Shared Function SetGender(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                Dim newGender As Pokemon.Genders

                If [Enum].TryParse(CType(parameters(0), String), newGender) Then

                    p.Gender = newGender

                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="ability")>
        Public Shared Function GetAbility(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return New AbilityPrototype(p.Ability)

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="ability")>
        Public Shared Function SetAbility(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(AbilityPrototype)) Then

                Dim p = GetPokemon(This)
                p.Ability = AbilityPrototype.GetAbility(parameters(0))

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="types")>
        Public Shared Function GetTypes(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)

            If p.Type2.Type = Element.Types.Blank Then
                Return {p.Type1.Type.ToString()}
            Else
                Return {p.Type1.Type.ToString(), p.Type2.Type.ToString()}
            End If

        End Function

#Region "Stats"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="stats")>
        Public Shared Function GetStats(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return New With
            {
                .hp = p.HP,
                .maxHp = p.MaxHP,
                .atk = p.Attack,
                .def = p.Defense,
                .spAtk = p.SpAttack,
                .spDef = p.SpDefense,
                .speed = p.Speed
            }

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="EVs")>
        Public Shared Function GetEVs(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return New With
            {
                .hp = p.EVHP,
                .atk = p.EVAttack,
                .def = p.EVDefense,
                .spAtk = p.EVSpAttack,
                .spDef = p.EVSpDefense,
                .speed = p.EVSpeed
            }

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="setEV")>
        Public Shared Function SetEV(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String), GetType(Integer)}) Then

                Dim p = GetPokemon(This)

                Dim evType = CType(parameters(0), String).ToLowerInvariant()
                Dim evValue = CType(parameters(1), Integer)

                Select Case evType.ToLower()
                    Case "hp"
                        p.EVHP = evValue
                    Case "atk"
                        p.EVAttack = evValue
                    Case "def"
                        p.EVDefense = evValue
                    Case "spatk"
                        p.EVSpAttack = evValue
                    Case "spdef"
                        p.EVSpDefense = evValue
                    Case "speed"
                        p.EVSpeed = evValue
                End Select

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="setIV")>
        Public Shared Function SetIV(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String), GetType(Integer)}) Then

                Dim p = GetPokemon(This)

                Dim ivType = CType(parameters(0), String).ToLowerInvariant()
                Dim ivValue = CType(parameters(1), Integer)

                Select Case ivType.ToLower()
                    Case "hp"
                        p.IVHP = ivValue
                    Case "atk"
                        p.IVAttack = ivValue
                    Case "def"
                        p.IVDefense = ivValue
                    Case "spatk"
                        p.IVSpAttack = ivValue
                    Case "spdef"
                        p.IVSpDefense = ivValue
                    Case "speed"
                        p.IVSpeed = ivValue
                End Select

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="giveEVs")>
        Public Shared Function GetGiveEVs(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return New With
            {
                .hp = p.GiveEVHP,
                .atk = p.GiveEVAttack,
                .def = p.GiveEVDefense,
                .spAtk = p.GiveEVSpAttack,
                .spDef = p.GiveEVSpDefense,
                .speed = p.GiveEVSpeed
            }

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="IVs")>
        Public Shared Function GetIVs(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return New With
            {
                .hp = p.IVHP,
                .atk = p.IVAttack,
                .def = p.IVDefense,
                .spAtk = p.IVSpAttack,
                .spDef = p.IVSpDefense,
                .speed = p.IVSpeed
            }

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="hp")>
        Public Shared Function GetHP(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.HP

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="hp")>
        Public Shared Function SetHP(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                Dim p = GetPokemon(This)
                p.HP = CType(parameters(0), Integer)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="exp")>
        Public Shared Function GetExp(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Experience

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="exp")>
        Public Shared Function SetExp(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                Dim p = GetPokemon(This)
                p.Experience = CType(parameters(0), Integer)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="heal")>
        Public Shared Function Heal(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer), 1) Then

                Dim p = GetPokemon(This)
                Dim hp = New ParamHelper(parameters).Pop(p.MaxHP)
                p.Heal(hp)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

#Region "Moves"

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="getMoves")>
        Public Shared Function GetMoves(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Attacks.Select(Function(a As BattleSystem.Attack)
                                        Return New MovePrototype(a)
                                    End Function).ToArray()

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="removeMoveAt")>
        Public Shared Function RemoveMoveAt(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                Dim p = GetPokemon(This)
                Dim moveIndex = CType(parameters(0), Integer)

                If p.Attacks.Count > moveIndex Then
                    p.Attacks.RemoveAt(moveIndex)
                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="clearMoves")>
        Public Shared Function ClearMoves(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            p.Attacks.Clear()

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="addMove")>
        Public Shared Function AddMove(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(MovePrototype)) Then

                Dim p = GetPokemon(This)
                If p.Attacks.Count < 4 Then

                    Dim move = CType(parameters(0), MovePrototype)
                    p.Attacks.Add(MovePrototype.GetAttack(move))

                End If

            End If

            Return NetUndefined.Instance

        End Function

#End Region

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="reload")>
        Public Shared Function ReloadDefinitions(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            p.ReloadDefinitions()

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="status")>
        Public Shared Function GetStatus(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim p = GetPokemon(This)
            Return p.Status

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="status")>
        Public Shared Function SetStatus(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim p = GetPokemon(This)
                Dim newStatus As Pokemon.StatusProblems

                If [Enum].TryParse(CType(parameters(0), String), newStatus) Then

                    p.Status = newStatus

                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="canEvolve")>
        Public Shared Function CanEvolve(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(String)) Then

                Dim helper = New ParamHelper(parameters)

                Dim triggerStr = helper.Pop("LevelUp")
                Dim trigger As EvolutionCondition.EvolutionTrigger

                If Not [Enum].TryParse(triggerStr, trigger) Then
                    trigger = EvolutionCondition.EvolutionTrigger.LevelUp
                End If

                Dim evolutionArg = helper.Pop("")

                Dim p = GetPokemon(This)
                Return p.CanEvolve(trigger, evolutionArg)

            End If

            Return False

        End Function

    End Class

End Namespace
