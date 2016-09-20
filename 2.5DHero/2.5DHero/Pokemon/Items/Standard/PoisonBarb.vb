Namespace Items.Standard

    <Item(81, "Poison Barb")>
    Public Class PoisonBarb

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 70
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(192, 72, 24, 24)
        End Sub

    End Class

End Namespace
