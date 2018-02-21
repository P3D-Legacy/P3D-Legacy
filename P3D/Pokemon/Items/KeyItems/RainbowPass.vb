Namespace Items.KeyItems

    <Item(284, "Rainbow Pass")>
    Public Class RainbowPass

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A pass for ferries between Vermilion and the Sevii Islands. It features a drawing of a rainbow."

        Public Sub New()
            _textureRectangle = New Rectangle(144, 264, 24, 24)
        End Sub

    End Class

End Namespace
