Namespace BattleSystem

    Public Class EndBattleQueryObject

        Inherits QueryObject

        Dim blackout As Boolean = False

        Public Sub New(ByVal blackout As Boolean)
            MyBase.New(QueryTypes.EndBattle)

            Me.blackout = blackout
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            BV2Screen.EndBattle(Me.blackout)
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Return New EndBattleQueryObject(CBool(input))
        End Function

        Public Overrides Function ToString() As String
            Return "{ENDBATTLE|" & Me.blackout.ToNumberString() & "}"
        End Function

    End Class

End Namespace