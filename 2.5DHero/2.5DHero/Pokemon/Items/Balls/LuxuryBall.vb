Namespace Items.Balls

    <Item(174, "Luxury Ball")>
    Public Class LuxuryBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A particularly comfortable Pokéball that makes a wild Pokémon quickly grow friendlier after being caught."

        Public Sub New()
            _textureRectangle = New Rectangle(432, 216, 24, 24)
        End Sub

    End Class

End Namespace
