Namespace Items.Standard

    <Item(102, "Black Glasses")>
    Public Class BlackGlasses

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokemon. A pair of shady-looking glasses that boost the power of Dark-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(96, 96, 24, 24)
        End Sub

    End Class

End Namespace
