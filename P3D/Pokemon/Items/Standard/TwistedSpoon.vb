Namespace Items.Standard
    'LOL Uri Gella is raging about this one
    <Item(96, "Twisted Spoon")>
    Public Class TwistedSpoon

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a spoon imbued with telekinetic power that boosts Psychic-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(480, 72, 24, 24)
        End Sub

    End Class

End Namespace
