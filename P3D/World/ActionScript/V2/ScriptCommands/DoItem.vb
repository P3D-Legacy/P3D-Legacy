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
                            Message = "<playername> handed over the~" & Item.OneLineName() & "!"
                        Else
                            Message = "<playername> handed over the~" & Item.OneLinePluralName() & "!"
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
                        Dim receiveString As String = Localization.GetString("item_received_single", "Received the~") & item.OneLineName() & ".*"
                        If Amount > 1 Then
                            receiveString = Localization.GetString("item_received_multiple", "Received") & " " & Amount & "~" & item.OneLinePluralName() & ".*"
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
                Case "select"
                    Dim allowedPages As Integer()
                    Dim allowedItems As New List(Of String)
                    Dim Inventory As NewInventoryScreen

                    If argument <> "" Then
                        Dim data() As String = argument.Split(CChar(","))

                        Dim pageNumbers As New List(Of Integer)
                        If data.Length > 0 Then
                            Dim typeData As String() = data(0).Split(CChar(";"))

                            If typeData.Count = 0 Or data(0) = "" Then
                                pageNumbers = {0, 1, 2, 3, 4, 5, 6, 7}.ToList
                            Else
                                For i = 0 To typeData.Count - 1
                                    If typeData(i).Contains("-1") Then
                                        pageNumbers = {0, 1, 2, 3, 4, 5, 6, 7}.ToList
                                        Exit For
                                    Else
                                        Select Case typeData(i).ToLower()
                                            Case "standard", "0"
                                                pageNumbers.Add(0)
                                            Case "medicine", "1"
                                                pageNumbers.Add(1)
                                            Case "plants", "2"
                                                pageNumbers.Add(2)
                                            Case "balls", "pokeballs", "3"
                                                pageNumbers.Add(3)
                                            Case "machines", "4"
                                                pageNumbers.Add(4)
                                            Case "keyitems", "5"
                                                pageNumbers.Add(5)
                                            Case "mail", "6"
                                                pageNumbers.Add(6)
                                            Case "battleitems", "7"
                                                pageNumbers.Add(7)
                                        End Select
                                    End If
                                Next
                            End If
                        End If
                        allowedPages = pageNumbers.ToArray


                        If data.Length > 1 Then
                            Dim itemData As String() = data(1).Split(CChar(";"))
                            If itemData.Count > 0 AndAlso data(1) <> "" Then
                                For i = 0 To itemData.Count - 1
                                    If itemData(i).Contains(CChar("-")) Then
                                        Dim MinMax As String() = itemData(i).Split(CChar("-"))
                                        Dim gmMinMax As String()
                                        If MinMax(0).StartsWith("gm") AndAlso MinMax(1).StartsWith("gm") Then
                                            gmMinMax = {MinMax(0).Remove(0, 2), MinMax(1).Remove(0, 2)}.ToArray
                                            For number = CInt(gmMinMax(0)) To CInt(gmMinMax(1))
                                                allowedItems.Add("gm" & number)
                                            Next
                                        Else
                                            For number = CInt(MinMax(0)) To CInt(MinMax(1))
                                                allowedItems.Add(number.ToString)
                                            Next
                                        End If
                                    ElseIf itemData(i).Contains("-1") Then
                                        allowedItems = {"-1"}.ToList
                                        Exit For
                                    Else
                                        allowedItems.Add(itemData(i))
                                    End If
                                Next
                            Else
                                allowedItems = {"-1"}.ToList
                            End If
                        Else
                            allowedItems = {"-1"}.ToList
                        End If

                        Inventory = New NewInventoryScreen(Core.CurrentScreen, allowedPages, Nothing, allowedItems, True)
                    Else
                        Inventory = New NewInventoryScreen(Core.CurrentScreen, allowedItems, True)
                    End If
                    If Inventory IsNot Nothing Then
                        Core.SetScreen(Inventory)

                        If CurrentScreen.Identification <> Screen.Identifications.InventoryScreen Then
                            IsReady = True
                        End If
                    End If
                    CanContinue = False
            End Select

            IsReady = True
        End Sub

    End Class

End Namespace