Namespace Items.Balls

    <Item(158, "Dusk Ball")>
    Public Class DuskBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that makes it easier to catch wild Pokémon at night or in dark places like caves."

        Public Sub New()
            _textureRectangle = New Rectangle(360, 216, 24, 24)
        End Sub

    End Class

End Namespace
