Namespace Items.Standard

    <Item(77, "Sharp Beak")>
    Public Class SharpBeak

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It's a long, sharp beak that boosts the power of Flying-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 50
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 72, 24, 24)
        End Sub

    End Class

End Namespace
