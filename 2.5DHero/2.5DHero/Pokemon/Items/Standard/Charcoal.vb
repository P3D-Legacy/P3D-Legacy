Namespace Items.Standard

    <Item(138, "Charcoal")>
    Public Class Charcoal

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(336, 120, 24, 24)
        End Sub

    End Class

End Namespace
