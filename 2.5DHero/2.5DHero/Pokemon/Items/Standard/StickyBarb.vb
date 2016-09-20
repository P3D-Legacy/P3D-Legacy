Namespace Items.Standard

    <Item(70, "Sticky Barb")>
    Public Class StickyBarb

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(24, 168, 24, 24)
        End Sub

    End Class

End Namespace
