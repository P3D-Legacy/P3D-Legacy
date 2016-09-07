Namespace Items.Berries

    Public Class OranBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2006, "Oran", 10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can restore its own HP by 10 points during battle.", "3.5cm", "Super Hard", 2, 3)

            Me.Spicy = 10
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 10
            Me.Sour = 10

            Me.Type = Element.Types.Poison
            Me.Power = 60

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
            Return HealPokemon(PokeIndex, 10)
        End Function

    End Class

End Namespace