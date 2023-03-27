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
            Dim s As Screen = Core.CurrentScreen
            While s.Identification <> Screen.Identifications.BattleScreen AndAlso s.PreScreen IsNot Nothing
                s = s.PreScreen
            End While
            If s.Identification = Screen.Identifications.BattleScreen Then
                Return False
            Else
                Return True
            End If
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
                Dim s As Screen = Core.CurrentScreen
                While s.Identification <> Screen.Identifications.BattleScreen AndAlso s.PreScreen IsNot Nothing
                    s = s.PreScreen
                End While
                If s.Identification = Screen.Identifications.BattleScreen Then
                    Dim TempBattleScreen As BattleSystem.BattleScreen = CType(s, BattleSystem.BattleScreen)

                    TempBattleScreen.BattleQuery.Clear()
                    TempBattleScreen.BattleQuery.Add(TempBattleScreen.FocusBattle())
                    TempBattleScreen.BattleQuery.Insert(0, New BattleSystem.ToggleMenuQueryObject(True))
                    TempBattleScreen.Battle.InitializeRound(TempBattleScreen, New BattleSystem.Battle.RoundConst With {.StepType = BattleSystem.Battle.RoundConst.StepTypes.Item, .Argument = Me.ID.ToString()})
                    Core.SetScreen(TempBattleScreen)
                End If
            Else
                Screen.TextBox.Show("The move already has~full PP.", {}, True, True)
            End If
        End Sub

    End Class

End Namespace
