Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Vector3")>
    Friend NotInheritable Class Vector3Wrapper

        Public Function ToVector3() As Vector3
            Return New Vector3(CType(x, Single), CType(y, Single), CType(z, Single))
        End Function

        <ScriptVariable(VariableName:="x")>
        Public x As Double = 0D

        <ScriptVariable(VariableName:="y")>
        Public y As Double = 0D

        <ScriptVariable(VariableName:="z")>
        Public z As Double = 0D

        Public Sub New() : End Sub

        Public Sub New(x As Double, y As Double, z As Double)
            Me.x = x
            Me.y = y
            Me.z = z
        End Sub

        Public Sub New(v As Vector3)
            x = v.X
            y = v.Y
            z = v.Z
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {TypeContract.Number, TypeContract.Number, TypeContract.Number}, 3) Then

                Dim helper = New ParamHelper(parameters)

                objLink.SetMember("x", helper.Pop(0D))
                objLink.SetMember("y", helper.Pop(0D))
                objLink.SetMember("z", helper.Pop(0D))

            End If

            Return NetUndefined.Instance

        End Function

    End Class

End Namespace
