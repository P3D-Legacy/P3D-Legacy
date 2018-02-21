Namespace Items.Standard

    <Item(583, "Power Bracer")>
    Public Class PowerBracer

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It reduces Speed but allows the holder's Attack stat to grow more after battling."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(144, 288, 24, 24)
        End Sub

    End Class

End Namespace
