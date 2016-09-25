Namespace Items.Standard

    <Item(91, "Amulet Coin")>
    Public Class AmuletCoin

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It doubles any prize money received if the holding Pokémon joins in a battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(360, 72, 24, 24)
        End Sub

    End Class

End Namespace
