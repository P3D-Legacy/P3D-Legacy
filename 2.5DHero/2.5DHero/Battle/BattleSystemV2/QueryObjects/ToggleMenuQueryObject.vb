Namespace BattleSystem

    Public Class ToggleMenuQueryObject

        Inherits QueryObject

        Private _ready As Boolean = False
        Private _switchTo As Boolean = True

        Public Sub New(ByVal MenuVisible As Boolean)
            MyBase.New(QueryTypes.ToggleMenu)

            Me._switchTo = Not MenuVisible
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            BV2Screen.BattleMenu.Visible = Me._switchTo

            Me._ready = True
        End Sub

        Public Overrides ReadOnly Property IsReady() As Boolean
            Get
                Return Me._ready
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Return New ToggleMenuQueryObject(CBool(input))
        End Function

        Public Overrides Function ToString() As String
            Return "{TOGGLEMENU|" & Me._switchTo.ToNumberString() & "}"
        End Function

    End Class

End Namespace