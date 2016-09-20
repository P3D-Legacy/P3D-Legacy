Namespace Items

    Public MustInherit Class StoneItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
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
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New EvolutionScreen(Core.CurrentScreen, {PokeIndex}.ToList(), Me.ID.ToString(), EvolutionCondition.EvolutionTrigger.ItemUse), Color.Black, False))

                PlayerStatistics.Track("[22]Evolution stones used", 1)
                RemoveItem()

                Return True
            Else
                Screen.TextBox.Show("Cannot use on~" & p.GetDisplayName(), {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace
