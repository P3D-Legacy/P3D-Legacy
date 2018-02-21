Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Player")>
    Friend NotInheritable Class PlayerPrototype

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="hasPokedex", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function GetHasPokedex(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Core.Player.HasPokedex

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="hasPokedex", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function SetHasPokedex(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Boolean)) Then

                Core.Player.HasPokedex = CType(parameters(0), Boolean)
                ' register team in pokedex
                If Core.Player.HasPokedex Then
                    Dim data = Core.Player.PokedexData
                    For Each pokemon In Core.Player.Pokemons
                        data = Pokedex.RegisterPokemon(data, pokemon)
                    Next
                    Core.Player.PokedexData = data
                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="hasPokegear", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function GetHasPokegear(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Core.Player.HasPokegear

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="hasPokegear", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function SetHasPokegear(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Boolean)) Then

                Core.Player.HasPokegear = CType(parameters(0), Boolean)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="inventory", IsStatic:=True)>
        <ApiMethodSignature(GetType(InventoryPrototype))>
        Public Shared Function GetInventory(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return New InventoryPrototype()

        End Function

    End Class

End Namespace
