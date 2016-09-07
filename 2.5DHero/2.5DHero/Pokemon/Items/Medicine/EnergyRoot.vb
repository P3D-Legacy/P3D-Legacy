Namespace Items.Medicine

    Public Class EnergyRoot

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Energy Root", 800, ItemTypes.Medicine, 122, 1, 0, New Rectangle(24, 120, 24, 24), "An extremely bitter medicinal root. When consumed, it restores 200 HP to an injured Pokémon.")

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
            Dim r As Boolean = HealPokemon(PokeIndex, 200)
            If r = True Then
                Core.Player.Pokemons(PokeIndex).ChangeFriendShip(Pokemon.FriendShipCauses.EnergyRoot)
            End If
            Return r
        End Function

    End Class

End Namespace