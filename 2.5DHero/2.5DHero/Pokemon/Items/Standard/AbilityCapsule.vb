Namespace Items.Standard

    Public Class AbilityCapsule

        Inherits Item

        Public Sub New()
            MyBase.New("Ability Capsule", 1000, ItemTypes.Standard, 187, 1, 1, New Rectangle(0, 240, 24, 24), "A capsule that allows a Pokémon with two Abilities to switch between these Abilities when it is used.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = True
            Me._canBeUsedInBattle = False
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim currentAbility As Integer = -1

            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.NewAbilities.Count = 1 Then
                Screen.TextBox.Show("You cannot use this on~" & p.GetDisplayName() & ".")
                Return False
            End If

            For i = 0 To p.NewAbilities.Count - 1
                If p.Ability.ID = p.NewAbilities(i).ID Then
                    currentAbility = i
                    Exit For
                End If
            Next

            If currentAbility = -1 Then
                Screen.TextBox.Show("You cannot use this on~" & p.GetDisplayName() & ".")
                Return False
            Else
                Dim oldAbilityName As String = p.Ability.Name

                Dim newAbility As Integer = currentAbility
                While newAbility = currentAbility
                    newAbility = Core.Random.Next(0, p.NewAbilities.Count)
                End While

                p.Ability = Ability.GetAbilityByID(p.NewAbilities(newAbility).ID)

                Screen.TextBox.Show(p.GetDisplayName() & " forgot how~to use " & oldAbilityName & ".*It learned~" & p.NewAbilities(newAbility).Name & " instead." & RemoveItem())
                Return True
            End If
        End Function

    End Class

End Namespace