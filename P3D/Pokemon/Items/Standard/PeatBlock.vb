Namespace Items.Standard

    <Item(654, "Peat Block")>
    Public Class PeatBlock

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A block of muddy material that can be used as fuel for burning when it is dried. It’s loved by a certain Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000
        Public Overrides ReadOnly Property FlingDamage As Integer = 80
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(312, 192, 24, 24)
        End Sub

    End Class

End Namespace
