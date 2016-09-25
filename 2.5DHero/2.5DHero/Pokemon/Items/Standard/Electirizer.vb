Namespace Items.Standard

    <Item(120, "Electirizer")>
    Public Class Electirizer

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A box packed with a tremendous amount of electric energy. It is loved by a certain Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(312, 192, 24, 24)
        End Sub

    End Class

End Namespace
