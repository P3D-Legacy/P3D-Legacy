Namespace Items.Balls

    <Item(4, "Great Ball")>
    Public Class GreatBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A good, high-performance Pokéball that provides a higher Pokémon catch rate than a standard Pokéball can."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 600
        Public Overrides ReadOnly Property CatchMultiplier As Single = 1.5F

        Public Sub New()
            _textureRectangle = New Rectangle(72, 0, 24, 24)
        End Sub

    End Class

End Namespace
