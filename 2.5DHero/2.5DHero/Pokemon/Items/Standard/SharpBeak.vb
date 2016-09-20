Namespace Items.Standard

    <Item(77, "Sharp Beak")>
    Public Class SharpBeak

        Inherits Item

        Public Overrides ReadOnly Property FlingDamage As Integer = 50
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 72, 24, 24)
        End Sub

    End Class

End Namespace
