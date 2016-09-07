Namespace Items.Medicine

    Public Class RageCandyBar

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("RageCandyBar", 300, ItemTypes.Medicine, 114, 1, 0, New Rectangle(360, 96, 24, 24), "A famous Mahogany Town candy tourists like to buy and take home. It restores the HP of one Pokémon by 20 points.")

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