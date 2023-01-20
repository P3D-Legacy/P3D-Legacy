Namespace Items.Standard

    <Item(581, "Macho Brace")>
    Public Class MachoBrace

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. This stiff, heavy brace helps Pokemon grow strong but cuts Speed in battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(96, 288, 24, 24)
        End Sub

    End Class

End Namespace
