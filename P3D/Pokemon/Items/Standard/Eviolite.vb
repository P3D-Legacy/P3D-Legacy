Namespace Items.Standard

    <Item(298, "Eviolite")>
    Public Class Eviolite

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A mysterious Evolutionary lump. When held by a Pokémon that can still evolve, it raises both Defense and Sp. Def."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(336, 288, 24, 24)
        End Sub

    End Class

End Namespace
