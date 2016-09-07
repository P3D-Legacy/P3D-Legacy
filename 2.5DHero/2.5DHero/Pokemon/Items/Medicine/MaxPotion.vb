Namespace Items.Medicine

    Public Class MaxPotion

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Max Potion", 2500, ItemTypes.Medicine, 15, 1, 0, New Rectangle(312, 0, 24, 24), "A spray-type medicine for treating wounds. It will completely restore the max HP of a single Pokémon.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._isHealingItem = True
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