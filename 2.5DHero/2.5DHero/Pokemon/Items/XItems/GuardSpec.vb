Namespace Items.XItems

    <Item(68, "Guard Spec.")>
    Public Class GuardSpec

        Inherits Item

        Public Sub New()
            MyBase.New("Guard Spec.", 700, ItemTypes.BattleItems, 68, 1, 0, New Rectangle(408, 24, 24, 24), "An item that prevents stat reduction among the Trainer's party Pokémon for five turns after it is used in battle.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._requiresPokemonSelectInBattle = False
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim foundBattleScreen As Boolean = True
            Dim s As Screen = Core.CurrentScreen
            While s.Identification <> Screen.Identifications.BattleScreen
                s = s.PreScreen
                If s.PreScreen Is Nothing Then
                    foundBattleScreen = False
                    Exit While
                End If
            End While

            If foundBattleScreen = True Then
                If CType(s, BattleSystem.BattleScreen).FieldEffects.OwnGuardSpec = 0 Then
                    CType(s, BattleSystem.BattleScreen).FieldEffects.OwnGuardSpec = 1

                    Screen.TextBox.Show("Guard Spec. prevents~stat reduction." & RemoveItem(), {}, False, False)
                    PlayerStatistics.Track("[53]Status booster used", 1)

                    Return True
                End If

                Screen.TextBox.Show("It didn't have any effect...", {}, False, False)
                Return False
            Else
                Logger.Log(Logger.LogTypes.Warning, "GuardSpec.vb: Used outside of battle environment!")
                Return False
            End If
        End Function

    End Class

End Namespace
