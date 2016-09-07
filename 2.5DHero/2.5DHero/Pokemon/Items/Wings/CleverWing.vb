Namespace Items.Wings

    Public Class CleverWing

        Inherits WingItem

        Public Sub New()
            MyBase.New("Clever Wing", 3000, ItemTypes.Medicine, 258, 1.0F, 0, New Rectangle(384, 240, 24, 24), "An item for use on a Pokémon. It slightly increases the base Sp. Def. stat of a single Pokémon.")
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseWing(p.EVSpDefense, p) = True Then
                p.EVSpDefense += 1

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Special Defense.", {}, False, False)
                PlayerStatistics.Track("[254]Wings used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s Special Defense.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace