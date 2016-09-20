Namespace Items.Standard

    <Item(146, "Leftovers")>
    Public Class Leftovers

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. The holder's HP is slowly but steadily restored throughout every battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 64
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 120, 24, 24)
        End Sub

    End Class

End Namespace
