Namespace Items.Standard

    <Item(30, "Lucky Punch")>
    Public Class LuckyPunch

        Inherits Item

        Public Overrides ReadOnly Property FlingDamage As Integer = 40
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(144, 24, 24, 24)
        End Sub

    End Class

End Namespace
