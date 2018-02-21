Namespace Items.Standard

    <Item(153, "Balm Mushroom")>
    Public Class BalmMushroom

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A rare mushroom which gives off a nice fragrance. A maniac will buy it for a high price."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 50000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(48, 216, 24, 24)
        End Sub

    End Class

End Namespace
