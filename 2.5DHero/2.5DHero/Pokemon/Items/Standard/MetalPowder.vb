Namespace Items.Standard

    <Item(35, "Metal Powder")>
    Public Class MetalPowder

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by Ditto. Extremely fine yet hard, this odd powder boosts the Defense stat."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 10
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 24, 24, 24)
        End Sub

    End Class

End Namespace
