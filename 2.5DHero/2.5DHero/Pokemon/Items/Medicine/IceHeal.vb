Namespace Items.Medicine

    Public Class IceHeal

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Ice Heal", 250, ItemTypes.Medicine, 11, 1, 1, New Rectangle(216, 0, 24, 24), "A spray-type medicine for freezing. It can be used once to defrost a Pokémon that has been frozen solid.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealIce(PokeIndex)
        End Function

    End Class

End Namespace