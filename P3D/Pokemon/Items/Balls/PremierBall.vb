Namespace Items.Balls

    <Item(3, "Premier Ball")>
    Public Class PremierBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat rare Pokéball that was made as a commemorative item used to celebrate an event of some sort."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200

        Public Sub New()
            _textureRectangle = New Rectangle(216, 216, 24, 24)
        End Sub

    End Class

End Namespace
