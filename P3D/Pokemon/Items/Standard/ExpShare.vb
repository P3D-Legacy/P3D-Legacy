Namespace Items.Standard

    <Item(57, "Exp Share")>
    Public Class ExpShare

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. The holder gets a share of a battle's Exp. Points without battling."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(216, 48, 24, 24)
        End Sub

    End Class

End Namespace
