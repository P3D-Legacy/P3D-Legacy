Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Entity")>
    Friend NotInheritable Class EntityPrototype

        <Reference>
        Public ref As Entity

        Public Shared Function GetEntity(This As Object) As Entity
            Return CType(This, EntityPrototype).ref
        End Function

#Region "Position"

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="position")>
        Public Shared Function GetPosition(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim entity = GetEntity(This)
            Return New Vector3Prototype(entity.Position)

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="position")>
        Public Shared Function SetPosition(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Vector3Prototype)) Then

                Dim entity = GetEntity(This)
                entity.Position = CType(parameters(0), Vector3Prototype).ToVector3()

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="x", IsStatic:=True)>
        Public Shared Function GetX(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetEntity(This).Position.X

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="x", IsStatic:=True)>
        Public Shared Function SetX(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetEntity(This).Position.X = CType(parameters(0), Single)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="y", IsStatic:=True)>
        Public Shared Function GetY(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetEntity(This).Position.Y

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="y", IsStatic:=True)>
        Public Shared Function SetY(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetEntity(This).Position.Y = CType(parameters(0), Single)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="z", IsStatic:=True)>
        Public Shared Function GetZ(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetEntity(This).Position.Z

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="z", IsStatic:=True)>
        Public Shared Function SetZ(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetEntity(This).Position.Z = CType(parameters(0), Single)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

    End Class

End Namespace
