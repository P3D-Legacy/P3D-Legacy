Namespace Items.Balls

    <Item(80, "Net Ball")>
    Public Class NetBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that is more effective when attempting to catch Water- or Bug-type Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(48, 168, 24, 24)
        End Sub

    End Class

End Namespace
