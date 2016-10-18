Namespace Items.Standard

    <Item(293, "Damp Rock")>
    Public Class DampRock

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It extends the duration of the move Rain Dance when used by the holder."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(360, 72, 24, 24)
        End Sub

    End Class

End Namespace

