Imports Kolben.Adapters

Namespace Scripting.V3

    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=True)>
    Friend Class ApiMethodSignatureAttribute

        Inherits Attribute

        Public Property ReturnType As Type()
        Public Property ParamNames As String()
        Public Property ParamTypes As Type()
        Public Property OptionalNum As Integer

        Public Sub New(returnType As Type(), paramNames As String(), paramTypes As Type(), optionalNum As Integer)
            Me.ReturnType = returnType
            Me.ParamNames = paramNames
            Me.ParamTypes = paramTypes
            Me.OptionalNum = optionalNum
        End Sub

        Public Sub New()
            Me.New({GetType(NetUndefined)}, {}, {}, 0)
        End Sub

        Public Sub New(returnType As Type)
            Me.New({returnType}, {}, {}, 0)
        End Sub

        Public Sub New(returnType As Type())
            Me.New(returnType, {}, {}, 0)
        End Sub

        Public Sub New(paramNames As String(), paramTypes As Type())
            Me.New({GetType(NetUndefined)}, paramNames, paramTypes, 0)
        End Sub

        Public Sub New(paramNames As String(), paramTypes As Type(), optionalNum As Integer)
            Me.New({GetType(NetUndefined)}, paramNames, paramTypes, optionalNum)
        End Sub

        Public Sub New(paramName As String, paramType As Type)
            Me.New({GetType(NetUndefined)}, {paramName}, {paramType}, 0)
        End Sub

        Public Sub New(paramName As String, paramType As Type, optionalNum As Integer)
            Me.New({GetType(NetUndefined)}, {paramName}, {paramType}, optionalNum)
        End Sub

        Public Sub New(returnType As Type, paramName As String, paramType As Type)
            Me.New({returnType}, {paramName}, {paramType}, 0)
        End Sub

        Public Sub New(returnType As Type, paramName As String, paramType As Type, optionalNum As Integer)
            Me.New({returnType}, {paramName}, {paramType}, optionalNum)
        End Sub

        Public Sub New(returnType As Type, paramNames As String(), paramTypes As Type(), optionalNum As Integer)
            Me.New({returnType}, paramNames, paramTypes, optionalNum)
        End Sub

        Public Sub New(returnTypes As Type(), paramName As String, paramType As Type)
            Me.New(returnTypes, {paramName}, {paramType}, 0)
        End Sub

    End Class

End Namespace
