Public Class LoamySoil

    Inherits Entity

    Public Overrides Sub Initialize()
        MyBase.Initialize()
        If Me.Model Is Nothing Then
            Me.Visible = False
        Else
            Me.Visible = True
        End If
    End Sub

    Public Overrides Sub ClickFunction()
        Dim hasBerry As Boolean = False
        For Each Entity As Entity In Screen.Level.Entities
            If Entity.EntityID = "BerryPlant" And Entity.Position = Me.Position Then
                hasBerry = True
                Entity.ClickFunction()
                Exit For
            End If
        Next
        If hasBerry = False Then
            Screen.TextBox.Show("Do you want to plant a~berry here?%Yes|No%", {Me})
            SoundManager.PlaySound("select")
        End If
    End Sub

    Public Overrides Sub ResultFunction(ByVal Result As Integer)
        If Result = 0 Then
            Dim selScreen As New NewInventoryScreen(Core.CurrentScreen, {2}, 2, Nothing)
            selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection
            selScreen.CanExit = True

            AddHandler selScreen.SelectedObject, AddressOf PlantBerryHandler
            Core.SetScreen(selScreen)
        End If
    End Sub

    Public Sub PlantBerryHandler(ByVal params As Object())
        PlantBerry(CInt(params(0)))
    End Sub

    Public Sub PlantBerry(ByVal ChosenBerry As Integer)
        Dim testItem As Item = Item.GetItemByID(ChosenBerry)
        If testItem.isBerry = True Then
            Dim Berry As Items.Berry = CType(Item.GetItemByID(ChosenBerry), Items.Berry)

            BerryPlant.AddBerryPlant(Screen.Level.LevelFile, Me.Position, Berry.BerryIndex)
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show("You planted a~" & Berry.Name & " Berry here.", {})
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

End Class