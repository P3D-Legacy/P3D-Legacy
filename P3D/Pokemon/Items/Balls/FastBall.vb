Namespace Items.Balls

    <Item(161, "Fast Ball")>
    Public Class FastBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A Pokéball that makes it easier to catch Pokémon which are quick to run away."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(144, 144, 24, 24)
        End Sub

    End Class

End Namespace
