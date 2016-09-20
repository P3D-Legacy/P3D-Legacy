Namespace Items.Wings

    <Item(255, "Muscle Wing")>
    Public Class MuscleWing

        Inherits WingItem

        Public Overrides ReadOnly Property Description As String = "An item for use on a Pokémon. It slightly increases the base Attack stat of a single Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(312, 240, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseWing(p.EVAttack, p) = True Then
                p.EVAttack += 1

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Attack.", {}, False, False)
                PlayerStatistics.Track("[254]Wings used", 1)

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
