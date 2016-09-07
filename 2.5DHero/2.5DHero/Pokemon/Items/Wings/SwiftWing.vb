Namespace Items.Wings

    Public Class SwiftWing

        Inherits WingItem

        Public Sub New()
            MyBase.New("Swift Wing", 3000, ItemTypes.Medicine, 259, 1.0F, 0, New Rectangle(408, 240, 24, 24), "An item for use on a Pokémon. It slightly increases the base Speed stat of a single Pokémon.")
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseWing(p.EVSpeed, p) = True Then
                p.EVSpeed += 1

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Speed.", {}, False, False)
                PlayerStatistics.Track("[254]Wings used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s Speed.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace