Namespace Items.Berries

    Public Class KelpsyBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2021, "Kelpsy", 10800, "A Berry to be consumed by Pokémon. Using it on a Pokémon makes it more friendly but lowers its base Attack.", "38.1cm", "Hard", 2, 6)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 10
            Me.Sour = 10

            Me.Type = Element.Types.Fighting
            Me.Power = 70
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.EVAttack > 0 Then
                Dim reduce As Integer = 10
                If p.EVAttack < reduce Then
                    reduce = p.EVAttack
                End If
                
                p.ChangeFriendShip(Pokemon.FriendShipCauses.EVBerry)
                p.EVAttack -= reduce
                p.CalculateStats()

                Screen.TextBox.Show("Raised friendship of~" & p.GetDisplayName() & "." & RemoveItem(), {}, True, False)
                Return True
            Else
                Screen.TextBox.Show("Cannot raise the friendship~of " & p.GetDisplayName() & ".", {}, True, False)
                Return False
            End If
        End Function

    End Class

End Namespace