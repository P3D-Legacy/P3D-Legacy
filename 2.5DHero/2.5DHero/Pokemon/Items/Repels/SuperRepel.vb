Namespace Items.Repels

    <Item(42, "Super Repel")>
    Public Class SuperRepel

        Inherits RepelItem

        Public Overrides ReadOnly Property Description As String = "An item that prevents any low-level wild Pokémon from jumping out at you for 200 steps after its use."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500
        Public Overrides ReadOnly Property RepelSteps As Integer = 200

        Public Sub New()
            _textureRectangle = New Rectangle(432, 24, 24, 24)
        End Sub

    End Class

End Namespace
