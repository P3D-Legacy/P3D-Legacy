Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Pokedex")>
    Friend NotInheritable Class PokedexPrototype

        <Reference>
        Public ref As Integer

        Public Shared Function GetId(This As Object) As Integer
            Return CType(This, PokedexPrototype).ref
        End Function

        Public Shared Function GetPokedex(This As Object) As Pokedex
            Return Core.Player.Pokedexes(CType(This, PokedexPrototype).ref)
        End Function

        Public Sub New() : End Sub

        Public Sub New(id As Integer)
            ref = id
        End Sub

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="id")>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetId(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim id = GetId(This)
            Return id

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="caught")>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetCaught(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim pokedex = GetPokedex(This)
            Return pokedex.Obtained

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="seen")>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetSeen(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim pokedex = GetPokedex(This)
            Return pokedex.Seen

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="autoDetect", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function GetAutoDetect(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Pokedex.AutoDetect

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="autoDetect", IsStatic:=True)>
        <ApiMethodSignature(GetType(Boolean))>
        Public Shared Function SetAutoDetect(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Boolean)}) Then

                Pokedex.AutoDetect = CType(parameters(0), Boolean)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="caughtTotal", IsStatic:=True)>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetCaughtTotal(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Pokedex.CountEntries(Core.Player.PokedexData, {2, 3})

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="shinyTotal", IsStatic:=True)>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetShinyTotal(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Pokedex.CountEntries(Core.Player.PokedexData, {3})

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="seenTotal", IsStatic:=True)>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetSeenTotal(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return Pokedex.CountEntries(Core.Player.PokedexData, {1})

        End Function

    End Class

End Namespace
