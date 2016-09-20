Namespace Items.Standard

    <Item(126, "Lucky Egg")>
    Public Class LuckyEgg

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(120, 120, 24, 24)
        End Sub

    End Class

End Namespace
