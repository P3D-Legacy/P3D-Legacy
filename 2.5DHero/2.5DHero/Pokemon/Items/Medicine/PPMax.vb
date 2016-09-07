Namespace Items.Medicine

    Public Class PPMax

        Inherits Item

        Public Sub New()
            MyBase.New("PP Max", 9800, ItemTypes.Medicine, 502, 1, 1, New Rectangle(120, 240, 24, 24), "A medicine that can optimally raise the maximum PP of a single move that has been learned by the target Pokémon.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Core.SetScreen(New ChooseAttackScreen(Core.CurrentScreen, Core.Player.Pokemons(PokeIndex), True, True, AddressOf UseOnAttack))
            Return True
        End Function

        Private Sub UseOnAttack(ByVal Pokemon As Pokemon, ByVal AttackIndex As Integer)
            Dim raisedPP As Boolean = False

            For i = 0 To 2
                If Pokemon.Attacks(AttackIndex).RaisePP() = True Then
                    raisedPP = True
                End If
            Next

            If raisedPP = True Then
                SoundManager.PlaySound("single_heal", False)
                Dim t As String = "Raised PP of~" & Pokemon.Attacks(AttackIndex).Name & "." & RemoveItem()
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Screen.TextBox.Show(t, {}, True, True)
            Else
                Screen.TextBox.Show("The move already has~full PP.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace