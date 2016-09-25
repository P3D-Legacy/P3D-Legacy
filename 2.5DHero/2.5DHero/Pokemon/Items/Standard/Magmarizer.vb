Namespace Items.Standard

    <Item(100, "Magmarizer")>
    Public Class Magmarizer

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A box packed with a tremendous amount of magma energy. It is loved by a certain Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(288, 192, 24, 24)
        End Sub

    End Class

End Namespace
