Namespace Items.Standard

    <Item(163, "Light Ball")>
    Public Class LightBall

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 144, 24, 24)
        End Sub

    End Class

End Namespace
