Namespace Items.Balls

    <Item(164, "Friend Ball")>
    Public Class FriendBall

        Inherits BallItem

        Public Overrides ReadOnly Property Description As String = "A Pokéball that makes caught Pokémon more friendly."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 150

        Public Sub New()
            _textureRectangle = New Rectangle(192, 144, 24, 24)
        End Sub

    End Class

End Namespace
