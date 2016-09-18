Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Pokemon")>
    Friend NotInheritable Class PokemonPrototype

        ' When a Pokémon from the player's party is referenced, use its index in the list to have a reference instead of shallow copy.
        Private Const PLAYER_POKEMON_PREFIX As String = "PLAYER_POKEMON_"

        <ScriptVariable(VariableName:="data")>
        Public data As String = ""

        Public Shared Function GetPokemon(This As Object) As Pokemon

            Dim pokemonData = CType(This, PokemonPrototype).data

            If pokemonData.StartsWith(PLAYER_POKEMON_PREFIX) Then

                Dim index = CInt(pokemonData.Remove(0, PLAYER_POKEMON_PREFIX.Length))
                Return Core.Player.Pokemons(index)

            Else

                Return Pokemon.GetPokemonByData(pokemonData)

            End If

        End Function

        Private Shared Sub SaveData(This As Object, objLink As ScriptObjectLink, p As Pokemon)

            If Not CType(This, PokemonPrototype).data.StartsWith(PLAYER_POKEMON_PREFIX) Then

                Dim newData = p.GetSaveData()
                objLink.SetMember("data", newData)

            End If

        End Sub

        Public Sub New(data As String)
            Me.data = data
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If parameters.Length = 1 AndAlso TypeContract.Ensure(parameters, GetType(String)) Then

                objLink.SetMember("data", CType(parameters(0), String))

            ElseIf parameters.Length = 1 AndAlso TypeContract.Ensure(parameters, GetType(Integer)) Then

                objLink.SetMember("data", PLAYER_POKEMON_PREFIX & CType(parameters(0), Integer).ToString())

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
                SaveData(This, objLink, p)

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
                SaveData(This, objLink, p)

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
                SaveData(This, objLink, p)

            End If

            Return NetUndefined.Instance

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
                SaveData(This, objLink, p)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

    End Class

End Namespace
