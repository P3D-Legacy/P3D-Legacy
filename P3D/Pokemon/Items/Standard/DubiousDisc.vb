Namespace Items.Standard

    <Item(185, "Dubious Disc")>
    Public Class DubiousDisc

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A transparent device overflowing with dubious data. Its producer is unknown."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property FlingDamage As Integer = 50
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(0, 168, 24, 24)
        End Sub

    End Class

End Namespace
