Namespace Items.Standard

    <Item(138, "Charcoal")>
    Public Class Charcoal

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a combustible fuel that boosts the power of Fire-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4900
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(336, 120, 24, 24)
        End Sub

    End Class

End Namespace
