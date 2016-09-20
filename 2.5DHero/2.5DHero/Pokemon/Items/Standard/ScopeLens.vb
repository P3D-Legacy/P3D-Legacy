Namespace Items.Standard

    <Item(140, "Scope Lens")>
    Public Class ScopeLens

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a lens that boosts the holder's critical-hit ratio."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 64
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(384, 120, 24, 24)
        End Sub

    End Class

End Namespace
