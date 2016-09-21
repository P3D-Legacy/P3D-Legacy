Namespace Items.Repels

    <Item(20, "Repel")>
    Public Class Repel

        Inherits RepelItem

        Public Overrides ReadOnly Property Description As String = "An item that prevents any low-level wild Pokémon from jumping out at you for 100 steps after its use."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 350
        Public Overrides ReadOnly Property RepelSteps As Integer = 100

        Public Sub New()
            _textureRectangle = New Rectangle(432, 0, 24, 24)
        End Sub

    End Class

End Namespace
