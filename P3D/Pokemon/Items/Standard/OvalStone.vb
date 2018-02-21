Namespace Items.Standard

    <Item(179, "Oval Stone")>
    Public Class OvalStone

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that makes certain species of Pokémon evolve. It is shaped like an egg."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(192, 216, 24, 24)
        End Sub

    End Class

End Namespace
