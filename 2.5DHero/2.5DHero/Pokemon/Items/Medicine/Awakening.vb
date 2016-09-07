Namespace Items.Medicine

    Public Class Awakening

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Awakening", 250, ItemTypes.Medicine, 12, 1, 1, New Rectangle(240, 0, 24, 24), "A spray-type medicine used against sleep. It can be used once to rouse a Pokémon from the clutches of sleep.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return WakeUp(PokeIndex)
        End Function

    End Class

End Namespace