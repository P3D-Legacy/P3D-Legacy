Namespace Items.Standard

    <Item(189, "Big Nugget")>
    Public Class BigNugget

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(48, 240, 24, 24)
        End Sub

    End Class

End Namespace
