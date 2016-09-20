Namespace Items.XItems

    <Item(68, "Guard Spec.")>
    Public Class GuardSpec

        Inherits XItem

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 700
        Public Overrides ReadOnly Property Description As String = "An item that prevents stat reduction among the Trainer's party Pokémon for five turns after it is used in battle."

        Public Sub New()
            _textureRectangle = New Rectangle(408, 24, 24, 24)
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
