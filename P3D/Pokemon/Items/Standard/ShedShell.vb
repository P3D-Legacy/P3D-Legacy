Namespace Items.Standard

    <Item(154, "Shed Shell")>
    Public Class ShedShell

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A tough, discarded carapace to be held by a Pokémon. It enables the holder to switch with a waiting Pokémon in battle."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(72, 216, 24, 24)
        End Sub

    End Class

End Namespace
