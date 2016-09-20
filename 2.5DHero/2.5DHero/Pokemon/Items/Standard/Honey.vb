Namespace Items.Plants

    <Item(253, "Honey")>
    Public Class Honey

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 240, 24, 24)
        End Sub

    End Class

End Namespace
