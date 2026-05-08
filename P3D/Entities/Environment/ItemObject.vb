Enum PickupTypes
    Item
    Money
    Coins
    BattlePoints
End Enum
Public Class ItemObject

    Inherits Entity

    Shared AnimationTexturesTemp As New Dictionary(Of String, Texture2D)
    Dim AnimationName As String = ""
    Dim Animation As Animation = Nothing

    Dim LevelName As String = ""
    Dim PickupType As PickupTypes = PickupTypes.Item
    Dim PickupAmount As Integer = 1
    Dim Item As Item
    Dim ItemID As Integer = 0
    Dim checkedExistence As Boolean = False
    Dim AnimationPath As String = ""
    Dim X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn As Integer
    Dim CurrentRectangle As New Rectangle(0, 0, 0, 0)
    Dim CanInteractWith As Boolean = True
    Public HiddenDelay As Date = Nothing

    Public Overloads Sub Initialize(Optional ByVal AnimationData As List(Of List(Of Integer)) = Nothing)
        MyBase.Initialize()

        If StringHelper.IsNumeric(Me.AdditionalValue.GetSplit(1)) = False Then

            Select Case Me.AdditionalValue.GetSplit(1).ToLower
                Case "money"
                    Me.PickupType = PickupTypes.Money
                Case "coins"
                    Me.PickupType = PickupTypes.Coins
                Case "battlepoints"
                    Me.PickupType = PickupTypes.BattlePoints
            End Select
            Me.PickupAmount = CInt(Me.AdditionalValue.GetSplit(2).ToLower())

            If Me.AdditionalValue.Split(",").Count > 3 Then
                Me.LevelName = Me.AdditionalValue.GetSplit(3).ToLower()
            End If
            Me.Visible = False
            Me.Collision = False
        Else
            Me.PickupType = PickupTypes.Item
            If Me.AdditionalValue.Split(",").Count > 2 Then
                Me.LevelName = Me.AdditionalValue.GetSplit(2).ToLower()
            End If

            Me.Item = Item.GetItemByID(Me.AdditionalValue.GetSplit(1))
            Me.ItemID = CInt(Me.AdditionalValue.GetSplit(0))

            Me.Textures(0) = Me.Item.Texture
            If Me.ActionValue = 0 Then
                Me.Visible = Visible
            ElseIf Me.ActionValue = 1 Then
                Me.Visible = False
                Me.Collision = False
            ElseIf Me.ActionValue = 2 Then
                If Core.Player.Inventory.HasMegaBracelet() Then
                    Me.Visible = Visible
                    'sparkles
                    If AnimationData IsNot Nothing Then
                        X = AnimationData(0)(0)
                        Y = AnimationData(0)(1)
                        width = AnimationData(0)(2)
                        height = AnimationData(0)(3)
                        rows = AnimationData(0)(4)
                        columns = AnimationData(0)(5)
                        animationSpeed = AnimationData(0)(6)
                        startRow = AnimationData(0)(7)
                        startColumn = AnimationData(0)(8)
                        AnimationPath = "ItemAnimations"
                    Else
                        X = 0
                        Y = 0
                        width = 48
                        height = 48
                        rows = 5
                        columns = 10
                        animationSpeed = 60
                        startRow = 0
                        startColumn = 0
                        AnimationPath = "SparkleAnimation"
                    End If
                    CreateAnimationTextureTemp()

                    Me.Animation = New Animation(TextureManager.GetTexture("Textures\Routes"), rows, columns, 16, 16, animationSpeed, startRow, startColumn)

                Else
                    Me.Visible = False
                    Me.Collision = False
                    CanInteractWith = False
                End If
            End If
        End If

        Me.NeedsUpdate = True
    End Sub

    Public Shared Sub ClearAnimationResources()
        AnimationTexturesTemp.Clear()
    End Sub

    Private Sub CreateAnimationTextureTemp()
        'If Core.GameOptions.GraphicStyle = 1 Then
        Dim r As New Rectangle(X, Y, width, height)
        Me.AnimationName = AnimationPath & "," & X & "," & Y & "," & height & "," & width
        If AnimationTexturesTemp.ContainsKey(AnimationName & "_0") = False Then
            For i = 0 To Me.rows - 1
                For j = 0 To Me.columns - 1
                    AnimationTexturesTemp.Add(AnimationName & "_" & (j + columns * i).ToString, TextureManager.GetTexture(AnimationPath, New Rectangle(r.X + r.Width * j, r.Y + r.Height * i, r.Width, r.Height)))
                Next
            Next
        End If
    End Sub

    Public Overrides Sub Update()
        If checkedExistence = False Then
            checkedExistence = True

            If ItemExists(Me) = True Then
                If Me.LevelName = "" Then
                    Me.LevelName = Screen.Level.LevelFile.ToLower()
                End If
                RemoveItem(Me, Me.LevelName)
            End If
        End If

        If Me.IsHiddenItem() = True Then
            If HiddenDelay <> Nothing AndAlso Date.Now >= HiddenDelay Then
                If Me.Opacity > 0.0F Then
                    Me.Opacity -= 0.01F
                    If Me.Opacity <= 0.0F Then
                        Me.Opacity = 0.0F
                        Me.Visible = False
                    End If
                    Me.NormalOpacity = Me.Opacity
                End If
            End If
        End If

        MyBase.Update()
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
            Me.CreatedWorld = False
        End If

        If Not Animation Is Nothing Then
            Animation.Update(0.01)
            If CurrentRectangle <> Animation.TextureRectangle Then
                ChangeTexture()
                CurrentRectangle = Animation.TextureRectangle
            End If
        End If

        MyBase.UpdateEntity()
    End Sub

    Private Sub ChangeTexture()
        'If Core.GameOptions.GraphicStyle = 1 Then

        If AnimationTexturesTemp.Count = 0 Then
            ClearAnimationResources()
            CreateAnimationTextureTemp()
        End If
        Dim i = Animation.CurrentRow
        Dim j = Animation.CurrentColumn
        Me.Textures(0) = ItemObject.AnimationTexturesTemp(AnimationName & "_" & (j + columns * i))

        'End If
    End Sub

    Public Overrides Sub ClickFunction()
        If CanInteractWith = True AndAlso (Me.PickupType = PickupTypes.Coins AndAlso Core.Player.Inventory.GetItemAmount(54.ToString) = 0 OrElse
            Me.PickupType = PickupTypes.BattlePoints AndAlso ActionScript.IsRegistered("pokegear_card_frontier") = False) Then
            CanInteractWith = False
        End If
        If CanInteractWith Then
            If Me.LevelName = "" Then
                Me.LevelName = Screen.Level.LevelFile.ToLower()
            End If
            RemoveItem(Me, Me.LevelName)

            Screen.TextBox.TextColor = TextBox.PlayerColor
            Dim foundString As String = ""

            If Me.PickupType = PickupTypes.Item Then
                foundString = Localization.GetString("item_found", "<player.name> found~").Replace("<player.name>", Core.Player.Name) & Me.Item.OneLineName()
                If Me.Item.OriginalName.Contains("HM") Then
                    SoundManager.PlaySound("Receive_HM", True)
                Else
                    SoundManager.PlaySound("Receive_Item", True)
                End If

                If Item.ItemType = Items.ItemTypes.Machines Then
                    If Item.IsGameModeItem = True Then
                        foundString &= " " & CType(Item, GameModeItem).gmTeachMove.Name & "!*"
                    Else
                        foundString &= " " & CType(Item, Items.TechMachine).Attack.Name & "!*"
                    End If
                Else
                    foundString &= "!*"
                End If

                Screen.TextBox.Show(foundString & Core.Player.Inventory.GetMessageReceive(Item, 1), {Me})
                Dim ItemID As String
                If Me.Item.IsGameModeItem Then
                    ItemID = Me.Item.gmID
                Else
                    ItemID = Me.Item.ID.ToString
                End If
                Core.Player.Inventory.AddItem(ItemID, 1)
                Core.Player.CheckItemCountScriptDelay(ItemID)
                PlayerStatistics.Track("Items found", 1)

                Core.Player.AddPoints(1, "Found an item.")
            Else
                Select Case PickupType
                    Case PickupTypes.Money
                        foundString = Localization.GetString("currency_found_money", "<Player.Name> found $[AMOUNT]!").Replace("<player.name>", Core.Player.Name).Replace("[AMOUNT]", Me.PickupAmount.ToString)
                        Core.Player.Money += Math.Max(1, PickupAmount)
                    Case PickupTypes.Coins
                        If PickupAmount <= 1 Then
                            foundString = Localization.GetString("currency_found_coins_single", "<Player.Name> found~a coin!").Replace("<player.name>", Core.Player.Name)
                        Else
                            foundString = Localization.GetString("currency_found_coins_multiple", "<Player.Name> found~[AMOUNT] coins!").Replace("<player.name>", Core.Player.Name).Replace("[AMOUNT]", Me.PickupAmount.ToString)
                        End If
                        Dim coins As Integer = PickupAmount

                        If CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) > 0 AndAlso Core.Player.Coins + coins > CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) Then
                            coins = CInt(GameModeManager.GetGameRuleValue("CoinCaseCap", "0")) - Core.Player.Coins
                        End If

                        Core.Player.Coins += Math.Max(1, PickupAmount)

                        If coins > 0 Then
                            PlayerStatistics.Track("Obtained Coins", coins)
                        End If
                    Case PickupTypes.BattlePoints
                        If PickupAmount <= 1 Then
                            foundString = Localization.GetString("currency_found_battlepoints_single", "<Player.Name> found~a Battle Point!").Replace("<player.name>", Core.Player.Name)
                        Else
                            foundString = Localization.GetString("currency_found_battlepoints_multiple", "<Player.Name> found~[AMOUNT] Battle Points!").Replace("<player.name>", Core.Player.Name).Replace("[AMOUNT]", Me.PickupAmount.ToString)
                        End If
                        Dim bp As Integer = Math.Max(1, PickupAmount)

                        Core.Player.BP += bp

                        If bp > 0 Then
                            PlayerStatistics.Track("Obtained BP", bp)
                        End If
                End Select
                SoundManager.PlaySound("Receive_Item", True)
                Screen.TextBox.Show(foundString)

            End If

        End If
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

    Public Function ItemExists(ByVal ItemObject As ItemObject) As Boolean
        If Core.Player.ItemData <> "" Then
            If Core.Player.ItemData.Contains(",") = True Then
                Dim IDs() As String = Core.Player.ItemData.ToLower().Split(CChar(","))

                If IDs.Contains((Screen.Level.LevelFile.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower()) = True OrElse IDs.Contains((Me.LevelName.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower()) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                If Core.Player.ItemData.ToLower() = (Screen.Level.LevelFile.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower() OrElse Core.Player.ItemData.ToLower() = (Me.LevelName.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower() Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Sub RemoveItem(ByVal ItemObject As ItemObject, ByVal LevelName As String)
        Screen.Level.Entities.Remove(ItemObject)

        If Core.Player.ItemData = "" Then
            Core.Player.ItemData = (LevelName.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower()
        Else
            Dim IDs() As String = Core.Player.ItemData.Split(CChar(","))
            If IDs.Contains((LevelName.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower()) = False Then
                Core.Player.ItemData &= "," & (LevelName.ToLower() & "|" & ItemObject.ItemID.ToString()).ToLower()
            End If
        End If
    End Sub

    Public Function IsHiddenItem() As Boolean
        If Me.Collision = False AndAlso Me.ActionValue = 1 AndAlso Me.PickupType = PickupTypes.Item Then
            Return True
        Else
            Return False
        End If
    End Function

End Class