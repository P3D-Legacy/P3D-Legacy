Namespace Items.Balls

    <Item(168, "Repeat Ball")>
    Public Class RepeatBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that works especially well on Pokémon species that have been caught before."

        Public Sub New()
            _textureRectangle = New Rectangle(384, 216, 24, 24)
        End Sub

    End Class

End Namespace
