Namespace Items.Standard

    <Item(263, "Odd Incense")>
    Public Class OddIncense

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(0, 264, 24, 24)
        End Sub

    End Class

End Namespace
