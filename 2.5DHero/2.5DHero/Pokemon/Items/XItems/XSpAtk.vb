Namespace Items.XItems

    <Item(53, "X Sp. Atk.")>
    Public Class XSpecial

        Inherits Item

        Public Sub New()
            MyBase.New("X Sp. Atk.", 350, ItemTypes.BattleItems, 53, 1, 0, New Rectangle(144, 48, 24, 24), "An item that boosts the Sp. Atk stat of a Pokémon during a battle. It wears off once the Pokémon is withdrawn.")

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

                If p.StatSpAttack < 6 Then
                    p.StatSpAttack += 1

                    Screen.TextBox.Show("Boosted " & p.GetDisplayName() & "'s~Special Attack!" & RemoveItem(), {}, False, False)
                    PlayerStatistics.Track("[53]Status booster used", 1)

                    Return True
                End If

                Screen.TextBox.Show("Cannot boost~ " & p.GetDisplayName() & "'s Special Attack!", {}, False, False)
                Return False
            Else
                Logger.Log(Logger.LogTypes.Warning, "XSpecial.vb: Used outside of battle environment!")
                Return False
            End If
        End Function

    End Class

End Namespace
