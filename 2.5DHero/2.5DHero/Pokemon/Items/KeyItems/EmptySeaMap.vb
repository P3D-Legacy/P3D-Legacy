Namespace Items.KeyItems

    <Item(292, "Empty Sea Map")>
    Public Class EmptySeaMap

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A sea chart on an odd blue parchment showing a point in the open ocean."

        Public Sub New()
            _textureRectangle = New Rectangle(192, 264, 24, 24)
        End Sub

    End Class

End Namespace