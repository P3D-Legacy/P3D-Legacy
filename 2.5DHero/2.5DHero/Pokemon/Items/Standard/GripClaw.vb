Namespace Items.Standard

    <Item(176, "Grip Claw")>
    Public Class GripClaw

        Inherits Item

        Public Overrides ReadOnly Property FlingDamage As Integer = 90
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 216, 24, 24)
        End Sub

    End Class

End Namespace
