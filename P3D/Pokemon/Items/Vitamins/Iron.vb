Namespace Items.Vitamins

    <Item(28, "Iron")>
    Public Class Iron

        Inherits VitaminItem

        Public Overrides ReadOnly Property Description As String = "A nutritious drink for Pokémon. When consumed, it raises the base Defense stat of a single Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(96, 24, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseVitamin(p.EVDefense, p) = True Then
                p.EVDefense += 10
                p.ChangeFriendShip(Pokemon.FriendShipCauses.Vitamin)

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Defense.", {}, False, False)
                PlayerStatistics.Track("[25]Vitamins used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s Defense.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace
