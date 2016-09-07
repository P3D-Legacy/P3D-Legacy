Namespace Items.Vitamins

    Public Class Calcium

        Inherits Items.VitaminItem

        Public Sub New()
            MyBase.New("Calcium", 9800, ItemTypes.Medicine, 31, 1, 1, New Rectangle(168, 24, 24, 24), "A nutritious drink for Pokémon. When consumed, it raises the base Sp. Atk stat of a single Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = True
            Me._canBeUsedInBattle = False
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If CanUseVitamin(p.EVSpAttack, p) = True Then
                p.EVSpAttack += 10
                p.ChangeFriendShip(Pokemon.FriendShipCauses.Vitamin)

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Raised " & p.GetDisplayName() & "'s~Special Attack.", {}, False, False)
                PlayerStatistics.Track("[25]Vitamins used", 1)

                p.CalculateStats()
                RemoveItem()
                Return True
            Else
                Screen.TextBox.Show("Cannot raise~" & p.GetDisplayName() & "'s Special~Attack.", {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace