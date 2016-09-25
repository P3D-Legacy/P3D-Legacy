Namespace Items.Plants

    <Item(86, "Tiny Mushroom")>
    Public Class TinyMushroom

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Plants
        Public Overrides ReadOnly Property Description As String = "A very small and rare mushroom. It's popular with a certain class of collectors and sought out by them."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 72, 24, 24)
        End Sub

    End Class

End Namespace
