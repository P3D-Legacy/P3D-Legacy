Namespace Items.Balls

    <Item(160, "Lure Ball")>
    Public Class LureBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A Pokéball for catching Pokémon hooked by a Rod when fishing."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(120, 144, 24, 24)
        End Sub

    End Class

End Namespace
