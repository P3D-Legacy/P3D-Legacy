Namespace Items.Medicine

    <Item(11, "Ice Heal")>
    Public Class IceHeal

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "A spray-type medicine for freezing. It can be used once to defrost a Pokémon that has been frozen solid."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 250

        Public Sub New()
            _textureRectangle = New Rectangle(216, 0, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealIce(PokeIndex)
        End Function

    End Class

End Namespace
