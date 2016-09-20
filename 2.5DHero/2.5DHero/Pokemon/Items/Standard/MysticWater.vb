Namespace Items.Standard

    <Item(95, "Mystic Water")>
    Public Class MysticWater

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 72, 24, 24)
        End Sub

    End Class

End Namespace
