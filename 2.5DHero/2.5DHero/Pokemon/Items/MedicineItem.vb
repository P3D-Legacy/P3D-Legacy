Namespace Items

    ''' <summary>
    ''' Represents a Medicine Item.
    ''' </summary>
    Public MustInherit Class MedicineItem

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Medicine

        ''' <summary>
        ''' Tries to heal a Pokémon from the player's party. If this succeeds, the method returns True.
        ''' </summary>
        ''' <param name="PokeIndex">The index of the Pokémon in the player's party.</param>
        ''' <param name="HP">The HP that should be healed.</param>
        Public Function HealPokemon(ByVal PokeIndex As Integer, ByVal HP As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If HP < 0 Then
                HP = CInt(Pokemon.MaxHP / (100 / (HP * (-1))))
            End If

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            Else
                If Pokemon.HP = Pokemon.MaxHP Then
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & " has full~HP already.", {})

                    Return False
                Else
                    Dim diff As Integer = Pokemon.MaxHP - Pokemon.HP
                    diff = CInt(MathHelper.Clamp(diff, 1, HP))

                    Pokemon.Heal(HP)

                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = "Restored " & Pokemon.GetDisplayName() & "'s~HP by " & diff & "."
                    t &= RemoveItem()

                    SoundManager.PlaySound("single_heal", False)
                    Screen.TextBox.Show(t, {})
                    PlayerStatistics.Track("[17]Medicine Items used", 1)

                    Return True
                End If
            End If
        End Function

        ''' <summary>
        ''' Tries to cure a Pokémon from Poison.
        ''' </summary>
        ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
        Public Function CurePoison(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            ElseIf Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Poison Or Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.BadPoison Then
                Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None

                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the poison of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(t, {})

                Return True
            Else
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not poisoned.", {})

                Return False
            End If
        End Function

        ''' <summary>
        ''' Tries to wake a Pokémon up from Sleep.
        ''' </summary>
        ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
        Public Function WakeUp(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            ElseIf Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Sleep Then
                Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None

                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the sleep of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Return True
            Else
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not asleep.", {})

                Return False
            End If
        End Function

        ''' <summary>
        ''' Tries to heal a Pokémon from Burn.
        ''' </summary>
        ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
        Public Function HealBurn(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            ElseIf Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Burn Then
                Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None

                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the burn of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Return True
            Else
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not burned.", {})

                Return False
            End If
        End Function

        ''' <summary>
        ''' Tries to heal a Pokémon from Ice.
        ''' </summary>
        ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
        Public Function HealIce(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            ElseIf Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Freeze Then
                Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None

                Core.Player.Inventory.RemoveItem(Me.ID, 1)

                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the ice of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Return True
            Else
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not frozen.", {})

                Return False
            End If
        End Function

        ''' <summary>
        ''' Tries to heal a Pokémon from Paralysis.
        ''' </summary>
        ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
        Public Function HealParalyze(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            ElseIf Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Paralyzed Then
                Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.None

                Core.Player.Inventory.RemoveItem(Me.ID, 1)

                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the paralyzis~of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Return True
            Else
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not~paralyzed.", {})

                Return False
            End If
        End Function

    End Class

End Namespace
