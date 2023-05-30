Namespace ScriptVersion2

    Partial Class ScriptComparer

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the <inventory> constructs.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoInventory(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "countitem"
                    Dim ItemID As String = argument.GetSplit(0)

                    Return Core.Player.Inventory.GetItemAmount(ItemID)
                Case "countitems"
                    Dim c As Integer = 0
                    For i = 0 To Core.Player.Inventory.Count - 1
                        c += Core.Player.Inventory(i).Amount
                    Next
                    Return c
                Case "name"
                    Dim ItemID As String = argument.GetSplit(0)

                    If argument.Contains(",") Then
                        Select Case argument.GetSplit(1).ToLower()
                            Case "p", "plural"
                                Return Item.GetItemByID(ItemID).PluralName
                            Case "s", "singular"
                                Return Item.GetItemByID(ItemID).Name
                        End Select
                    End If

                    Return Item.GetItemByID(ItemID).Name
                Case "id"
                    Dim item As Item = Item.GetItemByName(argument)
                    If Not item Is Nothing Then
                        Return item.ID
                    End If
                    Return 0
                Case "juicecolor"
                    Dim ItemID As String = argument.GetSplit(0)
                    If Item.GetItemByID(ItemID).PluralName.ToLower.EndsWith("berries") Then
                        Dim b As Items.Berry = CType(Item.GetItemByID(ItemID), Items.Berry)

                        Return b.JuiceColor
                    End If

                    Return "black"
                Case "juicegroup"
                    Dim ItemID As String = argument.GetSplit(0)
                    If Item.GetItemByID(ItemID).PluralName.ToLower.EndsWith("berries") Then
                        Dim b As Items.Berry = CType(Item.GetItemByID(ItemID), Items.Berry)

                        Return b.JuiceGroup
                    End If

                    Return 0

            End Select
            Return DEFAULTNULL
        End Function

    End Class

End Namespace