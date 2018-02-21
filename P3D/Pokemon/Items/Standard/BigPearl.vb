Namespace Items.Standard

    <Item(111, "Big Pearl")>
    Public Class BigPearl

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A rather large pearl that has a very nice silvery sheen. It can be sold to shops for a high price."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 7500
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(288, 96, 24, 24)
        End Sub

    End Class

End Namespace
