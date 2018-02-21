Namespace Items.Balls

    <Item(1, "Masterball")>
    Public Class Masterball

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "The best Pokéball with the ultimate level of performance. With it, you will catch any wild Pokémon without fail."
        Public Overrides ReadOnly Property CanBeTraded As Boolean = False
        Public Overrides ReadOnly Property CatchMultiplier As Single = 255.0F

        Public Sub New()
            _textureRectangle = New Rectangle(0, 0, 24, 24)
        End Sub

    End Class

End Namespace
