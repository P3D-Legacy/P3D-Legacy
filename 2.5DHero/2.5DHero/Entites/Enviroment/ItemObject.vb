Public Class ItemObject

    Inherits Entity

    Dim Item As Item
    Dim ItemID As Integer = 0
    Dim checkedExistence As Boolean = False

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Me.Item = Item.GetItemByID(CInt(Me.AdditionalValue.GetSplit(1)))
        Me.ItemID = CInt(Me.AdditionalValue.GetSplit(0))

        Me.Textures(0) = Me.Item.Texture
        If Me.ActionValue = 0 Then
            Me.Visible = Visible
        ElseIf Me.ActionValue = 1 Then
            Me.Visible = False
            Me.Collision = False
        End If

        Me.NeedsUpdate = True
    End Sub

    Public Overrides Sub Update()
        If checkedExistence = False Then
            checkedExistence = True

            If ItemExists(Me) = True Then
                RemoveItem(Me)
            End If
        End If

        If Me.IsHiddenItem() = True Then
            If Me.Opacity > 0.0F Then
                Me.Opacity -= 0.01F
                If Me.Opacity <= 0.0F Then
                    Me.Opacity = 1.0F
                    Me.Visible = False
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

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub ClickFunction()
        RemoveItem(Me)
        SoundManager.PlaySound("item_found", True)
        Screen.TextBox.TextColor = TextBox.PlayerColor
        Screen.TextBox.Show(Core.Player.Name & " found~" & Me.Item.Name & "!*" & Core.Player.Inventory.GetMessageReceive(Item, 1), {Me})
        Core.Player.Inventory.AddItem(Me.Item.ID, 1)
        PlayerStatistics.Track("Items found", 1)

        Core.Player.AddPoints(1, "Found an item.")
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

    Public Shared Function ItemExists(ByVal ItemObject As ItemObject) As Boolean
        If Core.Player.ItemData <> "" Then
            If Core.Player.ItemData.Contains(",") = True Then
                Dim IDs() As String = Core.Player.ItemData.ToLower().Split(CChar(","))

                If IDs.Contains((Screen.Level.LevelFile & "|" & ItemObject.ItemID.ToString()).ToLower()) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                If Core.Player.ItemData.ToLower() = (Screen.Level.LevelFile & "|" & ItemObject.ItemID.ToString()).ToLower() Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Sub RemoveItem(ByVal ItemObject As ItemObject)
        Screen.Level.Entities.Remove(ItemObject)

        If Core.Player.ItemData = "" Then
            Core.Player.ItemData = (Screen.Level.LevelFile & "|" & ItemObject.ItemID.ToString()).ToLower()
        Else
            Dim IDs() As String = Core.Player.ItemData.Split(CChar(","))
            If IDs.Contains((Screen.Level.LevelFile & "|" & ItemObject.ItemID.ToString()).ToLower()) = False Then
                Core.Player.ItemData &= "," & (Screen.Level.LevelFile & "|" & ItemObject.ItemID.ToString()).ToLower()
            End If
        End If
    End Sub

    Public Function IsHiddenItem() As Boolean
        If Me.Collision = False And Me.ActionValue = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class