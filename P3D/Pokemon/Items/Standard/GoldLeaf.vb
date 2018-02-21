Namespace Items.Standard

    <Item(75, "Gold Leaf")>
    Public Class GoldLeaf

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A strange, gold-colored leaf."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(120, 72, 24, 24)
        End Sub

    End Class

End Namespace
