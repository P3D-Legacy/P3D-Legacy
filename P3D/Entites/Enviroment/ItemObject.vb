Public Class ItemObject

    Inherits Entity

    Shared AnimationTexturesTemp As New Dictionary(Of String, Texture2D)
    Dim AnimationName As String = ""
    Dim Animation As Animation = Nothing

    Dim Item As Item
    Dim ItemID As Integer = 0
    Dim checkedExistence As Boolean = False
    Dim AnimationPath As String = ""
    Dim X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn As Integer
    Dim CurrentRectangle As New Rectangle(0, 0, 0, 0)
    Dim CanInteractWith As Boolean = True

    Public Overloads Sub Initialize(Optional ByVal AnimationData As List(Of List(Of Integer)) = Nothing)
        MyBase.Initialize()

        Me.Item = Item.GetItemByID(CInt(Me.AdditionalValue.GetSplit(1)))
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
        If CanInteractWith Then
            RemoveItem(Me)
            If Me.Item.Name.Contains("HM") Then
                SoundManager.PlaySound("Receive_HM", True)
            Else
                SoundManager.PlaySound("Receive_Item", True)
            End If
            Screen.TextBox.TextColor = TextBox.PlayerColor
            Screen.TextBox.Show(Core.Player.Name & " found~" & Me.Item.Name & "!*" & Core.Player.Inventory.GetMessageReceive(Item, 1), {Me})
            Core.Player.Inventory.AddItem(Me.Item.ID, 1)
            PlayerStatistics.Track("Items found", 1)

            Core.Player.AddPoints(1, "Found an item.")
        End If
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, False)
        Else
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
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