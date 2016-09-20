Namespace Items.Standard

    <Item(190, "Heart Scale")>
    Public Class HeartScale
        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 216, 24, 24)
        End Sub

    End Class

End Namespace
