Namespace Items.Standard

    <Item(36, "Nugget")>
    Public Class Nugget

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A nugget of the purest gold that gives off a lustrous gleam in direct light. It can be sold at a high price to shops."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 10000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(288, 24, 24, 24)
        End Sub

    End Class

End Namespace
