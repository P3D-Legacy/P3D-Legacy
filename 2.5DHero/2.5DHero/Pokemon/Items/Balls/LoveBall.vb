Namespace Items.Balls

    <Item(166, "Love Ball")>
    Public Class LoveBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "Pokéball for catching Pokémon that are the opposite gender of your Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(240, 144, 24, 24)
        End Sub

    End Class

End Namespace
