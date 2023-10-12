Namespace Items.Standard

    <Item(654, "Peat Block")>
    Public Class PeatBlock

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A block of muddy material that can be used as fuel for burning when it is dried. It’s loved by a certain Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000
        Public Overrides ReadOnly Property FlingDamage As Integer = 80

        Public Sub New()
            _textureRectangle = New Rectangle(408, 408, 24, 24)
        End Sub

    End Class

End Namespace
