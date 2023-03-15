Namespace Items.Medicine

    <Item(502, "PP Max")>
    Public Class PPMax

        Inherits MedicineItem
        Public Overrides ReadOnly Property Description As String = "A medicine that can optimally raise the maximum PP of a single move that has been learned by the target Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800
        Public Overrides ReadOnly Property PluralName As String = "PP Maxes"

        Public Sub New()
            _textureRectangle = New Rectangle(120, 240, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Core.SetScreen(New ChooseAttackScreen(Core.CurrentScreen, Core.Player.Pokemons(PokeIndex), True, True, AddressOf UseOnAttack))
            If Core.CurrentScreen.Identification <> Screen.Identifications.ChooseAttackScreen Then
                If ChooseAttackScreen.Selected <> -1 Then
                    Return True
                End If
            End If
            Return False
        End Function

        Private Sub UseOnAttack(ByVal Pokemon As Pokemon, ByVal AttackIndex As Integer)
            Dim raisedPP As Boolean = False

            For i = 0 To 2
                If Pokemon.Attacks(AttackIndex).RaisePP() = True Then
                    raisedPP = True
                End If
            Next

            If raisedPP = True Then
                SoundManager.PlaySound("Use_Item", False)
                Dim t As String = "Raised PP of~" & Pokemon.Attacks(AttackIndex).Name & "." & RemoveItem()
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Screen.TextBox.Show(t, {}, True, True)
            Else
                Screen.TextBox.Show("The move already has~full PP.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
