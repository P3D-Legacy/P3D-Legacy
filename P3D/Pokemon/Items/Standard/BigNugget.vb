Namespace Items.Standard

    <Item(189, "Big Nugget")>
    Public Class BigNugget

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A big nugget of pure gold that gives off a lustrous gleam. It can be sold at a high price to shops."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 20000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(48, 240, 24, 24)
        End Sub

    End Class

End Namespace
