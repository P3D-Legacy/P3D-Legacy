Namespace Items

    Public MustInherit Class StoneItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
                CType(CurrentScreen, PartyScreen).EvolutionItemID = Me.ID.ToString
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If

        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return Me.UseStone(PokeIndex)
        End Function

        Public Function UseStone(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.IsEgg() = False And p.CanEvolve(EvolutionCondition.EvolutionTrigger.ItemUse, Me.ID.ToString()) = True Then
                RemoveItem()

                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New EvolutionScreen(Core.CurrentScreen, {PokeIndex}.ToList(), Me.ID.ToString(), EvolutionCondition.EvolutionTrigger.ItemUse), Color.Black, False))

                PlayerStatistics.Track("[22]Evolution stones used", 1)

                Return True
            Else
                Screen.TextBox.Show("Cannot use on~" & p.GetDisplayName(), {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace
