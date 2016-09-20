Namespace Items.Balls

    <Item(165, "Moon Ball")>
    Public Class MoonBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A Pokéball for catching Pokémon that evolve using the Moon Stone."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(216, 144, 24, 24)
        End Sub

    End Class

End Namespace
