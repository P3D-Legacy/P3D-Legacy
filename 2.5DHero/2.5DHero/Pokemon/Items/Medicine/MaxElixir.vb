Namespace Items.Medicine

    <Item(21, "Max Elixir")>
    Public Class MaxElixir

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "This medicine can fully restore the PP of all of the moves that have been learned by a Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4500

        Public Sub New()
            _textureRectangle = New Rectangle(456, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim missingPP As Boolean = False
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            For Each Attack As BattleSystem.Attack In Pokemon.Attacks
                If Attack.CurrentPP < Attack.MaxPP Then
                    missingPP = True
                End If
                Attack.CurrentPP = Attack.MaxPP
            Next

            If missingPP = True Then
                Dim t As String = "Restored PP of~" & Pokemon.GetDisplayName() & "'s attacks."
                t &= RemoveItem()
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(t, {}, True, True)
                Return True
            Else
                Screen.TextBox.Show("The Pokémon's PP are~full already.", {}, True, True)
                Return False
            End If
        End Function

    End Class

End Namespace
