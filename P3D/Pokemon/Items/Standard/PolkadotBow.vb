Namespace Items.Standard

    <Item(170, "Polkadot Bow")>
    Public Class PolkadotBow

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A pink bow. A certain Pokémon likes this item."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(192, 240, 24, 24)
        End Sub

    End Class

End Namespace
