Namespace Items.Standard

    <Item(503, "Sachet")>
    Public Class Sachet

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(144, 240, 24, 24)
        End Sub

    End Class

End Namespace
