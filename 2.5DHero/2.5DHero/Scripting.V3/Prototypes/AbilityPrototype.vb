Option Strict On
Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Ability")>
    Friend NotInheritable Class AbilityPrototype

        Public Shared Function GetAbility(This As Object) As Ability
            Return CType(This, AbilityPrototype).ref
        End Function

        <Reference>
        Public ref As Ability

        Public Sub New() : End Sub

        Public Sub New(a As Ability)
            ref = a
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                objLink.SetReference(NameOf(ref), Ability.GetAbilityByID(CType(parameters(0), Integer)))

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="name")>
        Public Shared Function GetName(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim a = GetAbility(This)
            Return a.Name

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="id")>
        Public Shared Function GetId(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim a = GetAbility(This)
            Return a.ID

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="description")>
        Public Shared Function GetDescription(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim a = GetAbility(This)
            Return a.Description

        End Function

    End Class

End Namespace
