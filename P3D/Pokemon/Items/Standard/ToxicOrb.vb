Namespace Items.Standard

    <Item(505, "Toxic Orb")>
    Public Class ToxicOrb

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a bizarre orb that will badly poison the holder during battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 16
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(216, 240, 24, 24)
        End Sub

    End Class

End Namespace
