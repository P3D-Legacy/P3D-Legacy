Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Move")>
    Friend NotInheritable Class MovePrototype

        <ScriptVariable>
        Public id As Integer = 0

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

    End Class

End Namespace
