Namespace Items.Standard

    <Item(289, "Lax Incense")>
    Public Class LaxIncense

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. The beguiling aroma of this incense may cause attacks to miss its holder."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9600
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(240, 264, 24, 24)
        End Sub

    End Class

End Namespace
