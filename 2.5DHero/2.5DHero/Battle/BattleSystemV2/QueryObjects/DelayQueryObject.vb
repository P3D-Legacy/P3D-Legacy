Namespace BattleSystem

    Public Class DelayQueryObject

        Inherits QueryObject

        Private _delay As Integer = 0

        Public Sub New(ByVal Delay As Integer)
            MyBase.New(QueryTypes.Delay)

            Me._delay = Delay
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If Me._delay > 0 Then
                Me._delay -= 1
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                If Me._delay = 0 Then
                    Return True
                End If
                Return False
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(input As String) As QueryObject
            Return New DelayQueryObject(CInt(input))
        End Function

        Public Overrides Function ToString() As String
            Return "{DELAY|" & Me._delay.ToString() & "}"
        End Function

    End Class

End Namespace