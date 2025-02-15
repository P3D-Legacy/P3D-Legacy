Namespace Items.Medicine

    <Item(13, "Paralyze Heal")>
    Public Class ParalyzeHeal

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "A spray-type medicine for paralysis. It can be used once to free a Pokémon that has been paralyzed."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200

        Public Sub New()
            _textureRectangle = New Rectangle(264, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealParalyze(PokeIndex)
        End Function

    End Class

End Namespace
