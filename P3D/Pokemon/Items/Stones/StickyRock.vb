Namespace Items.Stones

    <Item(262, "Sticky Rock")>
    Public Class StickyRock

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 300
        Public Overrides ReadOnly Property Description As String = "It's a stone that sticks to other stones, a Pokémon that holds it will find more stones."
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 240, 24, 24)
        End Sub

    End Class

End Namespace
