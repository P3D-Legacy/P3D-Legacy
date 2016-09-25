Namespace Items.Standard

    <Item(171, "Wide Lens")>
    Public Class WideLens

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It's a magnifying lens that slightly boosts the accuracy of moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(144, 216, 24, 24)
        End Sub

    End Class

End Namespace
