Namespace Items.Medicine

    Public Class BerryJuice

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Berry Juice", 100, ItemTypes.Medicine, 139, 1, 0, New Rectangle(360, 120, 24, 24), "A 100 percent pure juice made of Berries. When consumed, it restores 20 HP to an injured Pokémon.")

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
            Return HealPokemon(PokeIndex, 20)
        End Function

    End Class

End Namespace