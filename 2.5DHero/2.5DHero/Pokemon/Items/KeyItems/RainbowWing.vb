Namespace Items.KeyItems

    <Item(178, "Rainbow Wing")>
    Public Class RainbowWing

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A mystical rainbow feather that sparkles."

        Public Sub New()
            _textureRectangle = New Rectangle(408, 144, 24, 24)
        End Sub

    End Class

End Namespace
