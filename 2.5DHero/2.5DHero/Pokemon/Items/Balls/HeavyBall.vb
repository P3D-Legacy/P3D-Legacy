Namespace Items.Balls

    <Item(157, "Heavy Ball")>
    Public Class HeavyBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A Pokéball for catching very heavy Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(48, 144, 24, 24)
        End Sub

    End Class

End Namespace
