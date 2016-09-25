Namespace Items.Standard

    <Item(126, "Lucky Egg")>
    Public Class LuckyEgg

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is an egg filled with happiness that earns extra Exp. Points in battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(120, 120, 24, 24)
        End Sub

    End Class

End Namespace
