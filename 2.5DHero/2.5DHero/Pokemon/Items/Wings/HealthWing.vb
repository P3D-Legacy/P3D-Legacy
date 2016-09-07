Namespace Items.Wings

    Public Class HealthWing

        Inherits WingItem

        Public Sub New()
            MyBase.New("Health Wing", 3000, ItemTypes.Medicine, 254, 1.0F, 0, New Rectangle(288, 240, 24, 24), "An item for use on a Pokémon. It slightly increases the base HP of a single Pokémon.")
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseWing(p.EVHP, p) = True Then
                p.EVHP += 1

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~HP.", {}, False, False)
                PlayerStatistics.Track("[254]Wings used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s HP.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace