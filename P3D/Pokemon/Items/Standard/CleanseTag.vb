Namespace Items.Standard

    <Item(94, "CleanseTag")>
    Public Class CleanseTag

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It helps keep wild Pokémon away if the holder is the head of the party."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(432, 72, 24, 24)
        End Sub

    End Class

End Namespace
