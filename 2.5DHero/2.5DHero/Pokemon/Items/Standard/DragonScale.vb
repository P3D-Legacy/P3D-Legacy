Namespace Items.Standard

    <Item(151, "Dragon Scale")>
    Public Class DragonScale

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A very tough and inflexible scale. Dragon-type Pokémon may be holding this item when caught."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 120, 24, 24)
        End Sub

    End Class

End Namespace
