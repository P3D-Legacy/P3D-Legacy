Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Item")>
    Friend NotInheritable Class ItemPrototype

        <Reference>
        Public ref As Item

        Public Shared Function GetItem(This As Object) As Item
            Return CType(This, ItemPrototype).ref
        End Function

        Public Sub New() : End Sub

        Public Sub New(item As Item)
            ref = item
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        <ApiMethodSignature({"id", "additionalData"}, {GetType(Integer), GetType(String)}, 1)>
        <ApiMethodSignature({"name", "additionalData"}, {GetType(String), GetType(String)}, 1)>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Integer), GetType(String)}, 1) Then

                Dim helper = New ParamHelper(parameters)

                Dim itemRef = Item.GetItemByID(helper.Pop(Of Integer))
                itemRef.AdditionalData = helper.Pop("")

                objLink.SetReference(NameOf(ref), itemRef)

            ElseIf TypeContract.Ensure(parameters, {GetType(String), GetType(String)}, 1) Then

                Dim helper = New ParamHelper(parameters)

                Dim itemRef = Item.GetItemByName(helper.Pop(Of String))
                itemRef.AdditionalData = helper.Pop("")

                objLink.SetReference(NameOf(ref), itemRef)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="id")>
        <ApiMethodSignature(GetType(Integer))>
        Public Shared Function GetId(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim item = GetItem(This)
            Return item.ID

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="name")>
        <ApiMethodSignature(GetType(String))>
        Public Shared Function GetName(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim item = GetItem(This)
            Return item.Name

        End Function

    End Class

End Namespace
