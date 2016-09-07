Namespace Items.Medicine

    Public Class ParalyzeHeal

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Paralyze Heal", 200, ItemTypes.Medicine, 13, 1, 1, New Rectangle(264, 0, 24, 24), "A spray-type medicine for paralysis. It can be used once to free a Pokémon that has been paralyzed.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealParalyze(PokeIndex)
        End Function

    End Class

End Namespace