Namespace Items.Vitamins

    <Item(27, "Protein")>
    Public Class Protein

        Inherits VitaminItem

        Public Overrides ReadOnly Property Description As String = "A nutritious drink for Pokémon. When consumed, it raises the base Attack stat of a single Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(72, 24, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseVitamin(p.EVAttack, p) = True Then
                p.EVAttack += 10
                p.ChangeFriendShip(Pokemon.FriendShipCauses.Vitamin)

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Attack.", {}, False, False)
                PlayerStatistics.Track("[25]Vitamins used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s Attack.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace
