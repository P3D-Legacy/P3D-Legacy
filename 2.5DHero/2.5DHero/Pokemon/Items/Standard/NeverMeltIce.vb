Namespace Items.Standard

    <Item(107, "Never-Melt Ice")>
    Public Class NeverMeltIce

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(216, 96, 24, 24)
        End Sub

    End Class

End Namespace
