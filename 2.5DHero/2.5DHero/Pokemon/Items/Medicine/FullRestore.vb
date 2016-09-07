Namespace Items.Medicine

    Public Class FullRestore

        Inherits Items.MedicineItem

        Public Sub New()
            MyBase.New("Full Restore", 3000, ItemTypes.Medicine, 14, 1, 0, New Rectangle(288, 0, 24, 24), "A medicine that can be used to fully restore the HP of a single Pokémon and heal any status conditions it has.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._isHealingItem = True
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            Dim v1 As Boolean = HealPokemon(PokeIndex, p.MaxHP)
            Dim v2 As Boolean = False

            If p.Status <> Pokemon.StatusProblems.Fainted And p.Status <> Pokemon.StatusProblems.None Or p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                    p.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                End If

                p.Status = Pokemon.StatusProblems.None
                v2 = True
                If v1 = False Then
                    Dim t As String = "Healed " & p.GetDisplayName() & "!"
                    t &= RemoveItem()

                    SoundManager.PlaySound("single_heal", False)
                    Screen.TextBox.Show(t, {})
                End If
            End If

            If v2 = True Or v1 = True Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
                Return True
            End If

            Return False
        End Function

    End Class

End Namespace