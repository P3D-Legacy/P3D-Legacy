Namespace Items.Standard

    <Item(103, "Slowpoketail")>
    Public Class SlowPokeTail

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A very tasty tail of something. It sells for a high price."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(120, 96, 24, 24)
        End Sub

    End Class

End Namespace
