Namespace Items.Vitamins

    <Item(25, "Zinc")>
    Public Class Zinc

        Inherits VitaminItem

        Public Overrides ReadOnly Property Description As String = "A nutritious drink for Pokémon. When consumed, it raises the base Sp. Def stat of a single Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(168, 192, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseVitamin(p.EVSpDefense, p) = True Then
                p.EVSpDefense += 10
                p.ChangeFriendShip(Pokemon.FriendShipCauses.Vitamin)

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Special Defense.", {}, False, False)
                PlayerStatistics.Track("[25]Vitamins used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s Special~Defense.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace
