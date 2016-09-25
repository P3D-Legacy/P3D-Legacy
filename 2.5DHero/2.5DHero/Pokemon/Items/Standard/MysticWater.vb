Namespace Items.Standard

    <Item(95, "Mystic Water")>
    Public Class MysticWater

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. This teardrop-shaped gem boosts the power of Water-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 72, 24, 24)
        End Sub

    End Class

End Namespace
