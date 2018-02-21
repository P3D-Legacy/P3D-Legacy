Namespace Items.Standard

    <Item(148, "Soothe Bell")>
    Public Class SootheBell

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. The comforting chime of this bell calms the holder, making it friendly."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(0, 216, 24, 24)
        End Sub

    End Class

End Namespace
