Namespace Items.Medicine

    <Item(123, "Heal Powder")>
    Public Class HealPowder

        Inherits Item

        Public Sub New()
            MyBase.New("Heal Powder", 450, ItemTypes.Medicine, 123, 1, 1, New Rectangle(48, 120, 24, 24), "A very bitter medicine powder. When consumed, it heals all of a Pokémon's status conditions.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            Else
                If Pokemon.Status <> net.Pokemon3D.Game.Pokemon.StatusProblems.None Then
                    Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None
                    Pokemon.ChangeFriendShip(Pokemon.FriendShipCauses.HealPowder)

                    Core.Player.Inventory.RemoveItem(Me.ID, 1)

                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = Pokemon.GetDisplayName() & "~gets healed up!"
                    t &= RemoveItem()

                    SoundManager.PlaySound("single_heal", False)
                    Screen.TextBox.Show(t, {})
                    PlayerStatistics.Track("[17]Medicine Items used", 1)

                    Return True
                Else
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fully healed!", {})

                    Return False
                End If
            End If
        End Function

    End Class

End Namespace
