Namespace Items.Medicine

    <Item(17, "Super Potion")>
    Public Class SuperPotion

        Inherits MedicineItem

        Public Sub New()
            MyBase.New("Super Potion", 700, ItemTypes.Medicine, 17, 1, 0, New Rectangle(360, 0, 24, 24), "A spray-type medicine for treating wounds. It can be used to restore 50 HP to an injured Pokémon.")

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
            Return HealPokemon(PokeIndex, 50)
        End Function

    End Class

End Namespace
