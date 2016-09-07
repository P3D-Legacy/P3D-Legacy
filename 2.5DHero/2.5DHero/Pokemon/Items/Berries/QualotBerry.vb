Namespace Items.Berries

    Public Class QualotBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2022, "Qualot", 10800, "A Berry to be consumed by Pokémon. Using it on a Pokémon makes it more friendly but lowers its base Defense.", "11.0cm", "Hard", 2, 6)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Poison
            Me.Power = 60
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.EVDefense > 0 Then
                Dim reduce As Integer = 10
                If p.EVDefense < reduce Then
                    reduce = p.EVDefense
                End If
                
                p.ChangeFriendShip(Pokemon.FriendShipCauses.EVBerry)
                p.EVDefense -= reduce
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