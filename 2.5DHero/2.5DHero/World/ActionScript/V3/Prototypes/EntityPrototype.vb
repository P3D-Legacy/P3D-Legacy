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
        <ApiMethodSignature(GetType(Vector3Prototype))>
        Public Shared Function GetPosition(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim entity = GetEntity(This)
            Return New Vector3Prototype(entity.Position)

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="position")>
        <ApiMethodSignature(GetType(Vector3Prototype))>
        Public Shared Function SetPosition(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Vector3Prototype)) Then

                Dim entity = GetEntity(This)
                entity.Position = CType(parameters(0), Vector3Prototype).ToVector3()
                entity.CreatedWorld = False

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="x")>
        <ApiMethodSignature(GetType(Double))>
        Public Shared Function GetX(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetEntity(This).Position.X

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="x")>
        <ApiMethodSignature(GetType(Double))>
        Public Shared Function SetX(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetEntity(This).Position.X = CType(parameters(0), Single)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="y")>
        <ApiMethodSignature(GetType(Double))>
        Public Shared Function GetY(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetEntity(This).Position.Y

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="y")>
        <ApiMethodSignature(GetType(Double))>
        Public Shared Function SetY(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetEntity(This).Position.Y = CType(parameters(0), Single)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="z")>
        <ApiMethodSignature(GetType(Double))>
        Public Shared Function GetZ(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Return GetEntity(This).Position.Z

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="z")>
        <ApiMethodSignature(GetType(Double))>
        Public Shared Function SetZ(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number}) Then

                GetEntity(This).Position.Z = CType(parameters(0), Single)

            End If

            Return NetUndefined.Instance

        End Function

#End Region

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="scale")>
        <ApiMethodSignature(GetType(Vector3Prototype))>
        Public Shared Function GetScale(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim entity = GetEntity(This)
            Return New Vector3Prototype(entity.Scale)

        End Function

        <ScriptFunction(ScriptFunctionType.Setter, VariableName:="scale")>
        <ApiMethodSignature(GetType(Vector3Prototype))>
        Public Shared Function SetScale(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Vector3Prototype)) Then

                Dim entity = GetEntity(This)
                entity.Scale = CType(parameters(0), Vector3Prototype).ToVector3()
                entity.CreatedWorld = False

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="remove")>
        <ApiMethodSignature()>
        Public Shared Function Remove(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim entity = GetEntity(This)
            entity.CanBeRemoved = True
            Return NetUndefined.Instance

        End Function

    End Class

End Namespace
