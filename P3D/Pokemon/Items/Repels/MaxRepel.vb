Namespace Items.Repels

    <Item(43, "Max Repel")>
    Public Class MaxRepel

        Inherits RepelItem

        Public Overrides ReadOnly Property Description As String = "An item that prevents any low-level wild Pokémon from jumping out at you for 250 steps after its use."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 700
        Public Overrides ReadOnly Property RepelSteps As Integer = 250

        Public Sub New()
            _textureRectangle = New Rectangle(456, 24, 24, 24)
        End Sub

    End Class

End Namespace
