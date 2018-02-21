Namespace Items.Standard

    <Item(141, "Protector")>
    Public Class Protector

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A protective item of some sort. It is extremely stiff and heavy. It is loved by a certain Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(408, 192, 24, 24)
        End Sub

    End Class

End Namespace
