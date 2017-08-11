Namespace Items.Berries

    <Item(2009, "Sitrus")>
    Public Class SitrusBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(21600, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can restore its own HP by a small amount during battle.", "9.5cm", "Very Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 10
            Me.Sour = 10

            Me.Type = Element.Types.Psychic
            Me.Power = 60
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)
            Return HealPokemon(PokeIndex, CInt(Math.Ceiling(p.MaxHP / 4)))
        End Function

    End Class

End Namespace
