Namespace Items.Medicine

    Public Class Antidote

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Antidote", 100, ItemTypes.Medicine, 9, 1, 1, New Rectangle(168, 0, 24, 24), "A spray-type medicine for poisoning. It can be used once to lift the effects of being poisoned from a Pokémon.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return CurePoison(PokeIndex)
        End Function

    End Class

End Namespace