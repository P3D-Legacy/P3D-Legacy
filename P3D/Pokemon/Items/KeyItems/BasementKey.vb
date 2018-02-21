Namespace Items.KeyItems

    <Item(133, "Basement Key")>
    Public Class BasementKey

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A key that opens a door in the Goldenrod Tunnel."

        Public Sub New()
            _textureRectangle = New Rectangle(288, 120, 24, 24)
        End Sub

    End Class

End Namespace
