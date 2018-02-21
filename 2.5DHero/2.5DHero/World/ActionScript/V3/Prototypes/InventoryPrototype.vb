Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Player")>
    Friend NotInheritable Class InventoryPrototype

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="add")>
        <ApiMethodSignature({"item", "amount"}, {GetType(ItemPrototype), GetType(Integer)}, 1)>
        <ApiMethodSignature({"id", "amount"}, {GetType(Integer), GetType(Integer)}, 1)>
        Public Shared Function Add(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(ItemPrototype), GetType(Integer)}, 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim item = helper.Pop(Of ItemPrototype)
                Dim amount = helper.Pop(1)

                Core.Player.Inventory.AddItem(ItemPrototype.GetItem(item).ID, amount)

            ElseIf TypeContract.Ensure(parameters, {GetType(Integer), GetType(Integer)}, 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim itemId = helper.Pop(Of Integer)
                Dim amount = helper.Pop(1)

                Core.Player.Inventory.AddItem(itemId, amount)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="remove")>
        <ApiMethodSignature({"item", "amount"}, {GetType(ItemPrototype), GetType(Integer)}, 1)>
        <ApiMethodSignature({"id", "amount"}, {GetType(Integer), GetType(Integer)}, 1)>
        Public Shared Function Remove(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(ItemPrototype), GetType(Integer)}, 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim item = helper.Pop(Of ItemPrototype)
                Dim amount = helper.Pop(1)

                Core.Player.Inventory.RemoveItem(ItemPrototype.GetItem(item).ID, amount)

            ElseIf TypeContract.Ensure(parameters, {GetType(Integer), GetType(Integer)}, 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim itemId = helper.Pop(Of Integer)
                Dim amount = helper.Pop(1)

                Core.Player.Inventory.RemoveItem(itemId, amount)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="clear")>
        <ApiMethodSignature("item", GetType(ItemPrototype), 1)>
        <ApiMethodSignature("id", GetType(Integer), 1)>
        Public Shared Function Clear(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(ItemPrototype), 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim item = helper.Pop(Of ItemPrototype)

                If (item IsNot Nothing) Then
                    Core.Player.Inventory.RemoveItem(ItemPrototype.GetItem(item).ID)
                Else
                    Core.Player.Inventory.Clear()
                End If

            ElseIf TypeContract.Ensure(parameters, GetType(Integer), 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim itemId = helper.Pop(0)

                If (itemId > 0) Then
                    Core.Player.Inventory.RemoveItem(itemId)
                Else
                    Core.Player.Inventory.Clear()
                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="count")>
        <ApiMethodSignature(GetType(Integer), "item", GetType(ItemPrototype), 1)>
        <ApiMethodSignature(GetType(Integer), "id", GetType(Integer), 1)>
        Public Shared Function Count(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(ItemPrototype), 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim item = helper.Pop(Of ItemPrototype)

                If (item IsNot Nothing) Then
                    Return Core.Player.Inventory.GetItemAmount(ItemPrototype.GetItem(item).ID)
                Else
                    Return Core.Player.Inventory.Count
                End If

            ElseIf TypeContract.Ensure(parameters, GetType(Integer), 1) Then

                Dim helper = New ParamHelper(parameters)
                Dim itemId = helper.Pop(0)

                If (itemId > 0) Then
                    Return Core.Player.Inventory.GetItemAmount(itemId)
                Else
                    Return Core.Player.Inventory.Count
                End If

            End If

            Return 0

        End Function

    End Class

End Namespace
