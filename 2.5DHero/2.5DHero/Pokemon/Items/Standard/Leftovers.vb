Namespace Items.Standard

    <Item(146, "Leftovers")>
    Public Class Leftovers

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 64
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 120, 24, 24)
        End Sub

    End Class

End Namespace
