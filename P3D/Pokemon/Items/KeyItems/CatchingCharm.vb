Namespace Items.KeyItems

    <Item(657, "Catching Charm")>
    Public Class CatchingCharm

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "Having one of these mysterious, unshakable charms makes it more likely you’ll get a critical catch."

        Public Sub New()
            _textureRectangle = New Rectangle(480, 408, 24, 24)
        End Sub

    End Class

End Namespace
