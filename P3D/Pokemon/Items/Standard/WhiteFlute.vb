Namespace Items.Standard

    <Item(147, "White Flute")>
    Public Class WhiteFlute

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A lovely toy flute to admire. It's made from white glass."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 192, 24, 24)
        End Sub

    End Class

End Namespace
