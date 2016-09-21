Namespace Items.Apricorns

    <Item(99, "Black Apricorn")>
    Public Class BlkApricorn

        Inherits Apricorn

        Public Overrides ReadOnly Property Description As String = "A black Apricorn It has an indescribable scent."

        Public Sub New()
            _textureRectangle = New Rectangle(48, 96, 24, 24)
        End Sub

    End Class

End Namespace
