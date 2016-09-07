Namespace Items.XItems

    Public Class DireHit

        Inherits Item

        Public Sub New()
            MyBase.New("Dire Hit", 650, ItemTypes.BattleItems, 44, 1, 0, New Rectangle(480, 24, 24, 24), "An item that raises the critical-hit ratio greatly. It can be used only once and wears off if the Pokémon is withdrawn.")

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
                Dim p As Pokemon = CType(s, BattleSystem.BattleScreen).OwnPokemon

                If CType(s, BattleSystem.BattleScreen).FieldEffects.OwnFocusEnergy = 0 Then
                    CType(s, BattleSystem.BattleScreen).FieldEffects.OwnFocusEnergy = 1

                    Screen.TextBox.Show(p.GetDisplayName() & "~got pumped!" & RemoveItem(), {}, False, False)
                    PlayerStatistics.Track("[53]Status booster used", 1)

                    Return True
                End If

                Screen.TextBox.Show("Cannot boost~ " & p.GetDisplayName() & "'s hit ratio!", {}, False, False)
                Return False
            Else
                Logger.Log(Logger.LogTypes.Warning, "DireHit.vb: Used outside of battle environment!")
                Return False
            End If
        End Function

    End Class

End Namespace