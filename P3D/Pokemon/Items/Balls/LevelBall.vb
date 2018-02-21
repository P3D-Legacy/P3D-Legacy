Namespace Items.Balls

    <Item(159, "Level Ball")>
    Public Class LevelBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A Pokéball for catching Pokémon that are a lower level than your own. "
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(96, 144, 24, 24)
        End Sub

    End Class

End Namespace
