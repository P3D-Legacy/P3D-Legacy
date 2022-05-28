Namespace Items.Medicine

    <Item(62, "PP Up")>
    Public Class PPUp

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "A medicine that can slightly raise the maximum PP of a single move that has been learned by the target Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800

        Public Sub New()
            _textureRectangle = New Rectangle(336, 48, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Core.SetScreen(New ChooseAttackScreen(Core.CurrentScreen, Core.Player.Pokemons(PokeIndex), True, True, AddressOf UseOnAttack))
            Return True
        End Function

        Private Sub UseOnAttack(ByVal Pokemon As Pokemon, ByVal AttackIndex As Integer)
            If Pokemon.Attacks(AttackIndex).RaisePP() = True Then
                SoundManager.PlaySound("Use_Item", False)
                Dim t As String = "Raised PP of~" & Pokemon.Attacks(AttackIndex).Name & "."
                t &= RemoveItem()
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Screen.TextBox.Show(t, {}, True, True)
            Else
                Screen.TextBox.Show("The move already has~full PP.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
