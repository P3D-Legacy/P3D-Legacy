Namespace Items.Standard

    <Item(264, "Sea Incense")>
    Public Class SeaIncense

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. This incense has a curious aroma that boosts the power of Water-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9600
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(24, 264, 24, 24)
        End Sub

    End Class

End Namespace
