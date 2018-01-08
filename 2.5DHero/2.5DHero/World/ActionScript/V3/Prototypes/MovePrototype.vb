Option Strict On
Imports Kolben.Adapters
Imports net.Pokemon3D.Game.BattleSystem

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Move")>
    Friend NotInheritable Class MovePrototype

        <Reference>
        Public ref As Attack

        Public Shared Function GetAttack(This As Object) As Attack
            Return CType(This, MovePrototype).ref
        End Function

        Public Sub New() : End Sub

        Public Sub New(a As Attack)
            ref = a
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                objLink.SetMember("id", CType(parameters(0), Integer))

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="name")>
        Public Shared Function GetName(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.Name

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="pp")>
        Public Shared Function GetPP(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.CurrentPP

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="maxPP")>
        Public Shared Function GetMaxPP(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.MaxPP

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="id")>
        Public Shared Function GetId(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.ID

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="category")>
        Public Shared Function GetCategory(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.Category.ToString()

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="description")>
        Public Shared Function GetDescription(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.Description

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="type")>
        Public Shared Function GetElementType(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim attack = GetAttack(This)
            Return attack.Type.Type.ToString()

        End Function

    End Class

End Namespace
