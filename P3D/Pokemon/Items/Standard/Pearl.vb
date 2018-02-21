Namespace Items.Standard

    <Item(110, "Pearl")>
    Public Class Pearl

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A rather small pearl that has a very nice silvery sheen to it. It can be sold cheaply to shops."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1400
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 96, 24, 24)
        End Sub

    End Class

End Namespace
