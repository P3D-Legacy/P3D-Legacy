Namespace Items.Standard

    <Item(113, "Spell Tag")>
    Public Class SpellTag

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a sinister, eerie tag that boosts the power of Ghost-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(336, 96, 24, 24)
        End Sub

    End Class

End Namespace
