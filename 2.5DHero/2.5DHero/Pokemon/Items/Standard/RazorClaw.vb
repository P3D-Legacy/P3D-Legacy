Namespace Items.Standard

    <Item(184, "Razor Claw")>
    Public Class RazorClaw

        Inherits Item

        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 48
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 144, 24, 24)
        End Sub

    End Class

End Namespace
