Namespace Items.Medicine

    Public Class Fanta

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Fanta", 750, ItemTypes.Medicine, 266, 1, 0, New Rectangle(48, 264, 24, 24), "Fizzzzzz. When consumed, it restores 250 HP to an injured Pokémon.")

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
            Return HealPokemon(PokeIndex, 250)
        End Function

    End Class

End Namespace