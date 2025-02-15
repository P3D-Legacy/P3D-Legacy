Namespace Items.Medicine

    <Item(16, "Hyper Potion")>
    Public Class HyperPotion

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A spray-type medicine for treating wounds. It can be used to restore 120 HP to an injured Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1500

        Public Sub New()
            _textureRectangle = New Rectangle(336, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItems", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealPokemon(PokeIndex, 120)
        End Function

    End Class

End Namespace
