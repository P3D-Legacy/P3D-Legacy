Namespace Items.Standard

    <Item(83, "Prism Scale")>
    Public Class PrismScale

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A mysterious scale that causes a certain Pokémon to evolve. It shines in rainbow colors."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(72, 168, 24, 24)
        End Sub

    End Class

End Namespace
