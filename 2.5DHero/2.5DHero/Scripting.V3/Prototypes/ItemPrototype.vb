Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Item")>
    Friend NotInheritable Class ItemPrototype

        <ScriptVariable>
        Public id As Integer = 0

        <ScriptVariable>
        Public data As String = ""

        Public Shared Function ToItem(This As Object) As Item

            Dim itemId = CType(This, ItemPrototype).id
            Dim resultItem = Item.GetItemByID(itemId)
            resultItem.AdditionalData = CType(This, ItemPrototype).data

            Return resultItem

        End Function

        Public Sub New(id As Integer, data As String)
            Me.id = id
            Me.data = data
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object


            If TypeContract.Ensure(parameters, {GetType(Integer), GetType(String)}, 1) Then

                Dim helper = New ParamHelper(parameters)

                objLink.SetMember("id", helper.Pop(Of Integer))
                objLink.SetMember("data", helper.Pop(""))

            ElseIf TypeContract.Ensure(parameters, {GetType(String), GetType(String)}, 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim namedItem = Item.GetItemByName(helper.Pop(Of String))

                If namedItem IsNot Nothing Then
                    objLink.SetMember("id", namedItem.ID)
                End If

                objLink.SetMember("data", helper.Pop(""))

            End If

            Return NetUndefined.Instance

        End Function

    End Class

End Namespace
