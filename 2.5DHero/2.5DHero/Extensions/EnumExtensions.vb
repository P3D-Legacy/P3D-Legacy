Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module EnumExtensions
    <Extension> _
    Public Function SomethingConstructorArray(Of TSomething)(somethingTypeEnum As [Enum], assemblyWhereToSearch As Assembly) As Func(Of TSomething)()
        Dim somethingTypeNameArray = [Enum].GetValues(somethingTypeEnum.[GetType]())
        Dim sometingConstructorArray = New Func(Of TSomething)(somethingTypeNameArray.Cast(Of Integer)().Max()) {}

        For Each attackTypeName In somethingTypeNameArray
            Dim type = GetTypeFromNameAndAbstract(Of TSomething)(attackTypeName.ToString(), assemblyWhereToSearch)
            sometingConstructorArray(CInt(attackTypeName)) = If(type IsNot Nothing, DirectCast(Function() DirectCast(Activator.CreateInstance(type), TSomething), Func(Of TSomething)), Nothing)
        Next

        Return sometingConstructorArray
    End Function

    Private Function GetTypeFromNameAndAbstract(Of T)(className As String, assembly As Assembly) As Type
        Return assembly.DefinedTypes.Where(Function(typeInfo) typeInfo.Name = className And typeInfo.IsSubclassOf(GetType(T))).Select(Function(typeInfo) typeInfo.AsType()).FirstOrDefault()
    End Function
End Module