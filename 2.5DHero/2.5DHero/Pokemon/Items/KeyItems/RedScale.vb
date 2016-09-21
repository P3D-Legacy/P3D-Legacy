Namespace Items.KeyItems

    <Item(66, "Red Scale")>
    Public Class RedScale

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A scale from the red Gyarados. It glows red like a flame."

        Public Sub New()
            _textureRectangle = New Rectangle(432, 48, 24, 24)
        End Sub

    End Class

End Namespace
