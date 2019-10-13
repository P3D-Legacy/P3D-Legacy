Namespace Items.KeyItems

    <Item(652, "Dark stone")>
    Public Class DarkStone

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "Zekrom's body was destroyed and changed into this stone. It is said to be waiting for the emergence of a hero."

        Public Sub New()
            _textureRectangle = New Rectangle(240, 408, 24, 24)
        End Sub

    End Class

End Namespace
