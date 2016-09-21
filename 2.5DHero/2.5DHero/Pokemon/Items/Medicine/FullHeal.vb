Namespace Items.Medicine

    <Item(38, "Full Heal")>
    Public Class FullHeal

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A spray-type medicine that is broadly effective. It can be used once to heal all the status conditions of a Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 600

        Public Sub New()
            _textureRectangle = New Rectangle(336, 24, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            Else
                If Pokemon.Status <> net.Pokemon3D.Game.Pokemon.StatusProblems.None Or Pokemon.HasVolatileStatus(Game.Pokemon.VolatileStatus.Confusion) = True Then
                    Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None

                    If Pokemon.HasVolatileStatus(Game.Pokemon.VolatileStatus.Confusion) = True Then
                        Pokemon.RemoveVolatileStatus(Game.Pokemon.VolatileStatus.Confusion)
                    End If

                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = Pokemon.GetDisplayName() & "~gets healed up!"
                    t &= RemoveItem()

                    SoundManager.PlaySound("single_heal", False)
                    Screen.TextBox.Show(t, {})
                    PlayerStatistics.Track("[17]Medicine Items used", 1)

                    Return True
                Else
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fully healed!", {}, True, True)

                    Return False
                End If
            End If
        End Function

    End Class

End Namespace
