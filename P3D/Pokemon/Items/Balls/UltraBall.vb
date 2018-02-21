Namespace Items.Balls

    <Item(2, "Ultra Ball")>
    Public Class UltraBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "An ultra-high performance Pokéball that provides a higher success rate for catching Pokémon than a Great Ball."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1200
        Public Overrides ReadOnly Property CatchMultiplier As Single = 2.0F

        Public Sub New()
            _textureRectangle = New Rectangle(24, 0, 24, 24)
        End Sub

    End Class

End Namespace
