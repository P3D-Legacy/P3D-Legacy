Public Class LoamySoil

    Inherits Entity

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Me.Visible = False
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
            Core.SetScreen(New InventoryScreen(Core.CurrentScreen, {4}, 4, AddressOf Me.PlantBerry))
        End If
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
        Me.Draw(Me.Model, Textures, False)
    End Sub

End Class