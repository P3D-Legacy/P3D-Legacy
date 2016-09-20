Namespace Items.Plants

    <Item(86, "Tiny Mushroom")>
    Public Class TinyMushroom

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 72, 24, 24)
        End Sub

    End Class

End Namespace
