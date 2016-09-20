Namespace Items.Medicine

    <Item(72, "Moomoo Milk")>
    Public Class MooMooMilk

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A bottle of highly nutritious milk. When consumed, it restores 100 HP to an injured Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500

        Public Sub New()
            _textureRectangle = New Rectangle(72, 72, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealPokemon(PokeIndex, 100)
        End Function

    End Class

End Namespace
