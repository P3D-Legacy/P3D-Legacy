Namespace Items.Standard

    <Item(599, "Psychic Seed")>
    Public Class PsychicSeed

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It boosts Sp. Defense on Psychic Terrain. It can only be used once."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(384, 312, 24, 24)
        End Sub

    End Class

End Namespace

