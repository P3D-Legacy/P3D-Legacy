Namespace Items.Medicine

    <Item(10, "Burn Heal")>
    Public Class BurnHeal

        Inherits MedicineItem

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 250
        Public Overrides ReadOnly Property Description As String = "A spray-type medicine for treating burns. It can be used once to heal a Pokemon suffering from a burn."

        Public Sub New()
            _textureRectangle = New Rectangle(192, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pok�mon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealBurn(PokeIndex)
        End Function

    End Class

End Namespace
