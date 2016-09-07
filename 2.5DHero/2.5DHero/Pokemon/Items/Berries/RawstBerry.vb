Namespace Items.Berries

    Public Class RawstBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2003, "Rawst", 10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can recover from a burn on its own in battle.", "3.2cm", "Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Grass
            Me.Power = 60
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealBurn(PokeIndex)
        End Function

    End Class

End Namespace