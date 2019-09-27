Namespace Items.Standard

    <Item(600, "Terrain Extender")>
    Public Class TerrainExtender

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It extends the duration of the terrain caused by the holder's move or Ability."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(288, 312, 24, 24)
        End Sub

    End Class

End Namespace

