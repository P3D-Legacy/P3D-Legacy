Namespace Items.Standard

    <Item(183, "Razor Fang")>
    Public Class RazorFang

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It may make foes and allies flinch when the holder inflicts damage."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 48
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 144, 24, 24)
        End Sub

    End Class

End Namespace
