Namespace Items.Standard

    <Item(73, "Quick Claw")>
    Public Class QuickClaw

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 64
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(96, 72, 24, 24)
        End Sub

    End Class

End Namespace
