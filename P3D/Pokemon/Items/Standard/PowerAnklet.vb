Namespace Items.Standard

    <Item(587, "Power Anklet")>
    Public Class PowerAnklet

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It reduces Speed but allows the holder's Speed stat to grow more after battling."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(240, 288, 24, 24)
        End Sub

    End Class

End Namespace
