Namespace Items.Standard

    <Item(119, "Focus Band")>
    Public Class FocusBand

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. The holder may endure a potential KO attack, leaving it with just 1 HP."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 64
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 96, 24, 24)
        End Sub

    End Class

End Namespace
