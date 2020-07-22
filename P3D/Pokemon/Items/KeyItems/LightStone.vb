Namespace Items.KeyItems

    <Item(651, "Light Stone")>
    Public Class LightStone

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "Reshiram's body was destroyed and changed into this stone. It is said to be waiting for the emergence of a hero."

        Public Sub New()
            _textureRectangle = New Rectangle(216, 408, 24, 24)
        End Sub

    End Class

End Namespace
