Namespace Items.Standard

    <Item(591, "Expert Belt")>
    Public Class ExpertBelt

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It's a well-worn belt that slightly boosts the power of supereffective moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(384, 288, 24, 24)
        End Sub

    End Class

End Namespace
