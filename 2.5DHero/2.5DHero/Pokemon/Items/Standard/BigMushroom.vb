Namespace Items.Plants

    <Item(87, "Big Mushroom")>
    Public Class BigMushroom

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Plants
        Public Overrides ReadOnly Property Description As String = "A very large and rare mushroom. It's popular with a certain class of collectors and sought out by them."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 5000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(288, 72, 24, 24)
        End Sub

    End Class

End Namespace
