Namespace Items.KeyItems

    <Item(128, "Machine Part")>
    Public Class MachinePart

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "An important machine part for the Power Plant that was stolen."

        Public Sub New()
            _textureRectangle = New Rectangle(168, 120, 24, 24)
        End Sub

    End Class

End Namespace
