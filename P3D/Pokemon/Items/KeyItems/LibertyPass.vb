Namespace Items.KeyItems

    <Item(592, "Liberty Pass")>
    Public Class LibertyPass

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A special pass to go to Liberty Garden in Unova. It depicts a Lighthouse."

        Public Sub New()
            _textureRectangle = New Rectangle(408, 288, 24, 24)
        End Sub

    End Class

End Namespace