Namespace Items.Standard

    <Item(170, "Berserk Gene")>
    Public Class BerserkGene

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A strand of DNA that overflows with pulsating energy. It sharply raises offensive capabilities in a pinch, but causes confusion."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(192, 240, 24, 24)
        End Sub

    End Class

End Namespace
