Namespace Items.Medicine

    <Item(15, "Max Potion")>
    Public Class MaxPotion

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A spray-type medicine for treating wounds. It will completely restore the max HP of a single Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2500

        Public Sub New()
            _textureRectangle = New Rectangle(312, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            Return HealPokemon(PokeIndex, p.MaxHP)
        End Function

    End Class

End Namespace
