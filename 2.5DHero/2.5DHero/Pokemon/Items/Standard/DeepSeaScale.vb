Namespace Items.Standard

    <Item(162, "DeepSeaScale")>
    Public Class DeepSeaScale

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(120, 216, 24, 24)
        End Sub

    End Class

End Namespace
