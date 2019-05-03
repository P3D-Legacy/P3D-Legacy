Namespace Items.Standard

    <Item(594, "Snowball")>
    Public Class Snowball

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It boosts Attack if hit with an Ice-type attack. It can only be used once."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 312, 24, 24)
        End Sub

    End Class

End Namespace
