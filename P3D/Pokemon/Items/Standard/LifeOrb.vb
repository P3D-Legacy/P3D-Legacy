Namespace Items.Standard

    <Item(506, "Life Orb")>
    Public Class LifeOrb

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It boosts the power of moves, but at the cost of some HP on each hit."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(240, 240, 24, 24)
        End Sub

    End Class

End Namespace
