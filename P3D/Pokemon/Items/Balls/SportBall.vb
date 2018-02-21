Namespace Items.Balls

    <Item(177, "Sport Ball")>
    Public Class SportBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A special Pokéball for the Bug-Catching Contest."
        Public Overrides ReadOnly Property CatchMultiplier As Single = 1.5F

        Public Sub New()
            _textureRectangle = New Rectangle(384, 144, 24, 24)
        End Sub

    End Class

End Namespace
