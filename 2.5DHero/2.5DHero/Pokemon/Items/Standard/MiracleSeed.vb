Namespace Items.Standard

    <Item(117, "Miracle Seed")>
    Public Class MiracleSeed

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a seed imbued with life force that boosts the power of Grass-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(432, 96, 24, 24)
        End Sub

    End Class

End Namespace
