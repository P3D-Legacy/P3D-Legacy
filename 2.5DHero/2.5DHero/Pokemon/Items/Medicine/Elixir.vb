Namespace Items.Medicine

    Public Class Elixir

        Inherits Item

        Public Sub New()
            MyBase.New("Elixir", 3000, ItemTypes.Medicine, 65, 1, 1, New Rectangle(408, 48, 24, 24), "This medicine can restore 10 PP to each of the moves that have been learned by a Pokémon.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
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
                Attack.CurrentPP = CInt(MathHelper.Clamp(Attack.CurrentPP + 10, 0, Attack.MaxPP))
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