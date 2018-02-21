Namespace Items.Balls

    <Item(150, "Timer Ball")>
    Public Class TimerBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that becomes progressively more effective the more turns that are taken in battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(336, 216, 24, 24)
        End Sub

    End Class

End Namespace
