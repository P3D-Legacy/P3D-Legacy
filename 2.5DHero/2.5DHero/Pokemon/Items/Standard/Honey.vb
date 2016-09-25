Namespace Items.Plants

    <Item(253, "Honey")>
    Public Class Honey

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Plants
        Public Overrides ReadOnly Property Description As String = "Honey produced by a Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 240, 24, 24)
        End Sub

    End Class

End Namespace
