Namespace Items.Apricorns

    <Item(101, "Pink Apricorn")>
    Public Class PnkApricorn

        Inherits Apricorn

        Public Overrides ReadOnly Property Description As String = "A pink Apricorn. It has a nice, sweet scent."

        Public Sub New()
            _textureRectangle = New Rectangle(72, 96, 24, 24)
        End Sub

    End Class

End Namespace
