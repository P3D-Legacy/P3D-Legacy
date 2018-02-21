Namespace Items.Repels

    Public MustInherit Class RepelItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public MustOverride ReadOnly Property RepelSteps As Integer

        Public Overrides Sub Use()
            If Core.Player.RepelSteps <= 0 Then
                Core.Player.Inventory.RemoveItem(ID, 1)
                Player.Temp.LastUsedRepel = ID

                SoundManager.PlaySound("repel_use", False)
                Screen.TextBox.Show(Core.Player.Name & " used a~" & Name, {}, True, True)
                Core.Player.RepelSteps = RepelSteps
                PlayerStatistics.Track("[42]Repels used", 1)
            Else
                Screen.TextBox.Show("The Repel is still~in effect.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
