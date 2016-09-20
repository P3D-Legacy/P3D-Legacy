Namespace Items.Standard

    <Item(173, "Pearl String")>
    Public Class PearlString

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 50000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(408, 216, 24, 24)
        End Sub

    End Class

End Namespace
