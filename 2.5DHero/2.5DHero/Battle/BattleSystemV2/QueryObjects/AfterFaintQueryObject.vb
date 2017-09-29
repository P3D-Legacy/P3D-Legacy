Namespace BattleSystem

    Public Class AfterFaintQueryObject

        Inherits QueryObject

        Private _isHost As Boolean
        Private _ready As Boolean = False

        Public Sub New(ByVal IsHost As Boolean)
            MyBase.New(QueryTypes.AfterFaint)
            _isHost = IsHost
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If BV2Screen.IsHost Then
                If _isHost Then
                    BV2Screen.OwnFaint = True
                Else
                    BV2Screen.OppFaint = True
                End If
            Else
                If _isHost Then
                    Logger.Debug("[Battle]: The host's pokemon faints")
                    BV2Screen.OppFaint = True
                Else
                    Logger.Debug("[Battle]: The client's pokemon faints")
                    BV2Screen.OwnFaint = True
                End If
            End If
            Me._ready = True
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return _ready
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return False
        End Function

        Public Shared Shadows Function FromString(input As String) As QueryObject
            Return New AfterFaintQueryObject(CBool(input))
        End Function

        Public Overrides Function ToString() As String
            Return "{FAINT|" & Me._isHost.ToString() & "}"
        End Function

    End Class

End Namespace