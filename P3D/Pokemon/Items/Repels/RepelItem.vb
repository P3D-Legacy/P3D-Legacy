Namespace Items.Repels

    Public MustInherit Class RepelItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public MustOverride ReadOnly Property RepelSteps As Integer

        Public Overrides Sub Use()
            If Core.Player.RepelSteps <= 0 Then
                Player.Temp.LastUsedRepel = ID
                SoundManager.PlaySound("Use_Repel", False)
                Core.Player.RepelSteps = RepelSteps
                PlayerStatistics.Track("[42]Repels used", 1)
                Dim t As String = Core.Player.Name & " used a~" & Name & "."
                t &= RemoveItem()
                Screen.TextBox.Show(t, {}, True, True)
            Else
                Screen.TextBox.Show("The Repel is still~in effect.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
