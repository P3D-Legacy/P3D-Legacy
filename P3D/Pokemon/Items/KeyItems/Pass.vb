Namespace Items.KeyItems

    <Item(134, "Pass")>
    Public Class Pass

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A ticket required for riding the Magnet Train. It allows you to ride whenever and however much you'd like."

        Public Sub New()
            _textureRectangle = New Rectangle(312, 120, 24, 24)
        End Sub

    End Class

End Namespace
