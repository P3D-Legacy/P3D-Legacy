Namespace BattleSystem
    Public Class TriggerNewRoundPVPQueryObject

        Inherits QueryObject

        Dim _ready As Boolean = False

        Public Sub New()
            MyBase.New(QueryTypes.TriggerNewRound)
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            BattleScreen.ReceivedInput = ""
            BV2Screen.SentHostData = False
            BattleScreen.ReceivedQuery = ""
            BV2Screen.SentInput = False

            If BV2Screen.IsHost = False Then
                BV2Screen.BattleMenu.Visible = True
                BV2Screen.ClientWaitForData = True
            Else
                BV2Screen.SendEndRoundData()
            End If

            Me._ready = True
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return _ready
            End Get
        End Property

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Return New TriggerNewRoundPVPQueryObject()
        End Function

        Public Overrides Function ToString() As String
            Return "{TRIGGERNEWROUNDPVP| }"
        End Function

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

    End Class

End Namespace