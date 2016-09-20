Namespace Items.KeyItems

    <Item(56, "Crystal Wing")>
    Public Class CrystalWing

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A mystical feather entirely made out of crystal."

        Public Sub New()
            _textureRectangle = New Rectangle(240, 192, 24, 24)
        End Sub

    End Class

End Namespace
