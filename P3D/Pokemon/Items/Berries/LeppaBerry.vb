Namespace Items.Berries

    <Item(2005, "Leppa")>
    Public Class LeppaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(14400, "A Berry to be consumed by a Pokémon. If a Pokémon holds one, it can restore 10 PP to a depleted move during battle.", "2.8cm", "Very Hard", 2, 3)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 10
            Me.Sour = 10

            Me.Type = Element.Types.Fighting
            Me.Power = 80
            Me.JuiceColor = "red"
            Me.JuiceGroup = 1
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If
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
            If Pokemon.Attacks(AttackIndex).CurrentPP < Pokemon.Attacks(AttackIndex).MaxPP Then
                Pokemon.Attacks(AttackIndex).CurrentPP = CInt(MathHelper.Clamp(Pokemon.Attacks(AttackIndex).CurrentPP + 10, 0, Pokemon.Attacks(AttackIndex).MaxPP))

                Dim t As String = "Restored PP of~" & Pokemon.Attacks(AttackIndex).Name & "."
                t &= RemoveItem()

                SoundManager.PlaySound("Use_Item", False)
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
