Namespace Items.Standard

    <Item(595, "Cell Battery")>
    Public Class CellBattery

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It boosts Attack if hit with an Electric-type attack. It can only be used once."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property PluralName As String = "Cell Batteries"

        Public Sub New()
            _textureRectangle = New Rectangle(192, 312, 24, 24)
        End Sub

    End Class

End Namespace
