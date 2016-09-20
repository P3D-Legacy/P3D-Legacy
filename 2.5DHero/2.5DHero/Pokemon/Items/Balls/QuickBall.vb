Namespace Items.Balls

    <Item(129, "Quick Ball")>
    Public Class QuickBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that has a more successful catch rate if used at the start of a wild encounter."

        Public Sub New()
            _textureRectangle = New Rectangle(120, 168, 24, 24)
        End Sub

    End Class

End Namespace
