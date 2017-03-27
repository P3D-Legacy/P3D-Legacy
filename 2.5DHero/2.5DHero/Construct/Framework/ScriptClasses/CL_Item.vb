Namespace Construct.Framework.Classes

    <ScriptClass("Item")>
    <ScriptDescription("A class to work with items.")>
    Public Class CL_Item

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("Add")>
        <ScriptDescription("Adds an item to the player's inventory.")>
        Private Function M_Add(ByVal argument As String) As String
            Dim amount As Integer = 1
            Dim ItemID As String = argument
            If argument.Contains(",") = True Then
                amount = Int(argument.GetSplit(1))
                ItemID = argument.GetSplit(0)
            End If

            If Converter.IsNumeric(ItemID) = False Then
                Dim item As Item = Item.GetItemByName(ItemID)

                If Not item Is Nothing Then
                    Game.Core.Player.Inventory.AddItem(item.ID, amount)
                End If
            Else
                Game.Core.Player.Inventory.AddItem(Int(ItemID), amount)
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Remove")>
        <ScriptDescription("Removes an item from the player's inventory.")>
        Private Function M_Remove(ByVal argument As String) As String
            Dim amount As Integer = 1
            Dim ItemID As String = argument
            Dim showMessage As Boolean = True

            If argument.Contains(",") = True Then
                amount = Int(argument.GetSplit(1))
                ItemID = argument.GetSplit(0)
                If argument.CountSeperators(",") >= 2 Then
                    showMessage = Bool(argument.GetSplit(2))
                End If
            End If

            Dim Item As Item = Item.GetItemByID(Int(ItemID))

            Game.Core.Player.Inventory.RemoveItem(Item.ID, amount)

            If showMessage = True Then
                Dim Message As String = ""
                If amount = 1 Then
                    Message = "<playername> handed over the~" & Item.Name & "!"
                Else
                    Message = "<playername> handed over the~" & Item.PluralName & "!"
                End If

                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.TextColor = TextBox.PlayerColor
                Screen.TextBox.Show(Message, {})

                ActiveLine.EndExecutionFrame = True
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Clear")>
        <ScriptDescription("Clears all instances of the given item id from the player's inventory.")>
        Private Function M_Clear(ByVal argument As String) As String
            If argument <> "" Then
                Dim ItemID As Integer = Int(argument)
                Dim amount As Integer = Game.Core.Player.Inventory.GetItemAmount(ItemID)

                If amount > 0 Then
                    Game.Core.Player.Inventory.RemoveItem(ItemID, amount)
                End If
            Else
                Game.Core.Player.Inventory.Clear()
            End If

            Return Core.Null
        End Function

        <ScriptCommand("MessageGive")>
        <ScriptDescription("Displays a message that the player got items.")>
        Private Function M_MessageGive(ByVal argument As String) As String
            Dim ItemID As String = argument.GetSplit(0)
            Dim Item As Item = Nothing
            If Converter.IsNumeric(ItemID) = False Then
                Item = Item.GetItemByName(ItemID)
            Else
                Item = Item.GetItemByID(Int(ItemID))
            End If

            Dim Amount As Integer = Int(argument.GetSplit(1))

            If Not Item Is Nothing Then
                Dim receiveString As String = "Received the~" & Item.Name & ".*"
                If Amount > 1 Then
                    receiveString = "Received " & Amount & "~" & Item.PluralName & ".*"
                End If

                SoundManager.PlaySound("item_found", True)

                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.TextColor = TextBox.PlayerColor
                Screen.TextBox.Show(receiveString & Game.Core.Player.Inventory.GetMessageReceive(Item, Amount), {})

                ActiveLine.EndExecutionFrame = True
            End If

            Return Core.Null
        End Function

        <ScriptCommand("UseRepel")>
        <ScriptDescription("Uses a repel on the player.")>
        Private Function M_UseRepel(ByVal argument As String) As String
            Dim itemID As Integer = Int(argument)
            Dim steps As Integer = 0
            Select Case itemID
                Case 20
                    steps = 100
                Case 42
                    steps = 200
                Case 43
                    steps = 250
            End Select
            Game.Core.Player.RepelSteps += steps

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Count")>
        <ScriptDescription("Counts the amount of one type of item.")>
        Private Function F_Count(ByVal argument As String) As String
            Dim ItemID As Integer = Int(argument.GetSplit(0))

            Return ToString(Game.Core.Player.Inventory.GetItemAmount(ItemID))
        End Function

        <ScriptConstruct("CountAll")>
        <ScriptDescription("Counts the amount of all items.")>
        Private Function F_CountAll(ByVal argument As String) As String
            Dim c As Integer = 0
            For i = 0 To Game.Core.Player.Inventory.Count - 1
                c += Game.Core.Player.Inventory(i).Amount
            Next
            Return ToString(c)
        End Function

        <ScriptConstruct("Name")>
        <ScriptDescription("Returns the name of an item by its ID.")>
        Private Function F_Name(ByVal argument As String) As String
            Dim ItemID As String = argument

            Dim item As Item = Item.GetItemByID(Int(ItemID))

            If Not item Is Nothing Then
                Return item.Name
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("ID")>
        <ScriptDescription("Returns the ID of an item by its name.")>
        Private Function F_ID(ByVal argument As String) As String
            Dim item As Item = Item.GetItemByName(argument)
            If Not item Is Nothing Then
                Return ToString(item.ID)
            End If
            Return ToString(0)
        End Function

#End Region

    End Class

End Namespace