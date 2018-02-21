Namespace Items.Medicine

    <Item(47, "Soda Pop")>
    Public Class SodaPop

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A highly carbonated soda drink. When consumed, it restores 60 HP to an injured Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 300

        Public Sub New()
            _textureRectangle = New Rectangle(24, 48, 24, 24)
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
            Return HealPokemon(PokeIndex, 60)
        End Function

    End Class

End Namespace
