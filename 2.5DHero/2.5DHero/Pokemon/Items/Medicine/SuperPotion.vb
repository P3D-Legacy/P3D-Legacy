Namespace Items.Medicine

    <Item(17, "Super Potion")>
    Public Class SuperPotion

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A spray-type medicine for treating wounds. It can be used to restore 60 HP to an injured Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 700

        Public Sub New()
            _textureRectangle = New Rectangle(360, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealPokemon(PokeIndex, 60)
        End Function

    End Class

End Namespace
