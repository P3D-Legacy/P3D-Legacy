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
                Dim CorrectTokenName As String = "item_use_RegularItem_A"
                Dim CorrectTokenContent As String = "<Player.Name> used~a [ITEMNAME]."
                If "aoeiu".Contains(Name(0)) Then
                    CorrectTokenName = "item_use_RegularItem_An"
                    CorrectTokenContent = "<Player.Name> used~an [ITEMNAME]."
                End If
                Dim t As String = Localization.GetString(CorrectTokenName, CorrectTokenContent).Replace("[ITEMNAME]", Name)
                t &= RemoveItem()
                Screen.TextBox.Show(t, {}, True, True)
            Else
                Screen.TextBox.Show(Localization.GetString("item_cannot_use_RepelItem", "The Repel is still~in effect."), {}, True, True)
            End If
        End Sub

    End Class

End Namespace
