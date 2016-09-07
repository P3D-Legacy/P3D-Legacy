Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @item commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoItem(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "give"
                    Dim amount As Integer = 1
                    Dim ItemID As String = argument
                    If argument.Contains(",") = True Then
                        amount = int(argument.GetSplit(1))
                        ItemID = argument.GetSplit(0)
                    End If

                    If ScriptConversion.IsArithmeticExpression(ItemID) = False Then
                        Dim item As Item = Item.GetItemByName(ItemID)

                        If Not item Is Nothing Then
                            Core.Player.Inventory.AddItem(item.ID, amount)
                        End If
                    Else
                        Core.Player.Inventory.AddItem(int(ItemID), amount)
                    End If
                Case "remove"
                    Dim amount As Integer = 1
                    Dim ItemID As String = argument
                    Dim showMessage As Boolean = True

                    If argument.Contains(",") = True Then
                        amount = int(argument.GetSplit(1))
                        ItemID = argument.GetSplit(0)
                        If argument.CountSeperators(",") >= 2 Then
                            showMessage = CBool(argument.GetSplit(2))
                        End If
                    End If

                    Dim Item As Item = Item.GetItemByID(int(ItemID))

                    Core.Player.Inventory.RemoveItem(Item.ID, amount)

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

                        CanContinue = False
                    End If
                Case "clearitem"
                    If argument <> "" Then
                        Dim ItemID As Integer = int(argument)
                        Dim amount As Integer = Core.Player.Inventory.GetItemAmount(ItemID)

                        If amount > 0 Then
                            Core.Player.Inventory.RemoveItem(ItemID, amount)
                        End If
                    Else
                        Core.Player.Inventory.Clear()
                    End If
                Case "messagegive"
                    Dim ItemID As String = argument.GetSplit(0)
                    Dim Item As Item = Nothing
                    If ScriptConversion.IsArithmeticExpression(ItemID) = False Then
                        Item = Item.GetItemByName(ItemID)
                    Else
                        Item = Item.GetItemByID(int(ItemID))
                    End If

                    Dim Amount As Integer = int(argument.GetSplit(1))

                    If Not Item Is Nothing Then
                        Dim receiveString As String = "Received the~" & Item.Name & ".*"
                        If Amount > 1 Then
                            receiveString = "Received " & Amount & "~" & Item.PluralName & ".*"
                        End If

                        SoundManager.PlaySound("item_found", True)

                        Screen.TextBox.reDelay = 0.0F
                        Screen.TextBox.TextColor = TextBox.PlayerColor
                        Screen.TextBox.Show(receiveString & Core.Player.Inventory.GetMessageReceive(Item, Amount), {})

                        CanContinue = False
                    End If
                Case "repel"
                    Dim itemID As Integer = int(argument)
                    Dim steps As Integer = 0
                    Select Case itemID
                        Case 20
                            steps = 100
                        Case 42
                            steps = 200
                        Case 43
                            steps = 250
                    End Select
                    Core.Player.RepelSteps += steps
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace