Namespace Items.Standard

    <Item(74, "Zoom Lens")>
    Public Class ZoomLens

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. If the holder moves after its target moves, its accuracy will be boosted."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 144, 24, 24)
        End Sub

    End Class

End Namespace
