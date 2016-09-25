Namespace Items.Standard

    <Item(60, "Silver Leaf")>
    Public Class SilverLeaf

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A strange, silver-colored leaf."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(288, 48, 24, 24)
        End Sub

    End Class

End Namespace
