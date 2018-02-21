Namespace Items.Standard

    <Item(81, "Poison Barb")>
    Public Class PoisonBarb

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. This small, poisonous barb boosts the power of Poison-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 70
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(192, 72, 24, 24)
        End Sub

    End Class

End Namespace
