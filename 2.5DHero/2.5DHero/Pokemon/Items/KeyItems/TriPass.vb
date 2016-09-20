Namespace Items.KeyItems

    <Item(265, "Tri-Pass")>
    Public Class TriPass

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A pass for ferries between One, Two, and Three Island. It has a drawing of three islands."

        Public Sub New()
            _textureRectangle = New Rectangle(480, 48, 24, 24)
        End Sub

    End Class

End Namespace
