Namespace Items.Berries

    Public Class PechaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2002, "Pecha", 10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can recover from poisoning on its own in battle.", "4.0cm", "Very Soft", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Electric
            Me.Power = 60
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return CurePoison(PokeIndex)
        End Function

    End Class

End Namespace