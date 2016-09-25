Namespace Items.Standard

    <Item(108, "Magnet")>
    Public Class Magnet

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a powerful magnet that boosts the power of Electric-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(240, 96, 24, 24)
        End Sub

    End Class

End Namespace
