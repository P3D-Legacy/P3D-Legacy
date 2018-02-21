Namespace Items.Balls

    <Item(181, "Safari Ball")>
    Public Class SafariBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A special Pokéball that is used only in the Great Marsh and the Safari Zone. It is decorated in a camouflage pattern."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CatchMultiplier As Single = 1.5F

        Public Sub New()
            _textureRectangle = New Rectangle(72, 144, 24, 24)
        End Sub

    End Class

End Namespace
