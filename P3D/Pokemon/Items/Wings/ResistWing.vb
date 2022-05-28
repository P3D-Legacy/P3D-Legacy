Namespace Items.Wings

    <Item(256, "Resist Wing")>
    Public Class ResistWing

        Inherits WingItem

        Public Overrides ReadOnly Property Description As String = "An item for use on a Pokémon. It slightly increases the base Defense stat of a single Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(336, 240, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseWing(p.EVDefense, p) = True Then
                p.EVDefense += 1

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Defense.", {}, False, False)
                PlayerStatistics.Track("[254]Wings used", 1)

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
