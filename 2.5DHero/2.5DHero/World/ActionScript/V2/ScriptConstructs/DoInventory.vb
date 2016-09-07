Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <inventory> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoInventory(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "countitem"
                    Dim ItemID As Integer = int(argument.GetSplit(0))

                    Return Core.Player.Inventory.GetItemAmount(ItemID)
                Case "countitems"
                    Dim c As Integer = 0
                    For i = 0 To Core.Player.Inventory.Count - 1
                        c += Core.Player.Inventory(i).Amount
                    Next
                    Return c
                Case "name"
                    Dim ItemID As String = argument

                    Return Item.GetItemByID(int(ItemID)).Name
                Case "id"
                    Dim item As Item = Item.GetItemByName(argument)
                    If Not item Is Nothing Then
                        Return item.ID
                    End If
                    Return 0
            End Select
            Return DEFAULTNULL
        End Function

    End Class

End Namespace