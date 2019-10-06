Namespace Items.Medicine

    <Item(114, "Rage Candy Bar")>
    Public Class RageCandyBar

        Inherits MedicineItem

        Public Overrides ReadOnly Property IsHealingItem As Boolean = True
        Public Overrides ReadOnly Property Description As String = "A famous Mahogany Town candy tourists like to buy and take home. It can be used once to heal all the status conditions of a Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 300

        Public Sub New()
            _textureRectangle = New Rectangle(360, 96, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItem", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})

                Return False
            Else
                If Pokemon.Status <> P3D.Pokemon.StatusProblems.None Or Pokemon.HasVolatileStatus(P3D.Pokemon.VolatileStatus.Confusion) = True Then
                    Pokemon.Status = P3D.Pokemon.StatusProblems.None

                    If Pokemon.HasVolatileStatus(P3D.Pokemon.VolatileStatus.Confusion) = True Then
                        Pokemon.RemoveVolatileStatus(P3D.Pokemon.VolatileStatus.Confusion)
                    End If

                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = Pokemon.GetDisplayName() & "~gets healed up!"
                    t &= RemoveItem()

                    SoundManager.PlaySound("single_heal", False)
                    Screen.TextBox.Show(t, {})
                    PlayerStatistics.Track("[17]Medicine Items used", 1)

                    Return True
                Else
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fully healed!", {}, True, True)

                    Return False
                End If
            End If
        End Function

    End Class

End Namespace
