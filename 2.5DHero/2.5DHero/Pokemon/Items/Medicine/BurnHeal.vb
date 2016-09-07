Namespace Items.Medicine

    Public Class BurnHeal

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Burn Heal", 250, ItemTypes.Medicine, 10, 1, 1, New Rectangle(192, 0, 24, 24), "A spray-type medicine for treating burns. It can be used once to heal a Pokemon suffering from a burn.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealBurn(PokeIndex)
        End Function

    End Class

End Namespace