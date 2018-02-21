Namespace Items.Medicine

    <Item(121, "Energy Powder")>
    Public Class EnergyPowder

        Inherits MedicineItem

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500
        Public Overrides ReadOnly Property Description As String = "A bitter medicine powder. When consumed, it restores 50 HP to an injured Pokémon."
        Public Overrides ReadOnly Property IsHealingItem As Boolean = True

        Public Sub New()
            _textureRectangle = New Rectangle(0, 120, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim success As Boolean = HealPokemon(PokeIndex, 50)
            If success Then
                Core.Player.Pokemons(PokeIndex).ChangeFriendShip(Pokemon.FriendShipCauses.EnergyPowder)
            End If
            Return success
        End Function

    End Class

End Namespace
