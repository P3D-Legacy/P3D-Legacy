Namespace Items.Standard

    <Item(577, "Flame Orb")>
    Public Class FlameOrb

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a bizarre orb that inflicts a burn on the holder in battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 264, 24, 24)
        End Sub

    End Class

End Namespace
