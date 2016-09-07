Namespace Items.Medicine

    Public Class Lemonade

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Lemonade", 350, ItemTypes.Medicine, 48, 1, 0, New Rectangle(48, 48, 24, 24), "A very sweet and refreshing drink. When consumed, it restores 80 HP to an injured Pokémon.")

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
            Return HealPokemon(PokeIndex, 80)
        End Function

    End Class

End Namespace