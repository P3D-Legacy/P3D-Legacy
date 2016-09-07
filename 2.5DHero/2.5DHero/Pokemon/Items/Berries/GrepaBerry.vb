Namespace Items.Berries

    Public Class GrepaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2024, "Grepa", 10800, "A Berry to be consumed by Pokémon. Using it on a Pokémon makes it more friendly but lowers its base Sp. Def.", "14.9cm", "Soft", 2, 6)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Flying
            Me.Power = 70
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.EVSpDefense > 0 Then
                Dim reduce As Integer = 10
                If p.EVSpDefense < reduce Then
                    reduce = p.EVSpDefense
                End If

                p.ChangeFriendShip(Pokemon.FriendShipCauses.EVBerry)
                p.EVSpDefense -= reduce
                p.CalculateStats()

                Screen.TextBox.Show("Raised friendship of~" & p.GetDisplayName() & "." & Me.RemoveItem(), {}, True, False)
                Return True
            Else
                Screen.TextBox.Show("Cannot raise the friendship~of " & p.GetDisplayName() & ".", {}, True, False)
                Return False
            End If
        End Function

    End Class

End Namespace