Namespace Items.KeyItems

    <Item(127, "Card Key")>
    Public Class CardKey

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A card key that opens a shutter in the Radio Tower."

        Public Sub New()
            _textureRectangle = New Rectangle(144, 120, 24, 24)
        End Sub

    End Class

End Namespace
