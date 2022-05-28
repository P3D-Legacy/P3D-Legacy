Namespace Items.Medicine

    <Item(39, "Revive")>
    Public Class Revive

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A medicine that can revive fainted Pokémon. It also restores half of a fainted Pokémon's HP."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1500

        Public Sub New()
            _textureRectangle = New Rectangle(360, 24, 24, 24)
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
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then
                Pokemon.Status = P3D.Pokemon.StatusProblems.None
                Pokemon.HP = CInt(Math.Floor(Pokemon.MaxHP / 2))

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is revitalized.", {}, False, False)
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                RemoveItem()

                Return True
            Else
                Screen.TextBox.Show("Cannot use revive~on this Pokémon.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace
