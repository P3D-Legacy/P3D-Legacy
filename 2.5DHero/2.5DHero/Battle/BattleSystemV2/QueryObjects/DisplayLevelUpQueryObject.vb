Namespace BattleSystem

    Public Class DisplayLevelUpQueryObject

        Inherits QueryObject

        Dim p As Pokemon
        Dim oldStats() As Integer

        Public Sub New(ByVal p As Pokemon, ByVal oldStats() As Integer)
            MyBase.New(QueryTypes.DisplayLevelUp)

            Me.p = net.Pokemon3D.Game.Pokemon.GetPokemonByData(p.GetSaveData())
            Me.oldStats = oldStats
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            Core.SetScreen(New BattleGrowStatsScreen(Core.CurrentScreen, p, oldStats))
        End Sub

    End Class

End Namespace