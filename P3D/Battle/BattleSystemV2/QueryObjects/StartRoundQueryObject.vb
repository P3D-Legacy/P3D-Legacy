Namespace BattleSystem

    Public Class StartRoundQueryObject

        Inherits QueryObject
        Dim _ready As Boolean = False

        Public Sub New()
            MyBase.New(QueryTypes.StartRound)

        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            BV2Screen.Battle.StartRound(BV2Screen)
            _ready = True
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return _ready
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(input As String) As QueryObject
            Return New StartRoundQueryObject()
        End Function

        Public Overrides Function ToString() As String
            Return "{STARTROUND|0}"
        End Function

    End Class

End Namespace