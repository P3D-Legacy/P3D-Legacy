Namespace Items.Balls

    <Item(79, "Dive Ball")>
    Public Class DiveBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A somewhat different Pokéball that works especially well when catching Pokémon that live underwater."

        Public Sub New()
            _textureRectangle = New Rectangle(288, 144, 24, 24)
        End Sub

    End Class

End Namespace
