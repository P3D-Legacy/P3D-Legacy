Namespace Items.Standard

    <Item(118, "Thick Club")>
    Public Class ThickClub

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 500
        Public Overrides ReadOnly Property FlingDamage As Integer = 90
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 96, 24, 24)
        End Sub

    End Class

End Namespace
