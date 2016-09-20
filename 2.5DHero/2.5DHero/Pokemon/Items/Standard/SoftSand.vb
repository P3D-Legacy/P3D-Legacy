Namespace Items.Standard

    <Item(76, "Soft Sand")>
    Public Class SoftSand

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(144, 72, 24, 24)
        End Sub

    End Class

End Namespace
