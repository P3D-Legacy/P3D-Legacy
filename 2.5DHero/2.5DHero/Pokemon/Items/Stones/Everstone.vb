Namespace Items.Stones

    <Item(112, "Everstone")>
    Public Class Everstone

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. A Pokémon holding this peculiar stone is prevented from evolving."
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(312, 96, 24, 24)
        End Sub

    End Class

End Namespace
