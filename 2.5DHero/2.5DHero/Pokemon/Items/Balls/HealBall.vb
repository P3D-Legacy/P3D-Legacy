Namespace Items.Balls

    <Item(186, "Heal Ball")>
    Public Class HealBall

        Inherits BallItem

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 300
        Public Overrides ReadOnly Property Description As String = "A remedial Pokéball that restores the HP of a Pokémon caught with it and eliminiates any status conditions. "

        Public Sub New()
            _textureRectangle = New Rectangle(456, 216, 24, 24)
        End Sub

    End Class

End Namespace
