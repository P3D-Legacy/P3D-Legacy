Namespace Items.Standard

    <Item(677, "Destiny Knot")>
    Public Class DestinyKnot

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A long, thin, bright red string to be held by a Pokémon. If the holder becomes infatuated, the foe does too."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(432, 384, 24, 24)
        End Sub

    End Class

End Namespace
