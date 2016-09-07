Namespace Items.Medicine

    Public Class Revive

        Inherits Item

        Public Sub New()
            MyBase.New("Revive", 1500, ItemTypes.Medicine, 39, 1, 0, New Rectangle(360, 24, 24, 24), "A medicine that can revive fainted Pokémon. It also restores half of a fainted Pokémon's HP.")

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
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None
                Pokemon.HP = CInt(Math.Floor(Pokemon.MaxHP / 2))

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is revitalized.", {}, False, False)
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                RemoveItem()

                Return True
            Else
                Screen.TextBox.Show("Cannot use revive~on this Pokémon.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace