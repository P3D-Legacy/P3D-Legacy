Namespace Items.Standard

    <Item(144, "Dragon Fang")>
    Public Class DragonFang

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. This hard and sharp fang boosts the power of Dragon-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 70
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(432, 120, 24, 24)
        End Sub

    End Class

End Namespace
