Namespace Items.Standard

    <Item(98, "Black Belt")>
    Public Class BlackBelt

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(24, 96, 24, 24)
        End Sub

    End Class

End Namespace
