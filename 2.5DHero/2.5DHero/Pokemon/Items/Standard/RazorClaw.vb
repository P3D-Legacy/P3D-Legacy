Namespace Items.Standard

    <Item(184, "Razor Claw")>
    Public Class RazorClaw

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a sharply hooked claw that ups the holder's critical-hit ratio."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 48
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 144, 24, 24)
        End Sub

    End Class

End Namespace
