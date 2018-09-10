Namespace Items.KeyItems

    <Item(592, "Liberty Sea Map")>
    Public Class LibertySeaMap

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A new sea chart that shows the way to Liberty Garden in Unova. It depicts a Lighthouse."

        Public Sub New()
            _textureRectangle = New Rectangle(408, 288, 24, 24)
        End Sub

    End Class

End Namespace