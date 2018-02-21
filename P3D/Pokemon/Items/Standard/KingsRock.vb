Namespace Items.Standard

    <Item(82, "King's Rock")>
    Public Class KingsRock

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. When the holder successfully inflicts damage, the target may also flinch."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 64
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(216, 72, 24, 24)
        End Sub

    End Class

End Namespace
