Namespace Items.KeyItems

    <Item(115, "GS Ball")>
    Public Class GSBall

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A mysterious Pokéball. Its purpose is unknown."

        Public Sub New()
            _textureRectangle = New Rectangle(384, 96, 24, 24)
        End Sub

    End Class

End Namespace
