Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @item commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoItem(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "give"
                    Dim amount As Integer = 1
                    Dim ItemID As String = argument
                    Dim item As Item
                    If argument.Contains(",") = True Then
                        amount = int(argument.GetSplit(1))
                        ItemID = argument.GetSplit(0)
                    End If

                    If ScriptConversion.IsArithmeticExpression(ItemID) = False AndAlso ItemID.StartsWith("gm") = False Then
                        item = Item.GetItemByName(ItemID)
                    Else
                        item = Item.GetItemByID(ItemID)
                    End If
                    If Not item Is Nothing Then
                        Dim _itemID As String
                        If item.IsGameModeItem Then
                            _itemID = item.gmID
                        Else
                            _itemID = item.ID.ToString
                        End If
                        If item.MaxStack < Core.Player.Inventory.GetItemAmount(_itemID) + amount Then
                            amount = int(item.MaxStack - Core.Player.Inventory.GetItemAmount(_itemID)).Clamp(0, 999)
                        End If
                        Core.Player.Inventory.AddItem(_itemID, amount)
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

                    Dim Item As Item = Item.GetItemByID(ItemID)

                    Dim _itemID As String
                    If Item.IsGameModeItem Then
                        _itemID = Item.gmID
                    Else
                        _itemID = Item.ID.ToString
                    End If

                    Core.Player.Inventory.RemoveItem(_itemID, amount)

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
                        Dim ItemID As String = argument
                        Dim amount As Integer = Core.Player.Inventory.GetItemAmount(ItemID)

                        If amount > 0 Then
                            Core.Player.Inventory.RemoveItem(ItemID, amount)
                        End If
                    Else
                        Core.Player.Inventory.Clear()
                    End If
                Case "messagegive"
                    Dim ItemID As String = argument.GetSplit(0)
                    Dim item As Item
                    If ScriptConversion.IsArithmeticExpression(ItemID) = False AndAlso ItemID.StartsWith("gm") = False Then
                        item = Item.GetItemByName(ItemID)
                    Else
                        item = Item.GetItemByID(ItemID)
                    End If

                    Dim Amount As Integer = int(argument.GetSplit(1))

                    If Not item Is Nothing Then
                        Dim receiveString As String = "Received the~" & item.Name & ".*"
                        If Amount > 1 Then
                            receiveString = "Received " & Amount & "~" & item.PluralName & ".*"
                        End If

                        SoundManager.PlaySound("Receive_Item", True)

                        Screen.TextBox.reDelay = 0.0F
                        Screen.TextBox.TextColor = TextBox.PlayerColor
                        Screen.TextBox.Show(receiveString & Core.Player.Inventory.GetMessageReceive(item, Amount), {})

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