Namespace Items.Apricorns

    <Item(89, "Blue Apricorn")>
    Public Class BluApricorn

        Inherits Apricorn

        Public Overrides ReadOnly Property Description As String = "A blue Apricorn. It smells a bit like grass."

        Public Sub New()
            _textureRectangle = New Rectangle(336, 72, 24, 24)
        End Sub

    End Class

End Namespace
