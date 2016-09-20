Namespace Items.Standard

    <Item(106, "Smoke Ball")>
    Public Class SmokeBall

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(192, 96, 24, 24)
        End Sub

    End Class

End Namespace
