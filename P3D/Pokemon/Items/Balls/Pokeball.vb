Namespace Items.Balls

    <Item(5, "Pokéball")>
    Public Class Pokeball

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "An item for catching Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200

        Public Sub New()
            _textureRectangle = New Rectangle(96, 0, 24, 24)
        End Sub

    End Class

End Namespace
