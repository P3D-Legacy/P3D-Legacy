Namespace Items.Vitamins

    <Item(29, "Carbos")>
    Public Class Carbos

        Inherits VitaminItem

        Public Overrides ReadOnly Property Description As String = "A nutritious drink for Pokémon. When consumed, it raises the base Speed stat of a single Pokémon."
        Public Overrides ReadOnly Property PluralName As String = "Carboses"

        Public Sub New()
            _textureRectangle = New Rectangle(120, 24, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseVitamin(p.EVSpeed, p) = True Then
                p.EVSpeed += 10
                p.ChangeFriendShip(Pokemon.FriendShipCauses.Vitamin)

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Speed.", {}, False, False)
                PlayerStatistics.Track("[25]Vitamins used", 1)

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
