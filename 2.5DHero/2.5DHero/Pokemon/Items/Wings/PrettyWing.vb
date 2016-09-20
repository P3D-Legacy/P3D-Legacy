Namespace Items.Wings

    <Item(260, "Pretty Wing")>
    Public Class PrettyWing

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property Description As String = "Though this feather is beautiful, it's just a regular feather and has no effect on Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200

        Public Sub New()
            _textureRectangle = New Rectangle(432, 240, 24, 24)
        End Sub

    End Class

End Namespace
