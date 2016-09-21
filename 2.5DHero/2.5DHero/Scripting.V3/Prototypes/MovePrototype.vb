Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Move")>
    Friend NotInheritable Class MovePrototype

        <ScriptVariable>
        Public id As Integer = 0

        Public Shared Function ToAttack(This As Object) As BattleSystem.Attack

            Dim moveId = CType(This, MovePrototype).id
            Return BattleSystem.Attack.GetAttackByID(moveId)

        End Function

        Public Sub New() : End Sub

        Public Sub New(id As Integer)
            Me.id = id
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

            Dim attack = ToAttack(This)
            Return attack.Name

        End Function

    End Class

End Namespace
