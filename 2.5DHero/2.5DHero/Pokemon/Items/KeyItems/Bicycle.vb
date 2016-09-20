Namespace Items.KeyItems

    <Item(6, "Bicycle")>
    Public Class Bicycle

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A folding Bicycle that enables much faster movement than the Running Shoes."

        Public Sub New()
            _textureRectangle = New Rectangle(120, 0, 24, 24)
        End Sub

    End Class

End Namespace
