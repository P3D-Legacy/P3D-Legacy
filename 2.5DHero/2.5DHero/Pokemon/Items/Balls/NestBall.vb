Namespace Items.Balls

    <Item(188, "Nest Ball")>
    Public Class NestBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that becomes more effective the lower the level of the wild Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(24, 240, 24, 24)
        End Sub

    End Class

End Namespace
