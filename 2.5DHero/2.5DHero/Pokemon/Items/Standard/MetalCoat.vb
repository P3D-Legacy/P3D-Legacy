Namespace Items.Standard

    <Item(143, "Metal Coat")>
    Public Class MetalCoat

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by a Pokémon. It is a special metallic film that can boost the power of Steel-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(408, 120, 24, 24)
        End Sub

    End Class

End Namespace
