Namespace Items.Standard

    <Item(90, "Silk Scarf")>
    Public Class SilkScarf

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It's a sumptuous scarf that boosts the power of Normal-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 192, 24, 24)
        End Sub

    End Class

End Namespace
