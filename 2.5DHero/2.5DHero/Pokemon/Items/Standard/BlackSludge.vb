Namespace Items.Standard

    <Item(182, "Black Sludge")>
    Public Class BlackSludge

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A held item that gradually restores the HP of Poison-type Pokémon. It inflicts damage on all other types."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(432, 144, 24, 24)
        End Sub

    End Class

End Namespace
