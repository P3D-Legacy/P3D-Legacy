Namespace Items.Standard

    <Item(125, "HardStone")>
    Public Class HardStone

        Inherits Item

        Public Overrides ReadOnly Property FlingDamage As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(96, 120, 24, 24)
        End Sub

    End Class

End Namespace
