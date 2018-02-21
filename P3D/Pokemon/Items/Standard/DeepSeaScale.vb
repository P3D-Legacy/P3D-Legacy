Namespace Items.Standard

    <Item(162, "DeepSeaScale")>
    Public Class DeepSeaScale

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by Clamperl. This scale shines with a faint pink and raises the holder's Sp. Def stat."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(120, 216, 24, 24)
        End Sub

    End Class

End Namespace
