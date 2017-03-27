Namespace Items.Medicine

    <Item(114, "RageCandyBar")>
    Public Class RageCandyBar

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A famous Mahogany Town candy tourists like to buy and take home. It restores the HP of one Pokémon by 20 points."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 300

        Public Sub New()
            _textureRectangle = New Rectangle(360, 96, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(OldGameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealPokemon(PokeIndex, 20)
        End Function

    End Class

End Namespace
