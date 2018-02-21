Namespace Items.XItems

    <Item(44, "Dire Hit")>
    Public Class DireHit

        Inherits XItem

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 650
        Public Overrides ReadOnly Property Description As String = "An item that raises the critical-hit ratio greatly. It can be used only once and wears off if the Pokémon is withdrawn."

        Public Sub New()
            _textureRectangle = New Rectangle(480, 24, 24, 24)
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim foundBattleScreen As Boolean = True
            Dim s As Screen = Core.CurrentScreen
            While s.Identification <> Screen.Identifications.BattleScreen
                If s.PreScreen Is Nothing Then
                    foundBattleScreen = False
                    Exit While
                End If
                s = s.PreScreen
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
